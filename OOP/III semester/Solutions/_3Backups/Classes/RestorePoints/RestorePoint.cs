using System;
using System.Collections.Generic;
using Backups.Classes.Storages;

namespace Backups.Classes.RestorePoints
{
    public class RestorePoint : IRestorePoint
    {
        public RestorePoint(
            string path,
            string name = null,
            DateTime dateTime = default)
        {
            Storages = new List<Storage>();
            Path = path;
            Name = name ?? Guid.NewGuid().ToString();
            CreationDate = dateTime;
            FullPath = System.IO.Path.Combine(Path, Name);
        }

        public string Name { get; }

        public string Path { get; }

        public string FullPath { get; }

        public DateTime CreationDate { get; }

        public IEnumerable<Storage> StoragesI => Storages;

        private List<Storage> Storages { get; set; }

        public void AddStorages(List<Storage> storages)
        {
            Storages = storages;
        }

        public void Print()
        {
            Console.Write(Name + " ");
        }
    }
}