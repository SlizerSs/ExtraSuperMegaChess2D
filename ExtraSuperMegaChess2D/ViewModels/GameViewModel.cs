using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChessLogic;
namespace ExtraSuperMegaChess2D
{
    public class GameViewModel : NotifyPropertyChanged
    {
        private Board _board = new Board();
        private Chess chess = new Chess();
        private ICommand _newGameCommand;
        private ICommand _clearCommand;
        private ICommand _cellCommand;

        public IEnumerable<char> Numbers => "87654321";
        public IEnumerable<char> Letters => "ABCDEFGH";

        public Board Board
        {
            get => _board;
            set
            {
                _board = value;
                OnPropertyChanged();
            }
        }

        //public ICommand NewGameCommand => _newGameCommand ??= new RelayCommand(parameter =>
        //{
        //    SetupBoard();
        //});
        public ICommand NewGameCommand
        {
            get
            {
                return _newGameCommand ??
                (_newGameCommand = new RelayCommand(parameter =>
                {
                    SetupBoard();
                }));
            }

        }

        //public ICommand ClearCommand => _clearCommand ??= new RelayCommand(parameter =>
        //{
        //    Board = new Board();
        //});
        public ICommand ClearCommand
        {
            get
            {
                return _clearCommand ??
                (_clearCommand = new RelayCommand(parameter =>
                {
                    Board = new Board();
                }));
            }
        }
        //public ICommand CellCommand => _cellCommand ??= new RelayCommand(parameter =>
        //{
        //    Cell cell = (Cell)parameter;
        //    Cell activeCell = Board.FirstOrDefault(x => x.Active);
        //    if (cell.State != CellState.Empty)
        //    {
        //        if (!cell.Active && activeCell != null)
        //            activeCell.Active = false;
        //        cell.Active = !cell.Active;
        //    }
        //    else if (activeCell != null)
        //    {
        //        activeCell.Active = false;
        //        cell.State = activeCell.State;
        //        activeCell.State = CellState.Empty;
        //    }
        //}, parameter => parameter is Cell cell && (Board.Any(x => x.Active) || cell.State != CellState.Empty));
        public ICommand CellCommand
        {
            get
            {
                return _cellCommand ??
                (_cellCommand = new RelayCommand(parameter =>
                {
                    Cell cell = (Cell)parameter;
                    Cell activeCell = Board.FirstOrDefault(x => x.Active);
                    if (cell.State != CellState.Empty)
                    {
                        if (!cell.Active && activeCell != null)
                            activeCell.Active = false;
                        cell.Active = !cell.Active;
                    }
                    else if (activeCell != null)
                    {
                        activeCell.Active = false;
                        cell.State = activeCell.State;
                        activeCell.State = CellState.Empty;
                    }
                }, parameter => parameter is Cell cell && (Board.Any(x => x.Active) || cell.State != CellState.Empty)));
            }
        }
        private void SetupBoard()
        {
            Board board = new Board();
            board[0, 0] = CellState.BlackRook;
            board[0, 1] = CellState.BlackKnight;
            board[0, 2] = CellState.BlackBishop;
            board[0, 3] = CellState.BlackQueen;
            board[0, 4] = CellState.BlackKing;
            board[0, 5] = CellState.BlackBishop;
            board[0, 6] = CellState.BlackKnight;
            board[0, 7] = CellState.BlackRook;
            for (int i = 0; i < 8; i++)
            {
                board[1, i] = CellState.BlackPawn;
                board[6, i] = CellState.WhitePawn;
            }
            board[7, 0] = CellState.WhiteRook;
            board[7, 1] = CellState.WhiteKnight;
            board[7, 2] = CellState.WhiteBishop;
            board[7, 3] = CellState.WhiteQueen;
            board[7, 4] = CellState.WhiteKing;
            board[7, 5] = CellState.WhiteBishop;
            board[7, 6] = CellState.WhiteKnight;
            board[7, 7] = CellState.WhiteRook;
            Board = board;
        }
        void ShowFigures()
        {
            int nr = 0;
            for(int y = 0; y<8;y++)
                for(int x = 0; x < 8; x++)
                {
                    string figure = chess.GetFigureAt(x, y).ToString();
                    if (figure == ".") continue;
                    //PlaceFigure(figure, x, y);

                }
            for (; nr < 32; nr++)
            {

            }

        }
        public GameViewModel()
        {
        }
    }
}
