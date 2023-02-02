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
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.BaseInfo;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;
using WebBasedMES.ViewModels.Lot;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Controllers.QualityManage
{
    public class InOutFileManageController : ControllerBase
    {
        private readonly ILogger<InOutFileManageController> _logger;
        
        private readonly IOrderMngRepository _orderRepo;
        private readonly IStoreMngRepository _storeMngRepo;
        private readonly IInvenMngRepository _invenMngRepo;
        private readonly IOutMngRepository _outMngRepo;
        private readonly IOutStoreMngRepository _outStoreMngRepository;
        private readonly IOutOrderMngRepository _outOrderMngRepository;
        private readonly ILotMngRepository _lotMngRepository;

        

        public InOutFileManageController(ILogger<InOutFileManageController> logger, 
            IOrderMngRepository orderRepo,
            IStoreMngRepository storeMngRepo,
            IInvenMngRepository invenMngRepo,
            IOutMngRepository outMngRepo,
            IOutStoreMngRepository outStoreMngRepository,
            IOutOrderMngRepository outOrderMngRepository,
            ILotMngRepository lotMngRepository
            )
        {
            _logger = logger;
            _orderRepo = orderRepo;
            _storeMngRepo = storeMngRepo;
            _outMngRepo = outMngRepo;
            _invenMngRepo = invenMngRepo;
            _outStoreMngRepository = outStoreMngRepository;
            _outOrderMngRepository = outOrderMngRepository;
            _lotMngRepository = lotMngRepository;
        }

        [HttpPost("/api/inAndOut/contract-register/download")]
        public async Task<IActionResult> ContractMasterList([FromBody] OrderReq001 orderRequest)
        {
            var result = await _orderRepo.orderMstList(orderRequest);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "수주현황.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("수주현황");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "수주번호";
                worksheet.Cell("C1").Value = "등록일";
                worksheet.Cell("D1").Value = "납품요청일";
                worksheet.Cell("E1").Value = "거래처";
                worksheet.Cell("F1").Value = "과세정보";
                worksheet.Cell("G1").Value = "공급가액";
                worksheet.Cell("H1").Value = "세액";
                worksheet.Cell("I1").Value = "총금액";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.orderNo.ToString();
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.orderDate.ToString();
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.requestDeliveryDate.ToString();
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.partnerName.ToString();
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.partnerTaxInfo.ToString();
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.orderSellPrice.ToString();
                    worksheet.Cell("H" + (i + 1).ToString()).Value = (res.orderSellPrice * 0.1).ToString("0");
                    worksheet.Cell("I" + (i + 1).ToString()).Value = (res.orderSellPrice * 1.1).ToString("0");
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }


        //발주요청
        [HttpPost("/api/inAndOut/order-register/download")]
        public async Task<IActionResult> OutStoreMasterList([FromBody] OutStoreReq001 outStoreRequest)
        {
            var result = await _outStoreMngRepository.outStoreMstList(outStoreRequest);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "발주현황.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("발주현황");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "발주번호";
                worksheet.Cell("C1").Value = "등록일";
                worksheet.Cell("D1").Value = "납품요청일";
                worksheet.Cell("E1").Value = "거래처";
                worksheet.Cell("F1").Value = "과세정보";
                worksheet.Cell("G1").Value = "공급가액";
                worksheet.Cell("H1").Value = "세액";
                worksheet.Cell("I1").Value = "총금액";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.outStoreNo.ToString();
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.outStoreDate.ToString();
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.requestDeliveryDate.ToString();
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.partnerName.ToString();
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.partnerTaxInfo.ToString();
                    worksheet.Cell("G" + (i + 1).ToString()).Value = res.outStoreSupplyPrice.ToString();
                    worksheet.Cell("H" + (i + 1).ToString()).Value = (res.outStoreSupplyPrice * 0.1).ToString("0");
                    worksheet.Cell("I" + (i + 1).ToString()).Value = (res.outStoreSupplyPrice * 1.1).ToString("0");
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/inAndOut/income-register/download")]
        public async Task<IActionResult> StoreMstList([FromBody] StoreReq001 req)
        {
            var result = await _storeMngRepo.storeMstList(req);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "입고관리.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("입고관리");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "입고번호";
                worksheet.Cell("C1").Value = "등록일";
                worksheet.Cell("D1").Value = "거래처";
                worksheet.Cell("E1").Value = "과세정보";
                worksheet.Cell("F1").Value = "공급가액";
                worksheet.Cell("G1").Value = "세액";
                worksheet.Cell("H1").Value = "총금액";
                worksheet.Cell("I1").Value = "등록자";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.receivingNo.ToString();
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.receivingDate.ToString();
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.partnerName.ToString();
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.partnerTaxInfo.ToString();
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.receivingSupplyPrice.ToString();
                    worksheet.Cell("G" + (i + 1).ToString()).Value = (res.receivingSupplyPrice * 0.1).ToString();
                    worksheet.Cell("H" + (i + 1).ToString()).Value = (res.receivingSupplyPrice * 1.1).ToString("0");
                    worksheet.Cell("I" + (i + 1).ToString()).Value = (res.registerName).ToString();
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/inAndOut/income-record/download")]
        public async Task<IActionResult> StoreMstList3([FromBody] StoreReq001 req)
        {
            var result = await _storeMngRepo.storeMstList3(req);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "입고현황.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("입고현황");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "입고번호";
                worksheet.Cell("C1").Value = "등록일";
                worksheet.Cell("D1").Value = "거래처";
                worksheet.Cell("E1").Value = "과세정보";
                worksheet.Cell("F1").Value = "공급가액";
                worksheet.Cell("G1").Value = "세액";
                worksheet.Cell("H1").Value = "총금액";
                worksheet.Cell("I1").Value = "등록자";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.receivingNo.ToString();
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.receivingDate.ToString();
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.partnerName.ToString();
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.partnerTaxInfo.ToString();
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.receivingSupplyPrice.ToString();
                    worksheet.Cell("G" + (i + 1).ToString()).Value = (res.receivingSupplyPrice * 0.1).ToString();
                    worksheet.Cell("H" + (i + 1).ToString()).Value = (res.receivingSupplyPrice * 1.1).ToString("0");
                    worksheet.Cell("I" + (i + 1).ToString()).Value = (res.registerName).ToString();
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }


        [HttpPost("/api/inAndOut/material-use-record/download")]
        public async Task<IActionResult> MaterialUseRecord([FromBody] InvenMngModelReq0001 req)
        {
            var result = await _invenMngRepo.getProgressList(req);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "자재투입현황.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("자재투입현황");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "품목코드";
                worksheet.Cell("C1").Value = "품목구분";
                worksheet.Cell("D1").Value = "품목이름";
                worksheet.Cell("E1").Value = "규격";
                worksheet.Cell("F1").Value = "단위";
                worksheet.Cell("G1").Value = "작업지시번호";
                worksheet.Cell("H1").Value = "작업일자";
                worksheet.Cell("I1").Value = "공정코드";
                worksheet.Cell("J1").Value = "공정이름";
                worksheet.Cell("K1").Value = "설비코드";
                worksheet.Cell("L1").Value = "설비이름";
                worksheet.Cell("M1").Value = "소요량";
                worksheet.Cell("N1").Value = "LOSS";
                worksheet.Cell("O1").Value = "총소요량";
                worksheet.Cell("P1").Value = "생산수량";
                worksheet.Cell("Q1").Value = "합계";
                worksheet.Cell("R1").Value = "투입자재LOT";

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
                    worksheet.Cell("G" + (i + 1).ToString()).Value = (res.workOrderNo).ToString();
                    worksheet.Cell("H" + (i + 1).ToString()).Value = (res.workOrderDate).ToString();
                    worksheet.Cell("I" + (i + 1).ToString()).Value = (res.processCode).ToString();
                    worksheet.Cell("J" + (i + 1).ToString()).Value = (res.processName).ToString();
                    worksheet.Cell("K" + (i + 1).ToString()).Value = (res.facilitiesCode).ToString();
                    worksheet.Cell("L" + (i + 1).ToString()).Value = (res.facilitiesName).ToString();
                    //worksheet.Cell("M" + (i + 1).ToString()).Value = res.requiredQuantity.ToString("0.000");
                    worksheet.Cell("N" + (i + 1).ToString()).Value = (res.LOSS).ToString("0.000");
                   // worksheet.Cell("O" + (i + 1).ToString()).Value = (res.requiredQuantity + res.LOSS).ToString("0.000");
                    worksheet.Cell("P" + (i + 1).ToString()).Value = (res.productionQuantity).ToString();
                   // worksheet.Cell("Q" + (i + 1).ToString()).Value = ((res.requiredQuantity + res.LOSS)* res.productionQuantity).ToString("0.000");
                    worksheet.Cell("R" + (i + 1).ToString()).Value = (res.productLOT).ToString();
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }



        [HttpPost("/api/inAndOut/material-in-out-by-partner/download")]
        public async Task<IActionResult> MaterialInOutByPartner([FromBody] LotRequestCrud req)
        {
            var result = await _lotMngRepository.getLotCounts(req);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "거래처별자재입출고현황.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("거래처별자재입출고현황");

                worksheet.Cell("A1").Value = "No";
                worksheet.Cell("B1").Value = "거래처코드";
                worksheet.Cell("C1").Value = "거래처이름";
                worksheet.Cell("D1").Value = "유형";
                worksheet.Cell("E1").Value = "입출고번호";
                worksheet.Cell("F1").Value = "입출고일";
                worksheet.Cell("G1").Value = "품목코드";
                worksheet.Cell("H1").Value = "품목구분";
                worksheet.Cell("I1").Value = "품목이름";
                worksheet.Cell("J1").Value = "규격";
                worksheet.Cell("K1").Value = "단위";
                worksheet.Cell("L1").Value = "과세유형";
                worksheet.Cell("M1").Value = "기준단위";
                worksheet.Cell("N1").Value = "기준단위수량";
                worksheet.Cell("O1").Value = "입출고수량";
                worksheet.Cell("P1").Value = "단가";
                worksheet.Cell("Q1").Value = "공급가액";
                worksheet.Cell("R1").Value = "세액";
                worksheet.Cell("S1").Value = "총금액";
                worksheet.Cell("T1").Value = "LOT";

                int i = 0;
                foreach (var res in result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 1).ToString()).Value = res.partnerName.ToString();
                    worksheet.Cell("C" + (i + 1).ToString()).Value = res.partnerName.ToString();
                    worksheet.Cell("D" + (i + 1).ToString()).Value = res.type.ToString();
                    worksheet.Cell("E" + (i + 1).ToString()).Value = res.docuNo.ToString();
                    worksheet.Cell("F" + (i + 1).ToString()).Value = res.registerDate.ToString();
                    worksheet.Cell("G" + (i + 1).ToString()).Value = (res.productCode).ToString();
                    worksheet.Cell("H" + (i + 1).ToString()).Value = (res.productClassification).ToString();
                    worksheet.Cell("I" + (i + 1).ToString()).Value = (res.productName).ToString();
                    worksheet.Cell("J" + (i + 1).ToString()).Value = (res.productStandard).ToString();
                    worksheet.Cell("K" + (i + 1).ToString()).Value = (res.productUnit).ToString();
                    worksheet.Cell("L" + (i + 1).ToString()).Value = (res.productTaxInfo).ToString();
                    worksheet.Cell("M" + (i + 1).ToString()).Value = res.productStandardUnit.ToString();
                    worksheet.Cell("N" + (i + 1).ToString()).Value = (res.productStandardUnitCount).ToString();
                    worksheet.Cell("O" + (i + 1).ToString()).Value = (res.count).ToString();
                    worksheet.Cell("P" + (i + 1).ToString()).Value = (res.unitPrice).ToString();
                    worksheet.Cell("Q" + (i + 1).ToString()).Value = (res.supplyPrice).ToString();
                    worksheet.Cell("R" + (i + 1).ToString()).Value = (res.supplyPrice*0.1).ToString();
                    worksheet.Cell("S" + (i + 1).ToString()).Value = (res.supplyPrice * 1.1).ToString();
                    worksheet.Cell("T" + (i + 1).ToString()).Value = (res.productLOT).ToString();
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
