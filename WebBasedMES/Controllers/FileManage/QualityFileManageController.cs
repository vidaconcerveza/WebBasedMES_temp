using ClosedXML.Excel;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using WebBasedMES.Services.Repositories.Quality;
using WebBasedMES.ViewModels.BaseInfo;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Controllers.QualityManage
{
    public class QualityFileManageController : ControllerBase
    {
        private readonly ILogger<FaultyManageController> _logger;
        private readonly IFaultyMngRepository _repo;
        private readonly IImportCheckMngRepository _importCheckRepo;
        private readonly IProcessCheckMngRepository _processCheckRepo;
        private readonly IOutOrderCheckMngRepository _outOrderCheckRepo;
        private readonly IOutOrderCheckDefectiveMngRepository _outOrderDefectiveRepo;
        private readonly IStoreCheckDefectiveMngRepository _importDefectiveRepo;
        private readonly IProcessCheckDefectiveMngRepository _processDefectiveRepo;
        private readonly IProductDefectiveMngRepository _productDefectiveRepo;
        private readonly IVoltageCheckMngRepository _voltageRepo;

        public QualityFileManageController(ILogger<FaultyManageController> logger, 
            IFaultyMngRepository repo, 
            IImportCheckMngRepository importCheckRepo,
            IProcessCheckMngRepository processCheckRepo,
            IProcessCheckDefectiveMngRepository processDefectiveRepo,
            IOutOrderCheckMngRepository outOrderCheckRepo,
            IOutOrderCheckDefectiveMngRepository outOrderDefectiveRepo,
            IStoreCheckDefectiveMngRepository importDefectiveRepo,
            IProductDefectiveMngRepository productDefectiveRepo,
            IVoltageCheckMngRepository voltageRepo)
        {
            _logger = logger;
            _repo = repo;
            _importCheckRepo = importCheckRepo;
            _importDefectiveRepo = importDefectiveRepo;

            _processCheckRepo = processCheckRepo;
            _processDefectiveRepo = processDefectiveRepo;
            _outOrderCheckRepo = outOrderCheckRepo;
            _outOrderDefectiveRepo = outOrderDefectiveRepo;

            _productDefectiveRepo = productDefectiveRepo;

            _voltageRepo = voltageRepo;
        }

        //new) 1) 기타불량등록
        [HttpPost("/api/quality/faulty-master/download")]
        public async Task<IActionResult> faultyMstList([FromBody] FaultyReq001 faultyReq001)
        {

            var result = await _repo.faultyMstList(faultyReq001);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "기타불량유형.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("기타불량유형");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "불량발생일";
                worksheet.Cell("C1").Value = "불량코드";
                worksheet.Cell("D1").Value = "불량유형";
                worksheet.Cell("E1").Value = "품목코드";
                worksheet.Cell("F1").Value = "품목구분";
                worksheet.Cell("G1").Value = "품목이름";
                worksheet.Cell("H1").Value = "규격";
                worksheet.Cell("I1").Value = "단위";
                worksheet.Cell("J1").Value = "기준단위";
                worksheet.Cell("K1").Value = "LOT";
                worksheet.Cell("L1").Value = "비고";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.defectiveDate;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.defectiveCode;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.defectiveName;
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.productCode;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.productClassification;
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.productName;
                    worksheet.Cell("H" + (i + 1).ToString()).Value = res.productStandard;
                    worksheet.Cell("I" + (i + 1).ToString()).Value = res.productUnit;
                    worksheet.Cell("J" + (i + 1).ToString()).Value = res.productStandardUnit;
                    worksheet.Cell("K" + (i + 1).ToString()).Value = res.productLOT;
                    worksheet.Cell("L" + (i + 1).ToString()).Value = res.defectiveProductMemo;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/quality/material-in-inspection-record/download")]
        public async Task<IActionResult> importCheckMstList([FromBody] ImportCheckReq001 importCheckReq001)
        {
            var result = await _importCheckRepo.importCheckMstList(importCheckReq001);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "수입검사현황.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("수입검사현황");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "입고번호";
                worksheet.Cell("C1").Value = "입고일";
                worksheet.Cell("D1").Value = "거래처";
                worksheet.Cell("E1").Value = "품목코드";
                worksheet.Cell("F1").Value = "품목구분";
                worksheet.Cell("G1").Value = "품목이름";
                worksheet.Cell("H1").Value = "규격";
                worksheet.Cell("I1").Value = "구매단위";
                worksheet.Cell("J1").Value = "입고수량";
                worksheet.Cell("K1").Value = "LOT";
                worksheet.Cell("L1").Value = "수입검사결과";
                worksheet.Cell("M1").Value = "등록자";
                worksheet.Cell("N1").Value = "비고";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.receivingNo;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.receivingDate;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.partnerName;
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.productCode;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.productClassification;
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.productName;
                    worksheet.Cell("H" + (i + 1).ToString()).Value = res.productStandard;
                    worksheet.Cell("I" + (i + 1).ToString()).Value = res.productStandardUnit;
                    worksheet.Cell("J" + (i + 1).ToString()).Value = res.productReceivingCount.ToString();
                    worksheet.Cell("K" + (i + 1).ToString()).Value = res.productLOT;
                    worksheet.Cell("L" + (i + 1).ToString()).Value = res.productImportCheckResult == 0 ? "합격" : (res.productImportCheckResult == 1 ? "부분합격": (res.productImportCheckResult == 2 ? "불합격" :"미검사"));
                    worksheet.Cell("M" + (i + 1).ToString()).Value = res.registerName;
                    worksheet.Cell("N" + (i + 1).ToString()).Value = res.receivingProductMemo;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/quality/process-inspection-record/download")]
        public async Task<IActionResult> processCheckMstList([FromBody] ProcessCheckReq001 processCheckReq001)
        {
            var result = await _processCheckRepo.processCheckMstList(processCheckReq001);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "공정검사현황.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("공정검사현황");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "작업지시번호";
                worksheet.Cell("C1").Value = "작업지시일";
                worksheet.Cell("D1").Value = "공정코드";
                worksheet.Cell("E1").Value = "공정이름";
                worksheet.Cell("F1").Value = "자체/외주";
                worksheet.Cell("G1").Value = "거래처";
                worksheet.Cell("H1").Value = "설비코드";
                worksheet.Cell("I1").Value = "설비이름";
                worksheet.Cell("J1").Value = "작업자";
                worksheet.Cell("K1").Value = "생산수량";
                worksheet.Cell("L1").Value = "공정검사결과";
                worksheet.Cell("M1").Value = "LOT";
                worksheet.Cell("N1").Value = "비고";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.workOrderNo;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.workOrderDate;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.processCode;
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.processName;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.isOutSourcing == 0 ? "자체" :"외주";
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.partnerName;
                    worksheet.Cell("H" + (i + 1).ToString()).Value = res.facilitiesCode;
                    worksheet.Cell("I" + (i + 1).ToString()).Value = res.facilitiesName;
                    worksheet.Cell("J" + (i + 1).ToString()).Value = res.workerName;
                    worksheet.Cell("K" + (i + 1).ToString()).Value = res.productionQuantity;
                    worksheet.Cell("L" + (i + 1).ToString()).Value = res.processCheckResult == 0 ? "합격" : (res.processCheckResult == 1 ? "부분합격" : (res.processCheckResult == 2 ? "불합격" : "미검사"));
                    worksheet.Cell("M" + (i + 1).ToString()).Value = res.productLOT;
                    worksheet.Cell("N" + (i + 1).ToString()).Value = res.workProcessMemo;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }


        [HttpPost("/api/quality/product-out-inspection-record/download")]
        public async Task<IActionResult> outOrderCheckMstList([FromBody] OutOrderCheckReq001 outOrderCheckReq001)
        {

            var result = await _outOrderCheckRepo.outOrderCheckMstList(outOrderCheckReq001);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "출하검사현황.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("출하검사현황");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "출고번호";
                worksheet.Cell("C1").Value = "출고일";
                worksheet.Cell("D1").Value = "거래처코드";
                worksheet.Cell("E1").Value = "거래처이름";
                worksheet.Cell("F1").Value = "품목코드";
                worksheet.Cell("G1").Value = "품목구분";
                worksheet.Cell("H1").Value = "품목이름";
                worksheet.Cell("I1").Value = "규격";
                worksheet.Cell("J1").Value = "단위";
                worksheet.Cell("K1").Value = "수량";
                worksheet.Cell("L1").Value = "LOT";
                worksheet.Cell("M1").Value = "출하검사결과";
                worksheet.Cell("N1").Value = "등록자";
                worksheet.Cell("O1").Value = "비고";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.shipmentNo;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.shipmentDate;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.partnerCode;
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.partnerName;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.productCode;
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.productClassification;
                    worksheet.Cell("H" + (i + 1).ToString()).Value = res.productName;
                    worksheet.Cell("I" + (i + 1).ToString()).Value = res.productStandard;
                    worksheet.Cell("J" + (i + 1).ToString()).Value = res.productUnit;
                    worksheet.Cell("K" + (i + 1).ToString()).Value = res.quantity;
                    worksheet.Cell("L" + (i + 1).ToString()).Value = res.productLOT;
                    worksheet.Cell("M" + (i + 1).ToString()).Value = res.productShipmentCheckResult == 0 ? "합격" : (res.productShipmentCheckResult == 1 ? "부분합격" : (res.productShipmentCheckResult == 2 ? "불합격" : "미검사"));
                    worksheet.Cell("N" + (i + 1).ToString()).Value = res.registerName;
                    worksheet.Cell("O" + (i + 1).ToString()).Value = res.shipmentProductMemo;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }


        [HttpPost("/api/quality/material-in-inspection-defective-record/download")]
        public async Task<IActionResult> storeCheckDefectiveMstList([FromBody] StoreCheckDefectiveReq001 storeCheckDefectiveReq001)
        {

            var result = await _importDefectiveRepo.storeCheckDefectiveMstList(storeCheckDefectiveReq001);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "수입검사불량현황.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("수입검사불량현황");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "입고번호";
                worksheet.Cell("C1").Value = "입고일";
                worksheet.Cell("D1").Value = "거래처";
                worksheet.Cell("E1").Value = "품목코드";
                worksheet.Cell("F1").Value = "품목구분";
                worksheet.Cell("G1").Value = "품목이름";
                worksheet.Cell("H1").Value = "규격";
                worksheet.Cell("I1").Value = "구매단위";
                worksheet.Cell("J1").Value = "입고수량";
                worksheet.Cell("K1").Value = "양품수량";
                worksheet.Cell("L1").Value = "불량수량";
                worksheet.Cell("M1").Value = "LOT";
                worksheet.Cell("N1").Value = "수입검사여부";
                worksheet.Cell("O1").Value = "수입검사결과";
                worksheet.Cell("P1").Value = "등록자";
                worksheet.Cell("Q1").Value = "비고";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.receivingNo;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.receivingDate;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.partnerName;
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.productCode;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.productClassification;
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.productName;
                    worksheet.Cell("H" + (i + 1).ToString()).Value = res.productStandard;
                    worksheet.Cell("I" + (i + 1).ToString()).Value = res.productStandardUnit;
                    worksheet.Cell("J" + (i + 1).ToString()).Value = res.productReceivingCount;
                    worksheet.Cell("K" + (i + 1).ToString()).Value = res.productReceivingCount - res.productDefectiveQuantity;
                    worksheet.Cell("L" + (i + 1).ToString()).Value = res.productDefectiveQuantity;
                    worksheet.Cell("M" + (i + 1).ToString()).Value = res.productLOT;
                    worksheet.Cell("N" + (i + 1).ToString()).Value = res.productImportCheck? "검사" : "미검사";
                    worksheet.Cell("O" + (i + 1).ToString()).Value = res.productImportCheckResult == 0 ? "합격" : (res.productImportCheckResult == 1 ? "부분합격" : (res.productImportCheckResult == 2 ? "불합격" : "미검사"));
                    worksheet.Cell("P" + (i + 1).ToString()).Value = res.registerName;
                    worksheet.Cell("Q" + (i + 1).ToString()).Value = res.receivingProductMemo;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        // 1) 공정검사 불량현황 메인화면
        [HttpPost("/api/quality/process-inspection-defective-record/download")]
        public async Task<IActionResult> processCheckDefectiveMstList([FromBody] ProcessCheckDefectiveReq001 processCheckDefectiveReq001)
        {

            var result = await _processDefectiveRepo.processCheckDefectiveMstList(processCheckDefectiveReq001);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "공정검사불량현황.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("공정검사불량현황");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "작업지시번호";
                worksheet.Cell("C1").Value = "작업지시일";
                worksheet.Cell("D1").Value = "공정코드";
                worksheet.Cell("E1").Value = "공정이름";
                worksheet.Cell("F1").Value = "자체/외주";
                worksheet.Cell("G1").Value = "거래처";
                worksheet.Cell("H1").Value = "설비코드";
                worksheet.Cell("I1").Value = "설비이름";
                worksheet.Cell("J1").Value = "작업자";
                worksheet.Cell("K1").Value = "생산수량";
                worksheet.Cell("L1").Value = "양품수량";
                worksheet.Cell("M1").Value = "불량수량";
                worksheet.Cell("N1").Value = "공정검사여부";
                worksheet.Cell("O1").Value = "공정검사결과";
                worksheet.Cell("P1").Value = "LOT";
                worksheet.Cell("Q1").Value = "비고";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.workOrderNo;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.workOrderDate;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.processCode;
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.processName;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.isOutSourcing==1? "외주":"자체";
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.partnerName;
                    worksheet.Cell("H" + (i + 1).ToString()).Value = res.facilitiesCode;
                    worksheet.Cell("I" + (i + 1).ToString()).Value = res.facilitiesName;
                    worksheet.Cell("J" + (i + 1).ToString()).Value = res.workerName;
                    worksheet.Cell("K" + (i + 1).ToString()).Value = res.productionQuantity;
                    worksheet.Cell("L" + (i + 1).ToString()).Value = res.productionQuantity - res.productDefectiveQuantity;
                    worksheet.Cell("M" + (i + 1).ToString()).Value = res.productDefectiveQuantity;
                    worksheet.Cell("N" + (i + 1).ToString()).Value = res.processCheck ? "검사" : "미검사";
                    worksheet.Cell("O" + (i + 1).ToString()).Value = res.processCheckResult == 0 ? "합격" : (res.processCheckResult == 1 ? "부분합격" : (res.processCheckResult == 2 ? "불합격" : "미검사"));
                    worksheet.Cell("P" + (i + 1).ToString()).Value = res.productLOT;
                    worksheet.Cell("Q" + (i + 1).ToString()).Value = res.workProcessMemo;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/quality/product-out-inspection-defective-record/download")]
        public async Task<IActionResult> outOrderCheckDefectiveMstList([FromBody] OutOrderCheckDefectiveReq001 outOrderCheckDefectiveReq001)
        {

            var result = await _outOrderDefectiveRepo.outOrderCheckDefectiveMstList(outOrderCheckDefectiveReq001);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "출하검사불량현황.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("출하검사불량현황");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "출고번호";
                worksheet.Cell("C1").Value = "출고일";
                worksheet.Cell("D1").Value = "거래처";
                worksheet.Cell("E1").Value = "품목코드";
                worksheet.Cell("F1").Value = "품목구분";
                worksheet.Cell("G1").Value = "품목이름";
                worksheet.Cell("H1").Value = "규격";
                worksheet.Cell("I1").Value = "단위";
                worksheet.Cell("J1").Value = "수량";
                worksheet.Cell("K1").Value = "불량수량";
                worksheet.Cell("L1").Value = "LOT";
                worksheet.Cell("M1").Value = "출하검사여부";
                worksheet.Cell("N1").Value = "출하검사결과";
                worksheet.Cell("O1").Value = "등록자";
                worksheet.Cell("P1").Value = "비고";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.shipmentNo;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.shipmentDate;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.partnerName;
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.productCode;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.productClassification;
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.productName;
                    worksheet.Cell("H" + (i + 1).ToString()).Value = res.productStandard;
                    worksheet.Cell("I" + (i + 1).ToString()).Value = res.productUnit;
                    worksheet.Cell("J" + (i + 1).ToString()).Value = res.quantity;
                    worksheet.Cell("K" + (i + 1).ToString()).Value = res.productDefectiveQuantity;
                    worksheet.Cell("L" + (i + 1).ToString()).Value = res.productLOT;
                    worksheet.Cell("M" + (i + 1).ToString()).Value = res.productShipmentCheck ? "검사":"미검사";
                    worksheet.Cell("N" + (i + 1).ToString()).Value = res.productShipmentCheckResult == 0 ? "합격" : (res.productShipmentCheckResult == 1 ? "부분합격" : (res.productShipmentCheckResult == 2 ? "불합격" : "미검사"));
                    worksheet.Cell("O" + (i + 1).ToString()).Value = res.registerName;
                    worksheet.Cell("P" + (i + 1).ToString()).Value = res.shipmentProductMemo;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/quality/productDefective/mst/list")]
        public async Task<IActionResult> productDefectiveMstList([FromBody] ProductDefectiveReq001 productDefectiveReq001)
        {

            var result = await _productDefectiveRepo.productDefectiveMstList(productDefectiveReq001);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "제품별불량현황.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("제품별불량현황");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "품목코드";
                worksheet.Cell("C1").Value = "품목구분";
                worksheet.Cell("D1").Value = "품목이름";
                worksheet.Cell("E1").Value = "규격";
                worksheet.Cell("F1").Value = "단위";
                worksheet.Cell("G1").Value = "LOT";
                worksheet.Cell("H1").Value = "발생유형";
                worksheet.Cell("I1").Value = "불량코드";
                worksheet.Cell("J1").Value = "불량유형";
                worksheet.Cell("K1").Value = "불량수량";
                worksheet.Cell("L1").Value = "기준단위";
                worksheet.Cell("M1").Value = "비고";
                worksheet.Cell("N1").Value = "등록자";
                worksheet.Cell("O1").Value = "일자";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.productCode;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.productClassification;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.productName;
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.productStandard;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.productUnit;
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.productLOT;
                    worksheet.Cell("H" + (i + 1).ToString()).Value = res.type;
                    worksheet.Cell("I" + (i + 1).ToString()).Value = res.defectiveCode;
                    worksheet.Cell("J" + (i + 1).ToString()).Value = res.defectiveName;
                    worksheet.Cell("K" + (i + 1).ToString()).Value = res.productDefectiveQuantity;
                    worksheet.Cell("L" + (i + 1).ToString()).Value = res.productStandardUnit;
                    worksheet.Cell("M" + (i + 1).ToString()).Value = res.defectiveProductMemo;
                    worksheet.Cell("N" + (i + 1).ToString()).Value = res.registerName;
                    worksheet.Cell("O" + (i + 1).ToString()).Value = res.defectiveDate;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }
        

       [HttpPost("/api/quality/voltage-inspection-manage/download")]
       public async Task<IActionResult> QualityVoltageCheckManage([FromBody] VoltageInspectionRequest voltageCheckReq001)
       {

           var result = await _voltageRepo.GetVoltageInspections(voltageCheckReq001);
           string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
           string fileName = "전압검사.xlsx";

           using (var workbook = new XLWorkbook())
           {
               IXLWorksheet worksheet = workbook.Worksheets.Add("전압검사");

               worksheet.Cell("A1").Value = "No";
               worksheet.Cell("B1").Value = "검사일";
               worksheet.Cell("C1").Value = "제품코드";
               worksheet.Cell("D1").Value = "제품구분";
               worksheet.Cell("E1").Value = "제품이름";
               worksheet.Cell("F1").Value = "LOT";
               worksheet.Cell("G1").Value = "프레스 TON";
               worksheet.Cell("H1").Value = "메인모터전류";
               worksheet.Cell("I1").Value = "슬라이드 전류";
               worksheet.Cell("J1").Value = "모터 SPM";
               worksheet.Cell("K1").Value = "밀림 SPL";
               worksheet.Cell("L1").Value = "메인모터 과전류";
               worksheet.Cell("M1").Value = "슬라이드모터과전류";
               worksheet.Cell("N1").Value = "프레스 최대 SPM";
               worksheet.Cell("O1").Value = "실제 로드셀 톤값";
               worksheet.Cell("P1").Value = "A1 속도";
               worksheet.Cell("Q1").Value = "A1 밀림";
               worksheet.Cell("R1").Value = "A2 속도";
               worksheet.Cell("S1").Value = "A2 밀림";
               worksheet.Cell("T1").Value = "A3 속도";
               worksheet.Cell("U1").Value = "A3 밀림";
               worksheet.Cell("V1").Value = "A4 속도";
               worksheet.Cell("W1").Value = "A5 밀림";

               int i = 0;
               foreach (var res in result.Data)
               {
                   i++;
                   worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                   worksheet.Cell("B" + (i + 1).ToString()).Value = res.InspectionDate;
                   worksheet.Cell("C" + (i + 1).ToString()).Value = res.ProductCode;
                   worksheet.Cell("D" + (i + 1).ToString()).Value = res.ProductClassification;
                   worksheet.Cell("E" + (i + 1).ToString()).Value = res.ProductName;
                   worksheet.Cell("F" + (i + 1).ToString()).Value = res.Lot;
                   worksheet.Cell("G" + (i + 1).ToString()).Value = res.Ton;
                   worksheet.Cell("H" + (i + 1).ToString()).Value = res.MainMotorAmp;
                   worksheet.Cell("I" + (i + 1).ToString()).Value = res.SlideAmp;
                   worksheet.Cell("J" + (i + 1).ToString()).Value = res.MotorSpm;
                   worksheet.Cell("K" + (i + 1).ToString()).Value = "";
                   worksheet.Cell("L" + (i + 1).ToString()).Value = res.MainMotorOverAmp;
                   worksheet.Cell("M" + (i + 1).ToString()).Value = res.SlideMotorOverAmp;
                   worksheet.Cell("N" + (i + 1).ToString()).Value = res.PressMaxSpm;
                   worksheet.Cell("O" + (i + 1).ToString()).Value = res.LoadCell;
                   worksheet.Cell("P" + (i + 1).ToString()).Value = res.A1Speed;
                   worksheet.Cell("Q" + (i + 1).ToString()).Value = res.A1Slp;
                   worksheet.Cell("R" + (i + 1).ToString()).Value = res.A2Speed;
                   worksheet.Cell("S" + (i + 1).ToString()).Value = res.A2Slp;
                   worksheet.Cell("T" + (i + 1).ToString()).Value = res.A3Speed;
                   worksheet.Cell("U" + (i + 1).ToString()).Value = res.A3Slp;
                   worksheet.Cell("V" + (i + 1).ToString()).Value = res.A4Speed;
                   worksheet.Cell("W" + (i + 1).ToString()).Value = res.A4Slp;
               }
               using (var stream = new MemoryStream())
               {
                   workbook.SaveAs(stream);
                   var content = stream.ToArray();
                   return File(content, contentType, fileName);
               }
           }
       }


       [HttpPost("/api/quality/temp-humid-manage/download")]
       public async Task<IActionResult> GetQualityTempHumids([FromBody] TempHumidRequest req)
       {

           var result = await _voltageRepo.GetTempHumids(req);
           string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
           string fileName = "온습도관리.xlsx";

           using (var workbook = new XLWorkbook())
           {
               IXLWorksheet worksheet = workbook.Worksheets.Add("온습도관리");

               worksheet.Cell("A1").Value = "No";
               worksheet.Cell("B1").Value = "일시";
               worksheet.Cell("C1").Value = "설비코드";
               worksheet.Cell("D1").Value = "설비구분";
               worksheet.Cell("E1").Value = "설비이름";
               worksheet.Cell("F1").Value = "현재온도";
               worksheet.Cell("G1").Value = "현재습도";
               worksheet.Cell("H1").Value = "온도상한";
               worksheet.Cell("I1").Value = "온도하한";


               int i = 0;
               foreach (var res in result.Data)
               {
                   i++;
                   worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                   worksheet.Cell("B" + (i + 1).ToString()).Value = res.Date;
                   worksheet.Cell("C" + (i + 1).ToString()).Value = res.FacilityCode;
                   worksheet.Cell("D" + (i + 1).ToString()).Value = res.FacilityType;
                   worksheet.Cell("E" + (i + 1).ToString()).Value = res.FacilityName;
                   worksheet.Cell("F" + (i + 1).ToString()).Value = res.Temp;
                   worksheet.Cell("G" + (i + 1).ToString()).Value = res.Humid;
                   worksheet.Cell("H" + (i + 1).ToString()).Value = res.Temp_UL;
                   worksheet.Cell("I" + (i + 1).ToString()).Value = res.Temp_LL;

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
