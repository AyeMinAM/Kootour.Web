using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MVCSite.DAC.Entities;
using MVCSite.Common;

namespace MVCSite.DAC.Common
{

    public static class SelectOptionsHelper
    {

        public static List<SelectListItem> CreateOptions(int start,int end, string[] textArr,bool withEmptyItem = false)
        {
            var items = new List<SelectListItem>();
            if (withEmptyItem)
            {
                items.Add(new SelectListItem
                {
                    Text = string.Empty,
                    Value = null
                });
            }
            for (var item = start; item <= end; item++)
            {
                items.Add(new SelectListItem
                {
                    Text = textArr[(int)item],
                    Value = item.ToString()
                });
            }
            return items;
        }

        public static List<SelectListItem> BoardGetVisibilityOptions(bool withEmptyItem = false)
        {
            var items = new List<SelectListItem>();
            if (withEmptyItem)
            {
                items.Add(new SelectListItem
                {
                    Text = string.Empty,
                    Value = null
                });
            }
            for (var item = VisibilityType.Members; item <= VisibilityType.Public; item++)
            {
                items.Add(new SelectListItem
                {
                    Text = EnumTranslation.VisibilityTypeCn[(int)item -1],
                    Value = ((byte)item).ToString()
                });
            }
            return items;
        }
        public static List<SelectListItem> GetDigitsOptions(int start,int end,bool withEmptyItem = false)
        {
            var items = new List<SelectListItem>();
            if (withEmptyItem)
            {
                items.Add(new SelectListItem
                {
                    Text = string.Empty,
                    Value = null
                });
            }
            for (var item = start; item <= end; item++)
            {
                items.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Value = item.ToString()
                });
            }
            return items;
        }

    }
}
