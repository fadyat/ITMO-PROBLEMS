using System;
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

        public LinkedList<RestorePoint> Fetch(LinkedList<RestorePoint> restorePoints)
        {
            var result = new LinkedList<RestorePoint>(restorePoints);
            int toRemove = Math.Max(result.Count - _numberToSave, 0);
            while (toRemove > 0)
            {
                result.RemoveFirst();
                toRemove--;
            }

            return result;
        }
    }
}