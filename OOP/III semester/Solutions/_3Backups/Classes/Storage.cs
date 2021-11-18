using System.Collections.Generic;
using Backups.Classes.StorageMethods;

namespace Backups.Classes
{
    public class Storage
    {
        public Storage(
            string storageName,
            string path,
            IEnumerable<JobObject> objects,
            IStorageMethod storageMethod)
        {
            Path = storageMethod.ConstructPath(path, storageName + ".zip");
            storageMethod.Archive(objects, Path);
        }

        public string Path { get; }
    }
}