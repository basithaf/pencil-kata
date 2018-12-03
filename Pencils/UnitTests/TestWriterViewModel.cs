using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pencils.Models;
using Pencils.ViewModels;

namespace UnitTests
{
    [TestClass]
    public class TestWriterViewModel
    {
        private WriterViewModel vm;

        [TestInitialize]
        public void TestInit()
        {
            vm = new WriterViewModel();
        }

        [TestCleanup]
        public void TestClean()
        {
            vm = null;
        }

        [TestMethod]
        public void TestPencilWriteToPage()
        {
            // Test method and command invocation
            vm.WriteString.Value = "testing ";
            vm.WriteToCurrentPage();
            Assert.AreEqual("testing ", vm.CurrentPage.Value.Contents.Value);
            vm.WriteCommand.Execute(null);
            Assert.AreEqual("testing testing ", vm.CurrentPage.Value.Contents.Value);
        }

        [TestMethod]
        public void TestPencilErase()
        {
            // Test method and command invocation
            vm.CurrentPage.Value.Contents.Value = "blah blah blah";
            vm.EraseString.Value = "blah";
            vm.EraseFromCurrentPage();
            Assert.AreEqual("blah blah     ", vm.CurrentPage.Value.Contents.Value);
            vm.EraseCommand.Execute(null);
            Assert.AreEqual("blah          ", vm.CurrentPage.Value.Contents.Value);
        }

        [TestMethod]
        public void TestPencilEdit()
        {
            // Test method and command invocation
            vm.CurrentPage.Value.Contents.Value = "blah      blah";
            vm.EditString.Value = "blah";
            vm.EditPlace.Value = 5;

            vm.EditCurrentPage();
            Assert.AreEqual("blah blah blah", vm.CurrentPage.Value.Contents.Value);

            vm.CurrentPage.Value.Contents.Value = "blah      blah";
            vm.EditCommand.Execute(null);
            Assert.AreEqual("blah blah blah", vm.CurrentPage.Value.Contents.Value);
        }

        [TestMethod]
        public void TestPencilSharpen()
        {
            // Test method and command invocation
            // This assumes that Pencil.DEFAULT_INITIAL_LENGTH >= 2
            vm.CurrentPencil.Value.PointDurability.Value = 6;
            vm.SharpenPencil();
            Assert.AreEqual(Pencil.DEFAULT_MAX_DURABILITY,
                vm.CurrentPencil.Value.PointDurability.Value);

            vm.CurrentPencil.Value.PointDurability.Value = 6;
            vm.SharpenCommand.Execute(null);
            Assert.AreEqual(Pencil.DEFAULT_MAX_DURABILITY,
                vm.CurrentPencil.Value.PointDurability.Value);
        }

        [TestMethod]
        public void TestNewPencil()
        {
            // Test method with default values
            vm.CurrentPencil.Value.PointDurability.Value = 10;
            vm.CurrentPencil.Value.EraserDurability.Value = 10;
            vm.CurrentPencil.Value.CurrentLength.Value = 0;

            vm.NewPencil();

            Assert.AreEqual(Pencil.DEFAULT_MAX_DURABILITY,
                vm.CurrentPencil.Value.PointDurability.Value);
            Assert.AreEqual(Pencil.DEFAULT_ERASER_DURABILITY,
                vm.CurrentPencil.Value.EraserDurability.Value);
            Assert.AreEqual(Pencil.DEFAULT_INITIAL_LENGTH,
                vm.CurrentPencil.Value.CurrentLength.Value);

            // Test method with non-default values
            vm.CurrentPencil.Value.PointDurability.Value = 10;
            vm.CurrentPencil.Value.EraserDurability.Value = 10;
            vm.CurrentPencil.Value.CurrentLength.Value = 0;

            vm.NewPencilDurability.Value = 50000;
            vm.NewPencilEraserDurability.Value = 22;
            vm.NewPencilLength.Value = 5;

            vm.NewPencil();

            Assert.AreEqual(50000,
                vm.CurrentPencil.Value.PointDurability.Value);
            Assert.AreEqual(22,
                vm.CurrentPencil.Value.EraserDurability.Value);
            Assert.AreEqual(5,
                vm.CurrentPencil.Value.CurrentLength.Value);

            // Test command invocation
            vm.CurrentPencil.Value.PointDurability.Value = 10;
            vm.CurrentPencil.Value.EraserDurability.Value = 10;
            vm.CurrentPencil.Value.CurrentLength.Value = 0;

            vm.NewPencilDurability.Value = 50000;
            vm.NewPencilEraserDurability.Value = 22;
            vm.NewPencilLength.Value = 5;

            vm.NewPencilCommand.Execute(null);

            Assert.AreEqual(50000,
                vm.CurrentPencil.Value.PointDurability.Value);
            Assert.AreEqual(22,
                vm.CurrentPencil.Value.EraserDurability.Value);
            Assert.AreEqual(5,
                vm.CurrentPencil.Value.CurrentLength.Value);
        }

        [TestMethod]
        public void TestNewPage()
        {
            // Test method and command invocation
            vm.CurrentPage.Value.Contents.Value = "something";
            vm.NewPage();
            Assert.AreEqual("", vm.CurrentPage.Value.Contents.Value);

            vm.CurrentPage.Value.Contents.Value = "something";
            vm.NewPageCommand.Execute(null);
            Assert.AreEqual("", vm.CurrentPage.Value.Contents.Value);
        }
    }
}
