using Microsoft.AspNetCore.Mvc.Rendering;
using Restauracja.Models;

namespace Restauracja.ViewModels
{
    public class PaginationViewModel<T>
    {
        public List<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public SelectList? DropdownValues { get; set; }
        public string? SelectedValue { get; set; } 

        public List<Favourites>? Favourites { get; set; }
    }
}
