using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.ViewModels;
using Restauracja.Models;
namespace Restauracja.Services
{
    public interface IDishService
    {
        Task<CreateDishViewModel> FillCreateDishViewModelForSaveAsync(CreateDishViewModel createDishViewModel);
        Task<PaginationViewModel<Dish>> FillPaginationViewModelAsync(int page, string searchString, long minPrice, long maxPrice, string categoryName);
        Task<Dish> GetDishByIdAsync(long? id);
        Task<CreateDishViewModel> FillCreateDishViewModelForDisplayAsync();
        Task<CreateDishViewModel> FillCreateDishViewModelForDisplayEditFormAsync(long? id, Dish dish);
        Task<CreateDishViewModel> FillCreateDishViewModelForEditAsync(CreateDishViewModel createDishViewModel);
        void AddToFavourites(int id);
        void RemoveFromFavourites(int id);
    }
    public class DishService : IDishService
    {
        private readonly RestauracjaContext _context;
        private readonly IUserService _userService;
        public DishService(RestauracjaContext restauracjaContext, IUserService userService) 
        {
            _context = restauracjaContext;
            _userService = userService;
        }

        

        public async Task<CreateDishViewModel> FillCreateDishViewModelForDisplayAsync()
        {
            CreateDishViewModel createDishViewModel = new CreateDishViewModel();
            createDishViewModel.CategoryViewModel = new CategoryViewModel
            {
                Categories = await _context.Category.Select(cast => new SelectListItem
                {
                    Value = cast.CategoryID.ToString(),
                    Text = cast.Name
                }).ToListAsync()
            };
            createDishViewModel.IngredientViewModel = _context.Ingredient.Select(i => new IngredientViewModel
            {
                IngredientID = i.IngredientID,
                Name = i.Name,
                IsSelected = false
            });
            return createDishViewModel;
        }

        public async Task<CreateDishViewModel> FillCreateDishViewModelForDisplayEditFormAsync(long? id, Dish dish)
        {
            CreateDishViewModel createDishViewModel = new CreateDishViewModel();
            createDishViewModel.Dish = dish;
            createDishViewModel.CategoryViewModel = new CategoryViewModel
            {
                Categories = await _context.Category.Select(cast => new SelectListItem
                {
                    Value = cast.CategoryID.ToString(),
                    Text = cast.Name
                }).ToListAsync()
            };
            createDishViewModel.IngredientViewModel = _context.Ingredient.Select(i => new IngredientViewModel
            {
                IngredientID = i.IngredientID,
                Name = i.Name,
                IsSelected = dish.Ingredients.Contains(i) ? true : false
            });
            return createDishViewModel;
        }

        public async Task<CreateDishViewModel> FillCreateDishViewModelForEditAsync(CreateDishViewModel createDishViewModel)
        {
            //createDishViewModel.CategoryViewModel = new CategoryViewModel
            //{
            //    Categories = await _context.Category.Select(cast => new SelectListItem
            //    {
            //        Value = cast.CategoryID.ToString(),
            //        Text = cast.Name
            //    }).ToListAsync()
            //};
            //createDishViewModel.IngredientViewModel = _context.Ingredient.Select(i => new IngredientViewModel
            //{
            //    IngredientID = i.IngredientID,
            //    Name = i.Name,
            //    IsSelected = false
            //});


            List<DishIngredient> dishIngredients = _context.DishIngredient.ToList();

            dishIngredients = dishIngredients.Where(di => di.DishID == createDishViewModel.Dish.DishID).ToList();
            foreach (var item in dishIngredients)
            {
                _context.DishIngredient.Remove(item);

            }
            _context.SaveChanges();



            if (createDishViewModel.SelectedIngredients != null)
            {
                var ingredients = _context.Ingredient
                    .Where(i => createDishViewModel.SelectedIngredients.Contains(i.IngredientID))
                    .ToListAsync();
                createDishViewModel.Dish.Ingredients = ingredients.Result;
            }
            

            if (createDishViewModel.Category != null)
            {
                createDishViewModel.Dish.Category = _context.Category.ToListAsync().Result.Where(el => el.CategoryID == createDishViewModel.Category).ToList()[0];
            }

            if (createDishViewModel.ImageFile != null && createDishViewModel.ImageFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await createDishViewModel.ImageFile.CopyToAsync(memoryStream);
                byte[] data = memoryStream.ToArray();
                createDishViewModel.Dish.Image = Convert.ToBase64String(data);
            }
            else
            {
                Dish tempDish = GetDishByIdAsync(createDishViewModel.Dish.DishID).Result;

                if (tempDish.Image != null)
                {
                    createDishViewModel.Dish.Image = tempDish.Image;
                }
            }
            return createDishViewModel;
        }

