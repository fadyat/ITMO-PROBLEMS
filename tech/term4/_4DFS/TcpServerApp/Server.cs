using System.Collections.Immutable;
using System.Net.Sockets;
using System.Text;
using Analyzer;

namespace TcpServerApp;

public class Server
{
    private static readonly Dictionary<string, int> Requests = new()
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

    public bool Active { get; private set; }

    public Server()
    {
        Active = false;
        _connectedNodes = new Dictionary<string, ConnectedNode>();
    }

    public void AnalyzeRequests()
    {
        Console.WriteLine("Enter command for server: ");
        var parsedCommand = new List<string>();
        var correct = false;
        while (!correct)
        {
            try
            {
                var command = Console.ReadLine();
                parsedCommand = CommandAnalyzer.ParseInputCommand(command).ToList();
                correct = CommandAnalyzer.IsCorrectCommand(Requests, parsedCommand);
            }
            catch (FormatException e)
            {
                Console.WriteLine($"\t{e.Message}");
                correct = false;
            }
        }

        CommandSelector(parsedCommand);
    }

    // method only for benchmarks
    public void AnalyzeRequests(string command)
    {
        CommandSelector(CommandAnalyzer.ParseInputCommand(command).ToList());
    }

    public void Start()
    {
        Active = true;
    }

    public void Stop()
    {
        Active = false;
    }

    private void CommandSelector(IReadOnlyList<string> parsedCommand)
    {
        var mainCommand = parsedCommand[0];

        switch (mainCommand)
        {
            case "/add-node":
                AddNode(parsedCommand[1], parsedCommand[2], parsedCommand[3], parsedCommand[4]);
                break;
            case "/add-file":
                AddFile(parsedCommand[1], parsedCommand[2]);
                break;
            case "/remove-file":
                RemoveFile(parsedCommand[1]);
                break;
            case "/exec":
                ExecuteCommands(parsedCommand[1]);
                break;
            case "/clean-node":
                CleanNode(parsedCommand[1]);
                break;
            case "/balance-nodes":
                BalanceNodes();
                break;
            case "/show-nodes":
                ShowNodes();
                break;
            case "/stop":
                Stop();
                break;
        }
    }

    private void AddNode(string nodeName, string ipAddress, string port, string globalMemory)
    {
        try
        {
            var connectedNode = new ConnectedNode(ipAddress, port, globalMemory);
            _connectedNodes.Add(nodeName, connectedNode);
            // Console.WriteLine("\t'{0}' with '{1}' successfully added to connected list!", nodeName, connectedNode);
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
            var fileDataBytes = File.ReadAllBytesAsync(fsPath).Result;
            SendData(updatedNode, partialPathBytes);
            SendData(updatedNode, fileDataBytes);
            updatedNode.SaveTransportedFileSourceData(fsPath, partialPath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static void AddFile(string fsPath, string partialPath, ConnectedNode updatedNode)
    {
        try
        {
            var optionBytes = Encoding.ASCII.GetBytes("/save 2");
            SendData(updatedNode, optionBytes);
            var partialPathBytes = Encoding.ASCII.GetBytes(partialPath);
            var fileDataBytes = File.ReadAllBytesAsync(fsPath).Result;
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
        stream.WriteAsync(data, 0, data.Length);
        stream.Close();
        client.Close();
    }

    private void ExecuteCommands(string fileWithCommandsPath)
    {
        var commands = File.ReadLines(fileWithCommandsPath);

        foreach (var command in commands)
        {
            var parsedCommand = CommandAnalyzer.ParseInputCommand(command).ToList();
            if (!CommandAnalyzer.IsCorrectCommand(Requests, parsedCommand))
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
        var totalFreeMemory = GetTotalConnectedNodesFreeMemory() - (cleaningNode.GlobalMemory - cleaningNode.UsedMemory);
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
                // add file to new free node
                AddFile(fsPath, partialPath);
                // remove file from previous node
                RemoveFile(fsPath, partialPath, cleaningNode);
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
        var allNodesFiles = new List<(string nodeName, string fsPath, string partialPath, long usedMemory)>();
        foreach (var (nodeName, connectedNode) in _connectedNodes)
        {
            foreach (var (fsPath, partialPaths) in connectedNode.SavedFiles)
            {
                // fsPath may not exist?
                allNodesFiles.AddRange(partialPaths.Select(partialPath =>
                    (nodeName, fsPath, partialPath, new FileInfo(fsPath).Length))
                );
            }
        }

        var nodesNames = _connectedNodes.Keys.ToImmutableList();
        var transportedNodes = nodesNames.ToDictionary(
            nodeName => nodeName,
            _ => (long) 0
        );

        var newNodesContent = nodesNames.ToDictionary(
            nodeName => nodeName,
            _ => new List<(string prevNode, string fsPath, string partialPath)>()
        );

        allNodesFiles.Sort((x, y) => x.usedMemory.CompareTo(y.usedMemory));
        allNodesFiles.Reverse();

        // greedy is correct?
        foreach (var file in allNodesFiles)
        {
            var currentNodeName = GetNodeNameWithLowestPercentOfUsedMemory(transportedNodes);

            // update usedMemory in node
            transportedNodes[currentNodeName] += file.usedMemory;

            // moving file to new currentNode
            newNodesContent[currentNodeName].Add((file.nodeName, file.fsPath, file.partialPath));
        }

        // remove incorrect files
        foreach (var (nodeName, values) in newNodesContent)
        {
            foreach (var x in values.Where(x => nodeName != x.prevNode))
            {
                RemoveFile(x.fsPath, x.partialPath, _connectedNodes[x.prevNode]);
            }
        }

        // move files
        foreach (var (nodeName, values) in newNodesContent)
        {
            foreach (var x in values.Where(x => nodeName != x.prevNode))
            {
                AddFile(x.fsPath, x.partialPath, _connectedNodes[nodeName]);
            }
        }
    }

    // may use binary tree for optimal search
    private string GetNodeNameWithLowestPercentOfUsedMemory(Dictionary<string, long> nodes)
    {
        var minNodeName = string.Empty;
        double minPercentOfUsedMemory = 1;
        foreach (var (node, usedMemory) in nodes)
        {
            var nodeGlobalMemory = _connectedNodes[node].GlobalMemory;
            var currentPercentOfUsedMemory = (double) usedMemory / nodeGlobalMemory;
            if (!(currentPercentOfUsedMemory <= minPercentOfUsedMemory)) continue;

            minNodeName = node;
            minPercentOfUsedMemory = currentPercentOfUsedMemory;
        }

        return minNodeName;
    }
}