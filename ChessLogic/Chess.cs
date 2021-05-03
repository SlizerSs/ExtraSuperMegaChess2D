using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    /// <summary>
    /// Main class of chess logic
    /// </summary>
    public class Chess
    {
        public string fen { get; private set; }
        Board board;
        Moves moves;
        List<FigureMoving> allMoves;
        public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            this.fen = fen;
            board = new Board(fen);
            moves = new Moves(board);
        }
        Chess (Board board)
        {
            this.board = board;
            this.fen = board.fen;
            moves = new Moves(board);
        }
        /// <summary>
        /// Method for a figure moving
        /// </summary>
        /// <param name="move">Details of move (example e7e8Q)</param>
        /// <returns>New view of game field</returns>
        public Chess Move(string move)
        {
            FigureMoving fm = new FigureMoving(move);
            if (!moves.CanMove(fm))
                return this;
            if (board.IsCheckAfterMove(fm))
                return this;
            Board nextBoard = board.Move(fm);

            return(new Chess(nextBoard));
        }
        /// <summary>
        /// Gets position of a figure
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public char GetFigureAt(int x, int y)
        {
            Square square = new Square(x, y);
            Figure f = board.GetFigureAt(square);
            return f == Figure.none ? '.' : (char)f;
        }
        void FindAllMoves()
        {
            allMoves = new List<FigureMoving>();
            foreach (FigureOnSquare fs in board.YieldFigures())
                foreach(Square to in Square.YieldSquares())
                {
                    FigureMoving fm = new FigureMoving(fs, to);
                    if (moves.CanMove(fm))
                        if(!board.IsCheckAfterMove(fm))
                            allMoves.Add(fm);
                }
        }
        public List<string> GetAllMoves()
        {
            FindAllMoves();
            List<string> list = new List<string>();
            foreach (FigureMoving fm in allMoves)
                list.Add(fm.ToString());
            return list;
        }
        public bool IsCheck()
        {
            return board.IsCheck();
        }
        public bool IsMate()
        {
            FindAllMoves();
            if (IsCheck())
                if(allMoves.Count()==0)
                    return true;
            return false;
        }
        public bool IsStalemate()
        {
            FindAllMoves();
            if (allMoves.Count() == 0)
                if (!IsCheck())
                    return true;
            return false;
        }
    }
}
