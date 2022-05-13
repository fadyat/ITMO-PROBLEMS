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
        {"/balance-nodes", 0},
        {"/show-nodes", 0},
        {"/stop", 0}
    };

    private readonly Dictionary<string, ConnectedNode> _connectedNodes;

    public Server()
    {
        Active = false;
        _connectedNodes = new Dictionary<string, ConnectedNode>();
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
            AddNode(parsedCommand[1], parsedCommand[2], parsedCommand[3], parsedCommand[4]);
        }
        else if (mainCommand == "/add-file")
        {
            AddFile(parsedCommand[1], parsedCommand[2]);
        }
        else if (mainCommand == "/remove-file")
        {
            RemoveFile(parsedCommand[1]);
        }
        else if (mainCommand == "/exec")
        {
            ExecuteCommands(parsedCommand[1]);
        }
        else if (mainCommand == "/clean-node")
        {
            CleanNode(parsedCommand[1]);
        }
        else if (mainCommand == "/balance-nodes")
        {
            BalanceNodes();
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

    private void AddNode(string nodeName, string ipAddress, string port, string globalMemory)
    {
        try
        {
            var connectedNode = new ConnectedNode(ipAddress, port, globalMemory);
            _connectedNodes.Add(nodeName, connectedNode);
            Console.WriteLine("\t'{0}' with '{1}' successfully added to connected list!", nodeName, connectedNode);
        }
        catch (Exception e) when (e is FileNotFoundException or SocketException or FormatException)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void RemoveNode(string nodeName)
    {
        _connectedNodes.Remove(nodeName);
    }

    private void AddFile(string fsPath, string partialPath)
    {
        try
        {
            var fileSize = new FileInfo(fsPath).Length;
            var updatedNode = _connectedNodes.Values.First(connectedNode => connectedNode.HaveFreeMemory(fileSize));
            var optionBytes = Encoding.ASCII.GetBytes("/save 2");
            SendData(updatedNode, optionBytes);
            var partialPathBytes = Encoding.ASCII.GetBytes(partialPath);
            var fileDataBytes = File.ReadAllBytes(fsPath);
            SendData(updatedNode, partialPathBytes);
            SendData(updatedNode, fileDataBytes);
            updatedNode.SaveTransportedFileSourceData(fsPath, partialPath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void RemoveFile(string fsPath)
    {
        foreach (var connectedNode in _connectedNodes.Values.ToImmutableList())
        {
            var partialPaths = connectedNode.GetFilesToRemove(fsPath);
            if (!partialPaths.Any()) continue;

            var option = Encoding.ASCII.GetBytes($"/remove {partialPaths.Count}");
            SendData(connectedNode, option);
            foreach (var partialPath in partialPaths)
            {
                var fileLocation = Encoding.ASCII.GetBytes(partialPath);
                connectedNode.RemoveTransportedFileData(fsPath, partialPath);
                SendData(connectedNode, fileLocation);
            }
        }
    }

    private static void RemoveFile(string fsPath, string partialPath, ConnectedNode connectedNode)
    {
        var option = Encoding.ASCII.GetBytes("/remove 1");
        SendData(connectedNode, option);
        var fileLocation = Encoding.ASCII.GetBytes(partialPath);
        connectedNode.RemoveTransportedFileData(fsPath, partialPath);
        SendData(connectedNode, fileLocation);
    }

    private static void SendData(ConnectedNode connectedNode, byte[] data)
    {
        var client = new TcpClient();
        client.Connect(connectedNode.IpAddress, connectedNode.Port);
        var stream = client.GetStream();
        stream.Write(data, 0, data.Length);
        stream.Close();
        client.Close();
    }

    private void ExecuteCommands(string fileWithCommandsPath)
    {
        var commands = File.ReadLines(fileWithCommandsPath);

        foreach (var command in commands)
        {
            var parsedCommand = ParseInputCommand(command);
            if (!IsCorrectCommand(parsedCommand))
            {
                Console.WriteLine($"'{command}' is incorrect command!");
                return;
            }

            CommandSelector(parsedCommand);
        }
    }

    private void CleanNode(string nodeName)
    {
        var cleaningNode = _connectedNodes[nodeName];
        var totalFreeMemory =
            GetTotalConnectedNodesFreeMemory() - (cleaningNode.GlobalMemory - cleaningNode.UsedMemory);
        if (totalFreeMemory < cleaningNode.UsedMemory)
        {
            Console.WriteLine($"\tNot enough free nodes for cleaning node '{cleaningNode}'");
            return;
        }

        RemoveNode(nodeName);
        var cleaningNodeFiles = cleaningNode.SavedFiles;
        foreach (var (fsPath, partialPaths) in cleaningNodeFiles)
        {
            foreach (var partialPath in partialPaths)
            {
                AddFile(fsPath, partialPath); // add file to new free node
                RemoveFile(fsPath, partialPath, cleaningNode); // remove file from previous node
            }
        }

        AddNode(nodeName, cleaningNode.IpAddress.ToString(), cleaningNode.Port.ToString(),
            cleaningNode.GlobalMemory.ToString());
    }

    private void ShowNodes()
    {
        foreach (var (nodeName, connectedNode) in _connectedNodes)
        {
            Console.WriteLine($"{nodeName} {connectedNode}");
        }
    }

    private long GetTotalConnectedNodesFreeMemory()
    {
        var used = _connectedNodes.Values.Sum(x => x.UsedMemory);
        var glob = _connectedNodes.Values.Sum(x => x.GlobalMemory);
        return glob - used;
    }

    private void BalanceNodes()
    {
        var nodesValues = _connectedNodes.Values;
        var avgNodeUsedMemory =
            (double) nodesValues.Sum(x => x.UsedMemory) / nodesValues.Sum(x => x.NumberOfSavedFiles());
        var avgUsedMemoryOnEveryNode = nodesValues.Select(x =>
            x.NumberOfSavedFiles() == 0 ? 0 : x.UsedMemory / x.NumberOfSavedFiles());


        var allNodesFiles = new List<(string nodeName, string fsPath, string partialPath, long usedMemory)>();
        foreach (var (nodeName, connectedNode) in _connectedNodes)
        {
            foreach (var (fsPath, partialPaths) in connectedNode.SavedFiles)
            {
                // new FileInfo(fsPath).Length) is correct ???
                allNodesFiles.AddRange(partialPaths.Select(partialPath =>
                    (nodeName, fsPath, partialPath, new FileInfo(fsPath).Length))
                );
            }
        }

        var transportedNodes = _connectedNodes.Keys.ToDictionary(
            nodeName => nodeName,
            _ => (long) 0
        );

        var newNodesContent = _connectedNodes.Keys.ToDictionary(
            nodeName => nodeName,
            _ => new List<(string fsPath, string partialPath)>()
        );

        allNodesFiles.Sort((x, y) => x.usedMemory.CompareTo(y.usedMemory));
        allNodesFiles.Reverse();

        // add greedy here
        foreach (var file in allNodesFiles)
        {
            Console.WriteLine(file);
        }
    }
}