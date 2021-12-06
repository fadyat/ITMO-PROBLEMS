using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Classes;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Backups.Classes.Storages;
using Backups.Services;
using NUnit.Framework;

namespace Backups.Tests
{
    [TestFixture]
    public class BackupsTest
    {
        private BackupJobService _backupJobService;
        private string _position;
        private IStorageMethod _fileSystemStorage;
        private HashSet<IJobObject> _standardFiles;

        [SetUp]
        public void SetUp()
        {
            _position = Directory.GetParent(Directory.GetCurrentDirectory())?
                .Parent?
                .Parent?
                .FullName;

            _fileSystemStorage = new AbstractFileSystemStorage();
            _backupJobService = new BackupJobService(
                _position,
                _fileSystemStorage,
                "BackupsTest");

            _standardFiles = new HashSet<IJobObject>
            {
                new JobObject("/Users/artyomfadeyev/Documents/a.txt"),
                new JobObject("/Users/artyomfadeyev/Documents/b.txt"),
                new JobObject("/Users/artyomfadeyev/Documents/c.txt")
            };
        }

        [Test]
        public void BackupCreationSplitStorages()
        {
            var backupJob1 = new BackupJob(
                _backupJobService.Path,
                _standardFiles,
                new SplitStorages(),
                _backupJobService.StorageMethod);

            _backupJobService.CreateBackup(backupJob1);

            Assert.True(_fileSystemStorage.ExistsDirectory(backupJob1.FullPath));
            Assert.AreEqual(_standardFiles.Count, backupJob1.Objects.ToList().Count);
        }

        [Test]
        public void BackupCreationSingleStorage()
        {
            var backupJob2 = new BackupJob(
                _backupJobService.Path,
                _standardFiles,
                new SingleStorage(),
                _backupJobService.StorageMethod);

            _backupJobService.CreateBackup(backupJob2);

            Assert.True(_fileSystemStorage.ExistsDirectory(backupJob2.FullPath));
            Assert.AreEqual(_standardFiles.Count, backupJob2.Objects.ToList().Count);
        }

        [Test]
        public void RestorePointCreationSplitStorages()
        {
            var backupJob1 = new BackupJob(
                _backupJobService.Path,
                _standardFiles,
                new SplitStorages(),
                _backupJobService.StorageMethod);

            var b1P1 = new RestorePoint(
                backupJob1.Path,
                backupJob1.Objects,
                backupJob1.StorageAlgorithm,
                _backupJobService.StorageMethod);

            backupJob1.CreateRestorePoint(b1P1);
            Assert.True(_fileSystemStorage.ExistsDirectory(b1P1.FullPath));
            Assert.AreEqual(backupJob1.Objects.ToList().Count, b1P1.StoragesI.ToList().Count);
            foreach (Storage storage in b1P1.StoragesI)
            {
                Assert.True(_fileSystemStorage.ExistsFile(storage.FullPath));
            }
        }

        [Test]
        public void RestorePointCreationSingleStorage()
        {
            var backupJob2 = new BackupJob(
                _backupJobService.Path,
                _standardFiles,
                new SingleStorage(),
                _backupJobService.StorageMethod);

            var b2P1 = new RestorePoint(
                backupJob2.Path,
                backupJob2.Objects,
                backupJob2.StorageAlgorithm,
                _backupJobService.StorageMethod);

            backupJob2.CreateRestorePoint(b2P1);

            Assert.True(_fileSystemStorage.ExistsDirectory(b2P1.FullPath));
            Assert.AreEqual(1, b2P1.StoragesI.ToList().Count);
            foreach (Storage storage in b2P1.StoragesI)
            {
                Assert.True(_fileSystemStorage.ExistsFile(storage.FullPath));
            }
        }

