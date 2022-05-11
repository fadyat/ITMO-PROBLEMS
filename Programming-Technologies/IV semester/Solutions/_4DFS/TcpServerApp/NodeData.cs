using System.Collections.Immutable;
using System.Net;
using System.Net.NetworkInformation;

namespace TcpServerApp;

public class NodeData
{
    private readonly Dictionary<string, List<string>> _savedFiles;

    public IPAddress IpAddress { get; }

    public int Port { get; }

    public int Size { get; }

    public ImmutableDictionary<string, ImmutableList<string>> SavedFiles
    {
        get
        {
            var tmpDictionary = new Dictionary<string, ImmutableList<string>>();
            foreach (var (key, value) in _savedFiles)
            {
                tmpDictionary.Add(key, value.ToImmutableList());
            }

            return tmpDictionary.ToImmutableDictionary();
        }
    }

    private NodeData(string ip, string port, string size)
    {
        IpAddress = IPAddress.Parse(ip);
        Port = Convert.ToInt32(port);
        Size = Convert.ToInt32(size);
        _savedFiles = new Dictionary<string, List<string>>();
        CheckTcpConnection();
    }

    public NodeData(IReadOnlyList<string> args)
        : this(args[0], args[1], args[2])
    {
    }

    private void CheckTcpConnection()
    {
        var active = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().ToImmutableList();
        var correctNode = active.Any(x => Equals(x.Address, IpAddress) & Equals(x.Port, Port));

        if (!correctNode)
        {
            throw new FormatException($"Node with TcpListener '{IpAddress}:{Port}' doesn't exist!");
        }
    }

    public void SaveTransportedFileSourceData(string fsPath, string location)
    {
        if (!_savedFiles.ContainsKey(fsPath))
        {
            _savedFiles.Add(fsPath, new List<string>());
        }

        _savedFiles[fsPath].Add(location);
    }

    public ImmutableList<string> GetFilesToRemove(string fsPath)
    {
        var files = _savedFiles.ContainsKey(fsPath)
            ? _savedFiles[fsPath].ToImmutableList()
            : ImmutableList<string>.Empty;

        return files;
    }

    public void RemoveTransportedFileData(string fsPath, string location)
    {
        if (_savedFiles.ContainsKey(fsPath))
        {
            _savedFiles[fsPath].Remove(location);
        }

        if (!_savedFiles[fsPath].Any())
        {
            _savedFiles.Remove(fsPath);
        }
    }

    public bool Filled()
    {
        return Equals(UsedFilesNumber(), Size);
    }

    public override string ToString()
    {
        return $"{IpAddress}:{Port} {UsedFilesNumber()}/{Size}";
    }

    protected bool Equals(NodeData other)
    {
        return IpAddress.Equals(other.IpAddress) && Port == other.Port;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IpAddress, Port);
    }

    public int UsedFilesNumber()
    {
        return _savedFiles.Values.Sum(list => list.Count);
    }
}