using FierceStukCloud.Core.Services;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.Windows;

namespace FierceStukCloud.Pc.Services
{
    public class DialogService : IDialogService
    {
        public void ShowMessage(string text) => MessageBox.Show(text);

        public void ShowFolder(string path) => Process.Start("explorer", path);

        public string FileBrowserDialog()
        {
            var dlg = new CommonOpenFileDialog();
            dlg.DefaultExtension = ".mp3";
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                return dlg.FileName;
            else
                return "";
        }

        public string FolderBrowserDialog()
        {
            var dlg = new CommonOpenFileDialog();
            dlg.IsFolderPicker = true;
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                return dlg.FileName;
            else
                return "";
        }
    }
}
