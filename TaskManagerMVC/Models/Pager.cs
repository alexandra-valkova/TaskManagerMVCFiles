using System;

namespace TaskManagerMVC.Models
{
    public class Pager
    {
        public int TotalItems { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }

        public string Prefix { get; set; }

        public string Action { get; set; }

        public string Controller { get; set; }

        public Pager() : this(0, 1, "", "", "", 3)
        {

        }

        public Pager(int totalItems, int? page, string prefix, string action, string controller, int pageSize = 3)
        {
            int totalPages = (int)Math.Ceiling(totalItems / (decimal)pageSize);
            int currentPage = page != null ? (int)page : 1;
            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if (startPage <= 0) // First pages
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }

            if (endPage > totalPages) // Last pages
            {
                endPage = totalPages;

                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            Prefix = prefix;
            Action = action;
            Controller = controller;
        }
    }
}