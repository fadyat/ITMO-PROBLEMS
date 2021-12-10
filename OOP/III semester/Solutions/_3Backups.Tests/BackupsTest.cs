using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            IBackupJob backupJob1 = _backupJobService.CreateBackup(_standardFiles, new SplitStorages());

            Assert.True(_fileSystemStorage.ExistsDirectory(backupJob1.FullPath));
            Assert.AreEqual(_standardFiles.Count, backupJob1.Objects.ToList().Count);
        }

        [Test]
        public void BackupCreationSingleStorage()
        {
            IBackupJob backupJob2 = _backupJobService.CreateBackup(_standardFiles, new SingleStorage());

            Assert.True(_fileSystemStorage.ExistsDirectory(backupJob2.FullPath));
            Assert.AreEqual(_standardFiles.Count, backupJob2.Objects.ToList().Count);
        }

        [Test]
        public void RestorePointCreationSplitStorages()
        {
            IBackupJob backupJob1 = _backupJobService.CreateBackup(_standardFiles, new SplitStorages());
            RestorePoint b1P1 = backupJob1.CreateRestorePoint();
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
            IBackupJob backupJob2 = _backupJobService.CreateBackup(_standardFiles, new SingleStorage());
            RestorePoint b2P1 = backupJob2.CreateRestorePoint();

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
            IBackupJob backupJob1 = _backupJobService.CreateBackup(_standardFiles, new SplitStorages());
            RestorePoint b1P1 = backupJob1.CreateRestorePoint();
            Assert.True(_fileSystemStorage.ExistsDirectory(b1P1.FullPath));

            IJobObject toRemove = backupJob1.Objects.First();
            backupJob1.RemoveJobObject(toRemove);
            Assert.AreEqual(_standardFiles.Count - 1, backupJob1.Objects.ToList().Count);

            RestorePoint b1P2 = backupJob1.CreateRestorePoint();
            Assert.True(_fileSystemStorage.ExistsDirectory(b1P2.FullPath));
            Assert.AreEqual(backupJob1.Objects.ToList().Count, b1P2.StoragesI.ToList().Count);

            foreach (Storage storage in b1P2.StoragesI)
            {
                Assert.True(_fileSystemStorage.ExistsFile(storage.FullPath));
            }

            backupJob1.AddJobObject(toRemove);
            Assert.AreEqual(_standardFiles.Count, backupJob1.Objects.ToList().Count);

            RestorePoint b1P3 = backupJob1.CreateRestorePoint();
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
            IBackupJob backupJob2 = _backupJobService.CreateBackup(_standardFiles, new SingleStorage());
            RestorePoint b2P1 = backupJob2.CreateRestorePoint();
            Assert.True(_fileSystemStorage.ExistsDirectory(b2P1.FullPath));

            IJobObject toRemove = backupJob2.Objects.First();
            backupJob2.RemoveJobObject(toRemove);
            Assert.AreEqual(_standardFiles.Count - 1, backupJob2.Objects.ToList().Count);

            RestorePoint b2P2 = backupJob2.CreateRestorePoint();
            Assert.True(_fileSystemStorage.ExistsDirectory(b2P2.FullPath));
            Assert.AreEqual(1, b2P2.StoragesI.ToList().Count);

            foreach (Storage storage in b2P2.StoragesI)
            {
                Assert.True(_fileSystemStorage.ExistsFile(storage.FullPath));
            }

            backupJob2.AddJobObject(toRemove);
            Assert.AreEqual(_standardFiles.Count, backupJob2.Objects.ToList().Count);

            RestorePoint b2P3 = backupJob2.CreateRestorePoint();
            Assert.True(_fileSystemStorage.ExistsDirectory(b2P3.FullPath));
            Assert.AreEqual(1, b2P3.StoragesI.ToList().Count);

            foreach (Storage storage in b2P3.StoragesI)
            {
                Assert.True(_fileSystemStorage.ExistsFile(storage.FullPath));
            }
        }
    }
}