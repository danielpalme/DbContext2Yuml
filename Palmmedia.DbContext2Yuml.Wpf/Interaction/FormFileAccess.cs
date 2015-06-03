using System.IO;
using Microsoft.Win32;

namespace Palmmedia.DbContext2Yuml.Wpf.Interaction
{
    public class FormFileAccess : IFileAccess
    {
        public string SelectFile(string extension)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.Filter = string.Format("{0} (*.{1})|*.{1}", extension.ToUpperInvariant(), extension);

            if (fileDialog.ShowDialog().GetValueOrDefault())
            {
                return fileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

        public void SaveFile(string extension, byte[] fileContent)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = string.Format("{0} (*.{1})|*.{1}", extension.ToUpperInvariant(), extension);
            if (saveFileDialog.ShowDialog().GetValueOrDefault())
            {
                File.WriteAllBytes(saveFileDialog.FileName, fileContent);
            }
        }
    }
}
