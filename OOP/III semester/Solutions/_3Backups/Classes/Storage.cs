using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Classes.StorageMethods;

namespace Backups.Classes
{
    public class Storage
    {
        private readonly string _path;

        public Storage(
            string storageName,
            string path,
            IEnumerable<string> filesPath,
            IStorageMethod storageMethod)
        {
            _path = storageMethod.ConstructPath(path, storageName + ".zip");
            storageMethod.Archive(filesPath, _path);
        }
    }
}