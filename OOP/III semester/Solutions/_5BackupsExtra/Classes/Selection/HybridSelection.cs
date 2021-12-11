using System.Collections.Generic;
using System.Linq;
using Backups.Classes.RestorePoints;

namespace BackupsExtra.Classes.Selection
{
    public class HybridSelection : ISelection
    {
        private readonly List<ISelection> _clearAlgorithms;
        private readonly bool _unite;

        public HybridSelection(List<ISelection> clearAlgorithms, bool unite)
        {
            _clearAlgorithms = clearAlgorithms;
            _unite = unite;
        }

        public IEnumerable<RestorePoint> Fetch(LinkedList<RestorePoint> restorePoints)
        {
            var answer = new HashSet<RestorePoint>(restorePoints);
            foreach (IEnumerable<RestorePoint> result in _clearAlgorithms
                .Select(clearAlgo => clearAlgo.Fetch(restorePoints)))
            {
                switch (_unite)
                {
                    case true:
                        answer.IntersectWith(result);
                        break;
                    default:
                        answer.UnionWith(result);
                        break;
                }
            }

            return answer;
        }
    }
}