using System.ComponentModel.DataAnnotations;

namespace Restauracja.Models
{
    public class User
    {
        public int UserId { get; set; }
        
        public string Email { get; set; }
        [Required(ErrorMessage = "Imie jest wymagane")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Miasto jest wymagane")]
        public string City { get; set; }
        [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
        [RegularExpression("\\d{2}-\\d{3}", ErrorMessage = "Kod pocztowy jest niepoprawny")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Adres jest wymagany")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Numer telefonu jest wymagane")]
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; } = false;
        public string ActivationCode { get; set; }

        public int RoleId { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<Cart>? Carts { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
