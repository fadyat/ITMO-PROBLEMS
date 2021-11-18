using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Backups.Classes.StorageMethods
{
    public class FileSystemStorage : IStorageMethod
    {
        public string ConstructPath(string path, string name)
        {
            path = Path.Combine(path, name);
            return path;
        }

        public void MakeDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void Archive(IEnumerable<JobObject> from, string where)
        {
            ZipArchive zipArchive = ZipFile.Open(where, ZipArchiveMode.Create);
            foreach (JobObject jobObject in from)
            {
                string fileName = Path.GetFileName(jobObject.Path);
                zipArchive.CreateEntryFromFile(jobObject.Path, fileName);
            }
        }

        public bool ExistsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        public bool ExistsFile(string path)
        {
            return File.Exists(path);
        }
    }
}