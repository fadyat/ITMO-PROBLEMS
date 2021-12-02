using System.Collections.Generic;
using System.Linq;
using Backups.Classes;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Backups.Exceptions;

namespace Backups.Services
{
    public class BackupJobService : IBackupJobService
    {
        private readonly List<BackupJob> _backups;

        public BackupJobService(
            string location,
            IStorageMethod storageMethod,
            string folderName = "Repository")
        {
            StorageMethod = storageMethod;
            _backups = new List<BackupJob>();
            IssuedBackupId = 100000;
            FolderName = folderName;
            Location = StorageMethod.ConstructPath(location, folderName);
            storageMethod.MakeDirectory(Location);
        }

        public IEnumerable<BackupJob> Backups => _backups;

        public IStorageMethod StorageMethod { get; }

        public string Location { get; }

        public string FolderName { get; }

        public int IssuedBackupId { get; private set; }

        public BackupJob CreateBackup(
            HashSet<JobObject> objects,
            IStorageAlgorithm storageAlgorithm,
            string name = "backupJob_")
        {
            if (Equals(name, null))
            {
                throw new BackupException("Backup name couldn't be null!");
            }

            name += name.EndsWith("_") ? (_backups.Count + 1).ToString() : string.Empty;

            var backupJob = new BackupJob(
                IssuedBackupId++,
                Location,
                objects,
                name,
                storageAlgorithm,
                StorageMethod);

            _backups.Add(backupJob);
            return backupJob;
        }

        public BackupJob GetBackup(int id)
        {
            BackupJob backup = _backups.SingleOrDefault(backup => Equals(backup.Id, id));

            if (Equals(backup, null))
            {
                throw new BackupException("No such backup!");
            }

            return backup;
        }
    }
}