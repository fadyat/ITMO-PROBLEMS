using System.Collections.Generic;
using Backups.Services;
using BackupsExtra.Classes.BackupJobsExtra;
using BackupsExtra.Classes.StorageMethodsExtra;

namespace BackupsExtra.Services
{
    public class BackupExtraJobService : BackupJobService
    {
        public BackupExtraJobService(string path, IStorageExtraMethod storageMethod, string name = null)
            : base(path, storageMethod, name)
        {
            StorageMethod = storageMethod;
            Backups = new HashSet<IBackupJobExtra>();
        }

        public new IStorageExtraMethod StorageMethod { get; }

        public new IEnumerable<IBackupJobExtra> BackupsI => Backups;

        private HashSet<IBackupJobExtra> Backups { get; }

        public void CreateBackup(IBackupJobExtra backupJob)
        {
            Backups.Add(backupJob);
            StorageMethod.MakeDirectory(backupJob.FullPath);
        }
    }
}