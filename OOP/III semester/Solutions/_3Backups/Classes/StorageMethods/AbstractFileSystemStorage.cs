using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Classes.JobObjects;

namespace Backups.Classes.StorageMethods
{
    public class AbstractFileSystemStorage : IStorageMethod
    {
        public AbstractFileSystemStorage()
        {
            PathDirectories = new HashSet<string>();
            PathFiles = new HashSet<string>();
            ArchivedFiles = new Dictionary<string, int>();
        }

        protected HashSet<string> PathDirectories { get; }

        protected HashSet<string> PathFiles { get; }

        protected Dictionary<string, int> ArchivedFiles { get; }

        public string ConstructPath(string path, string name)
        {
            path = Path.Combine(path, name);
            return path;
        }

        public void MakeDirectory(string path)
        {
            PathDirectories.Add(path);
        }

        public void Archive(IEnumerable<IJobObject> from, string where)
        {
            PathFiles.Add(where);
            ArchivedFiles.Add(where, from.Count()); // pseudo-archive
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