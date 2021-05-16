using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ChessAPI.Models;
using ClientServerApi;
using Newtonsoft.Json;

namespace ChessAPI.Controllers
{
    public class SidesController : Controller
    {
        private ModelChessDB db = new ModelChessDB();

        // GET: Sides
        public List<Side> Index()
        {
            var sides = db.Sides.Include(s => s.Game).Include(s => s.Player);
            List<Side> list = sides.ToList();
            return list;
        }

        // GET: Sides/Details/5
        public Side Details(int? id)
        {
            Side side = db.Sides.Find(id);
            return side;
        }

        // GET: Sides/Create
        [HttpPost]
        public string Create()
        {
            MakeNewSideData sideData;
            using (StreamReader stream = new StreamReader(HttpContext.Request.GetBufferlessInputStream()))
            {
                string body = stream.ReadToEnd();
                sideData = JsonConvert.DeserializeObject<MakeNewSideData>(body);
                // body = "param=somevalue&param2=someothervalue"
            }
            Logic logic = new Logic();
            Side side = logic.MakeNewSide(sideData.ID, sideData.UserID, sideData.Color);

            ViewBag.GameID = new SelectList(db.Games, "GameID", "FEN");
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Name");
            return new JavaScriptSerializer().Serialize(side);

        }

        // POST: Sides/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public string Create([Bind(Include = "SideID,GameID,PlayerID,Color")] Side side)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Sides.Add(side);
        //        db.SaveChanges();
        //    }

        //    ViewBag.GameID = new SelectList(db.Games, "GameID", "FEN", side.GameID);
        //    ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Name", side.PlayerID);
        //    return new JavaScriptSerializer().Serialize(side);

        //}

        //// GET: Sides/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Side side = db.Sides.Find(id);
        //    if (side == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.GameID = new SelectList(db.Games, "GameID", "FEN", side.GameID);
        //    ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Name", side.PlayerID);
        //    return View(side);
        //}

        //// POST: Sides/Edit/5
        //// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        //// сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "SideID,GameID,PlayerID,Color")] Side side)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(side).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.GameID = new SelectList(db.Games, "GameID", "FEN", side.GameID);
        //    ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Name", side.PlayerID);
        //    return View(side);
        //}

        //// GET: Sides/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Side side = db.Sides.Find(id);
        //    if (side == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(side);
        //}

        //// POST: Sides/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Side side = db.Sides.Find(id);
        //    db.Sides.Remove(side);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
