namespace NyamNyamDesktopApp.Services
{
    public class OpenPhotoDialogService : OpenFileDialogService
    {
        public OpenPhotoDialogService() : base()
        {
            _fileDialog.Filter = "Image Files (*.jpg,*.png)|*.jpg;*.png";
        }
    }
}
