using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.Lots;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Services.Repositories.Quality
{
    public class ProcessCheckDefectiveMngRepository : IProcessCheckDefectiveMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public ProcessCheckDefectiveMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }



        public async Task<Response<IEnumerable<ProcessCheckDefectiveRes001>>> processCheckDefectiveMstList(ProcessCheckDefectiveReq001 req)
        {
            try
            {
                var res = await _db.WorkerOrderProducePlans
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkerOrder.WorkOrderDate >= Convert.ToDateTime(req.workOrderStartDate))
                    .Where(x => x.WorkerOrder.WorkOrderDate <= Convert.ToDateTime(req.workOrderEndDate))
                    .Where(x => x.WorkerOrder.WorkOrderNo.Contains(req.workOrderNo))
                    .Where(x => req.processId == 0 ? true : x.ProducePlansProcess.ProductProcess.Process.Id == req.processId)
                    .Where(x => x.ProcessProgress.WorkOrderProducePlan.Register.FullName.Contains(req.workerName))
                    .Where(x => req.processCheckResult == 5 ? true : x.ProcessCheckResult == req.processCheckResult)
                    .OrderBy(x=> x.WorkerOrder.WorkOrderNo)

                    .Select(x => new ProcessCheckDefectiveRes001
                    {
                        workerOrderProducePlanId = x.WorkerOrderProducePlanId,

                        workOrderNo = x.WorkerOrder.WorkOrderNo,
                        workOrderDate = x.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),

                        processCode = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null? 
                            x.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y=>y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y=>y.ProductProcess.Process.Code).FirstOrDefault(),

                        processName = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),

                        isOutSourcing = x.InOutSourcing,
                        partnerName = x.Partner.Name,
                        facilitiesCode = x.Facility.Code,
                        facilitiesName = x.Facility.Name,
                        moldCode = x.Mold.Code,
                        moldName = x.Mold.Name,

                        workerName = x.ProcessProgress.WorkOrderProducePlan.Register.FullName,

                        productionQuantity = x.ProcessProgress != null? x.ProcessProgress.ProductionQuantity : 0,
                        productDefectiveQuantity = _db.ProcessDefectives
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.ProcessProgress.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId)
                            .Select(y=>y.DefectiveCount).Sum(),
                        productGoodQuantity = x.ProcessProgress != null ? x.ProcessProgress.ProductionQuantity - _db.ProcessDefectives
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.ProcessProgress.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId)
                            .Select(y => y.DefectiveCount).Sum() : 0,

                        processCheck = x.ProcessCheck == 0? false : true,
                        processCheckResult = x.ProcessCheckResult,
                        productLOT = _db.Lots
                            .Where(y => y.ProcessProgress.ProcessProgressId == x.ProcessProgress.ProcessProgressId)
                            .Where(y => y.IsDeleted == 0)
                            .Where(y=>y.ProcessType == "P")
                            .Select(y => y.LotName).FirstOrDefault() ?? "",
                        workProcessMemo = x.WorkProcessMemo ?? "",
                        dateFlag = x.WorkerOrder.WorkOrderDate
                    })
                    .ToListAsync();



                var res2 = res.Where(x => x.productLOT.Contains(req.productLOT)).Where(x => x.productDefectiveQuantity > 0);

                //LOT으로 필터링 한번더. 


                var Res = new Response<IEnumerable<ProcessCheckDefectiveRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res2
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<ProcessCheckDefectiveRes001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<ProcessCheckDefectiveRes002>> processCheckDefectiveMstPop(ProcessCheckDefectiveReq001 req)
        {
            try
            {
                var res = await _db.WorkerOrderProducePlans
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkerOrderProducePlanId == req.workerOrderProducePlanId)
                    .Select(x => new ProcessCheckDefectiveRes002
                    {
                        workerOrderProducePlanId = x.WorkerOrderProducePlanId,
                        workOrderNo = x.WorkerOrder.WorkOrderNo,
                        workOrderDate = x.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),
                        workStartDateTime = x.ProcessProgress.WorkStartDateTime != null ? x.ProcessProgress.WorkStartDateTime.ToString("yyyy-MM-dd HH:mm") : "-",
                        workEndDateTime = x.ProcessProgress.WorkEndDateTime != null ? x.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd HH:mm") : "-",

                        processCode = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),

                        processName = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),


                        isOutSourcing = x.InOutSourcing,
                        partnerName = x.Partner.Name,
                        
                        facilitiesCode = x.Facility.Code,
                        facilitiesName = x.Facility.Name,
                        moldCode = x.Mold.Code,
                        moldName = x.Mold.Name,

                        workerName = x.ProcessProgress.WorkOrderProducePlan.Register.FullName,

                        productName = x.Product.Name,
                        productCode = x.Product.Code,
                        productClassification = x.Product.CommonCode.Name,
                        productUnit = x.Product.Unit,
                        productStandard = x.Product.Standard,

                        productionQuantity = x.ProcessProgress != null ? x.ProcessProgress.ProductionQuantity : 0,
                        productDefectiveQuantity = _db.ProcessDefectives
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.ProcessProgress.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId)
                            .Select(y => y.DefectiveCount).Sum(),
                        productGoodQuantity = x.ProcessProgress != null ? x.ProcessProgress.ProductionQuantity - _db.ProcessDefectives
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.ProcessProgress.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId)
                            .Select(y => y.DefectiveCount).Sum() : 0,


                        processCheck = x.ProcessCheck == 1? true : false,
                        processCheckResult = x.ProcessCheckResult,
                        productLOT = _db.Lots
                            .Where(y => y.ProcessProgress.ProcessProgressId == x.ProcessProgress.ProcessProgressId)
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.ProcessType == "P")
                            .Select(y => y.LotName).FirstOrDefault() ?? "",

                        workProcessMemo = x.WorkProcessMemo
                    })
                    .FirstOrDefaultAsync();


                var Res = new Response<ProcessCheckDefectiveRes002>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<ProcessCheckDefectiveRes002>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<ProcessCheckDefectiveRes003>>> processCheckDefectiveList(ProcessCheckDefectiveReq001 req)
        {
            try
            {
                var res = await _db.ProcessDefectives
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.ProcessProgress.WorkerOrderProducePlanId == req.workerOrderProducePlanId)
                    .Select(x => new ProcessCheckDefectiveRes003
                    {
                        defectiveCode = x.Defective.Code,
                        defectiveName = x.Defective.Name,
                        defectiveQuantity = x.DefectiveCount,
                        defectiveProductMemo = x.DefectiveProductMemo,
                    })
                    .ToListAsync();
                
                var Res = new Response<IEnumerable<ProcessCheckDefectiveRes003>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<ProcessCheckDefectiveRes003>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }




    }
}
