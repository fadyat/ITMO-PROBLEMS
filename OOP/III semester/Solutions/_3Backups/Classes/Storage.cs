using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Backups.Classes
{
    public class Storage
    {
        private string _path;

        public Storage(string storageName, string path, IEnumerable<string> filesPath)
        {
            _path = Path.Combine(path, storageName + ".zip");
            ZipArchive zipArchive = ZipFile.Open(_path, ZipArchiveMode.Create);
            foreach (string filePath in filesPath)
            {
                string fileName = Path.GetFileName(filePath);
                zipArchive.CreateEntryFromFile(filePath, fileName);
            }
        }
    }
}