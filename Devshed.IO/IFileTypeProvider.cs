namespace Devshed.IO
{
    public interface IFileTypeProvider
    {
        FileType GetMimeType(string extension);
    }
}