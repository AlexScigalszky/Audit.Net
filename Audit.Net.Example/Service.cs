using Audit.Net.Example.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Audit.Net.Example
{
    public class Service
    {
        private readonly BloggingContext db;

        public Service(BloggingContext context)
        {
            db = context;
        }

        public void Remove(Blog blog)
        {
            Console.WriteLine("Delete the blog");
            db.Remove(blog);
            db.SaveChanges();
        }

        public void AddPost(Blog blog)
        {
            Console.WriteLine("Updating the blog and adding a post");
            blog.Url = "https://devblogs.microsoft.com/dotnet";
            blog.Posts.Add(
                new Post
                {
                    Title = "Hello World",
                    Content = "I wrote an app using EF Core!"
                });
            db.SaveChanges();
        }

        public Blog Read()
        {
            Console.WriteLine("Querying for a blog");
            var blog = db.Blogs
                .OrderBy(b => b.BlogId)
                .First();
            return blog;
        }

        public void Create()
        {
            Console.WriteLine("Inserting a new blog");
            db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
            db.SaveChanges();
        }

        public void ShowAudits()
        {
            Console.WriteLine("Showing Audit table");
            ConsoleHelper.PrintRow(
                    "Audit Id",
                    "DateTime",
                    "Entity Id",
                    "Entity Type",
                    "Operation",
                    "User Id");
            var rows = db.Audits;
            foreach (var x in rows)
            {
                ConsoleHelper.PrintRow(
                    x.Id + string.Empty,
                    x.DateTime + string.Empty,
                    x.EntityId + string.Empty,
                    x.EntityType,
                    x.Operation,
                    x.UserId + string.Empty);
            }
        }
    }
}
