using System;

namespace MySchoolApi
{
    public class AuthenticationSettings
    {
        public string JwtKey { get; set; }
        public int JwtExpiredDays { get; set; }
        public string JwtIssuer { get; set; }
    }
}
