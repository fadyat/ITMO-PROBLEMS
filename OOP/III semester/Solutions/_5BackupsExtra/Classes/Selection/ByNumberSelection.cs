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

        public IEnumerable<RestorePoint> Fetch(LinkedList<RestorePoint> restorePoints)
        {
            var result = new LinkedList<RestorePoint>(restorePoints);
            int toSave = _numberToSave;
            while (toSave > 0)
            {
                result.RemoveFirst();
                toSave--;
            }

            return result;
        }
    }
}