using System.Collections.Generic;

namespace Backups.Classes
{
    public class BackupJob
    {
        private readonly HashSet<string> _filePaths;

        public BackupJob(int id, string path, HashSet<string> filePaths)
        {
            Id = id;
            string backupJobName = "backupJob" + Id;
            Path = System.IO.Path.Combine(path, backupJobName);
            System.IO.Directory.CreateDirectory(Path);
            _filePaths = filePaths;
        }

        public int Id { get; }

        public string Path { get; }

        public IEnumerable<string> FilePaths => _filePaths;

        public void AddFile(string filePath)
        {
            _filePaths.Add(filePath);
        }

        public void RemoveFile(string filePath)
        {
            _filePaths.Remove(filePath);
        }
    }
}