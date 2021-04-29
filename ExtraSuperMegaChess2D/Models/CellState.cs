using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraSuperMegaChess2D
{
    public enum CellState
    {
        Empty,       // пусто
        WhiteKing = 'K',   // король
        WhiteQueen = 'Q',  // ферзь
        WhiteRook = 'R',   // ладья
        WhiteKnight = 'N', // конь
        WhiteBishop = 'B', // слон
        WhitePawn = 'P',   // пешка

        BlackKing = 'k',
        BlackQueen = 'q',
        BlackRook = 'r',
        BlackKnight = 'n',
        BlackBishop = 'b',
        BlackPawn = 'p'
    }
}
