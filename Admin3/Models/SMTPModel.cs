using Microsoft.AspNetCore.Mvc;

namespace Admin3.Models
{
    public class SMTPModel
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string From { get; set; }
    }

}
