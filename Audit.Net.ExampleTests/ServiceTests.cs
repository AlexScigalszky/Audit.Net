using Audit.Net.Example.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Audit.Net.Example.Tests
{
    [TestClass()]
    public class ServiceTests
    {
        [TestMethod()]
        public void CreateTest()
        {
            using (var db = new BloggingContext())
            {
                db.Database.Migrate();

                var blogsExpected = db.Blogs.Count() + 1;
                var auditsExpected = db.Audits.Count() + 1;
                var sr = new Service(db);
                sr.Create();

                Assert.AreEqual(blogsExpected, db.Blogs.Count());
                Assert.AreEqual(auditsExpected, db.Audits.Count());
            }
        }



        [TestMethod()]
        public void AddPostTest()
        {
            using (var db = new BloggingContext())
            {
                db.Database.Migrate();

                var sr = new Service(db);
                sr.Create();
                var postsExpected = db.Posts.Count() + 1;
                var blogsExpected = db.Blogs.Count();
                var auditsExpected = db.Audits.Count() + 1;
                var blog = db.Blogs.First();
                sr.AddPost(blog);

                Assert.AreEqual(postsExpected, db.Posts.Count());
                Assert.AreEqual(blogsExpected, db.Blogs.Count());
                Assert.AreEqual(auditsExpected, db.Audits.Count());
            }
        }

        [TestMethod()]
        public void RemoveTest()
        {
            using (var db = new BloggingContext())
            {
                db.Database.Migrate();

                var blogsExpected = db.Blogs.Count() - 1;
                var auditsExpected = db.Audits.Count() + 1;
                var sr = new Service(db);
                var blog = db.Blogs.First();
                sr.Remove(blog);

                Assert.AreEqual(blogsExpected, db.Blogs.Count());
                Assert.AreEqual(auditsExpected, db.Audits.Count());
            }
        }
    }
}