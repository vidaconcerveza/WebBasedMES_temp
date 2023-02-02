using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.Services
{
    public class IdentityErrorDescriberkr:IdentityErrorDescriber
    {
        public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = $"알 수 없는 오류가 발생했습니다." }; }
        public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = "동시성 문제가 발생했습니다. 데이터가 다른 곳에서 수정된 것 같습니다." }; }
        public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = "잘못된 비밀번호입니다." }; }
        public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = "유효하지 않은 토큰입니다." }; }
        public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "로그인하려는 이 사용자는 이미 다른 계정과 연결되어있습니다." }; }
        public override IdentityError InvalidUserName(string userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = $"사용자명 '{userName}'는 유효하지 않습니다. 허용되지 않은 문자가 포함되어 있습니다." }; }
        public override IdentityError InvalidEmail(string email) { return new IdentityError { Code = nameof(InvalidEmail), Description = $"이메일 '{email}'는 유효하지 않습니다." }; }
        public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = $"사용자명 '{userName}'는 이미 사용 중입니다." }; }
        public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = $"이메일 '{email}'는 이미 사용 중 입니다." }; }
        public override IdentityError InvalidRoleName(string role) { return new IdentityError { Code = nameof(InvalidRoleName), Description = $"역할 '{role}'는 유효하지 않습니다." }; }
        public override IdentityError DuplicateRoleName(string role) { return new IdentityError { Code = nameof(DuplicateRoleName), Description = $"역할 '{role}'는 이미 사용 중 입니다." }; }
        public override IdentityError UserAlreadyHasPassword() { return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = "사용자가 이미 비밀번호를 설정했습니다." }; }
        public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "이 사용자는 Lockout이 설정되지 않았습니다." }; }
        public override IdentityError UserAlreadyInRole(string role) { return new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"역할 '{role}'에 존재하는 사용자입니다." }; }
        public override IdentityError UserNotInRole(string role) { return new IdentityError { Code = nameof(UserNotInRole), Description = $"역할 '{role}'에 포함되지 않은 사용자입니다." }; }
        public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = $"비밀번호는 최소 {length}자 이상이어야 합니다." }; }
        public override IdentityError PasswordRequiresNonAlphanumeric() { return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "비밀번호에는 영문자, 숫자가 아닌 문자가 하나 이상 있어야 합니다." }; }
        public override IdentityError PasswordRequiresDigit() { return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "비밀번호에는 적어도 하나의 숫자가 있어야합니다." }; }
        public override IdentityError PasswordRequiresLower() { return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "비밀번호에는 영어 소문자가 하나 이상 있어야합니다." }; }
        public override IdentityError PasswordRequiresUpper() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "비밀번호에는 영어 대문자가 하나 이상 있어야합니다." }; }
        public override IdentityError RecoveryCodeRedemptionFailed() { return new IdentityError { Code = nameof(RecoveryCodeRedemptionFailed), Description = "복구 코드 사용에 실패했습니다." }; }
        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) { return new IdentityError { Code = nameof(PasswordRequiresUniqueChars), Description = "비밀번호는 최소한 {0}개의 다른 문자를 사용해야합니다." }; }
    }
}
