using Lab2.Data;
using Lab2.Models;
using Lab2.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests
{
    public class Test_BookService
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


            _context.Books.Add(new Lab2.Models.Book
            {
                Id = 100,
                Title = "Book1",
                Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                Genre = Lab2.Models.BookGenre.Adventure,
                Price = 100.0,
                Publisher = "Malcolm Inc.",
                Language = "English",
                NumberOfPages = 130,
                YearOfRelease = 2000,
                BookDateTime = Convert.ToDateTime("2020-10-13T03:15:00"),
            });

            _context.Books.Add(new Lab2.Models.Book
            {
                Id = 111,
                Title = "Book2",
                Description = "An organized crime dynasty's aging patriarch transfers control of his clandestine empire to his reluctant son.",
                Genre = Lab2.Models.BookGenre.Drama,
                Price = 173.3,
                Publisher = "Sunny Philadelphia",
                Language = "Swedish",
                NumberOfPages = 240,
                YearOfRelease = 2015,
                BookDateTime = Convert.ToDateTime("2015-12-12T13:15:00"),
            });

            _context.SaveChanges();
        }

        [TearDown]
        public void Teardown()
        {
            Console.WriteLine("In teardown");

            foreach (var book in _context.Books)
            {
                _context.Remove(book);
            }
            _context.SaveChanges();
        }

        [Test]
        public void Test_BookExists()
        {
            var service = new BookManagementService(_context);
            Assert.True(service.BookExists(100));
            Assert.False(service.BookExists(99));
        }

        [Test]
        public void Test_GetById()
        {
            var service = new BookManagementService(_context);
            Assert.AreEqual(1, service.GetBook(100).ResponseOk.Count);
            Assert.AreEqual(0, service.GetBook(99).ResponseOk.Count);
        }

        [Test]
        public void Test_GetAll()
        {
            var service = new BookManagementService(_context);
            Assert.AreEqual(2, service.GetBooks().ResponseOk.Count);
        }
    }
}