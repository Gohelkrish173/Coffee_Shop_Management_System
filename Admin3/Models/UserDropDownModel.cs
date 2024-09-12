using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace Admin3.Models
{
    public class UserDropDownModel
    {
        public int UserID { get; set; }
        public String UserName { get; set; }
    }
}
