using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Classes.JobObjects;
using Backups.Classes.Storages;

namespace Backups.Classes.StorageMethods
{
    public class AbstractFileSystemStorage : IStorageMethod
    {
        public AbstractFileSystemStorage()
        {
            PathDirectories = new HashSet<string>();
            PathFiles = new HashSet<string>();
            ArchivedFiles = new Dictionary<string, List<IJobObject>>();
        }

        protected HashSet<string> PathDirectories { get; }

        protected HashSet<string> PathFiles { get; }

        protected Dictionary<string, List<IJobObject>> ArchivedFiles { get; }

        public string ConstructPath(string path, string name)
        {
            path = Path.Combine(path, name);
            return path;
        }

        public void MakeDirectory(string path)
        {
            if (PathDirectories.Contains(path)) return;
            PathDirectories.Add(path);
        }

        public void Archive(Storage from)
        {
            PathFiles.Add(from.FullPath);
            ArchivedFiles.Add(from.FullPath, from.JobObjects.ToList());
        }

        public bool ExistsDirectory(string path)
        {
            return PathDirectories.Contains(path);
        }

        public bool ExistsFile(string path)
        {
            return PathFiles.Contains(path);
        }
    }
}