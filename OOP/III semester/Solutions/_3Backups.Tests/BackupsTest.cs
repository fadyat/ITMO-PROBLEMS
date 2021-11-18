using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Classes;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
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
        private List<JobObject> _standardFiles;

        [SetUp]
        public void SetUp()
        {
            _position = Directory.GetParent(Directory.GetCurrentDirectory())
                .Parent?
                .Parent?
                .FullName;

            _fileSystemStorage = new AbstractFileSystemStorage(); // IStorageMethod 
            _backupJobService = new BackupJobService(
                _position,
                _fileSystemStorage,
                "BackupsTest");

            _standardFiles = new List<JobObject>
            {
                new JobObject("/Users/artyomfadeyev/Documents/a.txt"),
                new JobObject("/Users/artyomfadeyev/Documents/b.txt"),
                new JobObject("/Users/artyomfadeyev/Documents/c.txt")
            };
        }

        [Test]
        public void BackupCreationSplitStorages()
        {
            BackupJob backupJob1 = _backupJobService.CreateBackup(
                new HashSet<JobObject>(_standardFiles),
                new SplitStorages(),
                "BackupCreationSplitStorages");
            
            Assert.True(_fileSystemStorage.ExistsDirectory(backupJob1.Path));
            Assert.AreEqual(_standardFiles.Count, backupJob1.Objects.ToList().Count);
        }

        [Test]
        public void BackupCreationSingleStorage()
        {
            BackupJob backupJob2 = _backupJobService.CreateBackup(
                new HashSet<JobObject>(_standardFiles),
                new SingleStorage(),
                "BackupCreationSingleStorage");

            Assert.True(_fileSystemStorage.ExistsDirectory(backupJob2.Path));
            Assert.AreEqual(_standardFiles.Count, backupJob2.Objects.ToList().Count);
        }

        [Test]
        public void RestorePointCreationSplitStorages()
        {
            BackupJob backupJob1 = _backupJobService.CreateBackup(
                new HashSet<JobObject>(_standardFiles),
                new SplitStorages(),
                "RestorePointCreationSplitStorages");

            RestorePoint b1P1 = backupJob1.CreateRestorePoint();
            Assert.True(_fileSystemStorage.ExistsDirectory(b1P1.Path));
            Assert.AreEqual(
                backupJob1.Objects.ToList().Count,
                b1P1.Storages.ToList().Count);
            foreach (Storage storage in b1P1.Storages)
            {
                Assert.True(_fileSystemStorage.ExistsFile(storage.Path));
            }
        }

        [Test]
        public void RestorePointCreationSingleStorage()
        {
            BackupJob backupJob2 = _backupJobService.CreateBackup(
                new HashSet<JobObject>(_standardFiles),
                new SingleStorage(),
                "RestorePointCreationSingleStorage");

            RestorePoint b2P1 = backupJob2.CreateRestorePoint();
            Assert.True(_fileSystemStorage.ExistsDirectory(b2P1.Path));
            Assert.AreEqual(1, b2P1.Storages.ToList().Count);
            foreach (Storage storage in b2P1.Storages)
            {
                Assert.True(_fileSystemStorage.ExistsFile(storage.Path));
            }
        }

        [Test]
        public void BackupOperationsSplitStorages()
        {
            BackupJob backupJob1 = _backupJobService.CreateBackup(
                new HashSet<JobObject>(_standardFiles),
                new SplitStorages(),
                "BackupOperationsSplitStorages");

            backupJob1.CreateRestorePoint();
            JobObject toRemove = _standardFiles.First();
            backupJob1.RemoveJobObject(toRemove);
            Assert.AreEqual(
                _standardFiles.Count - 1,
                backupJob1.Objects.ToList().Count);

            RestorePoint b1P2 = backupJob1.CreateRestorePoint();
            Assert.True(_fileSystemStorage.ExistsDirectory(b1P2.Path));
            Assert.AreEqual(
                backupJob1.Objects.ToList().Count,
                b1P2.Storages.ToList().Count);
            foreach (Storage storage in b1P2.Storages)
            {
                Assert.True(_fileSystemStorage.ExistsFile(storage.Path));
            }

            backupJob1.AddJobObject(toRemove);
            Assert.AreEqual(
                _standardFiles.Count,
                backupJob1.Objects.ToList().Count);

            RestorePoint b1P3 = backupJob1.CreateRestorePoint();
            Assert.True(_fileSystemStorage.ExistsDirectory(b1P3.Path));
            Assert.AreEqual(
                backupJob1.Objects.ToList().Count,
                b1P3.Storages.ToList().Count);
            foreach (Storage storage in b1P3.Storages)
            {
                Assert.True(_fileSystemStorage.ExistsFile(storage.Path));
            }
        }

        [Test]
        public void BackupOperationsSingleStorage()
        {
            BackupJob backupJob2 = _backupJobService.CreateBackup(
                new HashSet<JobObject>(_standardFiles),
                new SingleStorage(),
                "BackupOperationsSingleStorage");

            backupJob2.CreateRestorePoint();
            JobObject toRemove = _standardFiles.First();
            backupJob2.RemoveJobObject(toRemove);
            Assert.AreEqual(
                _standardFiles.Count - 1,
                backupJob2.Objects.ToList().Count);

            RestorePoint b2P2 = backupJob2.CreateRestorePoint();
            Assert.True(_fileSystemStorage.ExistsDirectory(b2P2.Path));
            Assert.AreEqual(1, b2P2.Storages.ToList().Count);
            foreach (Storage storage in b2P2.Storages)
            {
                Assert.True(_fileSystemStorage.ExistsFile(storage.Path));
            }

            backupJob2.AddJobObject(toRemove);
            Assert.AreEqual(
                _standardFiles.Count,
                backupJob2.Objects.ToList().Count);

            RestorePoint b2P3 = backupJob2.CreateRestorePoint();
            Assert.True(_fileSystemStorage.ExistsDirectory(b2P3.Path));
            Assert.AreEqual(1, b2P3.Storages.ToList().Count);
            foreach (Storage storage in b2P3.Storages)
            {
                Assert.True(_fileSystemStorage.ExistsFile(storage.Path));
            }
        }
    }
}