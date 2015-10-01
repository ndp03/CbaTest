using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Utility.Sorting;

namespace Movies.Entities
{
    public class MovieFilter
    {
        public MovieFilter()
        {
            PageSize = 20; // default page size, should be from .config
            SortOrder = SortDirection.None;
        }

        public int PageSize { get; set; }

        public int CurrentPageIndex { get; set; }  // index start with 0

        public string SortBy { get; set; }

        public SortDirection SortOrder { get; set; }

        public string SearchBy { get; set; }    // if empty, will attempt to search all property

        public string SearchText { get; set; }

        public bool IsCaseSensitive { get; set; }

        public override string ToString()
        {
            return string.Format("MovieFilter parameters: PageSize={0} CurrentPageNumber={1} SortBy={2} SortOrder={3} SearchText={4} IsCaseSensitive={5}",
                PageSize, CurrentPageIndex, SortBy, SortOrder, SearchText, IsCaseSensitive);
        }
    }
}
