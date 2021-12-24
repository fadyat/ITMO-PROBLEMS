using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageAlgorithms;
using Backups.Services;
using BackupsExtra.Classes.BackupJobsExtra;
using BackupsExtra.Classes.BackupLogs;
using BackupsExtra.Classes.Recovery;
using BackupsExtra.Classes.Selection;
using BackupsExtra.Classes.StorageMethodsExtra;
using BackupsExtra.Services;
using NUnit.Framework;

namespace BackupsExtra.Tests
{
    public class BackupsExtraTest
    {
        private BackupJobServiceExtra _service;
        private List<IJobObject> _objects;
        private IStorageMethodExtra _storageMethod;

        [SetUp]
        public void SetUp()
        {
            string dataStorage = Path.Combine(
                Path.Combine(
                    Directory.GetParent(Directory.GetCurrentDirectory())?
                        .Parent?
                        .FullName!),
                "Data"
            );

            _storageMethod = new AbstractFileSystemStorageExtra();

            _service = new BackupJobServiceExtra(
                new BackupJobService(dataStorage, _storageMethod),
                _storageMethod
            );

            _objects = new List<IJobObject>
            {
                new JobObject("/Users/artyomfadeyev/Documents/a.txt"),
                new JobObject("/Users/artyomfadeyev/Documents/b.txt"),
                new JobObject("/Users/artyomfadeyev/Documents/c.txt")
            };
        }

        [Test]
        public void ClearTest_NumberSelection()
        {
            BackupJobExtra backup1 = _service.CreateBackup(
                _objects,
                new SplitStorages(),
                new ConsoleLogger()
            );

            RestorePoint rp1B1 = backup1.CreateRestorePoint();
            RestorePoint rp2B1 = backup1.CreateRestorePoint();
            RestorePoint rp3B1 = backup1.CreateRestorePoint();
            backup1.Clear(new ByNumberSelection(1));

            Assert.False(_storageMethod.ExistsDirectory(rp1B1.FullPath));
            Assert.False(_storageMethod.ExistsDirectory(rp2B1.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp3B1.FullPath));
            Assert.AreEqual(1, backup1.RestorePoints.Count);
        }

        [Test]
        public void ClearTest_DateTimeSelection()
        {
            BackupJobExtra backup2 = _service.CreateBackup(
                _objects,
                new SplitStorages(),
                new ConsoleLogger()
            );

            DateTime date = DateTime.Now;
            RestorePoint rp1B2 = backup2.CreateRestorePoint(dateTime: date);
            RestorePoint rp2B2 = backup2.CreateRestorePoint(dateTime: date.AddDays(1));
            RestorePoint rp3B2 = backup2.CreateRestorePoint(dateTime: date.AddDays(1).AddMinutes(1));
            RestorePoint rp4B2 = backup2.CreateRestorePoint(dateTime: date.AddDays(2));
            backup2.Clear(new ByDateSelection(date.AddDays(1)));

            Assert.False(_storageMethod.ExistsDirectory(rp1B2.FullPath));
            Assert.False(_storageMethod.ExistsDirectory(rp2B2.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp3B2.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp4B2.FullPath));
            Assert.AreEqual(2, backup2.RestorePoints.Count);
        }

        [Test]
        public void ClearTest_HybridSelection()
        {
            BackupJobExtra backup3 = _service.CreateBackup(
                _objects,
                new SplitStorages(),
                new ConsoleLogger()
            );

            DateTime date = DateTime.Now;
            RestorePoint rp1B3 = backup3.CreateRestorePoint(dateTime: date);
            RestorePoint rp2B3 = backup3.CreateRestorePoint(dateTime: date.AddDays(1));
            RestorePoint rp3B3 = backup3.CreateRestorePoint(dateTime: date.AddDays(1).AddMinutes(1));
            RestorePoint rp4B3 = backup3.CreateRestorePoint(dateTime: date.AddDays(2));
            backup3.Clear(new HybridSelection(
                new List<ISelection>
                {
                    new ByDateSelection(date.AddDays(1)), // 2
                    new ByNumberSelection(3) // 3
                },
                true) // 3
            );
            
            Assert.False(_storageMethod.ExistsDirectory(rp1B3.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp2B3.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp3B3.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp4B3.FullPath));
            Assert.AreEqual(3, backup3.RestorePoints.Count);
            
            BackupJobExtra backup4 = _service.CreateBackup(
                _objects,
                new SplitStorages(),
                new ConsoleLogger()
            );

            RestorePoint rp1B4 = backup4.CreateRestorePoint(dateTime: date);
            RestorePoint rp2B4 = backup4.CreateRestorePoint(dateTime: date.AddDays(1));
            RestorePoint rp3B4 = backup4.CreateRestorePoint(dateTime: date.AddDays(1).AddMinutes(1));
            RestorePoint rp4B4 = backup4.CreateRestorePoint(dateTime: date.AddDays(2));
            backup4.Clear(new HybridSelection(
                new List<ISelection>
                {
                    new ByDateSelection(date.AddDays(1)), // 2
                    new ByNumberSelection(3) // 3
                },
                false) // 2
            );
            
            Assert.False(_storageMethod.ExistsDirectory(rp1B4.FullPath));
            Assert.False(_storageMethod.ExistsDirectory(rp2B4.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp3B4.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp4B4.FullPath));
            Assert.AreEqual(2, backup4.RestorePoints.Count);
        }

