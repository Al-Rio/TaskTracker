using Microsoft.EntityFrameworkCore;
public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
    {
    }
    // The Assignments table
    public DbSet<Assignment> Assignments { get; set; }
}