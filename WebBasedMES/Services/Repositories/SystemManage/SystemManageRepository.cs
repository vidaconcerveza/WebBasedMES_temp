using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.JwtAuth;
using WebBasedMES.Services.JwtAuth;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.SystemManage;

namespace WebBasedMES.Services.Repositories.SystemManage
{
    public class SystemManageRepository : ISystemManageRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public SystemManageRepository(ApiDbContext db, UserManager<ApplicationUser> userManager, IOptionsMonitor<JwtConfig> optionMonitor)
        {
            _db = db;
            _userManager = userManager;
            _jwtConfig = optionMonitor.CurrentValue;
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }


        public async Task<Response<IEnumerable<UserPopupResponse>>> GetUsersPopupBySearch(UserManageRequest _req)
        {
            try
            {
                var userList = await _userManager.Users
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.IsOut == false)
                    .Where(x => _req.RegisterIsApproved == "ALL" ? true : x.IsApproved == (_req.RegisterIsApproved == "Y" ? true : false))
                    .Where(x => x.FullName.Contains(_req.SearchInput) || x.Department.Name.Contains(_req.SearchInput))
                    
                   .Select(x => new UserPopupResponse
                   {
                       RegisterId = x.Id,
                       RegisterDepartment = x.Department.Name,
                       RegisterName = x.FullName,
                       RegisterIsApproved = x.IsApproved ? "승인":"미승인",
                       RegisterEmail = x.Email,
                       RegisterPhoneNumber = x.PhoneNumber,
                       RegisterNo = x.IdNumber,

                       UserId = x.Id,
                       UserDepartment = x.Department.Name,
                       UserFullName = x.FullName,
                       UserIsApproved = x.IsApproved ? "승인":"미승인",
                       UserEmail = x.Email,
                       UserPhoneNumber = x.PhoneNumber,
                       UserNo = x.IdNumber,
                       UserPosition = x.Position.Name
                   }).ToListAsync();

                var Res = new Response<IEnumerable<UserPopupResponse>>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = userList
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<UserPopupResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        //1.사용자 관리

        public async Task<Response<IEnumerable<UserManageResponse>>> GetUsersBySearch(UserManageRequest _user)
        {
            try
            {
                var userList = await _userManager.Users
                    .Where(x => _user.IsApprovedStr == "ALL" ? true : x.IsApproved == (_user.IsApprovedStr == "Y" ? true : false))
                    .Where(x => x.FullName.Contains(_user.SearchStr) || x.Department.Name.Contains(_user.SearchStr) || x.IdNumber.Contains(_user.SearchStr) || x.Position.Name.Contains(_user.SearchStr))
                    .Where(x => x.IsDeleted == false)
                   .Select(x => new UserManageResponse
                   {
                       Uuid = x.Id,
                       Department = x.Department.Name,
                       FullName = x.FullName,
                       Email = x.Email.Contains("tempemail.com") ? "" : x.Email,
                       IdNumber = x.IdNumber,
                       IsApproved = x.IsApproved == true ? 1 : 0,
                       Position = x.Position.Name,
                       BirthDay = x.BirthDay.ToString("yyyy-MM-dd"),
                       InDate = x.InDate.ToString("yyyy-MM-dd"),
                       OutDate = x.OutDate.ToString("yyyy-MM-dd"),
                       IsOut = x.IsOut == true ? 1 : 0,
                       EmployType = x.EmployType.Name,
                       PhoneNumber = x.PhoneNumber,
                       UserRole = x.UserRole.Name,
                   }).ToListAsync();

                var Res = new Response<IEnumerable<UserManageResponse>>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = userList
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<UserManageResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<IEnumerable<UserManageResponse>>> GetAllUsers()
        {
            try
            {
                var userList = await _userManager.Users
                    .Where(x => x.IsDeleted == false)
                   .Select(x => new UserManageResponse
                   {
                       Uuid = x.Id,
                       Department = x.Department.Name,
                       FullName = x.FullName,
                       Email = x.Email,
                       IdNumber = x.IdNumber,
                       IsApproved = x.IsApproved == true ? 1 : 0,
                       Position = x.Position.Name,
                       BirthDay = x.BirthDay.ToString("yyyy-MM-dd"),
                       InDate = x.InDate.ToString("yyyy-MM-dd"),
                       OutDate = x.OutDate.ToString("yyyy-MM-dd"),
                       IsOut = x.IsOut == true ? 1 : 0,
                       EmployType = x.EmployType.Name,
                       PhoneNumber = x.PhoneNumber,
                       UserRole = x.UserRole.Name,
                   }).ToListAsync();

                var Res = new Response<IEnumerable<UserManageResponse>>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = userList
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<UserManageResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        // By Id 
        public async Task<Response<IEnumerable<UserManageResponse>>> GetUsers(string[] uuids)
        {
            try
            {
                List<UserManageResponse> userList = new List<UserManageResponse>();

                foreach (var i in uuids)
                {
                    var user = await _userManager.Users.Where(x => x.Id == i && x.IsDeleted == false)
                        .Select(x => new UserManageResponse
                        {
                            Uuid = x.Id,
                            Department = x.Department.Name,
                            FullName = x.FullName,
                            Email = x.Email,
                            IdNumber = x.IdNumber,
                            IsApproved = x.IsApproved == true ? 1 : 0,
                            Position = x.Position.Name,
                            BirthDay = x.BirthDay.ToString("yyyy-MM-dd"),
                            InDate = x.InDate.ToString("yyyy-MM-dd"),
                            OutDate = x.OutDate.ToString("yyyy-MM-dd"),
                            IsOut = x.IsOut == true ? 1 : 0,
                            EmployType = x.EmployType.Name,
                            PhoneNumber = x.PhoneNumber,
                            UserRole = x.UserRole.Name,
                        }).FirstOrDefaultAsync();

                    userList.Add(user);
                }

                var Res = new Response<IEnumerable<UserManageResponse>>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = userList
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<UserManageResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<UserManageResponse>> GetUser(string uuid)
        {
            try
            {
                var result = await _userManager.Users
                    .Where(x => x.Id == uuid && x.IsDeleted == false)
                    .Select(x => new UserManageResponse
                    {
                        Uuid = x.Id,
                        Department = x.Department.Name,
                        FullName = x.FullName,
                        Email = x.Email.Contains("tempemail.com") ? "" : x.Email,
                        IdNumber = x.IdNumber,
                        IsApproved = x.IsApproved == true ? 1 : 0,
                        Position = x.Position.Name,
                        BirthDay = x.BirthDay.ToString("yyyy-MM-dd") == "0001-01-01"? "" : x.BirthDay.ToString("yyyy-MM-dd"),
                        InDate = x.InDate.ToString("yyyy-MM-dd"),
                        OutDate = x.OutDate.ToString("yyyy-MM-dd"),
                        IsOut = x.IsOut == true ? 1 : 0,
                        EmployType = x.EmployType.Name,
                        PhoneNumber = x.PhoneNumber,
                        UserRole = x.UserRole.Name,
                        Memo = x.Memo,
                        Menus = x.Menu.Where(y => y.Name != "즐겨찾기").Select(x => new Menu
                        {
                            Id = x.Id,
                            Icon = x.Icon,
                            IsAbleToAccess = x.IsAbleToAccess,
                            IsAbleToRead = x.IsAbleToRead,
                            IsAbleToReadWrite = x.IsAbleToReadWrite,
                            IsAbleToDelete = x.IsAbleToDelete,
                            Path = x.Path,
                            Order = x.Order,
                            Name = x.Name,

                            Submenu = x.Submenu.Select(y => new SubMenu
                            {
                                Id = y.Id,
                                IsAbleToAccess = y.IsAbleToAccess,
                                IsAbleToRead = y.IsAbleToRead,
                                IsAbleToReadWrite = y.IsAbleToReadWrite,
                                IsAbleToDelete = y.IsAbleToDelete,
                                IsFavorite = y.IsFavorite,
                                Path = y.Path,
                                Order = y.Order,
                                Name = y.Name,
                            }).OrderBy(y => y.Order).ToList()



                        }).OrderBy(x => x.Order).ToList()

                    })
                .FirstAsync();

                List<SubMenu> _fav = new List<SubMenu>();
                if (result.Menus != null)
                {
                    foreach (var x in result.Menus)
                    {
                        foreach (var y in x.Submenu)
                        {
                            if (y.IsFavorite)
                            {
                                _fav.Add(y);
                            }
                        }
                    }
                }


                var result2 = await _userManager.Users
                    .Where(x => x.Id == uuid && x.IsDeleted == false)
                    .Select(x => new UserManageResponse
                    {
                        Uuid = x.Id,
                        Department = x.Department.Name,
                        FullName = x.FullName,
                        Email = x.Email.Contains("tempemail.com") ? "" : x.Email,
                        IdNumber = x.IdNumber,
                        IsApproved = x.IsApproved == true ? 1 : 0,
                        Position = x.Position.Name,
                        BirthDay = x.BirthDay.ToString("yyyy-MM-dd"),
                        InDate = x.InDate.ToString("yyyy-MM-dd"),
                        OutDate = x.OutDate.ToString("yyyy-MM-dd"),
                        IsOut = x.IsOut == true ? 1 : 0,
                        EmployType = x.EmployType.Name,
                        PhoneNumber = x.PhoneNumber,
                        UserRole = x.UserRole.Name,
                        FavoriteMenu = _fav,
                        Memo = x.Memo,

                        Menus = x.Menu.Where(y => y.Name != "즐겨찾기").Select(x => new Menu
                        {
                            Id = x.Id,
                            Icon = x.Icon,
                            IsAbleToAccess = x.IsAbleToAccess,
                            IsAbleToRead = x.IsAbleToRead,
                            IsAbleToReadWrite = x.IsAbleToReadWrite,
                            IsAbleToDelete = x.IsAbleToDelete,
                            Path = x.Path,
                            Order = x.Order,
                            Name = x.Name,

                            Submenu = x.Submenu.Select(y => new SubMenu
                            {
                                Id = y.Id,
                                IsAbleToAccess = y.IsAbleToAccess,
                                IsAbleToRead = y.IsAbleToRead,
                                IsAbleToReadWrite = y.IsAbleToReadWrite,
                                IsAbleToDelete = y.IsAbleToDelete,
                                IsFavorite = y.IsFavorite,
                                Path = y.Path,
                                Order = y.Order,
                                Name = y.Name,
                            }).OrderBy(y => y.Order).ToList()



                        }).OrderBy(x => x.Order).ToList()

                    })
                    .FirstAsync();


                var Res = new Response<UserManageResponse>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result2
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<UserManageResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        //1.사용자 관리
        public async Task<Response<IEnumerable<UserManageResponse>>> CreateUser(UserManageRequest user)
        {
            try
            {
                var checkUser = await _userManager.FindByEmailAsync(user.Email);
                if (checkUser != null)
                {
                    var _res = new Response<IEnumerable<UserManageResponse>>
                    {
                        IsSuccess = false,
                        ErrorMessage = "중복되는 사용자가 있습니다.",
                        Data = null
                    };
                    return _res;
                }

                var _department = await _db.Departments.Where(x => x.Name == user.Department).FirstAsync();
                var _employType = await _db.EmployTypes.Where(x => x.Name == user.EmployType).FirstAsync();
                var _position = await _db.Positions.Where(x => x.Name == user.Position).FirstAsync();
                var _userRole = await _db.UserRoles.Where(x => x.Name == user.UserRole).FirstAsync();

                if (user.BirthDay == "")
                    user.BirthDay = "0001-01-01";

                if (user.Email == "")
                    user.Email = user.IdNumber + "@tempemail.com";

                var _user = new ApplicationUser
                {
                    FullName = user.FullName,
                    Department = _department,
                    EmployType = _employType,
                    Position = _position,
                    UserRole = _userRole,
                    IdNumber = user.IdNumber,
                    Email = user.Email,
                    UserName = user.IdNumber,
                    BirthDay = Convert.ToDateTime(user.BirthDay),
                    InDate = Convert.ToDateTime(user.InDate),
                    PhoneNumber = user.PhoneNumber,
                    IsApproved = user.IsApproved == 1? true : false,
                    Memo = user.Memo,
                    Menu = _db.DefaultMenus.Select(x => new Menu
                    {
                        Icon = x.Icon,
                        IsAbleToAccess = x.IsAbleToAccess,
                        IsAbleToRead = x.IsAbleToRead,
                        IsAbleToReadWrite = x.IsAbleToReadWrite,
                        IsAbleToDelete = x.IsAbleToDelete,
                        Path = x.Path,
                        Order = x.Order,
                        Name = x.Name,
                        Submenu = x.Submenu.Select(y => new SubMenu
                        {
                            IsAbleToAccess = y.IsAbleToAccess,
                            IsAbleToRead = y.IsAbleToRead,
                            IsAbleToReadWrite = y.IsAbleToReadWrite,
                            IsAbleToDelete = y.IsAbleToDelete,
                            IsFavorite = y.IsFavorite,
                            Path = y.Path,
                            Order = y.Order,
                            Name = y.Name,
                        }).OrderBy(y => y.Order).ToList()
                    }).ToList()
                };
                var result = await _userManager.CreateAsync(_user, "Password!2#4");

                if (result.Succeeded)
                {
                    await GenerateJwtToken(_user);

                    var userList = await _userManager.Users
                        .Where(x => x.IsDeleted == false)
                        .Select(x => new UserManageResponse
                        {
                            Uuid = x.Id,
                            Department = x.Department.Name,
                            FullName = x.FullName,
                            Email = x.Email,
                            IdNumber = x.IdNumber,
                            IsApproved = x.IsApproved == true ? 1 : 0,
                            Position = x.Position.Name,
                            BirthDay = x.BirthDay.ToString("yyyy-MM-dd"),
                            InDate = x.InDate.ToString("yyyy-MM-dd"),
                            OutDate = x.OutDate.ToString("yyyy-MM-dd"),
                            IsOut = x.IsOut == true ? 1 : 0,
                            EmployType = x.EmployType.Name,
                            PhoneNumber = x.PhoneNumber,
                            UserRole = x.UserRole.Name,
                        }).ToListAsync();

                    var Res = new Response<IEnumerable<UserManageResponse>>
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = userList
                    };

                    return Res;
                }
                else
                {
                    var Res = new Response<IEnumerable<UserManageResponse>>
                    {
                        IsSuccess = false,
                        ErrorMessage = "",
                        Data = null
                    };
                    return Res;
                }

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<UserManageResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
      
        }
        public async Task<Response<IEnumerable<UserManageResponse>>> ReplaceUser(List<UserManageRequest> users)
        {
            try
            {
                int updated = 0;
                int created = 0;
                string errorMessage = "";
                int cnt = 0;

                foreach (var user in users)
                {
                    cnt++;

                    var tempUser = await _db.Users.Where(x => x.UserName == user.IdNumber).FirstOrDefaultAsync();
                    if (tempUser != null)
                    {
                        var checkUser = await _userManager.FindByEmailAsync(tempUser.Email);
                        var _department = await _db.Departments.Where(x => x.Name == user.Department).FirstOrDefaultAsync();
                        var _employType = await _db.EmployTypes.Where(x => x.Name == user.EmployType).FirstAsync();
                        var _position = await _db.Positions.Where(x => x.Name == user.Position).FirstOrDefaultAsync();
                        var _userRole = await _db.UserRoles.Where(x => x.Name == user.UserRole).FirstAsync();

                        checkUser.FullName = user.FullName;
                        checkUser.Department = _department;
                            checkUser.EmployType = _employType;
                        checkUser.Position = _position;
                         checkUser.UserRole = _userRole;
                        checkUser.IdNumber = user.IdNumber;
                        checkUser.Email = user.Email;
                        checkUser.UserName = user.IdNumber;
                        checkUser.BirthDay = Convert.ToDateTime(user.BirthDay);
                        checkUser.InDate = Convert.ToDateTime(user.InDate);
                        checkUser.OutDate = Convert.ToDateTime(user.OutDate);
                        checkUser.PhoneNumber = user.PhoneNumber;
                        checkUser.IsApproved = user.IsApproved == 1 ? true : false;
                        //checkUser.IsOut = user.IsOut == 1 ? true : false;
                        checkUser.Memo = user.Memo;

                        await _userManager.UpdateAsync(checkUser);
                        await Save();
                        updated++;
                    }
                    else
                    {
                        var _department = await _db.Departments.Where(x => x.Name == user.Department).FirstOrDefaultAsync();
                         var _employType = await _db.EmployTypes.Where(x => x.Name == user.EmployType).FirstAsync();
                        var _position = await _db.Positions.Where(x => x.Name == user.Position).FirstOrDefaultAsync();
                          var _userRole = await _db.UserRoles.Where(x => x.Name == user.UserRole).FirstAsync();

                        var _user = new ApplicationUser
                        {
                            FullName = user.FullName,
                            Department = _department,
                             EmployType = _employType,
                            Position = _position,
                             UserRole = _userRole,
                            IdNumber = user.IdNumber,
                            Email = user.Email,
                            UserName = user.IdNumber,
                            BirthDay = Convert.ToDateTime(user.BirthDay),
                            InDate = Convert.ToDateTime(user.InDate),
                            PhoneNumber = user.PhoneNumber,
                            IsApproved = user.IsApproved == 1 ? true : false,
                            Memo = user.Memo,
                            Menu = _db.DefaultMenus.Select(x => new Menu
                            {
                                Icon = x.Icon,
                                IsAbleToAccess = x.IsAbleToAccess,
                                IsAbleToRead = x.IsAbleToRead,
                                IsAbleToReadWrite = x.IsAbleToReadWrite,
                                IsAbleToDelete = x.IsAbleToDelete,
                                Path = x.Path,
                                Order = x.Order,
                                Name = x.Name,
                                Submenu = x.Submenu.Select(y => new SubMenu
                                {
                                    IsAbleToAccess = y.IsAbleToAccess,
                                    IsAbleToRead = y.IsAbleToRead,
                                    IsAbleToReadWrite = y.IsAbleToReadWrite,
                                    IsAbleToDelete = y.IsAbleToDelete,
                                    IsFavorite = y.IsFavorite,
                                    Path = y.Path,
                                    Order = y.Order,
                                    Name = y.Name,
                                }).OrderBy(y => y.Order).ToList()
                            }).ToList()
                        };
                        var result = await _userManager.CreateAsync(_user, "Password!2#4");
                        if (result.Succeeded)
                        {
                            await GenerateJwtToken(_user);
                            created++;
                        }
                        else
                        {
                            errorMessage = cnt.ToString() + "번째 행을 확인해주세요.";
                        }
                    }
                }

                var userList = await _userManager.Users
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new UserManageResponse
                    {
                        Uuid = x.Id,
                        Department = x.Department.Name,
                        FullName = x.FullName,
                        Email = x.Email,
                        IdNumber = x.IdNumber,
                        IsApproved = x.IsApproved == true ? 1 : 0,
                        Position = x.Position.Name,
                        BirthDay = x.BirthDay.ToString("yyyy-MM-dd"),
                        InDate = x.InDate.ToString("yyyy-MM-dd"),
                        OutDate = x.OutDate.ToString("yyyy-MM-dd"),
                        IsOut = x.IsOut == true ? 1 : 0,
                        EmployType = x.EmployType.Name,
                        PhoneNumber = x.PhoneNumber,
                        UserRole = x.UserRole.Name,
                    }).ToListAsync();

                var Res = new Response<IEnumerable<UserManageResponse>>
                {
                    IsSuccess = cnt == (updated + created) ? true : false,
                    ErrorMessage = cnt == (updated + created) ? "" : errorMessage,
                    Data = userList
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<UserManageResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<UserManageResponse>>> UpdateUser(UserManageRequest user, string uuid)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(uuid);
                var _department = await _db.Departments.Where(x => x.Name == user.Department).FirstAsync();
                var _employType = await _db.EmployTypes.Where(x => x.Name == user.EmployType).FirstAsync();
                var _position = await _db.Positions.Where(x => x.Name == user.Position).FirstAsync();
                var _userRole = await _db.UserRoles.Where(x => x.Name == user.UserRole).FirstAsync();

                _user.FullName = user.FullName;
                _user.Department = _department;
                _user.EmployType = _employType;
                _user.Position = _position;
                _user.UserRole = _userRole;

                _user.IdNumber = user.IdNumber;
                _user.Email = user.Email;
                _user.UserName = user.IdNumber;
                _user.BirthDay = Convert.ToDateTime(user.BirthDay);
                _user.InDate = Convert.ToDateTime(user.InDate);
                _user.OutDate = Convert.ToDateTime(user.OutDate);
                _user.PhoneNumber = user.PhoneNumber;
                _user.IsApproved = user.IsApproved == 1 ? true : false;
                _user.IsOut = user.IsOut == 1 ? true : false;
                _user.Memo = user.Memo;

                await _userManager.UpdateAsync(_user);
                await Save();

                var userList = await _userManager.Users
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new UserManageResponse
                    {
                        Uuid = x.Id,
                        Department = x.Department.Name,
                        FullName = x.FullName,
                        Email = x.Email,
                        IdNumber = x.IdNumber,
                        IsApproved = x.IsApproved == true ? 1 : 0,
                        Position = x.Position.Name,
                        BirthDay = x.BirthDay.ToString("yyyy-MM-dd"),
                        InDate = x.InDate.ToString("yyyy-MM-dd"),
                        OutDate = x.OutDate.ToString("yyyy-MM-dd"),
                        IsOut = x.IsOut == true ? 1 : 0,
                        EmployType = x.EmployType.Name,
                        PhoneNumber = x.PhoneNumber,
                        UserRole = x.UserRole.Name,
                    }).ToListAsync();

                var Res = new Response<IEnumerable<UserManageResponse>>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = userList
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<UserManageResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        public async Task<Response<IEnumerable<UserManageResponse>>> DeleteUser(string[] uuid)
        {
            try
            {
                foreach (var i in uuid)
                {
                    var _user = await _userManager.FindByIdAsync(i);
                    _user.IsDeleted = true;
                    _user.Email = i + "@delete.user";
                    _user.UserName = i + "@delete.user";
                    _user.NormalizedEmail = i + "@delete.user";
                    _user.NormalizedUserName = i + "@delete.user";
                    _user.IdNumber = i + "@delete.user";
                    await _userManager.UpdateAsync(_user);
                    await Save();
                }

                var userList = await _userManager.Users
                       .Where(x => x.IsDeleted == false)
                       .Select(x => new UserManageResponse
                       {
                           Uuid = x.Id,
                           Department = x.Department.Name,
                           FullName = x.FullName,
                           Email = x.Email,
                           IdNumber = x.IdNumber,
                           IsApproved = x.IsApproved == true ? 1 : 0,
                           Position = x.Position.Name,
                           BirthDay = x.BirthDay.ToString("yyyy-MM-dd"),
                           InDate = x.InDate.ToString("yyyy-MM-dd"),
                           OutDate = x.OutDate.ToString("yyyy-MM-dd"),
                           IsOut = x.IsOut == true ? 1 : 0,
                           EmployType = x.EmployType.Name,
                           PhoneNumber = x.PhoneNumber,
                           UserRole = x.UserRole.Name,
                       }).ToListAsync();

                var Res = new Response<IEnumerable<UserManageResponse>>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = userList
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<UserManageResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }

        public async Task<Response<IEnumerable<UserManageResponse>>> InitializeUserPassword(string[] uuid)
        {
            try
            {
                foreach(var user in uuid)
                {
                    var _user = await _userManager.FindByIdAsync(user);
                    var token =await _userManager.GeneratePasswordResetTokenAsync(_user);
                    await _userManager.ResetPasswordAsync(_user,token,"Password!2#4");
                }
                await Save();

                var userList = await _userManager.Users
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new UserManageResponse
                    {
                        Uuid = x.Id,
                        Department = x.Department.Name,
                        FullName = x.FullName,
                        Email = x.Email,
                        IdNumber = x.IdNumber,
                        IsApproved = x.IsApproved == true ? 1 : 0,
                        Position = x.Position.Name,
                        BirthDay = x.BirthDay.ToString("yyyy-MM-dd"),
                        InDate = x.InDate.ToString("yyyy-MM-dd"),
                        OutDate = x.OutDate.ToString("yyyy-MM-dd"),
                        IsOut = x.IsOut == true ? 1 : 0,
                        EmployType = x.EmployType.Name,
                        PhoneNumber = x.PhoneNumber,
                        UserRole = x.UserRole.Name,
                    }).ToListAsync();

                var Res = new Response<IEnumerable<UserManageResponse>>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = userList
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<UserManageResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }

        public async Task<Response<UserMenuResponse>> GetUserMenus(string uuid)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(uuid);
                var Res = new Response<UserMenuResponse>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = new UserMenuResponse
                    {
                        FullName = _user.FullName,
                        Department = _user.Department.Name,
                        Position = _user.Position.Name,
                        Email = _user.Email,
                        PhoneNumber = _user.PhoneNumber,
                        Menus = _user.Menu.Select(x => new Menu
                        {
                            Id = x.Id,
                            Icon = x.Icon,
                            IsAbleToAccess = x.IsAbleToAccess,
                            IsAbleToRead = x.IsAbleToRead,
                            IsAbleToReadWrite = x.IsAbleToReadWrite,
                            IsAbleToDelete = x.IsAbleToDelete,
                            Path = x.Path,
                            Order = x.Order,
                            Name = x.Name,
                            Submenu = x.Submenu.Select(y => new SubMenu
                            {
                                Id = y.Id,
                                IsAbleToAccess = y.IsAbleToAccess,
                                IsAbleToRead = y.IsAbleToRead,
                                IsAbleToReadWrite = y.IsAbleToReadWrite,
                                IsAbleToDelete = y.IsAbleToDelete,
                                IsFavorite = y.IsFavorite,

                                Path = x.Path,
                                Order = y.Order,
                                Name = y.Name,
                            }).OrderBy(y => y.Order).ToList()
                        }).OrderBy(x => x.Order).ToList()

                    }
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<UserMenuResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };
                return Res;
            }
        }

        public async Task<Response<UserManageResponse>> UpdateUserMenusByPost(UserMenuRequest menu)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(menu.Uuid);
                _user.Menu = menu.Menus;

                await _userManager.UpdateAsync(_user);
                await Save();


                var result = await _userManager.Users
                    .Where(x => x.Id == menu.Uuid && x.IsDeleted == false)
                    .Select(x => new UserManageResponse
                    {
                        Uuid = x.Id,
                        Department = x.Department.Name,
                        FullName = x.FullName,
                        Email = x.Email,
                        IdNumber = x.IdNumber,
                        IsApproved = x.IsApproved == true ? 1 : 0,
                        Position = x.Position.Name,
                        BirthDay = x.BirthDay.ToString("yyyy-MM-dd"),
                        InDate = x.InDate.ToString("yyyy-MM-dd"),
                        OutDate = x.OutDate.ToString("yyyy-MM-dd"),
                        IsOut = x.IsOut == true ? 1 : 0,
                        EmployType = x.EmployType.Name,
                        PhoneNumber = x.PhoneNumber,
                        UserRole = x.UserRole.Name,
                        Menus = x.Menu.Select(x => new Menu
                        {
                            Id = x.Id,
                            Icon = x.Icon,
                            IsAbleToAccess = x.IsAbleToAccess,
                            IsAbleToRead = x.IsAbleToRead,
                            IsAbleToReadWrite = x.IsAbleToReadWrite,
                            IsAbleToDelete = x.IsAbleToDelete,
                            Path = x.Path,
                            Order = x.Order,
                            Name = x.Name,
                            Submenu = x.Submenu.Select(y => new SubMenu
                            {
                                Id = y.Id,
                                IsAbleToAccess = y.IsAbleToAccess,
                                IsAbleToRead = y.IsAbleToRead,
                                IsAbleToReadWrite = y.IsAbleToReadWrite,
                                IsAbleToDelete = y.IsAbleToDelete,
                                IsFavorite = y.IsFavorite,
                                Path = y.Path,
                                Order = y.Order,
                                Name = y.Name,
                            }).OrderBy(y => y.Order).ToList()
                        }).OrderBy(x => x.Order).ToList()

                    })
                .FirstAsync();







                var Res = new Response<UserManageResponse>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<UserManageResponse>
                {
                    IsSuccess = true,
                    Data = null,
                    ErrorMessage = ex.Message.ToString()
                };

                return Res;
            }
        }


        public async Task<Response<UserManageResponse>> UpdateUserMenus(UserMenuRequest menu, string uuid)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(uuid);
                _user.Menu = menu.Menus;

                await _userManager.UpdateAsync(_user);
                await Save();


                var result = await _userManager.Users
                    .Where(x => x.Id == uuid && x.IsDeleted == false)
                    .Select(x => new UserManageResponse
                    {
                        Uuid = x.Id,
                        Department = x.Department.Name,
                        FullName = x.FullName,
                        Email = x.Email,
                        IdNumber = x.IdNumber,
                        IsApproved = x.IsApproved == true ? 1 : 0,
                        Position = x.Position.Name,
                        BirthDay = x.BirthDay.ToString("yyyy-MM-dd"),
                        InDate = x.InDate.ToString("yyyy-MM-dd"),
                        OutDate = x.OutDate.ToString("yyyy-MM-dd"),
                        IsOut = x.IsOut == true ? 1 : 0,
                        EmployType = x.EmployType.Name,
                        PhoneNumber = x.PhoneNumber,
                        UserRole = x.UserRole.Name,
                        Menus = x.Menu.Select(x => new Menu
                        {
                            Id = x.Id,
                            Icon = x.Icon,
                            IsAbleToAccess = x.IsAbleToAccess,
                            IsAbleToRead = x.IsAbleToRead,
                            IsAbleToReadWrite = x.IsAbleToReadWrite,
                            IsAbleToDelete = x.IsAbleToDelete,
                            Path = x.Path,
                            Order = x.Order,
                            Name = x.Name,
                            Submenu = x.Submenu.Select(y => new SubMenu
                            {
                                Id = y.Id,
                                IsAbleToAccess = y.IsAbleToAccess,
                                IsAbleToRead = y.IsAbleToRead,
                                IsAbleToReadWrite = y.IsAbleToReadWrite,
                                IsAbleToDelete = y.IsAbleToDelete,
                                IsFavorite = y.IsFavorite,
                                Path = y.Path,
                                Order = y.Order,
                                Name = y.Name,
                            }).OrderBy(y => y.Order).ToList()
                        }).OrderBy(x => x.Order).ToList()

                    })
                .FirstAsync();

                List<SubMenu> _fav = new List<SubMenu>();
                if (result.Menus != null)
                {
                    foreach (var x in result.Menus)
                    {
                        foreach (var y in x.Submenu)
                        {
                            if (y.IsFavorite)
                            {
                                _fav.Add(y);
                            }
                        }
                    }
                }



                var result2 = await _userManager.Users
                    .Where(x => x.Id == uuid && x.IsDeleted == false)
                    .Select(x => new UserManageResponse
                    {
                        Uuid = x.Id,
                        Department = x.Department.Name,
                        FullName = x.FullName,
                        Email = x.Email,
                        IdNumber = x.IdNumber,
                        IsApproved = x.IsApproved == true ? 1 : 0,
                        Position = x.Position.Name,
                        BirthDay = x.BirthDay.ToString("yyyy-MM-dd"),
                        InDate = x.InDate.ToString("yyyy-MM-dd"),
                        OutDate = x.OutDate.ToString("yyyy-MM-dd"),
                        IsOut = x.IsOut == true ? 1 : 0,
                        EmployType = x.EmployType.Name,
                        PhoneNumber = x.PhoneNumber,
                        UserRole = x.UserRole.Name,
                        FavoriteMenu = _fav,
                        Menus = x.Menu.Where(y => y.Name != "즐겨찾기").Select(x => new Menu
                        {
                            Id = x.Id,
                            Icon = x.Icon,
                            IsAbleToAccess = x.IsAbleToAccess,
                            IsAbleToRead = x.IsAbleToRead,
                            IsAbleToReadWrite = x.IsAbleToReadWrite,
                            IsAbleToDelete = x.IsAbleToDelete,
                            Path = x.Path,
                            Order = x.Order,
                            Name = x.Name,

                            Submenu = x.Submenu.Select(y => new SubMenu
                            {
                                Id = y.Id,
                                IsAbleToAccess = y.IsAbleToAccess,
                                IsAbleToRead = y.IsAbleToRead,
                                IsAbleToReadWrite = y.IsAbleToReadWrite,
                                IsAbleToDelete = y.IsAbleToDelete,
                                IsFavorite = y.IsFavorite,
                                Path = y.Path,
                                Order = y.Order,
                                Name = y.Name,
                            }).OrderBy(y => y.Order).ToList()



                        }).OrderBy(x => x.Order).ToList()

                    })
                    .FirstAsync();


                var Res = new Response<UserManageResponse>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result2
                };

                return Res;




                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<UserManageResponse>
                {
                    IsSuccess = true,
                    Data = null,
                    ErrorMessage = ex.Message.ToString()
                };

                return Res;
            }
        }

        public async Task<Response<BusinessInfo>> GetBusinessInfo()
        {
            try
            {
                var info = await _db.BusinessInfo.FirstOrDefaultAsync();

                var Res = new Response<BusinessInfo>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = info
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<BusinessInfo>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };
                return Res;
            }
        }

        public async Task<Response<BusinessInfo>> UpdateBusinessInfo(BusinessInfo info)
        {
            try
            {
                var _info = await _db.BusinessInfo.FirstOrDefaultAsync();

                _info.IndustryType = info.IndustryType;
                _info.Introduce = info.Introduce;
                _info.Email = info.Email;
                _info.FaxNumber = info.FaxNumber;
                _info.Address = info.Address;
                _info.BusinessNumber = info.BusinessNumber;
                _info.BusinessType = info.BusinessType;
                _info.ContactNumber = info.ContactNumber;
                _info.CorporationNumber = info.CorporationNumber;
                _info.LogoUrl = info.LogoUrl;
                _info.Name = info.Name;
                _info.President = info.President;
                _info.TaxRegistrationId = info.TaxRegistrationId;
              //  _info.UploadFile = info.UploadFile;

                _db.BusinessInfo.Update(_info);
                await Save();

                var Res = new Response<BusinessInfo>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _info
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<BusinessInfo>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };
                return Res;
            }
        }







        //NOTICE

        public async Task<Response<IEnumerable<NoticeManageResponse>>> GetNoticesBySearch(NoticeManageRequest _notice)
        {
            try
            {

                var notices = await _db.Notices
                    .Where(x => x.Title.Contains(_notice.SearchStr) || x.Title.Contains(_notice.SearchStr) || x.Creator.FullName.Contains(_notice.SearchStr))
                    .Where(x => x.CreateOn >= Convert.ToDateTime(_notice.StartDate))
                    .Where(x => x.CreateOn <= Convert.ToDateTime(_notice.EndDate).AddDays(1))
                    .Select(x => new NoticeManageResponse
                    {
                        CreateOn = x.CreateOn.ToString("yyyy-MM-dd HH:mm:ss"),
                        // Content = x.Content,
                        Creator = x.Creator.FullName,
                        Id = x.Id,
                        Title = x.Title,
                        // uploadFiles = x.UploadFiles.Where(y => y.IsDeleted == false).ToArray(),
                    }).ToListAsync();

                var Res = new Response<IEnumerable<NoticeManageResponse>>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = notices
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<NoticeManageResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<NoticeManageResponse>>> GetNoticesToMain(NoticeManageRequest _notice)
        {
            try
            {

                var notices = await _db.Notices
                    .Where(x => x.Title.Contains(_notice.SearchStr) || x.Title.Contains(_notice.SearchStr) || x.Creator.FullName.Contains(_notice.SearchStr))
                    .Where(x => x.CreateOn >= Convert.ToDateTime(_notice.StartDate))
                    .Where(x => x.CreateOn <= Convert.ToDateTime(_notice.EndDate))
                    .Select(x => new NoticeManageResponse
                    {
                        CreateOn = x.CreateOn.ToString("yyyy-MM-dd HH:mm:ss"),
                        // Content = x.Content,
                        Creator = x.Creator.FullName,
                        Id = x.Id,
                        Title = x.Title,
                        // uploadFiles = x.UploadFiles.Where(y => y.IsDeleted == false).ToArray(),
                    }).Take(5).ToListAsync();

                var Res = new Response<IEnumerable<NoticeManageResponse>>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = notices
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<NoticeManageResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<NoticeManageResponse>>> GetAllNotices()
        {
            try
            {
                var notices = await _db.Notices
                    .Select(x => new NoticeManageResponse
                    {
                        CreateOn = x.CreateOn.ToString("yyyy-MM-dd HH:mm:ss"),
                        // Content = x.Content,
                        Creator = x.Creator.FullName,
                        Id = x.Id,
                        Title = x.Title,
                        // uploadFiles = x.UploadFiles.Where(y => y.IsDeleted == false).ToArray(),
                    }).ToListAsync();

                var Res = new Response<IEnumerable<NoticeManageResponse>>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = notices
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<NoticeManageResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.ToString(),
                    Data = null
                };

                return Res;
            }
        }



        public async Task<Response<NoticeManageResponse>> GetNotice(int Id)
        {
            try
            {
                var notice = await _db.Notices
                    .Where(x => x.Id == Id)
                    .Select(x => new NoticeManageResponse
                    {
                        CreateOn = x.CreateOn.ToString("yyyy-MM-dd HH:mm:ss"),
                        Content = x.Content,
                        Creator = x.Creator.FullName,
                        Id = x.Id,
                        Title = x.Title,
                        uploadFiles = x.UploadFiles.ToArray(),
                    }).FirstOrDefaultAsync();

                var Res = new Response<NoticeManageResponse>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = notice
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<NoticeManageResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<NoticeManageResponse>>> CreateNotice(NoticeManageRequest notice)
        {
            try
            {
                var _creator = await _userManager.FindByIdAsync(notice.Uuid);
                var _notice = new Notice
                {
                    Content = notice.Content,
                    CreateOn = DateTime.UtcNow.AddHours(9),
                    Title = notice.Title,
                    Creator = _creator,
                    UploadFiles = notice.uploadFiles,
                };

                await _db.Notices.AddAsync(_notice);
                await Save();

                var notices = await _db.Notices
                    .Select(x => new NoticeManageResponse
                    {
                        CreateOn = x.CreateOn.ToString("yyyy-MM-dd HH:mm:ss"),
                        Creator = x.Creator.FullName,
                        Id = x.Id,
                        Title = x.Title,
                    }).ToListAsync();

                var Res = new Response<IEnumerable<NoticeManageResponse>>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = notices
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<NoticeManageResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }


        }
        public async Task<Response<IEnumerable<NoticeManageResponse>>> UpdateNotice(NoticeManageRequest notice)
        {
            try
            {
                // var _creator = await _userManager.FindByIdAsync(notice.Uuid);
                var _notice = await _db.Notices.Include(x=>x.UploadFiles).Where(x => x.Id == notice.Id).FirstOrDefaultAsync();

                _notice.Content = notice.Content;
                _notice.Title = notice.Title;
                _notice.UploadFiles = notice.uploadFiles;

                _db.Notices.Update(_notice);
                await Save();
/*
                var _notice2 = await _db.Notices.Include(x => x.UploadFiles).Where(x => x.Id == notice.Id).FirstOrDefaultAsync();

                _notice2.UploadFiles = notice.uploadFiles.ToArray();

                _db.Notices.Update(_notice2);
*/
              //  await Save();

                var Res = new Response<IEnumerable<NoticeManageResponse>>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<NoticeManageResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<NoticeManageResponse>>> DeleteNotice(NoticeManageRequest notice)
        {
            try
            {
                foreach (var i in notice.Ids)
                {
                    var _notice = await _db.Notices.Where(x => x.Id == i).FirstOrDefaultAsync();
                    _db.Notices.Remove(_notice);
                    await Save();
                }

                var notices = await _db.Notices
                    .Select(x => new NoticeManageResponse
                    {
                        CreateOn = x.CreateOn.ToString("yyyy-MM-dd HH:mm:ss"),
                        Creator = x.Creator.FullName,
                        Id = x.Id,
                        Title = x.Title,
                    }).ToListAsync();

                var Res = new Response<IEnumerable<NoticeManageResponse>>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = notices
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<NoticeManageResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }




        private async Task GenerateJwtToken(ApplicationUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(60), // 5-10 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);


            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevorked = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(14),
                Token = RandomString(35) + Guid.NewGuid(),
                AccessToken = jwtToken,
            };

            await _db.RefreshTokens.AddAsync(refreshToken);
            await _db.SaveChangesAsync();
        }

        public async Task<Response<IEnumerable<UserLogResponse>>> GetUserLogs(UserLogRequest req)
        {
            try
            {
                var res = await _db.UserLog
                    .Where(x => x.CreateTime >= Convert.ToDateTime(req.StartDate))
                    .Where(x => x.CreateTime <= Convert.ToDateTime(req.EndDate).AddDays(1))
                    .Where(x => x.ApplicationUser.UserName.Contains(req.SearchStr)
                        || x.ApplicationUser.Department.Name.Contains(req.SearchStr)
                        || x.ApplicationUser.Position.Name.Contains(req.SearchStr)
                        || x.Location.Contains(req.SearchStr))
                    .OrderByDescending(x => x.CreateTime)
                    .Select(x => new UserLogResponse
                    {
                        Department = x.ApplicationUser.Department.Name,
                        CreateTime = x.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        Ipv4 = x.Ipv4,
                        Location = x.Location,
                        Message = x.ResultMessage,
                        Position = x.ApplicationUser.Position.Name,
                        UserName = x.UserName
                    }).ToArrayAsync();

                var Res = new Response<IEnumerable<UserLogResponse>>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<UserLogResponse>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }




    }
}
