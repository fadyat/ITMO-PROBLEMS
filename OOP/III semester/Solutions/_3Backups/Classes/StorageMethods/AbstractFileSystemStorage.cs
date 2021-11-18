using System.Collections.Generic;
using System.IO;

namespace Backups.Classes.StorageMethods
{
    public class AbstractFileSystemStorage : IStorageMethod
    {
        private readonly HashSet<string> _pathDirectories;
        private readonly HashSet<string> _pathFiles;

        public AbstractFileSystemStorage()
        {
            _pathDirectories = new HashSet<string>();
            _pathFiles = new HashSet<string>();
        }

        public string ConstructPath(string path, string name)
        {
            path = Path.Combine(path, name);
            return path;
        }

        public void MakeDirectory(string path)
        {
            _pathDirectories.Add(path);
        }

        public void Archive(IEnumerable<JobObject> from, string where)
        {
            _pathFiles.Add(where); // archive

            // files inside ...
        }

        public bool ExistsDirectory(string path)
        {
            return _pathDirectories.Contains(path);
        }

        public bool ExistsFile(string path)
        {
            return _pathFiles.Contains(path);
        }
    }
}