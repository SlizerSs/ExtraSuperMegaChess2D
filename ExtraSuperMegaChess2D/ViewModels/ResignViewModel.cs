using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExtraSuperMegaChess2D
{
    class ResignViewModel
    {
        private ICommand _okCommand;
        public ResignViewModel()
        {

        }
        public ICommand OkCommand
        {
            get
            {
                return _okCommand ??
                (_okCommand = new RelayCommand(parameter =>
                {
                    var win = Window.GetWindow(parameter as Button);
                    win.DialogResult = true;

                }));
            }
        }
    }
}
