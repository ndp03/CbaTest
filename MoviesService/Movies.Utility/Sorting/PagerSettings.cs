using System;
using System.Collections.Generic;
using Movies.Utility.Sorting;

namespace HVManager.Util.Sorting
{
    public class PagerSettings
    {
        private const int DefaultPageSize = 20;

        public PagerSettings()
        {
            PageNumber = 1;
            PageSize = DefaultPageSize;
            Paginate = true;
            SortExpressions = new List<SortExpression>();
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public bool Paginate { get; set; }

        public string SortBy { get; set; }

        public SortDirection SortDirection { get; set; }

        public List<SortExpression> SortExpressions { get; set; }

        public string TotalBy { get; set; }

        public override string ToString()
        {
            return String.Format("PageNumber = {0}, PageSize = {1}, Paginate = {2}, SortBy = \"{3}\", SortDirection = {4}, TotalBy = \"{5}\"", PageNumber, PageSize, Paginate, SortBy, SortDirection, TotalBy);
        }
    }
}