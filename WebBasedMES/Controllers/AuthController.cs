using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.JwtAuth;
using WebBasedMES.Data.Models.SystemLog;
using WebBasedMES.Services.JwtAuth;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Auth;
using WebBasedMES.ViewModels.User;

namespace WebBasedMES.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AuthController> _Logger;
        private readonly ApiDbContext _db;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParams;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AuthController> logger, ApiDbContext db, IOptionsMonitor<JwtConfig> optionMonitor, TokenValidationParameters tokenValidationParams)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _Logger = logger;
            _db = db;
            _jwtConfig = optionMonitor.CurrentValue;
            _tokenValidationParams = tokenValidationParams;
        }

        #region User Login & Check Token



        [HttpPost("/api/user/login")]
        public async Task<IActionResult> UserLogIn([FromBody] UserLoginRequest user)
        {
            try
            {
                var _user = _db.Users
                    .Include(x => x.Menu)
                    .ThenInclude(x => x.Submenu)
                    .Include(x => x.UserRole)
                    .Where(x => x.UserName == user.Email).FirstOrDefault();


                if (_user == null)
                {
                    var resp = new Response<string>
                    {
                        IsSuccess = false,
                        ErrorMessage = "존재하지 않는 사용자입니다.",
                        Data = null,
                    };
                    return BadRequest(resp);
                }

                if (!(user.CountryCode == "KR" || user.CountryCode == null))

                {
                    await _db.UserLog.AddAsync(new UserLog
                    {
                        ApplicationUser = _user,
                        CreateTime = DateTime.UtcNow.AddHours(9),
                        IsLogIn = false,
                        IsLogOut = false,
                        UserName = _user.UserName,
                        ResultMessage = "해외 접속 차단",
                        Ipv4 = user.Ipv4,
                        Location = user.Location

                    });
                    await _db.SaveChangesAsync();

                    var resp = new Response<string>
                    {
                        IsSuccess = false,
                        ErrorMessage = "대한민국 이외의 국가에서는 접속할 수 없습니다.",
                        Data = null,
                    };
                    return BadRequest(resp);
                }

                else
                {

                    if (_user.LoginFailTimeout != null)
                    {
                        if (_user.LoginFailTimeout.AddMinutes(10) >= DateTime.UtcNow.AddHours(9))
                        {
                            await _db.UserLog.AddAsync(new UserLog
                            {
                                ApplicationUser = _user,
                                CreateTime = DateTime.UtcNow.AddHours(9),
                                IsLogIn = false,
                                IsLogOut = false,
                                UserName = _user.UserName,
                                ResultMessage = "패스워드 오류 로그인 제한",
                                Ipv4 = user.Ipv4,
                                Location = user.Location
                            });
                            await _db.SaveChangesAsync();


                            var resp = new Response<string>
                            {
                                IsSuccess = false,
                                ErrorMessage = "패스워드 5회 이상 오류로 인해 10분간 로그인이 제한됩니다.(" + _user.LoginFailTimeout.AddMinutes(10).ToString("yyyy-MM-dd HH:mm:ss") + "이후)",
                                Data = null,
                            };
                            return BadRequest(resp);
                        }
                    }


                    List<SubMenu> _fav = new List<SubMenu>();

                    if (_user.Menu != null)
                    {
                        foreach (var x in _user.Menu)
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

                    var result = await _signInManager.PasswordSignInAsync(_user, user.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (!_user.IsApproved)
                        {
                            var resp = new Response<string>
                            {
                                IsSuccess = false,
                                ErrorMessage = "승인되지 않은 사용자입니다.",
                                Data = null,
                            };

                            await _db.UserLog.AddAsync(new UserLog
                            {
                                ApplicationUser = _user,
                                CreateTime = DateTime.UtcNow.AddHours(9),
                                IsLogIn = false,
                                IsLogOut = false,
                                UserName = _user.UserName,
                                ResultMessage = "미승인 사용자 로그인 시도",
                                Ipv4 = user.Ipv4,
                                Location = user.Location
                            });
                            await _db.SaveChangesAsync();

                            return BadRequest(resp);
                        }
                        else
                        {
                            _Logger.LogInformation(_user.UserName + "(" + _user.FullName + ") (이)가 로그인했습니다.");

                            await _db.UserLog.AddAsync(new UserLog
                            {
                                ApplicationUser = _user,
                                CreateTime = DateTime.UtcNow.AddHours(9),
                                IsLogIn = true,
                                IsLogOut = false,
                                UserName = _user.UserName,
                                ResultMessage = "사용자 로그인",
                                Ipv4 = user.Ipv4,
                                Location = user.Location
                            });
                            //Login Count 
                            _user.LoginFailCount = 0;
                            _user.LoginIp = user.Ipv4;
                            _db.Users.Update(_user);

                            await _db.SaveChangesAsync();

                            var jwtToken = await GenerateJwtToken(_user, _fav);

                            try
                            {
                                var _company = await _db.BusinessInfo.FirstOrDefaultAsync();
                                if (_user != null)
                                {
                                    string uri = "https://log.smart-factory.kr/apisvc/sendLogData.json?";
                                    uri += ("crtfcKey=" + _company.CrtfcKey + "&");
                                    uri += ("logDt=" + DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd HH:mm:ss.fff") + "&");
                                    uri += ("useSe=" + "접속" + "&");
                                    uri += ("conectIp=" + _user.LoginIp + "&");
                                    uri += ("sysUser=" + _user.IdNumber + "&");
                                    uri += ("dataUsgqty=0");
                                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                                    req.Method = "POST";
                                    req.ContentType = "application/x-www-form-urlencoded";
                                    req.Timeout = 3 * 1000;

                                    using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
                                    {
                                        HttpStatusCode status = res.StatusCode;

                                        var encoding = Encoding.GetEncoding(res.CharacterSet);
                                        Stream respStream = res.GetResponseStream();

                                        var reader = new StreamReader(respStream, encoding);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }



                            var resp = new Response<AuthResult>
                            {
                                IsSuccess = true,
                                ErrorMessage = "",
                                Data = jwtToken,
                            };
                            return Ok(resp);
                        }
                    }
                    else
                    {
                        _user.LoginFailCount++;
                        if (_user.LoginFailCount > 4)
                        {
                            _user.LoginFailTimeout = DateTime.UtcNow.AddHours(9);


                            await _db.UserLog.AddAsync(new UserLog
                            {
                                ApplicationUser = _user,
                                CreateTime = DateTime.UtcNow.AddHours(9),
                                IsLogIn = false,
                                IsLogOut = false,
                                UserName = _user.UserName,
                                ResultMessage = "로그인 시도 횟수 초과",
                            });
                        }
                        _db.Users.Update(_user);


                        await _db.UserLog.AddAsync(new UserLog
                        {
                            ApplicationUser = _user,
                            CreateTime = DateTime.UtcNow.AddHours(9),
                            IsLogIn = false,
                            IsLogOut = false,
                            UserName = _user.UserName,
                            ResultMessage = "로그인 실패(패스워드 에러)",
                        });

                        await _db.SaveChangesAsync();



                        var resp = new Response<string>
                        {
                            IsSuccess = false,
                            ErrorMessage = "로그인에 실패했습니다. 아이디, 패스워드를 확인해주세요." + (_user.LoginFailCount > 0 ? "(시도 횟수 : " + _user.LoginFailCount.ToString() : ")"),
                            Data = null,
                        };
                        return BadRequest(resp);
                    }
                }

            }
            catch (Exception ex)
            {
                var resp = new Response<string>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.StackTrace.ToString(),
                    Data = null,
                };
                return BadRequest(resp);
            }
        }

        [HttpPost("/api/user/logout")]
        public async Task<IActionResult> UserLogOut([FromBody] UserLogOutRequest user)
        {
            try
            {
                var _user = await _userManager.FindByEmailAsync(user.Email);
                await _signInManager.SignOutAsync();
                _Logger.LogInformation(_user.UserName + "(" + _user.FullName + ") (이)가 로그아웃했습니다.");

                await _db.UserLog.AddAsync(new UserLog
                {
                    ApplicationUser = _user,
                    CreateTime = DateTime.UtcNow.AddHours(9),
                    IsLogIn = false,
                    IsLogOut = true,
                    UserName = _user.UserName,
                    ResultMessage = "사용자 로그아웃",
                    Location = "",
                    Ipv4 = ""
                });



                var resp = new Response<string>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null,
                };
                return Ok(resp);

            }
            catch (Exception ex)
            {
                var resp = new Response<string>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.StackTrace.ToString(),
                    Data = null,
                };
                return BadRequest(resp);
            }
        }


        [HttpPost("/api/user/passwordChange")]
        public async Task<IActionResult> UserPasswordChange([FromBody] UserPasswordChangeRequest user)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(user.Uuid);
                var result = await _userManager.ChangePasswordAsync(_user, user.CurrentPassword, user.ConfirmPassword);

                var resp = new Response<string>
                {
                    IsSuccess = result.Succeeded,
                    ErrorMessage = "",
                    Data = result.Errors.Count() > 1 ? result.Errors.FirstOrDefault().Description : "",
                };
                return Ok(resp);
            }
            catch (Exception ex)
            {
                var resp = new Response<string>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = ""
                };
                return BadRequest(resp);
            }
        }


        [HttpPost("/api/user/auth")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest req)
        {

            var result = await VerifyAndGenerateToken(req);

            if (!result.IsSuccess)
            {
                var resp = new Response<string>
                {
                    IsSuccess = false,
                    ErrorMessage = result.Errors,
                    Data = null,
                };

                return BadRequest(resp);
            }
            else
            {
                var resp = new Response<AuthResult>
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result,
                };
                return Ok(resp);
            }
        }


        #endregion


        private async Task<AuthResult> GenerateJwtToken(ApplicationUser user, List<SubMenu> _fav)
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


            return new AuthResult()
            {
                IsSuccess = true,
                AccessToken = jwtToken,
                RefreshToken = refreshToken.Token,
                UserInfo = new UserInfo
                {
                    Uuid = user.Id,
                    Department = user.Department != null ? user.Department.Name : "",
                    Position = user.Position != null ? user.Position.Name : "",
                    Role = user.UserRole != null ? user.UserRole.Name : "관리자",
                    UserName = user.FullName,
                    Email = user.Email,
                    FavoriteMenu = _fav,
                    Company = _db.BusinessInfo.Select(x => x.Name).FirstOrDefault(),
                    ConectIp = user.LoginIp,
                    CrtfcKey = _db.BusinessInfo.Select(x => x.CrtfcKey).FirstOrDefault(),
                    UseModel = _db.BusinessInfo.Select(x => x.Option1).FirstOrDefault(),  //Option1 = 차종 사용 유무
                    UseMold = _db.BusinessInfo.Select(x => x.Option2).FirstOrDefault(),  //Option2 = 금형미사용 모델
                    UseTemp = _db.BusinessInfo.Select(x => x.Option3).FirstOrDefault(),  //Option3 = 온도 모델

                    Menus = user.Menu.Select(x => new AuthMainMenu
                    {
                        Id = x.Path,
                        Title = x.Name,
                        IsAbleToAccess = x.IsAbleToAccess,
                        IsAbleToRead = x.IsAbleToRead,
                        IsAbleToReadWrite = x.IsAbleToReadWrite,
                        IsAbleToDelete = x.IsAbleToDelete,
                        Order = x.Order,
                        SubMenu = x.Submenu.Select(y => new AuthSubMenu
                        {
                            Id = y.Path,
                            Title = y.Name,
                            Url = y.Path,
                            IsAbleToAccess = user.UserRole != null ? (user.UserRole.Name == "관리자" ? true : y.IsAbleToAccess) : y.IsAbleToAccess,
                            IsAbleToRead = user.UserRole != null ? (user.UserRole.Name == "관리자" ? true : y.IsAbleToRead) : y.IsAbleToRead,
                            IsAbleToReadWrite = user.UserRole != null ? (user.UserRole.Name == "관리자" ? true : y.IsAbleToReadWrite) : y.IsAbleToReadWrite,
                            IsAbleToDelete = user.UserRole != null ? (user.UserRole.Name == "관리자" ? true : y.IsAbleToDelete) : y.IsAbleToDelete,
                            Order = y.Order
                        }).OrderBy(a => a.Order).ToList()
                    }).OrderBy(a => a.Order).ToList(),


                    UserId = user.IdNumber,
                }

            };
        }

        private async Task<AuthResult> VerifyAndGenerateToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Validation 1 - Validation JWT token format
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.AccessToken, _tokenValidationParams, out var validatedToken);

                // Validation 2 - Validate encryption alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }

                // Validation 3 - validate expiry date
                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expiryDate < DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        IsSuccess = false,
                        Errors = "토큰 유효기간이 만료됬습니다."

                    };
                }

                // validation 4 - validate existence of the token
                var storedToken = _db.RefreshTokens.FirstOrDefault(x => x.AccessToken == tokenRequest.AccessToken);

                if (storedToken == null)
                {
                    return new AuthResult()
                    {
                        IsSuccess = false,
                        Errors =
                            "토큰이 존재하지 않습니다."
                    };
                }

                // Validation 5 - validate if used
                /*
                if (storedToken.IsUsed)
                {
                    return new AuthResult()
                    {
                        IsSuccess = false,
                        Errors =
                            "토큰이 이미 사용중입니다.",

                    };
                }
                */
                // Validation 6 - validate if revoked
                if (storedToken.IsRevorked)
                {
                    return new AuthResult()
                    {
                        IsSuccess = false,
                        Errors =
                            "Token has been revoked"
                    };
                }

                // Validation 7 - validate the id
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        IsSuccess = false,
                        Errors =
                            "토큰이 일치하지 않습니다. "
                    };
                }

                // update current token 

                storedToken.IsUsed = true;
                _db.RefreshTokens.Update(storedToken);
                await _db.SaveChangesAsync();

                // Generate a new token
                //var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);

                var _user = _db.Users.Include(x => x.UserRole).Include(x => x.Menu).ThenInclude(x => x.Submenu).Where(x => x.Id == storedToken.UserId).FirstOrDefault();

                List<SubMenu> _fav = new List<SubMenu>();
                if (_user.Menu != null)
                {
                    foreach (var x in _user.Menu)
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

                return await GenerateJwtToken(_user, _fav);

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
                {
                    return new AuthResult()
                    {
                        IsSuccess = false,
                        Errors =
                            "토큰이 만료됬습니다. 다시 로그인해주세요"
                    };
                }
                else
                {
                    return new AuthResult()
                    {
                        IsSuccess = false,
                        Errors =
                            "Something went wrong."
                    };
                }
            }
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
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
