using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ExtraSuperMegaChess2D
{
    class UserModel
    {
        [Required(ErrorMessage = "Не указано имя пользователя")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 30")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указан пароль")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина пароля должна быть от 3 до 50")]
        public string Password { get; set; }
        //[Required(ErrorMessage = "Подтвердите пароль")]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "Длина пароля должна быть от 3 до 50")]
        //[Compare("Password", ErrorMessage = "Пароли не совпадают")]
        //public string ConfirmPassword { get; set; }
    }
}
