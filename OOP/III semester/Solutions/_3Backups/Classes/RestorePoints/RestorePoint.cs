using System;
using System.Collections.Generic;
using Backups.Classes.Storages;
using Newtonsoft.Json;

namespace Backups.Classes.RestorePoints
{
    public class RestorePoint
    {
        public RestorePoint(string path, LinkedList<Storage> storages, string name, DateTime dateTime)
        {
            Storages = storages;
            Path = path;
            Name = name;
            CreationDate = dateTime;
            FullPath = System.IO.Path.Combine(Path, Name);
        }

        public string Name { get; }

        public string Path { get; }

        [JsonIgnore]
        public string FullPath { get; }

        public DateTime CreationDate { get; }

        public IEnumerable<Storage> StoragesI => Storages;

        protected LinkedList<Storage> Storages { get; }
    }
}