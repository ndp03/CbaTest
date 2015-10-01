using System;
using System.Collections.Generic;

namespace Movies.Utility.Sorting
{
    public class PagedList<T>
    {
        public PagedList(List<T> items, int totalItems, int pageIndex, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            PageIndex = pageIndex;
            NextPageNumber = pageIndex + 1;
            PageSize = pageSize == 0 ? 1 : pageSize;
            TotalPages = (int)Math.Ceiling((decimal)TotalItems / PageSize);
            HasMoreItems = TotalItems > ((PageIndex + 1) * PageSize);
            NextPageSize = NextPageNumber < TotalPages ? PageSize : TotalItems % PageSize;
        }

        public bool HasMoreItems { get; private set; }

        public List<T> Items { get; private set; }

        public int NextPageNumber { get; private set; }

        public int NextPageSize { get; private set; }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public int TotalItems { get; private set; }

        public int TotalPages { get; private set; }
    }
}