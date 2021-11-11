using System;
using System.Collections.Generic;
using Backups.Classes;
using Backups.Classes.StorageAlgorithms;

namespace Backups.Services
{
    public class RestorePointService
    {
        private readonly List<RestorePoint> _restorePoints;
        private int _issuedRestorePointId;

        public RestorePointService()
        {
            _issuedRestorePointId = 100000;
            _restorePoints = new List<RestorePoint>();
        }

        public RestorePoint CreateRestorePoint(
            string name,
            BackupJob backupJob,
            IStorageAlgorithm storageAlgorithm)
        {
            var restorePoint = new RestorePoint(name, _issuedRestorePointId++, backupJob, storageAlgorithm);
            _restorePoints.Add(restorePoint);
            return restorePoint;
        }
    }
}