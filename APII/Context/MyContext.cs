using APII.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options)
    {
    }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Department>()
            .HasMany(e => e.employees)
            .WithOne(e => e.department)
            .HasForeignKey(e => e.Department_id)
            .IsRequired();
    }
}


