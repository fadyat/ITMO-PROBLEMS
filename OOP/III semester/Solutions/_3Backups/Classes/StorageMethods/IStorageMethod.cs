using Backups.Classes.Storages;

namespace Backups.Classes.StorageMethods
{
    public interface IStorageMethod
    {
        string ConstructPath(string path, string name);

        void MakeDirectory(string path);

        void Archive(Storage from);

        bool ExistsDirectory(string path);

        bool ExistsFile(string path);
    }
}