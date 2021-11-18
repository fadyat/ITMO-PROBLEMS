using System.Collections.Generic;
using System.Linq;
using Backups.Classes;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Backups.Exceptions;

namespace Backups.Services
{
    public class BackupJobService
    {
        private readonly List<BackupJob> _backups;
        private readonly IStorageMethod _storageMethod;
        private readonly string _location;
        private int _issuedBackupId;

        public BackupJobService(
            string location,
            IStorageMethod storageMethod,
            string folderName = "Repository")
        {
            _storageMethod = storageMethod;
            _backups = new List<BackupJob>();
            _issuedBackupId = 100000;
            _location = _storageMethod.ConstructPath(location, folderName);
            storageMethod.MakeDirectory(_location);
        }

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
                _issuedBackupId++,
                _location,
                objects,
                name,
                storageAlgorithm,
                _storageMethod);

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