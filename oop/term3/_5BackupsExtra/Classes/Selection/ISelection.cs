using System.Collections.Generic;
using Backups.Classes.RestorePoints;

namespace BackupsExtra.Classes.Selection
{
    public interface ISelection
    {
        LinkedList<RestorePoint> Fetch(LinkedList<RestorePoint> restorePoints);
    }
}