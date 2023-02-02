using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;

namespace WebBasedMES.Services.Repositories.InAndOut
{
    public class InvenMngRepository : IInvenMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        

        public InvenMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            
        }

        public async Task<Response<IEnumerable<InvenMngModelRes0001>>> getProgressList(InvenMngModelReq0001 req)
        {
            try
            {

                var res = await _db.LotCounts
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.Lot.ProcessType == "C")
                    .Where(x => x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderNo.Contains(req.workOrderNo))
                    .Where(x => x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderDate >= Convert.ToDateTime(req.workOrderStartDate) && x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderDate <= Convert.ToDateTime(req.workOrderEndDate))
                    .Where(x => x.Lot.LotName.Contains(req.productLOT))
                    .Select(x => new InvenMngModelRes0001
                    {
                        WorkerOrderProducePlanId = x.Lot.ProcessProgress.WorkerOrderProducePlanId,

                        workOrderDate = x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),
                        workOrderNo = x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderNo,

                        facilitiesCode = x.Lot.ProcessProgress.WorkOrderProducePlan.Facility.Code,
                        facilitiesName = x.Lot.ProcessProgress.WorkOrderProducePlan.Facility.Name,

                        productId = x.Product.Id,
                        productClassification = x.Product.CommonCode.Name,
                        productCode = x.Product.Code,
                        productName = x.Product.Name,
                        productStandard = x.Product.Standard,
                        productUnit = x.Product.Unit,
                        
                        processCode = x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null? x.Lot.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y=>y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y=>y.ProductProcess.Process.Code).FirstOrDefault(),
                        processName = x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.Lot.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),
                        processId = x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.Lot.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Id :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Id).FirstOrDefault(),

                        productionQuantity = x.Lot.ProcessProgress.ProductionQuantity,
                        
                        LOSS = (float)_db.ProductItems
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.ProductId == x.Product.Id)
                            .Where(y=> x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.Lot.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.ProductProcessId == y.ProductProcessId : 
                                _db.WorkerOrderWithoutPlans.Where(z=>z.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.Lot.ProcessProgress.WorkerOrderProducePlanId).Select(z=> z.ProductProcess.ProductProcessId).FirstOrDefault() ==  y.ProductProcessId
                            ).Select(y=>y.Loss).FirstOrDefault(),

                        requiredQuantity = (float)_db.ProductItems
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.ProductId == x.Product.Id)
                            .Where(y => x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.Lot.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.ProductProcessId == y.ProductProcessId :
                                _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.Lot.ProcessProgress.WorkerOrderProducePlanId).Select(z => z.ProductProcess.ProductProcessId).FirstOrDefault() == y.ProductProcessId
                            ).Select(y => y.Require).FirstOrDefault(),
                        
                        totalInputQuantity = x.ConsumeCount, 

                        productLOT = x.Lot.LotName,

                    }).ToListAsync();



                var filteredRes = res
                    .Where(x => req.processId == 0 ? true : x.processId == req.processId)
                    .Where(x => req.productId == 0 ? true : x.productId == req.productId)
                    
                    
                    .ToList();

                var Res = new Response<IEnumerable<InvenMngModelRes0001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = filteredRes
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<InvenMngModelRes0001>>()
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