        [Test]
        public void BackupOperationsSplitStorages()
        {
            var backupJob1 = new BackupJob(
                _backupJobService.Path,
                _standardFiles,
                new SplitStorages(),
                _backupJobService.StorageMethod);

            var b1P1 = new RestorePoint(
                backupJob1.Path,
                backupJob1.Objects,
                backupJob1.StorageAlgorithm,
                _backupJobService.StorageMethod);

            backupJob1.CreateRestorePoint(b1P1);
            Assert.True(_fileSystemStorage.ExistsDirectory(b1P1.FullPath));

            IJobObject toRemove = backupJob1.Objects.First();
            backupJob1.RemoveJobObject(toRemove);
            Assert.AreEqual(_standardFiles.Count, backupJob1.Objects.ToList().Count);
            Assert.AreEqual(2, backupJob1.Objects.ToList().Count);

            var b1P2 = new RestorePoint(
                backupJob1.Path,
                backupJob1.Objects,
                backupJob1.StorageAlgorithm,
                _backupJobService.StorageMethod);

            backupJob1.CreateRestorePoint(b1P2);
            Assert.True(_fileSystemStorage.ExistsDirectory(b1P2.FullPath));
            Assert.AreEqual(backupJob1.Objects.ToList().Count, b1P2.StoragesI.ToList().Count);

            foreach (Storage storage in b1P2.StoragesI)
            {
                Assert.True(_fileSystemStorage.ExistsFile(storage.FullPath));
            }

            backupJob1.AddJobObject(toRemove);
            Assert.AreEqual(_standardFiles.Count, backupJob1.Objects.ToList().Count);

            var b1P3 = new RestorePoint(
                backupJob1.Path,
                backupJob1.Objects,
                backupJob1.StorageAlgorithm,
                _backupJobService.StorageMethod);

            backupJob1.CreateRestorePoint(b1P3);
            Assert.True(_fileSystemStorage.ExistsDirectory(b1P3.FullPath));
            Assert.AreEqual(backupJob1.Objects.ToList().Count, b1P3.StoragesI.ToList().Count);

            foreach (Storage storage in b1P3.StoragesI)
            {
                Assert.True(_fileSystemStorage.ExistsFile(storage.FullPath));
            }
        }

        [Test]
        public void BackupOperationsSingleStorage()
        {
            var backupJob1 = new BackupJob(
                _backupJobService.Path,
                _standardFiles,
                new SingleStorage(),
                _backupJobService.StorageMethod);

            var b2P1 = new RestorePoint(
                backupJob1.Path,
                backupJob1.Objects,
                backupJob1.StorageAlgorithm,
                _backupJobService.StorageMethod);

            backupJob1.CreateRestorePoint(b2P1);
            Assert.True(_fileSystemStorage.ExistsDirectory(b2P1.FullPath));

            IJobObject toRemove = backupJob1.Objects.First();
            backupJob1.RemoveJobObject(toRemove);
            Assert.AreEqual(_standardFiles.Count, backupJob1.Objects.ToList().Count);
            Assert.AreEqual(2, backupJob1.Objects.ToList().Count);

            var b2P2 = new RestorePoint(
                backupJob1.Path,
                backupJob1.Objects,
                backupJob1.StorageAlgorithm,
                _backupJobService.StorageMethod);

            backupJob1.CreateRestorePoint(b2P2);
            Assert.True(_fileSystemStorage.ExistsDirectory(b2P2.FullPath));
            Assert.AreEqual(1, b2P2.StoragesI.ToList().Count);

            foreach (Storage storage in b2P2.StoragesI)
            {
                Assert.True(_fileSystemStorage.ExistsFile(storage.FullPath));
            }

            backupJob1.AddJobObject(toRemove);
            Assert.AreEqual(_standardFiles.Count, backupJob1.Objects.ToList().Count);

            var b2P3 = new RestorePoint(
                backupJob1.Path,
                backupJob1.Objects,
                backupJob1.StorageAlgorithm,
                _backupJobService.StorageMethod);

            backupJob1.CreateRestorePoint(b2P3);
            Assert.True(_fileSystemStorage.ExistsDirectory(b2P3.FullPath));
            Assert.AreEqual(1, b2P3.StoragesI.ToList().Count);

            foreach (Storage storage in b2P3.StoragesI)
            {
                Assert.True(_fileSystemStorage.ExistsFile(storage.FullPath));
            }
        }
    }
}