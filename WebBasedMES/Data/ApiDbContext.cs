using WebBasedMES.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebBasedMES.Data.Models.JwtAuth;
using WebBasedMES.Data.Models.Utilities;
using WebBasedMES.Data.Models.BaseInfo;
using WebBasedMES.Data.Models.Mold;
using WebBasedMES.Data.Models.Lots;
using WebBasedMES.Data.Models.ProducePlan;
using WebBasedMES.ViewModels.Lot;
using WebBasedMES.ViewModels.ProducePlan;
using WebBasedMES.ViewModels.Process;
using WebBasedMES.ViewModels.Quality;
using WebBasedMES.ViewModels.ProduceStatus;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;
using WebBasedMES.Data.Models.InspectionRepair;
using WebBasedMES.Data.Models.Barcode;
using WebBasedMES.Data.Models.FacilityManage;
using WebBasedMES.Data.Models.Lot;
using WebBasedMES.Data.Models.SystemLog;

namespace WebBasedMES.Data
{
    public class ApiDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApiDbContext() { }
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<SubMenu> Submenus { get; set; }

        public DbSet<DefaultMenu> DefaultMenus { get; set; }
        public DbSet<DefaultSubMenu> DefaultSubmenus { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<EmployType> EmployTypes { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<BusinessInfo> BusinessInfo { get; set; }

        public DbSet<Notice> Notices { get; set; }
        public DbSet<UploadFile> UploadFiles { get; set; }

        public DbSet<BaseInfoCode> BaseInfoCodes { get; set; }

        public DbSet<SortCode> SortCodes { get; set; }
        public DbSet<CommonCode> CommonCodes { get; set; }

        public DbSet<Partner> Partners { get; set; }
        public DbSet<Item> Items { get; set; }

        public DbSet<Defective> Defectives { get; set; }
        public DbSet<Facility> Facilitys { get; set; }
        public DbSet<InspectionItem> InspectionItems { get; set; }
        public DbSet<InspectionType> InspectionTypes { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<ProcessFacility> ProcessFacilities { get; set; }
        public DbSet<ProcessDownTimeType> ProcessDownTimeTypes { get; set; }

        public DbSet<ProcessDefective_Master> ProcessDefectives_Master { get; set; }




        public DbSet<ProcessDefective> ProcessDefectives { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductProcess> ProductProcesses { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<ProductProduce> ProductProduces { get; set; }

        //Mold
        public DbSet<Mold> Molds { get; set; }
        public DbSet<MoldDraft> MoldDrafts { get; set; }
        //public DbSet<Draft> Drafts { get; set; }

        public DbSet<MoldGroup> MoldGroups { get; set; }
        public DbSet<MoldGroupElement> MoldGroupElements { get; set; }

        public DbSet<MoldCleaning> MoldCleanings { get; set; }
        //public DbSet<MoldCleaningElement> MoldCleaningElements { get; set; }

        public DbSet<PreventiveMaintenanceMold> PreventiveMaintenanceMolds { get; set; }
        public DbSet<PreventiveMaintenanceFacility> PreventiveMaintenanceFacilitys { get; set; }

        //Lot
        public DbSet<WebBasedMES.Data.Models.Lots.ProcessProgress> ProcessProgresses { get; set; }
        public DbSet<LotEntity> Lots { get; set; }
        public DbSet<LotCount> LotCounts { get; set; }


        public DbSet<MoldLocation> MoldLocations { get; set; }

        //입출고관리-수주등록
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        ////입출고관리-수주등록
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<OrderProduct> OrderProducts { get; set; }
        //public DbSet<OrderItem> OrderItems { get; set; }

        ////입출고관리-발주등록
        public DbSet<OutStore> OutStores { get; set; }
        //public DbSet<OutStoreItem> OutStoreItems { get; set; }
        public DbSet<OutStoreProduct> OutStoreProducts { get; set; }


        ////입출고관리-입고등록
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreOutStoreProduct> StoreOutStoreProducts { get; set; }
        //public DbSet<StoreOutStoreItem> StoreOutStoreItems { get; set; }
        public DbSet<StoreOutStoreProductDefective> StoreOutStoreProductDefectives { get; set; }
        public DbSet<StoreOutStoreProductInspectionType> StoreOutStoreProductInspectionTypes { get; set; }

        ////입출고관리-출고등록
        public DbSet<OutOrder> OutOrders { get; set; }
        public DbSet<OutOrderProduct> OutOrderProducts { get; set; }
        public DbSet<OutOrderProductDefective> OutOrderProductsDefectives { get; set; }
        public DbSet<OutOrderProductStock> OutOrderProductsStocks { get; set; }

        ////재고관리-재고관리
        //public DbSet<Inventory> Inventorys { get; set; }
        //public DbSet<Warehouse> Warehouses { get; set; }

        ////점검수리관리-점검관리
        //public DbSet<CheckFacility> CheckFacilitys { get; set; }
        //public DbSet<RepairFacility> RepairFacilitys { get; set; }

        //생산계획관리
        public DbSet<ProducePlan> ProducePlans { get; set; }
        public DbSet<ProducePlansProduct> ProducePlanProducts { get; set; }
        public DbSet<ProducePlansProcess> ProducePlanProcesses { get; set; }

        //작업지시관리
        public DbSet<WorkerOrder> WorkerOrders { get; set; }
        public DbSet<WorkerOrderProducePlan> WorkerOrderProducePlans { get; set; }
        public DbSet<WorkerOrderWithoutPlan> WorkerOrderWithoutPlans { get; set; }

        //생산공정관리
        public DbSet<ProcessNotWork> ProcessNotWork { get; set; }
        //test
        public DbSet<procesureT1> procesureT1s { get; set; }


        //점검관리
        public DbSet<FacilityInspection> FacilityInspections { get; set; }
        public DbSet<FacilityInspectionItem> FacilityInspectionItems { get; set; }
        public DbSet<MoldInspection> MoldInspections { get; set; }
        public DbSet<MoldInspectionItem> MoldInspectionItems { get; set; }

        public DbSet<RepairAsk> RepairAsks { get; set; }
        public DbSet<RepairLog> RepairLogs { get; set; }


        public DbSet<BarcodeMaster> BarcodeMasters { get; set; }

        public DbSet<FacilityBaseInfo> FacilityBaseInfos { get; set; }
        public DbSet<FacilityControl> FacilityControls { get; set; }


        public DbSet<FacilityStatus> FacilityStatus { get; set; }
        public DbSet<FacilityStatusLog> FacilityStatusLog { get; set; }
        public DbSet<FacilityErrorLog> FacilityErrorLog { get; set; }
        public DbSet<FacilityErrorCode> FacilityErrorCode { get; set; }
        public DbSet<MoldStatusLog> MoldStatusLog { get; set; }

        public DbSet<FacilityOperation> FacilityOperations { get; set; }
        public DbSet<FacilityOperationInputItem> FacilityOperationInputItems { get; set; }
        public DbSet<FacilityOperationInputItemStock> FacilityOperationInputItemStocks { get; set; }


        //VW TABLE
        public DbSet<VW_KPI> VW_KPI { get; set; }
        public DbSet<Inventory> Inventory { get; set; }


        /*Raw SQL 테스트*/
        public DbSet<ProducePlanReponse001> 생산계획관리_목록조회 { get; set; }
        public DbSet<ProducePlanReponse002> 생산계획관리_목록조회_제품상세내역 { get; set; }
        public DbSet<ProducePlanReponse004> 생산계획관리_목록조회_제품상세내역_공정목록 { get; set; }
        public DbSet<ProducePlanReponse005> 생산계획관리_목록조회_제품상세내역_공정목록_투입품목 { get; set; }
        public DbSet<GetRequiredAmountsResponse001> 생산계획관리_소요량산출 { get; set; }
        public DbSet<GetReportByProcessesResponse001> 생산계획관리_공정별작업일보관리_공정목록 { get; set; }
        public DbSet<GetReportByProcessWorkOrdersResponse001> 생산계획관리_공정별작업일보관리_공정목록_작업지시목록 { get; set; }
        public DbSet<GetReportByProcessWorkOrderProducePlansResponse001> 생산계획관리_공정별작업일보관리_공정목록_작업지시목록_작업상세내역 { get; set; }
        public DbSet<GetProductionManageByProductsResponse001> 생산계획관리_제품별생산량관리_목록 { get; set; }
        public DbSet<GetProductionManageByPeriodsResponse001> 생산계획관리_기간별생산량관리_목록 { get; set; }
        public DbSet<GetDefectiveManageByPeriodsResponse001> 생산계획관리_기간별불량수량관리_목록 { get; set; }
        public DbSet<GetProductionManageByMonthResponse001> 생산계획관리_월별생산량관리_목록 { get; set; }

       // public DbSet<WorkOrderReponse001> 작업지시관리_목록조회 { get; set; }
        public DbSet<WorkOrderResponse005> 작업지시관리_목록조회_공정목록 { get; set; }

        public DbSet<WorkOrderProducePlanResponse001> 생산공정관리_공정진행현황관리_작업목록 { get; set; }
        public DbSet<ProcessProgressResponse001> 생산공정관리_공정진행현황관리_작업목록_공정별생산실적 { get; set; }
        public DbSet<ProcessDefectiveResponse001> 생산공정관리_공정진행현황관리_작업목록_공정별생산실적_불량유형 { get; set; }
        public DbSet<ProcessNotWorkResponse001> 생산공정관리_공정진행현황관리_작업목록_공정별생산실적_비가동 { get; set; }
        public DbSet<ProductItemsResponse001> 생산공정관리_공정진행현황관리_작업목록_공정별생산실적_투입품목 { get; set; }

        public DbSet<ProcessNotWorkResponse003> 생산공정관리_공정진행현황관리_작업목록_공정별생산실적_비가동_수정버튼클릭팝업 { get; set; }
        public DbSet<ProcessDefectiveResponse003> 생산공정관리_공정진행현황관리_작업목록_공정별생산실적_불량유형_수정버튼클릭팝업 { get; set; }

        public DbSet<ProductItemsResponse002> 생산공정관리_공정진행현황관리_투입품목수정버튼팝업 { get; set; }


        public DbSet<EtcDefective> EtcDefectives { get; set; }
        public DbSet<EtcDefectivesDetail> EtcDefectivesDetails { get; set; }


        public DbSet<UserLog> UserLog { get; set; }
        //품질관리
        //생산현황모니터링
        public DbSet<FaultyRes003> 품질관리_기타불량등록_등록수정화면_목록조회 { get; set; }
        public DbSet<ProcessProgressStatusRes001> 생산현황모니터링_공정진행현황_공정진행현황 { get; set; }
        public DbSet<FacilityWorkStatusRes001> 생산현황모니터링_설비가동현황_설비가동현황 { get; set; }
        public DbSet<OrderProduceStatusRes001> 생산현황모니터링_수주대비생산현황_수주대비생산현황 { get; set; }
        public DbSet<ProducePlanProduceStatusRes001> 생산현황모니터링_계획대비생산현황_계획대비생산현황 { get; set; }
        public DbSet<MoldStatusRes001> 생산현황모니터링_금형관리현황_정기점검대상목록 { get; set; }
        public DbSet<MoldStatusRes002> 생산현황모니터링_금형관리현황_세척대상목록 { get; set; }
        public DbSet<FacilityStatusRes001> 생산현황모니터링_설비관리현황_설비관리현황 { get; set; }
        //생산현황모니터링

        //입출고
        public DbSet<OrderRes005> 공통_품목검색_팝업 { get; set; }
        public DbSet<StoreRes013> 입출고관리_입고등록_발주품목리스트 { get; set; }
        public DbSet<InvenMngModelRes0001> 입출고관리_자재투입현황_자재투입현황리스트 { get; set; }
        //입출고



        //Voltage Inspection
        public DbSet<FacilityStatus_Inspection> FacilityStatus_Inspection { get; set; }
        public DbSet<VoltageInspection> VoltageInspections { get; set; }



    }
}
