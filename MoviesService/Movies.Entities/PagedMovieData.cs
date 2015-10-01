using System.Collections.Generic;

namespace MovieCompanyTest.PeterXiao.Entities
{
    public class PagedMovieData
    {
        public int CurrentPageNumber { get; set; }

        public int TotalPageNumber { get; set; }

        public List<MovieCompanyData> MovieCompanyDataList { get; set; }
    }
}
