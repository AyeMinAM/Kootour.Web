using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MVCSite.DAC.Extensions
{
    public class GroupedSelectListItem : SelectListItem
    {
        public string GroupKey { get; set; }
        public string GroupName { get; set; }
    }

    public static class EnumerableExtensions
    {
        public static string PrintOut<T>(this IEnumerable<T> enumerable, Func<T, string> text)
        {
            var sb = new StringBuilder();
            foreach (var item in enumerable)
            {
                sb.Append(text(item));
            }
            return sb.ToString();
        }
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null)
                throw new ArgumentNullException("enumerable");

            if (action == null)
                throw new ArgumentNullException("action");

            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        public static List<SelectListItem> ToSelectList<T>(
            this IEnumerable<T> enumerable,
            Func<T, string> text,
            Func<T, string> value,
            string defaultOption)
        {
            var items = enumerable.Select(f => new SelectListItem()
            {
                Text = text(f),
                Value = value(f)
            }).ToList();
            items.Insert(0, new SelectListItem()
            {
                Text = defaultOption,
                Value = null
            });
            return items;
        }
        public static List<SelectListItem> ToSelectList<T>(
           this IEnumerable<T> enumerable,
           Func<T, string> text,
           Func<T, string> value)
        {
            var items = enumerable.Select(f => new SelectListItem()
            {
                Text = text(f),
                Value = value(f)
            }).ToList();
            return items;
        }

        public static List<GroupedSelectListItem> ToGroupedSelectList<T>(
           this IEnumerable<T> enumerable,
           Func<T, string> groupKey,
           Func<T, string> groupName,
           Func<T, string> text,
           Func<T, string> value)
        {
            var items = enumerable.Select(f => new GroupedSelectListItem()
            {
                Text = text(f),
                Value = value(f),
                GroupKey = groupKey(f),
                GroupName = groupName(f)
            }).ToList();
            return items;
        }
    }
}
