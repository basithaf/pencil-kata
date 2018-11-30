using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pencils.Models;

namespace UnitTests
{
    [TestClass]
    public class TestPencil
    {
        [TestMethod]
        public void TestPencilWriteToPage()
        {
            var page = new Page();
            var pencil = new Pencil();
            pencil.WriteToPage("lorem ipsum", page);
            Assert.AreEqual("lorem ipsum", page.Contents);
        }
    }
}
