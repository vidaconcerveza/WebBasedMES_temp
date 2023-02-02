using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.BaseInfo;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.BaseInfo;
using WebBasedMES.ViewModels.InspectionRepair;
using WebBasedMES.ViewModels.ProducePlan;

namespace WebBasedMES.Services.Repositories.BaseInfo
{
    public class BaseInfoRepository : IBaseInfoRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public BaseInfoRepository (ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task Save ()
        {
            await _db.SaveChangesAsync();
        }

        #region Department
        public async Task<Response<IEnumerable<DepartmentResponse>>> GetAllDepartments ()
        {
            try
            {
                var _department = await _db.Departments
                    .Select(x => new DepartmentResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    }).ToListAsync();

                var Res = new Response<IEnumerable<DepartmentResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _department
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<DepartmentResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<DepartmentResponse>> GetDepartment (int id)
        {
            try
            {
                var _department = await _db.Departments
                    .Where(x => x.Id == id)
                    .Select(x => new DepartmentResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    }).FirstOrDefaultAsync();


                var Res = new Response<DepartmentResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _department
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<DepartmentResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<bool>> CreateDepartment (DepartmentRequest department, int id)
        {
            try
            {
                var find = await _db.Departments.Where(x => x.Name == department.Name || x.Code == department.Code).ToListAsync();
                if (find.Count > 0)
                {
                    var Res = new Response<bool>()
                    {
                        IsSuccess = false,
                        ErrorMessage = "중복된 이름 또는 코드가 있습니다.",
                        Data = true,
                    };
                    return Res;
                }
                else
                {
                    var _department = new Department()
                    {
                        Name = department.Name,
                        Code = department.Code,
                        Memo = department.Memo,
                    };

                    await _db.Departments.AddAsync(_department);
                    await Save();

                    var Res = new Response<bool>()
                    {
                        IsSuccess = false,
                        ErrorMessage = "",
                        Data = true,
                    };
                    return Res;
                }
            }
            catch (Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = true,
                };
                return Res;
            }
        }
        public async Task<Response<bool>> UpdateDepartment (DepartmentRequest department, int id)
        {
            try
            {
                var _department = await _db.Departments.FindAsync(id);

                _department.Name = department.Name;
                _department.Code = department.Code;
                _department.Memo = department.Memo;


                _db.Departments.Update(_department);
                await Save();

                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = "",
                    Data = true,
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = true,
                };
                return Res;
            }
        }
        public async Task<Response<bool>> DeleteDepartment (int id)
        {
            try
            {
                var _department = await _db.Departments.FindAsync(id);
                _db.Departments.Remove(_department);
                await Save();

                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = "",
                    Data = true,
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = true,
                };
                return Res;
            }
        }
        #endregion Department

