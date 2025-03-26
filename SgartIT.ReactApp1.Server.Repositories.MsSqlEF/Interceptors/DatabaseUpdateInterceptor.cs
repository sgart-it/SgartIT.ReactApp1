using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SgartIT.ReactApp1.Server.DTO.Entities;

namespace SgartIT.ReactApp1.Server.Repositories.MsSqlEf.Interceptors;

public class DatabaseUpdateInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        // Do something before saving changes
        if (eventData.Context is not null)
        {
            var dbContext = eventData.Context;
            foreach (var entry in dbContext.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                // mi assicuro che le date di creazione e modifica siano sempre aggiornate
                if (entry.Entity is TodoEntity todo)
                {
                    todo.ModifyDate = DateTime.UtcNow;
                    if (entry.State == EntityState.Added)
                    {
                        todo.CreationDate = todo.ModifyDate;
                    }
                }
            }
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    //public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    //{
    //    // Do something after saving changes
    //    return base.SavedChangesAsync(eventData, result, cancellationToken);
    //}

}
