using System;
using System.Collections.Generic;

namespace MyMvc5App
{
    public class FilterSortPageInfo
    {
        public string Filters { get; set; }     // array of Filter objects in JSON format

        public bool Search { get; set; }        // instead of filtering by columns, a general search can be an option
        public string SearchValue { get; set; } 

        public string Sort { get; set; }
        public string SortDir { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }

        public FilterSortPageInfo()
        {
            Filters = "[]";                     // empty array in JSON format
            SortDir = "ASC";
            Page = 1;
            PageSize = 10;
            Search = true;
            SearchValue = "";
        }

        // this function uses a delegate to reset it's settings
        public void ResetSettings(Action<FilterSortPageInfo> resetSettings)
        {
            resetSettings(this);
        }
    }

    public class Filter
    {
        public string Field { get; set; }
        public string Value { get; set; }
    }

    public class FilteredSortedPagedList<T>
    {
        public int TotalRecords { get; set; }
        public IEnumerable<T> List { get; set; }
    }
}
