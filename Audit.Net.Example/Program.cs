using Audit.Net.Example.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Audit.Net.Example
{
    class Program
    {
        /// <summary>
        /// This example was inpired by https://docs.microsoft.com/en-us/ef/core/get-started/?tabs=visual-studio
        /// </summary>
        static void Main()
        {
            using (var db = new BloggingContext())
            {
                db.Database.Migrate();
                // Create
                Console.WriteLine("Inserting a new blog");
                db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
                db.SaveChanges();

                // Read
                Console.WriteLine("Querying for a blog");
                var blog = db.Blogs
                    .OrderBy(b => b.BlogId)
                    .First();

                // Update
                Console.WriteLine("Updating the blog and adding a post");
                blog.Url = "https://devblogs.microsoft.com/dotnet";
                blog.Posts.Add(
                    new Post
                    {
                        Title = "Hello World",
                        Content = "I wrote an app using EF Core!"
                    });
                db.SaveChanges();

                // Delete
                Console.WriteLine("Delete the blog");
                db.Remove(blog);
                db.SaveChanges();

                Console.WriteLine("Showing Audit table");
                Console.WriteLine(string.Join(Environment.NewLine, db.Audits.Select(x => $"{x.DateTime} {x.Id} - {x.Operation}")));
                Console.ReadKey();
            }
        }
    }
}
