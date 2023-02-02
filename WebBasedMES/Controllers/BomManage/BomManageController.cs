using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Services.Repositories.Bom;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.BaseInfo;
using WebBasedMES.ViewModels.Bom;
using WebBasedMES.ViewModels.File;

namespace WebBasedMES.Controllers.Bom
{
    public class BomManageController : ControllerBase
    {
        private readonly ILogger<BomManageController> _logger;
        private readonly IBomRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public BomManageController(ILogger<BomManageController> logger, IBomRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;
        }

        [HttpPost("/api/bom/getProductInputItems")]
        public async Task<IActionResult> GetProductInputItems([FromBody] ProductInputItemRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProductInputItems(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/bom/getProductInputItemAdd")]
        public async Task<IActionResult> GetProductInputItemAdd([FromBody] ProductInputItemRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProductInputItemAdd(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        //File Download
        [HttpPost("/api/bom/downloadBomExcelFile")]
        public async Task<IActionResult> DownloadBomExcelFile([FromBody] ProductRequest prd)
        {
            var boms = await _repo.DownloadBomExcelFile(prd);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "BOM.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("BOM");

                worksheet.Cell("A1").Value = "품목코드";
                worksheet.Cell("B1").Value = "품목이름";
                worksheet.Cell("C1").Value = "공정코드";
                worksheet.Cell("D1").Value = "공정이름";
                worksheet.Cell("E1").Value = "공정순서";

                worksheet.Cell("F1").Value = "생산품목코드";
                worksheet.Cell("G1").Value = "생산품목이름";
                worksheet.Cell("H1").Value = "생산수량(cavity)";

                worksheet.Cell("I1").Value = "투입자재코드";
                worksheet.Cell("J1").Value = "투입자재이름";
                worksheet.Cell("K1").Value = "LOSS";
                worksheet.Cell("L1").Value = "투입수량(소요량)";


                int i = 0;
                foreach (var product in boms.Data)
                {
                    if (product.ProductProcesses.Count() > 0)
                    {
                        foreach (var process in product.ProductProcesses)
                        {
                            if (process.ProductItems.Count() > 0)
                            {
                                foreach (var inputItem in process.ProductItems)
                                {
                                    i++;

                                    worksheet.Cell("A" + (i + 1).ToString()).Value = product.ProductCode;
                                    worksheet.Cell("B" + (i + 1).ToString()).Value = product.ProductName;
                                    worksheet.Cell("C" + (i + 1).ToString()).Value = process.ProcessCode;
                                    worksheet.Cell("D" + (i + 1).ToString()).Value = process.ProcessName;
                                    worksheet.Cell("E" + (i + 1).ToString()).Value = process.ProcessOrder;

                                    worksheet.Cell("F" + (i + 1).ToString()).Value = process.ProductProducedCode;
                                    worksheet.Cell("G" + (i + 1).ToString()).Value = process.ProductProducedName;
                                    worksheet.Cell("H" + (i + 1).ToString()).Value = "";

                                    worksheet.Cell("I" + (i + 1).ToString()).Value = inputItem.ProductCode;
                                    worksheet.Cell("J" + (i + 1).ToString()).Value = inputItem.ProductName;
                                    worksheet.Cell("K" + (i + 1).ToString()).Value = inputItem.Loss.ToString("0.000");
                                    worksheet.Cell("L" + (i + 1).ToString()).Value = inputItem.Require.ToString("0.000");
                                }
                            }
                            else
                            {
                                i++;

                                worksheet.Cell("A" + (i + 1).ToString()).Value = product.ProductCode;
                                worksheet.Cell("B" + (i + 1).ToString()).Value = product.ProductName;
                                worksheet.Cell("C" + (i + 1).ToString()).Value = process.ProcessCode;
                                worksheet.Cell("D" + (i + 1).ToString()).Value = process.ProcessName;
                                worksheet.Cell("E" + (i + 1).ToString()).Value = process.ProcessOrder;

                                worksheet.Cell("F" + (i + 1).ToString()).Value = process.ProductProducedCode;
                                worksheet.Cell("G" + (i + 1).ToString()).Value = process.ProductProducedName;
                                worksheet.Cell("H" + (i + 1).ToString()).Value = "";
                            }
                        }
                    }
                    else
                    {
                        i++;
                        worksheet.Cell("A" + (i + 1).ToString()).Value = product.ProductCode;
                        worksheet.Cell("B" + (i + 1).ToString()).Value = product.ProductName;
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/bom/uploadBomExcelFile")]
        public async Task<IActionResult> UploadBomExcelFile([FromForm] FileModel file)
        {
            var _file = file;
            var stream = file.FormFile.OpenReadStream();

            var workbook = new XLWorkbook(stream);
            var workSheet = workbook.Worksheets.First();
            var range = workSheet.RangeUsed();


            // List<BomUpdateRequest> boms = new List<BomUpdateRequest>();
            string errMessage = "";

            List<BomUpdateInterface> boms = new List<BomUpdateInterface>();
            try
            {

                for (int i = 2; i < range.RowCount(); i++)
                {
                    string _ProductCode = workSheet.Cell(i, 1).Value.ToString();
                    errMessage = i.ToString() + " 1";
                    string _ProcessCode = workSheet.Cell(i, 3).Value.ToString();
                    errMessage = i.ToString() + " 2";

                    int _ProcessOrder = Convert.ToInt32(workSheet.Cell(i, 5).Value.ToString());
                    errMessage = i.ToString() + " 3";

                    string _ProducedProductCode = workSheet.Cell(i, 6).Value.ToString();
                    errMessage = i.ToString() + " 4";

                    int _Cavity = workSheet.Cell(i, 8).Value.ToString() != "" ? Convert.ToInt32(workSheet.Cell(i, 8).Value.ToString()) : 0;
                    errMessage = i.ToString() + " 5";

                    string _InputItemCode = workSheet.Cell(i, 9).Value.ToString();
                    errMessage = i.ToString() + " 6";

                    double _Loss = workSheet.Cell(i, 11).Value.ToString() != "" ? Convert.ToDouble(workSheet.Cell(i, 11).Value.ToString()) : 0.0;
                    errMessage = i.ToString() + " 7";

                    double _Require = workSheet.Cell(i, 12).Value.ToString() != "" ? Convert.ToDouble(workSheet.Cell(i, 12).Value.ToString()) : 0.0;
                    errMessage = i.ToString() + " 8";

                    var bom = new BomUpdateInterface
                    {
                        ProductCode = workSheet.Cell(i, 1).Value.ToString(),
                        ProcessCode = workSheet.Cell(i, 3).Value.ToString(),
                        ProcessOrder = Convert.ToInt32(workSheet.Cell(i, 5).Value.ToString()),
                        ProducedProductCode = workSheet.Cell(i, 6).Value.ToString(),
                        Cavity = workSheet.Cell(i, 8).Value.ToString() != "" ? Convert.ToInt32(workSheet.Cell(i, 8).Value.ToString()) : 0,
                        InputItemCode = workSheet.Cell(i, 9).Value.ToString(),
                        Loss = workSheet.Cell(i, 11).Value.ToString() != "" ? Convert.ToDouble(workSheet.Cell(i, 11).Value.ToString()) : 0.0,
                        Require = workSheet.Cell(i, 12).Value.ToString() != "" ? Convert.ToDouble(workSheet.Cell(i, 12).Value.ToString()) : 0.0,
                    };
                    boms.Add(bom);
                }
            }catch(Exception ex)
            {
                Console.WriteLine(errMessage);
            }



            if (boms.Count() < 1)
            {
                return Ok();
            }
            else
            {

                var result = await _repo.UploadBomExcelFile(boms);
                if (result.IsSuccess)
                    return Ok(result);
                else
                    return Ok(result);
            }
        }


        public async Task SmartFactoryLog(string uuid, string type, string dataSize)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(uuid);
                var _company = await _db.BusinessInfo.FirstOrDefaultAsync();
                if (_user != null)
                {
                    string uri = "https://log.smart-factory.kr/apisvc/sendLogData.json?";
                    uri += ("crtfcKey=" + _company.CrtfcKey + "&");
                    uri += ("logDt=" + DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd HH:mm:ss.fff") + "&");
                    uri += ("useSe=" + type + "&");
                    uri += ("conectIp=" + _user.LoginIp + "&");
                    uri += ("sysUser=" + _user.IdNumber + "&");
                    uri += ("dataUsgqty=" + dataSize);
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                    req.Method = "POST";
                    req.ContentType = "application/x-www-form-urlencoded";
                    req.Timeout = 3 * 1000;

                    using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
                    {
                        HttpStatusCode status = res.StatusCode;
                        Stream respStream = res.GetResponseStream();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


    }
}
