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
    public class ProcessCheckMngRepository : IProcessCheckMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public ProcessCheckMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }

     

        public async Task<Response<IEnumerable<ProcessCheckRes001>>> processCheckMstList(ProcessCheckReq001 req)
        {
            try
            {
                var res = await _db.WorkerOrderProducePlans
                    .Where(x => x.WorkerOrder.WorkOrderDate >= Convert.ToDateTime(req.workOrderStartDate))
                    .Where(x => x.WorkerOrder.WorkOrderDate <= Convert.ToDateTime(req.workOrderEndDate))
                    .Where(x => x.WorkerOrder.WorkOrderNo.Contains(req.workOrderNo))
                    .Where(x => x.Register.FullName.Contains(req.WorkerName))
                    .Where(x => req.processCheckResult == 5 ? true : x.ProcessCheckResult == req.processCheckResult)
                    .Where(x => req.processId == 0 ? true : x.ProducePlansProcess.ProductProcess.Process.Id == req.processId)
                    .Where(x =>x.IsDeleted == 0)
                    .OrderBy(x=>x.WorkerOrder.WorkOrderNo)
                    .Select(x => new ProcessCheckRes001
                    {
                        workOrderNo = x.WorkerOrder.WorkOrderNo,
                        processId = x.WorkerOrder.ProducePlansProduct != null? 
                            x.ProducePlansProcess.ProductProcess.Process.Id : 
                            _db.WorkerOrderWithoutPlans.Where(y=>y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y=>y.ProductProcess.ProcessId).FirstOrDefault(),

                        processCode = x.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Code ?? "" :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),
                        processName = x.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Name ?? "" :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),

                        partnerName = x.Partner.Name??"",
                        isOutSourcing = x.InOutSourcing,
                        
                        facilityId = x.Facility !=null ? x.Facility.Id : 0,
                        facilitiesCode = x.Facility.Code??"",
                        facilitiesName = x.Facility.Name??"",

                        moldId = x.Mold != null? x.Mold.Id : 0,
                        moldCode = x.Mold.Code ?? "",
                        moldName = x.Mold.Name ?? "",

                        processCheckResult = x.ProcessCheckResult,
                        workerOrderProducePlanId = x.WorkerOrderProducePlanId,
                        productionQuantity = x.ProcessProgress !=null? x.ProcessProgress.ProductionQuantity : 0,

                        productLOT = _db.Lots.Where(y=> y.ProcessProgress.ProcessProgressId == x.ProcessProgress.ProcessProgressId).Select(y=>y.LotName).FirstOrDefault() ?? "",
                        workOrderDate = x.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),
                        workerName = x.Register.FullName ?? "",
                        workProcessMemo = x.WorkProcessMemo ?? "",
                        workOrderId = x.WorkerOrder.WorkerOrderId,
                        flag = x.WorkerOrder.WorkOrderDate
                        
                    })
                    .ToListAsync();

                var res2 = res.Where(x => x.productLOT.Contains(req.productLOT));


                var Res = new Response<IEnumerable<ProcessCheckRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res2
                };

                return Res;
            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<ProcessCheckRes001>>()
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
