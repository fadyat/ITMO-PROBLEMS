using System.IO;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Backups.Classes.Storages;
using BackupsExtra.Classes.StorageMethodsExtra;

namespace BackupsExtra.Classes.Recovery
{
    public class DifferentLocationRecover
    {
        private readonly string _toDirectory;

        public DifferentLocationRecover(string toDirectory)
        {
            _toDirectory = toDirectory;
        }

        public void Restore(IStorageExtraMethod storageExtraMethod, RestorePoint restorePoint)
        {
            if (!storageExtraMethod.ExistsDirectory(_toDirectory))
            {
                storageExtraMethod.MakeDirectory(_toDirectory);
            }

            foreach (Storage storage in restorePoint.PublicStorages)
            {
                storageExtraMethod.Recover(storage.FullPath, _toDirectory);
            }
        }
    }
}