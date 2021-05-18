using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChessClient;
using ChessLogic;
namespace ExtraSuperMegaChess2D
{
    public class GameViewModel : NotifyPropertyChanged, EndGameInterface
    {
        private Board _board = new Board();
        private Chess chess;
        private ICommand _resignCommand;
        private ICommand _cellCommand;
        private string from;
        private string to;
        private string figure;
        private string move;
        private string lastMove = "";
        public IEnumerable<char> Numbers => "87654321";
        public IEnumerable<char> Letters => "ABCDEFGH";
        private Client client;
        private Timer timer;
        private PlayerInfo Player { get; set; }
        private GameInfo Game { get; set; }
        private string _opponentName;
        public string OpponentName
        {
            get => _opponentName;
            set { _opponentName = value; OnPropertyChanged(); }
        }

        private Action EndGame;
        public GameViewModel(PlayerInfo player, GameInfo game)
        {
            chess = new Chess(game.FEN);
            Player = player;
            Game = game;
            client = new Client("http://localhost:56213/Games/Details", Player.PlayerID);
            Client client1 = new Client("http://localhost:56213/Games/Details", Player.PlayerID);
            Client client2 = new Client("http://localhost:56213/Games/OpponentDetails", Player.PlayerID);

            SetupBoard();
            if (chess.GetMoveColor() == Game.YourColor)
                MarkValidFigures();
            string n = "";
            TimerCallback tm = async x =>
            {
                Game = await client1.GetGameInfo(Game.GameID);
                await GetOpponentName(n, client2);

                if (chess.fen != Game.FEN)
                {
                    chess = new Chess(Game.FEN);
                    if (chess.GetMoveColor() == Game.YourColor)
                    {
                        SetupBoard();
                        MarkValidFigures();
                    }
                }
                if(Game.Status == "done")
                {
                    Client playerClient = new Client("http://localhost:56213/Players/Details", Player.PlayerID);
                    Player = await playerClient.GetPlayerInfo();
                    Application.Current.Dispatcher.Invoke( () => {
                        timer.Dispose();
                        StartWindow sw = new StartWindow(Player);
                        sw.Show();
                        EndGame?.Invoke();
                    });
                    
                }

            };
            timer = new Timer(tm, null, 0, 1000);

        }

        event Action EndGameInterface.EndGame
        {
            add
            {
                EndGame += value;
            }

            remove
            {
                EndGame -= value;
            }
        }

        public Board Board
        {
            get => _board;
            set
            {
                _board = value;
                OnPropertyChanged();
            }
        }

        public ICommand ResignCommand
        {
            get
            {
                return _resignCommand ??
                (_resignCommand = new RelayCommand(async parameter =>
                {
                    ResignWindow resignWindow = new ResignWindow();

                    if (resignWindow.ShowDialog() == true)
                    {
                        Client client2 = new Client("http://localhost:56213/Games/Details", Player.PlayerID);
                        await client2.EndGame(Game.GameID, false);
                    }

                }));
            }
        }

        public ICommand CellCommand
        {
            get
            {
                return _cellCommand ??
                (_cellCommand = new RelayCommand(async parameter =>
                {

                    Cell cell = (Cell)parameter;
                    Cell activeCell = Board.FirstOrDefault(x => x.Active);
                    if (cell.State != CellState.Empty && activeCell == null)
                    {
                        from = GetSquare(cell);
                        cell.Active = true;
                        MarkValidMoves(cell);
                    }
                    if (activeCell != null)
                    {
                        
                        to = GetSquare(cell);
                        figure = ((char)activeCell.State).ToString();

                        move = figure + from + to;
                        if (chess.Move(move)!=chess && chess.GetMoveColor() == Game.YourColor && Game.Status=="play")
                        {
                            Game = await client.SendMove(Game.GameID, move);
                            cell.State = activeCell.State;

                            activeCell.State = CellState.Empty;

                            chess = chess.Move(move);

                            if (chess.IsMate())
                            {
                                await client.EndGame(Game.GameID, true);
                            }
                            lastMove = move;
                            UnMarkValidFigures();
                        }
                        UnMarkValidMoves();

                        //MarkValidFigures();
                        from = null;
                        to = null;
                        move = null;
                        activeCell.Active = false;
                        activeCell = null;
                        
                    }
                }, parameter => parameter is Cell cell && (Board.Any(x => x.Active) || cell.State != CellState.Empty)));
            }
        }
        private void UnMarkValidMoves()
        {
            foreach (Cell cell in Board)
                cell.WhereMove = false;
        }
        private void MarkValidMoves(Cell fromCell)
        {

            foreach (string moves in chess.GetAllMoves())
            {
                int x;
                int y;
                GetCoord(moves.Substring(1, 2), out x, out y);
                int xto;
                int yto;
                GetCoord(moves.Substring(3, 2), out xto, out yto);
                Cell moveCell = Board.FirstOrDefault(t => fromCell.x == x && fromCell.y == TransformCoord(y) &&
                                            t.x == xto && t.y == TransformCoord(yto));
                if(moveCell!=null)
                    moveCell.WhereMove = true;
            }
        }
        private void MarkValidFigures()
        {
            UnMarkValidFigures();
            foreach (string moves in chess.GetAllMoves())
            {
                int x;
                int y;
                GetCoord(moves.Substring(1, 2), out x, out y);
                Board.FirstOrDefault(t => t.x == x && t.y == TransformCoord(y)).CanMove = true;
            }


        }
        private void UnMarkValidFigures()
        {
            foreach (Cell cell in Board)
                cell.CanMove = false;
        }
        public void GetCoord(string name, out int x, out int y)
        {
            x = 9;
            y = 9;
            if (name.Length == 2 &&
               name[0] >= 'a' && name[0] <= 'h' &&
               name[1] >= '1' && name[1] <= '8')
            {
                x = name[0] - 'a';
                y = name[1] - '1';
            }

        }
        private int TransformCoord(int coord)
        {
            return Math.Abs(coord - 7);
        }
        private string GetSquare(Cell cell)
        {
            int x = cell.x;
            int y = TransformCoord(cell.y);
            return ((char)('a' + x)).ToString() + (y + 1).ToString();
        }
        private void SetupBoard()
        {
            Board board = new Board();
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                {
                    string figure = chess.GetFigureAt(x, y).ToString();
                    if (figure == ".") continue;
                    PlaceFigure(board,figure, x, TransformCoord(y));

                }
            Board = board;
        }

        void PlaceFigure(Board board,string figure, int x, int y)
        {
            board[y, x] = (CellState)Convert.ToChar(figure);
        }
        async Task PeriodicRefresh()
        {
            while (true)
            {
                await Task.Delay(1000);

            }
        }
        async Task GetOpponentName(string n, Client client2)
        {
            n = (await client2.GetOpponent(Game.GameID)).Name;
            if (n == Player.Name)
                OpponentName = "Ожидаем";
            else
                OpponentName = n;
        }

    }
}
