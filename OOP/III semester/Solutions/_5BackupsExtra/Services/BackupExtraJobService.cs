using System.Collections.Generic;
using Backups.Classes.BackupJobs;
using Backups.Classes.StorageMethods;
using Backups.Services;

namespace BackupsExtra.Services
{
    public class BackupExtraJobService : IBackupJobService
    {
        private readonly BackupJobService _backupJobService;

        public BackupExtraJobService(string path, IStorageMethod storageMethod, string name = null)
        {
            _backupJobService = new BackupJobService(path, storageMethod, name);
        }

        public IEnumerable<IBackupJob> Backups => _backupJobService.Backups;

        public IStorageMethod StorageMethod => _backupJobService.StorageMethod;

        public string Path => _backupJobService.Path;

        public string FullPath => _backupJobService.FullPath;

        public string Name => _backupJobService.Name;

        public void CreateBackup(IBackupJob backupJob)
        {
            _backupJobService.CreateBackup(backupJob);
        }
    }
}