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
            // Write something to page
            testPencil.WriteToPage("lorem ipsum", testPage);
            Assert.AreEqual("lorem ipsum", testPage.Contents);

            // Check that this adds to what was written previously
            testPencil.WriteToPage(" blah blah blah...", testPage);
            Assert.AreEqual("lorem ipsum blah blah blah...", testPage.Contents);
        }

        [TestMethod]
        public void TestPencilDurability()
        {
            var durabilityLeft = Pencil.DEFAULT_MAX_DURABILITY;

            // Should use 4 durability
            testPencil.WriteToPage("four", testPage);
            Assert.AreEqual(durabilityLeft -= 4, testPencil.PointDurability);

            // Should also use 4 durability -- whitespace
            testPencil.WriteToPage(" four \t\n", testPage);
            Assert.AreEqual(durabilityLeft -= 4, testPencil.PointDurability);

            // Should use 5 durability -- capital
            testPencil.WriteToPage("Five", testPage);
            Assert.AreEqual(durabilityLeft -= 5, testPencil.PointDurability);
        }

        [TestMethod]
        public void TestDurabilityRunsOut()
        {
            // Use a low-durability pencil for simplicity
            testPencil = new Pencil(5);

            // Don't write chars if not enough durability
            testPencil.WriteToPage("enough", testPage);
            Assert.AreEqual("enoug", testPage.Contents);

            // Even with no durability, we should still be able to write whitespace
            testPencil.WriteToPage(" \t\n", testPage);
            Assert.AreEqual("enoug \t\n", testPage.Contents);
        }

        [TestMethod]
        public void TestPencilSharpen()
        {
            // Confirm that sharpening restores max durability
            testPencil.WriteToPage("using up some durability", testPage);
            testPencil.Sharpen();
            Assert.AreEqual(Pencil.DEFAULT_MAX_DURABILITY, testPencil.PointDurability);
        }

        [TestMethod]
        public void TestPencilLength()
        {
            // Test that Sharpening works for each unit of length
            int i = Pencil.DEFAULT_INITIAL_LENGTH;
            while(i > 0)
            {
                // Check current length, then use some durability
                Assert.AreEqual(i, testPencil.CurrentLength);
                testPencil.WriteToPage("something", testPage);
                Assert.AreNotEqual(Pencil.DEFAULT_MAX_DURABILITY, testPencil.PointDurability);

                // Confirm length decreases and durability restored after each sharpening
                testPencil.Sharpen();
                i--;
                Assert.AreEqual(i, testPencil.CurrentLength);
                Assert.AreEqual(Pencil.DEFAULT_MAX_DURABILITY, testPencil.PointDurability);
            }

            // This one shouldn't work!
            testPencil.WriteToPage("can't sharpen anymore!", testPage);
            var currentDurability = testPencil.PointDurability;
            testPencil.Sharpen();
            Assert.AreEqual(currentDurability, testPencil.PointDurability);
            Assert.AreEqual(0, testPencil.CurrentLength);
        }

        [TestMethod]
        public void TestErase()
        {
            testPencil.WriteToPage("one two three", testPage);
            testPencil.Erase("three", testPage);
            Assert.AreEqual("one two      ", testPage.Contents);
        }

        [TestMethod]
        public void TestEraseLastOccurrence()
        {
            // Should replace the last occurrence with whitespace
            testPencil.WriteToPage("la la la", testPage);
            testPencil.Erase("la", testPage);
            Assert.AreEqual("la la   ", testPage.Contents);
            testPencil.Erase("la", testPage);
            Assert.AreEqual("la      ", testPage.Contents);
            testPencil.Erase("la", testPage);
            Assert.AreEqual("        ", testPage.Contents);

            // This should leave the string as-is!
            testPencil.Erase("la", testPage);
            Assert.AreEqual("        ", testPage.Contents);
        }
    }
}
