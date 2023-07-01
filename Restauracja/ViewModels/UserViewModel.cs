using System.ComponentModel.DataAnnotations;

namespace Restauracja.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Mail jest niepoprawny")]
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
        [Required(ErrorMessage = "Numer telefonu jest wymagany")]
        public string PhoneNumber { get; set; }
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Hasło musi mieć conajmniej 8 znaków, oraz musi zawierać minimum jedną dużą litere, cyfre i znak specjalny")]
        [Required(ErrorMessage = "Hasło jest wymagane")]
        public string Password { get; set; }
    }
}
