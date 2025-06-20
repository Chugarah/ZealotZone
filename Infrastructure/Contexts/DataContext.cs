using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

/// <summary>
/// Inspired by Hans :) Thanks
/// </summary>
/// <param name="options"></param>
public class DataContext(DbContextOptions<DataContext> options)
    : IdentityDbContext<UserEntity>(options)
{
    /*
        # Navigate to your Infrastructure project directory
        cd Infrastructure

        # Add migration
        dotnet ef migrations add InitialMigration

        # Update database
        dotnet ef database update
     */

    // Hans Recommended this when we want to use Lazyloading public virtual...

    // This will be added as last after Many-to-many relationships has been created
    // for sample Project and Services,

    /// <summary>
    /// Configures the database options for the current DbContext instance.
    /// This method is used to set provider-specific options, enable or disable features
    /// like lazy loading, caching, and to configure sensitive data logging for developer purposes.
    /// </summary>
    /// <param name="optionsBuilder">A builder used to configure options for the DbContext.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Using Logs this to log sensitive data for developers
        optionsBuilder.EnableSensitiveDataLogging();
        // We can also enable caching here
        optionsBuilder.EnableServiceProviderCaching();

        // Enable lazy loading
        // Microsoft.EntityFrameworkCore.Proxies
        optionsBuilder.UseLazyLoadingProxies();
    }

    /// <summary>
    /// This is needed if we have multiple keys in a table to add the composite key (Two Primary Keys)
    /// Then this is needed to be added to the OnModelCreating. What should I do without Hans? :)
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // modelBuilder.Entity<TableName>().HasKey(x => new { x.Key1, x.Key2 });
        // If we want to make a column unique but could also be set in our Entity as well and not just here
        // modelBuilder.Entity<TableName>().HasIndex(x => x.Key1).IsUnique();

        // Composite Key for Customers and a unique constraint for Name and ContactPersonId
        /*
        modelBuilder.Entity<CustomersEntity>(entity =>
        {
            // Composite Key for Customers ID
            entity.HasKey(e => new { e.Id });
            // Adding a unique constraint to the Name and ContactPersonId
            entity
                .HasIndex(e => new { e.Name, e.ContactPersonId })
                .IsUnique()
                .HasDatabaseName("IX_Customers_Name_ContactPersonId");
        });
        */

        // Configure the many-to-many relationship between Users and Roles
        // This is a many-to-many relationship between Users and Roles
        // Code and comments generated by AI Phind 100%
        // https://stackoverflow.com/questions/32655959/entityframework-codefirst-cascade-delete-for-same-table-many-to-many-relationsh
        //

        // Configure the many-to-many relationship between Projects and Services
        // This is a many-to-many relationship between Project and Services
        // Code and comments generated by AI Phind 100%
        /*
        modelBuilder
            .Entity<ProjectsEntity>()
            .HasMany(p => p.ServiceContractsEntity)
            .WithMany(s => s.ProjectsEntity)
            .UsingEntity<Dictionary<string, object>>(
                "ProjectServiceContracts",
                j =>
                    j.HasOne<ServiceContractsEntity>().WithMany().OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne<ProjectsEntity>().WithMany().OnDelete(DeleteBehavior.Restrict),
                j => j.ToTable("ProjectServiceContracts")
            );
*/
        // Unique constraint for the Firstname and LastName in the Roles table
        //    modelBuilder.Entity<Users>(entity =>
        //    {
        //        entity
        //            .HasIndex(u => new { u.FirstName, u.LastName })
        //            .IsUnique()
        //            .HasDatabaseName("IX_Users_FirstName_LastName");
        //    });

        // Inspired by Robin
        // Setting the Identity column to start at 100 and increment by 1
        /*
        modelBuilder
            .Entity<ProjectsEntity>()
            .Property(p => p.Id)
            .UseIdentityColumn(100, 1);
*/
        // Loading the configurations from the DataSeeding folder to seed the database
        // Inspired by Robin and Partially created by AI Phind
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}
