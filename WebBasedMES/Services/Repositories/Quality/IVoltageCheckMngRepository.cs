using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Services.Repositories.Quality
{
    public interface IVoltageCheckMngRepository
    {
        // 1) 전압검사 관리 메인 화면
        Task<Response<IEnumerable<VoltageCheckRes001>>> QualityVoltageCheckManage(VoltageCheckReq001 voltageCheckReq001);
        Task<Response<IEnumerable<BottomDeadPointResponse>>> QualityBottomDeadPointManage(VoltageCheckReq001 voltageCheckReq001);
        Task<Response<IEnumerable<SlideCurrentResponse>>> QualitySlideManage(VoltageCheckReq001 voltageCheckReq001);
        Task<Response<IEnumerable<TonCheckResponse>>> QualityTonManage(VoltageCheckReq001 voltageCheckReq001);

        Task<Response<IEnumerable<VoltageInspectionResponse>>> GetVoltageInspections(VoltageInspectionRequest req);
        Task<Response<bool>> CreateVoltageInspections(VoltageInspectionRequest req);
        Task<Response<bool>> DeleteVoltageInspections(VoltageInspectionRequest req);
        Task<Response<InspectionDataResponse>> GetInspectionData();
        Task<Response<IEnumerable<TempHumidResponse>>> GetTempHumids(TempHumidRequest req);

        Task Save ();
    }
}