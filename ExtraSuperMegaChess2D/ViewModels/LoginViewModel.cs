using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    class LoginViewModel : NotifyPropertyChanged
    {
        // команда добавления нового объекта
        private RelayCommand loginCommand;
        private RelayCommand registrCommand;
        public UserModel User { get; set; }
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }
        public LoginViewModel()
        {
            User = new UserModel();
        }
        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ??
                  (loginCommand = new RelayCommand(async obj =>
                  {
                      var passwordBox = obj as PasswordBox;
                      User.Password = passwordBox.Password;

                      var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                      var context = new ValidationContext(User);
                      if (!Validator.TryValidateObject(User, context, results, true))
                      {
                          foreach (var error in results)
                          {
                              MessageBox.Show(error.ErrorMessage);
                          }
                      }
                      else
                      {
                          Client client = new Client();
                          IsBusy = true;
                          List<PlayerInfo> players = await client.GetAllPlayers();
                          IsBusy = false;
                          PlayerInfo player = new PlayerInfo();
                          foreach (PlayerInfo p in players)
                          {
                              if (p.Name == User.Name)
                                  player = p;
                          }
                          if (HashGenerator.VerifyHashedPassword(player.Password, User.Password))
                          {
                              StartWindow startWindow = new StartWindow(player);
                              startWindow.Show();
                              CloseWindow(Window.GetWindow(passwordBox));
                          }
                          else
                          {
                              MessageBox.Show("Неверное имя пользователя или пароль");
                          }
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
                      User.Password = passwordBox.Password;

                      var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                      var context = new ValidationContext(User);
                      if (!Validator.TryValidateObject(User, context, results, true))
                      {
                          foreach (var error in results)
                          {
                              MessageBox.Show(error.ErrorMessage);
                          }
                      }
                      else
                      {
                          Client client = new Client();
                          IsBusy = true;
                          List<PlayerInfo> players = await client.GetAllPlayers();
                          IsBusy = false;
                          foreach (PlayerInfo pl in players)
                          {
                              if (pl.Name == User.Name)
                              {
                                  MessageBox.Show("Пользователь с таким именем уже существует");
                                  return;
                              }
                          }
                          client = new Client();
                          IsBusy = true;
                          await client.MakeNewPlayer(User.Name, HashGenerator.HashPassword(User.Password));
                          IsBusy = false;
                          MessageBox.Show("Регистрация прошла успешно");
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
