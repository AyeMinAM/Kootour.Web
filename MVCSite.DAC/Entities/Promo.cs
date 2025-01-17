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
    public partial class Promo
    {
        #region Primitive Properties
    
        public virtual int ID
        {
            get;
            set;
        }
    
        public virtual string PromoName
        {
            get;
            set;
        }
    
        public virtual string Code
        {
            get;
            set;
        }
    
        public virtual Nullable<int> PromoValue
        {
            get;
            set;
        }
    
        public virtual Nullable<double> PromoPercent
        {
            get;
            set;
        }
    
        public virtual Nullable<bool> OpenToUse
        {
            get;
            set;
        }
    
        public virtual Nullable<int> MinTouristsToUse
        {
            get;
            set;
        }
    
        public virtual Nullable<int> MinValueToUse
        {
            get;
            set;
        }
    
        public virtual System.DateTime BeginDate
        {
            get;
            set;
        }
    
        public virtual System.DateTime EndDate
        {
            get;
            set;
        }
    
        public virtual Nullable<int> GuideID
        {
            get;
            set;
        }
    
        public virtual Nullable<int> TourID
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

        #endregion

    }
}
