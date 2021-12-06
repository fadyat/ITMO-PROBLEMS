using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;

namespace Backups
{
    internal static class Program
    {
        private static void Main()
        {
            var bank = new BackupJob(
                "123",
                new HashSet<IJobObject>
                {
                    new JobObject("1"),
                    new JobObject("2"),
                },
                new SingleStorage(),
                new AbstractFileSystemStorage());

            Console.WriteLine(bank.Objects.ToList().Count);

            IJobObject c = bank.Objects.First();

            bank.RemoveJobObject(c);

            Console.WriteLine(bank.Objects.ToList().Count);
        }
    }
}