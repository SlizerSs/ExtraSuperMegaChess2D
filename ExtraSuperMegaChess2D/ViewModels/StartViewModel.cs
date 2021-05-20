using ChessClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ExtraSuperMegaChess2D
{
    public class StartViewModel : NotifyPropertyChanged
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        private RelayCommand startNewGameCommand;
        private RelayCommand connectToGameCommand;

        private List<GameInfo> _games;
        public List<GameInfo> Games
        {
            get => _games;
            set { _games = value; OnPropertyChanged(); }
        }
        public PlayerInfo Player { get; set; }
        Timer timer;

        public StartViewModel(PlayerInfo player)
        {
            Player = player;
            Client client = new Client();
            Games = new List<GameInfo>();
            

            Client client1 = new Client();

            TimerCallback tm = async x =>
            {
                Games = ExceptDone(await client1.GetAllGames());
            };
            timer = new Timer(tm, null, 0, 1000);
        }
        public RelayCommand StartNewGameCommand
        {
            get
            {
                return startNewGameCommand ??
                  (startNewGameCommand = new RelayCommand(async obj =>
                  {
                      var bt = obj as Button;
                      Client client = new Client(Player.PlayerID);
                      IsBusy = true;
                      GameInfo newGame = await client.GetCurrentGame();
                      IsBusy = false;
                      GameWindow gw = new GameWindow(Player, newGame);
                      gw.Show();
                      CloseWindow(Window.GetWindow(bt));

                  }));
            }
        }
        public RelayCommand ConnectToGameCommand
        {
            get
            {
                return connectToGameCommand ??
                  (connectToGameCommand = new RelayCommand(async parameter =>
                  {
                      var param = parameter as Tuple<int, ListBox>;
                      int openGameID = param.Item1;
                      
                      Client client = new Client(Player.PlayerID);
                      Client clientSide = new Client(Player.PlayerID);
                      IsBusy = true;
                      GameInfo openGame = await client.GetGameInfo(openGameID);
                      if (openGame.Black=="" || openGame.Black == Player.Name || openGame.White == Player.Name)
                      {
                          if(openGame.Black != Player.Name && openGame.White != Player.Name)
                          {
                              await clientSide.MakeNewSide(openGame.GameID, "b");
                              Client clientStatus = new Client(Player.PlayerID);
                              await clientStatus.ChangeStatus(openGame.GameID, "play");
                              IsBusy = false;
                          }
                            
                          GameWindow gw = new GameWindow(Player, openGame);
                          gw.Show();
                          
                          CloseWindow(Window.GetWindow(param.Item2));
                      }
                      IsBusy = false;

                  }));
            }
        }
        private List<GameInfo> ExceptDone(List<GameInfo> list)
        {
            List<GameInfo> result = new List<GameInfo>();
            foreach (GameInfo l in list)
                if (l.Status != "done")
                    result.Add(l);
            return result;
        }
        private void CloseWindow(Window win)
        {
            if (win == null)
                return;

            win.Close();
        }
    }

}
