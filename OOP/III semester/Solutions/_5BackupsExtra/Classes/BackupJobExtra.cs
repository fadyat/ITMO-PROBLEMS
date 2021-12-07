using System.Collections.Generic;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Backups.Exceptions;
using BackupsExtra.Classes.Selection;

namespace BackupsExtra.Classes
{
    public class BackupJobExtra : BackupJob
    {
        public BackupJobExtra(
            string path,
            IEnumerable<IJobObject> objects,
            IStorageAlgorithm storageAlgorithm,
            IStorageMethod storageMethod,
            string name = null)
            : base(path, objects, storageAlgorithm, storageMethod, name)
        {
        }

        public void Clear(ISelection selection)
        {
            var result = selection.Clear(LinkedRestorePoints) as LinkedList<IRestorePoint>;
            if (result is { Count: <= 0 })
                throw new BackupException("Clear algorithm can't remove all points!");

            LinkedRestorePoints = result;
        }
    }
}