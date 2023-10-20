using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Backups.Classes.JobObjects;
using Newtonsoft.Json;

namespace Backups.Classes.Storages
{
    public class Storage
    {
        public Storage(string name, string path, IEnumerable<IJobObject> objects)
        {
            Objects = objects.ToHashSet();
            Name = name;
            Path = path;
            FullPath = System.IO.Path.Combine(path, Name);
        }

        public string Name { get; }

        public string Path { get; }

        [JsonIgnore]
        public string FullPath { get; }

        [JsonIgnore]
        public ImmutableList<IJobObject> JobObjects => Objects.ToImmutableList();

        [JsonProperty]
        private HashSet<IJobObject> Objects { get; }
    }
}