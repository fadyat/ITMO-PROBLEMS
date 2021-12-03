using System;
using System.Collections.Generic;
using System.IO;
using Backups.Classes;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Backups.Services;
using BackupsExtra.Classes;
using BackupsExtra.Services;

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

            var backupJobService = new BackupExtraJobService(position, new AbstractFileSystemStorage(), "Kek");
            BackupJob backup = backupJobService.CreateBackup(
                new HashSet<JobObject>()
                {
                    new ("/Users/artyomfadeyev/Documents/a.txt"),
                    new ("/Users/artyomfadeyev/Documents/b.txt"),
                    new ("/Users/artyomfadeyev/Documents/c.txt"),
                },
                new SplitStorages(),
                "lol");

            backup.CreateRestorePoint();
            var fix = new StorageMethodExtraJson(position);

            fix.Save(backupJobService);
            BackupExtraJobService b = fix.Load();
            Console.WriteLine(b.Backups == null);
            Console.WriteLine(b.Location);
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