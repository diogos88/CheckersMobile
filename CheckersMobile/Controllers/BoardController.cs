/*
*CheckersMobile
*
*Copyright (C) 2014 Frédéric Hanna, Diogo Soares, David Desrochers and Étienne Chevalier
*
*This file is part of CheckersMobile.
*
*CheckersMobile is free software: you can redistribute it and/or modify it under the terms of the
*GNU General Public License as published by the Free Software Foundation, either version 2 of the
*License, or (at your option) any later version.
*
*CheckersMobile is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
*without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*
*See the GNU General Public License for more details. You should have received a copy of the GNU
*General Public License along with CheckersMobile. If not, see <http://www.gnu.org/licenses/>.
*
*Authors: Frédéric Hanna
*         Diogo Soares
*         David Desrochers
*         Étienne Chevalier
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CheckerManager;
namespace CheckersMobile.Controllers
{

    public class BoardController : Controller
    {

        

        public BoardController()
        {
             
        }

        public ActionResult Index()
        {
            GameManager game = new GameManager();

            ViewBag.message = "Allo!";
            ViewBag.board = game.board2D;
            ViewBag.boardSizeX = game.boardSizeX;
            ViewBag.boardSizeY = game.boardSizeY;

            Session["game"] = game;

            return View();
        }

        // Ajax
        public ActionResult StartNewGame(int color, int type)
        {
            GameManager game = new GameManager();
            game.CreateBoard(color, type);

            while (game.turnPlayer == GameManager.PLAYER_2)
               game.enemyinfo();

            ViewBag.boardSizeX = game.boardSizeX;
            ViewBag.boardSizeY = game.boardSizeY;

            ViewBag.PLAYER_1 = GameManager.PLAYER_1;
            ViewBag.PLAYER_2 = GameManager.PLAYER_2;

            Session["game"] = game;

            return PartialView("Board", game);

        }


        // Ajax
        public ActionResult MovePiece(int squareIndexStart, int squareIndexEnd)
        {
            GameManager game = (GameManager)Session["game"];
            
            Session["valid"] = false;
            // cant move same piece at same square
            if (squareIndexStart == squareIndexEnd) return Json(new { board = game }, JsonRequestBehavior.AllowGet); 

            // can only move my own pieces
            if (game.turnPlayer != game.yourcolor) return Json(new { board = game }, JsonRequestBehavior.AllowGet); 
            
            // if movement valid, then change board value and return new board
            Point source = game.getCell2D(squareIndexStart);
            Point destination = game.getCell2D(squareIndexEnd);

            // try to make the move
            if (game.Move(source.X, source.Y, destination.X, destination.Y))
            {
               while(game.turnPlayer == GameManager.PLAYER_2)
                  game.enemyinfo();
              
                // success
                Session["game"] = game;
                Session["valid"] = true;
            }

           // TODO
            var winner = game.CheckWinner();
            if (winner == GameManager.PLAYER_1)
               Debug.WriteLine("Player 1 WIN");
            else if (winner == GameManager.PLAYER_2)
               Debug.WriteLine("Player 2 WIN");

            // invalid move
            return Json(new { board = game, winner = winner }, JsonRequestBehavior.AllowGet); 
        }

        private String CreateAndInitializeBoard()
        {
            string board = "";
            for (int i = 8; i >= 1; --i)
            {
                board += "-";
                for (char c = 'a'; c <= 'h'; ++c)
                {
                    board += "| ";
                }
                board += "<br>";
            }
            return board;
        }


    }
}
