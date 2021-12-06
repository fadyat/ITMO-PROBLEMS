using Backups.Classes.StorageMethods;

namespace Backups.Classes.Storages
{
    public class Storage
    {
        public Storage(string storageName, string path, IStorageMethod storageMethod)
        {
            Name = storageName + ".zip";
            StorageMethod = storageMethod;
            Path = path;
            FullPath = StorageMethod.ConstructPath(path, Name);
        }

        public string Name { get; }

        public string Path { get; }

        public string FullPath { get; }

        public IStorageMethod StorageMethod { get; }
    }
}