using Microsoft.AspNetCore.Http;
using PanamaPrintApp.Models;
using System.Collections.Generic;

namespace PanamaPrintApp.Service
{
    public interface IExcelService
    {
        List<Price> ExcelImport(IFormFile file) { return new List<Price>(); }
    }
}
