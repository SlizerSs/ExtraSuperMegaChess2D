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
        public static readonly DependencyProperty EndGameProperty = DependencyProperty.Register("", typeof(EndGameInterface), typeof(EndGameBehavior), new FrameworkPropertyMetadata(default(EndGameInterface)));
        public EndGameInterface EndGame { get => (EndGameInterface)GetValue(EndGameProperty);
                                          set => SetValue(EndGameProperty, value);
        }
        protected override void OnAttached()
        {
            EndGame.EndGame += OnEndGame;
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
