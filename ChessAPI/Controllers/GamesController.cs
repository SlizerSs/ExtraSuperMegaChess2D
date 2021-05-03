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

namespace ChessAPI.Controllers
{
    public class GamesController : Controller
    {
        private ModelChessDB db = new ModelChessDB();

        // GET: Games
        public List<string> Index()
        {
            List<string> result = new List<string>();
            List<Game> list = db.Games.ToList();
            foreach (Game g in list)
                result.Add(new JavaScriptSerializer().Serialize(g));
            return result;
        }

        // GET: Games/Details/5
        public string Details(int? id)
        {
            Game game = db.Games.Find(id);
            if (game == null)
                return null;

            return new JavaScriptSerializer().Serialize(game);
        }

        public string Move(int id, string move)
        {
            Logic logic = new Logic();
            Game game = logic.MakeMove(id, move);
            return new JavaScriptSerializer().Serialize(game);
        }
        // GET: Games/Create
        public string Create()
        {
            Logic logic = new Logic();
            Game game = logic.MakeNewGame();
            return new JavaScriptSerializer().Serialize(game);
        }

        // POST: Games/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Create([Bind(Include = "GameID,FEN,Status")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Games.Add(game);
                db.SaveChanges();
                
            }

            return new JavaScriptSerializer().Serialize(game);
        }

        // GET: Games/Delete/5
        public string Delete(int? id)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return null;
            }
            return new JavaScriptSerializer().Serialize(game);
        }

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
