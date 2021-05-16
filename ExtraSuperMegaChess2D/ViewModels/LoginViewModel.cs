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
using ChessClient;
namespace ExtraSuperMegaChess2D
{
    class LoginViewModel
    {
        // команда добавления нового объекта
        private RelayCommand loginCommand;
        private RelayCommand registrCommand;
        public string Name { get; set; }
        public string Password { get; set; }
        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ??
                  (loginCommand = new RelayCommand(async obj =>
                  {

                      var passwordBox = obj as PasswordBox;
                      
                      Password = passwordBox.Password;
                      if (Name != null && Password != null)
                      {
                          Client client = new Client("http://localhost:56213/Players");

                          List<PlayerInfo> players = await client.GetAllPlayers();

                          PlayerInfo player = new PlayerInfo();
                          foreach (PlayerInfo p in players)
                          {
                              if (p.Name == Name)
                                  player = p;
                          }
                          if (HashGenerator.VerifyHashedPassword(player.Password, Password))
                          {
                              StartWindow startWindow = new StartWindow(player);
                              startWindow.Show();
                              CloseWindow(Window.GetWindow(passwordBox));
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
                  (registrCommand = new RelayCommand(async obj =>
                  {
                      var passwordBox = obj as PasswordBox;
                      Password = passwordBox.Password;
                      if (Name != null && Password != null)
                      {
                          Client client = new Client("http://localhost:56213/Players");

                          List<PlayerInfo> players = await client.GetAllPlayers();
                          foreach (PlayerInfo pl in players)
                          {
                              if (pl.Name == Name)
                              {
                                  MessageBox.Show("Пользователь с таким именем уже существует");
                                  return;
                              }
                          }
                          client = new Client("http://localhost:56213/Players/Create/");
                          await client.MakeNewPlayer(Name, HashGenerator.HashPassword(Password));
                          MessageBox.Show("Регистрация прошла успешно");
                      }
                      else
                      {
                          MessageBox.Show("Введите имя и пароль");
                      }

                  }));
            }
        }
        private void CloseWindow(Window win)
        {
            if (win == null)
                return;

            win.Close();
        }

    }
}
