using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace MVCSite.Web.ViewModels
{
    public class SignInSignUpControl
    {
        //Sign In

        public string SignInError { get; set; }

        [Display(Name = "User name")]
        public string SignInUserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string SignInPassword { get; set; }

        [Display(Name = "Remember me?")]
        public bool SignInRememberMe { get; set; }
    }
    public class SignUpControl
    {
        //SignUp

        public string SignUpError { get; set; }

        [Display(Name = "User name")]
        public string SignUpUserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string SignUpEmail { get; set; }

        [Display(Name = "Country")]
        public string SignUpCountry { get; set; }

        [Display(Name = "City")]
        public string SignUpCity { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string SignUpPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string SignUpConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
    }

}