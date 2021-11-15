using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pfm.Models
{
    public class TransactionPagedList<T>
    {
        public int TotalCount { get; set; }
        [Range(1,100)]
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public SortOrder SortOrder { get; set; }
        public string SortBy { get; set; }
        public List<T> Items { get; set; }
    }
}