using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChessServer
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IServiceChess" в коде и файле конфигурации.
    [ServiceContract]
    public interface IServiceChess
    {
        [OperationContract]
        int Connect();

        [OperationContract]
        int Disconnect();
    }
}
