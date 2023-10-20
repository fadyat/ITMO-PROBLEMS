using System.Collections.Immutable;
using System.Net;
using System.Net.NetworkInformation;

namespace TcpServerApp;

public class ConnectedNode
{
    private readonly Dictionary<string, List<string>> _savedFiles;

    public IPAddress IpAddress { get; }

    public int Port { get; }

    public long GlobalMemory { get; } // in Bytes
    
    public long UsedMemory { get; private set; }

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

    public ConnectedNode(string ip, string port, string size)
    {
        IpAddress = IPAddress.Parse(ip);
        Port = Convert.ToInt32(port);
        GlobalMemory = Convert.ToInt32(size);
        UsedMemory = 0;
        _savedFiles = new Dictionary<string, List<string>>();
        CheckTcpConnection();
    }

    private void CheckTcpConnection()
    {
        var active = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().ToImmutableList();
        var correctNode = active.Any(x => Equals(x.Address, IpAddress) & Equals(x.Port, Port));

        if (!correctNode)
        {
            throw new FormatException($"Node with TcpListener '{this}' doesn't exist!");
        }
    }

    public void SaveTransportedFileSourceData(string fsPath, string partialPath)
    {
        if (!_savedFiles.ContainsKey(fsPath))
        {
            _savedFiles.Add(fsPath, new List<string>());
        }

        UsedMemory += new FileInfo(fsPath).Length;
        _savedFiles[fsPath].Add(partialPath);
    }

    public ImmutableList<string> GetFilesToRemove(string fsPath)
    {
        var files = _savedFiles.ContainsKey(fsPath)
            ? _savedFiles[fsPath].ToImmutableList()
            : ImmutableList<string>.Empty;

        return files;
    }
    
    public void RemoveTransportedFileData(string fsPath, string partialPath)
    {
        if (_savedFiles.ContainsKey(fsPath))
        {
            UsedMemory -= new FileInfo(fsPath).Length;
            _savedFiles[fsPath].Remove(partialPath);
        }

        if (!_savedFiles[fsPath].Any())
        {
            _savedFiles.Remove(fsPath);
        }
    }

    public bool HaveFreeMemory(long newFileSize)
    {
        return UsedMemory + newFileSize < GlobalMemory;
    }
    
    public override string ToString()
    {
        return $"{IpAddress}:{Port} {UsedMemory}/{GlobalMemory}";
    }

    protected bool Equals(ConnectedNode other)
    {
        return IpAddress.Equals(other.IpAddress) && Port == other.Port;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IpAddress, Port);
    }

    public int NumberOfSavedFiles()
    {
        return _savedFiles.Sum(x => x.Value.Count);
    }
}