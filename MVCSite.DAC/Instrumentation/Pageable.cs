using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCSite.DAC.Instrumentation
{

    public interface IPageable : IEnumerable, IPageInfo
    {
        IEnumerable Page { get; }
    }
    public interface IPageable<T> : IEnumerable<T>, IPageable
    {
        new IEnumerable<T> Page { get; }
    }

    public interface IPageInfo
    {
        int PagesCount { get; }
        int PageNumber { get; }
        int PageMaxSize { get; }
        bool IsFirst { get; }
        bool IsLast { get; }
        int TotalCount { get; set; }
    }

    public class Pageable<T> : IPageable<T>
    {
        public IEnumerable<T> Page { get; set; }
        public int PageNumber { get; set; }
        public int PageMaxSize { get; set; }
        public int TotalCount { get; set; }

        public bool IsFirst
        {
            get { return PageNumber == 1; }
        }

        public bool IsLast
        {
            get { return PageNumber * PageMaxSize >= TotalCount; }
        }

        public int PagesCount
        {
            get { return (int)Math.Ceiling((double)TotalCount / PageMaxSize); }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Page.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerable IPageable.Page
        {
            get { return Page; }
        }
    }

    public static class PageableExtensions
    {
        public static IPageable<T> ToPageable<T>(this IOrderedQueryable<T> query, int pageNumber, int pageSize)
        {
            return new Pageable<T>
            {
                Page = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray(),
                PageNumber = pageNumber,
                PageMaxSize = pageSize,
                TotalCount = query.Count()
            };
        }
        public static IPageable<T> ToPageable<T>(this IOrderedEnumerable<T> query, int pageNumber, int pageSize)
        {
            return new Pageable<T>
            {
                Page = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray(),
                PageNumber = pageNumber,
                PageMaxSize = pageSize,
                TotalCount = query.Count()
            };
        }
        public static IPageable<T> ToPageable<T>(this IEnumerable<T> page, int pageNumber, int pageSize, int totalCount)
        {
            return new Pageable<T>
            {
                Page = page,
                PageNumber = pageNumber,
                PageMaxSize = pageSize,
                TotalCount = totalCount
            };
        }

        public static IPageable<T> ToPageable<T>(this IEnumerable<T> enumerable, IPageable pageable)
        {
            return new Pageable<T>
            {
                Page = enumerable,
                PageNumber = pageable.PageNumber,
                PageMaxSize = pageable.PageMaxSize,
                TotalCount = pageable.TotalCount
            };
        }

    }
}
