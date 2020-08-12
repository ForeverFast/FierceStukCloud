namespace FierceStukCloud.Core.Services
{
    public interface IDialogService
    {
        void ShowMessage(string text);
        void ShowFolder(string path);
        string FileBrowserDialog();
        string FolderBrowserDialog();
    }
}