        [Test]
        public void MergeTest_NumberSelection()
        {
            BackupJobExtra backup1 = _service.CreateBackup(
                _objects,
                new SplitStorages(),
                new ConsoleLogger()
            );

            var d = new JobObject("/Users/artyomfadeyev/Documents/d.txt");
            RestorePoint rp1B1 = backup1.CreateRestorePoint(); // a b c
            backup1.AddJobObject(d);
            backup1.RemoveJobObject(_objects[0]);
            backup1.RemoveJobObject(_objects[1]);
            RestorePoint rp2B1 = backup1.CreateRestorePoint(); // c d
            backup1.RemoveJobObject(d);
            RestorePoint rp3B1 = backup1.CreateRestorePoint(); // c
            backup1.Merge(new ByNumberSelection(2));

            Assert.False(_storageMethod.ExistsDirectory(rp1B1.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp2B1.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp3B1.FullPath));
            Assert.AreEqual(2, backup1.RestorePoints.Count);
            Assert.AreEqual(4, backup1.Front().PublicStorages.Count); // a b c d

            backup1.Merge(new ByNumberSelection(1));

            Assert.False(_storageMethod.ExistsDirectory(rp2B1.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp3B1.FullPath));
            Assert.AreEqual(1, backup1.RestorePoints.Count);
            Assert.AreEqual(4, backup1.Front().PublicStorages.Count); // a b c d 
        }

        [Test]
        public void MergeTest_DateTimeSelection()
        {
            BackupJobExtra backup2 = _service.CreateBackup(
                _objects,
                new SplitStorages(),
                new ConsoleLogger()
            );

            DateTime date = DateTime.Now;
            var d = new JobObject("/Users/artyomfadeyev/Documents/d.txt");
            RestorePoint rp1B2 = backup2.CreateRestorePoint(dateTime: date); // a b c
            backup2.AddJobObject(d);
            backup2.RemoveJobObject(_objects[0]);
            backup2.RemoveJobObject(_objects[1]);
            RestorePoint rp2B2 = backup2.CreateRestorePoint(dateTime: date.AddDays(1).AddMinutes(1)); // c d
            backup2.RemoveJobObject(d);
            RestorePoint rp3B2 = backup2.CreateRestorePoint(dateTime: date.AddDays(2).AddMinutes(1)); // c
            backup2.Merge(new ByDateSelection(date.AddDays(1)));

            Assert.False(_storageMethod.ExistsDirectory(rp1B2.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp2B2.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp3B2.FullPath));
            Assert.AreEqual(2, backup2.RestorePoints.Count);
            Assert.AreEqual(4, backup2.Front().PublicStorages.Count); // a b c d

            backup2.Merge(new ByDateSelection(date.AddDays(2)));

            Assert.False(_storageMethod.ExistsDirectory(rp2B2.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp3B2.FullPath));
            Assert.AreEqual(1, backup2.RestorePoints.Count);
            Assert.AreEqual(4, backup2.Front().PublicStorages.Count); // a b c d 
        }

