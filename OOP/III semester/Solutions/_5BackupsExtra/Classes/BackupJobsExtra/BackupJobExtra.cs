using System.Collections.Generic;
using System.Linq;
using Backups.Classes.BackupJobs;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.Storages;
using Backups.Exceptions;
using BackupsExtra.Classes.Selection;
using BackupsExtra.Classes.StorageMethodsExtra;

namespace BackupsExtra.Classes.BackupJobsExtra
{
    public class BackupJobExtra : BackupJobDecorator
    {
        public BackupJobExtra(BackupJobComponent component, IStorageExtraMethod storageExtraMethod)
            : base(component)
        {
            StorageMethod = storageExtraMethod;
        }

        public new IStorageExtraMethod StorageMethod { get; }

        public void Clear(ISelection selection)
        {
            if (selection.Fetch(LinkedRestorePoints) is not LinkedList<RestorePoint> result || !result.Any())
                throw new BackupException("Selection size can't be 0");

            foreach (RestorePoint point in LinkedRestorePoints
                .TakeWhile(point => !Equals(point, result.First())))
            {
                StorageMethod.RemoveRestorePoint(point);
            }

            LinkedRestorePoints.Clear();
            foreach (RestorePoint res in result)
            {
                LinkedRestorePoints.AddLast(res);
            }
        }

        public void Merge(ISelection selection)
        {
            if (selection.Fetch(LinkedRestorePoints) is not LinkedList<RestorePoint> result || !result.Any())
                throw new BackupException("Selection size can't be 0");

            // mistake
            // mistake
            // mistake
            // mistake
            // mistake
            var toMerge = new List<RestorePoint>();
            while (result.First() != LinkedRestorePoints.First())
            {
                toMerge.Add(LinkedRestorePoints.First());
                LinkedRestorePoints.RemoveFirst();
            }

            toMerge.Reverse();

            foreach (RestorePoint point in toMerge)
            {
                if (StorageMethod.GetType() == typeof(SingleStorage))
                {
                    StorageMethod.RemoveRestorePoint(point);
                    continue;
                }

                IEnumerable<Storage> addedStorages = StorageMethod.Merge(point, result.First());
                RestorePoint correctRestorePoint = LinkedRestorePoints.First();
                var newStorages = correctRestorePoint.StoragesI.Concat(addedStorages) as LinkedList<Storage>;
                LinkedRestorePoints.RemoveFirst();

                var newRestorePoint = new RestorePoint(
                    correctRestorePoint.Path,
                    newStorages,
                    correctRestorePoint.Name,
                    correctRestorePoint.CreationDate);

                LinkedRestorePoints.AddFirst(newRestorePoint);
            }
        }
    }
}