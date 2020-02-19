using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using MVCSite.ViewResource;
using MVCSite.DAC.Extensions;

namespace MVCSite.Web.ViewModels
{
    public class ForgotPassword : Layout
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class ResetPassword : Layout
    {
        public string Token { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }

    public class ResendConfirmation : Layout
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
