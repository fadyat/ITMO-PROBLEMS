namespace Backups.Classes.JobObjects
{
    public class JobObject : IJobObject
    {
        public JobObject(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}