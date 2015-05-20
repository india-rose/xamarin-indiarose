using System.IO;
using System.Threading.Tasks;

namespace IndiaRose.Interfaces
{
    public interface IResourceService
    {
        void ShowPdfFile(string pdfFileName);
        Task<Stream> OpenZip(string zipFileName);
        void Copy(string src,string dest);

    }
}
