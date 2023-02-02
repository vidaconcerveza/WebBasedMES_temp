using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.BaseInfo;
using WebBasedMES.Migrations;
using WebBasedMES.Services.Repositories;
using WebBasedMES.Services.Repositories.BaseInfo;
using WebBasedMES.ViewModels.BaseInfo;
using WebBasedMES.ViewModels.File;
using static System.Net.WebRequestMethods;
using ApplicationUser = WebBasedMES.Data.Models.ApplicationUser;

namespace WebBasedMES.Controllers.BaseInfoManage
{
    public class BaseInfoManageController : ControllerBase
    {
        private readonly ILogger<BaseInfoManageController> _logger;
        private readonly IBaseInfoRepository _repo;

        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public BaseInfoManageController(ILogger<BaseInfoManageController> logger, IBaseInfoRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;

            _db = db;
            _userManager = userManager;
        }

        #region DEPARTMENT
        [HttpGet("/api/baseinfo/department")]
        public async Task<IActionResult> GetAllDepartments([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetAllDepartments();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/department/{id}")]
        public async Task<IActionResult> GetDepartment(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetDepartment(id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentRequest dep, int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateDepartment(dep, id);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpDelete("/api/baseinfo/department/{id}")]

        public async Task<IActionResult> DeleteDepartment(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteDepartment(id);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        #endregion DEPARTMENT

        #region POSITION_직위
        //2. 직위
        [HttpGet("/api/baseinfo/position")]
        public async Task<IActionResult> GetAllPositions( [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetAllPositions();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/position/{id}")]
        public async Task<IActionResult> GetPosition(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetPosition(id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/position")]
        public async Task<IActionResult> CreatePosition([FromBody] PositionRequest dep, int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreatePosition(dep, id);
            if (result.IsSuccess)
            {
                string dataSize =  "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPut("/api/baseinfo/position/{id}")]
        public async Task<IActionResult> UpdatePosition([FromBody] PositionRequest dep, int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdatePosition(dep, id);
            return StatusCode(200, result);
        }
        [HttpDelete("/api/baseinfo/position/{id}")]

        public async Task<IActionResult> DeletePosition(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeletePosition(id);
            if (result.IsSuccess)
            {
                string dataSize =  "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        #endregion POSITION_직위

        #region EMPLOY TYPE (고용 타입)
        [HttpGet("/api/baseinfo/employ-type")]
        public async Task<IActionResult> GetAllEmployTypes([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetAllEmployTypes();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/employ-type/{id}")]
        public async Task<IActionResult> GetEmployType(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetEmployType(id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/employ-type")]
        public async Task<IActionResult> CreateEmployType([FromBody] EmployTypeRequest dep, int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateEmployType(dep, id);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPut("/api/baseinfo/employ-type/{id}")]
        public async Task<IActionResult> UpdateEmployType([FromBody] EmployTypeRequest dep, int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateEmployType(dep, id);
            if (result.IsSuccess)
            {
                string dataSize =  "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpDelete("/api/baseinfo/employ-type/{id}")]

        public async Task<IActionResult> DeleteEmployType(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteEmployType(id);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        #endregion EMPLOY TYPE

        #region USER ROLE (관리자/작업자/...)
        //4. UserRole
        [HttpGet("/api/baseinfo/user-role")]
        public async Task<IActionResult> GetAllUserRoles([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetAllUserRoles();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/user-role/{id}")]
        public async Task<IActionResult> GetUserRole(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetUserRole(id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/user-role")]
        public async Task<IActionResult> CreateUserRole([FromBody] UserRoleRequest dep, int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateUserRole(dep, id);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPut("/api/baseinfo/user-role/{id}")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UserRoleRequest dep, int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateUserRole(dep, id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpDelete("/api/baseinfo/user-role/{id}")]

        public async Task<IActionResult> DeleteUserRole(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteUserRole(id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        #endregion USER ROLE

        #region SORT CODE (분류 코드) & COMMON CODE

        [HttpPost("/api/baseinfo/sort-code-manage/getSortCodes")]
        public async Task<IActionResult> GetSortCodesBySearch([FromBody] SortCodeRequest sortCode, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetSortCodesBySearch(sortCode);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/sort-code-manage/getSortCode")]
        public async Task<IActionResult> GetBaseInfoCode([FromBody] SortCodeRequest sortCode, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetSortCode(sortCode.Id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/baseinfo/sort-code-manage/createSortCode")]
        public async Task<IActionResult> CreateSortCode([FromBody] SortCodeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateSortCode(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/sort-code-manage/updateSortCode")]
        public async Task<IActionResult> UpdateSortCode([FromBody] SortCodeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateSortCode(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/sort-code-manage/deleteSortCode")]
        public async Task<IActionResult> DeleteSortCode([FromBody] SortCodeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteSortCode(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }



        [HttpGet("/api/baseinfo/sort-code-manage")]
        public async Task<IActionResult> GetBaseInfoCode( [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetSortCodes();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }



        [HttpGet("/api/baseinfo/sort-code-manage/download-file")]
        public async Task<IActionResult> DownloadSortCodeList([FromHeader(Name = "Uuid")] string Uuid)
        {
            var _codes = await _repo.GetSortCodes();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "기준정보코드.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("분류코드");

                worksheet.Cell("A1").Value = "분류코드";
                worksheet.Cell("A2").Value = "No";
                worksheet.Cell("B2").Value = "분류코드";
                worksheet.Cell("C2").Value = "분류코드명";
                worksheet.Cell("D2").Value = "비고";


                int i = 0;
                foreach (var code in _codes.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 2).ToString()).Value = code.Id;
                    worksheet.Cell("B" + (i + 2).ToString()).Value = code.Code;
                    worksheet.Cell("C" + (i + 2).ToString()).Value = code.Name;
                    worksheet.Cell("D" + (i + 2).ToString()).Value = code.Memo;
                    worksheet.Cell("E" + (i + 2).ToString()).Value = code.IsUsing ? "사용" : "미사용";
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }


        [HttpPost("/api/baseinfo/common-code-manage/getCommonCodes")]
        public async Task<IActionResult> GetCommonCodesBySearch([FromBody] CommonCodeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetCommonCodesBySearch(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        //To use CommonCodes Menu
        [HttpGet("/api/baseinfo/common-code-manage/getCommonCodes/{code}")]
        public async Task<IActionResult> GetCommonCodes2(string code, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetCommonCodes(code);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/api/baseinfo/common-code-manage/getCommonCode")]
        public async Task<IActionResult> GetCommonCodeById([FromBody] CommonCodeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetCommonCode(req.Id);
                if (result.IsSuccess)
                {
                    string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                    await SmartFactoryLog(Uuid, "조회", dataSize);
                    return Ok(result);
                }
                else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/api/baseinfo/common-code-manage/createCommonCode")]
        public async Task<IActionResult> CreateCommonCodeByPost([FromBody] CommonCodeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateCommonCode(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/api/baseinfo/common-code-manage/updateCommonCode")]
        public async Task<IActionResult> UpdateCommonCodeByPost([FromBody] CommonCodeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateCommonCode(req.Id, req);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/api/baseinfo/common-code-manage/deleteCommonCode")]
        public async Task<IActionResult> DeleteCommonCodeByPost([FromBody] int[] Ids, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteCommonCodes(Ids);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }



        [HttpGet("/api/baseinfo/sort-code-manage/{id}")]
        public async Task<IActionResult> GetBaseInfoCode(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetSortCode(id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/common-code-manage/{id}")]
        public async Task<IActionResult> GetCommonCode(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetCommonCode(id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/common-code-manage/by-code/{code}")]
        public async Task<IActionResult> GetCommonCodeByCode(string code, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetCommonCodeByCode(code);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/common-code-manage")]
        public async Task<IActionResult> CreateCommonCode([FromBody] CommonCodeRequest code, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateCommonCode(code);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPut("/api/baseinfo/common-code-manage/{id}")]
        public async Task<IActionResult> UpdateCommonCode([FromBody] CommonCodeRequest code, int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateCommonCode(id, code);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpDelete("/api/baseinfo/common-code-manage/{id}")]
        public async Task<IActionResult> DeleteCommonCode(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteCommonCode(id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/common-code-manage/excel-download")]
        public async Task<IActionResult> DownloadCommonCodeList([FromBody] CommonCodeRequest req)
        {
            var _codes = await _repo.GetCommonCodes("ALL");
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "공통코드관리.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("공통코드");

                worksheet.Cell("A1").Value = "공통코드";
                worksheet.Cell("A2").Value = "No";
                worksheet.Cell("B2").Value = "분류코드";
                worksheet.Cell("C2").Value = "분류코드명";
                worksheet.Cell("D2").Value = "공통코드";
                worksheet.Cell("E2").Value = "공통코드명";
                worksheet.Cell("F2").Value = "등록일자";
                worksheet.Cell("G2").Value = "등록자";
                worksheet.Cell("H2").Value = "사용여부";
                worksheet.Cell("I2").Value = "비고";

                int i = 0;
                foreach (var code in _codes.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 2).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 2).ToString()).Value = code.SortCode;
                    worksheet.Cell("C" + (i + 2).ToString()).Value = code.SortCodeName;
                    worksheet.Cell("D" + (i + 2).ToString()).Value = code.Code;
                    worksheet.Cell("E" + (i + 2).ToString()).Value = code.Name;
                    worksheet.Cell("F" + (i + 2).ToString()).Value = code.CreateDate;
                    worksheet.Cell("G" + (i + 2).ToString()).Value = code.Creator;
                    worksheet.Cell("H" + (i + 2).ToString()).Value = code.IsUsing ? "사용" : "미사용";
                    worksheet.Cell("I" + (i + 2).ToString()).Value = code.Memo;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/baseinfo/common-code-manage/upload")]
        public async Task<IActionResult> UpdateCommonCodes([FromForm] FileModel file)
        {
            var _file = file;
            var stream = file.FormFile.OpenReadStream();

            var workbook = new XLWorkbook(stream);
            var workSheet = workbook.Worksheets.First();
            var range = workSheet.RangeUsed();

            List<CommonCodeResponse> codes = new List<CommonCodeResponse>();
            
            for (int i = 2; i < range.RowCount(); i++)
            {
                var code = new CommonCodeResponse
                {
                    SortCode = workSheet.Cell(i + 1, 2).Value.ToString(),
                    SortCodeName = workSheet.Cell(i + 1, 3).Value.ToString(),
                    Code = workSheet.Cell(i + 1, 4).Value.ToString(),
                    Name = workSheet.Cell(i + 1, 5).Value.ToString(),
                    CreateDate = workSheet.Cell(i + 1, 6).Value.ToString(),
                    Creator = workSheet.Cell(i + 1, 7).Value.ToString(),
                    IsUsing = workSheet.Cell(i + 1, 8).Value.ToString() == "사용" ? true : false,
                    Memo = workSheet.Cell(i + 1, 9).Value.ToString()
                };
                codes.Add(code);
            }

            if (codes.Count() < 1)
            {
                return Ok();
            }
            else
            {
                var result = await _repo.UpdateCommonCodes(codes);
                if (result.IsSuccess)
                    return Ok(result);
                else
                    return Ok(result);
            }
        }

        #endregion

        #region PARTNER (거래처)
        [HttpPost("/api/baseinfo/partner-manage/getPartners")]
        public async Task<IActionResult> GetPartnersBySearch([FromBody] PartnerRequest part, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetPartnersBySearch(part);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/api/baseinfo/partner-manage/getPartner")]
        public async Task<IActionResult> GetPartnersById([FromBody] PartnerRequest part, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetPartner(part.Id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/api/baseinfo/partner-manage/createPartner")]
        public async Task<IActionResult> CreatePartnerByPost([FromBody] PartnerRequest partner, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreatePartner(partner);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/partner-manage/updatePartner")]
        public async Task<IActionResult> UpdatePartnerByPost([FromBody] PartnerRequest partner, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdatePartner(partner, partner.Id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/partner-manage/deletePartner")]
        public async Task<IActionResult> DeletePartnerByPost([FromBody] PartnerRequest partner, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeletePartners(partner.Ids);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpGet("/api/baseinfo/partner-manage/{id}")]
        public async Task<IActionResult> GetPartner(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetPartner(id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/partner-manage")]
        public async Task<IActionResult> GetPartners([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetPartners();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/partner-manage")]
        public async Task<IActionResult> CreatePartner([FromBody] PartnerRequest partner, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreatePartner(partner);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPut("/api/baseinfo/partner-manage/{id}")]
        public async Task<IActionResult> UpdatePartner([FromBody] PartnerRequest partner, int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdatePartner(partner, id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/partner-manage/delete")]
        public async Task<IActionResult> DeletePartner([FromBody] PartnerRequest partner, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeletePartners(partner.Ids);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/partner-manage/excel-file")]
        public async Task<IActionResult> DownloadPartnerList([FromBody] PartnerRequest req)
        {
            var _partners = await _repo.GetPartners();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "거래처리스트.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("거래처");

                worksheet.Cell("A1").Value = "거래처코드";
                worksheet.Cell("B1").Value = "거래처구분";
                worksheet.Cell("C1").Value = "거래처이름";
                worksheet.Cell("D1").Value = "사업자번호";
                worksheet.Cell("E1").Value = "대표자";
                worksheet.Cell("F1").Value = "전화번호1";
                worksheet.Cell("G1").Value = "전화번호2";
                worksheet.Cell("H1").Value = "팩스번호";
                worksheet.Cell("I1").Value = "업태";
                worksheet.Cell("J1").Value = "종목";
                worksheet.Cell("K1").Value = "비고";
                worksheet.Cell("L1").Value = "거래처그룹";
                worksheet.Cell("M1").Value = "계좌정보(은행명)";
                worksheet.Cell("N1").Value = "계좌정보(계좌번호)";
                worksheet.Cell("O1").Value = "담당자";
                worksheet.Cell("P1").Value = "담당자 메일";
                worksheet.Cell("Q1").Value = "과세정보";
                worksheet.Cell("R1").Value = "상태";

                int i = 0;
                foreach (var partner in _partners.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = partner.Code;
                    worksheet.Cell("B" + (i + 1).ToString()).Value = partner.PartnerType;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = partner.Name;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = partner.BusinessNumber;
                    worksheet.Cell("E" + (i + 1).ToString()).Value = partner.President;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = partner.TelephoneNumber;

                    string group = "";
                    if (partner.Group_Buy)
                    {
                        group = "매입,";
                    }
                    if (partner.Group_Sell)
                    {
                        group += "매출,";
                    }
                    if (partner.Group_Finance)
                    {
                        group += "금융,";
                    }
                    if (partner.Group_Etc)
                    {
                        group += "기타";
                    }

                    worksheet.Cell("G" + (i + 1).ToString()).Value = "";
                    worksheet.Cell("H" + (i + 1).ToString()).Value = partner.FaxNumber;
                    worksheet.Cell("I" + (i + 1).ToString()).Value = partner.BusinessType;
                    worksheet.Cell("J" + (i + 1).ToString()).Value = partner.BusinessClass;
                    worksheet.Cell("K" + (i + 1).ToString()).Value = partner.Memo;
                    worksheet.Cell("L" + (i + 1).ToString()).Value = group;
                    worksheet.Cell("M" + (i + 1).ToString()).Value = partner.BankName;
                    worksheet.Cell("N" + (i + 1).ToString()).Value = partner.BankAccount;
                    worksheet.Cell("O" + (i + 1).ToString()).Value = partner.ContactName;
                    worksheet.Cell("P" + (i + 1).ToString()).Value = partner.ContactEmail;
                    worksheet.Cell("Q" + (i + 1).ToString()).Value = partner.TaxInfo;
                    worksheet.Cell("R" + (i + 1).ToString()).Value = partner.IsUsing ? "활성" : "비활성";
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/baseinfo/partner-manage/upload")]
        public async Task<IActionResult> UpdatePartners([FromForm] FileModel file)
        {
            var _file = file;
            var stream = file.FormFile.OpenReadStream();

            var workbook = new XLWorkbook(stream);
            var workSheet = workbook.Worksheets.First();
            var range = workSheet.RangeUsed();

            List<PartnerResponse> codes = new List<PartnerResponse>();
            try
            {
                for (int i = 1; i < range.RowCount(); i++)
                {
                    string group = workSheet.Cell(i + 1, 12).Value.ToString();
                    string[] words2 = group.Split(',');

                    bool buy = false;
                    bool sell = false;
                    bool fin = false;
                    bool etc = false;

                    for (int j = 0; j < words2.Length; j++)
                    {
                        if (words2[j] == "매출")
                        {
                            sell = true;
                        }
                        if (words2[j] == "매입")
                        {
                            buy = true;
                        }
                        if (words2[j] == "금융")
                        {
                            fin = true;
                        }
                        if (words2[j] == "기타")
                        {
                            etc = true;
                        }
                    }


                    var code = new PartnerResponse
                    {
                        Code = workSheet.Cell(i + 1, 1).Value.ToString(),
                        PartnerType = workSheet.Cell(i + 1, 2).Value.ToString(),
                        Name = workSheet.Cell(i + 1, 3).Value.ToString(),
                        BusinessNumber = workSheet.Cell(i + 1, 4).Value.ToString(),
                        President = workSheet.Cell(i + 1, 5).Value.ToString(),
                        TelephoneNumber = workSheet.Cell(i + 1, 6).Value.ToString(),
                        FaxNumber = workSheet.Cell(i + 1, 8).Value.ToString(),
                        BusinessType = workSheet.Cell(i + 1, 9).Value.ToString(),
                        BusinessClass = workSheet.Cell(i + 1, 10).Value.ToString(),
                        Memo = workSheet.Cell(i + 1, 11).Value.ToString(),
                        Group_Buy = buy,
                        Group_Sell = sell,
                        Group_Finance = fin,
                        Group_Etc = etc,
                        BankName = workSheet.Cell(i + 1, 13).Value.ToString(),
                        BankAccount = workSheet.Cell(i + 1, 14).Value.ToString(),
                        ContactName = workSheet.Cell(i + 1, 15).Value.ToString(),
                        ContactEmail = workSheet.Cell(i + 1, 16).Value.ToString(),
                        TaxInfo = workSheet.Cell(i + 1, 17).Value.ToString(),
                        IsUsing = workSheet.Cell(i + 1, 18).Value.ToString() == "활성" ? true : false
                    };
                    codes.Add(code);
                }

                if (codes.Count() < 1)
                {
                    return Ok();
                }
                else
                {
                    var result = await _repo.UpdatePartners(codes);
                    if (result.IsSuccess)
                        return Ok(result);
                    else
                        return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok();
            }

        }

        #endregion

        #region Item Manage
        [HttpPost("/api/baseinfo/item-manage/getItem")]
        public async Task<IActionResult> GetItem([FromBody] ProductRequest item, [FromHeader(Name = "Uuid")] string Uuid)
        {
            //   var result = await _repo.GetItem(id);
            var result = await _repo.GetProduct(item.ProductId);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/item-manage/all/{code}")]
        public async Task<IActionResult> GetItems(int code, [FromHeader(Name = "Uuid")] string Uuid)
        {
            //Code 는 공통코드. 공통코드에 따라 내보내줍니다.
            //var result = await _repo.GetItems(code);

            var result = await _repo.GetProducts(code);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/item-manage/createItem")]
        public async Task<IActionResult> CreateItem([FromBody] ProductRequest item, [FromHeader(Name = "Uuid")] string Uuid)
        {
            // var result = await _repo.CreateItem(item);
            var result = await _repo.CreateProduct(item);

            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/item-manage/updateItem")]
        public async Task<IActionResult> UpdateItem([FromBody] ProductRequest item, [FromHeader(Name = "Uuid")] string Uuid)
        {
            //  var result = await _repo.UpdateItem(item, id);
            var result = await _repo.UpdateProduct(item, item.ProductId);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/item-manage/deleteItem")]
        public async Task<IActionResult> DeleteItem([FromBody] ProductRequest item, [FromHeader(Name = "Uuid")] string Uuid)
        {
            // var result = await _repo.DeleteItems(id);
            var result = await _repo.DeleteProducts(item.ProductIds);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/item-manage/file")]
        public async Task<IActionResult> DownloadItemList()
        {
            var _items = await _repo.GetItems(0);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "품목리스트.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("품목");

                worksheet.Cell("A1").Value = "품목코드";
                worksheet.Cell("B1").Value = "품목구분";
                worksheet.Cell("C1").Value = "품목이름";
                worksheet.Cell("D1").Value = "규격";
                worksheet.Cell("E1").Value = "단위";
                worksheet.Cell("F1").Value = "과세유형";
                worksheet.Cell("G1").Value = "구매단가";
                worksheet.Cell("H1").Value = "판매단가";
                worksheet.Cell("I1").Value = "수입검사여부";
                worksheet.Cell("J1").Value = "출하검사여부";
                worksheet.Cell("K1").Value = "비고";
                worksheet.Cell("L1").Value = "사용여부";

                int i = 0;
                foreach (var item in _items.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = item.Code;
                    worksheet.Cell("B" + (i + 1).ToString()).Value = item.CommonCodeName;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = item.Name;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = item.Standard;
                    worksheet.Cell("E" + (i + 1).ToString()).Value = item.Unit;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = item.TaxType;
                    worksheet.Cell("G" + (i + 1).ToString()).Value = item.BuyPrice.ToString("C");
                    worksheet.Cell("H" + (i + 1).ToString()).Value = item.SellPrice.ToString("C");
                    worksheet.Cell("I" + (i + 1).ToString()).Value = item.ImportCheck ? "검사" : "미검사";
                    worksheet.Cell("J" + (i + 1).ToString()).Value = item.ExportCheck ? "검사" : "미검사";
                    worksheet.Cell("K" + (i + 1).ToString()).Value = item.Memo;
                    worksheet.Cell("L" + (i + 1).ToString()).Value = item.IsUsing ? "사용" : "미사용";
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/baseinfo/item-manage/upload")]
        public async Task<IActionResult> UpdateItems([FromForm] FileModel file)
        {
            var _file = file;
            var stream = file.FormFile.OpenReadStream();

            var workbook = new XLWorkbook(stream);
            var workSheet = workbook.Worksheets.First();
            var range = workSheet.RangeUsed();

            List<ItemResponse> items = new List<ItemResponse>();
            try
            {
                for (int i = 1; i < range.RowCount(); i++)
                {
                    var item = new ItemResponse
                    {
                        Code = workSheet.Cell(i + 1, 1).Value.ToString(),
                        CommonCodeName = workSheet.Cell(i + 1, 2).Value.ToString(),
                        Name = workSheet.Cell(i + 1, 3).Value.ToString(),
                        Standard = workSheet.Cell(i + 1, 4).Value.ToString(),
                        Unit = workSheet.Cell(i + 1, 5).Value.ToString(),
                        TaxType = workSheet.Cell(i + 1, 6).Value.ToString(),
                        BuyPrice = int.Parse(workSheet.Cell(i + 1, 7).Value.ToString(), System.Globalization.NumberStyles.Currency),
                        SellPrice = int.Parse(workSheet.Cell(i + 1, 7).Value.ToString(), System.Globalization.NumberStyles.Currency),
                        ImportCheck = workSheet.Cell(i + 1, 9).Value.ToString() == "검사" ? true : false,
                        ExportCheck = workSheet.Cell(i + 1, 10).Value.ToString() == "검사" ? true : false,
                        Memo = workSheet.Cell(i + 1, 11).Value.ToString(),
                        IsUsing = workSheet.Cell(i + 1, 12).Value.ToString() == "사용" ? true : false
                    };
                    items.Add(item);
                }

                if (items.Count() < 1)
                {
                    return Ok();
                }
                else
                {
                    var result = await _repo.UpdateItems(items);
                    if (result.IsSuccess)
                        return Ok(result);
                    else
                        return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return Ok();
            }

        }
        #endregion Item Manage

        #region InspectionType Manage


        [HttpPost("/api/baseinfo/inspection-type-manage/getInspectionTypes")]
        public async Task<IActionResult> GetInspectionTypesBySearch([FromBody] InspectionTypeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            //Code 는 공통코드. 공통코드에 따라 내보내줍니다.
            var result = await _repo.GetInspectionTypesBySearch(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/inspection-type-manage/getInspectionType")]
        public async Task<IActionResult> GetInspectionTypeById([FromBody] InspectionTypeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            //Code 는 공통코드. 공통코드에 따라 내보내줍니다.
            var result = await _repo.GetInspectionType(req.Id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/baseinfo/inspection-type-manage/createInspectionType")]
        public async Task<IActionResult> CreateInspectionTypeByPost([FromBody] InspectionTypeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            //Code 는 공통코드. 공통코드에 따라 내보내줍니다.
            var result = await _repo.CreateInspectionType(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/inspection-type-manage/updateInspectionType")]
        public async Task<IActionResult> UpdateInspectionTypeByPost([FromBody] InspectionTypeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            //Code 는 공통코드. 공통코드에 따라 내보내줍니다.
            var result = await _repo.UpdateInspectionType(req, req.Id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/inspection-type-manage/deleteInspectionType")]
        public async Task<IActionResult> DeleteInspectionTypeByPost([FromBody] InspectionTypeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            //Code 는 공통코드. 공통코드에 따라 내보내줍니다.
            var result = await _repo.DeleteInspectionTypes(req.Ids);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }



        [HttpGet("/api/baseinfo/inspection-type-manage/{id}")]
        public async Task<IActionResult> GetInspectionType(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetInspectionType(id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/inspection-type-manage/all/{code}")]
        public async Task<IActionResult> GetInspectionTypes(int code, [FromHeader(Name = "Uuid")] string Uuid)
        {
            //Code 는 공통코드. 공통코드에 따라 내보내줍니다.
            var result = await _repo.GetInspectionTypes(code);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/inspection-type-manage")]
        public async Task<IActionResult> CreateInspectionType([FromBody] InspectionTypeRequest inspectionType, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateInspectionType(inspectionType);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPut("/api/baseinfo/inspection-type-manage/{id}")]
        public async Task<IActionResult> UpdateInspectionType([FromBody] InspectionTypeRequest inspectionType, int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateInspectionType(inspectionType, id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/inspection-type-manage/delete")]
        public async Task<IActionResult> DeleteInspectionType([FromBody] int[] id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteInspectionTypes(id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/inspection-type-manage/file")]
        public async Task<IActionResult> DownloadInspectionTypeList( [FromHeader(Name = "Uuid")] string Uuid)
        {
            var _inspectionTypes = await _repo.GetInspectionTypes(0);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "품목리스트.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("품목");

                worksheet.Cell("A1").Value = "검사코드";
                worksheet.Cell("B1").Value = "검사유형";
                worksheet.Cell("C1").Value = "검사항목";
                worksheet.Cell("D1").Value = "검사기준";
                worksheet.Cell("E1").Value = "검사방법";
                worksheet.Cell("F1").Value = "비고";
                worksheet.Cell("G1").Value = "사용여부";

                int i = 0;
                foreach (var inspectionType in _inspectionTypes.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = inspectionType.Code;
                    worksheet.Cell("B" + (i + 1).ToString()).Value = inspectionType.Type;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = inspectionType.InspectionItem;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = inspectionType.InspectionStandard;
                    worksheet.Cell("E" + (i + 1).ToString()).Value = inspectionType.InspectionMethod;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = inspectionType.Memo;
                    worksheet.Cell("G" + (i + 1).ToString()).Value = inspectionType.IsUsing ? "사용" : "미사용";
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/baseinfo/inspection-type-manage/upload")]
        public async Task<IActionResult> UpdateInspectionTypes([FromForm] FileModel file)
        {
            var _file = file;
            var stream = file.FormFile.OpenReadStream();

            var workbook = new XLWorkbook(stream);
            var workSheet = workbook.Worksheets.First();
            var range = workSheet.RangeUsed();

            List<InspectionTypeResponse> inspectionTypes = new List<InspectionTypeResponse>();
            try
            {
                for (int i = 1; i < range.RowCount(); i++)
                {
                    var inspectionType = new InspectionTypeResponse
                    {
                        Code = workSheet.Cell(i + 1, 1).Value.ToString(),
                        Type = workSheet.Cell(i + 1, 2).Value.ToString(),
                        InspectionItem = workSheet.Cell(i + 1, 3).Value.ToString(),
                        InspectionStandard = workSheet.Cell(i + 1, 4).Value.ToString(),
                        InspectionMethod = workSheet.Cell(i + 1, 5).Value.ToString(),
                        Memo = workSheet.Cell(i + 1, 6).Value.ToString(),
                        IsUsing = workSheet.Cell(i + 1, 7).Value.ToString() == "사용" ? true : false
                    };
                    inspectionTypes.Add(inspectionType);
                }

                if (inspectionTypes.Count() < 1)
                {
                    return Ok();
                }
                else
                {
                    var result = await _repo.UpdateInspectionTypes(inspectionTypes);
                    if (result.IsSuccess)
                        return Ok(result);
                    else
                        return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return Ok();
            }

        }
        #endregion InspectionType Manage

        #region InspectionItem Manage

        [HttpPost("/api/baseinfo/inspection-item-manage/getInspectionItems")]
        public async Task<IActionResult> GetInspectionItemsBySearch([FromBody] InspectionItemRequest item, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetInspectionItemsBySearch(item);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/inspection-item-manage/getInspectionItem")]
        public async Task<IActionResult> GetInspectionItemsById([FromBody] InspectionItemRequest item, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetInspectionItem(item.Id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/inspection-item-manage/createInspectionItem")]
        public async Task<IActionResult> CreateInspectionItemsByPost([FromBody] InspectionItemRequest item, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateInspectionItem(item);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/inspection-item-manage/updateInspectionItem")]
        public async Task<IActionResult> UpdateInspectionItemsById([FromBody] InspectionItemRequest item, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateInspectionItem(item, item.Id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/inspection-item-manage/deleteInspectionItem")]
        public async Task<IActionResult> DeleteInspectionItemsById([FromBody] InspectionItemRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteInspectionItems(req.Ids);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpGet("/api/baseinfo/inspection-item-manage/{id}")]
        public async Task<IActionResult> GetInspectionItem(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetInspectionItem(id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/inspection-item-manage/all/{code}")]
        public async Task<IActionResult> GetInspectionItems(int code, [FromHeader(Name = "Uuid")] string Uuid)
        {
            //Code 는 공통코드. 공통코드에 따라 내보내줍니다.
            var result = await _repo.GetInspectionItems(code);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/inspection-item-manage")]
        public async Task<IActionResult> CreateInspectionItem([FromBody] InspectionItemRequest item, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateInspectionItem(item);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPut("/api/baseinfo/inspection-item-manage/{id}")]
        public async Task<IActionResult> UpdateInspectionItem([FromBody] InspectionItemRequest item, int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateInspectionItem(item, id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/inspection-item-manage/delete")]
        public async Task<IActionResult> DeleteInspectionItem([FromBody] int[] id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteInspectionItems(id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/inspection-item-manage/file")]
        public async Task<IActionResult> DownloadInspectionItemList()
        {
            var _items = await _repo.GetInspectionItems(0);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "품목리스트.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("점검항목");

                worksheet.Cell("A1").Value = "점검코드";
                worksheet.Cell("B1").Value = "점검구분";
                worksheet.Cell("C1").Value = "점검유형";
                worksheet.Cell("D1").Value = "점검주기";
                worksheet.Cell("E1").Value = "점검항목";
                worksheet.Cell("F1").Value = "판단기준";
                worksheet.Cell("G1").Value = "판단방법";
                worksheet.Cell("H1").Value = "비고";
                worksheet.Cell("I1").Value = "사용여부";

                int i = 0;
                foreach (var item in _items.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = item.Code;
                    worksheet.Cell("B" + (i + 1).ToString()).Value = item.Classify;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = item.Type;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = item.CommonCodeName;
                    worksheet.Cell("E" + (i + 1).ToString()).Value = item.InspectionItem;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = item.JudgeStandard;
                    worksheet.Cell("G" + (i + 1).ToString()).Value = item.JudgeMethod;
                    worksheet.Cell("H" + (i + 1).ToString()).Value = item.Memo;
                    worksheet.Cell("I" + (i + 1).ToString()).Value = item.IsUsing ? "사용" : "미사용";
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/baseinfo/inspection-item-manage/upload")]
        public async Task<IActionResult> UpdateInspectionItems([FromForm] FileModel file)
        {
            var _file = file;
            var stream = file.FormFile.OpenReadStream();

            var workbook = new XLWorkbook(stream);
            var workSheet = workbook.Worksheets.First();
            var range = workSheet.RangeUsed();

            List<InspectionItemResponse> items = new List<InspectionItemResponse>();
            try
            {
                for (int i = 1; i < range.RowCount(); i++)
                {
                    var item = new InspectionItemResponse
                    {
                        Code = workSheet.Cell(i + 1, 1).Value.ToString(),
                        Classify = workSheet.Cell(i + 1, 2).Value.ToString(),
                        Type = workSheet.Cell(i + 1, 3).Value.ToString(),
                        CommonCodeName = workSheet.Cell(i + 1, 4).Value.ToString(),
                        InspectionItem = workSheet.Cell(i + 1, 5).Value.ToString(),
                        JudgeStandard = workSheet.Cell(i + 1, 6).Value.ToString(),
                        JudgeMethod = workSheet.Cell(i + 1, 7).Value.ToString(),
                        Memo = workSheet.Cell(i + 1, 8).Value.ToString(),
                        IsUsing = workSheet.Cell(i + 1, 9).Value.ToString() == "사용" ? true : false
                    };
                    items.Add(item);
                }

                if (items.Count() < 1)
                {
                    return Ok();
                }
                else
                {
                    var result = await _repo.UpdateInspectionItems(items);
                    if (result.IsSuccess)
                        return Ok(result);
                    else
                        return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return Ok();
            }

        }

        #endregion InspectionItem Manage

        #region Defective Manage

        [HttpPost("/api/baseinfo/defective-master-manage/getDefectives")]
        public async Task<IActionResult> GetDefectivesBySearch([FromBody] DefectiveRequest _def, [FromHeader(Name = "Uuid")] string Uuid)
        {
            //Code 는 공통코드. 공통코드에 따라 내보내줍니다.
            var result = await _repo.GetDefectivesBySearch(_def);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/defective-master-manage/getDefective")]
        public async Task<IActionResult> GetDefectivesById([FromBody] DefectiveRequest _def, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetDefective(_def.Id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/defective-master-manage/createDefective")]
        public async Task<IActionResult> CreateDefectiveByPost([FromBody] DefectiveRequest _def, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateDefective(_def);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/defective-master-manage/updateDefective")]
        public async Task<IActionResult> UpdateDefectiveByPost([FromBody] DefectiveRequest _def, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateDefective(_def, _def.Id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/baseinfo/defective-master-manage/deleteDefective")]
        public async Task<IActionResult> DeleteDefectiveByPost([FromBody] DefectiveRequest _def, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteDefectives(_def.Ids);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }



        [HttpGet("/api/baseinfo/defective-master-manage/{id}")]
        public async Task<IActionResult> GetDefective(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetDefective(id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/defective-master-manage/all/{code}")]
        public async Task<IActionResult> GetDefectives(int code, [FromHeader(Name = "Uuid")] string Uuid)
        {
            //Code 는 공통코드. 공통코드에 따라 내보내줍니다.
            var result = await _repo.GetDefectives(code);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/defective-master-manage")]
        public async Task<IActionResult> CreateDefective([FromBody] DefectiveRequest defective, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateDefective(defective);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPut("/api/baseinfo/defective-master-manage/{id}")]
        public async Task<IActionResult> UpdateDefective([FromBody] DefectiveRequest defective, int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateDefective(defective, id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/defective-master-manage/delete")]
        public async Task<IActionResult> DeleteDefective([FromBody] int[] id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteDefectives(id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/defective-master-manage/file")]
        public async Task<IActionResult> DownloadDefectiveList()
        {
            var _defectives = _repo.GetDefectives(0);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "불량유형리스트.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("불량유형");

                worksheet.Cell("A1").Value = "불량코드";
                worksheet.Cell("B1").Value = "불량유형";
                worksheet.Cell("C1").Value = "비고";
                worksheet.Cell("D1").Value = "사용여부";
                worksheet.Cell("E1").Value = "등록일";
                worksheet.Cell("F1").Value = "등록자";

                int i = 0;
                foreach (var defective in _defectives.Result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = defective.Code;
                    worksheet.Cell("B" + (i + 1).ToString()).Value = defective.Name;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = defective.Memo;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = defective.IsUsing ? "사용" : "미사용";
                    worksheet.Cell("E" + (i + 1).ToString()).Value = defective.CreateDate;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = defective.Creator;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/baseinfo/defective-master-manage/upload")]
        public async Task<IActionResult> UpdateDefectives([FromForm] FileModel file)
        {
            var _file = file;
            var stream = file.FormFile.OpenReadStream();

            var workbook = new XLWorkbook(stream);
            var workSheet = workbook.Worksheets.First();
            var range = workSheet.RangeUsed();

            List<DefectiveResponse> defectives = new List<DefectiveResponse>();
            try
            {
                for (int i = 1; i < range.RowCount(); i++)
                {
                    var defective = new DefectiveResponse
                    {
                        Code = workSheet.Cell(i + 1, 1).Value.ToString(),
                        Name = workSheet.Cell(i + 1, 2).Value.ToString(),
                        Memo = workSheet.Cell(i + 1, 3).Value.ToString(),
                        IsUsing = workSheet.Cell(i + 1, 7).Value.ToString() == "사용" ? true : false,
                        CreateDate = workSheet.Cell(i + 1, 4).Value.ToString(),
                        Creator = workSheet.Cell(i + 1, 6).Value.ToString(),
                    };
                    defectives.Add(defective);
                }

                if (defectives.Count() < 1)
                {
                    return Ok();
                }
                else
                {
                    var result = await _repo.UpdateDefectives(defectives);
                    if (result.IsSuccess)
                        return Ok(result);
                    else
                        return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return Ok();
            }

        }
        #endregion Defective Manage

        #region Facility Manage
        //3)수리관리
        //3) 2)에서 설비코드 검색버튼 클릭 팝업

        //NEW....

        [HttpPost("/api/baseinfo/facility-master-manage/getFacilitiesPopup")]
        public async Task<IActionResult> GetFacilitiesBySearch([FromBody] FacilityPopupRequest facility, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacilitiesPopupBySearch(facility);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/facility-master-manage/getFacilities")]
        public async Task<IActionResult> GetFacilitiesBySearch([FromBody] FacilityRequest facility, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacilitiesBySearch(facility);
            if (result.IsSuccess)
            {
                string dataSize = result.Data!=null? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회",dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/api/baseinfo/facility-master-manage/getFacility")]
        public async Task<IActionResult> GetFacilityById([FromBody] FacilityRequest facility, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacility(facility.Id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/facility-master-manage/createFacility")]
        public async Task<IActionResult> CreateFacilityByPost([FromBody] FacilityRequest facility, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateFacility(facility);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/facility-master-manage/updateFacility")]
        public async Task<IActionResult> UpdateFacilityByPost([FromBody] FacilityRequest facility, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateFacility(facility, facility.Id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/facility-master-manage/deleteFacility")]
        public async Task<IActionResult> DeleteFacilityByPost([FromBody] FacilityRequest facility, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteFacilitys(facility.Ids);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }




        //
        [HttpGet("/api/baseinfo/facilityList")]
        public async Task<IActionResult> facilityList([FromBody] FacilityRequest facilityRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.FacilityList(facilityRequest);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpGet("/api/baseinfo/facility-master-manage/{id}")]
        public async Task<IActionResult> GetFacility(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacility(id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/facility-master-manage/all/{code}")]
        public async Task<IActionResult> GetFacilitys(int code, [FromHeader(Name = "Uuid")] string Uuid)
        {
            //Code 는 공통코드. 공통코드에 따라 내보내줍니다.
            var result = await _repo.GetFacilitys(code);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/facility-master-manage")]
        public async Task<IActionResult> CreateFacility([FromBody] FacilityRequest facility, [FromHeader(Name = "Uuid")] string Uuid)
        {

            var result = await _repo.CreateFacility(facility);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPut("/api/baseinfo/facility-master-manage/{id}")]
        public async Task<IActionResult> UpdateFacility([FromBody] FacilityRequest facility, int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateFacility(facility, id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/facility-master-manage/delete")]
        public async Task<IActionResult> DeleteFacility([FromBody] int[] id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteFacilitys(id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/facility-master-manage/file")]
        public async Task<IActionResult> DownloadFacilityList()
        {
            var _facility = await _repo.GetFacilitys(0);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "품목리스트.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("설비");

                worksheet.Cell("A1").Value = "설비코드";
                worksheet.Cell("B1").Value = "설비구분";
                worksheet.Cell("C1").Value = "설비이름";
                worksheet.Cell("D1").Value = "규격";
                worksheet.Cell("E1").Value = "브랜드명";
                worksheet.Cell("F1").Value = "모델명";
                worksheet.Cell("G1").Value = "구매처";

                worksheet.Cell("H1").Value = "구매일";
                worksheet.Cell("I1").Value = "구매금액";
                worksheet.Cell("J1").Value = "UID(IP)";
                worksheet.Cell("K1").Value = "전류최대값";
                worksheet.Cell("L1").Value = "톤최대값";
                worksheet.Cell("M1").Value = "비고";
                worksheet.Cell("N1").Value = "사용여부";

                int i = 0;
                foreach (var facility in _facility.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = facility.Code;
                    worksheet.Cell("B" + (i + 1).ToString()).Value = facility.CommonCodeName;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = facility.Name;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = facility.Standard;
                    worksheet.Cell("E" + (i + 1).ToString()).Value = facility.Brand;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = facility.Model;
                    worksheet.Cell("G" + (i + 1).ToString()).Value = facility.Agent;

                    worksheet.Cell("H" + (i + 1).ToString()).Value = facility.PurchaseDate;
                    worksheet.Cell("I" + (i + 1).ToString()).Value = facility.Price.ToString("C");
                    worksheet.Cell("J" + (i + 1).ToString()).Value = facility.Uid;
                    worksheet.Cell("K" + (i + 1).ToString()).Value = facility.MaxCurrent;
                    worksheet.Cell("L" + (i + 1).ToString()).Value = facility.MaxTon;
                    worksheet.Cell("M" + (i + 1).ToString()).Value = facility.Memo;
                    worksheet.Cell("N" + (i + 1).ToString()).Value = facility.IsUsing ? "사용" : "미사용";
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/baseinfo/facility-master-manage/upload")]
        public async Task<IActionResult> UpdateFacilitys([FromForm] FileModel file)
        {
            var _file = file;
            var stream = file.FormFile.OpenReadStream();

            var workbook = new XLWorkbook(stream);
            var workSheet = workbook.Worksheets.First();
            var range = workSheet.RangeUsed();

            List<FacilityResponse> facility = new List<FacilityResponse>();
            try
            {
                for (int i = 1; i < range.RowCount(); i++)
                {
                    var _facility = new FacilityResponse
                    {
                        Code = workSheet.Cell(i + 1, 1).Value.ToString(),
                        CommonCodeName = workSheet.Cell(i + 1, 2).Value.ToString(),
                        Name = workSheet.Cell(i + 1, 3).Value.ToString(),
                        Standard = workSheet.Cell(i + 1, 4).Value.ToString(),
                        Brand = workSheet.Cell(i + 1, 5).Value.ToString(),
                        Model = workSheet.Cell(i + 1, 6).Value.ToString(),
                        Agent = workSheet.Cell(i + 1, 7).Value.ToString(),
                        PurchaseDate = workSheet.Cell(i + 1, 8).Value.ToString(),
                        Price = 0,//int.Parse(workSheet.Cell(i + 1, 9).Value.ToString(), System.Globalization.NumberStyles.Currency),
                        Uid = workSheet.Cell(i + 1, 10).Value.ToString(),
                        MaxCurrent = workSheet.Cell(i + 1, 11).Value.ToString(),
                        MaxTon = workSheet.Cell(i + 1, 12).Value.ToString(),
                        Memo = workSheet.Cell(i + 1, 13).Value.ToString(),
                        IsUsing = workSheet.Cell(i + 1, 14).Value.ToString() == "사용" ? true : false
                    };
                    facility.Add(_facility);
                }

                if (facility.Count() < 1)
                {
                    return Ok();
                }
                else
                {
                    var result = await _repo.UpdateFacilitys(facility);
                    if (result.IsSuccess)
                        return Ok(result);
                    else
                        return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return Ok();
            }

        }
        #endregion Facility Manage

        #region Process Manage

        [HttpPost("/api/baseinfo/process-master-manage/getProcesses")]
        public async Task<IActionResult> GetProcessesBySearch([FromBody] ProcessRequest proc, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProcessesBySearch(proc);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/api/baseinfo/process-master-manage/getProcessesPopup")]
        public async Task<IActionResult> GetProcessesPopupBySearch([FromBody] ProcessPopupRequest proc, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProcessesPopupBySearch(proc);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        [HttpPost("/api/baseinfo/process-master-manage/getProcess")]
        public async Task<IActionResult> GetProcessByPost([FromBody] ProcessRequest proc, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProcess(proc.Id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/baseinfo/process-master-manage/getProcessFacility")]
        public async Task<IActionResult> GetProcessFacility([FromBody] ProcessRequest proc, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProcessFacility(proc.Id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/process-master-manage/getProcessDownTime")]
        public async Task<IActionResult> GetProcessDownTime([FromBody] ProcessRequest proc, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProcessDownTime(proc.Id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/process-master-manage/getProcessDefective")]
        public async Task<IActionResult> GetProcessDefective([FromBody] ProcessRequest proc, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProcessDefective(proc.Id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }



        [HttpPost("/api/baseinfo/process-master-manage/createProcess")]
        public async Task<IActionResult> CreateProcessByPost([FromBody] ProcessRequest proc, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateProcess(proc);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/process-master-manage/updateProcess")]
        public async Task<IActionResult> UpdateProcessByPost([FromBody] ProcessRequest proc, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateProcess(proc, proc.Id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/process-master-manage/deleteProcess")]
        public async Task<IActionResult> DeleteProcessByPost([FromBody] ProcessRequest proc, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteProcesses(proc.Ids);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }




        [HttpGet("/api/baseinfo/process-master-manage/{id}")]
        public async Task<IActionResult> GetProcess(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProcess(id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/process-master-manage/all/{code}")]
        public async Task<IActionResult> GetProcesses(int code, [FromHeader(Name = "Uuid")] string Uuid)
        {
            //Code 는 공통코드. 공통코드에 따라 내보내줍니다.
            var result = await _repo.GetProcesses(code);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/process-master-manage")]
        public async Task<IActionResult> CreateProcess([FromBody] ProcessRequest process, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateProcess(process);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPut("/api/baseinfo/process-master-manage/{id}")]
        public async Task<IActionResult> UpdateProcess([FromBody] ProcessRequest process, int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateProcess(process, id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/process-master-manage/delete")]
        public async Task<IActionResult> DeleteProcess([FromBody] int[] id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteProcesses(id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/process-master-manage/excel-file")]
        public async Task<IActionResult> DownloadProcessList([FromBody]ProcessRequest req)
        {
            var _processs = await _repo.GetProcesses(0);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "공정리스트.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("공정관리");

                worksheet.Cell("A1").Value = "공정코드";
                worksheet.Cell("B1").Value = "공정구분";
                worksheet.Cell("C1").Value = "공정이름";
                worksheet.Cell("D1").Value = "설비사용여부";
                worksheet.Cell("E1").Value = "공정검사여부";
                worksheet.Cell("F1").Value = "비고";
                worksheet.Cell("G1").Value = "사용여부";

                int i = 0;
                foreach (var process in _processs.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = process.Code;
                    worksheet.Cell("B" + (i + 1).ToString()).Value = process.CommonCodeName;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = process.Name;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = process.FacilityUse ? "사용" : "미사용";
                    worksheet.Cell("E" + (i + 1).ToString()).Value = process.ProcessInspection ? "검사" : "미검사";
                    worksheet.Cell("F" + (i + 1).ToString()).Value = process.Memo;
                    worksheet.Cell("G" + (i + 1).ToString()).Value = process.IsUsing ? "사용" : "미사용";
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/baseinfo/process-master-manage/upload")]
        public async Task<IActionResult> UpdateProcesss([FromForm] FileModel file)
        {
            var _file = file;
            var stream = file.FormFile.OpenReadStream();

            var workbook = new XLWorkbook(stream);
            var workSheet = workbook.Worksheets.First();
            var range = workSheet.RangeUsed();

            List<ProcessResponse> processs = new List<ProcessResponse>();
            try
            {
                for (int i = 1; i < range.RowCount(); i++)
                {
                    var process = new ProcessResponse
                    {
                        Code = workSheet.Cell(i + 1, 1).Value.ToString(),
                        CommonCodeName = workSheet.Cell(i + 1, 2).Value.ToString(),
                        Name = workSheet.Cell(i + 1, 3).Value.ToString(),
                        FacilityUse = workSheet.Cell(i + 1, 4).Value.ToString() == "사용" ? true : false,
                        ProcessInspection = workSheet.Cell(i + 1, 5).Value.ToString() == "검사" ? true : false,
                        Memo = workSheet.Cell(i + 1, 6).Value.ToString(),
                        IsUsing = workSheet.Cell(i + 1, 7).Value.ToString() == "사용" ? true : false,
                    };
                    processs.Add(process);
                }

                if (processs.Count() < 1)
                {
                    return Ok();
                }
                else
                {
                    var result = await _repo.UpdateProcesses(processs);
                    if (result.IsSuccess)
                        return Ok(result);
                    else
                        return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return Ok();
            }

        }


        #endregion Process Manage

        #region Product Manage

        //2022.06.24 BOM QUERY 
        [HttpPost("/api/baseinfo/bom-manage/getBoms")]
        public async Task<IActionResult> GetBomsBySearch([FromBody] ProductRequest prd, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetBomsBySearch(prd);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/api/baseinfo/bom-manage/getBomsPopup")]
        public async Task<IActionResult> GetBomsPopupBySearch([FromBody] ProductPopupRequest prd, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetBomsPopupBySearch(prd);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        //2022.06.24 BOM QUERY 


        [HttpPost("/api/baseinfo/bom-manage/getBom")]
        public async Task<IActionResult> GetBomById([FromBody] ProductRequest prd, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetBomById(prd);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        //2022.06.24 BOM QUERY 
        [HttpPost("/api/baseinfo/bom-manage/updateBom")]
        public async Task<IActionResult> UpdateBom([FromBody] BomUpdateRequest bom, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateBom(bom);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        // PRODUCT
        [HttpPost("/api/baseinfo/bom-manage/getProducts")]
        public async Task<IActionResult> GetProductsBySearch([FromBody] ProductRequest prd, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProductsBySearch(prd);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/api/baseinfo/bom-manage/getProduct")]
        public async Task<IActionResult> GetProductById([FromBody] ProductRequest prd, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProduct(prd.Id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/api/baseinfo/bom-manage/createProduct")]
        public async Task<IActionResult> CreateProductByPost([FromBody] ProductRequest prd, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateProduct(prd);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/api/baseinfo/bom-manage/updateProduct")]
        public async Task<IActionResult> UpdateProductByPost([FromBody] ProductRequest prd, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateProduct(prd, prd.Id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/api/baseinfo/bom-manage/deleteProduct")]
        public async Task<IActionResult> DeleteProductByPost([FromBody] int[] Ids, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteProducts(Ids);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/api/baseinfo/bom-manage/getBom2")]
        public async Task<IActionResult> GetBomById2([FromBody] ProductRequest prd, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetBomById2(prd);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/api/baseinfo/bom-manage/getInputProducts")]
        public async Task<IActionResult> GetInputProducts([FromBody] ProductRequest prd, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetInputProducts(prd);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/api/baseinfo/bom-manage/getProducedProducts")]
        public async Task<IActionResult> GetProducedProducts([FromBody] ProductRequest prd, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProducedProducts(prd);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }






        [HttpGet("/api/baseinfo/bom-manage/{id}")]
        public async Task<IActionResult> GetProduct(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProduct(id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/baseinfo/bom-manage/all/{code}")]
        public async Task<IActionResult> GetProducts(int code, [FromHeader(Name = "Uuid")] string Uuid)
        {
            //Code 는 공통코드. 공통코드에 따라 내보내줍니다.
            var result = await _repo.GetProducts(code);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/bom-manage")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequest item, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateProduct(item);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPut("/api/baseinfo/bom-manage/{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductRequest item, int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateProduct(item, id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/baseinfo/bom-manage/delete")]
        public async Task<IActionResult> DeleteProduct([FromBody] int[] id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteProducts(id);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        #endregion Product Manage
        [HttpPost("/api/baseinfo/process-not-work/getNotWorkPopup")]
        public async Task<IActionResult> GetProcessNotWorksBySearch([FromBody] ProcessNotWorkPopupRequest prd, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProcessNotWorksBySearch(prd);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        [HttpPost("/api/baseinfo/common-code-manage/cleaning-excel-download")]
        public async Task<IActionResult> DownloadCleaningCommonCodeList([FromBody] CommonCodeRequest req)
        {
            var _codes = await _repo.GetCleaningCommonCodes();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "세척항목관리.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("공통코드");

                worksheet.Cell("A1").Value = "공통코드";
                worksheet.Cell("A2").Value = "No";
                worksheet.Cell("B2").Value = "분류코드";
                worksheet.Cell("C2").Value = "분류코드명";
                worksheet.Cell("D2").Value = "공통코드";
                worksheet.Cell("E2").Value = "공통코드명";
                worksheet.Cell("F2").Value = "등록일자";
                worksheet.Cell("G2").Value = "등록자";
                worksheet.Cell("H2").Value = "사용여부";
                worksheet.Cell("I2").Value = "비고";

                int i = 0;
                foreach (var code in _codes.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 2).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 2).ToString()).Value = code.SortCode;
                    worksheet.Cell("C" + (i + 2).ToString()).Value = code.SortCodeName;
                    worksheet.Cell("D" + (i + 2).ToString()).Value = code.Code;
                    worksheet.Cell("E" + (i + 2).ToString()).Value = code.Name;
                    worksheet.Cell("F" + (i + 2).ToString()).Value = code.CreateDate;
                    worksheet.Cell("G" + (i + 2).ToString()).Value = code.Creator;
                    worksheet.Cell("H" + (i + 2).ToString()).Value = code.IsUsing ? "사용" : "미사용";
                    worksheet.Cell("I" + (i + 2).ToString()).Value = code.Memo;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/baseinfo/common-code-manage/repair-excel-download")]
        public async Task<IActionResult> DownloadRepairCommonCodeList([FromBody] CommonCodeRequest req)
        {
            var _codes = await _repo.GetRepairCommonCodes();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "수리항목관리.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("공통코드");

                worksheet.Cell("A1").Value = "공통코드";
                worksheet.Cell("A2").Value = "No";
                worksheet.Cell("B2").Value = "분류코드";
                worksheet.Cell("C2").Value = "분류코드명";
                worksheet.Cell("D2").Value = "공통코드";
                worksheet.Cell("E2").Value = "공통코드명";
                worksheet.Cell("F2").Value = "등록일자";
                worksheet.Cell("G2").Value = "등록자";
                worksheet.Cell("H2").Value = "사용여부";
                worksheet.Cell("I2").Value = "비고";

                int i = 0;
                foreach (var code in _codes.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 2).ToString()).Value = i.ToString();
                    worksheet.Cell("B" + (i + 2).ToString()).Value = code.SortCode;
                    worksheet.Cell("C" + (i + 2).ToString()).Value = code.SortCodeName;
                    worksheet.Cell("D" + (i + 2).ToString()).Value = code.Code;
                    worksheet.Cell("E" + (i + 2).ToString()).Value = code.Name;
                    worksheet.Cell("F" + (i + 2).ToString()).Value = code.CreateDate;
                    worksheet.Cell("G" + (i + 2).ToString()).Value = code.Creator;
                    worksheet.Cell("H" + (i + 2).ToString()).Value = code.IsUsing ? "사용" : "미사용";
                    worksheet.Cell("I" + (i + 2).ToString()).Value = code.Memo;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        #region NOT WORK (비가동 유형)
        [HttpPost("/api/baseinfo/commoncode-manage/getDownTimes")]
        public async Task<IActionResult> GetDownTimes([FromBody]DownTimeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetDownTimes(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        #endregion


        [HttpPost("/api/baseinfo/commoncode-manage/getModels")]
        public async Task<IActionResult> GetModels([FromBody] DownTimeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetModels(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        
        public async Task SmartFactoryLog(string uuid, string type, string dataSize)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(uuid);
                var _company = await _db.BusinessInfo.FirstOrDefaultAsync();
                if(_user != null)
                {
                    string uri = "https://log.smart-factory.kr/apisvc/sendLogData.json?";
                    uri += ("crtfcKey=" + _company.CrtfcKey + "&");
                    uri += ("logDt=" + DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd HH:mm:ss.fff") + "&");
                    uri += ("useSe=" + type + "&");
                    uri += ("conectIp=" + _user.LoginIp + "&");
                    uri += ("sysUser=" + _user.IdNumber + "&");
                    uri += ("dataUsgqty=" + dataSize);
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                    req.Method = "POST";
                    req.ContentType = "application/x-www-form-urlencoded";
                    req.Timeout = 3 * 1000;

                    using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
                    {
                        HttpStatusCode status = res.StatusCode;
                        Stream respStream = res.GetResponseStream();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        
    }
}
