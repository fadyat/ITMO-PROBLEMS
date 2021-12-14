using System;
using System.Collections.Generic;
using System.IO;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageAlgorithms;
using Backups.Services;
using BackupsExtra.Classes.BackupJobsExtra;
using BackupsExtra.Classes.BackupLogs;
using BackupsExtra.Classes.Recovery;
using BackupsExtra.Classes.Selection;
using BackupsExtra.Classes.Serialization;
using BackupsExtra.Classes.StorageMethodsExtra;
using BackupsExtra.Services;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

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

            if (path == null) return;

            var service = new BackupExtraJobService(
                new BackupJobService(path, new FileSystemStorageExtra(), "7"),
                new FileSystemStorageExtra());

            BackupJobExtra backup = service.CreateBackup(
                new HashSet<IJobObject>
                {
                    new JobObject("/Users/artyomfadeyev/Documents/a.txt"),
                    new JobObject("/Users/artyomfadeyev/Documents/b.txt"),
                    new JobObject("/Users/artyomfadeyev/Documents/c.txt"),
                },
                new SingleStorage(),
                new ConsoleLogger(),
                "backup");

            backup.CreateRestorePoint("1");

            backup.CreateRestorePoint("2");
            backup.CreateRestorePoint("3");
            backup.Clear(new ByNumberSelection(2));
            backup.RemoveJobObject(new JobObject("/Users/artyomfadeyev/Documents/c.txt"));
            backup.CreateRestorePoint("4");
            backup.RemoveJobObject(new JobObject("/Users/artyomfadeyev/Documents/b.txt"));
            backup.CreateRestorePoint("5");
            backup.AddJobObject(new JobObject("/Users/artyomfadeyev/Documents/d.txt"));
            backup.AddJobObject(new JobObject("/Users/artyomfadeyev/Documents/adqweqwe.txt"));
            backup.CreateRestorePoint("6");
            backup.Merge(new ByNumberSelection(1));
            RestorePoint lastRp = backup.Top();
            var recovery = new DifferentLocationRecover(Path.Combine(path, "RECOVER"));
            recovery.Restore(backup.StorageMethod, lastRp);

            /*
            var json = new StorageMethodExtraJson(path);

            json.Save(service);*/

            // BackupExtraJobService load = json.Load();

            // Console.WriteLine(load.FullPath);
            // foreach (BackupJobExtra back in load.BackupsI)
            // {
            // Console.Write(back.Name + " " + back.RestorePoints.Count);
            // }

            // Console.WriteLine();
        }
    }
}