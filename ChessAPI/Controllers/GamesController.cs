using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ChessAPI.Models;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;
using ClientServerApi;

namespace ChessAPI.Controllers
{
    public class GamesController : Controller
    {
        private ModelChessDB db = new ModelChessDB();

        // GET: Games
        public string Index()
        {
            string result = "";
            Logic logic = new Logic();

            var games = db.Games.ToList(); ;

            foreach (Game pl in games)
            {
                result += logic.LiteGameDetails(pl);
            }
            return result;
        }

        // GET: Games/Details/5
        [HttpPost]
        public string GameDetails()
        {
            GameSendMoveData gameData;
            using (StreamReader stream = new StreamReader(HttpContext.Request.GetBufferlessInputStream()))
            {
                string body = stream.ReadToEnd();
                gameData = JsonConvert.DeserializeObject<GameSendMoveData>(body);
                // body = "param=somevalue&param2=someothervalue"
            }
            Game game = db.Games.Find(gameData.ID);
            Logic logic = new Logic();

            return logic.GameDetails(game, gameData.UserID);
        }
        [HttpPost]
        public string Details()
        {
            GameSendMoveData moveData;
            using (StreamReader stream = new StreamReader(HttpContext.Request.GetBufferlessInputStream()))
            {
                string body = stream.ReadToEnd();
                moveData = JsonConvert.DeserializeObject<GameSendMoveData>(body);
                // body = "param=somevalue&param2=someothervalue"
            }
            Logic logic = new Logic();
            Game game = logic.MakeMove(moveData.ID, moveData.Move);
            return logic.GameDetails(game, moveData.UserID);
        }
        [HttpPost]
        public string OpponentDetails()
        {
            GetOpponentData getopponentData;
            using (StreamReader stream = new StreamReader(HttpContext.Request.GetBufferlessInputStream()))
            {
                string body = stream.ReadToEnd();
                getopponentData = JsonConvert.DeserializeObject<GetOpponentData>(body);
                // body = "param=somevalue&param2=someothervalue"
            }
            Logic logic = new Logic();
            Player player = logic.GetOpponent(getopponentData.ID, getopponentData.UserID);
            return logic.PlayerDetails(player);
        }
        [HttpPost]
        // POST: Games/Create
        public string Create()
        {
            int userID;
            using (StreamReader stream = new StreamReader(HttpContext.Request.GetBufferlessInputStream()))
            {
                string body = stream.ReadToEnd();
                userID = int.Parse(JsonConvert.DeserializeObject<string>(body));
                // body = "param=somevalue&param2=someothervalue"
            }
            Logic logic = new Logic();
            Game game = logic.MakeNewGame();
            Side side = logic.MakeNewSide(game.GameID, userID, "w");

            ViewBag.GameID = new SelectList(db.Sides, "GameID", "GameID");
            return logic.GameDetails(game, userID);
        }
        [HttpPost]
        public string ChangeStatus()
        {
            ChangeGameStatusData newStatus;
            using (StreamReader stream = new StreamReader(HttpContext.Request.GetBufferlessInputStream()))
            {
                string body = stream.ReadToEnd();
                newStatus = JsonConvert.DeserializeObject<ChangeGameStatusData>(body);
                // body = "param=somevalue&param2=someothervalue"
            }
            Logic logic = new Logic();
            Game game = logic.ChangeStatus(newStatus.ID, newStatus.Status);
            return logic.LiteGameDetails(game);
        }
        [HttpPost]
        public string EndGame()
        {
            EndGameData endGameData;
            using (StreamReader stream = new StreamReader(HttpContext.Request.GetBufferlessInputStream()))
            {
                string body = stream.ReadToEnd();
                endGameData = JsonConvert.DeserializeObject<EndGameData>(body);
                // body = "param=somevalue&param2=someothervalue"
            }
            Logic logic = new Logic();
            Game game = logic.EndGame(endGameData.ID, endGameData.UserID, endGameData.IsWinner, endGameData.IsStalemate);
            return logic.LiteGameDetails(game);
        }
        // POST: Games/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public string Create([Bind(Include = "GameID,FEN,Status")] Game game)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Games.Add(game);
        //        db.SaveChanges();

        //    }

        //    return new JavaScriptSerializer().Serialize(game);
        //}

        //// GET: Games/Delete/5
        //public string Delete(int? id)
        //{
        //    Game game = db.Games.Find(id);
        //    if (game == null)
        //    {
        //        return null;
        //    }
        //    return new JavaScriptSerializer().Serialize(game);
        //}

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public List<string> DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
            db.SaveChanges();

            List<string> result = new List<string>();
            List<Game> list = db.Games.ToList();
            foreach (Game g in list)
                result.Add(new JavaScriptSerializer().Serialize(g));
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
