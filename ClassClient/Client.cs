using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using ClientServerApi;

namespace ChessClient
{
    public class Client
    {
        private readonly HttpClient _httpClient;
        private readonly string _serverUrl; 
        private readonly Uri _apiUri;
        public int userID { get; private set; }

        public Client(int userid)
        {
            userID = userid;

            _serverUrl = "http://localhost:56213/";
            _httpClient = new HttpClient();
            _apiUri = new Uri(_serverUrl);
        }
        public Client()
        {
            _serverUrl = "http://localhost:56213/";
            _httpClient = new HttpClient();
            _apiUri = new Uri(_serverUrl);
        }

        public async Task<GameInfo> GetCurrentGame()
        {
            return new GameInfo(ParseJSON(await CallServer()));
        }

        public async Task<GameInfo> GetGameInfo(int GameID)
        {
            return new GameInfo(ParseJSON(await CallServer(GameID)));
        }

        public async Task<List<GameInfo>> GetAllGames()
        {
            List<GameInfo> result = new List<GameInfo>();

            foreach (NameValueCollection col in MultipleParseJSONForGames(await CallServerForGames()))
                result.Add(new GameInfo(col));
            return result;
        }

        public async Task<GameInfo> SendMove(int id, string move)
        {
            return new GameInfo(ParseJSON(await CallServer(id, move)));
        }
        public async Task<GameInfo> ChangeStatus(int gameID, string newStatus)
        {
            return new GameInfo(ParseJSON(await CallServerChangeStatus(gameID, newStatus)));
        }

        public async Task<PlayerInfo> MakeNewPlayer(string name, string password)
        {
            return new PlayerInfo(ParseJSON(await CallServerForPlayer(name, password)));
        }
        public async Task<List<PlayerInfo>> GetAllPlayers()
        {
            List<PlayerInfo> result = new List<PlayerInfo>();

            foreach (NameValueCollection col in MultipleParseJSONForPlayers(await CallServerForPlayer()))
                result.Add(new PlayerInfo(col));
            return result;
        }
        public async Task<PlayerInfo> GetPlayerInfo()
        {
            return new PlayerInfo(ParseJSON(await CallServerForPlayerInfo()));
        }
        public async Task MakeNewSide(int ID, string Color)
        {
            await CallServerForSide(ID, Color);
            return;
        }
        public async Task<PlayerInfo> GetOpponent(int ID)
        {
            return new PlayerInfo(ParseJSON(await CallServerForOpponent(ID)));
        }
        public async Task<string> GetSideColor(int ID)
        {
            return await CallServerForSideColor(ID, userID);
        }
        public async Task EndGame(int gameID, bool IsWinner, bool isStalemate)
        {
            await CallServerForEndGame(gameID, IsWinner, isStalemate);
            return;
        }
        private async Task<string> CallServer(int GameID)
        {
            var bodyData = JsonConvert.SerializeObject(new GameSendMoveData { UserID = userID, ID = GameID });
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Games/GameDetails/", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private async Task<string> CallServer(int param1, string param2 = "")
        {
            var bodyData = JsonConvert.SerializeObject(new GameSendMoveData { UserID = userID, ID = param1, Move = param2 });
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Games/Details/", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        private async Task<string> CallServer()
        {
            var bodyData = JsonConvert.SerializeObject(userID);
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Games/Create/", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private async Task<string> CallServerForPlayer(string param1 = "", string param2 = "")
        {

            var bodyData = JsonConvert.SerializeObject(new UserLoginData { Login = param1, HashedPassword = param2 });
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Players/Create/", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        private async Task<string> CallServerForPlayer()
        {

            var bodyData = "";
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Players", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        private async Task<string> CallServerForPlayerInfo()
        {

            var bodyData = JsonConvert.SerializeObject(userID);
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Players/Details/", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        private async Task<string> CallServerForOpponent(int gameID)
        {

            var bodyData = JsonConvert.SerializeObject(new GetOpponentData { ID = gameID , UserID = userID});
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Games/OpponentDetails/", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        private async Task<string> CallServerForGames()
        {

            var bodyData = "";
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Games", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        private async Task<string> CallServerForSide(int ID, string Color)
        {

            var bodyData = JsonConvert.SerializeObject(new MakeNewSideData { UserID = userID, ID = ID, Color = Color });
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Sides/Create/", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        private async Task<string> CallServerChangeStatus(int gameID, string newStatus)
        {

            var bodyData = JsonConvert.SerializeObject(new ChangeGameStatusData { ID= gameID, Status = newStatus });
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Games/ChangeStatus/", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        private async Task<string> CallServerForSideColor(int ID, int userID)
        {

            var bodyData = JsonConvert.SerializeObject(new GetOpponentData { ID= ID, UserID = userID });
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Players/SideColor/", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        private async Task<string> CallServerForEndGame(int gameID, bool IsWinner, bool IsStalemate)
        {

            var bodyData = JsonConvert.SerializeObject(new EndGameData { UserID = userID, ID = gameID, IsWinner = IsWinner, IsStalemate = IsStalemate });
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Games/EndGame/", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        private NameValueCollection ParseJSON(string json)
        {
            NameValueCollection list = new NameValueCollection();

            string pattern = @"""(\w+)\"":""?([^,""}]*)""?";

            foreach (Match m in Regex.Matches(json, pattern))
                if (m.Groups.Count == 3)
                    list[m.Groups[1].Value] = m.Groups[2].Value;

            return list;
        }
        private List<NameValueCollection> MultipleParseJSONForPlayers(string json)
        {
            NameValueCollection list = new NameValueCollection();
            List<NameValueCollection> result = new List<NameValueCollection>();
            string pattern = @"""(\w+)\"":""?([^,""}]*)""?";
            int x = 0;
            foreach (Match m in Regex.Matches(json, pattern))
            {
                if (m.Groups.Count == 3)
                    list[m.Groups[1].Value] = m.Groups[2].Value;
                x++;
                if (x%6==0)
                {
                    result.Add(list);
                    list = new NameValueCollection();
                }
            }

            return result;
        }
        private List<NameValueCollection> MultipleParseJSONForGames(string json)
        {
            NameValueCollection list = new NameValueCollection();
            List<NameValueCollection> result = new List<NameValueCollection>();
            string pattern = @"""(\w+)\"":""?([^,""}]*)""?";
            int x = 0;
            foreach (Match m in Regex.Matches(json, pattern))
            {
                if (m.Groups.Count == 3)
                    list[m.Groups[1].Value] = m.Groups[2].Value;
                x++;
                if (x%7==0)
                {
                    result.Add(list);
                    list = new NameValueCollection();
                }
            }

            return result;
        }
    }
}
