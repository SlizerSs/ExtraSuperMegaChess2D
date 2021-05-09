using ChessAPI.Controllers;
using ChessAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraSuperMegaChess2D
{
    public class StartViewModel : NotifyPropertyChanged
    {
        //синхронизация всех клиентов с сервером
        //сдача, предложение ничьи в игре, завершение игры и т.д.
        //связь с сервером
        private RelayCommand startNewGameCommand;

        private GamesController gc;

        private List<Game> _games;
        public List<Game> Games
        {
            get => _games;
            set { _games = value; OnPropertyChanged(); }
        }
        public Player Player { get; set; }
        
        public StartViewModel(Player player)
        {
            Player = player;
            gc = new GamesController();
            Games = new List<Game>();
            Games = gc.Index();
            

        }
        public RelayCommand StartNewGameCommand
        {
            get
            {
                return startNewGameCommand ??
                  (startNewGameCommand = new RelayCommand(obj =>
                  {
                      var player = obj as Player;
                      gc.Create(player);
                      Games = gc.Index();

                  }));
            }
        }

    }
}
