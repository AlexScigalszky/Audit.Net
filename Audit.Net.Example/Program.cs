using Audit.Net.Example.Models;
using Microsoft.EntityFrameworkCore;
using System;

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

                var sr = new Service(db);

                Console.WriteLine("Ready?");

                Console.ReadKey();

                // Create
                sr.Create();

                // Read
                Blog blog = sr.Read();

                // Update
                sr.AddPost(blog);

                // Delete
                sr.Remove(blog);

                // Audits
                sr.ShowAudits();

                Console.ReadKey();
            }
        }
    }
}
