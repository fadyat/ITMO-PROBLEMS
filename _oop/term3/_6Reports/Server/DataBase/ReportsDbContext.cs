using lab_6.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace lab_6.Server.DataBase
{
    public class ReportsDbContext : DbContext
    {
        public ReportsDbContext(DbContextOptions<ReportsDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}