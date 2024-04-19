using dataConversionApp.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace dataConversionApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly EmployeeDBEntities EmployeeDBentities = new EmployeeDBEntities();
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadAndInsert(HttpPostedFileBase[] files, string fileType)
        {
            if (files.Length > 5)
            {
                ModelState.AddModelError("files", "You can only upload up to 5 files at a time.");
                return View("Index");
            }

            if (files.Length == 0)
            {
                ModelState.AddModelError("files", "Please select at least one file to upload.");
                return View("Index");
            }

            if (string.IsNullOrEmpty(fileType))
            {
                ModelState.AddModelError("fileType", "Please select a file type.");
                return View("Index");
            }

            try
            {
                var users = ProcessFiles(files, fileType);
                BulkInsertUsers(users);
                ViewBag.SuccessMessage = "Data inserted successfully.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error processing files: {ex.Message}");
            }

            return View("Index");
        }

        private List<User> ProcessFiles(HttpPostedFileBase[] files, string fileType)
        {
            var users = new List<User>();

            foreach (var file in files)
            {
                using (var stream = file.InputStream)
                {
                    switch (fileType)
                    {
                        case "json":
                            users.AddRange(JsonConvert.DeserializeObject<List<User>>(new StreamReader(stream).ReadToEnd()));
                            break;
                        case "xml":
                            var xmlDoc = new XmlDocument();
                            xmlDoc.Load(stream);
                            users.AddRange(JsonConvert.DeserializeObject<List<User>>(JsonConvert.SerializeXmlNode(xmlDoc)));
                            break;
                        case "xlsx":
                            using (var package = new ExcelPackage(stream))
                            {
                                var worksheet = package.Workbook.Worksheets[0];
                                var rows = worksheet.Dimension.Rows;
                                for (int row = 2; row <= rows; row++)
                                {
                                    users.Add(new User
                                    {
                                        name = worksheet.Cells[row, 1].Value?.ToString(),
                                        salary = (int)worksheet.Cells[row, 2].Value,
                                        city = worksheet.Cells[row, 3].Value?.ToString()
                                    });
                                }
                            }
                            break;
                        default:
                            throw new Exception($"Unsupported file type: {fileType}");
                    }
                }
            }

            return users;
        }

        private void BulkInsertUsers(List<User> users)
        {
            EmployeeDBentities.Users.AddRange(users);
            EmployeeDBentities.SaveChanges();
        }
    }
}
