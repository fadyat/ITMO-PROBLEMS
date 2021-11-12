using System.Collections.Generic;
using Backups.Classes.StorageMethods;

namespace Backups.Classes
{
    public class Storage
    {
        public Storage(
            string storageName,
            string path,
            IEnumerable<string> filesPath,
            IStorageMethod storageMethod)
        {
            Path = storageMethod.ConstructPath(path, storageName + ".zip");
            storageMethod.Archive(filesPath, Path);
        }

        public string Path { get; }
    }
}