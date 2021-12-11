using System;
using System.Collections.Generic;
using System.IO;
using Backups.Classes.JobObjects;
using Backups.Classes.StorageAlgorithms;
using Backups.Services;
using BackupsExtra.Classes.BackupJobsExtra;
using BackupsExtra.Classes.Selection;
using BackupsExtra.Classes.Serialization;
using BackupsExtra.Classes.StorageMethodsExtra;
using BackupsExtra.Services;

namespace BackupsExtra
{
    internal class Program
    {
        private static void Main()
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory())?
                .Parent?
                .Parent?
                .FullName;

            var service = new BackupExtraJobService(
                new BackupJobService(path, new FileSystemStorageExtra()),
                new FileSystemStorageExtra());

            BackupJobExtra backup = service.CreateBackup(
                new HashSet<IJobObject>
                {
                    new JobObject("/Users/artyomfadeyev/Documents/a.txt"),
                    new JobObject("/Users/artyomfadeyev/Documents/b.txt"),
                    new JobObject("/Users/artyomfadeyev/Documents/c.txt"),
                },
                new SplitStorages());

            backup.CreateRestorePoint();
            backup.CreateRestorePoint();
            backup.CreateRestorePoint();

            backup.Clear(new ByNumberSelection(2));
            backup.RemoveJobObject(new JobObject("/Users/artyomfadeyev/Documents/c.txt"));
            backup.CreateRestorePoint();
            backup.RemoveJobObject(new JobObject("/Users/artyomfadeyev/Documents/b.txt"));
            backup.CreateRestorePoint();
            backup.AddJobObject(new JobObject("/Users/artyomfadeyev/Documents/d.txt"));
            backup.CreateRestorePoint();

            backup.Merge(new ByNumberSelection(3));

            // var json = new StorageMethodExtraJson(path);

            // json.Save(service);
            // BackupExtraJobService load = json.Load();
            // Console.WriteLine(load.FullPath);
        }
    }
}