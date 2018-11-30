using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pencils.Models;

namespace UnitTests
{
    [TestClass]
    public class TestPencil
    {
        Page testPage;
        Pencil testPencil;

        [TestInitialize]
        public void TestInit()
        {
            testPage = new Page();
            testPencil = new Pencil();
        }

        [TestCleanup]
        public void TestClean()
        {
            testPage = null;
            testPencil = null;
        }

        [TestMethod]
        public void TestPencilWriteToPage()
        {
            
            testPencil.WriteToPage("lorem ipsum", testPage);
            Assert.AreEqual("lorem ipsum", testPage.Contents);
            testPencil.WriteToPage(" blah blah blah...", testPage);
            Assert.AreEqual("lorem ipsum blah blah blah...", testPage.Contents);
        }

        [TestMethod]
        public void TestPencilDurability()
        {
            // Should use 4 durability
            testPencil.WriteToPage("four", testPage);
            Assert.AreEqual(9996, testPencil.PointDurability);

            // Should also use 4 durability -- whitespace
            testPencil.WriteToPage(" four \t\n", testPage);
            Assert.AreEqual(9992, testPencil.PointDurability);

            // Should use 5 durability -- capital
            testPencil.WriteToPage("Five", testPage);
            Assert.AreEqual(9987, testPencil.PointDurability);

            // Reset pencil and page to test running out of durability
            testPencil = new Pencil(5);
            testPage = new Page();

            // Don't write chars if not enough durability
            testPencil.WriteToPage("enough", testPage);
            Assert.AreEqual("enoug", testPage.Contents);

            // Even with no durability, we should still be able to write whitespace
            testPencil.WriteToPage(" \t\n", testPage);
            Assert.AreEqual("enoug \t\n", testPage.Contents);
        }
    }
}
