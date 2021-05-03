using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ChessAPI.Controllers;
using ChessAPI.Models;

namespace ExtraSuperMegaChess2D
{
    class LoginViewModel
    {
        // команда добавления нового объекта
        private RelayCommand loginCommand;
        private RelayCommand registrCommand;
        public string Name { get; set; }
        public string Password { get; set; }
        public Action CloseAction { get; set; }
        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ??
                  (loginCommand = new RelayCommand(obj =>
                  {
                      var passwordBox = obj as PasswordBox;
                      Password = passwordBox.Password;
                      if (Name != null && Password != null)
                      {
                          PlayersController pc = new PlayersController();
                          List<Player> pl = pc.Index();
                          Player player = new Player();
                          foreach (Player p in pl)
                          {
                              if (p.Name == Name)
                                  player = p;
                          }
                          if (HashGenerator.VerifyHashedPassword(player.Password, Password))
                          {
                              StartWindow startWindow = new StartWindow(player.PlayerID);
                              startWindow.Show();
                              CloseAction();
                          }
                          else
                          {
                              MessageBox.Show("Введён неверный пароль");
                          }
                          
                      }
                      else
                      {
                          MessageBox.Show("Введите имя и пароль");
                      }
                      
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
                      var passwordBox = obj as PasswordBox;
                      Password = passwordBox.Password;
                      if (Name != null && Password != null)
                      {
                          PlayersController pc = new PlayersController();
                          foreach (Player pl in pc.Index())
                          {
                              if(pl.Name == Name)
                              {
                                  MessageBox.Show("Пользователь с таким именем уже существует");
                                  return;
                              }
                          }
                          
                          pc.Create(Name, HashGenerator.HashPassword(Password));
                          MessageBox.Show("Регистрация прошла успешно");
                      }
                      else
                      {
                          MessageBox.Show("Введите имя и пароль");
                      }

                  }));
            }
        }

    }
}
