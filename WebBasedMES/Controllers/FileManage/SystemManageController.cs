using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using WebBasedMES.Services.Repositories.Quality;
using WebBasedMES.Services.Repositories.SystemManage;
using WebBasedMES.ViewModels.BaseInfo;
using WebBasedMES.ViewModels.Quality;
using WebBasedMES.ViewModels.SystemManage;

namespace WebBasedMES.Controllers.QualityManage
{
    public class SystemManageController : ControllerBase
    {
        private readonly ILogger<SystemManageController> _logger;
        private readonly ISystemManageRepository _repo;


        public SystemManageController(ILogger<SystemManageController> logger,
            ISystemManageRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpPost("/api/system/userLogs/download")]
        public async Task<IActionResult> GetUserLogsDownload([FromBody] UserLogRequest req)
        {

            var result = await _repo.GetUserLogs(req);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "사용자접속기록.xlsx";



            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("사용자접속기록");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "일시";
                worksheet.Cell("C1").Value = "부서";
                worksheet.Cell("D1").Value = "직위";
                worksheet.Cell("E1").Value = "사용자이름";
                worksheet.Cell("F1").Value = "메시지";
                worksheet.Cell("G1").Value = "IP";
                worksheet.Cell("H1").Value = "접속위치";


                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.CreateTime;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.Department;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.Position;
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.UserName;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.Message;
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.Ipv4;
                    worksheet.Cell("H" + (i + 1).ToString()).Value = res.Location;

                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

    }
}
