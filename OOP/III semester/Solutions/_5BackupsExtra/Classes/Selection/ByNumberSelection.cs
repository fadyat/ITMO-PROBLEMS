using System.Collections.Generic;
using Backups.Classes.RestorePoints;

namespace BackupsExtra.Classes.Selection
{
    public class ByNumberSelection : ISelection
    {
        private readonly int _numberToSave;

        public ByNumberSelection(int numberToSave)
        {
            _numberToSave = numberToSave;
        }

        public IEnumerable<IRestorePoint> Clear(LinkedList<IRestorePoint> restorePoints)
        {
            int toSave = _numberToSave;
            while (toSave > 0)
            {
                restorePoints.RemoveFirst();
                toSave--;
            }

            return restorePoints;
        }
    }
}