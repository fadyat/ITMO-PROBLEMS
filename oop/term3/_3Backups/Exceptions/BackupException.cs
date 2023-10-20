using System;

namespace Backups.Exceptions
{
    public class BackupException : Exception
    {
        public BackupException(string message)
            : base(message) { }
    }
}