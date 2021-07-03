using Lab2.Data;
using Lab2.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class Test_AuthoService
    {
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            Console.WriteLine("In setup.");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDB")
                            .Options;

            _context = new ApplicationDbContext(options, new OperationalStoreOptionsForTests());


            _context.Authors.Add(new Lab2.Models.Author
            {
                Id = 100,
                FirstName = "FN1",
                LastName = "LN1",
                AuthorDateTime = Convert.ToDateTime("2020-10-13T03:15:00"),
            });

            _context.Authors.Add(new Lab2.Models.Author
            {
                Id = 111,
                FirstName = "FN1",
                LastName = "LN1",
                AuthorDateTime = Convert.ToDateTime("2015-12-12T13:15:00"),
            });

            _context.SaveChanges();
        }

        [TearDown]
        public void Teardown()
        {
            Console.WriteLine("In teardown");

            foreach (var author in _context.Authors)
            {
                _context.Remove(author);
            }
            _context.SaveChanges();
        }

        [Test]
        public void Test_AuthorExists()
        {
            var service = new AuthorManagementService(_context);
            Assert.True(service.AuthorExists(100));
            Assert.False(service.AuthorExists(99));
        }

        [Test]
        public void Test_DeleteAuthor()
        {
            var service = new AuthorManagementService(_context);
            Assert.AreEqual(1, service.GetAuthorById(100).ResponseOk.Count);
            service.DeleteAuthor(100);
            Assert.AreEqual(0, service.GetAuthorById(100).ResponseOk.Count);
            Assert.AreEqual(1, service.GetAll().ResponseOk.Count);
        }
    }
}
