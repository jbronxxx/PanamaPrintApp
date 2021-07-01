using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PanamaPrintApp.Models;
using System.Collections.Generic;
using System.IO;

namespace PanamaPrintApp.Service
{
    public class ExcelService : IExcelService
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public ExcelService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        // Импортирует файл Excel 
        public List<Price> ExcelImport(IFormFile file)
        {
            List<Price> prices = new List<Price>();

            // Путь к папке wwwroot в папке с проектом
            string filePath = $"{_hostEnvironment.WebRootPath}\\{file.FileName}";

            using (FileStream stream = File.Create(filePath))
            {
                // Копирует файл Excel в папку wwwroot
                file.CopyTo(stream);

                // Закрывает открытый поток
                stream.Flush();
            }

            using (var excelOpen = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(excelOpen))
                {
                    // Читает файл Excel 
                    while (reader.Read())
                    {
                        // Создает список с прайслистом из файла Excel
                        prices.Add(new Price
                        {
                            PriceName = reader.GetValue(0).ToString(),
                            ServicePrice = reader.GetValue(1).ToString()
                        });
                    }
                }
            }
            // Возвращает созданный список с прайсом
            return prices;
        }
    }
}
