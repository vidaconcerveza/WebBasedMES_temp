using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.BaseInfo;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.BaseInfo;



namespace WebBasedMES.Services.Repositories.BaseInfo
{
    public interface IBaseInfoRepository
    {
        Task<Response<IEnumerable<DepartmentResponse>>> GetAllDepartments();
        Task<Response<DepartmentResponse>> GetDepartment(int id);
        Task<Response<bool>> CreateDepartment(DepartmentRequest dep, int id);
        Task<Response<bool>> UpdateDepartment(DepartmentRequest dep, int id);
        Task<Response<bool>> DeleteDepartment(int id);

        Task<Response<IEnumerable<PositionResponse>>> GetAllPositions();
        Task<Response<PositionResponse>> GetPosition(int id);
        Task<Response<bool>> CreatePosition(PositionRequest position, int id);
        Task<Response<bool>> UpdatePosition(PositionRequest position, int id);
        Task<Response<bool>> DeletePosition(int id);

        Task<Response<IEnumerable<EmployTypeResponse>>> GetAllEmployTypes();
        Task<Response<EmployTypeResponse>> GetEmployType(int id);
        Task<Response<bool>> CreateEmployType(EmployTypeRequest employ, int id);
        Task<Response<bool>> UpdateEmployType(EmployTypeRequest employ, int id);
        Task<Response<bool>> DeleteEmployType(int id);


        Task<Response<IEnumerable<UserRoleResponse>>> GetAllUserRoles();
        Task<Response<UserRoleResponse>> GetUserRole(int id);
        Task<Response<bool>> CreateUserRole(UserRoleRequest role, int id);
        Task<Response<bool>> UpdateUserRole(UserRoleRequest role, int id);
        Task<Response<bool>> DeleteUserRole(int id);

        //BaseInfoCode
        Task<Response<IEnumerable<SortCode>>> GetSortCodesBySearch(SortCodeRequest _sort);
        Task<Response<IEnumerable<SortCode>>> GetSortCodes();
        Task<Response<SortCodeResponse>> GetSortCode(int id);

        Task<Response<bool>> CreateSortCode(SortCodeRequest req);
        Task<Response<bool>> UpdateSortCode(SortCodeRequest req);
        Task<Response<bool>> DeleteSortCode(SortCodeRequest req);



        Task<Response<IEnumerable<CommonCodeResponse>>> GetCommonCodesBySearch(CommonCodeRequest req);
        Task<Response<SortCodeResponse>> CreateCommonCode(CommonCodeRequest code);
        Task<Response<SortCodeResponse>> UpdateCommonCode(int id, CommonCodeRequest code);
        Task<Response<SortCodeResponse>> DeleteCommonCode(int id);
        Task<Response<IEnumerable<CommonCodeResponse>>> DeleteCommonCodes(int[] id);
        Task<Response<CommonCodeResponse>> GetCommonCode(int id);
        Task<Response<IEnumerable<CommonCodeResponse>>> GetCommonCodeByCode(string code);
        Task<Response<IEnumerable<CommonCodeResponse>>> GetCommonCodes(string code);
        Task<Response<IEnumerable<string>>> UpdateCommonCodes(List<CommonCodeResponse> codes);

        //Partners
        Task<Response<IEnumerable<PartnerResponse>>> GetPartnersBySearch(PartnerRequest proc);
        Task<Response<PartnerResponse>> GetPartner(int id);
        Task<Response<IEnumerable<PartnerResponse>>> GetPartners();
        Task<Response<IEnumerable<PartnerResponse>>> CreatePartner(PartnerRequest partner);
        Task<Response<IEnumerable<PartnerResponse>>> UpdatePartner(PartnerRequest partner, int id);
        Task<Response<IEnumerable<string>>> UpdatePartners(List<PartnerResponse> partners);

        Task<Response<IEnumerable<PartnerResponse>>> DeletePartners(int[] id);


        //Items(Product)
        Task<Response<ItemResponse>> GetItem(int id);
        Task<Response<IEnumerable<ItemResponse>>> GetItems(int code);
        Task<Response<IEnumerable<ItemResponse>>> CreateItem(ItemRequest item);
        Task<Response<IEnumerable<ItemResponse>>> UpdateItem(ItemRequest item, int id);
        Task<Response<IEnumerable<string>>> UpdateItems(List<ItemResponse> item);
        Task<Response<IEnumerable<ItemResponse>>> DeleteItems(int[] id);


        Task<Response<IEnumerable<InspectionTypeResponse>>> GetInspectionTypesBySearch(InspectionTypeRequest req);
        Task<Response<InspectionTypeResponse>> GetInspectionType(int id);
        Task<Response<IEnumerable<InspectionTypeResponse>>> GetInspectionTypes(int code);
        Task<Response<IEnumerable<InspectionTypeResponse>>> CreateInspectionType(InspectionTypeRequest type);
        Task<Response<IEnumerable<InspectionTypeResponse>>> UpdateInspectionType(InspectionTypeRequest type, int id);
        Task<Response<IEnumerable<string>>> UpdateInspectionTypes(List<InspectionTypeResponse> type);
        Task<Response<IEnumerable<InspectionTypeResponse>>> DeleteInspectionTypes(int[] id);


