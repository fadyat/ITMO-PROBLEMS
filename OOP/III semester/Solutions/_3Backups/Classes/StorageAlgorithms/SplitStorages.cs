using System.Collections.Generic;
using System.IO;
using Backups.Classes.StorageMethods;

namespace Backups.Classes.StorageAlgorithms
{
    public class SplitStorages : IStorageAlgorithm
    {
        public List<Storage> Compression(
            string path,
            IEnumerable<JobObject> objects,
            IStorageMethod storageMethod)
        {
            var list = new List<Storage>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (JobObject jobObject in objects)
            {
                string fileName = Path.GetFileNameWithoutExtension(jobObject.Path);
                list.Add(new Storage(
                    fileName,
                    path,
                    new List<JobObject> { jobObject },
                    storageMethod));
            }

            return list;
        }
    }
}