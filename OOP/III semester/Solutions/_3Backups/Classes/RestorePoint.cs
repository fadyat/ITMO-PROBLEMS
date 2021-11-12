using System.Collections.Generic;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;

namespace Backups.Classes
{
    public class RestorePoint
    {
        private readonly List<Storage> _storages;

        public RestorePoint(
            int id,
            BackupJob backupJob,
            IStorageAlgorithm storageAlgorithm,
            IStorageMethod storageMethod,
            string name)
        {
            Id = id;
            name += name.EndsWith("_") ? Id.ToString() : string.Empty;
            Path = storageMethod.ConstructPath(backupJob.Path, name);
            storageMethod.MakeDirectory(Path);

            _storages = storageAlgorithm
                .Compression(Path, backupJob.FilePaths, storageMethod);
        }

        public int Id { get; }

        public string Path { get; }

        public IEnumerable<Storage> Storages => _storages;
    }
}