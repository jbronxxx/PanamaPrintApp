using Microsoft.AspNetCore.Http;
using PanamaPrintApp.Models;

namespace PanamaPrintApp.Service
{
    public interface IExcelService
    {
        void FileCreate(IFormFile file);

        ModelList ExcelReader(string path) { return new ModelList; }
    }
}
