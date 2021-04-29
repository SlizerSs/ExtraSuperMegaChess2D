using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExtraSuperMegaChess2D.Controls
{
    /// <summary>
    /// Логика взаимодействия для ChessPiece.xaml
    /// </summary>
    public partial class ChessPiece : UserControl
    {
        public static readonly DependencyProperty PieceProperty = DependencyProperty.Register("Piece", typeof(CellState), typeof(ChessPiece));

        public CellState Piece
        {
            get => (CellState)GetValue(PieceProperty);
            set => SetValue(PieceProperty, value);
        }

        public ChessPiece()
        {
            InitializeComponent();
        }
    }
}
