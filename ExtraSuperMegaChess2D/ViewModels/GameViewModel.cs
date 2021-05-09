using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        private string from;
        private string to;
        private string figure;
        private string move;

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

        public ICommand NewGameCommand
        {
            get
            {
                return _newGameCommand ??
                (_newGameCommand = new RelayCommand(parameter =>
                {
                    chess = new Chess();
                    SetupBoard();
                    MarkValidFigures();
                }));
            }

        }

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

        public ICommand CellCommand
        {
            get
            {
                return _cellCommand ??
                (_cellCommand = new RelayCommand(parameter =>
                {
                    //реализовать ход

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
                        if (chess.Move(move)!=chess)
                        {
                            cell.State = activeCell.State;

                            activeCell.State = CellState.Empty;
                            
                            chess = chess.Move(move);
                        }
                        UnMarkValidMoves();
                        MarkValidFigures();
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
            foreach (Cell cell in Board)
                cell.CanMove = false;
            foreach(string moves in chess.GetAllMoves())
            {
                int x;
                int y;
                GetCoord(moves.Substring(1, 2), out x, out y);
                Board.FirstOrDefault(t => t.x == x && t.y == TransformCoord(y)).CanMove = true;
            }


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
        public GameViewModel()
        {
        }
    }
}
