using System.ComponentModel.DataAnnotations;

namespace CtrlInvest.Services.Dtos
{
    public class RegisterViewModel: LoginViewModel
    {

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
