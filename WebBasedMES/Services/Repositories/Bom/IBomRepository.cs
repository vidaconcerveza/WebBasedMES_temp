using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.BaseInfo;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.BaseInfo;
using WebBasedMES.ViewModels.Bom;
using WebBasedMES.ViewModels.File;

namespace WebBasedMES.Services.Repositories.Bom
{
    public interface IBomRepository
    {
        Task<Response<IEnumerable<BomResponseSortByProcess>>> DownloadBomExcelFile(ProductRequest prd);
        Task<Response<bool>> UploadBomExcelFile(List<BomUpdateInterface> boms);

        Task<Response<IEnumerable<ProductInputItemResponse>>> GetProductInputItems(ProductInputItemRequest req);
        Task<Response<ProductInputItemResponse>> GetProductInputItemAdd(ProductInputItemRequest req);


        Task Save ();
    }
}