using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChessLogic;

namespace ChessAPI.Models
{
    public class Logic
    {
        ModelChessDB db;
        public Logic()
        {
            db = new ModelChessDB();
        }
        public Game MakeNewGame()
        {
            Chess chess = new Chess();
            Game game = new Game();
            game.FEN = chess.fen;
            game.Status = "wait";

            db.Games.Add(game);
            db.SaveChanges();

            return game;
        }
        public Game GetGame(int id)
        {
            return db.Games.Find(id);
        }
        public Game MakeMove(int id, string move)
        {
            Game game = GetGame(id);
            if (game == null) 
                return game;

            if (game.Status != "play")
                return game;

            Chess chess = new Chess(game.FEN);
            Chess chessNext = chess.Move(move);

            if (chessNext.fen == game.FEN)
                return game;

            game.FEN = chessNext.fen;

            if (chessNext.IsMate() || chessNext.IsStalemate())
                game.Status = "done";

            db.Entry(game).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return game;
        }
        public Player MakeNewPlayer(string name, string password)
        {
            Player player = new Player();
            player.Name = name;
            player.Password = password;

            db.Players.Add(player);
            db.SaveChanges();

            return player;
        }
        public Side MakeNewSide(int GameID, int PlayerID, string color)
        {
            Side side = new Side();
            side.GameID = GameID;
            side.PlayerID = PlayerID;
            side.Color = color;

            db.Sides.Add(side);
            db.SaveChanges();

            return side;
        }
        public PlayerStatistic MakeNewPS(int PlayerID)
        {
            PlayerStatistic ps = new PlayerStatistic();
            ps.PlayerID = PlayerID;
            ps.Games = 0;
            ps.Wins = 0;
            ps.Loses = 0;
            ps.Draws = 0;
            ps.Resigns = 0;
            
            db.PlayerStatistics.Add(ps);
            db.SaveChanges();

            return ps;
        }
    }
}