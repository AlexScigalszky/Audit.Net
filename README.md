# Audit.Net

This library add support for audit entities into a table as, for example, JSON.

## Audit.Net.Example
There is an Example project.

## How to use
 - Add into your solution, the project Audit.Net and import it into the data project.
 - Heritage the entities that you want to audit and make sure it has the abstract properties.
 - Add the Audit table in the DbContext
```C#
    public DbSet<Audit.Net.Models.Audit> Audits { get; set; }
```
- Fetch the user (who make the changes in entities) as `Audit.Net.Models.IUser`.
- Override the `SaveChanges()` method.
```C#
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var auditor = new Auditor(this, user, ChangeTracker);
        auditor.Prepare();
        var result = base.SaveChangesAsync(cancellationToken);
        auditor.Complete();
        base.SaveChanges();
        return result;
    }

    public override int SaveChanges()
    {
        var auditor = new Auditor(this, user, ChangeTracker);
        auditor.Prepare();
        var result = base.SaveChanges();
        auditor.Complete();
        base.SaveChanges();
        return result;
    }
```
