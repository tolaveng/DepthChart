using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database
{
    public class DatabaseSetting
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string DatabaseName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }

    public static class DatabaseConnection
    {
        public static string GetConnectionString(DatabaseSetting setting)
        {
            var port = !String.IsNullOrWhiteSpace(setting.Port)
                ? $",{setting.Port}" : "";
            return $"Server={setting.Host}{port};Database={setting.DatabaseName};" +
                $"User Id={setting.User};Password={setting.Password};";
        }
    }
}
