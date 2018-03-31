namespace FriendlyLocale.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPlatformCacheFileManager
    {
        bool ContainsFile(string filePath);

        IList<string> FindFileNames(string dir);

        Task UpsertFile(string filePath, string content);

        Task<string> GetFile(string filePath);
    }
}