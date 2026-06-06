using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Constants.User_error_handling
{
    public static class MessageUsermanagemen
    {
        public const string UserNotFound = "کاربر یافت نشد.";
        public const string DuplicateEmail = "ایمیل قبلاً ثبت شده است.";
        public const string WeakPassword = "رمز عبور ضعیف است.";
        public const string RegistrationFailed = "ثبت‌نام با شکست مواجه شد.";
        public const string RoleNotFound = "نقش مورد نظر وجود ندارد.";

        public static string InvalidRole { get; internal set; } = "نقش نامعتبر است.";
        public static string InvalidUser { get; internal set; }="کاربر نامعتبر است.";
    }
}
