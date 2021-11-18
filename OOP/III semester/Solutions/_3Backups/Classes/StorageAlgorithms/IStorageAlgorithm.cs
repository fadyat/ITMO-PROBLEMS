using System.Collections.Generic;
using Backups.Classes.StorageMethods;

namespace Backups.Classes.StorageAlgorithms
{
    public interface IStorageAlgorithm
    {
        List<Storage> Compression(
            string path,
            IEnumerable<JobObject> objects,
            IStorageMethod storageMethod);
    }
}