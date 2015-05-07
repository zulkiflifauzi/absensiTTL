using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AbsensiTTL.Models
{
    public class ViewModelChangePassword
    {
        [Required]
        [DisplayName("New Password")]
        public string Password { get; set; }

        [Required]
        [DisplayName("Re-type New Password")]
        public string RetypePassword { get; set; }
    }
}