        public async Task<CreateDishViewModel> FillCreateDishViewModelForSaveAsync(CreateDishViewModel createDishViewModel)
        {
            createDishViewModel.CategoryViewModel = new CategoryViewModel
            {
                Categories = await _context.Category.Select(cast => new SelectListItem
                {
                    Value = cast.CategoryID.ToString(),
                    Text = cast.Name
                }).ToListAsync()
            };
            createDishViewModel.IngredientViewModel = _context.Ingredient.Select(i => new IngredientViewModel
            {
                IngredientID = i.IngredientID,
                Name = i.Name,
                IsSelected = false
            });

            if (createDishViewModel.SelectedIngredients != null)
            {
                var ingredients = _context.Ingredient
                    .Where(i => createDishViewModel.SelectedIngredients.Contains(i.IngredientID))
                    .ToListAsync();
                createDishViewModel.Dish.Ingredients = ingredients.Result;
            }
            if (createDishViewModel.Category != null)
            {
                createDishViewModel.Dish.Category = _context.Category.ToListAsync().Result.Where(el => el.CategoryID == createDishViewModel.Category).ToList()[0];
            }
            if (createDishViewModel.ImageFile != null && createDishViewModel.ImageFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await createDishViewModel.ImageFile.CopyToAsync(memoryStream);
                byte[] data = memoryStream.ToArray();
                createDishViewModel.Dish.Image = Convert.ToBase64String(data);
            }
            else
            {
                createDishViewModel.Dish.Image = null;
            }
            return createDishViewModel;
        }

        public async Task<PaginationViewModel<Dish>> FillPaginationViewModelAsync(int page, string searchString, long minPrice, long maxPrice, string categoryName)
        {
            int userID = 0;
            if (_userService.checkIfSessionIsSet())
            {
                userID = _userService.GetUserId();
            }
            
            List<Dish> dishes = await _context.Dish
                .Include(d => d.Category)
                .ToListAsync();
            int pageSize = 5;
            int totalItems = dishes.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }
            List<Favourites> favourites = _context.Favorites.ToList();
            List<Dish> pagedDishes = new List<Dish>();
            if (favourites != null)
            {
                favourites = favourites.Where(f => f.UserId == userID).ToList();
                pagedDishes.AddRange(dishes.Where(d => favourites.Any(f => f.DishID == d.DishID)));
                pagedDishes.AddRange(dishes.Where(d => !favourites.Any(f => f.DishID == d.DishID)));
            }
            else
            {
                pagedDishes = dishes;
            }
            if (searchString != null)
            {
                pagedDishes = pagedDishes.Where(d => d.Name.Contains(searchString))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            }
            else
            {
                pagedDishes = pagedDishes
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            }
            if (minPrice != 0)
            {
                pagedDishes = pagedDishes.Where(d => d.Price >= minPrice).ToList();
            }
            if (maxPrice != long.MaxValue)
            {
                pagedDishes = pagedDishes.Where(d => d.Price <= maxPrice).ToList();
            }

            if (categoryName != null)
            {
                pagedDishes = pagedDishes.Where(d => d.Category.Name == categoryName).ToList();
            }

            
            

            var viewModel = new PaginationViewModel<Dish>
            {
                Items = pagedDishes,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                DropdownValues = new SelectList(_context.Dish.Include(d => d.Category).ToList().Select(d => d.Category.Name ).ToList().Distinct().ToList()),
                SelectedValue = categoryName,
                Favourites = favourites
            };
            return viewModel;
        }

        public async Task<Dish> GetDishByIdAsync(long? id)
        {
            return  await _context.Dish
                .Include(d => d.Category)
                .Include(i => i.Ingredients)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DishID == id);
        }
        
        public void AddToFavourites(int id)
        {
            int userID = _userService.GetUserId();

            Favourites favourites = new Favourites()
            {
                UserId = userID,
                DishID = id
            };

            _context.Favorites.Add(favourites);
            _context.SaveChanges();
        }

        public void RemoveFromFavourites(int id)
        {
            int userID = _userService.GetUserId();

            List<Favourites> favourites = _context.Favorites.ToList();
            if (favourites != null)
            {
                Favourites favouriteToDelete = favourites.Where(f => f.DishID == id && f.UserId == userID).First();

                _context.Favorites.Remove(favouriteToDelete);
                _context.SaveChanges();
            }
            
        }
    }
}
