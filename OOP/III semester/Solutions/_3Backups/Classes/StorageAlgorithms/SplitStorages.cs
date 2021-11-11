using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Backups.Classes.StorageAlgorithms
{
    public class SplitStorages : IStorageAlgorithm
    {
        public List<Storage> Compression(string path, IEnumerable<string> filePaths)
        {
            return (from filePath in filePaths
                    let fileName = Path.GetFileName(filePath)
                    select new Storage("Split_" + fileName, path, new List<string> { filePath }))
                .ToList();
        }
    }
}