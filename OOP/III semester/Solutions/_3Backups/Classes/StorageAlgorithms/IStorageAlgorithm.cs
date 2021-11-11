using System.Collections.Generic;

namespace Backups.Classes.StorageAlgorithms
{
    public interface IStorageAlgorithm
    {
        List<Storage> Compression(string path, IEnumerable<string> filePaths);
    }
}