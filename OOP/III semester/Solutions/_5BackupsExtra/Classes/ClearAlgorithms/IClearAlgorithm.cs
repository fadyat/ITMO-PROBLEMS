using System.Collections.Generic;
using Backups.Classes;

namespace BackupsExtra.Classes.ClearAlgorithms
{
    public interface IClearAlgorithm
    {
        IEnumerable<RestorePoint> Clear(LinkedList<RestorePoint> restorePoints);
    }
}