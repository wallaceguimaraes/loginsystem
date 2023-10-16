using System.ComponentModel.DataAnnotations;

namespace LoginSystem.Models.ViewModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Digite o login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Digite a senha")]
        public string Password { get; set; }
    }
}