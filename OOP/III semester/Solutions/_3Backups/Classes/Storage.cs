using Backups.Classes.StorageMethods;

namespace Backups.Classes
{
    public class Storage
    {
        public Storage(string storageName, string path, IStorageMethod storageMethod)
        {
            Name = storageName + ".zip";
            StorageMethod = storageMethod;
            Path = StorageMethod.ConstructPath(path, Name);
        }

        public string Name { get; }

        public string Path { get; }

        public IStorageMethod StorageMethod { get; }
    }
}