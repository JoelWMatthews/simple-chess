using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleChess.Game
{
    public static class GameManager
    {
        public static Board Board { get; private set; }

        public static readonly String StartingPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        public static readonly String TestingString = "r1bk3r/p2pBpNp/n4n2/1p1NP2P/6P1/3P4/P1P1K3/q5b1";

        public static PieceColor PlayerTurn = PieceColor.White;
        public static PieceColor Win = PieceColor.None;
        public static void Initialize()
        {
            Board = new Board(StartingPosition);
            PlayerTurn = PieceColor.White;
            Win = PieceColor.None;
        }

        public static void AddMove(this List<TilePair> pairs, TilePair pair, Board context, Piece piece, bool pruneCheck)
        {

            //Debug.Log($"Checking move: {piece} from {piece.Tile} to {pair}");

            if (pair.X > 7 || pair.X < 0 || pair.Y > 7 || pair.Y < 0)
            {
                //Debug.Log($"{piece} at {piece.Tile} to {pair}: Move is out of board bounds");
                return;
            }

            if (context.IsOccupied(pair) == piece.Color)
            {
                //Debug.Log($"{piece} at {piece.Tile} to {pair}: Move is onto a square occupied by the same color piece");
                return;
            }
            
            //Debug.Log($"{piece} at {piece.Tile} to {pair} board: {checkBoard.GetFen()}");

            if (!pruneCheck)
            {
                Board checkBoard = new Board(context.GetFen());
                
                checkBoard.MovePiece(new TilePair(piece.Tile.X, piece.Tile.Y), pair, true);
                
                PieceColor check = checkBoard.IsInCheck();
                //Debug.Log($"Results of IsInCheck for {piece} at {piece.Tile} to {pair}: {check}\n{checkBoard.GetFen()}");
                if (check == PieceColor.Both || check == piece.Color)
                {
                    //Debug.Log($"{piece} at {piece.Tile} to {pair}: Move puts the player in check");
                    return;
                }
            }

            pairs.Add(pair);
        }

        public static void ChangeTurn()
        {
            if (PlayerTurn == PieceColor.White)
            {
                PlayerTurn = PieceColor.Black;
            }
            else
            {
                PlayerTurn = PieceColor.White;
            }
        }
    }
}
