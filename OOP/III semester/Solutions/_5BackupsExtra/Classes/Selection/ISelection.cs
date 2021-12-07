using System.Collections.Generic;
using Backups.Classes.RestorePoints;

namespace BackupsExtra.Classes.Selection
{
    public interface ISelection
    {
        IEnumerable<IRestorePoint> Fetch(LinkedList<IRestorePoint> restorePoints);
    }
}