using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using OfficeOpenXml;
using Newtonsoft.Json;


namespace dataConversionApp.Controllers
{
    public class UtilityController : ApiController
    {
        [HttpGet]
        [Route("api/utility/convert")]
        public IHttpActionResult ConvertXLXSToJSONorXML(string fileName, string to)
        {
            try
            {
                //if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(to))
                //    return BadRequest("Please enter {fileName} and {to} parameters");

                //string filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Data"), fileName);

                //if (!File.Exists(filePath))
                //    return NotFound();

                //ExcelPackage package = new ExcelPackage(new FileInfo(filePath));

                //ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                //if (to.ToLower() == "json")
                //{
                //    var data = new { rows = worksheet.Cells.Value };
                //    string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
                //    string filePathForJSON = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Storage"), $"{fileName}.json");
                //    File.WriteAllText(filePathForJSON, jsonData);

                //    return Ok($"File has been converted to JSON and saved at {filePathForJSON}");
                //}
                //else if (to.ToLower() == "xml")
                //{
                //    return BadRequest("XML conversion is not supported yet.");
                //}

                //return BadRequest("Invalid conversion type. Supported types are JSON and XML.");
                return Ok("Hello World");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
