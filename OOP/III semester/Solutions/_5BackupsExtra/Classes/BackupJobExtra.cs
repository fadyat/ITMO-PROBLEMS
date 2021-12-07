using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageAlgorithms;
using Backups.Exceptions;
using BackupsExtra.Classes.Selection;
using BackupsExtra.Classes.StorageMethodsExtra;

namespace BackupsExtra.Classes
{
    public class BackupJobExtra : BackupJob
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
                throw new BackupException("Clear algorithm can't remove all points!");

            foreach (IRestorePoint point in LinkedRestorePoints
                .TakeWhile(point => !Equals(point, result.First())))
            {
                StorageMethod.RemoveRestorePoint(point);
            }

            LinkedRestorePoints = result;
        }
    }
}