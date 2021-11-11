using System;
using System.Collections.Generic;
using Backups.Classes.StorageAlgorithms;

namespace Backups.Classes
{
    public class RestorePoint
    {
        private readonly List<Storage> _storages;
        private int _id;
        private int _backupId;
        private string _path;
        private DateTime _time;

        public RestorePoint(
            int id,
            BackupJob backupJob,
            IStorageAlgorithm storageAlgorithm)
        {
            _id = id;
            _backupId = backupJob.Id;
            string restorePointName = "restorePoint" + _id;
            _path = System.IO.Path.Combine(backupJob.Path, restorePointName);
            System.IO.Directory.CreateDirectory(_path);
            _storages = storageAlgorithm.Compression(_path, backupJob.FilePaths);
            _time = DateTime.Now;
        }
    }
}