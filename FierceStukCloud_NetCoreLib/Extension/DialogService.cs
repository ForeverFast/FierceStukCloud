using System.Diagnostics;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FierceStukCloud_NetCoreLib.Extension
{
    public static class DialogService
    {
        public static void ShowMessage(string text) => MessageBox.Show(text);

        public static void ShowFolder(string path) => Process.Start("explorer", path);

        public static string FileBrowserDialog()
        {
            var dlg = new CommonOpenFileDialog();
            dlg.DefaultExtension = ".mp3";
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                return dlg.FileName;
            else
                return "";
        }

        public static string FolderBrowserDialog()
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
