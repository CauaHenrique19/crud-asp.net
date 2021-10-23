using System.IO;
using System.Threading.Tasks;

namespace WepApi1.Providers
{
    public class IFile 
    {
        public string name;
        public string type;
        public Stream content;
    }

    public class IUpdateFile
    {
        public string key;
        public string type;
        public Stream content;
    }

    public class IUploadedFile
    {
        public string Key;
        public string Location;
    }

    public interface FileUploader
    {
        public Task<IUploadedFile> upload(IFile file);
        public Task<IUploadedFile> update(IUpdateFile file);
        public Task<string> delete(string key);
    }
}
