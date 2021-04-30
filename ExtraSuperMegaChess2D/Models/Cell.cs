using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraSuperMegaChess2D
{
    public class Cell : NotifyPropertyChanged
    {
        private CellState _state;
        private bool _active;
        private bool _canMove;
        private bool _whereMove;
        public int x { get; private set; }
        public int y { get; private set; }
        public Cell(int y, int x)
        {
            this.x = x;
            this.y = y;
        }
        public CellState State
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged(); // сообщить интерфейсу, что значение поменялось, чтобы интефейс перерисовался в этом месте
            }
        }
        public bool Active // это будет показывать, что ячейка выделена пользователем
        {
            get => _active;
            set
            {
                _active = value;
                OnPropertyChanged();
            }
        }
        public bool CanMove // это будет показывать, что ячейка выделена пользователем
        {
            get => _canMove;
            set
            {
                _canMove = value;
                OnPropertyChanged();
            }
        }
        public bool WhereMove // это будет показывать, что ячейка выделена пользователем
        {
            get => _whereMove;
            set
            {
                _whereMove = value;
                OnPropertyChanged();
            }
        }
    }
}
