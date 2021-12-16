using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.Storages;
using Backups.Exceptions;
using BackupsExtra.Classes.BackupLogs;
using BackupsExtra.Classes.Recovery;
using BackupsExtra.Classes.Selection;
using BackupsExtra.Classes.StorageMethodsExtra;
using Newtonsoft.Json;

namespace BackupsExtra.Classes.BackupJobsExtra
{
    public class BackupJobExtra : BackupJobDecorator
    {
        public BackupJobExtra(BackupJobComponent component, IStorageMethodExtra storageMethod, IMyLogger myLogger)
            : base(component)
        {
            StorageMethod = storageMethod;
            MyLogger = myLogger;
            LinkedRestorePoints = new LinkedList<RestorePoint>();
            MyLogger.Info("BackupExtraJob was created!");
        }

        public new IStorageMethodExtra StorageMethod { get; }

        [JsonIgnore]
        public new ImmutableList<RestorePoint> RestorePoints => LinkedRestorePoints.ToImmutableList();

        [JsonProperty] // to protected field
        public new LinkedList<RestorePoint> LinkedRestorePoints { get; set; }

        [JsonProperty]
        private IMyLogger MyLogger { get; }

        public override void AddJobObject(IJobObject jobObject)
        {
            base.AddJobObject(jobObject);
            MyLogger.Info("Add new jobObject!");
        }

        public override void RemoveJobObject(IJobObject jobObject)
        {
            base.RemoveJobObject(jobObject);
            MyLogger.Info("Remove jobObject!");
        }

        public override RestorePoint CreateRestorePoint(string name = null, DateTime dateTime = default)
        {
            if (Equals(PublicObjects.Count, 0))
                throw new BackupException("No files for restore point!");

            name ??= Guid.NewGuid().ToString();
            string futureRestorePath = System.IO.Path.Combine(FullPath, name);
            StorageMethod.MakeDirectory(futureRestorePath);
            LinkedList<Storage> storages =
                StorageAlgorithm.CreateStorages(futureRestorePath, PublicObjects, StorageMethod);
            var restorePoint = new RestorePoint(FullPath, storages, name, dateTime);
            LinkedRestorePoints.AddLast(restorePoint);

            MyLogger.Info($"Restore point was created with {storages.Count} storages!");
            return restorePoint;
        }

        public void Clear(ISelection selection)
        {
            if (selection.Fetch(LinkedRestorePoints) is not { } result || !result.Any())
                throw new BackupException("Selection size can't be 0");

            foreach (RestorePoint point in LinkedRestorePoints
                .TakeWhile(point => !Equals(point, result.First())))
            {
                StorageMethod.RemoveRestorePoint(point);
            }

            LinkedRestorePoints = result;
            MyLogger.Info("Cleared restore points by selection!");
        }

        public void Merge(ISelection selection)
        {
            if (selection.Fetch(LinkedRestorePoints) is not { } result || !result.Any())
                throw new BackupException("Selection size can't be 0");

            var toMerge = LinkedRestorePoints.Except(result)
                .Reverse()
                .ToList();

            LinkedRestorePoints = result;
            foreach (RestorePoint point in toMerge)
            {
                if (StorageMethod.GetType() == typeof(SingleStorage))
                {
                    StorageMethod.RemoveRestorePoint(point);
                    continue;
                }

                RestorePoint correctRestorePoint = LinkedRestorePoints.First();
                LinkedRestorePoints.RemoveFirst();

                IEnumerable<Storage> addedStorages = StorageMethod.Merge(point, correctRestorePoint);
                var newStorages = new LinkedList<Storage>();
                foreach (Storage storage in correctRestorePoint.PublicStorages)
                {
                    newStorages.AddLast(storage);
                }

                foreach (Storage storage in addedStorages)
                {
                    newStorages.AddLast(storage);
                }

                var newRestorePoint = new RestorePoint(
                    correctRestorePoint.Path,
                    newStorages,
                    correctRestorePoint.Name,
                    correctRestorePoint.CreationDate);

                LinkedRestorePoints.AddFirst(newRestorePoint);
            }

            MyLogger.Info("Merged restore points by selection!");
        }

        public void Recover(IRecovery recovery, RestorePoint restorePoint)
        {
            recovery.Recover(StorageMethod, restorePoint);
        }

        public RestorePoint Top()
        {
            return LinkedRestorePoints.Last();
        }

        public RestorePoint Front()
        {
            return LinkedRestorePoints.First();
        }
    }
}