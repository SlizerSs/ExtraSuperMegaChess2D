using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraSuperMegaChess2D
{
    class StartViewModel
    {
        //получить инфу из бд по юзеру
        //сделать генерацию новой игры
        //сдача, предложение ничьи в игре, завершение игры и т.д.
        //связь с сервером
        public int PlayerID { get; set; }
        public StartViewModel(int player_id)
        {
            PlayerID = player_id;

        }
    }
}
