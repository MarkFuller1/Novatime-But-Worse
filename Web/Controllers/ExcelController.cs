using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.Threading.Tasks;
using Time.Interfaces;

namespace Time.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ExcelController : Controller
    {
        private readonly IExcelService _excelService;

        public ExcelController(IExcelService excel)
        {
            _excelService = excel;
        }

        [HttpGet]
        public async Task<IActionResult> downloadFile()
        {
            String filename = _excelService.buildExcelFileFromDBEntries();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), filename);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), filePath);
        }
        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}
