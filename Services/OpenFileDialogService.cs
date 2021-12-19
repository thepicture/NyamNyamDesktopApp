using Microsoft.Win32;

namespace NyamNyamDesktopApp.Services
{
    public class OpenFileDialogService : IDialogService
    {
        protected readonly OpenFileDialog _fileDialog;

        public OpenFileDialogService()
        {
            _fileDialog = new OpenFileDialog();
        }

        public object Result => _fileDialog.FileName == string.Empty
            ? null
            : _fileDialog.FileName;

        public bool Open()
        {
            return (bool)_fileDialog.ShowDialog();
        }
    }
}
