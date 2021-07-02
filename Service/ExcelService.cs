using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PanamaPrintApp.Models;
using System.IO;
using System.Linq;

namespace PanamaPrintApp.Service
{
    public class ExcelService : IExcelService
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public ExcelService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        /* TODO: 
         * сделать проверку на наличия уже созданного файла в папке с проектом
         */
        public string FileCreate(IFormFile file)
        {
            // Путь к папке wwwroot в папке с проектом
            string filePath = $"{_hostEnvironment.WebRootPath}\\{file.FileName}";

            using (FileStream stream = File.Create(filePath))
            {
                // Копирует файл Excel в папку wwwroot
                file.CopyTo(stream);

                // Закрывает открытый поток
                stream.Flush();
            }

            return filePath;
        }

        public ModelList ExcelReader(string path)
        {
            // Список моделей техники
            ModelList modelList = new ModelList();

            // Открывет Excel документ
            using (XLWorkbook workbook = new XLWorkbook(path, XLEventTracking.Disabled))
            {
                // Проходит по листам Excel документа
                foreach (IXLWorksheet worksheet in workbook.Worksheets)
                {
                    // Проходит по колонкам активного листа Excel документа
                    foreach (IXLColumn column in worksheet.ColumnsUsed().Skip(1))
                    {
                        Model model = new Model();

                        // Записывает в список моделей значения первых ячеек колонок в качестве названия техники,
                        // пропуская первую колонку
                        model.ModelName = column.Cell(1).Value.ToString();

                        foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                        {
                            Price price = new Price();

                            // Записывает значения первых ячеек первой колонки в качестве наименования услуги
                            price.PriceName = row.Cell(1).Value.ToString();

                            // Значения остальных ячеек в качестве цены на услуги
                            price.ServicePrice = row.Cell(column.ColumnNumber()).Value.ToString();

                            // Запись в список прайса на каждую модель
                            model.Prices.Add(price);
                        }

                        // Запись в список моделей
                        modelList.Models.Add(model);
                    }
                }
                return modelList;
            }
        }
    }
}
