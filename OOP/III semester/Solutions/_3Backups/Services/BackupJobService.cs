using System.Collections.Generic;
using Backups.Classes;

namespace Backups.Services
{
    public class BackupJobService
    {
        private readonly List<BackupJob> _backups;
        private readonly string _location;
        private int _issuedBackupId;

        public BackupJobService(string folderName, string location)
        {
            _location = System.IO.Path.Combine(location, folderName);
            System.IO.Directory.CreateDirectory(_location);
            _backups = new List<BackupJob>();
            _issuedBackupId = 100000;
        }

        public BackupJob CreateBackup(HashSet<string> filePaths)
        {
            var backupJob = new BackupJob(_issuedBackupId++, _location, filePaths);
            _backups.Add(backupJob);
            return backupJob;
        }
    }
}