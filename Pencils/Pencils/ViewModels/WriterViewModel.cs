using Pencils.Models;
using System.Windows.Input;

namespace Pencils.ViewModels
{
    public class WriterViewModel
    {
        #region Properties
        public Observable<Pencil> CurrentPencil { get; } = 
            new Observable<Pencil>(new Pencil());
        public Observable<Page> CurrentPage { get; } = 
            new Observable<Page>(new Page());
        public Observable<string> WriteString { get; } = 
            new Observable<string>("");
        public Observable<string> EraseString { get; } = 
            new Observable<string>("");
        public Observable<string> EditString { get; } = 
            new Observable<string>("");
        public Observable<int> EditPlace { get; } = 
            new Observable<int>(0);

        public Observable<int> NewPencilDurability =
            new Observable<int>(Pencil.DEFAULT_MAX_DURABILITY);
        public Observable<int> NewPencilEraserDurability =
            new Observable<int>(Pencil.DEFAULT_ERASER_DURABILITY);
        public Observable<int> NewPencilLength =
            new Observable<int>(Pencil.DEFAULT_INITIAL_LENGTH);
        #endregion Properties

        #region Commands
        public readonly ICommand WriteCommand, EraseCommand, 
            EditCommand, SharpenCommand, NewPencilCommand, NewPageCommand;
        #endregion Commands

        #region Methods
        public void WriteToCurrentPage()
        {
            CurrentPencil.Value.WriteToPage(
                WriteString.Value, CurrentPage.Value);
        }

        public void EraseFromCurrentPage()
        {
            CurrentPencil.Value.Erase(
                EraseString.Value, CurrentPage.Value);
        }

        public void EditCurrentPage()
        {
            CurrentPencil.Value.Edit(
                EditPlace.Value, EditString.Value, CurrentPage.Value);
        }

        public void SharpenPencil()
        {
            CurrentPencil.Value.Sharpen();
        }

        public void NewPencil()
        {
            CurrentPencil.Value = new Pencil(NewPencilDurability.Value, 
                NewPencilLength.Value, NewPencilEraserDurability.Value);
        }

        public void NewPage()
        {
            CurrentPage.Value = new Page();
        }
        #endregion Methods

        public WriterViewModel()
        {
            WriteCommand = new SimpleVmCommand(WriteToCurrentPage);
            EraseCommand = new SimpleVmCommand(EraseFromCurrentPage);
            EditCommand = new SimpleVmCommand(EditCurrentPage);
            SharpenCommand = new SimpleVmCommand(SharpenPencil);
            NewPencilCommand = new SimpleVmCommand(NewPencil);
            NewPageCommand = new SimpleVmCommand(NewPage);
        }
    }
}
