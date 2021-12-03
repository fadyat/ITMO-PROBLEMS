using System.Collections.Generic;
using System.IO;
using Backups.Classes.StorageMethods;

namespace Backups.Classes.StorageAlgorithms
{
    public class SplitStorages : IStorageAlgorithm
    {
        public List<Storage> CreateStorages(
            string path, IEnumerable<JobObject> objects, IStorageMethod storageMethod)
        {
            var list = new List<Storage>();

            foreach (JobObject jobObject in objects)
            {
                string fileName = Path.GetFileNameWithoutExtension(jobObject.Path);
                var storage = new Storage(fileName, path, storageMethod);
                storageMethod.Archive(new List<JobObject> { jobObject }, storage.Path);
                list.Add(storage);
            }

            return list;
        }
    }
}