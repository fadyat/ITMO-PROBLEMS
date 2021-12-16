using System.IO;
using System.IO.Compression;
using Backups.Classes.JobObjects;
using Backups.Classes.Storages;

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
            if (ExistsDirectory(path)) return;
            Directory.CreateDirectory(path);
        }

        public void Archive(Storage from)
        {
            if (ExistsFile(from.FullPath)) return;
            ZipArchive zipArchive = ZipFile.Open(from.FullPath, ZipArchiveMode.Create);
            foreach (IJobObject jobObject in from.JobObjects)
            {
                string fileName = Path.GetFileName(jobObject.Path);
                if (jobObject.Path == null || fileName == null) continue;
                zipArchive.CreateEntryFromFile(jobObject.Path, fileName);
            }

            zipArchive.Dispose();
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