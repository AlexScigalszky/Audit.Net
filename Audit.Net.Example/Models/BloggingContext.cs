using Audit.Net.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Audit.Net.Example.Models
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Net.Models.Audit> Audits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=blogging.db");

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // you can fetch the user from a services with Dependecy Injection 
            var user = new User()
            {
                Id = 1,
                Name = "Jonh"
            };
            var auditor = new Auditor(this, user, ChangeTracker);
            auditor.Prepare();
            var result = base.SaveChangesAsync(cancellationToken);
            auditor.Complete();
            base.SaveChanges();
            return result;
        }

        public override int SaveChanges()
        {
            // you can fetch the user from a services with Dependecy Injection 
            var user = new User()
            {
                Id = 1,
                Name = "Jonh"
            };
            var auditor = new Auditor(this, user, ChangeTracker);
            auditor.Prepare();
            var result = base.SaveChanges();
            auditor.Complete();
            base.SaveChanges();
            return result;
        }
    }

    public class Blog : AuditModel
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; } = new List<Post>();
    }

    public class Post: AuditModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }

    public class User : IUser
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