        #region Position
        public async Task<Response<IEnumerable<PositionResponse>>> GetAllPositions ()
        {
            try
            {
                var _position = await _db.Positions
                    .Select(x => new PositionResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    }).ToListAsync();

                var Res = new Response<IEnumerable<PositionResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _position
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<PositionResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<PositionResponse>> GetPosition (int id)
        {
            try
            {
                var _position = await _db.Positions
                    .Where(x => x.Id == id)
                    .Select(x => new PositionResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    }).FirstOrDefaultAsync();


                var Res = new Response<PositionResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _position
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<PositionResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<bool>> CreatePosition (PositionRequest position, int id)
        {
            try
            {
                var find = await _db.Positions.Where(x => x.Name == position.Name || x.Code == position.Code).ToListAsync();
                if (find.Count > 0)
                {
                    var Res = new Response<bool>()
                    {
                        IsSuccess = false,
                        ErrorMessage = "중복된 이름 또는 코드가 있습니다.",
                        Data = true,
                    };
                    return Res;
                }
                else
                {
                    var _position = new Position()
                    {
                        Name = position.Name,
                        Code = position.Code,
                        Memo = position.Memo,
                    };

                    await _db.Positions.AddAsync(_position);
                    await Save();

                    var Res = new Response<bool>()
                    {
                        IsSuccess = false,
                        ErrorMessage = "",
                        Data = true,
                    };
                    return Res;
                }
            }
            catch (Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = true,
                };
                return Res;
            }
        }
        public async Task<Response<bool>> UpdatePosition (PositionRequest position, int id)
        {
            try
            {
                var _position = await _db.Positions.FindAsync(id);

                _position.Name = position.Name;
                _position.Code = position.Code;
                _position.Memo = position.Memo;


                _db.Positions.Update(_position);
                await Save();

                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = "",
                    Data = true,
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = true,
                };
                return Res;
            }
        }
        public async Task<Response<bool>> DeletePosition (int id)
        {
            try
            {
                var _position = await _db.Positions.FindAsync(id);
                _db.Positions.Remove(_position);
                await Save();

                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = "",
                    Data = true,
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = true,
                };
                return Res;
            }
        }
        #endregion Position

        #region EmployType
        public async Task<Response<IEnumerable<EmployTypeResponse>>> GetAllEmployTypes ()
        {
            try
            {
                var _employType = await _db.EmployTypes
                    .Select(x => new EmployTypeResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    }).ToListAsync();

                var Res = new Response<IEnumerable<EmployTypeResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _employType
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<EmployTypeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<EmployTypeResponse>> GetEmployType (int id)
        {
            try
            {
                var _employType = await _db.EmployTypes
                    .Where(x => x.Id == id)
                    .Select(x => new EmployTypeResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    }).FirstOrDefaultAsync();


                var Res = new Response<EmployTypeResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _employType
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<EmployTypeResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<bool>> CreateEmployType (EmployTypeRequest employType, int id)
        {
            try
            {
                var find = await _db.EmployTypes.Where(x => x.Name == employType.Name || x.Code == employType.Code).ToListAsync();
                if (find.Count > 0)
                {
                    var Res = new Response<bool>()
                    {
                        IsSuccess = false,
                        ErrorMessage = "중복된 이름 또는 코드가 있습니다.",
                        Data = true,
                    };
                    return Res;
                }
                else
                {
                    var _employType = new EmployType()
                    {
                        Name = employType.Name,
                        Code = employType.Code,
                        Memo = employType.Memo,
                    };

                    await _db.EmployTypes.AddAsync(_employType);
                    await Save();

                    var Res = new Response<bool>()
                    {
                        IsSuccess = false,
                        ErrorMessage = "",
                        Data = true,
                    };
                    return Res;
                }
            }
            catch (Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = true,
                };
                return Res;
            }
        }
        public async Task<Response<bool>> UpdateEmployType (EmployTypeRequest employType, int id)
        {
            try
            {
                var _employType = await _db.EmployTypes.FindAsync(id);

                _employType.Name = employType.Name;
                _employType.Code = employType.Code;
                _employType.Memo = employType.Memo;

                _db.EmployTypes.Update(_employType);
                await Save();

                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = "",
                    Data = true,
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = true,
                };
                return Res;
            }
        }
        public async Task<Response<bool>> DeleteEmployType (int id)
        {
            try
            {
                var _employType = await _db.EmployTypes.FindAsync(id);
                _db.EmployTypes.Remove(_employType);
                await Save();

                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = "",
                    Data = true,
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = true,
                };
                return Res;
            }
        }
        #endregion EmployType

        #region UserRoles
        public async Task<Response<IEnumerable<UserRoleResponse>>> GetAllUserRoles ()
        {
            try
            {
                var _userRole = await _db.UserRoles
                    .Select(x => new UserRoleResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    }).ToListAsync();

                var Res = new Response<IEnumerable<UserRoleResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _userRole
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<UserRoleResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<UserRoleResponse>> GetUserRole (int id)
        {
            try
            {
                var _userRole = await _db.UserRoles
                    .Where(x => x.Id == id)
                    .Select(x => new UserRoleResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    }).FirstOrDefaultAsync();


                var Res = new Response<UserRoleResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _userRole
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<UserRoleResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<bool>> CreateUserRole (UserRoleRequest userRole, int id)
        {
            try
            {
                var find = await _db.UserRoles.Where(x => x.Name == userRole.Name || x.Code == userRole.Code).ToListAsync();
                if (find.Count > 0)
                {
                    var Res = new Response<bool>()
                    {
                        IsSuccess = false,
                        ErrorMessage = "중복된 이름 또는 코드가 있습니다.",
                        Data = true,
                    };
                    return Res;
                }
                else
                {
                    var _userRole = new UserRole()
                    {
                        Name = userRole.Name,
                        Code = userRole.Code,
                        Memo = userRole.Memo,
                    };

                    await _db.UserRoles.AddAsync(_userRole);
                    await Save();

                    var Res = new Response<bool>()
                    {
                        IsSuccess = false,
                        ErrorMessage = "",
                        Data = true,
                    };
                    return Res;
                }
            }
            catch (Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = true,
                };
                return Res;
            }
        }
        public async Task<Response<bool>> UpdateUserRole (UserRoleRequest userRole, int id)
        {
            try
            {
                var _userRole = await _db.UserRoles.FindAsync(id);

                _userRole.Name = userRole.Name;
                _userRole.Code = userRole.Code;
                _userRole.Memo = userRole.Memo;

                _db.UserRoles.Update(_userRole);
                await Save();

                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = "",
                    Data = true,
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = true,
                };
                return Res;
            }
        }
        public async Task<Response<bool>> DeleteUserRole (int id)
        {
            try
            {
                var _userRole = await _db.UserRoles.FindAsync(id);
                _db.UserRoles.Remove(_userRole);
                await Save();

                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = "",
                    Data = true,
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = true,
                };
                return Res;
            }
        }
        #endregion UserRoles

        #region 분류코드 & 공통코드


        public async Task<Response<IEnumerable<SortCode>>> GetSortCodesBySearch(SortCodeRequest _sort)
        {
            try
            {
                var _codes = await _db.SortCodes
                    .Where(x=> _sort.IsUsingStr == "ALL"? true: _sort.IsUsingStr =="Y"? x.IsUsing == true : x.IsUsing==false )
                    .Where(x => x.IsDeleted == false)
                    .Where(x=> x.Name.Contains(_sort.SearchStr) || x.Code.Contains(_sort.SearchStr) || x.Memo.Contains(_sort.SearchStr))
                    .Select(x => new SortCode
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing
                    }).ToListAsync();

                var Res = new Response<IEnumerable<SortCode>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _codes
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<SortCode>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }



        public async Task<Response<IEnumerable<SortCode>>> GetSortCodes ()
        {
            try
            {
                var _codes = await _db.SortCodes
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new SortCode
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        Memo = x.IsUsing ? "(사용)" : "(미사용)",
                        IsUsing = x.IsUsing
                    }).ToListAsync();

                var Res = new Response<IEnumerable<SortCode>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _codes
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<SortCode>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<SortCodeResponse>> GetSortCode (int id)
        {
            try
            {
                if (id > 0)
                {
                    var _code = _db.SortCodes
                        .Where(x => (x.Id == id) && x.IsUsing == true)
                        .Select(x => new SortCodeResponse
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            Memo = x.Memo,
                            CreateDate = x.CreateDate != null ? x.CreateDate.ToString("yyyy-MM-dd") : "",
                            RegisterId = x.Creator != null ? x.Creator.Id : "",
                            RegisterName = x.Creator !=null? x.Creator.UserName : "",
                            IsUsing = x.IsUsing,
                            Uuid = x.Creator != null ? x.Creator.Id : "",
                            CommonCodes = x.CommonCodes
                                .Where(y => y.IsDeleted == false && y.IsUsing == true)
                                .OrderBy(x => x.Code)
                                .Select(y => new CommonCodeResponse
                                {
                                    Id = y.Id,
                                    Code = y.Code,
                                    Name = y.Name,
                                    IsUsing = y.IsUsing,
                                    Memo = y.Memo,
                                    Creator = y.Creator.FullName,
                                    CreateDate = y.CreateDate.ToString("yyyy-MM-dd"),
                                }).ToList()
                        }).FirstOrDefault();


                    var Res = new Response<SortCodeResponse>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _code
                    };

                    return Res;
                }
                else
                {
                    var _code = _db.SortCodes
                        .Where(x => x.IsUsing == true)
                        .Select(x => new SortCodeResponse
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            Memo = x.Memo,
                            CreateDate = x.CreateDate != null ? x.CreateDate.ToString("yyyy-MM-dd") : "",
                            RegisterId = x.Creator != null ? x.Creator.Id : "",
                            RegisterName = x.Creator != null ? x.Creator.UserName : "",
                            IsUsing = x.IsUsing,
                            Uuid = x.Creator != null ? x.Creator.Id : "",
                            CommonCodes = x.CommonCodes
                                .Where(y => y.IsDeleted == false && y.IsUsing == true)
                                .OrderBy(x => x.Code)
                                .Select(y=>new CommonCodeResponse {
                                    Id = y.Id,
                                    Code = y.Code,
                                    Name = y.Name,
                                    IsUsing = y.IsUsing,
                                    Memo = y.Memo,
                                    Creator = y.Creator.FullName,
                                    CreateDate = y.CreateDate.ToString("yyyy-MM-dd"),
                                }).ToList()

                        }).FirstOrDefault();


                    var Res = new Response<SortCodeResponse>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _code
                    };

                    return Res;
                }

            }
            catch (Exception ex)
            {
                var Res = new Response<SortCodeResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<bool>> CreateSortCode(SortCodeRequest req)
        {
            try
            {
                string _code = req.Code;

                if (req.AutoCode)
                {
                    var prevCodes = await _db.SortCodes
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code.Length == 5)
                        .Select(y => y.Code.Substring(1, 5))
                        .OrderByDescending(y => y)
                        .FirstOrDefaultAsync();

                    if (prevCodes == null)
                    {
                        _code = "G0001";
                    }
                    else
                    {
                        try
                        {
                            _code = "G"+(Convert.ToInt32(prevCodes) + 1).ToString("0000");
                        }
                        catch
                        {
                            _code = "G0001";
                        }
                    }
                }


                if (!req.AutoCode)
                {
                    var codeCheck = await _db.SortCodes
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == req.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<bool>()
                        {
                            IsSuccess = false,
                            ErrorMessage = req.Code + "는 이미 존재하는 코드입니다.",
                            Data = false
                        };

                        return ErrorReturn;
                    }

                }

                var _user = await _userManager.FindByIdAsync(req.RegisterId);

                var _sortCode = new SortCode
                {
                    Code = _code,
                    Name = req.Name,
                    IsUsing = req.IsUsing,
                    Memo = req.Memo,
                    CreateDate = Convert.ToDateTime(req.CreateDate),
                    Creator = _user,
                };


                await _db.SortCodes.AddAsync(_sortCode);
                await Save();

                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = true
                };
                return Res;
            }
            catch(Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = false
                };
                return Res;
            }
        }
        public async Task<Response<bool>> UpdateSortCode(SortCodeRequest req)
        {
            try
            {

                var _sort = await _db.SortCodes.Where(x => x.Id == req.Id).FirstOrDefaultAsync();

                if (req.Code != _sort.Code)
                {
                    var codeCheck = await _db.SortCodes
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == req.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<bool>()
                        {
                            IsSuccess = false,
                            ErrorMessage = req.Code + "는 이미 사용중인 코드입니다.",
                            Data = false
                        };

                        return ErrorReturn;
                    }
                }


                //var _user = await _userManager.FindByIdAsync(code.Uuid);
                var _user = await _userManager.FindByIdAsync(req.RegisterId);

                _sort.Code = req.Code;
                _sort.Name = req.Name;
                _sort.Memo = req.Memo;
                _sort.IsUsing = req.IsUsing;
                _sort.CreateDate = Convert.ToDateTime(req.CreateDate);
                _sort.Creator = _user;

                _db.SortCodes.Update(_sort);
                await Save();


                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = true
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = false
                };

                return Res;
            }
        }

        public async Task<Response<bool>> DeleteSortCode(SortCodeRequest req)
        {
            try
            { 
                foreach(var i in req.Ids)
                {
                    var _sorts = await _db.SortCodes.Where(x => x.Id == i).FirstOrDefaultAsync();
                    _sorts.IsDeleted = true;

                    _db.SortCodes.Update(_sorts);

                }

                await Save();

                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = true
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = false
                };

                return Res;
            }
        }




        public async Task<Response<CommonCodeResponse>> GetCommonCode (int id)
        {
            try
            {
                var _common = await _db.CommonCodes
                    .Include(x=>x.SortCode)
                    .Where(x => x.Id == id && x.IsDeleted == false)
                    .Select(x => new CommonCodeResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        IsUsing = x.IsUsing,
                        Memo = x.Memo,
                        Creator = x.Creator.FullName,
                        RegisterName = x.Creator.FullName,
                        CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                        SortCode = x.SortCode.Code,
                        SortCodeName = x.SortCode.Name
                    })
                    .FirstOrDefaultAsync();

                var Res = new Response<CommonCodeResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _common
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<CommonCodeResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<CommonCodeResponse>>> GetCommonCodes(string code)
        {
            try
            {
                var _common = await _db.CommonCodes
                    .Include(x => x.SortCode)
                    .Where(x => code == "ALL"? true :  x.SortCode.Code == code)
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new CommonCodeResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                        Creator = x.Creator.FullName,
                        IsUsing = x.IsUsing,
                        Memo = x.Memo
                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<CommonCodeResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _common
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<CommonCodeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<IEnumerable<CommonCodeResponse>>> GetCommonCodeByCode(string code)
        {
            try
            {
                var _common = await _db.CommonCodes
                    .Include(x => x.SortCode)
                    .Where(x => x.SortCode.Code == code)
                    .Where(x=>x.IsDeleted == false)
                    .Where(x => x.IsUsing == true)
                    .Select(x => new CommonCodeResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                        Creator = x.Creator.FullName,
                        IsUsing = x.IsUsing,
                        Memo = x.Memo
                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<CommonCodeResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _common
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<CommonCodeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<CommonCodeResponse>>> GetCommonCodesBySearch (CommonCodeRequest _req)
        {
            try
            {
                var _common = await _db.CommonCodes
                    .Include(x => x.SortCode)
                    .Where(x => _req.IsUsingStr == "ALL"?true : _req.IsUsingStr == "Y"?x.IsUsing == true : x.IsUsing==false)
                    .Where(x => x.Name.Contains(_req.SearchStr) || x.Code.Contains(_req.SearchStr) || x.Memo.Contains(_req.SearchStr))
                    .Select(x => new CommonCodeResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                        Creator = x.Creator.FullName,
                        IsUsing = x.IsUsing,
                        Memo = x.Memo
                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<CommonCodeResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _common
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<CommonCodeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<SortCodeResponse>> CreateCommonCode (CommonCodeRequest code)
        {
            try
            {
                string _code = code.Code;

                if (code.AutoCode)
                {
                    var prevCodes = await _db.CommonCodes
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code.Length == 8)
                        .Select(y => y.Code.Substring(0, 8))
                        .OrderByDescending(y => y)
                        .FirstOrDefaultAsync();


                    if (prevCodes == null)
                    {
                        _code = "00000001";
                    }
                    else
                    {
                        try
                        {
                            _code = (Convert.ToInt32(prevCodes) + 1).ToString("00000000");
                        }
                        catch
                        {
                            _code = "00000001";
                        }
                    }
                }


                if (!code.AutoCode)
                {
                    var codeCheck = await _db.Facilitys
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == code.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<SortCodeResponse>()
                        {
                            IsSuccess = false,
                            ErrorMessage = code.Code + "는 이미 존재하는 코드입니다.",
                            Data = null
                        };

                        return ErrorReturn;
                    }

                }



                var _sort = await _db.SortCodes.Include(x => x.CommonCodes).Where(x => x.Code == code.SortCode).FirstOrDefaultAsync();
                var _user = await _userManager.FindByIdAsync(code.Uuid);

                var _common = new CommonCode
                {
                    Code = _code,
                    Name = code.Name,
                    IsUsing = code.IsUsing,
                    Memo = code.Memo,
                    CreateDate = Convert.ToDateTime(code.CreateDate),
                    Creator = _user,
                    SortCode = _sort
                };


                await _db.CommonCodes.AddAsync(_common);
                await Save();



                var result = await _db.SortCodes
                    .Include(x => x.CommonCodes)
                    .Where(x => x.Code == code.SortCode)
                    .Select(x => new SortCodeResponse
                    {
                        Code = x.Code,
                        Name = x.Name,
                        CommonCodes = x.CommonCodes.Where(y => y.IsDeleted == false).Select(y => new CommonCodeResponse
                        {
                            Id = y.Id,
                            Code = y.Code,
                            Name = y.Name,
                            IsUsing = y.IsUsing,
                            Memo = y.Memo,
                            Creator = y.Creator.FullName,
                            CreateDate = y.CreateDate.ToString("yyyy-MM-dd"),
                        }).ToList()
                    }).FirstOrDefaultAsync();


                var Res = new Response<SortCodeResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<SortCodeResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<SortCodeResponse>> UpdateCommonCode (int id, CommonCodeRequest code)
        {
            try
            {

                var _common = await _db.CommonCodes.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (code.Code != _common.Code)
                {
                    var codeCheck = await _db.CommonCodes
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == code.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<SortCodeResponse>()
                        {
                            IsSuccess = false,
                            ErrorMessage = code.Code + "는 이미 사용중인 코드입니다.",
                            Data = null
                        };

                        return ErrorReturn;
                    }
                }


                //var _user = await _userManager.FindByIdAsync(code.Uuid);
                var _sort = await _db.SortCodes.Where(x => x.Code == code.SortCode).FirstOrDefaultAsync();

                _common.Code = code.Code;
                _common.Name = code.Name;
                _common.Memo = code.Memo;
                _common.IsUsing = code.IsUsing;
                _common.CreateDate = Convert.ToDateTime(code.CreateDate);
                _common.SortCode = _sort;

                _db.CommonCodes.Update(_common);
                await Save();


                var result = await _db.SortCodes
                    .Include(x => x.CommonCodes)
                    .Where(x => x.Code == code.SortCode)
                    .Select(x => new SortCodeResponse
                    {
                        Code = x.Code,
                        Name = x.Name,
                        CommonCodes = x.CommonCodes.Where(y => y.IsDeleted == false).Select(y => new CommonCodeResponse
                        {
                            Id = y.Id,
                            Code = y.Code,
                            Name = y.Name,
                            IsUsing = y.IsUsing,
                            Memo = y.Memo,
                            Creator = y.Creator.FullName,
                            CreateDate = y.CreateDate.ToString("yyyy-MM-dd"),
                        }).ToList()
                    }).FirstOrDefaultAsync();

                var Res = new Response<SortCodeResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<SortCodeResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        public async Task<Response<SortCodeResponse>> DeleteCommonCode (int id)
        {
            try
            {
                var _common = await _db.CommonCodes.Include(x => x.SortCode).Where(x => x.Id == id).FirstOrDefaultAsync();
                string _sortCode = _common.SortCode.Code;

                _common.IsDeleted = true;
                _db.CommonCodes.Update(_common);
                await Save();

                var result = await _db.SortCodes
                    .Include(x => x.CommonCodes)
                    .Where(x => x.Code == _sortCode)
                    .Select(x => new SortCodeResponse
                    {
                        Code = x.Code,
                        Name = x.Name,
                        CommonCodes = x.CommonCodes.Where(y => y.IsDeleted == false).Select(y => new CommonCodeResponse
                        {
                            Id = y.Id,
                            Code = y.Code,
                            Name = y.Name,
                            IsUsing = y.IsUsing,
                            Memo = y.Memo,
                            Creator = y.Creator.FullName,
                            CreateDate = y.CreateDate.ToString("yyyy-MM-dd"),
                        }).ToList()
                    }).FirstOrDefaultAsync();

                var Res = new Response<SortCodeResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<SortCodeResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<IEnumerable<CommonCodeResponse>>> DeleteCommonCodes(int[] id)
        {
            try
            {
                List<CommonCode> _commonCodes = new List<CommonCode>();
                foreach (var i in id)
                {
                    var _commonCode = await _db.CommonCodes.Where(x => x.Id == i).FirstOrDefaultAsync();
                    _commonCodes.Add(_commonCode);
                }

                if (_commonCodes.Count > 0)
                {
                    _db.CommonCodes.RemoveRange(_commonCodes);
                    await Save();
                }

                /*
                var result = await _db.CommonCodes
                    .Include(x => x.CommonCode)
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new InspectionTypeResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Type = x.Type,
                        InspectionItem = x.InspectionItem,
                        InspectionMethod = x.InspectionMethod,
                        InspectionStandard = x.InspectionStandard,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                        IsDeleted = x.IsDeleted,
                    })
                    .ToArrayAsync();
                */
                var Res = new Response<IEnumerable<CommonCodeResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<CommonCodeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }

        public async Task<Response<IEnumerable<string>>> UpdateCommonCodes (List<CommonCodeResponse> codes)
        {
            List<string> Results = new List<string>();

            try
            {
                int cnt = 0;
                foreach (var code in codes)
                {
                    cnt++;
                    var _sort = await _db.SortCodes.Where(x => x.Code == code.SortCode).FirstOrDefaultAsync();
                    if (_sort == null)
                    {
                        Results.Add(cnt.ToString() + "라인 : 분류코드 " + code.SortCode + " 는 존재하지 않습니다.");
                        continue;
                    }
                    var _user = await _db.Users.Where(x => x.FullName == code.Creator).FirstOrDefaultAsync();
                    var _common = await _db.CommonCodes.Where(x => x.Code == code.Code).FirstOrDefaultAsync();

                    if (_common == null)
                    {
                        string _code = "";
                        var prevCodes = await _db.CommonCodes
                           .Where(y => y.IsDeleted == false)
                           .Where(y => y.Code.Length == 8)
                           .Select(y => y.Code.Substring(0, 8))
                           .OrderByDescending(y => y)
                           .FirstOrDefaultAsync();


                        if (prevCodes == null)
                        {
                            _code = "00000001";
                        }
                        else
                        {
                            try
                            {
                                _code = (Convert.ToInt32(prevCodes) + 1).ToString("00000000");
                            }
                            catch
                            {
                                _code = "00000001";
                            }
                        }

                        var temp = new CommonCode
                        {
                            Code = _code,
                            Name = code.Name,
                            IsUsing = code.IsUsing,
                            Memo = code.Memo,
                            CreateDate = Convert.ToDateTime(code.CreateDate),
                            Creator = _user,
                            SortCode = _sort
                        };

                        await _db.CommonCodes.AddAsync(temp);
                        await Save();
                        Results.Add(cnt.ToString() + "라인 : 공통코드 " + code.Code + " 생성");
                        continue;
                    }
                    else
                    {
                        if (code.Name == "" || code.Code == "")
                        {
                            Results.Add(cnt.ToString() + "라인 : 데이터 에러(코드 또는 이름이 없습니다)");
                            continue;
                        }
                        //등록자, 최초 등록시간은 패스
                        _common.Code = code.Code;
                        _common.Name = code.Name;
                        _common.IsUsing = code.IsUsing;
                        _common.Memo = code.Memo;
                        _common.SortCode = _sort;

                        _db.CommonCodes.Update(_common);
                        await Save();
                        Results.Add(cnt.ToString() + "라인 : 공통코드 " + code.Code + " 업데이트");

                        continue;
                    }
                }


                var Res = new Response<IEnumerable<string>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = Results
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<string>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = Results
                };

                return Res;
            }
        }

        #endregion 분류코드 & 공통코드

        #region Partner Manage

        public async Task<Response<IEnumerable<PartnerResponse>>> GetPartnersBySearch(PartnerRequest _part)
        {
            try
            {
                var prds = await _db.Partners
                    .Include(x => x.CommonCode)
                    .Where(x => x.IsDeleted == false)
                    .Where(x => _part.IsUsingStr == "ALL" ? true : x.IsUsing == (_part.IsUsingStr == "Y" ? true : false))
                    .Where(x => _part.TypeStr == "ALL" ? true : x.CommonCode.Name == _part.TypeStr)
                    .Where(x => (_part.SearchStr == "매입" || _part.SearchStr == "매출"||_part.SearchStr == "금융" || _part.SearchStr == "기타") ? true :( x.Name.Contains(_part.SearchStr) || x.Code.Contains(_part.SearchStr) || x.BusinessNumber.Contains(_part.SearchStr) || x.President.Contains(_part.SearchStr) || x.Memo.Contains(_part.SearchStr)))
                    .Where(x => _part.SearchStr == "매입" ? x.Group_Buy == true : true)
                    .Where(x => _part.SearchStr == "매출" ? x.Group_Sell == true : true)
                    .Where(x => _part.SearchStr == "금융" ? x.Group_Finance == true : true)
                    .Where(x => _part.SearchStr == "기타" ? x.Group_Etc == true : true)
                    .Select(
                      x => new PartnerResponse
                      {
                          Id = x.Id,
                          CommonCode = x.CommonCode.Name,
                          Code = x.Code,
                          Name = x.Name,
                          President = x.President,
                          BusinessNumber = x.BusinessNumber,
                          PresidentNumber = x.PresidentNumber,
                          TelephoneNumber = x.TelephoneNumber,
                          FaxNumber = x.FaxNumber,
                          Address = x.Address,
                          BusinessType = x.BusinessType,
                          BusinessClass = x.BusinessClass,
                          Memo = x.Memo,
                          Group_Buy = x.Group_Buy,
                          Group_Sell = x.Group_Sell,
                          Group_Finance = x.Group_Finance,
                          Group_Etc = x.Group_Etc,
                          BankName = x.BankName,
                          BankAccount = x.BankAccount,
                          ContactName = x.ContactName,
                          ContactEmail = x.ContactEmail,
                          TaxInfo = x.TaxInfo,
                          IsUsing = x.IsUsing,
                          PartnerType = x.PartnerType,
                          UploadFiles = x.UploadFiles.ToArray(),
                      }
                     )
                    .ToArrayAsync();


                var Res = new Response<IEnumerable<PartnerResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = prds
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<PartnerResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<PartnerResponse>> GetPartner (int id)
        {
            try
            {
                var _partner = await _db.Partners
                    .Where(x => x.Id == id)
                    .Select(x => new PartnerResponse
                    {
                        Id = x.Id,
                        CommonCode = x.CommonCode.Name,
                        Code = x.Code,
                        Name = x.Name,
                        President = x.President,
                        BusinessNumber = x.BusinessNumber,
                        PresidentNumber = x.PresidentNumber,
                        TelephoneNumber = x.TelephoneNumber,
                        FaxNumber = x.FaxNumber,
                        Address = x.Address,
                        BusinessType = x.BusinessType,
                        BusinessClass = x.BusinessClass,
                        Memo = x.Memo,
                        Group_Buy = x.Group_Buy,
                        Group_Sell = x.Group_Sell,
                        Group_Finance = x.Group_Finance,
                        Group_Etc = x.Group_Etc,
                        BankName = x.BankName,
                        BankAccount = x.BankAccount,
                        ContactName = x.ContactName,
                        ContactEmail = x.ContactEmail,
                        TaxInfo = x.TaxInfo,
                        IsUsing = x.IsUsing,
                        PartnerType = x.PartnerType,
                        UploadFiles = x.UploadFiles.ToArray(),
                    })
                    .FirstOrDefaultAsync();

                var Res = new Response<PartnerResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _partner
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<PartnerResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<PartnerResponse>>> GetPartners ()
        {
            try
            {
                var _partner = await _db.Partners
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new PartnerResponse
                    {
                        Id = x.Id,
                        CommonCode = x.CommonCode.Name,
                        Code = x.Code,
                        Name = x.Name,
                        President = x.President,
                        BusinessNumber = x.BusinessNumber,
                        PresidentNumber = x.PresidentNumber,
                        TelephoneNumber = x.TelephoneNumber,
                        FaxNumber = x.FaxNumber,
                        Address = x.Address,
                        BusinessType = x.BusinessType,
                        BusinessClass = x.BusinessClass,
                        Memo = x.Memo,
                        Group_Buy = x.Group_Buy,
                        Group_Sell = x.Group_Sell,
                        Group_Finance = x.Group_Finance,
                        Group_Etc = x.Group_Etc,
                        BankName = x.BankName,
                        BankAccount = x.BankAccount,
                        ContactName = x.ContactName,
                        ContactEmail = x.ContactEmail,
                        TaxInfo = x.TaxInfo,
                        PartnerType = x.PartnerType,

                        IsUsing = x.IsUsing,
                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<PartnerResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _partner
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<PartnerResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<PartnerResponse>>> CreatePartner (PartnerRequest x)
        {
            try
            {
                var _common = await _db.CommonCodes.Where(y => y.Name == x.PartnerType).FirstOrDefaultAsync();
                string _code = x.Code;

                if (x.AutoCode)
                {
                    var prevCodes = await _db.Partners
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code.Length == 5)
                        .Where(y => y.Code.Substring(0, 1) == "C")
                        .Select(y => y.Code.Substring(1, 4))
                        .OrderByDescending(y => y)
                        .FirstOrDefaultAsync();
                        

                    if(prevCodes == null)
                    {
                        _code = "C0001";
                    }
                    else
                    {
                        try
                        {
                            _code = "C" + (Convert.ToInt32(prevCodes) + 1).ToString("0000");
                        }
                        catch
                        {
                            _code = "C0001";
                        }
                    }
                }


                if (!x.AutoCode)
                {
                    var codeCheck = await _db.Partners
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == x.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturen = new Response<IEnumerable<PartnerResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = x.Code + "는 이미 존재하는 코드입니다.",
                            Data = null
                        };

                        return ErrorReturen;
                    }

                }


                var _partner = new Partner
                {
                    CommonCode = _common,
                    Code = _code,
                    Name = x.Name,
                    
                    President = x.President,
                    BusinessNumber = x.BusinessNumber,
                    PresidentNumber = x.PresidentNumber,
                    TelephoneNumber = x.TelephoneNumber,
                    FaxNumber = x.FaxNumber,
                    Address = x.Address,
                    BusinessType = x.BusinessType,
                    BusinessClass = x.BusinessClass,
                    Memo = x.Memo,
                    Group_Buy = x.Group_Buy,
                    Group_Sell = x.Group_Sell,
                    Group_Finance = x.Group_Finance,
                    Group_Etc = x.Group_Etc,
                    BankName = x.BankName,
                    BankAccount = x.BankAccount,
                    ContactName = x.ContactName,
                    ContactEmail = x.ContactEmail,
                    TaxInfo = x.TaxInfo,
                    IsUsing = x.IsUsing,
                    IsDeleted = x.IsDeleted,
                    PartnerType = x.PartnerType,
                    
                    UploadFiles =  x.UploadFiles == null? null: x.UploadFiles.ToArray(),

                };

                Console.WriteLine(_partner.Code);


                await _db.Partners.AddAsync(_partner);
                await Save();

                var result = await _db.Partners
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new PartnerResponse
                    {
                        Id = x.Id,

                        CommonCode = x.CommonCode.Name,
                        Code = x.Code,
                        Name = x.Name,
                        President = x.President,
                        BusinessNumber = x.BusinessNumber,
                        PresidentNumber = x.PresidentNumber,
                        TelephoneNumber = x.TelephoneNumber,
                        FaxNumber = x.FaxNumber,
                        Address = x.Address,
                        BusinessType = x.BusinessType,
                        BusinessClass = x.BusinessClass,
                        Memo = x.Memo,
                        Group_Buy = x.Group_Buy,
                        Group_Sell = x.Group_Sell,
                        Group_Finance = x.Group_Finance,
                        Group_Etc = x.Group_Etc,
                        BankName = x.BankName,
                        BankAccount = x.BankAccount,
                        ContactName = x.ContactName,
                        ContactEmail = x.ContactEmail,
                        TaxInfo = x.TaxInfo,
                        IsUsing = x.IsUsing,
                        IsDeleted = x.IsDeleted,
                        UploadFiles = x.UploadFiles.ToArray(),
                        PartnerType = x.PartnerType,

                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<PartnerResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<PartnerResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        public async Task<Response<IEnumerable<PartnerResponse>>> UpdatePartner (PartnerRequest partner, int id)
        {
            try
            {
                var _common = await _db.CommonCodes.Where(y => y.Name == partner.PartnerType).FirstOrDefaultAsync();
                var _partner = await _db.Partners.Include(x=>x.UploadFiles).Where(x => x.Id == id).FirstOrDefaultAsync();



                if(partner.Code != _partner.Code)
                {
                    var codeCheck = await _db.Partners
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == partner.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturen = new Response<IEnumerable<PartnerResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = partner.Code + "는 이미 사용중인 코드입니다.",
                            Data = null
                        };

                        return ErrorReturen;
                    }

                }


                _partner.CommonCode = _common;
                _partner.Code = partner.Code;
                _partner.Name = partner.Name;
                _partner.President = partner.President;
                _partner.BusinessNumber = partner.BusinessNumber;
                _partner.PresidentNumber = partner.PresidentNumber;
                _partner.TelephoneNumber = partner.TelephoneNumber;
                _partner.FaxNumber = partner.FaxNumber;
                _partner.Address = partner.Address;
                _partner.BusinessType = partner.BusinessType;
                _partner.BusinessClass = partner.BusinessClass;
                _partner.Memo = partner.Memo;
                _partner.Group_Buy = partner.Group_Buy;
                _partner.Group_Sell = partner.Group_Sell;
                _partner.Group_Finance = partner.Group_Finance;
                _partner.Group_Etc = partner.Group_Etc;
                _partner.BankName = partner.BankName;
                _partner.BankAccount = partner.BankAccount;
                _partner.ContactName = partner.ContactName;
                _partner.ContactEmail = partner.ContactEmail;
                _partner.TaxInfo = partner.TaxInfo;
                _partner.IsUsing = partner.IsUsing;
                _partner.IsDeleted = partner.IsDeleted;

                if (_partner.UploadFiles != null)
                {
                    _db.UploadFiles.RemoveRange(_partner.UploadFiles);
                }

                _partner.UploadFiles = partner.UploadFiles;


                _partner.PartnerType = partner.PartnerType;


                _db.Partners.Update(_partner);
                await Save();

                var result = await _db.Partners
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new PartnerResponse
                    {
                        Id = x.Id,

                        CommonCode = x.CommonCode.Name,
                        Code = x.Code,
                        Name = x.Name,
                        President = x.President,
                        BusinessNumber = x.BusinessNumber,
                        PresidentNumber = x.PresidentNumber,
                        TelephoneNumber = x.TelephoneNumber,
                        FaxNumber = x.FaxNumber,
                        Address = x.Address,
                        BusinessType = x.BusinessType,
                        BusinessClass = x.BusinessClass,
                        Memo = x.Memo,
                        Group_Buy = x.Group_Buy,
                        Group_Sell = x.Group_Sell,
                        Group_Finance = x.Group_Finance,
                        Group_Etc = x.Group_Etc,
                        BankName = x.BankName,
                        BankAccount = x.BankAccount,
                        ContactName = x.ContactName,
                        ContactEmail = x.ContactEmail,
                        TaxInfo = x.TaxInfo,
                        IsUsing = x.IsUsing,
                        IsDeleted = x.IsDeleted,
                        PartnerType = x.PartnerType,

                        UploadFiles = x.UploadFiles.ToArray(),
                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<PartnerResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<PartnerResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<string>>> UpdatePartners (List<PartnerResponse> partners)
        {
            List<string> Results = new List<string>();

            try
            {
                int cnt = 0;
                foreach (var p in partners)
                {
                    cnt++;
                    var _partner = await _db.Partners.Where(x => x.Code == p.Code).FirstOrDefaultAsync();

                    if (_partner == null)
                    {
                        var temp = new Partner
                        {
                            Code = p.Code,
                            Name = p.Name,
                            President = p.President,
                            BusinessNumber = p.BusinessNumber,
                            PresidentNumber = p.PresidentNumber,
                            TelephoneNumber = p.TelephoneNumber,
                            FaxNumber = p.FaxNumber,
                            Address = p.Address,
                            BusinessType = p.BusinessType,
                            BusinessClass = p.BusinessClass,
                            Memo = p.Memo,
                            Group_Buy = p.Group_Buy,
                            Group_Sell = p.Group_Sell,
                            Group_Finance = p.Group_Finance,
                            Group_Etc = p.Group_Etc,
                            BankName = p.BankName,
                            BankAccount = p.BankAccount,
                            ContactName = p.ContactName,
                            ContactEmail = p.ContactEmail,
                            TaxInfo = p.TaxInfo,
                            IsUsing = p.IsUsing,
                            PartnerType = p.PartnerType,
                        };

                        await _db.Partners.AddAsync(temp);
                        await Save();
                        Results.Add(cnt.ToString() + "라인 : 공통코드 " + p.Code + " 생성");
                        continue;
                    }
                    else
                    {
                        if (p.Name == "" || p.Code == "")
                        {
                            Results.Add(cnt.ToString() + "라인 : 데이터 에러(코드 또는 이름이 없습니다)");
                            continue;
                        }
                        _partner.Name = p.Name;
                        _partner.President = p.President;
                        _partner.BusinessNumber = p.BusinessNumber;
                        _partner.PresidentNumber = p.PresidentNumber;
                        _partner.TelephoneNumber = p.TelephoneNumber;
                        _partner.FaxNumber = p.FaxNumber;
                        _partner.Address = p.Address;
                        _partner.BusinessType = p.BusinessType;
                        _partner.BusinessClass = p.BusinessClass;
                        _partner.Memo = p.Memo;
                        _partner.Group_Buy = p.Group_Buy;
                        _partner.Group_Sell = p.Group_Sell;
                        _partner.Group_Finance = p.Group_Finance;
                        _partner.Group_Etc = p.Group_Etc;
                        _partner.BankName = p.BankName;
                        _partner.BankAccount = p.BankAccount;
                        _partner.ContactName = p.ContactName;
                        _partner.ContactEmail = p.ContactEmail;
                        _partner.TaxInfo = p.TaxInfo;
                        _partner.IsUsing = p.IsUsing;
                        _partner.PartnerType = p.PartnerType;

                        _db.Partners.Update(_partner);
                        await Save();
                        Results.Add(cnt.ToString() + "라인 : 공통코드 " + p.Code + " 업데이트");

                        continue;
                    }
                }


                var Res = new Response<IEnumerable<string>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = Results
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<string>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = Results
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<PartnerResponse>>> DeletePartners (int[] id)
        {
            try
            {
                List<Partner> _partners = new List<Partner>();
                foreach (var i in id)
                {
                    var _partner = await _db.Partners.Where(x => x.Id == i).FirstOrDefaultAsync();
                    _partners.Add(_partner);
                }

                if (_partners.Count > 0)
                {
                    _db.Partners.RemoveRange(_partners);
                    await Save();
                }

                var result = await _db.Partners
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new PartnerResponse
                    {
                        Id = x.Id,
                        PartnerType = x.PartnerType,
                        CommonCode = x.CommonCode.Name,
                        Code = x.Code,
                        Name = x.Name,
                        President = x.President,
                        BusinessNumber = x.BusinessNumber,
                        PresidentNumber = x.PresidentNumber,
                        TelephoneNumber = x.TelephoneNumber,
                        FaxNumber = x.FaxNumber,
                        Address = x.Address,
                        BusinessType = x.BusinessType,
                        BusinessClass = x.BusinessClass,
                        Memo = x.Memo,
                        Group_Buy = x.Group_Buy,
                        Group_Sell = x.Group_Sell,
                        Group_Finance = x.Group_Finance,
                        Group_Etc = x.Group_Etc,
                        BankName = x.BankName,
                        BankAccount = x.BankAccount,
                        ContactName = x.ContactName,
                        ContactEmail = x.ContactEmail,
                        TaxInfo = x.TaxInfo,
                        IsUsing = x.IsUsing,
                        IsDeleted = x.IsDeleted,
                        UploadFiles = x.UploadFiles.ToArray()
                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<PartnerResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<PartnerResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
       
        
        #endregion Partner Manage

        #region Item Manage
        public async Task<Response<ItemResponse>> GetItem (int id)
        {
            try
            {
                var _item = await _db.Items
                    .Include(x => x.CommonCode)
                    .Where(x => x.Id == id)
                    .Select(x => new ItemResponse
                    {
                        Id = x.Id,
                        CommonCode = x.CommonCode.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Unit = x.Unit,
                        Standard = x.Standard,
                        TaxType = x.TaxType,
                        BuyPrice = x.BuyPrice,
                        SellPrice = x.SellPrice,
                        OptimumStock = x.OptimumStock,
                        ImportCheck = x.ImportCheck,
                        ExportCheck = x.ExportCheck,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                        IsDeleted = x.IsDeleted,
                        UploadFile = x.UploadFile,
                        Picture = x.UploadFile == null ? "" : x.UploadFile.FileUrl,

                    })
                    .FirstOrDefaultAsync();

                var Res = new Response<ItemResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _item
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<ItemResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<ItemResponse>>> GetItems (int code)
        {
            try
            {
                if (code == 0)
                {
                    var _item = await _db.Products
                         .Include(x => x.CommonCode)
                        .Where(x => x.IsDeleted == false)
                        .Select(x => new ItemResponse
                        {
                            Id = x.Id,
                            CommonCode = x.CommonCode.Id,
                            CommonCodeName = x.CommonCode.Name,
                            Code = x.Code,
                            Name = x.Name,
                            Unit = x.Unit,
                            Standard = x.Standard,
                            TaxType = x.TaxType,
                            BuyPrice = x.BuyPrice,
                            SellPrice = x.SellPrice,
                            OptimumStock = x.OptimumStock,
                            ImportCheck = x.ImportCheck,
                            ExportCheck = x.ExportCheck,
                            Memo = x.Memo,
                            IsUsing = x.IsUsing,
                            IsDeleted = x.IsDeleted,
                            UploadFile = x.UploadFile,
                            Picture = x.UploadFile == null ? "" : x.UploadFile.FileUrl,
                        })
                        .ToArrayAsync();

                    var Res = new Response<IEnumerable<ItemResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _item
                    };
                    return Res;
                }
                else
                {
                    var _item = await _db.Items
                        .Include(x => x.CommonCode)
                        .Where(x => x.IsDeleted == false && x.CommonCode.Id == code)
                        .Select(x => new ItemResponse
                        {
                            Id = x.Id,
                            CommonCode = x.CommonCode.Id,
                            CommonCodeName = x.CommonCode.Name,
                            Code = x.Code,
                            Name = x.Name,
                            Unit = x.Unit,
                            Standard = x.Standard,
                            TaxType = x.TaxType,
                            BuyPrice = x.BuyPrice,
                            SellPrice = x.SellPrice,
                            OptimumStock = x.OptimumStock,
                            ImportCheck = x.ImportCheck,
                            ExportCheck = x.ExportCheck,
                            Memo = x.Memo,
                            IsUsing = x.IsUsing,
                            IsDeleted = x.IsDeleted,
                            UploadFile = x.UploadFile,
                            Picture = x.UploadFile == null ? "" : x.UploadFile.FileUrl,

                        })
                        .ToArrayAsync();

                    var Res = new Response<IEnumerable<ItemResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _item
                    };
                    return Res;
                }
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ItemResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<ItemResponse>>> CreateItem (ItemRequest x)
        {
            try
            {
                var _common = await _db.CommonCodes.Where(y => y.Name == x.CommonCode).FirstOrDefaultAsync();

                var _item = new Item
                {
                    CommonCode = _common,
                    Code = x.Code,
                    Name = x.Name,
                    Unit = x.Unit,
                    Standard = x.Standard,
                    TaxType = x.TaxType,
                    BuyPrice = x.BuyPrice,
                    SellPrice = x.SellPrice,
                    OptimumStock = x.OptimumStock,
                    ImportCheck = x.ImportCheck,
                    ExportCheck = x.ExportCheck,
                    Memo = x.Memo,
                    IsUsing = x.IsUsing,
                    IsDeleted = x.IsDeleted,
                    UploadFile = x.UploadFile

                };

                await _db.Items.AddAsync(_item);
                await Save();

                var result = await _db.Items
                    .Include(x => x.CommonCode)
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new ItemResponse
                    {
                        Id = x.Id,
                        CommonCode = x.CommonCode.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Unit = x.Unit,
                        Standard = x.Standard,
                        TaxType = x.TaxType,
                        BuyPrice = x.BuyPrice,
                        SellPrice = x.SellPrice,
                        OptimumStock = x.OptimumStock,
                        ImportCheck = x.ImportCheck,
                        ExportCheck = x.ExportCheck,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                        IsDeleted = x.IsDeleted,
                        UploadFile = x.UploadFile,
                        Picture = x.UploadFile == null ? "" : x.UploadFile.FileUrl,


                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<ItemResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ItemResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        public async Task<Response<IEnumerable<ItemResponse>>> UpdateItem (ItemRequest item, int id)
        {
            try
            {
                var _common = await _db.CommonCodes.Where(y => y.Name == item.CommonCode).FirstOrDefaultAsync();
                var _item = await _db.Items.Where(x => x.Id == id).FirstOrDefaultAsync();

                _item.CommonCode = _common;
                _item.Code = item.Code;
                _item.Name = item.Name;
                _item.Unit = item.Unit;
                _item.Standard = item.Standard;
                _item.TaxType = item.TaxType;
                _item.BuyPrice = item.BuyPrice;
                _item.SellPrice = item.SellPrice;
                _item.OptimumStock = item.OptimumStock;
                _item.ImportCheck = item.ImportCheck;
                _item.ExportCheck = item.ExportCheck;
                _item.Memo = item.Memo;
                _item.IsUsing = item.IsUsing;
                _item.UploadFile = item.UploadFile;

                _db.Items.Update(_item);
                await Save();

                var result = await _db.Items
                    .Include(x => x.CommonCode)
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new ItemResponse
                    {
                        Id = x.Id,
                        CommonCode = x.CommonCode.Id,
                        CommonCodeName = x.CommonCode.Name,
                        Code = x.Code,
                        Name = x.Name,
                        Unit = x.Unit,
                        Standard = x.Standard,
                        TaxType = x.TaxType,
                        BuyPrice = x.BuyPrice,
                        SellPrice = x.SellPrice,
                        OptimumStock = x.OptimumStock,
                        ImportCheck = x.ImportCheck,
                        ExportCheck = x.ExportCheck,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                        IsDeleted = x.IsDeleted,
                        UploadFile = x.UploadFile,
                        Picture = x.UploadFile == null ? "" : x.UploadFile.FileUrl,
                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<ItemResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ItemResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<string>>> UpdateItems (List<ItemResponse> items)
        {
            List<string> Results = new List<string>();

            try
            {
                int cnt = 0;
                foreach (var x in items)
                {
                    cnt++;
                    var _item = await _db.Products.Include(x => x.CommonCode).Where(i => i.Code == x.Code).FirstOrDefaultAsync();
                    var _common = await _db.CommonCodes.Where(y => y.Name == x.CommonCodeName).FirstOrDefaultAsync();

                    if (_item == null)
                    {
                        var temp = new Product
                        {
                            CommonCode = _common,
                            Code = x.Code,
                            Name = x.Name,
                            Unit = x.Unit,
                            Standard = x.Standard,
                            TaxType = x.TaxType,
                            BuyPrice = x.BuyPrice,
                            SellPrice = x.SellPrice,
                            OptimumStock = x.OptimumStock,
                            ImportCheck = x.ImportCheck,
                            ExportCheck = x.ExportCheck,
                            Memo = x.Memo,
                            IsUsing = x.IsUsing,
                            IsDeleted = x.IsDeleted,
                            //UploadFile = x.UploadFile
                        };

                        await _db.Products.AddAsync(temp);
                        await Save();
                        Results.Add(cnt.ToString() + "라인 : 공통코드 " + x.Code + " 생성");
                        continue;
                    }
                    else
                    {
                        if (x.Name == "" || x.Code == "")
                        {
                            Results.Add(cnt.ToString() + "라인 : 데이터 에러(코드 또는 이름이 없습니다)");
                            continue;
                        }
                        var _commonTemp = await _db.CommonCodes.Where(y => y.Name == x.CommonCodeName).FirstOrDefaultAsync();

                        _item.CommonCode = _commonTemp;
                        _item.Code = x.Code;
                        _item.Name = x.Name;
                        _item.Unit = x.Unit;
                        _item.Standard = x.Standard;
                        _item.TaxType = x.TaxType;
                        _item.BuyPrice = x.BuyPrice;
                        _item.SellPrice = x.SellPrice;
                        _item.OptimumStock = x.OptimumStock;
                        _item.ImportCheck = x.ImportCheck;
                        _item.ExportCheck = x.ExportCheck;
                        _item.Memo = x.Memo;
                        _item.IsUsing = x.IsUsing;
                        //_item.UploadFile = x.UploadFile;

                        _db.Products.Update(_item);
                        await Save();
                        Results.Add(cnt.ToString() + "라인 : 공통코드 " + x.Code + " 업데이트");

                        continue;
                    }
                }


                var Res = new Response<IEnumerable<string>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = Results
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<string>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = Results
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<ItemResponse>>> DeleteItems (int[] id)
        {
            try
            {
                List<Item> _items = new List<Item>();
                foreach (var i in id)
                {
                    var _item = await _db.Items.Include(x => x.CommonCode).Where(x => x.Id == i).FirstOrDefaultAsync();
                    _item.IsDeleted = true;
                    _items.Add(_item);
                }

                if (_items.Count > 0)
                {
                    _db.Items.UpdateRange(_items);
                    await Save();
                }
                var result = await _db.Items
                    .Include(x => x.CommonCode)
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new ItemResponse
                    {
                        Id = x.Id,
                        CommonCode = x.CommonCode.Id,
                        CommonCodeName = x.CommonCode.Name,
                        Code = x.Code,
                        Name = x.Name,
                        Unit = x.Unit,
                        Standard = x.Standard,
                        TaxType = x.TaxType,
                        BuyPrice = x.BuyPrice,
                        SellPrice = x.SellPrice,
                        OptimumStock = x.OptimumStock,
                        ImportCheck = x.ImportCheck,
                        ExportCheck = x.ExportCheck,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                        IsDeleted = x.IsDeleted,
                        UploadFile = x.UploadFile,
                        Picture = x.UploadFile == null ? "" : x.UploadFile.FileUrl,

                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<ItemResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ItemResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        #endregion Item Manage

        #region InspectionType Manage


        public async Task<Response<IEnumerable<InspectionTypeResponse>>> GetInspectionTypesBySearch(InspectionTypeRequest _req)
        {
            try
            {
                var _inspectionType = await _db.InspectionTypes
                    .Where(x => x.IsDeleted == false)
                    .Where(x => _req.IsUsingStr == "ALL" ? true : _req.IsUsingStr == "Y" ? x.IsUsing == true : x.IsUsing == false)
                    .Where(x => _req.TypeStr == "ALL" ? true : x.Type == _req.TypeStr)
                    .Where(x => x.Name.Contains(_req.SearchInput) || x.InspectionItem.Contains(_req.SearchInput) || x.Code.Contains(_req.SearchInput) || x.Memo.Contains(_req.SearchInput) || x.CommonCode.Name.Contains(_req.SearchInput) || x.Type.Contains(_req.SearchInput))
                        .Select(x => new InspectionTypeResponse
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            Type = x.Type,
                            InspectionItem = x.InspectionItem,
                            InspectionMethod = x.InspectionMethod,
                            InspectionStandard = x.InspectionStandard,
                            Memo = x.Memo,
                            IsUsing = x.IsUsing,
                            IsDeleted = x.IsDeleted,
                        })
                        .ToArrayAsync();

                    var Res = new Response<IEnumerable<InspectionTypeResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _inspectionType
                    };
                    return Res;
                
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<InspectionTypeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<InspectionTypeResponse>> GetInspectionType (int id)
        {
            try
            {
                var _inspectionType = await _db.InspectionTypes
                    .Where(x => x.Id == id)
                    .Select(x => new InspectionTypeResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Type = x.Type,
                        InspectionItem = x.InspectionItem,
                        InspectionMethod = x.InspectionMethod,
                        InspectionStandard = x.InspectionStandard,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                        IsDeleted = x.IsDeleted,

                    })
                    .FirstOrDefaultAsync();

                var Res = new Response<InspectionTypeResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _inspectionType
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<InspectionTypeResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<InspectionTypeResponse>>> GetInspectionTypes (int code)
        {
            try
            {
                if (code == 0)
                {
                    var _inspectionType = await _db.InspectionTypes
                        .Where(x => x.IsDeleted == false)
                        .Select(x => new InspectionTypeResponse
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            Type = x.Type,
                            InspectionItem = x.InspectionItem,
                            InspectionMethod = x.InspectionMethod,
                            InspectionStandard = x.InspectionStandard,
                            Memo = x.Memo,
                            IsUsing = x.IsUsing,
                            IsDeleted = x.IsDeleted,
                        })
                        .ToArrayAsync();

                    var Res = new Response<IEnumerable<InspectionTypeResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _inspectionType
                    };
                    return Res;
                }
                else
                {
                    var _inspectionType = await _db.InspectionTypes
                        .Where(x => x.IsDeleted == false)
                        .Select(x => new InspectionTypeResponse
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            Type = x.Type,
                            InspectionItem = x.InspectionItem,
                            InspectionMethod = x.InspectionMethod,
                            InspectionStandard = x.InspectionStandard,
                            Memo = x.Memo,
                            IsUsing = x.IsUsing,
                            IsDeleted = x.IsDeleted,
                        })
                        .ToArrayAsync();

                    var Res = new Response<IEnumerable<InspectionTypeResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _inspectionType
                    };
                    return Res;
                }
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<InspectionTypeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<InspectionTypeResponse>>> CreateInspectionType (InspectionTypeRequest x)
        {
            try
            {
                string _code = x.Code;

                if (x.AutoCode)
                {
                    var prevCodes = await _db.InspectionTypes
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code.Length == 5)
                        .Where(y => y.Code.Substring(0, 1) == "T")
                        .Select(y => y.Code.Substring(1, 4))
                        .OrderByDescending(y => y)
                        .FirstOrDefaultAsync();

                    if (prevCodes == null)
                    {
                        _code = "T0001";
                    }
                    else
                    {
                        try
                        {
                            _code = "T" + (Convert.ToInt32(prevCodes) + 1).ToString("0000");
                        }
                        catch
                        {
                            _code = "T0001";
                        }
                    }
                }

                if (!x.AutoCode)
                {
                    var codeCheck = await _db.InspectionTypes
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == x.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<IEnumerable<InspectionTypeResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = x.Code + "는 이미 존재하는 코드입니다.",
                            Data = null
                        };

                        return ErrorReturn;
                    }

                }


                var _inspectionType = new InspectionType
                {
                    Code = _code,
                    Name = x.Name,
                    Type = x.Type,
                    InspectionItem = x.InspectionItem,
                    InspectionMethod = x.InspectionMethod,
                    InspectionStandard = x.InspectionStandard,
                    Memo = x.Memo,
                    IsUsing = x.IsUsing,
                    IsDeleted = x.IsDeleted,

                };

                await _db.InspectionTypes.AddAsync(_inspectionType);
                await Save();

                var result = await _db.InspectionTypes
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new InspectionTypeResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Type = x.Type,
                        InspectionItem = x.InspectionItem,
                        InspectionMethod = x.InspectionMethod,
                        InspectionStandard = x.InspectionStandard,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                        IsDeleted = x.IsDeleted,

                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<InspectionTypeResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<InspectionTypeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        public async Task<Response<IEnumerable<InspectionTypeResponse>>> UpdateInspectionType (InspectionTypeRequest inspectionType, int id)
        {
            try
            {
                var _inspectionType = await _db.InspectionTypes.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (inspectionType.Code != _inspectionType.Code)
                {
                    var codeCheck = await _db.InspectionTypes
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == inspectionType.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<IEnumerable<InspectionTypeResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = inspectionType.Code + "는 이미 사용중인 코드입니다.",
                            Data = null
                        };

                        return ErrorReturn;
                    }
                }



                _inspectionType.Code = inspectionType.Code;
                _inspectionType.Name = inspectionType.Name;
                _inspectionType.Type = inspectionType.Type;
                _inspectionType.InspectionItem = inspectionType.InspectionItem;
                _inspectionType.InspectionMethod = inspectionType.InspectionMethod;
                _inspectionType.InspectionStandard = inspectionType.InspectionStandard;
                _inspectionType.Memo = inspectionType.Memo;
                _inspectionType.IsUsing = inspectionType.IsUsing;
                _inspectionType.IsDeleted = inspectionType.IsDeleted;

                _db.InspectionTypes.Update(_inspectionType);
                await Save();

                var result = await _db.InspectionTypes
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new InspectionTypeResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Type = x.Type,
                        InspectionItem = x.InspectionItem,
                        InspectionMethod = x.InspectionMethod,
                        InspectionStandard = x.InspectionStandard,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                        IsDeleted = x.IsDeleted,
                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<InspectionTypeResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<InspectionTypeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<string>>> UpdateInspectionTypes (List<InspectionTypeResponse> inspectionTypes)
        {
            List<string> Results = new List<string>();

            try
            {
                int cnt = 0;
                foreach (var x in inspectionTypes)
                {
                    cnt++;
                    var _inspectionType = await _db.InspectionTypes.Where(i => i.Code == x.Code).FirstOrDefaultAsync();
                    if (_inspectionType == null)
                    {
                        var temp = new InspectionType
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            Type = x.Type,
                            InspectionItem = x.InspectionItem,
                            InspectionMethod = x.InspectionMethod,
                            InspectionStandard = x.InspectionStandard,
                            Memo = x.Memo,
                            IsUsing = x.IsUsing,
                            IsDeleted = x.IsDeleted,
                        };

                        await _db.InspectionTypes.AddAsync(temp);
                        await Save();
                        Results.Add(cnt.ToString() + "라인 : 공통코드 " + x.Code + " 생성");
                        continue;
                    }
                    else
                    {
                        if (x.Name == "" || x.Code == "")
                        {
                            Results.Add(cnt.ToString() + "라인 : 데이터 에러(코드 또는 이름이 없습니다)");
                            continue;
                        }

                        _inspectionType.Code = x.Code;
                        _inspectionType.Name = x.Name;
                        _inspectionType.Type = x.Type;
                        _inspectionType.InspectionItem = x.InspectionItem;
                        _inspectionType.InspectionMethod = x.InspectionMethod;
                        _inspectionType.InspectionStandard = x.InspectionStandard;
                        _inspectionType.Memo = x.Memo;
                        _inspectionType.IsUsing = x.IsUsing;
                        _inspectionType.IsDeleted = x.IsDeleted;


                        _db.InspectionTypes.Update(_inspectionType);
                        await Save();
                        Results.Add(cnt.ToString() + "라인 : 공통코드 " + x.Code + " 업데이트");

                        continue;
                    }
                }


                var Res = new Response<IEnumerable<string>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = Results
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<string>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = Results
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<InspectionTypeResponse>>> DeleteInspectionTypes (int[] id)
        {
            try
            {
                List<InspectionType> _inspectionTypes = new List<InspectionType>();
                foreach (var i in id)
                {
                    var _inspectionType = await _db.InspectionTypes.Include(x => x.CommonCode).Where(x => x.Id == i).FirstOrDefaultAsync();
                    _inspectionTypes.Add(_inspectionType);
                }

                if (_inspectionTypes.Count > 0)
                {
                    foreach(var i in _inspectionTypes)
                    {
                        i.IsDeleted = true;
                        _db.InspectionTypes.Update(i);
                    }

                    await Save();
                }
                var result = await _db.InspectionTypes
                    .Include(x => x.CommonCode)
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new InspectionTypeResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Type = x.Type,
                        InspectionItem = x.InspectionItem,
                        InspectionMethod = x.InspectionMethod,
                        InspectionStandard = x.InspectionStandard,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                        IsDeleted = x.IsDeleted,
                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<InspectionTypeResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<InspectionTypeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        #endregion InspectionType Manage

        #region InspectionInspectionItem Manage


        public async Task<Response<IEnumerable<InspectionItemResponse>>> GetInspectionItemsBySearch(InspectionItemRequest _req)
        {
            try
            {
                var _inspectionType = await _db.InspectionItems
                    .Where(x => x.IsDeleted == false)
                    .Where(x => _req.IsUsingStr == "ALL" ? true : _req.IsUsingStr == "Y" ? x.IsUsing == true : x.IsUsing == false)
                    //.Where(x => _req.Type == "ALL"? true : _req.Type == "일상점검" ? x.Type == "일상점검" : x.Type == "정기점검")
                    .Where(x => _req.TypeStr == "ALL" ? true : x.Type == _req.TypeStr)
                    .Where(x => x.Name.Contains(_req.SearchInput) || x.Code.Contains(_req.SearchInput) || x.Memo.Contains(_req.SearchInput) || x.Type.Contains(_req.SearchInput))
                        .Select(x => new InspectionItemResponse
                        {
                            Id = x.Id,
                            CommonCode = x.Type == "일상점검" ? x.CommonCode.Id : 0,
                            CommonCodeName = x.Type == "일상점검" ? x.CommonCode.Name : "-",
                            Code = x.Code,
                            Name = x.Name,
                            Classify = x.Classify,
                            Type = x.Type,
                            InspectionType = x.InspectionType,
                            InspectionCount = x.InspectionCount.ToString(),
                            InspectionItem = x.InspectionItems,
                            JudgeStandard = x.JudgeStandard,
                            JudgeMethod = x.JudgeMethod,
                            IsUsing = x.IsUsing,
                            Memo = x.Memo,
                            Creator = x.Creator.FullName,
                            CreateDateTime = x.CreateDateTime.ToString("yyyy-MM-dd")
                        })
                        .ToArrayAsync();

                var Res = new Response<IEnumerable<InspectionItemResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _inspectionType
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<InspectionItemResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<InspectionItemResponse>> GetInspectionItem (int id)
        {
            try
            {
                var _item = await _db.InspectionItems
                    .Include(x => x.CommonCode)
                    .Where(x => x.Id == id)
                    .Select(x => new InspectionItemResponse
                    {
                        Id = x.Id,
                        CommonCode = x.Type == "일상점검" ? x.CommonCode.Id : 0,
                        CommonCodeName = x.Type == "일상점검" ? x.CommonCode.Name : "-",
                        Code = x.Code,
                        Name = x.Name,
                        Classify = x.Classify,
                        Type = x.Type,
                        InspectionType = x.InspectionType,
                        InspectionCount = x.Type == "일상점검" ? x.InspectionCount.ToString() : "-",
                        InspectionItem = x.InspectionItems,
                        JudgeStandard = x.JudgeStandard,
                        JudgeMethod = x.JudgeMethod,
                        IsUsing = x.IsUsing,
                        Memo = x.Memo,
                        Creator = x.Creator.FullName,
                        CreateDateTime = x.CreateDateTime.ToString("yyyy-MM-dd")
                    })
                    .FirstOrDefaultAsync();

                var Res = new Response<InspectionItemResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _item
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<InspectionItemResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<InspectionItemResponse>>> GetInspectionItems (int code)
        {
            try
            {

                    var _item = await _db.InspectionItems
                         .Include(x => x.CommonCode)
                        .Where(x => x.IsDeleted == false)
                        .Where(x => code == 0? true:code ==1? x.Type == "일상점검":x.Type =="정기점검")
                        .Select(x => new InspectionItemResponse
                        {
                            Id = x.Id,
                            CommonCode = x.Type == "일상점검" ? x.CommonCode.Id : 0,
                            CommonCodeName = x.Type == "일상점검" ? x.CommonCode.Name : "-",
                            Code = x.Code,
                            Name = x.Name,
                            Classify = x.Classify,
                            Type = x.Type,
                            InspectionType = x.InspectionType,
                            InspectionCount = x.Type == "일상점검" ? x.InspectionCount.ToString() : "-",
                            InspectionItem = x.InspectionItems,
                            JudgeStandard = x.JudgeStandard,
                            JudgeMethod = x.JudgeMethod,
                            IsUsing = x.IsUsing,
                            Memo = x.Memo,
                            Creator = x.Creator.FullName,
                            CreateDateTime = x.CreateDateTime.ToString("yyyy-MM-dd")
                        })
                        .ToArrayAsync();

                    var Res = new Response<IEnumerable<InspectionItemResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _item
                    };
                    return Res;
                

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<InspectionItemResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<InspectionItemResponse>>> CreateInspectionItem (InspectionItemRequest x)
        {
            try
            {
                var _common = await _db.CommonCodes.Where(y => y.Id == x.CommonCode).FirstOrDefaultAsync();
                var _user = await _userManager.FindByIdAsync(x.Creator);

                string _code = x.Code;

                if (x.AutoCode)
                {
                    var prevCodes = await _db.InspectionItems
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code.Length == 5)
                        .Where(y => y.Code.Substring(0, 1) == "S")
                        .Select(y => y.Code.Substring(1, 4))
                        .OrderByDescending(y => y)
                        .FirstOrDefaultAsync();


                    if (prevCodes == null)
                    {
                        _code = "S0001";
                    }
                    else
                    {
                        try
                        {
                            _code = "S" + (Convert.ToInt32(prevCodes) + 1).ToString("0000");
                        }
                        catch
                        {
                            _code = "S0001";
                        }
                    }
                }


                if (!x.AutoCode)
                {
                    var codeCheck = await _db.InspectionItems
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == x.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<IEnumerable<InspectionItemResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = x.Code + "는 이미 존재하는 코드입니다.",
                            Data = null
                        };

                        return ErrorReturn;
                    }

                }




                var _item = new InspectionItem
                {
                    CommonCode = _common,
                    Code = _code,
                    Name = x.Name,
                    Classify = x.Classify,
                    Type = x.Type,
                    InspectionType = x.InspectionType,
                    InspectionCount = x.InspectionCount,
                    InspectionItems = x.InspectionItem,
                    JudgeStandard = x.JudgeStandard,
                    JudgeMethod = x.JudgeMethod,
                    IsUsing = x.IsUsing,
                    Memo = x.Memo,
                //    Creator = _user,
                //    CreateDateTime = Convert.ToDateTime(x.CreateDateTime),
                };

                await _db.InspectionItems.AddAsync(_item);
                await Save();

                await Save();


                  var result = await _db.InspectionItems
                         .Include(x => x.CommonCode)
                        .Where(x => x.IsDeleted == false)
                        .Select(x => new InspectionItemResponse
                        {
                            Id = x.Id,
                            CommonCode = x.Type == "일상점검" ? x.CommonCode.Id : 0,
                            CommonCodeName = x.Type == "일상점검" ? x.CommonCode.Name : "-",
                            Code = x.Code,
                            Name = x.Name,
                            Classify = x.Classify,
                            Type = x.Type,
                            InspectionType = x.InspectionType,
                            InspectionCount = x.Type == "일상점검" ? x.InspectionCount.ToString() : "-",
                            InspectionItem = x.InspectionItems,
                            JudgeStandard = x.JudgeStandard,
                            JudgeMethod = x.JudgeMethod,
                            IsUsing = x.IsUsing,
                            Memo = x.Memo,
                            Creator = x.Creator.FullName,
                            CreateDateTime = x.CreateDateTime.ToString("yyyy-MM-dd")
                        })
                        .ToArrayAsync();


                var Res = new Response<IEnumerable<InspectionItemResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<InspectionItemResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        public async Task<Response<IEnumerable<InspectionItemResponse>>> UpdateInspectionItem (InspectionItemRequest item, int id)
        {
            try
            {
                var _common = await _db.CommonCodes.Where(y => y.Id == item.CommonCode).FirstOrDefaultAsync();
                var _item = await _db.InspectionItems.Where(x => x.Id == id).FirstOrDefaultAsync();


                if (item.Code != _item.Code)
                {
                    var codeCheck = await _db.InspectionItems
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == item.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<IEnumerable<InspectionItemResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = item.Code + "는 이미 사용중인 코드입니다.",
                            Data = null
                        };

                        return ErrorReturn;
                    }
                }


                _item.CommonCode = _common;
                _item.Code = item.Code;
                _item.Name = item.Name;
                _item.Type = item.Type;
                _item.Classify = item.Classify;
                _item.InspectionType = item.InspectionType;
                _item.InspectionCount = item.InspectionCount;
                _item.InspectionItems = item.InspectionItem;
                _item.JudgeStandard = item.JudgeStandard;
                _item.JudgeMethod = item.JudgeMethod;
                _item.IsUsing = item.IsUsing;
                _item.IsUsing = item.IsUsing;
                _item.Memo = item.Memo;
               // _item.CreateDateTime = Convert.ToDateTime(item.CreateDateTime);

                _db.InspectionItems.Update(_item);
                await Save();
                var result = await _db.InspectionItems
                       .Include(x => x.CommonCode)
                      .Where(x => x.IsDeleted == false)
                      .Select(x => new InspectionItemResponse
                      {
                          Id = x.Id,
                          CommonCode = x.Type == "일상점검" ? x.CommonCode.Id : 0,
                          CommonCodeName = x.Type == "일상점검" ? x.CommonCode.Name : "-",
                          Code = x.Code,
                          Name = x.Name,
                          Classify = x.Classify,
                          Type = x.Type,
                          InspectionType = x.InspectionType,
                          InspectionCount = x.Type == "일상점검" ? x.InspectionCount.ToString() : "-",
                          InspectionItem = x.InspectionItems,
                          JudgeStandard = x.JudgeStandard,
                          JudgeMethod = x.JudgeMethod,
                          IsUsing = x.IsUsing,
                          Memo = x.Memo,
                          Creator = x.Creator.FullName,
                          CreateDateTime = x.CreateDateTime.ToString("yyyy-MM-dd")
                      })
                      .ToArrayAsync();


                var Res = new Response<IEnumerable<InspectionItemResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<InspectionItemResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<string>>> UpdateInspectionItems (List<InspectionItemResponse> items)
        {
            List<string> Results = new List<string>();

            try
            {
                int cnt = 0;
                foreach (var x in items)
                {
                    cnt++;
                    var _item = await _db.InspectionItems.Include(x => x.CommonCode).Where(i => i.Code == x.Code).FirstOrDefaultAsync();
                    var _common = await _db.CommonCodes.Where(y => y.Name == x.CommonCodeName).FirstOrDefaultAsync();

                    if (_item == null)
                    {
                        var temp = new InspectionItem
                        {
                            CommonCode = _common,
                            Code = x.Code,
                            Classify = x.Classify,
                            Type = x.Type,
                            InspectionItems = x.InspectionItem,
                            JudgeStandard = x.JudgeStandard,
                            JudgeMethod = x.JudgeMethod,
                            IsUsing = x.IsUsing,
                            Memo = x.Memo,
                        };


                        await _db.InspectionItems.AddAsync(temp);
                        await Save();
                        Results.Add(cnt.ToString() + "라인 : 공통코드 " + x.Code + " 생성");
                        continue;
                    }
                    else
                    {
                        if (x.Name == "" || x.Code == "")
                        {
                            Results.Add(cnt.ToString() + "라인 : 데이터 에러(코드 또는 이름이 없습니다)");
                            continue;
                        }
                        var _commonTemp = await _db.CommonCodes.Where(y => y.Name == x.CommonCodeName).FirstOrDefaultAsync();

                        _item.CommonCode = (_item.CommonCode != null && _commonTemp == null) ? _item.CommonCode : _commonTemp;
                        _item.Code = x.Code;
                        _item.Classify = x.Classify;
                        _item.Type = x.Type;
                        _item.InspectionItems = x.InspectionItem;
                        _item.JudgeStandard = x.JudgeStandard;
                        _item.JudgeMethod = x.JudgeMethod;
                        _item.Memo = x.Memo;
                        _item.IsUsing = x.IsUsing;

                        _db.InspectionItems.Update(_item);
                        await Save();
                        Results.Add(cnt.ToString() + "라인 : 공통코드 " + x.Code + " 업데이트");

                        continue;
                    }
                }


                var Res = new Response<IEnumerable<string>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = Results
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<string>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = Results
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<InspectionItemResponse>>> DeleteInspectionItems (int[] id)
        {
            try
            {
                List<InspectionItem> _items = new List<InspectionItem>();
                foreach (var i in id)
                {
                    var _item = await _db.InspectionItems.Include(x => x.CommonCode).Where(x => x.Id == i).FirstOrDefaultAsync();
                    _items.Add(_item);
                }

                if (_items.Count > 0)
                {
                    _db.InspectionItems.RemoveRange(_items);
                    await Save();
                }
                var result = await _db.InspectionItems
                       .Include(x => x.CommonCode)
                      .Where(x => x.IsDeleted == false)
                      .Select(x => new InspectionItemResponse
                      {
                          Id = x.Id,
                          CommonCode = x.Type == "일상점검" ? x.CommonCode.Id : 0,
                          CommonCodeName = x.Type == "일상점검" ? x.CommonCode.Name : "-",
                          Code = x.Code,
                          Name = x.Name,
                          Classify = x.Classify,
                          Type = x.Type,
                          InspectionType = x.InspectionType,
                          InspectionCount = x.Type == "일상점검" ? x.InspectionCount.ToString() : "-",
                          InspectionItem = x.InspectionItems,
                          JudgeStandard = x.JudgeStandard,
                          JudgeMethod = x.JudgeMethod,
                          IsUsing = x.IsUsing,
                          Memo = x.Memo,
                          Creator = x.Creator.FullName,
                          CreateDateTime = x.CreateDateTime.ToString("yyyy-MM-dd")
                      })
                      .ToArrayAsync();


                var Res = new Response<IEnumerable<InspectionItemResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<InspectionItemResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };
                return Res;
            }
        }
        #endregion InspectionType Manage

        #region Defective Manage
        public async Task<Response<DefectiveResponse>> GetDefective (int id)
        {
            try
            {
                var _defective = await _db.Defectives
                    .Where(x => x.Id == id)
                    .Select(x => new DefectiveResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Creator = x.Creator.FullName,
                        CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                        IsDeleted = x.IsDeleted,
                    })
                    .FirstOrDefaultAsync();

                var Res = new Response<DefectiveResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _defective
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<DefectiveResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<DefectiveResponse>>> GetDefectivesBySearch(DefectiveRequest _def)
        {
            try
            {
                var _defective = await _db.Defectives
                    .Where(x => x.IsDeleted == false)
                    .Where(x => _def.IsUsingStr == "ALL" ? true : _def.IsUsingStr == "Y" ? x.IsUsing == true : x.IsUsing == false)
                    .Where(x => x.Name.Contains(_def.SearchStr) || x.Code.Contains(_def.SearchStr) || x.Memo.Contains(_def.SearchStr) || x.Creator.FullName.Contains(_def.SearchStr))
                    .Select(x => new DefectiveResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Creator = x.Creator.FullName,
                        CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                        IsDeleted = x.IsDeleted
                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<DefectiveResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _defective
                };
                return Res;
            }
            catch(Exception ex)
            {
                var Res = new Response<IEnumerable<DefectiveResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };
                return Res;
            }
        }

        public async Task<Response<IEnumerable<DefectiveResponse>>> GetDefectives (int code)
        {
            try
            {
                if (code == 0)
                {
                    var _defective = await _db.Defectives
                        .Where(x => x.IsDeleted == false)
                        .Select(x => new DefectiveResponse
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            Creator = x.Creator.FullName,
                            CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                            Memo = x.Memo,
                            IsUsing = x.IsUsing,
                            IsDeleted = x.IsDeleted
                        })
                        .ToArrayAsync();

                    var Res = new Response<IEnumerable<DefectiveResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _defective
                    };
                    return Res;
                }
                else
                {
                    var _defective = await _db.Defectives
                        .Where(x => x.IsDeleted == false)
                        .Select(x => new DefectiveResponse
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            Creator = x.Creator.FullName,
                            CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                            Memo = x.Memo,
                            IsUsing = x.IsUsing,
                            IsDeleted = x.IsDeleted
                        })
                        .ToArrayAsync();

                    var Res = new Response<IEnumerable<DefectiveResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _defective
                    };
                    return Res;
                }
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<DefectiveResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<DefectiveResponse>>> CreateDefective (DefectiveRequest x)
        {
            try
            {
                string _code = x.Code;

                if (x.AutoCode)
                {
                    var prevCodes = await _db.Defectives
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code.Length == 5)
                        .Where(y => y.Code.Substring(0, 1) == "D")
                        .Select(y => y.Code.Substring(1, 4))
                        .OrderByDescending(y => y)
                        .FirstOrDefaultAsync();


                    if (prevCodes == null)
                    {
                        _code = "D0001";
                    }
                    else
                    {
                        try
                        {
                            _code = "D" + (Convert.ToInt32(prevCodes) + 1).ToString("0000");
                        }
                        catch
                        {
                            _code = "D0001";
                        }
                    }
                }


                if (!x.AutoCode)
                {
                    var codeCheck = await _db.Partners
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == x.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturen = new Response<IEnumerable<DefectiveResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = x.Code + "는 이미 존재하는 코드입니다.",
                            Data = null
                        };

                        return ErrorReturen;
                    }

                }




                var _user = await _userManager.FindByIdAsync(x.Uuid);
                var _defective = new Defective
                {
                    Code = _code,
                    Name = x.Name,
                    Creator = _user,
                    CreateDate = Convert.ToDateTime(x.CreateDate),
                    Memo = x.Memo,
                    IsUsing = x.IsUsing,
                };

                await _db.Defectives.AddAsync(_defective);
                await Save();

                var result = await _db.Defectives
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new DefectiveResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Creator = x.Creator.FullName,
                        CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<DefectiveResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<DefectiveResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        public async Task<Response<IEnumerable<DefectiveResponse>>> UpdateDefective (DefectiveRequest defective, int id)
        {
            try
            {
                var _defective = await _db.Defectives.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (defective.Code != _defective.Code)
                {
                    var codeCheck = await _db.Defectives
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == defective.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<IEnumerable<DefectiveResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = defective.Code + "는 이미 사용중인 코드입니다.",
                            Data = null
                        };

                        return ErrorReturn;
                    }

                }


                _defective.Code = defective.Code;
                _defective.Name = defective.Name;
                _defective.Memo = defective.Memo;
                _defective.IsUsing = defective.IsUsing;

                _db.Defectives.Update(_defective);
                await Save();

                var result = await _db.Defectives
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new DefectiveResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Creator = x.Creator.FullName,
                        CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<DefectiveResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<DefectiveResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<string>>> UpdateDefectives (List<DefectiveResponse> defectives)
        {
            List<string> Results = new List<string>();

            try
            {
                int cnt = 0;
                foreach (var x in defectives)
                {
                    cnt++;
                    var _defective = await _db.Defectives.Where(i => i.Code == x.Code).FirstOrDefaultAsync();
                    if (_defective == null)
                    {
                        var _temp = new Defective
                        {
                            Code = x.Code,
                            Name = x.Name,
                            Creator = null,
                            CreateDate = DateTime.UtcNow.AddHours(9),
                            Memo = x.Memo,
                            IsUsing = x.IsUsing,
                        };

                        await _db.Defectives.AddAsync(_temp);
                        await Save();
                        Results.Add(cnt.ToString() + "라인 : 공통코드 " + x.Code + " 생성");
                        continue;
                    }
                    else
                    {
                        if (x.Name == "" || x.Code == "")
                        {
                            Results.Add(cnt.ToString() + "라인 : 데이터 에러(코드 또는 이름이 없습니다)");
                            continue;
                        }

                        _defective.Code = x.Code;
                        _defective.Name = x.Name;
                        _defective.Memo = x.Memo;
                        _defective.IsUsing = x.IsUsing;

                        _db.Defectives.Update(_defective);
                        await Save();
                        Results.Add(cnt.ToString() + "라인 : 공통코드 " + x.Code + " 업데이트");

                        continue;
                    }
                }


                var Res = new Response<IEnumerable<string>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = Results
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<string>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = Results
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<DefectiveResponse>>> DeleteDefectives (int[] id)
        {
            try
            {
                List<Defective> _defectives = new List<Defective>();
                foreach (var i in id)
                {
                    var _defective = await _db.Defectives.Include(x => x.CommonCode).Where(x => x.Id == i).FirstOrDefaultAsync();
                    _defective.IsDeleted = true;

                    _db.Defectives.Update(_defective);
                }

                await Save();

 
                var result = await _db.Defectives
                    .Include(x => x.CommonCode)
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new DefectiveResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Creator = x.Creator.FullName,
                        CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<DefectiveResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<DefectiveResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        #endregion Defective Manage

        #region Facility Manage



        public async Task<Response<IEnumerable<FacilityResponse>>> GetFacilitiesBySearch(FacilityRequest _req)
        {
            try
            {
                var _facilitys = await _db.Facilitys
                    .Include(x => x.CommonCode)
                    .Where(x => _req.IsUsingStr == "ALL" ? true : (_req.IsUsingStr == "Y" ? x.IsUsing == true : x.IsUsing == false))
                    .Where(x => (_req.TypeStr == null  || _req.TypeStr == "ALL")? true : x.CommonCode.Name == _req.TypeStr)
                    .Where(x => x.Name.Contains(_req.SearchInput) || x.Code.Contains(_req.SearchInput) | x.Memo.Contains(_req.SearchInput) || x.Uid.Contains(_req.SearchInput))
                    .Where(x => x.IsDeleted == false)
                    .OrderBy(x => x.Code).Reverse()
                    .Select(x => new FacilityResponse()
                    {
                        Id = x.Id,
                        CommonCode = x.CommonCode.Id,
                        CommonCodeName = x.CommonCode.Name,
                        Type = x.CommonCode.Name,
                        Code = x.Code,
                        Name = x.Name,
                        Standard = x.Standard,
                        Brand = x.Brand,
                        Model = x.Model,
                        Agent = x.Agent,
                        PurchaseDate = x.PurchaseDate.ToString("yyyy-MM-dd"),
                        Price = x.Price,
                        Uid = x.Uid,
                        MaxCurrent = x.MaxCurrent,
                        MaxTon = x.MaxTon,
                        DailyInspection = x.DailyInspection,
                        PeriodicalInspection = x.PeriodicalInspection,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                        ImageUrl = x.ImageUrl,
                        UploadFiles = x.UploadFiles.ToArray(),
                        Picture = x.ImageUrl == null ? "" : x.ImageUrl,

                    }).ToListAsync();


                var Res = new Response<IEnumerable<FacilityResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _facilitys
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FacilityResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<FacilityPopupResponse>>> GetFacilitiesPopupBySearch(FacilityPopupRequest _req)
        {
            try
            {

                if(_req.inspectionType == "DAILY" || _req.inspectionType == "PERIOD")
                {
                    var _facilitys = await _db.Facilitys
                        .Include(x => x.CommonCode)
                        .Where(x => x.IsDeleted == false)
                        .Where(x => _req.FacilitiesIsUsing == "ALL" ? true : (_req.FacilitiesIsUsing == "Y" ? x.IsUsing == true : x.IsUsing == false))
                        .Where(x => _req.FacilitiesClassification == "ALL" ? true : x.CommonCode.Name == _req.FacilitiesClassification)
                        .Where(x => x.Name.Contains(_req.SearchInput) || x.Code.Contains(_req.SearchInput) | x.Memo.Contains(_req.SearchInput))
                        .OrderBy(x => x.Code).Reverse()
                        .Select(x => new FacilityPopupResponse()
                        {
                            FacilityId = x.Id,
                            PurchaseDate = x.PurchaseDate.ToString("yyyy-MM-dd"),
                            ElectricCurrentMax = x.MaxCurrent,
                            FacilitiesClassification = x.CommonCode.Name,
                            FacilitiesCode = x.Code,
                            FacilitiesName = x.Name,
                            FacilitiesIsUsing = x.IsUsing,
                            FacilitiesMemo = x.Memo,
                            FacilitiesStandard = x.Standard,
                            TonMax = x.MaxTon,
                            FacilityInspectionItems = _db.InspectionItems
                                .Where(y=>y.IsDeleted == false)
                                .Where(y=>y.Classify == "설비")
                                .Where(y=> _req.inspectionType == "DAILY" ? y.Type =="일상점검" : y.Type=="정기점검")
                                .Where(y=> y.IsUsing == true)
                                .Select(y=> new FacilityInspectionItemInterface
                                {
                                    CauseOfError = "",
                                    ErrorManagementResult ="",
                                    FacilityInspectionId = 0,
                                    FacilityInspectionItemId = 0,
                                    InsepctionName = y.InspectionItems,
                                    InspectionCode = y.Code,    
                                    InspectionCountCriteria = y.InspectionCount,
                                    InspectionItem = y.InspectionItems,
                                    InspectionItemId = y.Id,
                                    InspectionJudgement = y.JudgeStandard,
                                    InspectionMethod = y.JudgeMethod,
                                    InspectionPeriod = y.CommonCode.Name,   
                                    InspectionResult = "",
                                }).ToList()
                        }).ToListAsync();


                    var Res = new Response<IEnumerable<FacilityPopupResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _facilitys
                    };

                    return Res;
                }

                else
                {
                    var _facilitys = await _db.Facilitys
                        .Include(x => x.CommonCode)
                        .Where(x => x.IsDeleted == false)
                        .Where(x => _req.FacilitiesIsUsing == "ALL" ? true : (_req.FacilitiesIsUsing == "Y" ? x.IsUsing == true : x.IsUsing == false))
                        .Where(x => _req.FacilitiesClassification == "ALL" ? true : x.CommonCode.Name == _req.FacilitiesClassification)
                        .Where(x => x.Name.Contains(_req.SearchInput) || x.Code.Contains(_req.SearchInput) | x.Memo.Contains(_req.SearchInput))
                        .OrderBy(x => x.Code).Reverse()
                        .Select(x => new FacilityPopupResponse()
                        {
                            FacilityId = x.Id,
                            PurchaseDate = x.PurchaseDate.ToString("yyyy-MM-dd"),
                            ElectricCurrentMax = x.MaxCurrent,
                            FacilitiesClassification = x.CommonCode.Name,
                            FacilitiesCode = x.Code,
                            FacilitiesName = x.Name,
                            FacilitiesIsUsing = x.IsUsing,
                            FacilitiesMemo = x.Memo,
                            FacilitiesStandard = x.Standard,
                            TonMax = x.MaxTon,
                            PeriodicalInspection = x.PeriodicalInspection == true ? 1:0

                        }).ToListAsync();


                    var Res = new Response<IEnumerable<FacilityPopupResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _facilitys
                    };

                    return Res;
                }





            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FacilityPopupResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<IEnumerable<FacilityResponse>>> FacilityList(FacilityRequest facilityRequest)
        {
            try
            {
                var _facilitys = await _db.Facilitys
                    .Include(x => x.CommonCode)
                    .Where(x => facilityRequest.IsUsingStr == "ALL" ? true : (facilityRequest.IsUsingStr == "Y" ? x.IsUsing == true : x.IsUsing == false))
                    .Where(x => facilityRequest.Type == null ? true : x.CommonCode.Name == facilityRequest.Type)
                    .OrderBy(x => x.PurchaseDate).Reverse()
                    .Select(x => new FacilityResponse()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Type = x.CommonCode.Name,
                        Name = x.Name,
                        Standard = x.Standard,
                        MaxCurrent = x.MaxCurrent,
                        MaxTon = x.MaxTon,
                        PurchaseDate = x.PurchaseDate.ToString("yyyy-MM-dd"),
                        Memo = x.Memo,
                        IsUsing = x.IsUsing
                    }).ToListAsync();

                var Res = new Response<IEnumerable<FacilityResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _facilitys
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FacilityResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<FacilityResponse>> GetFacility (int id)
        {
            try
            {
                var _facility = await _db.Facilitys
                    .Include(x => x.CommonCode)
                    .Where(x => x.Id == id)
                    .Select(x => new FacilityResponse
                    {
                        Id = x.Id,
                        CommonCode = x.CommonCode.Id,
                        CommonCodeName = x.CommonCode.Name,
                        Code = x.Code,
                        Name = x.Name,
                        Standard = x.Standard,
                        Brand = x.Brand,
                        Model = x.Model,
                        Agent = x.Agent,
                        PurchaseDate = x.PurchaseDate.ToString("yyyy-MM-dd"),
                        Price = x.Price,
                        Uid = x.Uid,
                        MaxCurrent = x.MaxCurrent,
                        MaxTon = x.MaxTon,
                        DailyInspection = x.DailyInspection,
                        PeriodicalInspection = x.PeriodicalInspection,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                       // UploadFile = x.UploadFile,
                        ImageUrl = x.ImageUrl,
                        UploadFiles = x.UploadFiles.ToArray(),
                        Picture = x.ImageUrl == null ? "" : x.ImageUrl,
                    })
                    .FirstOrDefaultAsync();

                var Res = new Response<FacilityResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _facility
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<FacilityResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<FacilityResponse>>> GetFacilitys (int code)
        {
            try
            {
                if (code == 0)
                {
                    var _facility = await _db.Facilitys
                         .Include(x => x.CommonCode)
                        .Where(x => x.IsDeleted == false)
                        .Select(x => new FacilityResponse
                        {
                            Id = x.Id,
                            CommonCode = x.CommonCode.Id,
                            CommonCodeName = x.CommonCode.Name,  //x.CommonCode.Name,
                            Code = x.Code,
                            Name = x.Name,
                            Standard = x.Standard,
                            Brand = x.Brand,
                            Model = x.Model,
                            Agent = x.Agent,
                            PurchaseDate = x.PurchaseDate.ToString("yyyy-MM-dd"),
                            Price = x.Price,
                            Uid = x.Uid,
                            MaxCurrent = x.MaxCurrent,
                            MaxTon = x.MaxTon,
                            DailyInspection = x.DailyInspection,
                            PeriodicalInspection = x.PeriodicalInspection,
                            Memo = x.Memo,
                            IsUsing = x.IsUsing,
                            //UploadFile = x.UploadFile != null? x.UploadFile : null,
                            //UploadFiles = x.UploadFiles != null? x.UploadFiles.ToArray() : List<UploadFile>,
                            Picture = x.ImageUrl == null ? "" : x.ImageUrl,
                        })
                        .ToArrayAsync();

                    var Res = new Response<IEnumerable<FacilityResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _facility
                    };
                    return Res;
                }
                else
                {
                    var _facility = await _db.Facilitys
                        .Include(x => x.CommonCode)
                        .Where(x => x.IsDeleted == false && x.CommonCode.Id == code)
                        .Select(x => new FacilityResponse
                        {
                            Id = x.Id,
                            CommonCode = x.CommonCode.Id,
                            CommonCodeName = x.CommonCode.Name,
                            Code = x.Code,
                            Name = x.Name,
                            Standard = x.Standard,
                            Brand = x.Brand,
                            Model = x.Model,
                            Agent = x.Agent,
                            PurchaseDate = x.PurchaseDate.ToString("yyyy-MM-dd"),
                            Price = x.Price,
                            Uid = x.Uid,
                            MaxCurrent = x.MaxCurrent,
                            MaxTon = x.MaxTon,
                            DailyInspection = x.DailyInspection,
                            PeriodicalInspection = x.PeriodicalInspection,
                            Memo = x.Memo,
                            IsUsing = x.IsUsing,
                            //UploadFile = x.UploadFile,
                            //UploadFiles = x.UploadFiles.ToArray(),
                            Picture = x.ImageUrl == null ? "" : x.ImageUrl,

                        })
                        .ToArrayAsync();

                    var Res = new Response<IEnumerable<FacilityResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _facility
                    };
                    return Res;
                }
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FacilityResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<FacilityResponse>>> CreateFacility (FacilityRequest x)
        {
            try
            {
                string _code = x.Code;

                if (x.AutoCode)
                {
                    var prevCodes = await _db.Facilitys
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code.Length == 5)
                        .Where(y => y.Code.Substring(0, 1) == "F")
                        .Select(y => y.Code.Substring(1, 4))
                        .OrderByDescending(y => y)
                        .FirstOrDefaultAsync();


                    if (prevCodes == null)
                    {
                        _code = "F0001";
                    }
                    else
                    {
                        try
                        {
                            _code = "F" + (Convert.ToInt32(prevCodes) + 1).ToString("0000");
                        }
                        catch
                        {
                            _code = "F0001";
                        }
                    }
                }


                if (!x.AutoCode)
                {
                    var codeCheck = await _db.Facilitys
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == x.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<IEnumerable<FacilityResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = x.Code + "는 이미 존재하는 코드입니다.",
                            Data = null
                        };

                        return ErrorReturn;
                    }

                }

                if (x.PurchaseDate == "")
                    x.PurchaseDate = "0001-01-01";

                var _common = await _db.CommonCodes.Where(y => y.Id == x.CommonCode).FirstOrDefaultAsync();
                var _facility = new Facility
                {
                    CommonCode = _common,
                    Code = _code,
                    Name = x.Name,
                    Standard = x.Standard,
                    Brand = x.Brand,
                    Model = x.Model,
                    Agent = x.Agent,
                    PurchaseDate = Convert.ToDateTime(x.PurchaseDate),
                    Price = x.Price,
                    Uid = x.Uid,
                    MaxCurrent = x.MaxCurrent,
                    MaxTon = x.MaxTon,
                    DailyInspection = x.DailyInspection,
                    PeriodicalInspection = x.PeriodicalInspection,
                    Memo = x.Memo,
                    IsUsing = x.IsUsing,
                    IsDeleted = x.IsDeleted,
                    ImageUrl = x.ImageUrl,
                    UploadFiles = x.UploadFiles,
                };

                await _db.Facilitys.AddAsync(_facility);
                await Save();


                var Res = new Response<IEnumerable<FacilityResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FacilityResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        public async Task<Response<IEnumerable<FacilityResponse>>> UpdateFacility (FacilityRequest facility, int id)
        {
            try
            {
                var _common = await _db.CommonCodes.Where(y => y.Id == facility.CommonCode).FirstOrDefaultAsync();
                var _facility = await _db.Facilitys
                    .Include(x => x.UploadFiles)
                    .Where(x => x.Id == id).FirstOrDefaultAsync();

                if (facility.Code != _facility.Code)
                {
                    var codeCheck = await _db.Facilitys
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == facility.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<IEnumerable<FacilityResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = facility.Code + "는 이미 사용중인 코드입니다.",
                            Data = null
                        };

                        return ErrorReturn;
                    }
                }

                _facility.CommonCode = _common;
                _facility.Code = facility.Code;
                _facility.Name = facility.Name;
                _facility.Standard = facility.Standard;
                _facility.Brand = facility.Brand;
                _facility.Model = facility.Model;
                _facility.Agent = facility.Agent;
                _facility.PurchaseDate = Convert.ToDateTime(facility.PurchaseDate);
                _facility.Price = facility.Price;
                _facility.Uid = facility.Uid;
                _facility.MaxCurrent = facility.MaxCurrent;
                _facility.MaxTon = facility.MaxTon;
                _facility.DailyInspection = facility.DailyInspection;
                _facility.PeriodicalInspection = facility.PeriodicalInspection;
                _facility.Memo = facility.Memo;
                _facility.IsUsing = facility.IsUsing;

                _facility.ImageUrl = facility.ImageUrl;

                


                
                _db.Facilitys.Update(_facility);
                await Save();



                var Res = new Response<IEnumerable<FacilityResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FacilityResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<string>>> UpdateFacilitys (List<FacilityResponse> facilitys)
        {
            List<string> Results = new List<string>();

            try
            {
                int cnt = 0;
                foreach (var x in facilitys)
                {
                    cnt++;
                    var _facility = await _db.Facilitys.Include(x => x.CommonCode).Where(i => i.Code == x.Code).FirstOrDefaultAsync();
                    var _common = await _db.CommonCodes.Where(y => y.Name == x.CommonCodeName).FirstOrDefaultAsync();

                    if (_facility == null)
                    {
                        var temp = new Facility
                        {
                            CommonCode = _common,
                            Code = x.Code,
                            Name = x.Name,
                            Standard = x.Standard,
                            Brand = x.Brand,
                            Model = x.Model,
                            Agent = x.Agent,
                            PurchaseDate = Convert.ToDateTime(x.PurchaseDate),
                            Price = x.Price,
                            Uid = x.Uid,
                            MaxCurrent = x.MaxCurrent,
                            MaxTon = x.MaxTon,
                            Memo = x.Memo,
                            IsUsing = x.IsUsing,
                        };

                        await _db.Facilitys.AddAsync(temp);
                        await Save();
                        Results.Add(cnt.ToString() + "라인 : 공통코드 " + x.Code + " 생성");
                        continue;
                    }
                    else
                    {
                        if (x.Name == "" || x.Code == "")
                        {
                            Results.Add(cnt.ToString() + "라인 : 데이터 에러(코드 또는 이름이 없습니다)");
                            continue;
                        }
                        var _commonTemp = await _db.CommonCodes.Where(y => y.Name == x.CommonCodeName).FirstOrDefaultAsync();

                        _facility.CommonCode = _commonTemp;
                        _facility.Code = x.Code;
                        _facility.Name = x.Name;
                        _facility.Standard = x.Standard;
                        _facility.Brand = x.Brand;
                        _facility.Model = x.Model;
                        _facility.Agent = x.Agent;
                        _facility.PurchaseDate = Convert.ToDateTime(x.PurchaseDate);
                        _facility.Price = x.Price;
                        _facility.Uid = x.Uid;
                        _facility.MaxCurrent = x.MaxCurrent;
                        _facility.MaxTon = x.MaxTon;
                        _facility.Memo = x.Memo;
                        _facility.IsUsing = x.IsUsing;

                        _db.Facilitys.Update(_facility);
                        await Save();
                        Results.Add(cnt.ToString() + "라인 : 공통코드 " + x.Code + " 업데이트");

                        continue;
                    }
                }


                var Res = new Response<IEnumerable<string>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = Results
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<string>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = Results
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<FacilityResponse>>> DeleteFacilitys (int[] id)
        {
            try
            {
                List<Facility> _facilitys = new List<Facility>();
                foreach (var i in id)
                {
                    var _facility = await _db.Facilitys.Include(x => x.CommonCode).Where(x => x.Id == i).FirstOrDefaultAsync();
                    if (_facility != null)
                    {
                        _facility.IsDeleted = true;
                        _facilitys.Add(_facility);
                    }

                }

                if (_facilitys.Count > 0)
                {
                    _db.Facilitys.UpdateRange(_facilitys);
                    await Save();
                }
                var result = await _db.Facilitys
                    .Include(x => x.CommonCode)
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new FacilityResponse
                    {
                        Id = x.Id,
                        CommonCode = x.CommonCode.Id,
                        CommonCodeName = x.CommonCode.Name,
                        Code = x.Code,
                        Name = x.Name,
                        Standard = x.Standard,
                        Brand = x.Brand,
                        Model = x.Model,
                        Agent = x.Agent,
                        PurchaseDate = x.PurchaseDate.ToString("yyyy-MM-dd"),
                        Price = x.Price,
                        Uid = x.Uid,
                        MaxCurrent = x.MaxCurrent,
                        MaxTon = x.MaxTon,
                        DailyInspection = x.DailyInspection,
                        PeriodicalInspection = x.PeriodicalInspection,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                        ImageUrl = x.ImageUrl,
                        UploadFiles = x.UploadFiles.ToArray(),
                        Picture = x.ImageUrl == null ? "" : x.ImageUrl,

                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<FacilityResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FacilityResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        #endregion Facility Manage

        #region Process Manage

        public async Task<Response<IEnumerable<ProcessResponse>>> GetProcessesBySearch(ProcessRequest _proc)
        {
            try
            {
                var procs = await _db.Processes
                    .Include(x => x.CommonCode)
                    .Where(x => x.IsDeleted == false)
                    .Where(x => _proc.IsUsingStr == "ALL" ? true : x.IsUsing == (_proc.IsUsingStr == "Y" ? true : false))
                    .Where(x => _proc.TypeStr == "ALL" ? true : x.CommonCode.Name == _proc.TypeStr)
                    .Where(x => x.Name.Contains(_proc.SearchStr) || x.Code.Contains(_proc.SearchStr))
                    .Select(
                      x => new ProcessResponse
                      {
                          Id = x.Id,
                          Code = x.Code,
                          CommonCode = x.CommonCodeId,
                          CommonCodeName = x.CommonCodeId != 0 ? _db.CommonCodes.Where(y => y.Id == x.CommonCodeId).Select(y => y.Name).First() : "",
                          Name = x.Name,
                          FacilityUse = x.FacilityUse,
                          ProcessInspection = x.ProcessInspection,
                          Memo = x.Memo,
                          IsUsing = x.IsUsing,
                      }
                     )
                    .ToArrayAsync();


                var Res = new Response<IEnumerable<ProcessResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = procs
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };
                return Res;
            }
        }


        public async Task<Response<IEnumerable<ProcessPopupResponse>>> GetProcessesPopupBySearch(ProcessPopupRequest _req)
        {
            try
            {
                var procs = await _db.Processes
                    .Include(x => x.CommonCode)
                    .Where(x => x.IsDeleted == false)
                    .Where(x => _req.ProcessIsUsing == "ALL" ? true : x.IsUsing == (_req.ProcessIsUsing == "Y" ? true : false))
                    .Where(x => x.Name.Contains(_req.SearchInput) || x.Code.Contains(_req.SearchInput))
                    .Select(
                      x => new ProcessPopupResponse
                      {
                          ProcessId = x.Id,
                          ProcessCheck = x.ProcessInspection,
                          ProcessClassification = x.CommonCode.Name,
                          ProcessCode = x.Code,
                          ProcessIsFacilities = x.FacilityUse,
                          ProcessIsUsing = x.IsUsing,
                          ProcessMemo = x.Memo,
                          ProcessName = x.Name
                      }
                     )
                    .ToArrayAsync();


                var Res = new Response<IEnumerable<ProcessPopupResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = procs
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessPopupResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };
                return Res;
            }
        }

        public async Task<Response<ProcessResponse>> GetProcess (int id)
        {
            try
            {
                var _procDownTimes = await _db.ProcessDownTimeTypes
                    .Include(x => x.DownTime)
                    .Where(x => x.ProcessId == id)
                    .Select(x => 
                        new ProcessDownTimeInterface
                        {   Id = x.Id,
                            IsUsing = x.IsUsing,
                            Code = x.DownTime.Code,
                            Memo = x.Memo,
                            Name = x.DownTime.Name,
                            Checked = false
                        }
                    )
                    .ToListAsync();


                var _procFacilites = await _db.ProcessFacilities
                    .Include(x=> x.Facility)
                    .Where(x=>x.ProcessId == id)
                    .Select(x=>
                        new ProcessFacilityInterface
                        {
                            Id = x.Id,
                            IsUsing = x.IsUsing,
                            Code = x.Facility.Code,
                            Memo = x.Memo,
                            Name = x.Facility.Name,
                            Checked = false

                        }
                    )
                    .ToListAsync();


                var _procDef = await _db.ProcessDefectives_Master
                    .Include(x => x.Defective)
                    .Where(x => x.ProcessId == id)
                    .Select(x =>
                    
                        new ProcessDefectiveInterface
                        {
                            Id = x.Id,
                            IsUsing = x.IsUsing,
                            Code = x.Defective.Code,
                            Memo = x.Memo,
                            Name = x.Defective.Name,
                            Checked = false
                        }
                    )
                    .ToListAsync();


                var _process = await _db.Processes
                    .Include(x => x.CommonCode)
                    .Where(x => x.Id == id)
                    .Select(x => new ProcessResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        CommonCodeName = x.CommonCode.Name,
                        CommonCode = x.CommonCode.Id,
                        Name = x.Name,
                        FacilityUse = x.FacilityUse,
                        ProcessInspection = x.ProcessInspection,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                        ProcessDownTimeTypes = _procDownTimes,
                        ProcessFacilitys = _procFacilites,
                        ProcessDefectives = _procDef
                    })
                    .FirstOrDefaultAsync();

                var Res = new Response<ProcessResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _process
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<ProcessResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }




        public async Task<Response<IEnumerable<ProcessFacilityInterface>>> GetProcessFacility(int Id)
        {
            try
            {
                var res = await _db.ProcessFacilities
                    .Include(x => x.Facility)
                    .Where(x => x.ProcessId == Id)
                    .Select(x =>
                        new ProcessFacilityInterface
                        {
                            Id = x.Id,
                            IsUsing = x.IsUsing,
                            Code = x.Facility.Code,
                            Memo = x.Memo,
                            Name = x.Facility.Name
                        }
                     )
                    .ToListAsync();

                var Res = new Response<IEnumerable<ProcessFacilityInterface>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;
            }
            catch(Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessFacilityInterface>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };
                return Res;
            }
        }
        public async Task<Response<IEnumerable<ProcessDownTimeInterface>>> GetProcessDownTime(int Id)
        {
            try
            {
                var res = await _db.ProcessDownTimeTypes
                    .Include(x => x.DownTime)
                    .Where(x => x.ProcessId == Id)
                    .Select(x =>
                        new ProcessDownTimeInterface
                        {
                            Id = x.Id,
                            IsUsing = x.IsUsing,
                            Code = x.DownTime.Code,
                            Memo = x.Memo,
                            Name = x.DownTime.Name
                        }
                     )
                    .ToListAsync();

                var Res = new Response<IEnumerable<ProcessDownTimeInterface>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessDownTimeInterface>>()
                {
                    IsSuccess = true,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };
                return Res;
            }
        }
        public async Task<Response<IEnumerable<ProcessDefectiveInterface>>> GetProcessDefective(int Id)
        {
            try
            {
                var res = await _db.ProcessDefectives_Master
                    .Include(x => x.Defective)
                    .Where(x => x.ProcessId == Id)
                    .Select(x =>
                        new ProcessDefectiveInterface
                        {
                            Id = x.Id,
                            IsUsing = x.IsUsing,
                            Code = x.Defective.Code,
                            Memo = x.Memo,
                            Name = x.Defective.Name
                        }
                     )
                    .ToListAsync();

                var Res = new Response<IEnumerable<ProcessDefectiveInterface>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessDefectiveInterface>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };
                return Res;
            }
        }

        public async Task<Response<IEnumerable<ProcessResponse>>> GetProcesses (int code)
        {
            try
            {
                if (code == 0)
                {
                    var _process = await _db.Processes
                        .Where(x => x.IsDeleted == false)
                        .Select(x => new ProcessResponse
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            CommonCodeName = x.CommonCode.Name,
                            CommonCode = x.CommonCode.Id,
                            FacilityUse = x.FacilityUse,
                            ProcessInspection = x.ProcessInspection,
                            Memo = x.Memo,
                            IsUsing = x.IsUsing,
                        })
                        .ToArrayAsync();

                    var Res = new Response<IEnumerable<ProcessResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _process
                    };
                    return Res;
                }
                else
                {
                    var _process = await _db.Processes
                        .Where(x => x.IsDeleted == false)
                        .Select(x => new ProcessResponse
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            CommonCodeName = x.CommonCode.Name,
                            CommonCode = x.CommonCode.Id,
                            FacilityUse = x.FacilityUse,
                            ProcessInspection = x.ProcessInspection,

                            Memo = x.Memo,
                            IsUsing = x.IsUsing,
                        })
                        .ToArrayAsync();

                    var Res = new Response<IEnumerable<ProcessResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _process
                    };
                    return Res;
                }
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<ProcessResponse>>> CreateProcess (ProcessRequest x)
        {
            try
            {
                string _code = x.Code;

                if (x.AutoCode)
                {
                    var prevCodes = await _db.Processes
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code.Length == 5)
                        .Where(y => y.Code.Substring(0, 1) == "P")
                        .Select(y => y.Code.Substring(1, 4))
                        .OrderByDescending(y => y)
                        .FirstOrDefaultAsync();


                    if (prevCodes == null)
                    {
                        _code = "P0001";
                    }
                    else
                    {
                        try
                        {
                            _code = "P" + (Convert.ToInt32(prevCodes) + 1).ToString("0000");
                        }
                        catch
                        {
                            _code = "P0001";
                        }
                    }
                }


                if (!x.AutoCode)
                {
                    var codeCheck = await _db.Processes
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == x.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<IEnumerable<ProcessResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = x.Code + "는 이미 존재하는 코드입니다.",
                            Data = null
                        };

                        return ErrorReturn;
                    }

                }

                List<ProcessDownTimeType> _downTimes = new List<ProcessDownTimeType>();
                if (x.ProcessDownTimeTypes != null)
                {
                    foreach (var y in x.ProcessDownTimeTypes)
                    {
                        var _downTime = new ProcessDownTimeType
                        {
                            IsUsing = y.IsUsing,
                            DownTime = _db.CommonCodes.Where(z => z.Code == y.Code).FirstOrDefault()
                        };
                        _downTimes.Add(_downTime);
                    }
                }

                List<ProcessFacility> _facilities = new List<ProcessFacility>();
                if (x.ProcessFacilitys != null)
                {
                    foreach (var y in x.ProcessFacilitys)
                    {
                        var _facility = new ProcessFacility
                        {
                            IsUsing = y.IsUsing,
                            Facility = _db.Facilitys.Where(z => z.Name == y.Name).FirstOrDefault()
                        };
                        _facilities.Add(_facility);
                    }
                }

                List<ProcessDefective_Master> _defectives = new List<ProcessDefective_Master>();
                if (x.ProcessDefectives != null)
                {
                    foreach (var y in x.ProcessDefectives)
                    {
                        var _defective = new ProcessDefective_Master
                        {
                            IsUsing = y.IsUsing,
                            Defective = _db.Defectives.Where(z => z.Code == y.Code).FirstOrDefault()
                        };
                        _defectives.Add(_defective);
                    }
                }

                var _process = new Process
                {
                    Id = x.Id,
                    Code = _code,
                    CommonCode = _db.CommonCodes.Find(x.CommonCode),
                    Name = x.Name,
                    FacilityUse = x.FacilityUse,
                    ProcessInspection = x.ProcessInspection,
                    ProcessDefectives_Master = _defectives,
                    ProcessDownTimeTypes = _downTimes,
                    ProcessFacilitys = _facilities,
                    Memo = x.Memo,
                    IsUsing = x.IsUsing,
                    IsDeleted = false
                };

                await _db.Processes.AddAsync(_process);
                await Save();

                var result = await _db.Processes
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new ProcessResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        CommonCodeName = x.CommonCode.Name,
                        CommonCode = x.CommonCode.Id,
                        FacilityUse = x.FacilityUse,
                        ProcessInspection = x.ProcessInspection,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<ProcessResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        public async Task<Response<IEnumerable<ProcessResponse>>> UpdateProcess(ProcessRequest process, int id)
        {
            try
            {

                var _process = await _db.Processes.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (process.Code != _process.Code)
                {
                    var codeCheck = await _db.Facilitys
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == process.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<IEnumerable<ProcessResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = process.Code + "는 이미 사용중인 코드입니다.",
                            Data = null
                        };

                        return ErrorReturn;
                    }
                }


                var _deleteFacility = await _db.ProcessFacilities.Where(x => x.ProcessId == id).ToArrayAsync();
                var _deleteDt = await _db.ProcessDownTimeTypes.Where(x => x.ProcessId == id).ToArrayAsync();
                //var _deleteDefective = await _db.ProcessDefectives.Where(x => x.Process.Id == id).ToArrayAsync();
                var _deleteDefective = await _db.ProcessDefectives_Master.Where(x => x.ProcessId == id).ToArrayAsync();

                if (_deleteFacility != null)
                {
                    _db.ProcessFacilities.RemoveRange(_deleteFacility);
                }
                if (_deleteDt != null)
                {
                    _db.ProcessDownTimeTypes.RemoveRange(_deleteDt);
                }
                if (_deleteDefective != null)
                {
                    _db.ProcessDefectives_Master.RemoveRange(_deleteDefective);
                }

                await Save();

                List<ProcessDownTimeType> _downTimes = new List<ProcessDownTimeType>();
                if (process.ProcessDownTimeTypes != null)
                {
                    foreach (var y in process.ProcessDownTimeTypes)
                    {
                        var _downTime = new ProcessDownTimeType
                        {
                            IsUsing = y.IsUsing,
                            DownTime = _db.CommonCodes.Where(z => z.Code == y.Code).FirstOrDefault(),
                            Memo = y.Memo
                        };
                        _downTimes.Add(_downTime);
                    }
                }

                List<ProcessFacility> _facilities = new List<ProcessFacility>();
                if (process.ProcessFacilitys != null)
                {
                    foreach (var y in process.ProcessFacilitys)
                    {
                        var _facility = new ProcessFacility
                        {
                            IsUsing = y.IsUsing,
                            Facility = _db.Facilitys.Where(z => z.Name == y.Name).FirstOrDefault(),
                            Memo = y.Memo

                        };
                        _facilities.Add(_facility);
                    }
                }

                List<ProcessDefective_Master> _defectives = new List<ProcessDefective_Master>();
                if (process.ProcessDefectives != null)
                {
                    foreach (var y in process.ProcessDefectives)
                    {
                        var _defective = new ProcessDefective_Master
                        {
                            IsUsing = y.IsUsing,
                            Defective = _db.Defectives.Where(z => z.Name == y.Name).FirstOrDefault(),
                            Memo = y.Memo

                        };
                        _defectives.Add(_defective);
                    }
                }



                _process.Code = process.Code;
                _process.Name = process.Name;
                _process.Memo = process.Memo;
                _process.IsUsing = process.IsUsing;
                _process.FacilityUse = process.FacilityUse;

                _process.ProcessInspection = process.ProcessInspection;
                _process.ProcessDefectives_Master = _defectives;
                _process.ProcessDownTimeTypes = _downTimes;
                _process.ProcessFacilitys = _facilities;
                _process.CommonCode = _db.CommonCodes.Find(process.CommonCode);


                _db.Processes.Update(_process);
                await Save();

                var result = await _db.Processes
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new ProcessResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        CommonCodeName = x.CommonCode.Name,
                        CommonCode = x.CommonCode.Id,
                        FacilityUse = x.FacilityUse,
                        ProcessInspection = x.ProcessInspection,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<ProcessResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<string>>> UpdateProcesses (List<ProcessResponse> processs)
        {
            List<string> Results = new List<string>();

            try
            {
                int cnt = 0;
                foreach (var x in processs)
                {
                    cnt++;
                    var _process = await _db.Processes.Where(i => i.Code == x.Code).FirstOrDefaultAsync();
                    if (_process == null)
                    {
                        var _temp = new Process
                        {
                            Code = x.Code,
                            Name = x.Name,
                            CommonCode = _db.CommonCodes.Where(y => y.Name == x.CommonCodeName).FirstOrDefault(),
                            FacilityUse = x.FacilityUse,
                            ProcessInspection = x.ProcessInspection,
                            Memo = x.Memo,
                            IsUsing = x.IsUsing,
                        };

                        await _db.Processes.AddAsync(_temp);
                        await Save();
                        Results.Add(cnt.ToString() + "라인 : 공통코드 " + x.Code + " 생성");
                        continue;
                    }
                    else
                    {
                        if (x.Name == "" || x.Code == "")
                        {
                            Results.Add(cnt.ToString() + "라인 : 데이터 에러(코드 또는 이름이 없습니다)");
                            continue;
                        }

                        _process.Code = x.Code;
                        _process.Name = x.Name;
                        _process.FacilityUse = x.FacilityUse;
                        _process.ProcessInspection = x.ProcessInspection;
                        _process.Memo = x.Memo;
                        _process.IsUsing = x.IsUsing;
                        _process.CommonCode = _db.CommonCodes.Where(y => y.Name == x.CommonCodeName).FirstOrDefault();

                        _db.Processes.Update(_process);
                        await Save();
                        Results.Add(cnt.ToString() + "라인 : 공통코드 " + x.Code + " 업데이트");

                        continue;
                    }
                }


                var Res = new Response<IEnumerable<string>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = Results
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<string>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = Results
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<ProcessResponse>>> DeleteProcesses (int[] id)
        {
            try
            {
                foreach (var i in id)
                {
                    var _process = await _db.Processes.Where(x => x.Id == i).FirstOrDefaultAsync();
                    _process.IsDeleted = true;
                    _db.Processes.Update(_process);
                    await Save();
                }


                var result = await _db.Processes
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new ProcessResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        FacilityUse = x.FacilityUse,
                        ProcessInspection = x.ProcessInspection,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,

                        CommonCodeName = x.CommonCode.Name,
                        CommonCode = x.CommonCode.Id,
                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<ProcessResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        #endregion Process Manage

        #region Product Manage

        // BOM GET ALL / GET BY ID / UPDATE(CREATE, DELETE)
        public async Task<Response<IEnumerable<ProductPopupResponse>>> GetBomsPopupBySearch(ProductPopupRequest _req)
        {
            try
            {
                if(_req.RequestType == "WO")
                {
                    var res2 = await _db.Products
                        .Where(x => x.IsDeleted == false)
                        .Where(x => x.Processes.Where(y => y.IsDeleted == 0).Count() > 0)
                        .Where(x => x.Name.Contains(_req.SearchInput) || x.Code.Contains(_req.SearchInput) || x.Memo.Contains(_req.SearchInput))
                        .Where(x => _req.ProductIsUsing == "ALL" ? true : _req.ProductIsUsing == "Y" ? x.IsUsing == true : false)
                        .Where(x => _req.ProductClassification == "ALL" ? true : x.CommonCode.Name.Contains(_req.ProductClassification))
                        .Select(x => new ProductPopupResponse
                        {
                            OrderId = 0,
                            ProductId = x.Id,
                            ProductCode = x.Code,
                            ProductName = x.Name,
                            ProductClassification = x.CommonCode.Name,
                            ProductStandard = x.Standard,
                            ProductUnit = x.Unit,
                            UploadFileName = x.UploadFile.FileName,
                            UploadFileUrl = x.UploadFile.FileUrl,
                            ProductIsUsing = x.IsUsing ? "사용" : "비사용",
                            ProductMemo = x.Memo,
                            ProductTaxInfo = x.TaxType,
                            OptimumStock = x.OptimumStock,
                            Inventory = _db.LotCounts.Where(y => y.Product.Id == x.Id).Where(y => y.IsDeleted == 0).Select(y => (0 - y.DefectiveCount - y.ConsumeCount - y.OutOrderCount + y.StoreOutCount + y.ProduceCount + y.ModifyCount)).Sum(),
                            ProductOrderCount = 0,

                            ModelId = x.Model != null ? x.Model.Id : 0,
                            ModelName = x.Model != null ? x.Model.Name : "",
                            ModelCode = x.Model != null ? x.Model.Code : "",
                            PartnerId = x.Partner != null ? x.Partner.Id : 0,
                            PartnerName = x.Partner != null ? x.Partner.Name : "",
                            PartnerCode = x.Partner != null ? x.Partner.Code : "",


                            WorkerOrderProducePlans = x.Processes
                                .Where(y => y.IsDeleted == 0)
                                .OrderBy(y=>y.ProcessOrder)
                                .Select(y => new ProducePlanProcessInterface002
                                {
                                    ProductProcessId = y.ProductProcessId,
                                    ProducePlansProcessId = 0,
                                    ProcessOrder = y.ProcessOrder,
                                    ProcessCode = y.Process.Code,
                                    ProcessName = y.Process.Name,
                                    ProcessPlanQuantity = 0,
                                    ProcessPlanBacklog = 0,
                                    ProductId = y.ProduceProductId,
                                    ProcessCheck = true,
                                    ProcessCheckResult = "미검사",
                                    FacilityId = 0,
                                    MoldId = 0,
                                    WorkerId = "",
                                    PartnerId = 0,
                                    ProcessWorkQuantity = 0,
                                }).ToList()

                        })
                        .ToListAsync();

                    var Res2 = new Response<IEnumerable<ProductPopupResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res2
                    };

                    return Res2;
                }
                if(_req.RequestType == "PP")
                {
                    var res3 = await _db.Products
                        .Where(x => x.IsDeleted == false)
                        .Where(x => x.Processes.Where(y=>y.IsDeleted == 0).Count()>0)
                        .Where(x => x.Name.Contains(_req.SearchInput) || x.Code.Contains(_req.SearchInput) || x.Memo.Contains(_req.SearchInput))
                        .Where(x => _req.ProductIsUsing == "ALL" ? true : _req.ProductIsUsing == "Y" ? x.IsUsing == true : false)
                        .Where(x => _req.ProductClassification == "ALL" ? true : x.CommonCode.Name.Contains(_req.ProductClassification))
                        .Select(x => new ProductPopupResponse
                        {
                            OrderId = 0,
                            ProductId = x.Id,
                            ProductCode = x.Code,
                            ProductName = x.Name,
                            ProductClassification = x.CommonCode.Name,
                            ProductStandard = x.Standard,
                            ProductUnit = x.Unit,
                            UploadFileName = x.UploadFile.FileName,
                            UploadFileUrl = x.UploadFile.FileUrl,
                            ProductIsUsing = x.IsUsing ? "사용" : "비사용",
                            ProductMemo = x.Memo,
                            ProductTaxInfo = x.TaxType,
                            OptimumStock = x.OptimumStock,
                            Inventory = _db.LotCounts.Where(y => y.Product.Id == x.Id).Where(y => y.IsDeleted == 0).Select(y => (0 - y.DefectiveCount - y.ConsumeCount - y.OutOrderCount + y.StoreOutCount + y.ProduceCount + y.ModifyCount)).Sum(),
                            ProductOrderCount = 0,

                            ModelId = x.Model != null ? x.Model.Id : 0,
                            ModelName = x.Model != null ? x.Model.Name : "",
                            ModelCode = x.Model != null ? x.Model.Code : "",
                            PartnerId = x.Partner != null ? x.Partner.Id : 0,
                            PartnerName = x.Partner != null ? x.Partner.Name : "",
                            PartnerCode = x.Partner != null ? x.Partner.Code : "",

                            ProducePlanProcesses = x.Processes
                                .Where(x => x.IsDeleted == 0)
                                .Select(y => new ProductProcessInterface2
                                {
                                    ProductProcessId = y.ProductProcessId,
                                    ProcessId = y.ProcessId,
                                    ProcessCode = y.Process.Code,
                                    ProcessName = y.Process.Name,
                                    ProcessOrder = y.ProcessOrder,


                                    ProductId = y.ProduceProductId,
                                    ProductUnit = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Unit).FirstOrDefault(),
                                    ProductClassification = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.CommonCode.Name).FirstOrDefault(),
                                    ProductName = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Name).FirstOrDefault(),
                                    ProductCode = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Code).FirstOrDefault(),
                                    ProductStandard = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Standard).FirstOrDefault(),
                    
                                    Inventory = _db.LotCounts.Where(z => z.Product.Id == y.ProduceProductId).Select(z => (0 - z.DefectiveCount - z.ConsumeCount - z.OutOrderCount + z.StoreOutCount + z.ProduceCount + z.ModifyCount)).Sum(),
                                    ProcessPlanQuantity = 0,

                                    ProcessInputItems = y.Items
                                        .Where(z => z.IsDeleted == 0)
                                        .Select(z => new ProductItemInterface2
                                        {
                                            ProcessId = z.ProcessId,
                                            ProcessCode = _db.Processes.Where(k => k.Id == z.ProcessId).Select(k => k.Code).FirstOrDefault(),
                                            ProcessName = _db.Processes.Where(k => k.Id == z.ProcessId).Select(k => k.Name).FirstOrDefault(),
                    
                                            ItemId = z.ProductId,
                                            ItemCode = z.Product.Code,
                                            ItemName = z.Product.Name,
                                            ItemClassification = z.Product.CommonCode.Name,
                                            ItemStandard = z.Product.Standard,
                                            ItemUnit = z.Product.Unit,
                                            Loss = (float)z.Loss,
                                            ProcessPlanQuantity = 0,
                                            RequiredQuantity = (float)z.Require,
                                            TotalRequiredQuantity = (float)(z.Loss * z.Require),
                                            TotalInputQuantity = 0,
                                            Inventory = _db.LotCounts.Where(k => k.Product.Id == z.Product.Id).Where(k => k.IsDeleted == 0).Select(k => (0 - k.DefectiveCount - k.ConsumeCount - k.OutOrderCount + k.StoreOutCount + k.ProduceCount + k.ModifyCount)).Sum(),
                                        }).ToList()
                                }).ToList(),
                        })
                        .ToListAsync();

                    var Res3 = new Response<IEnumerable<ProductPopupResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res3
                    };

                    return Res3;
                }


                var res = await _db.Products
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.Name.Contains(_req.SearchInput) || x.Code.Contains(_req.SearchInput) || x.Memo.Contains(_req.SearchInput))
                    .Where(x => _req.ProductIsUsing == "ALL" ? true : _req.ProductIsUsing == "Y" ? x.IsUsing == true : false)
                    .Where(x => _req.ProductClassification == "ALL" ? true : x.CommonCode.Name.Contains(_req.ProductClassification))
                    .Select(x => new ProductPopupResponse
                    {
                        OrderId = 0,
                        ProductId = x.Id,
                        ProductCode = x.Code,
                        ProductName = x.Name,
                        ProductClassification = x.CommonCode.Name,
                        ProductStandard = x.Standard,
                        ProductUnit = x.Unit,
                        UploadFileName = x.UploadFile.FileName,
                        UploadFileUrl = x.UploadFile.FileUrl,
                        ProductIsUsing = x.IsUsing ? "사용" : "비사용",
                        ProductMemo = x.Memo,
                        ProductTaxInfo = x.TaxType,
                        OptimumStock = x.OptimumStock,
                        Inventory = _db.LotCounts.Where(y => y.Product.Id == x.Id).Where(y => y.IsDeleted == 0).Select(y => (0 - y.DefectiveCount - y.ConsumeCount - y.OutOrderCount + y.StoreOutCount + y.ProduceCount + y.ModifyCount)).Sum(),
                         ProductOrderCount = 0,
                    })
                .ToListAsync();
                    
                var Res = new Response<IEnumerable<ProductPopupResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;





            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProductPopupResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null

            };

                return Res;
            }
        }



        public async Task<Response<IEnumerable<ProductResponse>>> GetBomsBySearch(ProductRequest _prd)
        {

            try
            {
                var prds = await _db.Products
                    .Include(x => x.CommonCode)
                    .Include(x => x.UploadFile)
                    .Include(x => x.Processes)
                    .Where(x => x.IsDeleted == false)
                    .Where(x => _prd.IsUsingStr == "ALL" ? true : x.IsUsing == (_prd.IsUsingStr == "Y" ? true : false))
                    .Where(x => _prd.TypeStr == "ALL" ? (x.CommonCode.Name == "완제품" || x.CommonCode.Name == "반제품" || x.CommonCode.Name == "원자재" || x.CommonCode.Name == "부자재") : x.CommonCode.Name == _prd.TypeStr)
                    .Where(x => x.Name.Contains(_prd.SearchStr) || x.Code.Contains(_prd.SearchStr) || x.Memo.Contains(_prd.SearchStr) ||x.Standard.Contains(_prd.SearchStr) || x.Model.Name.Contains(_prd.SearchStr) || x.Partner.Name.Contains(_prd.SearchStr))
                    .Select(
                      x => new ProductResponse
                      {
                          Id = x.Id,
                          Code = x.Code,
                          CommonCodeName = x.CommonCode.Name,
                          Name = x.Name,
                          Standard = x.Standard,
                          Unit = x.Unit,
                          UploadFile = x.UploadFile,
                          Picture = x.UploadFile != null ? x.UploadFile.FileUrl : "",
                          IsUsing = x.IsUsing,
                          ProcessCount = x.Processes.Where(y => y.IsDeleted == 0).Count(),
                          ItemCount = _db.ProductItems.Where(y=>y.IsDeleted == 0 ).Where(y=>y.ProductProcess.Product.Id == x.Id).Count(),
                          Memo = x.Memo,

                          ModelId = x.Model != null ? x.Model.Id : 0,
                          ModelName = x.Model != null ? x.Model.Name : "",
                          ModelCode = x.Model != null ? x.Model.Code : "",
                          PartnerId = x.Partner != null ? x.Partner.Id : 0,
                          PartnerName = x.Partner != null ? x.Partner.Name : "",
                          PartnerCode = x.Partner != null ? x.Partner.Code : "",

                      }
                     )
                    .OrderBy(x => x.Id)
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<ProductResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = prds
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProductResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<BomResponse>> GetBomById(ProductRequest _prd)
        {
            try
            {
                var _productProcessedIds = await _db.Products
                    .Include(x => x.Processes)
                    .Where(x => x.Id == _prd.ProductId)
                    .Where(x => x.IsDeleted == false)
                    .Select(x => x.Processes
                        .Where(y => y.IsDeleted == 0)
                        .Select(y => y.ProductProcessId)
                        .ToList()
                     )
                    .FirstOrDefaultAsync();

                List<BomProductItemInterface> _items = new List<BomProductItemInterface>();
                foreach (var procId in _productProcessedIds)
                {
                    var i = _db.ProductItems
                        .Where(x => x.ProductProcessId == procId && x.IsDeleted == 0)
                        .OrderBy(x=>x.ProcessOrder)
                        .ThenBy(x=>x.Priority)
                        .Select(y => new BomProductItemInterface
                        {

                            //고유 ID
                            ProductItemsId = y.ProductItemId,

                            //연결된 ProductProcess ID [FJ]
                            ProductProcessId = y.ProductProcessId,
                            ProcessId = y.ProcessId,
                            ProcessName = y.ProcessId != 0 ? _db.Processes.Where(z => z.Id == y.ProcessId).Select(z => z.Name).First() : "",
                            ProcessCode = y.ProcessId != 0 ? _db.Processes.Where(z => z.Id == y.ProcessId).Select(z => z.Code).First() : "",
                            ProcessOrder = y.ProcessOrder,

                            //투입제품 정보
                            ProductId = y.ProductId,
                            ProductName = y.ProductId != 0 ? _db.Products.Where(z => z.Id == y.ProductId).Select(z => z.Name).First() : "",
                            ProductCode = y.ProductId != 0 ? _db.Products.Where(z => z.Id == y.ProductId).Select(z => z.Code).First() : "",
                            Standard = y.ProductId != 0 ? _db.Products.Where(z => z.Id == y.ProductId).Select(z => z.Standard).First() : "",
                            Unit = y.ProductId != 0 ? _db.Products.Where(z => z.Id == y.ProductId).Select(z => z.Unit).First() : "",
                            ProductCommonCodeName = y.ProductId != 0 ? _db.Products.Include(z => z.CommonCode).Where(z => z.Id == y.ProductId).Select(z => z.CommonCode.Name).First() : "",

                            Require = (float)y.Require,
                            Memo = y.Memo,
                            Priority = y.Priority,
                            Loss = (float)y.Loss,
                            TotalRequire = (float)y.Loss + (float)y.Require


                        })
                        .ToList();

                    if (i != null) _items.AddRange(i);
                }



                var prd = await _db.Products
                    .Include(x => x.CommonCode)
                    .Include(x => x.UploadFile)
                    .Include(x => x.Processes).ThenInclude(x => x.Items)
                    .Where(x => x.Id == _prd.ProductId)
                    .Select(x => new BomResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        CommonCodeName = x.CommonCode.Name,
                        Name = x.Name,
                        Standard = x.Standard,
                        Unit = x.Unit,
                        UploadFile = x.UploadFile,
                        IsUsing = x.IsUsing,
                        Memo = x.Memo,
                        ProcessCount = x.Processes.Where(y => y.IsDeleted == 0).Count(),

                        ModelId = x.Model != null ? x.Model.Id : 0,
                        ModelName = x.Model != null ? x.Model.Name : "",
                        ModelCode = x.Model != null ? x.Model.Code : "",
                        PartnerId = x.Partner != null ? x.Partner.Id : 0,
                        PartnerName = x.Partner != null ? x.Partner.Name : "",
                        PartnerCode = x.Partner != null ? x.Partner.Code : "",

                        ProductProcesses = x.Processes
                            .Where(y => y.IsDeleted == 0)
                            .OrderBy(y=> y.ProcessOrder)
                            .Select(y => new BomProductProcessInterface
                            {
                                ProductId = x.Id,

                                
                                ProcessOrder = y.ProcessOrder,

                                ProcessId = y.ProcessId,
                                ProcessName = y.ProcessId != 0 ? _db.Processes.Where(z => z.Id == y.ProcessId).Select(z => z.Name).First() : "",
                                ProcessCode = y.ProcessId != 0 ? _db.Processes.Where(z => z.Id == y.ProcessId).Select(z => z.Code).First() : "",
                                ProcessType = y.ProcessId != 0 ? _db.Processes.Where(z => z.Id == y.ProcessId).Select(z => z.CommonCode.Name).First() : "",
                                PartnerId = y.PartnerId,
                                PartnerName = y.PartnerId != 0 ? _db.Partners.Where(z => z.Id == y.PartnerId).Select(z => z.Name).First() : "",
                                PartnerCode = y.PartnerId != 0 ? _db.Partners.Where(z => z.Id == y.PartnerId).Select(z => z.Code).First() : "",

                                IsOutSourcing = y.IsOutSourcing,

                                ProductProcessId = y.ProductProcessId,
                                ProduceProductId = y.ProduceProductId,
                                ProduceProductCode = y.ProduceProductId != 0 ? _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Code).First() : "",
                                ProduceProductName = y.ProduceProductId != 0 ? _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Name).First() : "",
                                ProduceProductType = y.ProduceProductId != 0 ? _db.Products.Include(z => z.CommonCode).Where(z => z.Id == y.ProduceProductId).Select(z => z.CommonCode.Name).First() : "",
                                ProduceProductStandard = y.ProduceProductId != 0 ? _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Standard).First() : "",
                                ProduceProductUnit = y.ProduceProductId != 0 ? _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Unit).First() : "",

                                Memo = y.Memo,
                                IsFinal = y.IsFinal,
                            })
                            .ToList(),

                        ProductItems = _items
                    })
                    .FirstOrDefaultAsync();

                var Res = new Response<BomResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = prd
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<BomResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<BomResponseSortByProcess>> GetBomById2(ProductRequest _prd)
        {
            try
            {
                var prd = await _db.Products
                    .Include(x => x.CommonCode)
                    .Include(x => x.UploadFile)
                    .Include(x => x.Processes).ThenInclude(x => x.Items)
                    .Where(x => x.Id == _prd.ProductId)
                    .Select(x => new BomResponseSortByProcess
                    {
                        ProductId = x.Id,  //Product(BOM) ID
                        ProductCode = x.Code, //품목코드
                        ProductName = x.Name,
                        ProductCommonCodeName = x.CommonCode.Name,//품목구분
                        ProductUnit = x.Unit,
                        ProductIsUsing = x.IsUsing,
                        ProductStandard = x.Standard,

                        ModelId = x.Model != null ? x.Model.Id : 0,
                        ModelName = x.Model != null ? x.Model.Name : "",
                        ModelCode = x.Model != null ? x.Model.Code : "",
                        PartnerId = x.Partner != null ? x.Partner.Id : 0,
                        PartnerName = x.Partner != null ? x.Partner.Name : "",
                        PartnerCode = x.Partner != null ? x.Partner.Code : "",

                        ProductProcesses = x.Processes
                            .Where(y => y.IsDeleted == 0)
                            .Select(y => new BomProductProcessResponseByProcess
                            {
                                ProductProcessId = y.ProductProcessId,
                                ProcessOrder = y.ProcessOrder,
                                ProcessCode = y.Process.Code,
                                ProcessName = y.Process.Name,
                                ProcessCommonCodeName = y.Process.CommonCode.Name,
                                IsUsing = y.Process.IsUsing,
                                IsFinal = y.IsFinal,
                                Memo = y.Memo,
                                IsOutSourcing = y.IsOutSourcing,

                                ProductProducedId = y.ProduceProductId,
                            
                                ProductProducedCode = y.ProduceProductId != 0 ? _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Code).First() : "",
                                ProductProducedName = y.ProduceProductId != 0 ? _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Name).First() : "",
                                ProductProducedCommonCodeName = y.ProduceProductId != 0 ? _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.CommonCode.Name).First() : "",
                                ProductProducedStandard = y.ProduceProductId != 0 ? _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Standard).First() : "",
                                ProductProducedUnit = y.ProduceProductId != 0 ? _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Unit).First() : "",

                                PartnerId = y.PartnerId,
                                PartnerCode = y.Partner.Code,
                                PartnerName = y.Partner.Name,

                                ProductItems = y.Items
                                    .Where(z => z.IsDeleted == 0)
                                    .Select(z => new BomProductItemReponseByProcess
                                    {
                                        ProductItemId = z.ProductItemId,
                                        ProductProcessId = y.ProductProcessId,

                                        ProcessId = y.Process.Id,
                                        ProcessName = y.Process.Name,
                                        ProcessCode = y.Process.Code,
                                        ProcessOrder = z.ProcessOrder,

                                        ProductId = z.Product.Id,
                                        ProductCode = z.Product.Code,
                                        ProductName = z.Product.Name,
                                        ProductUnit = z.Product.Unit,
                                        ProductStandard = z.Product.Standard,
                                        ProductCommonCodeName = z.Product.CommonCode.Name,

                                        Loss = (float)z.Loss,
                                        Memo = z.Memo,
                                        Require = (float)z.Require,
                                        Priority = z.Priority
                                    })
                                    .ToList()
                            }).ToList(),

                    })
                    .FirstOrDefaultAsync();



                //var produceProduct = _db.ProductProcesses


                var Res = new Response<BomResponseSortByProcess>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = prd
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<BomResponseSortByProcess>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<InputProducedProductResponse>>> GetInputProducts(ProductRequest _prd)
        {
            try
            {
                var prds = await _db.Products
                    .Include(x => x.CommonCode)
                    .Include(x => x.UploadFile)
                    .Include(x => x.Processes)
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.Id != _prd.ProductId)
                    .Where(x => _prd.IsUsingStr == "ALL" ? true : x.IsUsing == (_prd.IsUsingStr == "Y" ? true : false))
                    .Where(x => _prd.TypeStr == "ALL" ? (x.CommonCode.Name == "원자재" || x.CommonCode.Name == "부자재" || x.CommonCode.Name == "반제품" || x.CommonCode.Name == "완제품") : x.CommonCode.Name == _prd.TypeStr)
                    .Where(x => x.Name.Contains(_prd.SearchStr) || x.Code.Contains(_prd.SearchStr) || x.Memo.Contains(_prd.SearchStr) || x.Model.Name.Contains(_prd.SearchStr) || x.Partner.Name.Contains(_prd.SearchStr))
                    .Select(
                      x => new InputProducedProductResponse
                      {
                          ProductId = x.Id,
                          Code = x.Code,
                          CommonCodeName = x.CommonCode.Name,
                          Name = x.Name,
                          Standard = x.Standard,
                          Unit = x.Unit,
                          IsUsing = x.IsUsing,
                          Picture = x.UploadFile != null ? x.UploadFile.FileUrl : "",

                          ModelId = x.Model != null ? x.Model.Id : 0,
                          ModelName = x.Model != null ? x.Model.Name : "",
                          ModelCode = x.Model != null ? x.Model.Code : "",
                          PartnerId = x.Partner != null ? x.Partner.Id : 0,
                          PartnerName = x.Partner != null ? x.Partner.Name : "",
                          PartnerCode = x.Partner != null ? x.Partner.Code : "",
                      }
                     )
                    .ToArrayAsync();


                var Res = new Response<IEnumerable<InputProducedProductResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = prds
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<InputProducedProductResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<InputProducedProductResponse>>> GetProducedProducts(ProductRequest _prd)
        {
            try
            {
                var prds = await _db.Products
                    .Include(x => x.CommonCode)
                    .Include(x => x.UploadFile)
                    .Include(x => x.Processes)
                    .Where(x => x.IsDeleted == false)
                  //  .Where(x => x.Id != _prd.ProductId)
                    .Where(x => _prd.IsUsingStr == "ALL" ? true : x.IsUsing == (_prd.IsUsingStr == "Y" ? true : false))
                    .Where(x => _prd.TypeStr == "ALL" ? (x.CommonCode.Name == "완제품" || x.CommonCode.Name == "반제품") : x.CommonCode.Name == _prd.TypeStr)
                    .Where(x => x.Name.Contains(_prd.SearchStr) || x.Code.Contains(_prd.SearchStr) || x.Memo.Contains(_prd.SearchStr))
                    .Select(
                      x => new InputProducedProductResponse
                      {
                          ProductId = x.Id,
                          Code = x.Code,
                          CommonCodeName = x.CommonCode.Name,
                          Name = x.Name,
                          Standard = x.Standard,
                          Unit = x.Unit,
                          IsUsing = x.IsUsing,
                          Picture = x.UploadFile != null ? x.UploadFile.FileUrl : "",
                          Memo = x.Memo,

                          ModelId = x.Model != null ? x.Model.Id : 0,
                          ModelName = x.Model != null ? x.Model.Name : "",
                          ModelCode = x.Model != null ? x.Model.Code : "",
                          PartnerId = x.Partner != null ? x.Partner.Id : 0,
                          PartnerName = x.Partner != null ? x.Partner.Name : "",
                          PartnerCode = x.Partner != null ? x.Partner.Code : "",
                      }
                     )
                    .ToArrayAsync();


                var Res = new Response<IEnumerable<InputProducedProductResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = prds
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<InputProducedProductResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<bool>> UpdateBom(BomUpdateRequest bom)
        {


            try
            {

                var originData = await _db.Products
                    .Include(x => x.CommonCode)
                    .Include(x => x.UploadFile)
                    .Include(x => x.Processes).ThenInclude(x => x.Items)
                    .Where(x => x.Id == bom.ProductId)
                    .Select(x => new BomResponseSortByProcess
                    {
                        ProductId = x.Id,  //Product(BOM) ID
                        ProductCode = x.Code, //품목코드
                        ProductName = x.Name,
                        ProductCommonCodeName = x.CommonCode.Name,//품목구분
                        ProductUnit = x.Unit,
                        ProductIsUsing = x.IsUsing,
                        ProductStandard = x.Standard,
                        ProductProcesses = x.Processes
                            .Where(y => y.IsDeleted == 0)
                            .Select(y => new BomProductProcessResponseByProcess
                            {
                                ProductProcessId = y.ProductProcessId,

                                ProcessOrder = y.ProcessOrder,
                                ProcessCode = y.Process.Code,
                                ProcessName = y.Process.Name,
                                ProcessCommonCodeName = y.Process.CommonCode.Name,

                                IsUsing = y.Process.IsUsing,
                                IsFinal = y.IsFinal,
                                Memo = y.Memo,


                                ProductProducedId = y.ProduceProductId,
                                ProductProducedCode = y.Product.Code,
                                ProductProducedName = y.Product.Name,
                                ProductProducedCommonCodeName = y.Product.CommonCode.Name,
                                ProductProducedStandard = y.Product.Standard,
                                ProductProducedUnit = y.Product.Standard,

                                ProductItems = y.Items
                                    .Where(z => z.IsDeleted == 0)
                                    .Select(z => new BomProductItemReponseByProcess
                                    {
                                        ProductItemId = z.ProductItemId,

                                        ProcessName = y.Process.Name,
                                        ProcessId = y.Process.Id,
                                        ProcessCode = y.Process.Code,
                                        ProcessOrder = z.ProcessOrder,

                                        ProductId = z.ProductId,
                                        ProductCode = z.Product.Code,
                                        ProductName = z.Product.Name,
                                        ProductUnit = z.Product.Unit,
                                        ProductStandard = z.Product.Standard,
                                        ProductCommonCodeName = z.Product.CommonCode.Name,
                                        Loss = (float)z.Loss,
                                        Memo = z.Memo,
                                        Require = (float)z.Require,
                                        Priority = z.Priority
                                    })
                                    .ToList()
                            }).ToList(),

                    })
                    .FirstOrDefaultAsync();

                //0. 아이템 분류
                List<int> _deleteProductProcesses = new List<int>();
                List<ProductProcessUpdate> _updateProductProcesses = new List<ProductProcessUpdate>();
                List<ProductProcessUpdate> _createProductProcesses = new List<ProductProcessUpdate>();

                List<int> _deleteProductItems = new List<int>();
                List<ProductItemUpdate> _updateProductItems = new List<ProductItemUpdate>();
                List<ProductItemUpdate> _createProductItems = new List<ProductItemUpdate>();

                bool checkFlag = false;
                foreach (var _orig in originData.ProductProcesses)
                {
                    checkFlag = false;
                    foreach (var _proc in bom.ProductProcesses)
                    {
                        if (_proc.ProductProcessId == _orig.ProductProcessId)
                        {
                            _updateProductProcesses.Add(_proc);
                            checkFlag = true;
                        }
                    }
                    if (!checkFlag)
                    {
                        _deleteProductProcesses.Add(_orig.ProductProcessId);
                    }
                }

                foreach (var _proc in bom.ProductProcesses)
                {
                    if (_proc.ProductProcessId == 0)
                    {
                        _createProductProcesses.Add(_proc);
                    }
                }

                foreach (var _orig in originData.ProductProcesses)
                {

                    foreach (var _item in _orig.ProductItems)
                    {
                        checkFlag = false;

                        foreach (var _bomItem in bom.ProductItems)
                        {
                            if (_bomItem.ProductItemId == _item.ProductItemId)
                            {
                                _updateProductItems.Add(_bomItem);
                                checkFlag = true;
                            }
                        }

                        if (!checkFlag)
                        {
                            _deleteProductItems.Add(_item.ProductItemId);
                        }
                    }
                }


                foreach (var _item in bom.ProductItems)
                {
                    if (_item.ProductItemId == 0)
                    {
                        _createProductItems.Add(_item);
                    }
                }


                //1. 기존에 있었지만 사라진 아이템 DELETE
                //2. UPDATE
                //3. CREATE 순으로 진행.
                foreach (var i in _deleteProductProcesses)
                {
                    var _item = await _db.ProductProcesses.Where(x => x.ProductProcessId == i).FirstOrDefaultAsync();
                    _item.IsDeleted = 1;

                    _db.ProductProcesses.Update(_item);
                    await Save();
                }

                foreach (var _proc in _updateProductProcesses)
                {
                    var _item = await _db.ProductProcesses.Where(x => x.ProductProcessId == _proc.ProductProcessId).FirstOrDefaultAsync();
                    _item.ProcessOrder = _proc.ProcessOrder;
                    _item.PartnerId = _proc.PartnerId;
                    _item.ProcessId = _proc.ProcessId;
                    _item.IsFinal = _proc.IsFinal;
                    _item.ProduceProductId = _proc.ProduceProductId;
                    _item.IsOutSourcing = _proc.IsOutSourcing;
                    _item.Memo = _proc.Memo;
                    _db.ProductProcesses.Update(_item);
                    await Save();
                }

                foreach (var _proc in _createProductProcesses)
                {
                    var _product = _db.Products.Where(x => x.Id == bom.ProductId).FirstOrDefault();

                    var _item = new ProductProcess
                    {
                        ProcessOrder = _proc.ProcessOrder,
                        PartnerId = _proc.PartnerId,
                        ProcessId = _proc.ProcessId,
                        Product = _product,
                        IsFinal = _proc.IsFinal,
                        ProduceProductId = _proc.ProduceProductId,
                        IsOutSourcing = _proc.IsOutSourcing,
                        Memo = _proc.Memo
                    };

                    _db.ProductProcesses.Add(_item);
                    await Save();
                }







                foreach (var i in _deleteProductItems)
                {
                    var _item = await _db.ProductItems.Where(x => x.ProductItemId == i).FirstOrDefaultAsync();

                    _item.IsDeleted = 1;
                    _db.ProductItems.Update(_item);
                    await Save();
                }


                foreach (var _proc in _updateProductItems)
                {
                    var _item = await _db.ProductItems.Where(x => x.ProductItemId == _proc.ProductItemId).FirstOrDefaultAsync();


                    _item.ProcessOrder = _proc.ProcessOrder;
                    _item.ProductProcessId = _proc.ProductProcessId == 0 ? _db.ProductProcesses.Where(x => x.ProcessOrder == _proc.ProcessOrder && bom.ProductId == x.ProductId && x.IsDeleted == 0).Select(x => x.ProductProcessId).FirstOrDefault() : _proc.ProductProcessId;
                    _item.ProcessId = _proc.ProcessId;
                    _item.Product = _db.Products.Where(x => x.Id == _proc.ProductId).FirstOrDefault();
                    _item.ProductId = _proc.ProductId;

                    _item.Require = _proc.Require;
                    _item.Loss = _proc.Loss;
                    _item.Memo = _proc.Memo;
                    _item.Priority = _proc.Priority;

                    _db.ProductItems.Update(_item);
                    await Save();
                }


                foreach (var _proc in _createProductItems)
                {
                    var _item = new ProductItem
                    {
                        ProcessOrder = _proc.ProcessOrder,
                        ProductProcessId = _proc.ProductProcessId == 0 ? _db.ProductProcesses.Where(x => x.ProcessOrder == _proc.ProcessOrder && bom.ProductId == x.ProductId && x.IsDeleted == 0).Select(x => x.ProductProcessId).FirstOrDefault() : _proc.ProductProcessId,
                        ///ProductProcessId = _proc.ProductProcessId,
                        ProcessId = _proc.ProcessId,

                        ProductId = _proc.ProductId,
                        Product = _db.Products.Where(x => x.Id == _proc.ProductId).FirstOrDefault(),

                        IsDeleted = 0,
                        Require = _proc.Require,
                        Loss = _proc.Loss,
                        Memo = _proc.Memo,
                        Priority = _proc.Priority,
                    };

                    _db.ProductItems.Add(_item);
                    await Save();
                }

                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = true
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = false
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<ProductResponse>>> GetProductsBySearch(ProductRequest _prd)
        {
            try
            {
                var prds = await _db.Products
                    .Include(x => x.CommonCode)
                    .Include(x => x.UploadFile)
                    .Include(x => x.Processes).ThenInclude(x => x.Items)
                    //  .Include(x => x.Items)
                    .Where(x => x.IsDeleted == false)
                    .Where(x => _prd.IsUsingStr == "ALL" ? true : x.IsUsing == (_prd.IsUsingStr == "Y" ? true : false))
                    .Where(x => _prd.TypeStr == "ALL" ? true : x.CommonCode.Name == _prd.TypeStr)
                    .Select(
                      x => new ProductResponse
                      {
                          Id = x.Id,
                          CommonCode = x.CommonCode.Id,
                          CommonCodeName = x.CommonCode.Name,
                          Code = x.Code,
                          Name = x.Name,
                          Unit = x.Unit,
                          Standard = x.Standard,
                          OptimumStock = x.OptimumStock,
                          Memo = x.Memo,
                          UploadFile = x.UploadFile,
                          Picture = x.UploadFile == null ? "" : x.UploadFile.FileUrl,
                          ProcessCount = x.Processes.ToList().Count(),


                          ModelId = x.Model != null ? x.Model.Id : 0,
                          ModelName = x.Model != null ? x.Model.Name : "",
                          ModelCode = x.Model != null ? x.Model.Code : "",
                          PartnerId = x.Partner != null ? x.Partner.Id : 0,
                          PartnerName = x.Partner != null ? x.Partner.Name : "",
                          PartnerCode = x.Partner != null ? x.Partner.Code : "",
                          // ItemCount = x.Processes.,
                      }
                     )
                    .OrderBy(x => x.Id)
                    .ToArrayAsync();


                var Res = new Response<IEnumerable<ProductResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = prds
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProductResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<ProductResponse>> GetProduct(int id)
        {
            try
            {
                var _item = await _db.Products
                    .Include(x => x.CommonCode)
                    .Where(x => x.Id == id)
                    .Select(x => new ProductResponse
                    {
                        Id = x.Id,
                        CommonCode = x.CommonCode.Id,
                        CommonCodeName = x.CommonCode.Name,
                        Code = x.Code,
                        Name = x.Name,
                        Unit = x.Unit,
                        Standard = x.Standard,
                        OptimumStock = x.OptimumStock,
                        Memo = x.Memo,
                        UploadFile = x.UploadFile,
                        Picture = x.UploadFile == null ? "" : x.UploadFile.FileUrl,
                        TaxType = x.TaxType == null ? "-" : x.TaxType,
                        ImportCheck = x.ImportCheck == null ? false : x.ImportCheck,
                        ExportCheck = x.ExportCheck == null ? false : x.ExportCheck,
                        BuyPrice = x.BuyPrice == null ? 0 : x.BuyPrice,
                        SellPrice = x.SellPrice == null ? 0 : x.SellPrice,


                        ModelId = x.Model != null ? x.Model.Id : 0,
                        ModelName = x.Model != null ? x.Model.Name : "",
                        ModelCode = x.Model != null ? x.Model.Code : "",
                        PartnerId = x.Partner != null ? x.Partner.Id : 0,
                        PartnerName = x.Partner != null ? x.Partner.Name : "",
                        PartnerCode = x.Partner != null ? x.Partner.Code : "",



                    })
                    .FirstOrDefaultAsync();

                var Res = new Response<ProductResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _item
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<ProductResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<ProductResponse>>> GetProducts(int code)
        {
            try
            {
                if (code == 0)
                {
                    var _item = await _db.Products
                         .Include(x => x.CommonCode)
                        .Where(x => x.IsDeleted == false)
                        .Select(x => new ProductResponse
                        {
                            Id = x.Id,
                            CommonCode = x.CommonCode.Id,
                            CommonCodeName = x.CommonCode.Name,
                            Code = x.Code,
                            Name = x.Name,
                            Unit = x.Unit,
                            Standard = x.Standard,
                            OptimumStock = x.OptimumStock,
                            Memo = x.Memo,
                            UploadFile = x.UploadFile,
                            Picture = x.UploadFile == null ? "" : x.UploadFile.FileUrl,

                            TaxType = x.TaxType,
                            ImportCheck = x.ImportCheck,
                            ExportCheck = x.ExportCheck,
                            BuyPrice = x.BuyPrice,
                            SellPrice = x.SellPrice,

                            ModelId = x.Model != null ? x.Model.Id : 0,
                            ModelName = x.Model != null ? x.Model.Name:"",
                            ModelCode = x.Model != null ? x.Model.Code : "",
                            PartnerId = x.Partner != null ? x.Partner.Id : 0,
                            PartnerName = x.Partner != null ? x.Partner.Name : "",
                            PartnerCode = x.Partner != null ? x.Partner.Code : "",
                        })
                        .ToArrayAsync();

                    var Res = new Response<IEnumerable<ProductResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _item
                    };
                    return Res;
                }
                else
                {
                    var _item = await _db.Products
                        .Include(x => x.CommonCode)
                        .Where(x => x.IsDeleted == false && x.CommonCode.Id == code)
                        .Select(x => new ProductResponse
                        {
                            Id = x.Id,
                            CommonCode = x.CommonCode.Id,
                            CommonCodeName = x.CommonCode.Name,
                            Code = x.Code,
                            Name = x.Name,
                            Unit = x.Unit,
                            Standard = x.Standard,
                            OptimumStock = x.OptimumStock,
                            Memo = x.Memo,
                            UploadFile = x.UploadFile,
                            Picture = x.UploadFile == null ? "" : x.UploadFile.FileUrl,
                            TaxType = x.TaxType,
                            ImportCheck = x.ImportCheck,
                            ExportCheck = x.ExportCheck,
                            BuyPrice = x.BuyPrice,
                            SellPrice = x.SellPrice,


                            ModelId = x.Model != null ? x.Model.Id : 0,
                            ModelName = x.Model != null ? x.Model.Name : "",
                            ModelCode = x.Model != null ? x.Model.Code : "",
                            PartnerId = x.Partner != null ? x.Partner.Id : 0,
                            PartnerName = x.Partner != null ? x.Partner.Name : "",
                            PartnerCode = x.Partner != null ? x.Partner.Code : "",
                        })
                        .ToArrayAsync();
                    var Res = new Response<IEnumerable<ProductResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _item
                    };
                    return Res;
                }
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProductResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<ProductResponse>>> CreateProduct(ProductRequest x)
        {
            try
            {

                string _code = x.Code;

                if (x.AutoCode)
                {
                    var prevCodes = await _db.Products
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code.Length == 5)
                        .Where(y => y.Code.Substring(0, 1) == "I")
                        .Select(y => y.Code.Substring(1, 4))
                        .OrderByDescending(y => y)
                        .FirstOrDefaultAsync();


                    if (prevCodes == null)
                    {
                        _code = "I0001";
                    }
                    else
                    {
                        try
                        {
                            _code = "I" + (Convert.ToInt32(prevCodes) + 1).ToString("0000");
                        }
                        catch
                        {
                            _code = "I0001";
                        }
                    }
                }


                if (!x.AutoCode)
                {
                    var codeCheck = await _db.Products
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == x.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<IEnumerable<ProductResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = x.Code + "는 이미 존재하는 코드입니다.",
                            Data = null
                        };

                        return ErrorReturn;
                    }

                }


                var _common = await _db.CommonCodes.Where(y => y.Name == x.CommonCodeName).FirstOrDefaultAsync();
                var _partner = await _db.Partners.Where(y => y.Id == x.PartnerId).FirstOrDefaultAsync();
                var _model = await _db.CommonCodes.Where(y => y.Id == x.ModelId).FirstOrDefaultAsync();
                var _product = new Product
                {
                    CommonCode = _common,
                    Code = _code,
                    Name = x.Name,
                    Unit = x.Unit,
                    Standard = x.Standard,
                    OptimumStock = x.OptimumStock,
                    Memo = x.Memo,
                    IsUsing = x.IsUsing,
                    IsDeleted = x.IsDeleted,
                    UploadFile = x.UploadFile,
                    TaxType = x.TaxType,
                    ImportCheck = x.ImportCheck,
                    ExportCheck = x.ExportCheck,
                    BuyPrice = x.BuyPrice,
                    SellPrice = x.SellPrice,
                    Model = _model,
                    Partner = _partner
                };

                await _db.Products.AddAsync(_product);
                await Save();


                var Res = new Response<IEnumerable<ProductResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProductResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        public async Task<Response<IEnumerable<ProductResponse>>> UpdateProduct(ProductRequest product, int id)
        {
            try
            {
                var _product = await _db.Products.Include(x=>x.UploadFile).Where(x => x.Id == product.Id).FirstOrDefaultAsync();
                if (_product.Code != product.Code)
                {
                    var _duplicate = await _db.Products.Where(x => x.Code == product.Code && x.IsDeleted == false).FirstOrDefaultAsync();

                    if (_duplicate != null)
                    {
                        var error = new Response<IEnumerable<ProductResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = "중복된 제품 코드가 있습니다. 확인해주세요.",
                            Data = null
                        };

                        return error;
                    }
                }


                var _common = await _db.CommonCodes.Where(y => y.Name == product.CommonCodeName).FirstOrDefaultAsync();
                var _partner = await _db.Partners.Where(y => y.Id == product.PartnerId).FirstOrDefaultAsync();
                var _model = await _db.CommonCodes.Where(y => y.Id == product.ModelId).FirstOrDefaultAsync();


                _product.CommonCode = _common;
                _product.Code = product.Code;
                _product.Name = product.Name;
                _product.Unit = product.Unit;
                _product.Standard = product.Standard;
                _product.OptimumStock = product.OptimumStock;
                _product.Memo = product.Memo;
                _product.IsUsing = product.IsUsing;
                _product.IsDeleted = product.IsDeleted;
                _product.UploadFile = product.UploadFile;

                _product.TaxType = product.TaxType;
                _product.ImportCheck = product.ImportCheck;
                _product.ExportCheck = product.ExportCheck;
                _product.BuyPrice = product.BuyPrice;
                _product.SellPrice = product.SellPrice;

                _product.Model = _model;
                _product.Partner = _partner;

                _db.Products.Update(_product);

                await Save();

                var _result = await _db.Products
                     .Include(x => x.CommonCode)
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new ProductResponse
                    {
                        Id = x.Id,
                        CommonCode = x.CommonCode.Id,
                        CommonCodeName = x.CommonCode.Name,
                        Code = x.Code,
                        Name = x.Name,
                        Unit = x.Unit,
                        Standard = x.Standard,
                        OptimumStock = x.OptimumStock,
                        Memo = x.Memo,
                        UploadFile = x.UploadFile,
                        Picture = x.UploadFile == null ? "" : x.UploadFile.FileUrl,
                        TaxType = x.TaxType == null ? "-" : x.TaxType,
                        ImportCheck = x.ImportCheck == null ? false : x.ImportCheck,
                        ExportCheck = x.ExportCheck == null ? false : x.ExportCheck,
                        BuyPrice = x.BuyPrice == null ? 0 : x.BuyPrice,
                        SellPrice = x.SellPrice == null ? 0 : x.SellPrice,

                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<ProductResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _result
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProductResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
      
        }
        public async Task<Response<IEnumerable<ProductResponse>>> DeleteProducts(int[] id)
        {
            try
            {
                List<Product> _product = new List<Product>();
                foreach (var i in id)
                {
                    var _item = await _db.Products.Include(x => x.CommonCode).Where(x => x.Id == i).FirstOrDefaultAsync();
                    _product.Add(_item);
                }

                if (_product.Count > 0)
                {
                    foreach (var i in _product)
                    {
                        i.IsDeleted = true;
                    }

                    _db.Products.UpdateRange(_product);
                    await Save();
                }


                var _result = await _db.Products
                     .Include(x => x.CommonCode)
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new ProductResponse
                    {
                        Id = x.Id,
                        CommonCode = x.CommonCode.Id,
                        CommonCodeName = x.CommonCode.Name,
                        Code = x.Code,
                        Name = x.Name,
                        Unit = x.Unit,
                        Standard = x.Standard,
                        OptimumStock = x.OptimumStock,
                        Memo = x.Memo,
                        UploadFile = x.UploadFile,
                        Picture = x.UploadFile == null ? "" : x.UploadFile.FileUrl,
                        TaxType = x.TaxType == null ? "-" : x.TaxType,
                        ImportCheck = x.ImportCheck == null ? false : x.ImportCheck,
                        ExportCheck = x.ExportCheck == null ? false : x.ExportCheck,
                        BuyPrice = x.BuyPrice == null ? 0 : x.BuyPrice,
                        SellPrice = x.SellPrice == null ? 0 : x.SellPrice,

                    })
                    .ToArrayAsync();


                var Res = new Response<IEnumerable<ProductResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProductResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }

        #endregion Product Manage


       public async Task<Response<IEnumerable<ProcessNotWorkPopupResponse>>> GetProcessNotWorksBySearch(ProcessNotWorkPopupRequest req)
       {
            try
            {

                var res = await _db.CommonCodes
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.SortCode.Name == "비가동유형")
                    .Where(x => x.Name.Contains(req.SearchInput) || x.Code.Contains(req.SearchInput))
                    .Where(x => req.ShutdownIsUsing == "ALL"? true : req.ShutdownIsUsing == "Y"? x.IsUsing == true : x.IsUsing == false)
                    .Select(x => new ProcessNotWorkPopupResponse
                    {
                        shutdownCode = x.Code,
                        shutdownName = x.Name,
                        shutdownCodeId = x.Id,
                        shutdownIsUsing = x.IsUsing,
                        shutdownMemo = x.Memo
                    }).ToListAsync();
                    

                var Res = new Response<IEnumerable<ProcessNotWorkPopupResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessNotWorkPopupResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }

        public async Task<Response<IEnumerable<CommonCodeResponse>>> GetCleaningCommonCodes()
        {
            try
            {
                var _common = await _db.CommonCodes
                    .Include(x => x.SortCode)
                    .Where(x => x.SortCode.Code == "G0006" || x.SortCode.Code == "G0007")
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new CommonCodeResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        IsUsing = x.IsUsing,
                        Memo = x.Memo,
                        Creator = x.Creator.FullName,
                        CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                        SortCode = x.SortCode.Code,
                        SortCodeName = x.SortCode.Name
                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<CommonCodeResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _common
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<CommonCodeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<CommonCodeResponse>>> GetRepairCommonCodes()
        {
            try
            {
                var _common = await _db.CommonCodes
                    .Include(x => x.SortCode)
                    .Where(x => x.SortCode.Code == "G0008" || x.SortCode.Code == "G0009" || x.SortCode.Code == "G0013")

                    .Where(x => x.IsDeleted == false)
                    .Select(x => new CommonCodeResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        IsUsing = x.IsUsing,
                        Memo = x.Memo,
                        Creator = x.Creator.FullName,
                        CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                        SortCode = x.SortCode.Code,
                        SortCodeName = x.SortCode.Name
                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<CommonCodeResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _common
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<CommonCodeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<IEnumerable<CommonCodeResponse>>> GetDownTimes(DownTimeRequest req)
        {
            try
            {
                var _common = await _db.CommonCodes
                    .Include(x => x.SortCode)
                    .Where(x => x.SortCode.Name == "비가동유형")
                    .Where(x => x.IsDeleted == false)
                    .Where(x => req.IsUsing == "ALL" ? true : req.IsUsing == "Y"? x.IsUsing == true  : x.IsUsing == false)
                    .Where(x => x.Name.Contains(req.SearchStr) || x.Code.Contains(req.SearchStr) || x.Memo.Contains(req.SearchStr))
                    .Select(x => new CommonCodeResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                        Creator = x.Creator.FullName,
                        IsUsing = x.IsUsing,
                        Memo = x.Memo
                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<CommonCodeResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _common
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<CommonCodeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }



        public async Task<Response<IEnumerable<CommonCodeResponse>>> GetModels(DownTimeRequest req)
        {
            try
            {
                var _common = await _db.CommonCodes
                    .Include(x => x.SortCode)
                    .Where(x => x.SortCode.Code == "G0014")
                    .Where(x => x.IsDeleted == false)
                    .Where(x => req.IsUsing == "ALL" ? true : req.IsUsing == "Y" ? x.IsUsing == true : x.IsUsing == false)
                    .Where(x => x.Name.Contains(req.SearchStr) || x.Code.Contains(req.SearchStr) || x.Memo.Contains(req.SearchStr))
                    .Select(x => new CommonCodeResponse
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                        Creator = x.Creator.FullName,
                        IsUsing = x.IsUsing,
                        Memo = x.Memo
                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<CommonCodeResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _common
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<CommonCodeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

    }
}
