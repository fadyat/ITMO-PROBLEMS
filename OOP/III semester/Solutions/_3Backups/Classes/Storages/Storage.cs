using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Backups.Classes.JobObjects;
using Newtonsoft.Json;

namespace Backups.Classes.Storages
{
    public class Storage
    {
        public Storage(string storageName, string path, IEnumerable<IJobObject> jobObjects)
        {
            Objects = jobObjects.ToHashSet();
            Name = storageName;
            Path = path;
            FullPath = System.IO.Path.Combine(path, Name);
        }

        public string Name { get; }

        public string Path { get; }

        [JsonIgnore]
        public string FullPath { get; }

        public ImmutableList<IJobObject> JobObjects => Objects.ToImmutableList();

        private HashSet<IJobObject> Objects { get; }
    }
}