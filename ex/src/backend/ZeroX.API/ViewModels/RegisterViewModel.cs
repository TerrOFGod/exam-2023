using System.ComponentModel.DataAnnotations;

namespace ZeroX.API.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Адрес почты обязателен")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Text)]
    [MinLength(6, ErrorMessage = "Минимальная длина имени пользователя - 6 символов")]
    [MaxLength(20, ErrorMessage = "Максимальная длина имени пользователя - 20 символов")]
    public string? Username { get; set; }
    
    [Required(ErrorMessage = "Пароль обязателен")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^.*(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!#$%&?]).*$", 
        ErrorMessage = "At least 1 upper, 1 lower, 1 digit, 1 special.")]
    [MinLength(6, ErrorMessage = "Минимальная длина пароля - 6 символов")]
    public string? Password { get; set; }
    
    [Required(ErrorMessage = "Подтвердите пароль")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^.*(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!#$%&?]).*$", 
        ErrorMessage = "At least 1 upper, 1 lower, 1 digit, 1 special.")]
    [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
    public string? Confirm { get; set; } 
}