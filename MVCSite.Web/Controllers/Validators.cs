using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Interfaces;
using System.Web.Mvc;
using MVCSite.ViewResource;
using MVCSite.DAC.Common;
using MVCSite.Common;

namespace MVCSite.Web.Controllers
{
    public class Validators
    {


        public static void ValidRegisterEmail(IRepositoryUsers _repositoryUsers, ModelStateDictionary modelState,string email, string key = "Email", int userId = 0 )
        {
            if (ValidEmail(_repositoryUsers,modelState,email, key))
            {
                var oldUser = _repositoryUsers.GetByEmailOrNull(email);
                if (oldUser != null)
                {
                    if (userId == 0 || oldUser.ID != userId)
                        modelState.AddModelError(key, ValidationStrings.EmailAlreadyUsed);
                }
            }
            return;
        }
        public static void ValidRegisterUserName(IRepositoryUsers _repositoryUsers, ModelStateDictionary modelState, ISiteConfiguration _configuration, string userName, string key = "UserName", int userId = 0)
        {
            if (ValidUserName(_repositoryUsers, modelState, _configuration,userName, key))
            {
                var oldUser = _repositoryUsers.GetByEmailOrNull(userName);
                if (oldUser != null)
                {
                    if (userId == 0 || oldUser.ID != userId)
                        modelState.AddModelError(key, ValidationStrings.UserNameAlreadyUsed);
                }
            }
            return;
        }
        public static bool ValidPhoneNumber(IRepositoryUsers _repositoryUsers, ModelStateDictionary modelState, string phone, string key)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                modelState.AddModelError(key, ValidationStrings.InvalidPhoneNumber);
                return false;
            }
            if (!phone.IsPhoneNumber())
            {
                modelState.AddModelError(key, ValidationStrings.InvalidPhoneNumber);
                return false;
            }
            return true;
        }
        public static bool ValidEmail(IRepositoryUsers _repositoryUsers, ModelStateDictionary modelState, string email, string key)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                modelState.AddModelError(key, ValidationStrings.InvalidEmailAddress);
                return false;
            }
            if (!EmailHelper.IsEmailAddressValid(email))
            {
                modelState.AddModelError(key, ValidationStrings.InvalidEmailAddress);
                return false;
            }
            return true;
        }
        public static bool ValidEmailAndCheckDB(IRepositoryUsers _repositoryUsers, ModelStateDictionary modelState, string email, string key)
        {
            if (!ValidEmail(_repositoryUsers,modelState,email, key))
            {
                return false;
            }
            var oldUser = _repositoryUsers.GetByEmailOrNull(email);
            if (oldUser != null)
            {
                modelState.AddModelError(key, ValidationStrings.EmailAlreadyUsed);
                return false;
            }
            return true;
        }
        public static bool ValidUserName(IRepositoryUsers _repositoryUsers, ModelStateDictionary modelState, ISiteConfiguration _configuration, string userName, string key = "UserName")
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                modelState.AddModelError(key, ValidationStrings.InvalidUserNameLength);
                return false;
            }
            else if (userName.Length < 4)
            {
                modelState.AddModelError(key, ValidationStrings.InvalidUserNameLength);
                return false;
            }
            else if (!_configuration.UserName.ValidationRegex.IsMatch(userName))
            {
                modelState.AddModelError(key, _configuration.UserName.ValidationMessage);
                return false;
            }
            return true;
        }
        public static void ValidRegisterPassword( ModelStateDictionary modelState, ISiteConfiguration _configuration, string password, string key = "Password")
        {
            if (string.IsNullOrWhiteSpace(password))
                modelState.AddModelError(key, ValidationStrings.InvalidPasswordFormat);
            else if (!_configuration.Password.ValidationRegex.IsMatch(password))
                modelState.AddModelError(key, ValidationStrings.InvalidPasswordFormat);
            return;
        }
        public static void ValidRegisterNickName( ModelStateDictionary modelState, ISiteConfiguration _configuration, string nickName, string key = "NickName")
        {
            if (string.IsNullOrWhiteSpace(nickName))
                modelState.AddModelError(key, ValidationStrings.InvalidNickNameFormat);
            else if (!_configuration.NickName.ValidationRegex.IsMatch(nickName))
                modelState.AddModelError(key, ValidationStrings.InvalidNickNameFormat);
            return;
        }
        public static void ValidRegisterEmailUserNamePassword(IRepositoryUsers _repositoryUsers, ModelStateDictionary modelState,  ISiteConfiguration _configuration,string email, string userName, string password, string nickName, bool validateEmail)
        {
            if (validateEmail)
                ValidRegisterEmail(_repositoryUsers,modelState,email);
            else
                ValidRegisterUserName(_repositoryUsers,modelState, _configuration,userName);
            ValidRegisterPassword(modelState, _configuration,password);
            //ValidRegisterNickName(nickName);
            return;
        }
    }
}