        //Inspection Item
        Task<Response<IEnumerable<InspectionItemResponse>>> GetInspectionItemsBySearch(InspectionItemRequest req);
        Task<Response<InspectionItemResponse>> GetInspectionItem(int id);
        Task<Response<IEnumerable<InspectionItemResponse>>> GetInspectionItems(int code);
        Task<Response<IEnumerable<InspectionItemResponse>>> CreateInspectionItem(InspectionItemRequest type);
        Task<Response<IEnumerable<InspectionItemResponse>>> UpdateInspectionItem(InspectionItemRequest type, int id);
        Task<Response<IEnumerable<string>>> UpdateInspectionItems(List<InspectionItemResponse> type);
        Task<Response<IEnumerable<InspectionItemResponse>>> DeleteInspectionItems(int[] id);


        Task<Response<DefectiveResponse>> GetDefective(int id);
        Task<Response<IEnumerable<DefectiveResponse>>> GetDefectivesBySearch(DefectiveRequest def);
        Task<Response<IEnumerable<DefectiveResponse>>> GetDefectives(int code);
        Task<Response<IEnumerable<DefectiveResponse>>> CreateDefective(DefectiveRequest type);
        Task<Response<IEnumerable<DefectiveResponse>>> UpdateDefective(DefectiveRequest type, int id);
        Task<Response<IEnumerable<string>>> UpdateDefectives(List<DefectiveResponse> type);
        Task<Response<IEnumerable<DefectiveResponse>>> DeleteDefectives(int[] id);


        //Facility
        Task<Response<IEnumerable<FacilityPopupResponse>>> GetFacilitiesPopupBySearch(FacilityPopupRequest facility);

        Task<Response<IEnumerable<FacilityResponse>>> GetFacilitiesBySearch(FacilityRequest facility);
        Task<Response<IEnumerable<FacilityResponse>>> FacilityList(FacilityRequest facility);
        Task<Response<FacilityResponse>> GetFacility(int id);
        Task<Response<IEnumerable<FacilityResponse>>> GetFacilitys(int code);
        Task<Response<IEnumerable<FacilityResponse>>> CreateFacility(FacilityRequest facility);
        Task<Response<IEnumerable<FacilityResponse>>> UpdateFacility(FacilityRequest facility, int id);
        Task<Response<IEnumerable<string>>> UpdateFacilitys(List<FacilityResponse> facility);
        Task<Response<IEnumerable<FacilityResponse>>> DeleteFacilitys(int[] id);

        //Process
        Task<Response<IEnumerable<ProcessPopupResponse>>> GetProcessesPopupBySearch(ProcessPopupRequest proc);

        Task<Response<IEnumerable<ProcessResponse>>> GetProcessesBySearch(ProcessRequest proc);
        Task<Response<ProcessResponse>> GetProcess(int id);

        Task<Response<IEnumerable<ProcessFacilityInterface>>> GetProcessFacility(int Id);
        Task<Response<IEnumerable<ProcessDownTimeInterface>>> GetProcessDownTime(int Id);
        Task<Response<IEnumerable<ProcessDefectiveInterface>>> GetProcessDefective(int Id);


        Task<Response<IEnumerable<ProcessResponse>>> GetProcesses(int code);
        Task<Response<IEnumerable<ProcessResponse>>> CreateProcess(ProcessRequest type);
        Task<Response<IEnumerable<ProcessResponse>>> UpdateProcess(ProcessRequest type, int id);
        Task<Response<IEnumerable<string>>> UpdateProcesses(List<ProcessResponse> type);
        Task<Response<IEnumerable<ProcessResponse>>> DeleteProcesses(int[] id);



        Task<Response<IEnumerable<ProductResponse>>> GetBomsBySearch(ProductRequest prd);
        Task<Response<IEnumerable<ProductPopupResponse>>> GetBomsPopupBySearch(ProductPopupRequest prd);
        Task<Response<BomResponseSortByProcess>> GetBomById2(ProductRequest prd);

        Task<Response<BomResponse>> GetBomById(ProductRequest prd);
        Task<Response<bool>> UpdateBom(BomUpdateRequest bom);
        Task<Response<IEnumerable<ProductResponse>>> GetProductsBySearch(ProductRequest prd);
        Task<Response<ProductResponse>> GetProduct(int id);
        Task<Response<IEnumerable<ProductResponse>>> GetProducts(int code);
        Task<Response<IEnumerable<ProductResponse>>> CreateProduct(ProductRequest item);
        Task<Response<IEnumerable<ProductResponse>>> UpdateProduct(ProductRequest item, int id);
        //  Task<Response<IEnumerable<string>>> UpdateProducts (List<ProductResponse> item);
        Task<Response<IEnumerable<ProductResponse>>> DeleteProducts(int[] id);
        Task<Response<IEnumerable<InputProducedProductResponse>>> GetInputProducts(ProductRequest _prd);
        Task<Response<IEnumerable<InputProducedProductResponse>>> GetProducedProducts(ProductRequest _prd);
        Task<Response<IEnumerable<ProcessNotWorkPopupResponse>>> GetProcessNotWorksBySearch(ProcessNotWorkPopupRequest req);


        Task<Response<IEnumerable<CommonCodeResponse>>> GetCleaningCommonCodes();
        Task<Response<IEnumerable<CommonCodeResponse>>> GetRepairCommonCodes();

        Task<Response<IEnumerable<CommonCodeResponse>>> GetDownTimes(DownTimeRequest req);
        Task<Response<IEnumerable<CommonCodeResponse>>> GetModels(DownTimeRequest req);


        Task Save();
    }
}