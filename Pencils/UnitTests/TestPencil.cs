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

            // Write whitespace if not enough durability
            testPencil.WriteToPage("enough", testPage);
            Assert.AreEqual("enoug ", testPage.Contents);

            // Even with no durability, we should still be able to write whitespace
            testPencil.WriteToPage(" \t\nextra", testPage);
            Assert.AreEqual("enoug  \t\n     ", testPage.Contents);
        }

        [TestMethod]
        public void TestDurabilityPartiallyRunsOut()
        {
            // Use a low-durability pencil for simplicity
            testPencil = new Pencil(3);

            // This should skip the capital 'A' but write the lowercase 'r'
            testPencil.WriteToPage("we Are", testPage);
            Assert.AreEqual("we  r ", testPage.Contents);
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
            testPage.Contents = "one two three";
            testPencil.Erase("three", testPage);
            Assert.AreEqual("one two      ", testPage.Contents);
        }

        [TestMethod]
        public void TestEraseLastOccurrence()
        {
            // Should replace the last occurrence with whitespace
            testPage.Contents = "la la la";
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

        [TestMethod]
        public void TestEraserDurability()
        {
            var eraserDurability = Pencil.DEFAULT_ERASER_DURABILITY;

            testPage.Contents = "this will be erased";

            // Should use 6
            testPencil.Erase("erased", testPage);
            Assert.AreEqual(eraserDurability -= 6, testPencil.EraserDurability);
            
            // Erasing whitespace doesn't use durability
            testPencil.Erase("be ", testPage);
            Assert.AreEqual(eraserDurability -= 2, testPencil.EraserDurability);
        }

        [TestMethod]
        public void TestEraserDurabilityRunsOut()
        {
            testPage.Contents = "this will";

            // Check what happens if durability runs out
            testPencil = new Pencil(eraserDurability: 3);
            testPencil.Erase("will", testPage);
            Assert.AreEqual("this w   ", testPage.Contents);
            Assert.AreEqual(0, testPencil.EraserDurability);

            // Nothing should change at this step
            testPencil.Erase("this", testPage);
            Assert.AreEqual("this w   ", testPage.Contents);
            Assert.AreEqual(0, testPencil.EraserDurability);
        }

        [TestMethod]
        public void TestEdit()
        {
            // Edit with enough whitespace
            testPage.Contents = "what    you mean";
            testPencil.Edit(5, "do", testPage);
            Assert.AreEqual("what do you mean", testPage.Contents);
            Assert.AreEqual(Pencil.DEFAULT_MAX_DURABILITY - 2, testPencil.PointDurability);
        }

        [TestMethod]
        public void TestEditClash()
        {
            // Edit without enough whitespace
            testPage.Contents = "what  you mean";
            testPencil.Edit(5, "do", testPage);
            Assert.AreEqual("what d@ou mean", testPage.Contents);
            Assert.AreEqual(Pencil.DEFAULT_MAX_DURABILITY - 2, testPencil.PointDurability);
        }

        [TestMethod]
        public void TestEditEndOfPage()
        {
            // What should behavior be here...?
        }
    }
}
