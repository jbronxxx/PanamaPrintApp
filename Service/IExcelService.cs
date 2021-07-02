using Microsoft.AspNetCore.Http;
using PanamaPrintApp.Models;

namespace PanamaPrintApp.Service
{
    public interface IExcelService
    {
        string FileCreate(IFormFile file);

        ModelList ExcelReader(string path) { return new ModelList(); }
    }
}
