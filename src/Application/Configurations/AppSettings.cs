using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configurations
{
    public class AppSettings
    {
        public string? SecretKey { get; set; }
        public string? ValidAudience { get; set; }
        public string? ValidIssuer { get; set; }
        public string? TokenValidityInMinutes { get; set; }
        public string? RefreshTokenValidityInMinutes { get; set; }
    }
}