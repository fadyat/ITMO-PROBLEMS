using System.Collections.Generic;

namespace Backups.Classes.StorageAlgorithms
{
    public class SingleStorage : IStorageAlgorithm
    {
        public List<Storage> Compression(string path, IEnumerable<string> filePaths)
        {
            var storages = new List<Storage>();
            var newStorage = new Storage("SingleStorage", path, filePaths);
            storages.Add(newStorage);
            return storages;
        }
    }
}