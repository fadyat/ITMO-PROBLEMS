using System.Collections.Generic;
using Backups.Classes.JobObjects;
using Backups.Classes.StorageMethods;
using Backups.Classes.Storages;

namespace Backups.Classes.StorageAlgorithms
{
    public interface IStorageAlgorithm
    {
        LinkedList<Storage> CreateStorages(string path, IEnumerable<IJobObject> objects, IStorageMethod storageMethod);
    }
}