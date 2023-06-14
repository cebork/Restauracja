using Microsoft.AspNetCore.Mvc.Rendering;
using Restauracja.Models;

namespace Restauracja.ViewModels
{
    public class CategoryViewModel
    {
        public long CategoryID { get; set; }
        public string CategoryName { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }
}
