//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MVCSite.DAC.Entities
{
    public partial class User
    {
        #region Primitive Properties
    
        public virtual int ID
        {
            get;
            set;
        }
    
        public virtual string FirstName
        {
            get;
            set;
        }
    
        public virtual string LastName
        {
            get;
            set;
        }
    
        public virtual string Password
        {
            get;
            set;
        }
    
        public virtual string Email
        {
            get;
            set;
        }
    
        public virtual System.DateTime EnterTime
        {
            get;
            set;
        }
    
        public virtual System.DateTime ModifyTime
        {
            get;
            set;
        }
    
        public virtual bool IsConfirmed
        {
            get;
            set;
        }
    
        public virtual bool CanChangeName
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> LastLoginTime
        {
            get;
            set;
        }
    
        public virtual Nullable<byte> ResetPasswordTime
        {
            get;
            set;
        }
    
        public virtual string SignUpIP
        {
            get;
            set;
        }
    
        public virtual string SignUpBrowser
        {
            get;
            set;
        }
    
        public virtual string ConfirmationToken
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> TokenExpireTime
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> PasswordChangeTime
        {
            get;
            set;
        }
    
        public virtual Nullable<short> PasswordFailedCount
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> PasswordFailedTime
        {
            get;
            set;
        }
    
        public virtual string ChangeToEmail
        {
            get;
            set;
        }
    
        public virtual string Mobile
        {
            get;
            set;
        }
    
        public virtual string PhoneAreaCode
        {
            get;
            set;
        }
    
        public virtual string PhoneLocalCode
        {
            get;
            set;
        }
    
        public virtual Nullable<byte> DefaultPhoneType
        {
            get;
            set;
        }
    
        public virtual string Initials
        {
            get;
            set;
        }
    
        public virtual string FullName
        {
            get;
            set;
        }
    
        public virtual string Bio
        {
            get;
            set;
        }
    
        public virtual string AvatarPath
        {
            get;
            set;
        }
    
        public virtual Nullable<bool> UseAvatar
        {
            get;
            set;
        }
    
        public virtual Nullable<byte> NotificationEmailType
        {
            get;
            set;
        }
    
        public virtual string Title
        {
            get;
            set;
        }
    
        public virtual short Role
        {
            get;
            set;
        }
    
        public virtual bool IsOnline
        {
            get;
            set;
        }
    
        public virtual string Address
        {
            get;
            set;
        }
    
        public virtual int GeoX
        {
            get;
            set;
        }
    
        public virtual int GeoY
        {
            get;
            set;
        }
    
        public virtual int LocationId
        {
            get;
            set;
        }
    
        public virtual Nullable<byte> Sex
        {
            get;
            set;
        }
    
        public virtual string VideoPath
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> Birthday
        {
            get;
            set;
        }
    
        public virtual bool IsPhoneConfirmed
        {
            get;
            set;
        }
    
        public virtual string OpenID
        {
            get;
            set;
        }
    
        public virtual Nullable<byte> OpenSite
        {
            get;
            set;
        }
    
        public virtual bool IsEmailVerified4OpenID
        {
            get;
            set;
        }
    
        public virtual double Credits
        {
            get;
            set;
        }

        #endregion

    }
}
