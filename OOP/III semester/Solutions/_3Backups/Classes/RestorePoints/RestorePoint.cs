using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Backups.Classes.Storages;
using Newtonsoft.Json;

namespace Backups.Classes.RestorePoints
{
    public class RestorePoint
    {
        public RestorePoint(string path, LinkedList<Storage> storages, string name, DateTime creationDate)
        {
            Storages = storages;
            Path = path;
            Name = name;
            CreationDate = creationDate;
            FullPath = System.IO.Path.Combine(Path, Name);
        }

        public string Name { get; }

        public string Path { get; }

        [JsonIgnore]
        public string FullPath { get; }

        public DateTime CreationDate { get; }

        [JsonIgnore]
        public ImmutableList<Storage> PublicStorages => Storages.ToImmutableList();

        [JsonProperty]
        private LinkedList<Storage> Storages { get; }
    }
}