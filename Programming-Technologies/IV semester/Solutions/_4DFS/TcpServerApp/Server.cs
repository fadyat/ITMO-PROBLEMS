using System.Collections.Immutable;
using System.Net.Sockets;
using System.Text;
using Analyzer;

namespace TcpServerApp;

public class Server : RequestAnalyzer
{
    private new static readonly Dictionary<string, int> Requests = new()
    {
        {"/add-node", 4}, // <name> <ip> <port> <size>
        {"/add-file", 2}, // <file-path> <partial-path>
        {"/remove-file", 1}, // <file-path>
        {"/exec", 1}, // <file-path>
        {"/clean-node", 1}, // <node-name>
        {"/balance-node", 0},
        {"/show-nodes", 0},
        {"/stop", 0}
    };

    private readonly Dictionary<string, NodeData> _nodesData;

    public Server()
    {
        Active = false;
        _nodesData = new Dictionary<string, NodeData>();
    }

    public override void AnalyzeRequests()
    {
        Console.WriteLine("Enter command for server: ");
        string[]? parsedCommand = null;
        var correct = false;
        while (!correct)
        {
            try
            {
                var command = Console.ReadLine();
                parsedCommand = ParseInputCommand(command);
                correct = IsCorrectCommand(parsedCommand);
            }
            catch (FormatException e)
            {
                Console.WriteLine($"\t{e.Message}");
                correct = false;
            }
        }

        CommandSelector(parsedCommand);
    }

    protected override void CommandSelector(string[]? parsedCommand)
    {
        var mainCommand = parsedCommand![0];

        if (mainCommand == "/add-node")
        {
            AddNode(parsedCommand[1..]);
        }
        else if (mainCommand == "/add-file")
        {
            AddFile(parsedCommand[1..]);
        }
        else if (mainCommand == "/remove-file")
        {
            RemoveFile(parsedCommand[1..]);
        }
        else if (mainCommand == "/exec")
        {
            ExecCommands(parsedCommand[1..]);
        }
        else if (mainCommand == "/clean-node")
        {
            CleanNode(parsedCommand[1..]);
        }
        else if (mainCommand == "/balance-node")
        {
            // ...
        }
        else if (mainCommand == "/show-nodes")
        {
            ShowNodes();
        }
        else if (mainCommand == "/stop")
        {
            Stop();
        }
    }

    protected override bool IsCorrectCommand(IReadOnlyList<string>? parsedCommand)
    {
        var correctMainCommand = parsedCommand != null && Requests.Any() && Requests.ContainsKey(parsedCommand[0]);
        if (!correctMainCommand)
        {
            throw new FormatException("Unknown command!");
        }

        var correctArgs = Equals(Requests[parsedCommand![0]], parsedCommand.Count - 1);
        if (!correctArgs)
        {
            throw new FormatException($"Expected {Requests[parsedCommand[0]]} args, was {parsedCommand.Count - 1}");
        }

        return correctMainCommand & correctArgs;
    }

    private void AddNode(string[] args)
    {
        try
        {
            var nodeName = args[0];
            var nodeData = new NodeData(args[1..]);
            _nodesData.Add(nodeName, nodeData);
            Console.WriteLine("\t'{0}' with '{1}' successfully added to connected list!", nodeName, nodeData);
        }
        catch (Exception e) when (e is FileNotFoundException or SocketException or FormatException)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void RemoveNode(string nodeName)
    {
        _nodesData.Remove(nodeName);
    }

    private void AddFile(IReadOnlyList<string> args)
    {
        var fsPath = args[0];
        var i = 0;
        while (i < args[1].Length - 1 && (Equals(args[1][i], '/') || Equals(args[1][i], '\\'))) i++;

        var partialPath = args[1][i..];

        try
        {
            var nodeToUpdate = _nodesData.Values.First(nodeData => !nodeData.Filled());
            var option = Encoding.ASCII.GetBytes("/save 2");
            SendData(nodeToUpdate, option);
            var fileLocation = Encoding.ASCII.GetBytes(partialPath);
            var fileData = File.ReadAllBytes(fsPath);
            SendData(nodeToUpdate, fileLocation);
            SendData(nodeToUpdate, fileData);
            nodeToUpdate.SaveTransportedFileSourceData(fsPath, partialPath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void RemoveFile(IReadOnlyList<string> args)
    {
        var fsPath = args[0];

        _nodesData.Values.ToImmutableList().ForEach(nodeData =>
        {
            var locations = nodeData.GetFilesToRemove(fsPath);
            if (!locations.Any()) return;

            var option = Encoding.ASCII.GetBytes($"/remove {locations.Count}");
            SendData(nodeData, option);
            locations.ForEach(location =>
            {
                var fileLocation = Encoding.ASCII.GetBytes(location);
                nodeData.RemoveTransportedFileData(fsPath, location);
                SendData(nodeData, fileLocation);
            });
        });
    }

    private void RemoveFile(string fsPath, string location, NodeData nodeData)
    {
        var option = Encoding.ASCII.GetBytes("/remove 1");
        SendData(nodeData, option);
        var fileLocation = Encoding.ASCII.GetBytes(location);
        nodeData.RemoveTransportedFileData(fsPath, location);
        SendData(nodeData, fileLocation);
    }

    private static void SendData(NodeData nodeData, byte[] data)
    {
        var client = new TcpClient();
        client.Connect(nodeData.IpAddress, nodeData.Port);
        var stream = client.GetStream();
        stream.Write(data, 0, data.Length);
        stream.Close();
        client.Close();
    }

    private void ExecCommands(IReadOnlyList<string> args)
    {
        var fileWithCommands = args[0];
        var fileData = File.ReadLines(fileWithCommands);

        fileData.ToImmutableList().ForEach(line =>
        {
            var parsedCommand = ParseInputCommand(line);
            var correct = IsCorrectCommand(parsedCommand);
            if (!correct)
            {
                Console.WriteLine($"'{line}' is incorrect command!");
                return;
            }

            CommandSelector(parsedCommand);
        });
    }

    private void CleanNode(IReadOnlyList<string> args)
    {
        var nodeName = args[0];
        var nodeToCleanData = _nodesData[nodeName];
        var totalFreePlace = GetTotalNodesDataFreeSpace();
        totalFreePlace -= nodeToCleanData.Size - nodeToCleanData.UsedFilesNumber();
        if (totalFreePlace < nodeToCleanData.UsedFilesNumber())
        {
            Console.WriteLine($"\tNot enough free nodes for cleaning node '{nodeToCleanData}'");
            return;
        }

        RemoveNode(nodeName);
        var savedFiles = nodeToCleanData.SavedFiles;
        foreach (var (fsPath, partialFiles) in savedFiles)
        {
            partialFiles.ForEach(partialFile =>
            {
                // add file to new free node
                AddFile(new[] {fsPath, partialFile});
                // remove file from previous node
                RemoveFile(fsPath, partialFile, nodeToCleanData);
            });
        }

        AddNode(new[]
        {
            nodeName,
            nodeToCleanData.IpAddress.ToString(),
            nodeToCleanData.Port.ToString(),
            nodeToCleanData.Size.ToString()
        });
    }

    private void ShowNodes()
    {
        foreach (var (nodeName, nodeData) in _nodesData)
        {
            Console.WriteLine($"{nodeName} {nodeData}");
        }
    }

    private int GetTotalNodesDataFreeSpace()
    {
        var totalUsed = _nodesData.Values.Sum(x => x.UsedFilesNumber());
        var totalSize = _nodesData.Values.Sum(x => x.Size);
        return totalSize - totalUsed;
    }
}