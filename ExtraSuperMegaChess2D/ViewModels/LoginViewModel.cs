using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExtraSuperMegaChess2D
{
    class LoginViewModel
    {
        // команда добавления нового объекта
        private RelayCommand loginCommand;
        private RelayCommand registrCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ??
                  (loginCommand = new RelayCommand(obj =>
                  {
                      MessageBox.Show("Login");
                      GameWindow gameWindow = new GameWindow();
                      gameWindow.Show();
                  }));
            }
        }
        public RelayCommand RegistrCommand
        {
            get
            {
                return registrCommand ??
                  (registrCommand = new RelayCommand(obj =>
                  {
                      MessageBox.Show("Registration");

                  }));
            }
        }

    }
}
