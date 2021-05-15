using ChessClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExtraSuperMegaChess2D
{
    public class StartViewModel : NotifyPropertyChanged
    {
        //исправить трабл с добавлением новой игры
        //синхронизация всех клиентов с сервером
        //сдача, предложение ничьи в игре, завершение игры и т.д.

        private RelayCommand startNewGameCommand;

        private List<GameInfo> _games;
        public List<GameInfo> Games
        {
            get => _games;
            set { _games = value; OnPropertyChanged(); }
        }
        public PlayerInfo Player { get; set; }
        Timer timer;
        public Action CloseAction { get; set; }
        public StartViewModel(PlayerInfo player)
        {
            Player = player;
            Client client = new Client("http://localhost:56213/Games");
            Games = new List<GameInfo>();
            

            Client client1 = new Client("http://localhost:56213/Games");

            TimerCallback tm = async x =>
            {
                Games = await client1.GetAllGames();
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
                      Client client = new Client("http://localhost:56213/Games/Create", Player.PlayerID);
                      await client.GetCurrentGame();
                      GameWindow gw = new GameWindow(Player);
                      gw.Show();
                      CloseAction();

                  }));
            }
        }

    }
}
