using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo_API.Models.Abstracts;
using Todo_API.Models.Entities;

namespace Todo_API.Data;

public class TodoDbContext(DbContextOptions<TodoDbContext> options): IdentityDbContext<IdentityUser>(options: options)
{
    public DbSet<Todo> Todos { get; set; }


    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        
        UpdateTimedEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateTimedEntities();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateTimedEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override int SaveChanges()
    {
        UpdateTimedEntities();
        return base.SaveChanges();
    }

    private void UpdateTimedEntities()
    {
        foreach (var entityEntry in ChangeTracker.Entries().Where(x => x.Entity is ITimedEntity && (x.State == EntityState.Added || x.State == EntityState.Modified)))
        {
            var now = DateTime.UtcNow;
            if (entityEntry.State == EntityState.Added)
            {
                ((ITimedEntity)entityEntry.Entity).CreatedAt = now;
            }

            ((ITimedEntity)entityEntry.Entity).UpdatedAt = now;
        }
    }
}
