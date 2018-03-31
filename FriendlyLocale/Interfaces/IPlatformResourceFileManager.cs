namespace FriendlyLocale.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPlatformResourceFileManager
    {
        IList<string> FindFileNames(string dir);

        Task<string> GetFile(string filePath);
    }
}