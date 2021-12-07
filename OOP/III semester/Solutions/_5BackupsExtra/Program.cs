using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Backups.Services;
using BackupsExtra.Classes.Serialization;

namespace BackupsExtra
{
    internal class Program
    {
        private static void Main()
        {
            string position = Directory.GetParent(Directory.GetCurrentDirectory())?
                .Parent?
                .Parent?
                .FullName;

            if (position == null) return;
/*
            var backupJobService = new BackupJobService(position, new FileSystemStorage());

            var backup = new BackupJob(
                backupJobService.FullPath,
                new HashSet<IJobObject>
                {
                    new JobObject("/Users/artyomfadeyev/Documents/a.txt"),
                    new JobObject("/Users/artyomfadeyev/Documents/b.txt"),
                    new JobObject("/Users/artyomfadeyev/Documents/c.txt"),
                },
                new SplitStorages(),
                backupJobService.StorageMethod);

            backupJobService.CreateBackup(backup);

            backup.CreateRestorePoint(new RestorePoint(backup.FullPath));
            backup.RemoveJobObject(new JobObject("/Users/artyomfadeyev/Documents/a.txt"));
            backup.CreateRestorePoint(new RestorePoint(backup.FullPath));
*/
            var fix = new StorageMethodExtraJson(position);

            // fix.Save(backupJobService);
            IBackupJobService b = fix.Load();
            Console.WriteLine(b.Backups == null);
            Console.WriteLine(b.Path);
            /*
//             BackupJob b1 = b.CreateBackup(
//                 new HashSet<JobObject>()
//                 {
//                     new ("Users/artyomfadeyev/Documents/a.txt"),
//                     new ("Users/artyomfadeyev/Documents/b.txt"),
//                     new ("Users/artyomfadeyev/Documents/c.txt"),
//                 },
//                 new SplitStorages(),
//                 "lol1");
//
//             b1.CreateRestorePoint();*/
        }
    }
}