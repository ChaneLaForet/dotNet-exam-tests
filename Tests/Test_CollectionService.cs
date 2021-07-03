using Lab2.Data;
using Lab2.Services;
using Lab2.ViewModels.Collections;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class Test_CollectionService
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


            _context.Collections.Add(new Lab2.Models.Collection
            {
                Id = 100,
                CollectionName = "COL1",
                CollectionDateTime = Convert.ToDateTime("2020-10-13T03:15:00"),
            });

            _context.Collections.Add(new Lab2.Models.Collection
            {
                Id = 111,
                CollectionName = "COL2",
                CollectionDateTime = Convert.ToDateTime("2015-12-12T13:15:00"),
            });

            _context.SaveChanges();
        }

        [TearDown]
        public void Teardown()
        {
            Console.WriteLine("In teardown");

            foreach (var collection in _context.Collections)
            {
                _context.Remove(collection);
            }
            _context.SaveChanges();
        }

        [Test]
        public void Test_CollectionExists()
        {
            var service = new CollectionManagementService(_context);
            Assert.True(service.CollectionExists(100));
            Assert.False(service.CollectionExists(99));
        }

        [Test]
        public void Test_AddCollection()
        {
            var service = new CollectionManagementService(_context);
            NewCollectionRequest newCollection = new();
            service.AddCollection("aaaa", newCollection);
            Assert.AreEqual(1, service.GetAll("aaaa").ResponseOk.Count);
        }
    }
}
