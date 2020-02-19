
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.ViewResource;
using System.Web.Mvc;
namespace MVCSite.Web.ViewModels
{
    public class TourInclusionExclusionModel
    {
        public TourInclusionExclusionModel() { }
        public TourInclusionExclusionModel(TourInclusion tourInclusion)
        {
            ID = tourInclusion.ID;
            TourID = tourInclusion.TourID;
            Name = tourInclusion.Name;
            SortNo = tourInclusion.SortNo;
            EnterTime = tourInclusion.EnterTime;
            ModifyTime = tourInclusion.ModifyTime;
        }

        public TourInclusionExclusionModel(TourExclusion tourExclusion)
        {
            ID = tourExclusion.ID;
            TourID = tourExclusion.TourID;
            Name = tourExclusion.Name;
            SortNo = tourExclusion.SortNo;
            EnterTime = tourExclusion.EnterTime;
            ModifyTime = tourExclusion.ModifyTime;
        }
        public int ID { get; set; }
        public int TourID { get; set; }
        [StringLength(300, MinimumLength = 1, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]   
        public string Name { get; set; }
        public byte SortNo { get; set; }
        public DateTime EnterTime { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}
                          
                          