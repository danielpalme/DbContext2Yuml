namespace Palmmedia.DbContext2Yuml.Wpf.Interaction
{
    public interface IFileAccess
    {
        string SelectFile(string extension);

        void SaveFile(string extension, byte[] fileContent);
    }
}
