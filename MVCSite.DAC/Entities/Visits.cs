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
    public partial class Visits
    {
        #region Primitive Properties
    
        public virtual decimal ID
        {
            get;
            set;
        }
    
        public virtual string Session_id
        {
            get;
            set;
        }
    
        public virtual string IP
        {
            get;
            set;
        }
    
        public virtual string browser
        {
            get;
            set;
        }
    
        public virtual string referer
        {
            get;
            set;
        }
    
        public virtual System.DateTime visitdate
        {
            get;
            set;
        }
    
        public virtual string path
        {
            get;
            set;
        }
    
        public virtual string Pagevar
        {
            get;
            set;
        }
    
        public virtual Nullable<int> user_id
        {
            get;
            set;
        }
    
        public virtual Nullable<int> ipcountry_id
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> firstvisit
        {
            get;
            set;
        }
    
        public virtual string keyword
        {
            get;
            set;
        }

        #endregion

    }
}
