using System.Collections.Generic;
using System.Linq;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageAlgorithms;
using Backups.Exceptions;
using BackupsExtra.Classes.Selection;
using BackupsExtra.Classes.StorageMethodsExtra;

namespace BackupsExtra.Classes.BackupJobsExtra
{
    public class BackupJobExtra : BackupJob, IBackupJobExtra
    {
        public BackupJobExtra(
            string path,
            IEnumerable<IJobObject> objects,
            IStorageAlgorithm storageAlgorithm,
            IStorageExtraMethod storageMethod,
            string name = null)
            : base(path, objects, storageAlgorithm, storageMethod, name)
        {
            StorageMethod = storageMethod;
        }

        public new IStorageExtraMethod StorageMethod { get; }

        public void Clear(ISelection selection)
        {
            if (selection.Fetch(LinkedRestorePoints) is not LinkedList<IRestorePoint> result || !result.Any())
                throw new BackupException("Selection size can't be 0");

            foreach (IRestorePoint point in LinkedRestorePoints
                .TakeWhile(point => !Equals(point, result.First())))
            {
                StorageMethod.RemoveRestorePoint(point);
            }

            LinkedRestorePoints = result;
        }

        public void Merge(ISelection selection)
        {
            if (selection.Fetch(LinkedRestorePoints) is not LinkedList<IRestorePoint> result || !result.Any())
                throw new BackupException("Selection size can't be 0");

            var merged = new LinkedList<IRestorePoint>(LinkedRestorePoints);
            foreach (IRestorePoint point in LinkedRestorePoints
                .TakeWhile(point => !Equals(point, result.First())))
            {
                merged.Remove(point);
                if (StorageMethod.GetType() == typeof(SingleStorage))
                {
                    StorageMethod.RemoveRestorePoint(point);
                    continue;
                }

                StorageMethod.Merge(point, result.First());
            }

            LinkedRestorePoints = merged;
        }
    }
}