        [Test]
        public void MergeTest_HybridSelection()
        {
            BackupJobExtra backup3 = _service.CreateBackup(
                _objects,
                new SplitStorages(),
                new ConsoleLogger()
            );

            DateTime date = DateTime.Now;
            var d = new JobObject("/Users/artyomfadeyev/Documents/d.txt");
            RestorePoint rp1B3 = backup3.CreateRestorePoint(dateTime: date); // a b c
            backup3.AddJobObject(d);
            backup3.RemoveJobObject(_objects[0]);
            backup3.RemoveJobObject(_objects[1]);
            RestorePoint rp2B3 = backup3.CreateRestorePoint(dateTime: date.AddDays(1)); // c d
            backup3.RemoveJobObject(d);
            RestorePoint rp3B3 = backup3.CreateRestorePoint(dateTime: date.AddDays(1).AddMinutes(1)); // c
            backup3.AddJobObject(new JobObject("/Users/artyomfadeyev/Documents/e.txt"));
            backup3.RemoveJobObject(_objects[2]);
            RestorePoint rp4B3 = backup3.CreateRestorePoint(dateTime: date.AddDays(2)); // e
            backup3.Merge(new HybridSelection(
                new List<ISelection>
                {
                    new ByDateSelection(date.AddDays(1)), // 2
                    new ByNumberSelection(3) // 3
                },
                true) // 3
            );
            
            Assert.False(_storageMethod.ExistsDirectory(rp1B3.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp2B3.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp3B3.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp4B3.FullPath));
            Assert.AreEqual(3, backup3.RestorePoints.Count);
            Assert.AreEqual(4, backup3.Front().PublicStorages.Count); // a b c d 
            
            BackupJobExtra backup4 = _service.CreateBackup(
                _objects,
                new SplitStorages(),
                new ConsoleLogger()
            );
            
            RestorePoint rp1B4 = backup4.CreateRestorePoint(dateTime: date); // a b c
            backup4.AddJobObject(d);
            backup4.RemoveJobObject(_objects[0]);
            backup4.RemoveJobObject(_objects[1]);
            RestorePoint rp2B4 = backup4.CreateRestorePoint(dateTime: date.AddDays(1)); // c d
            backup4.RemoveJobObject(d);
            RestorePoint rp3B4 = backup4.CreateRestorePoint(dateTime: date.AddDays(1).AddMinutes(1)); // c
            backup4.AddJobObject(new JobObject("/Users/artyomfadeyev/Documents/e.txt"));
            backup4.RemoveJobObject(_objects[2]);
            RestorePoint rp4B4 = backup4.CreateRestorePoint(dateTime: date.AddDays(2)); // e
            backup4.Merge(new HybridSelection(
                new List<ISelection>
                {
                    new ByDateSelection(date.AddDays(1)), // 2
                    new ByNumberSelection(1) // 1
                },
                false) // 1
            );
            
            Assert.False(_storageMethod.ExistsDirectory(rp1B4.FullPath));
            Assert.False(_storageMethod.ExistsDirectory(rp2B4.FullPath));
            Assert.False(_storageMethod.ExistsDirectory(rp3B4.FullPath));
            Assert.True(_storageMethod.ExistsDirectory(rp4B4.FullPath));
            Assert.AreEqual(1, backup4.RestorePoints.Count);
            Assert.AreEqual(5, backup4.Front().PublicStorages.Count); // a b c d e
        }

        [Test]
        public void RestoreTest_OriginLocation()
        {
            BackupJobExtra backup1 = _service.CreateBackup(
                _objects,
                new SplitStorages(),
                new ConsoleLogger()
            );
            backup1.CreateRestorePoint();
            backup1.Recover(new OriginRecover(), backup1.Front());
            foreach (IJobObject obj in backup1.Front().PublicStorages
                .SelectMany(storage => storage.JobObjects))
            {
                Assert.True(_storageMethod.ExistsFile(obj.Path));
            }
        }

        [Test]
        public void RestoreTest_DifferentLocation()
        {
            BackupJobExtra backup1 = _service.CreateBackup(
                _objects,
                new SplitStorages(),
                new ConsoleLogger()
            );
            backup1.CreateRestorePoint();
            const string too = "kek";
            backup1.Recover(new DifferentLocationRecover(too), backup1.Front());
            foreach (IJobObject obj in backup1.Front().PublicStorages
                .SelectMany(storage => storage.JobObjects))
            {
                string name = Path.GetFileName(obj.Path);
                Assert.True(_storageMethod.ExistsFile(Path.Combine(too, name!)));
            }
        }
    }
}