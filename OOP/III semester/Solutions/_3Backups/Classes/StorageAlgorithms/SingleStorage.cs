using System.Collections.Generic;
using Backups.Classes.StorageMethods;

namespace Backups.Classes.StorageAlgorithms
{
    public class SingleStorage : IStorageAlgorithm
    {
        public List<Storage> Compression(
            string path,
            IEnumerable<string> filePaths,
            IStorageMethod storageMethod)
        {
            var storages = new List<Storage>();
            var newStorage = new Storage(
                "Single",
                path,
                filePaths,
                storageMethod);

            storages.Add(newStorage);
            return storages;
        }
    }
}