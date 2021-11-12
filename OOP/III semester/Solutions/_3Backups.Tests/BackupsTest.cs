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

        [SetUp]
        public void SetUp()
        {
            _position = Directory.GetParent(Directory.GetCurrentDirectory())
                .Parent?
                .Parent?
                .FullName;

            _fileSystemStorage = new FileSystemStorage();
            _backupJobService = new BackupJobService(
                _position,
                _fileSystemStorage,
                "Test");
        }

        [Test]
        public void BackupCreation()
        {
            BackupJob backupJob1 = _backupJobService.CreateBackup(
                new HashSet<string>
                {
                    "/Users/artyomfadeyev/Documents/a.txt",
                    "/Users/artyomfadeyev/Documents/b.txt",
                    "/Users/artyomfadeyev/Documents/c.txt",
                },
                new SplitStorages(),
                "BackupCreation_backupJob1");

            BackupJob backupJob2 = _backupJobService.CreateBackup(
                new HashSet<string>
                {
                    "/Users/artyomfadeyev/Documents/a.txt"
                },
                new SingleStorage(),
                "BackupCreation_backupJob2");
            
            Assert.True(_fileSystemStorage.ExistsDirectory(backupJob1.Path));
            Assert.AreEqual(3, backupJob1.FilePaths.ToList().Count);
            
            Assert.True(_fileSystemStorage.ExistsDirectory(backupJob2.Path));
            Assert.AreEqual(1, backupJob2.FilePaths.ToList().Count);
        }
    }
}