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
            Assert.AreEqual("lorem ipsum", testPage.Contents.Value);

            // Check that this adds to what was written previously
            testPencil.WriteToPage(" blah blah blah...", testPage);
            Assert.AreEqual("lorem ipsum blah blah blah...", testPage.Contents.Value);
        }

        [TestMethod]
        public void TestPencilDurability()
        {
            var durabilityLeft = Pencil.DEFAULT_MAX_DURABILITY;

            // Should use 4 durability
            testPencil.WriteToPage("four", testPage);
            Assert.AreEqual(durabilityLeft -= 4, testPencil.PointDurability.Value);

            // Should also use 4 durability -- whitespace
            testPencil.WriteToPage(" four \t\n", testPage);
            Assert.AreEqual(durabilityLeft -= 4, testPencil.PointDurability.Value);

            // Should use 5 durability -- capital
            testPencil.WriteToPage("Five", testPage);
            Assert.AreEqual(durabilityLeft -= 5, testPencil.PointDurability.Value);
        }

        [TestMethod]
        public void TestDurabilityRunsOut()
        {
            // Use a low-durability pencil for simplicity
            testPencil = new Pencil(5);

            // Write whitespace if not enough durability
            testPencil.WriteToPage("enough", testPage);
            Assert.AreEqual("enoug ", testPage.Contents.Value);

            // Even with no durability, we should still be able to write whitespace
            testPencil.WriteToPage(" \t\nextra", testPage);
            Assert.AreEqual("enoug  \t\n     ", testPage.Contents.Value);
        }

        [TestMethod]
        public void TestDurabilityPartiallyRunsOut()
        {
            // Use a low-durability pencil for simplicity
            testPencil = new Pencil(3);

            // This should skip the capital 'A' but write the lowercase 'r'
            testPencil.WriteToPage("we Are", testPage);
            Assert.AreEqual("we  r ", testPage.Contents.Value);
        }

        [TestMethod]
        public void TestPencilSharpen()
        {
            // Confirm that sharpening restores max durability
            testPencil.WriteToPage("using up some durability", testPage);
            testPencil.Sharpen();
            Assert.AreEqual(Pencil.DEFAULT_MAX_DURABILITY, testPencil.PointDurability.Value);
        }

        [TestMethod]
        public void TestPencilLength()
        {
            // Test that Sharpening works for each unit of length
            int i = Pencil.DEFAULT_INITIAL_LENGTH;
            while(i > 0)
            {
                // Check current length, then use some durability
                Assert.AreEqual(i, testPencil.CurrentLength.Value);
                testPencil.WriteToPage("something", testPage);
                Assert.AreNotEqual(Pencil.DEFAULT_MAX_DURABILITY, testPencil.PointDurability.Value);

                // Confirm length decreases and durability restored after each sharpening
                testPencil.Sharpen();
                i--;
                Assert.AreEqual(i, testPencil.CurrentLength.Value);
                Assert.AreEqual(Pencil.DEFAULT_MAX_DURABILITY, testPencil.PointDurability.Value);
            }

            // This one shouldn't work!
            testPencil.WriteToPage("can't sharpen anymore!", testPage);
            var currentDurability = testPencil.PointDurability.Value;
            testPencil.Sharpen();
            Assert.AreEqual(currentDurability, testPencil.PointDurability.Value);
            Assert.AreEqual(0, testPencil.CurrentLength.Value);
        }

        [TestMethod]
        public void TestErase()
        {
            testPage.Contents.Value = "one two three";
            testPencil.Erase("three", testPage);
            Assert.AreEqual("one two      ", testPage.Contents.Value);
        }

        [TestMethod]
        public void TestEraseLastOccurrence()
        {
            // Should replace the last occurrence with whitespace
            testPage.Contents.Value = "la la la";
            testPencil.Erase("la", testPage);
            Assert.AreEqual("la la   ", testPage.Contents.Value);
            testPencil.Erase("la", testPage);
            Assert.AreEqual("la      ", testPage.Contents.Value);
            testPencil.Erase("la", testPage);
            Assert.AreEqual("        ", testPage.Contents.Value);

            // This should leave the string as-is!
            testPencil.Erase("la", testPage);
            Assert.AreEqual("        ", testPage.Contents.Value);
        }

        [TestMethod]
        public void TestEraserDurability()
        {
            var eraserDurability = Pencil.DEFAULT_ERASER_DURABILITY;

            testPage.Contents.Value = "this will be erased";

            // Should use 6
            testPencil.Erase("erased", testPage);
            Assert.AreEqual(eraserDurability -= 6, testPencil.EraserDurability.Value);
            
            // Erasing whitespace doesn't use durability
            testPencil.Erase("be ", testPage);
            Assert.AreEqual(eraserDurability -= 2, testPencil.EraserDurability.Value);
        }

        [TestMethod]
        public void TestEraserDurabilityRunsOut()
        {
            testPage.Contents.Value = "this will";

            // Check what happens if durability runs out
            testPencil = new Pencil(eraserDurability: 3);
            testPencil.Erase("will", testPage);
            Assert.AreEqual("this w   ", testPage.Contents.Value);
            Assert.AreEqual(0, testPencil.EraserDurability.Value);

            // Nothing should change at this step
            testPencil.Erase("this", testPage);
            Assert.AreEqual("this w   ", testPage.Contents.Value);
            Assert.AreEqual(0, testPencil.EraserDurability.Value);
        }

        [TestMethod]
        public void TestEdit()
        {
            // Edit with enough whitespace
            testPage.Contents.Value = "what    you mean";
            testPencil.Edit(5, "do", testPage);
            Assert.AreEqual("what do you mean", testPage.Contents.Value);
            Assert.AreEqual(Pencil.DEFAULT_MAX_DURABILITY - 2, testPencil.PointDurability.Value);
        }

        [TestMethod]
        public void TestEditClash()
        {
            // Edit without enough whitespace
            testPage.Contents.Value = "what  you mean";
            testPencil.Edit(5, "do", testPage);
            Assert.AreEqual("what d@ou mean", testPage.Contents.Value);
            Assert.AreEqual(Pencil.DEFAULT_MAX_DURABILITY - 2, testPencil.PointDurability.Value);
        }

        [TestMethod]
        public void TestEditEdgeCases()
        {
            // Invalid Index
            testPage.Contents.Value = " do you mean";
            testPencil.Edit(-1, "what", testPage);
            Assert.AreEqual(" do you mean", testPage.Contents.Value);
            Assert.AreEqual(Pencil.DEFAULT_MAX_DURABILITY, testPencil.PointDurability.Value);

            // Hit end of the page -- should continue to write
            testPencil.Edit(10, "  der", testPage);
            Assert.AreEqual(" do you meander", testPage.Contents.Value);
            Assert.AreEqual(Pencil.DEFAULT_MAX_DURABILITY - 3, testPencil.PointDurability.Value);
        }
    }
}
