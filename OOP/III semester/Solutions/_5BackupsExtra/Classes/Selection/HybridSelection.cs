using System.Collections.Generic;
using System.Linq;
using Backups.Classes.RestorePoints;

namespace BackupsExtra.Classes.Selection
{
    public class HybridSelection : ISelection
    {
        private readonly List<ISelection> _selections;
        private readonly bool _unite;

        public HybridSelection(List<ISelection> selections, bool unite)
        {
            _selections = selections;
            _unite = unite;
        }

        public LinkedList<RestorePoint> Fetch(LinkedList<RestorePoint> restorePoints)
        {
            var answer = new HashSet<RestorePoint>();
            if (!_unite)
            {
                answer = new HashSet<RestorePoint>(restorePoints);
            }

            foreach (IEnumerable<RestorePoint> enumerable in _selections
                .Select(selection => selection.Fetch(restorePoints)))
            {
                var result = enumerable.ToHashSet();
                switch (_unite)
                {
                    case true:
                        answer.UnionWith(result);
                        break;
                    case false:
                        answer.IntersectWith(result);
                        break;
                }
            }

            // to recover restore points order
            var ans = new LinkedList<RestorePoint>();
            foreach (RestorePoint rp in restorePoints
                .Where(rp => answer.Contains(rp)))
            {
                ans.AddLast(rp);
            }

            return ans;
        }
    }
}