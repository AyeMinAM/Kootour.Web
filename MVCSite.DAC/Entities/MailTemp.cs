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
    public partial class MailTemp
    {
        #region Primitive Properties
    
        public virtual int RID
        {
            get;
            set;
        }
    
        public virtual string From
        {
            get;
            set;
        }
    
        public virtual string Tos
        {
            get;
            set;
        }
    
        public virtual string MailSubjects
        {
            get;
            set;
        }
    
        public virtual string MailContents
        {
            get;
            set;
        }
    
        public virtual string MailTypes
        {
            get;
            set;
        }
    
        public virtual string MailFormats
        {
            get;
            set;
        }
    
        public virtual Nullable<int> MailPriority
        {
            get;
            set;
        }
    
        public virtual Nullable<int> MailsStructure
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> EnterTime
        {
            get;
            set;
        }
    
        public virtual Nullable<int> EnterUser
        {
            get;
            set;
        }

        #endregion

    }
}
