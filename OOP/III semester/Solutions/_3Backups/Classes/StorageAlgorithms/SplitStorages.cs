using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Classes.StorageMethods;

namespace Backups.Classes.StorageAlgorithms
{
    public class SplitStorages : IStorageAlgorithm
    {
        public List<Storage> Compression(
            string path,
            IEnumerable<string> filePaths,
            IStorageMethod storageMethod)
        {
            return (from filePath in filePaths
                    let fileName = Path.GetFileNameWithoutExtension(filePath)
                    select new Storage(
                        fileName,
                        path,
                        new List<string> { filePath },
                        storageMethod))
                .ToList();
        }
    }
}