using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ExtraSuperMegaChess2D
{
    class EndGameBehavior : Behavior<Window>
    {
        public static readonly DependencyProperty EndGameProperty = DependencyProperty.Register(nameof(EndGame), typeof(EndGameInterface), typeof(EndGameBehavior), new FrameworkPropertyMetadata(default(EndGameInterface), OnCallback));

        private static void OnCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is EndGameBehavior x)
            {
                x.EndGame.EndGame += x.OnEndGame;
            }
        }

        public EndGameInterface EndGame { get => (EndGameInterface)GetValue(EndGameProperty);
                                          set => SetValue(EndGameProperty, value);
        }
        protected override void OnAttached()
        {

        }

        protected override void OnDetaching()
        {
            EndGame.EndGame -= OnEndGame;
        }

        private void OnEndGame()
        {
            AssociatedObject.Close();
        }
    }
}
