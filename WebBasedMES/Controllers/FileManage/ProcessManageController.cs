using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using WebBasedMES.Services.Repositories.BarcodeManage;
using WebBasedMES.Services.Repositories.InAndOut;
using WebBasedMES.Services.Repositories.Lots;
using WebBasedMES.Services.Repositories.ProcessManage;
using WebBasedMES.Services.Repositories.ProducePlanManage;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.BaseInfo;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;
using WebBasedMES.ViewModels.Lot;
using WebBasedMES.ViewModels.Process;
using WebBasedMES.ViewModels.ProducePlan;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Controllers.QualityManage
{
    public class ProcessManageController : ControllerBase
    {
        private readonly ILogger<ProcessManageController> _logger;
        
        private readonly IProcessRepository _processRepo;
        private readonly IProcessStatusRepository _processStatusRepo;
        private readonly IProducePlanRepository _producePlanRepo;
        private readonly IWorkOrderRepository _workOrderRepo;


        

        public ProcessManageController(ILogger<ProcessManageController> logger,
            IProcessRepository processRepo,
            IProcessStatusRepository processStatusRepo,
            IProducePlanRepository producePlanRepo,
            IWorkOrderRepository workOrderRepo
            )
        {
            _logger = logger;

            _processRepo = processRepo;
            _processStatusRepo = processStatusRepo;
            _producePlanRepo = producePlanRepo;
            _workOrderRepo = workOrderRepo;
        }

        [HttpPost("/api/produce/production-plan-manage/download")]
        public async Task<IActionResult> ProductionPlanManage([FromBody] ProducePlanRequest001 req)
        {
            var result = await _producePlanRepo.GetProducePlans(req);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "생산계획관리.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("생산계획관리");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "생산계획번호";
                worksheet.Cell("C1").Value = "생산계획시작일자";
                worksheet.Cell("D1").Value = "생산계획종료일자";
                worksheet.Cell("E1").Value = "총수량";
                worksheet.Cell("F1").Value = "등록일";
                worksheet.Cell("G1").Value = "등록자";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.productionPlanNo.ToString();
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.productionPlanStartDate.ToString();
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.productionPlanEndDate.ToString();
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.productionPlanProductCount.ToString();
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.registerDate.ToString();
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.registerName.ToString();
                  //  worksheet.Cell("H" + (i + 1).ToString()).Value = (res.orderSellPrice * 0.1).ToString("0");
                  //  worksheet.Cell("I" + (i + 1).ToString()).Value = (res.orderSellPrice * 1.1).ToString("0");
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }
        /*
        [HttpPost("/api/produce/require-amount/download")]
        public async Task<IActionResult> RequireAmount([FromBody] GetRequiredAmountsRequest001 req)
        {
            var result = await _producePlanRepo.GetRequiredAmounts(req);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "소요량산출.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("소요량산출");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "생산계획번호";
                worksheet.Cell("C1").Value = "생산계획시작일자";
                worksheet.Cell("D1").Value = "생산계획종료일자";
                worksheet.Cell("E1").Value = "총수량";
                worksheet.Cell("F1").Value = "등록일";
                worksheet.Cell("G1").Value = "등록자";

                int i = 0;
                foreach (var res in result.Data.)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.productionPlanNo.ToString();
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.productionPlanStartDate.ToString();
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.productionPlanEndDate.ToString();
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.productionPlanProductCount.ToString();
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.registerDate.ToString();
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.registerName.ToString();
                    //  worksheet.Cell("H" + (i + 1).ToString()).Value = (res.orderSellPrice * 0.1).ToString("0");
                    //  worksheet.Cell("I" + (i + 1).ToString()).Value = (res.orderSellPrice * 1.1).ToString("0");
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        */


        [HttpPost("/api/produce/work-order/download")]
        public async Task<IActionResult> WorkOrders([FromBody] WorkOrderRequest001 req)
        {
            var result = await _workOrderRepo.GetWorkOrders(req);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "작업지시관리.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("작업지시관리");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "작업지시번호";
                worksheet.Cell("C1").Value = "작업지시일";
                worksheet.Cell("D1").Value = "작업순서";
                worksheet.Cell("E1").Value = "제품코드";
                worksheet.Cell("F1").Value = "제품구분";
                worksheet.Cell("G1").Value = "제품이름";
                worksheet.Cell("H1").Value = "규격";
                worksheet.Cell("I1").Value = "단위";
                worksheet.Cell("J1").Value = "계획수량";
                worksheet.Cell("K1").Value = "계획잔량";
                worksheet.Cell("L1").Value = "지시수량";
                worksheet.Cell("M1").Value = "상태";
                worksheet.Cell("N1").Value = "등록일";
                worksheet.Cell("O1").Value = "등록자";
                worksheet.Cell("P1").Value = "생산계획번호";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.workOrderNo.ToString();
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.workOrderDate.ToString();
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.workOrderSequence.ToString();
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.productCode.ToString();
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.productClassification.ToString();
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.productName.ToString();
                    worksheet.Cell("H" + (i + 1).ToString()).Value = res.productStandard.ToString();
                    worksheet.Cell("I" + (i + 1).ToString()).Value = res.productUnit.ToString();
                    worksheet.Cell("J" + (i + 1).ToString()).Value = res.productPlanQuanity.ToString();
                    worksheet.Cell("K" + (i + 1).ToString()).Value = res.productPlanBacklog.ToString();
                    worksheet.Cell("L" + (i + 1).ToString()).Value = res.productWorkQuantity.ToString();
                    worksheet.Cell("M" + (i + 1).ToString()).Value = res.workOrderStatus.ToString();
                    worksheet.Cell("N" + (i + 1).ToString()).Value = res.registerDate.ToString();
                    worksheet.Cell("O" + (i + 1).ToString()).Value = res.registerName.ToString();
                    worksheet.Cell("P" + (i + 1).ToString()).Value = res.productionPlanNo.ToString();
                    //  worksheet.Cell("H" + (i + 1).ToString()).Value = (res.orderSellPrice * 0.1).ToString("0");
                    //  worksheet.Cell("I" + (i + 1).ToString()).Value = (res.orderSellPrice * 1.1).ToString("0");
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        //공정별 작업일보관리 ->공정 & 작업지시 한꺼번에 나올수 있게...
        [HttpPost("/api/produce/production-manage-by-product/download")]
        public async Task<IActionResult> ProductionManageByProduct([FromBody] GetProductionManageByProductsRequest001 req)
        {
            var result = await _producePlanRepo.GetProductionManageByProducts(req);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "제품별 생산량 관리.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("제품별 생산량관리");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "품목코드";
                worksheet.Cell("C1").Value = "품목구분";
                worksheet.Cell("D1").Value = "품목이름";
                worksheet.Cell("E1").Value = "규격";
                worksheet.Cell("F1").Value = "단위";
                worksheet.Cell("G1").Value = "작업지시번호";
                worksheet.Cell("H1").Value = "작업지시일시";
                worksheet.Cell("I1").Value = "작업종료일시";
                worksheet.Cell("J1").Value = "작업자";
                worksheet.Cell("K1").Value = "생산수량";
                worksheet.Cell("L1").Value = "양품수량";
                worksheet.Cell("M1").Value = "불량수량";
                worksheet.Cell("N1").Value = "소요시간";
                worksheet.Cell("O1").Value = "비가동시간";
                worksheet.Cell("P1").Value = "LOT";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.productCode.ToString();
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.productClassification.ToString();
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.productName.ToString();
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.productStandard.ToString();
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.productUnit.ToString();
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.workOrderNo.ToString();
                    worksheet.Cell("H" + (i + 1).ToString()).Value = res.workStartDateTime.ToString();
                    worksheet.Cell("I" + (i + 1).ToString()).Value = res.workEndDateTime.ToString();
                    worksheet.Cell("J" + (i + 1).ToString()).Value = res.workerName.ToString();
                    worksheet.Cell("K" + (i + 1).ToString()).Value = res.productionQuantity.ToString();
                    worksheet.Cell("L" + (i + 1).ToString()).Value = (res.productionQuantity - res.productDefectiveQuantity).ToString();
                    worksheet.Cell("M" + (i + 1).ToString()).Value = res.productDefectiveQuantity.ToString();
                    worksheet.Cell("N" + (i + 1).ToString()).Value = res.processElapsedTime.ToString();
                    worksheet.Cell("O" + (i + 1).ToString()).Value = res.downtime.ToString();
                    worksheet.Cell("P" + (i + 1).ToString()).Value = res.productLOT.ToString();
                    //  worksheet.Cell("H" + (i + 1).ToString()).Value = (res.orderSellPrice * 0.1).ToString("0");
                    //  worksheet.Cell("I" + (i + 1).ToString()).Value = (res.orderSellPrice * 1.1).ToString("0");
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/produce/production-manage-by-period/download")]
        public async Task<IActionResult> ProductionManageByPeriod([FromBody] GetProductionManageByPeriodsRequest001 req)
        {
            var result = await _producePlanRepo.GetProductionManageByPeriods(req);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "기간별 생산량 관리.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("기간별 생산량관리");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "작업지시일시";
                worksheet.Cell("C1").Value = "작업종료일시";
                worksheet.Cell("D1").Value = "작업지시번호";
                worksheet.Cell("E1").Value = "작업자";
                worksheet.Cell("F1").Value = "소요시간";
                worksheet.Cell("G1").Value = "비가동";
                worksheet.Cell("H1").Value = "품목코드";
                worksheet.Cell("I1").Value = "품목구분";
                worksheet.Cell("J1").Value = "품목이름";
                worksheet.Cell("K1").Value = "규격";
                worksheet.Cell("L1").Value = "단위";
                worksheet.Cell("M1").Value = "생산수량";
                worksheet.Cell("N1").Value = "양품수량";
                worksheet.Cell("O1").Value = "불량수량";
                worksheet.Cell("P1").Value = "LOT";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.workStartDateTime.ToString();
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.workEndDateTime.ToString();
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.workOrderNo.ToString();
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.workerName.ToString();
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.processElapsedTime.ToString();
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.downtime.ToString();
                    worksheet.Cell("H" + (i + 1).ToString()).Value = res.productCode.ToString();
                    worksheet.Cell("I" + (i + 1).ToString()).Value = res.productClassification.ToString();
                    worksheet.Cell("J" + (i + 1).ToString()).Value = res.productName.ToString();
                    worksheet.Cell("K" + (i + 1).ToString()).Value = res.productStandard.ToString();
                    worksheet.Cell("L" + (i + 1).ToString()).Value = res.productUnit.ToString();
                    worksheet.Cell("M" + (i + 1).ToString()).Value = res.productionQuantity.ToString();
                    worksheet.Cell("N" + (i + 1).ToString()).Value = (res.productionQuantity - res.productDefectiveQuantity).ToString();
                    worksheet.Cell("O" + (i + 1).ToString()).Value = res.productDefectiveQuantity.ToString();
                    worksheet.Cell("P" + (i + 1).ToString()).Value = res.productLOT.ToString();
                    //  worksheet.Cell("H" + (i + 1).ToString()).Value = (res.orderSellPrice * 0.1).ToString("0");
                    //  worksheet.Cell("I" + (i + 1).ToString()).Value = (res.orderSellPrice * 1.1).ToString("0");
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/produce/defetive-manage-by-period/download")]
        public async Task<IActionResult> DefectiveManageByPeriod([FromBody] GetDefectiveManageByPeriodsRequest001 req)
        {
            var result = await _producePlanRepo.GetDefectiveManageByPeriods(req);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "기간별 생산량 관리.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("기간별 생산량관리");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "불량발생일";
                worksheet.Cell("C1").Value = "불량코드";
                worksheet.Cell("D1").Value = "불량유형";
                worksheet.Cell("E1").Value = "품목코드";
                worksheet.Cell("F1").Value = "품목구분";
                worksheet.Cell("G1").Value = "품목이름";
                worksheet.Cell("H1").Value = "기준";
                worksheet.Cell("I1").Value = "단위";

                worksheet.Cell("J1").Value = "불량수량";
                worksheet.Cell("K1").Value = "발생유형";
                worksheet.Cell("L1").Value = "LOT";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.defectiveDate.ToString();
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.defectiveCode.ToString();
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.defectiveName.ToString();
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.productCode.ToString();
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.productClassification.ToString();
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.productName.ToString();
                    worksheet.Cell("H" + (i + 1).ToString()).Value = res.productStandard.ToString();
                    worksheet.Cell("I" + (i + 1).ToString()).Value = res.productUnit.ToString();

                    worksheet.Cell("J" + (i + 1).ToString()).Value = res.produceDefectiveQuantity.ToString();
                    worksheet.Cell("K" + (i + 1).ToString()).Value = res.type.ToString();
                    worksheet.Cell("L" + (i + 1).ToString()).Value = res.productLOT.ToString();
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }


        [HttpPost("/api/produce/production-manage-by-month/download")]
        public async Task<IActionResult> ProductionManageByMonth([FromBody] GetProductionManageByMonthRequest001 req)
        {
            var result = await _producePlanRepo.GetProductionManageByMonth(req);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "월별 생산량 관리.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("월별 생산량관리");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "품목코드";
                worksheet.Cell("C1").Value = "품목구분";
                worksheet.Cell("D1").Value = "품목이름";
                worksheet.Cell("E1").Value = "규격";
                worksheet.Cell("F1").Value = "단위";
                worksheet.Cell("G1").Value = "1";
                worksheet.Cell("H1").Value = "2";
                worksheet.Cell("I1").Value = "3";
                worksheet.Cell("J1").Value = "4";
                worksheet.Cell("K1").Value = "5";
                worksheet.Cell("L1").Value = "6";
                worksheet.Cell("M1").Value = "7";
                worksheet.Cell("N1").Value = "8";
                worksheet.Cell("O1").Value = "9";
                worksheet.Cell("P1").Value = "10";
                worksheet.Cell("Q1").Value = "11";
                worksheet.Cell("R1").Value = "12";
                worksheet.Cell("S1").Value = "합계";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.productCode.ToString();
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.productClassification.ToString();
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.productName.ToString();
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.productStandard.ToString();
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.productUnit.ToString();
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.jan.ToString();
                    worksheet.Cell("H" + (i + 1).ToString()).Value = res.feb.ToString();
                    worksheet.Cell("I" + (i + 1).ToString()).Value = res.mar.ToString();
                    worksheet.Cell("J" + (i + 1).ToString()).Value = res.apr.ToString();
                    worksheet.Cell("K" + (i + 1).ToString()).Value = res.may.ToString();
                    worksheet.Cell("L" + (i + 1).ToString()).Value = res.jun.ToString();
                    worksheet.Cell("M" + (i + 1).ToString()).Value = res.july.ToString();
                    worksheet.Cell("N" + (i + 1).ToString()).Value = res.aug.ToString();
                    worksheet.Cell("O" + (i + 1).ToString()).Value = res.sep.ToString();
                    worksheet.Cell("P" + (i + 1).ToString()).Value = res.oct.ToString();
                    worksheet.Cell("Q" + (i + 1).ToString()).Value = res.nov.ToString();
                    worksheet.Cell("R" + (i + 1).ToString()).Value = res.dec.ToString();
                    worksheet.Cell("S" + (i + 1).ToString()).Value = res.total.ToString();
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
