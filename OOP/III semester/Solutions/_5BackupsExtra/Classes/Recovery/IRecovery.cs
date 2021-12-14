using Backups.Classes.RestorePoints;
using BackupsExtra.Classes.StorageMethodsExtra;

namespace BackupsExtra.Classes.Recovery
{
    public interface IRecovery
    {
        void Restore(IStorageExtraMethod storageExtraMethod, RestorePoint restorePoint);
    }
}