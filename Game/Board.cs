using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace SimpleChess.Game
{
    public class Board
    {
        public Piece[][] Layout;

        private Piece _cachedDestination;
        private Piece _cachedOriginal;

        public Board()
        {
            
        }

        public Board(String fen)
        {
            SetFromString(fen);
        }
        public void ClearBoard()
        {
            Layout = new Piece[8][];
            for (int i = 0; i < Layout.Length; i++)
            {
                Layout[i] = new Piece[8];
            }
        }

        public void SetFromString(String fen)
        {
            ClearBoard();
            char[] newBoard = fen.ToCharArray();

            int y = 7;
            int x = 0;

            for (int i = 0; i < newBoard.Length; i++)
            {
                if (newBoard[i].Equals('/'))
                {
                    y--;
                    x = 0;
                    continue;
                }

                if (char.IsLetter(newBoard[i]))
                {
                    Layout[x][y] = GetPieceFromFen(newBoard[i]);
                    Layout[x][y].Tile = new TilePair(x, y);
                    x++;
                    continue;
                }

                if (char.IsNumber(newBoard[i]))
                {
                    x += int.Parse(newBoard[i].ToString());
                    continue;
                }
            }
        }
        
        public String GetFen()
        {
            StringBuilder fen = new StringBuilder();
            for (int y = 0; y < 8; y++)
            {
                int emptySquare = 0;
                for (int x = 0; x < 8; x++)
                {
                    if (Layout[x][y] == null)
                    {
                        emptySquare++;
                    }
                    else
                    {
                        char piece = Layout[x][y].GetFen();
                        if (emptySquare > 0)
                        {
                            fen.Append(emptySquare);
                            emptySquare = 0;
                        }
                        fen.Append(piece);
                    }
                }
                if (emptySquare > 0)
                {
                    fen.Append(emptySquare);
                }
                if (y < 7)
                {
                    fen.Append("/");
                }
            }
            return fen.ToString();
        }
        
        public Piece GetPieceFromFen(char fen)
        {
            PieceColor color;

            color = char.IsUpper(fen) ? PieceColor.White : PieceColor.Black;

            switch (fen.ToString().ToUpper())
            {
                case "R":
                    return new RookPiece(color);
                case "N":
                    return new KnightPiece(color);
                case "B":
                    return new BishopPiece(color);
                case "Q":
                    return new QueenPiece(color);
                case "K":
                    return new KingPiece(color);
                default:
                    return new PawnPiece(color);
            }
        }

        public List<Move> GetAllPossibleMoves(bool pruneCheck = false)
        {
            List<Move> allMoves = new List<Move>();

            foreach (var line in Layout)
            {
                foreach (var piece in line)
                {
                    if (Equals(null, piece)) continue;

                    List<TilePair> moves =  piece.GetMoves(this, pruneCheck);
                    foreach (var move in moves)
                    {
                        allMoves.Add(new Move(piece, move));
                    }
                }
            }

            return allMoves;
        }

        public List<Move> GetAllPossibleMoves(PieceColor color, bool pruneCheck = false)
        {
            List<Move> colorMoves = new List<Move>();

            List<Move> moves = GetAllPossibleMoves(pruneCheck);

            foreach (var move in moves)
            {
                if (move.Piece.Color == color)
                {
                    colorMoves.Add(move);
                }
            }

            return colorMoves;
        }

        public PieceColor IsInCheck()
        {
            bool whiteCheck = false;
            bool blackCheck = false;

            List<Move> allMoves = GetAllPossibleMoves(true);

            var sr = File.CreateText("Output.txt");

            foreach (var move in allMoves)
            {
                sr.WriteLine(move.Piece + " at " + move.Piece.Tile + " to " + move.Target);
            }
            
            sr.Close();

            foreach (var move in allMoves)
            {
                if (Equals(null, Layout[move.Target.X][move.Target.Y])) continue;
                if (Layout[move.Target.X][move.Target.Y].Type == PieceType.King)
                {
                    switch (Layout[move.Target.X][move.Target.Y].Color)
                    {
                        case PieceColor.White:
                            Debug.Log("White is in Check! (Checked by " + move.Piece + " at " + move.Piece.Tile);
                            whiteCheck = true;
                            break;
                        case PieceColor.Black:
                            Debug.Log("Black is in Check! (Checked by " + move.Piece + " at " + move.Piece.Tile);
                            blackCheck = true;
                            break;
                    }
                }
            }

            if (whiteCheck)
            {
                if (blackCheck)
                {
                    return PieceColor.Both;
                }

                return PieceColor.White;
            }

            if (blackCheck)
            {
                return PieceColor.Black;
            }

            return PieceColor.None;
        }

        public PieceColor IsOccupied(TilePair tile)
        {
            if (tile.X > 7 || tile.X < 0 || tile.Y > 7 || tile.Y < 0) return PieceColor.None;
            if (Equals(null,Layout[tile.X][tile.Y]))
            {
                return PieceColor.None;
            }

            return Layout[tile.X][tile.Y].Color;
        }

        public void MovePiece(TilePair original, TilePair destination, bool force = false)
        {

            if (original.Y == destination.Y && original.X == destination.X)
            {
                return;
            }

            if (IsOccupied(original) == PieceColor.None)
            {
                return;
            }

            if (!force)
            {
                List<TilePair> possibleMoves = new List<TilePair>();
                possibleMoves.AddRange(Layout[original.X][original.Y].GetMoves(this));

                bool found = false;
                foreach (var move in possibleMoves)
                {
                    if (move.X == destination.X && move.Y == destination.Y)
                    {
                        found = true;
                    }
                }

                if (!found) return;
            }

            _cachedOriginal = Layout[original.X][original.Y];
            _cachedDestination = Layout[destination.X][destination.Y];
            
            Layout[destination.X][destination.Y] = Layout[original.X][original.Y];
            Layout[original.X][original.Y] = null;
            Layout[destination.X][destination.Y].HasMoved = true;
            Layout[destination.X][destination.Y].Tile = new TilePair(destination.X, destination.Y);
            
            GameManager.ChangeTurn();
            
            if (!force) GameManager.Win = CheckForWin();
        }

        public void UndoForcedMove(TilePair original, TilePair destination)
        {
            Layout[original.X][original.Y] = _cachedOriginal;
            Layout[destination.X][destination.Y] = _cachedDestination;
            GameManager.ChangeTurn();
        }

        public PieceColor CheckForWin()
        {
            PieceColor colorCheck = IsInCheck();

            if (IsInCheck() == PieceColor.None) return PieceColor.None;

            switch (colorCheck)
            {
                case PieceColor.Black:
                    List<Move> blackMoves = GetAllPossibleMoves(PieceColor.Black);
                    if (blackMoves.Count == 0)
                    {
                        return PieceColor.White;
                    }

                    break;
                case PieceColor.White:
                    List<Move> whiteMoves = GetAllPossibleMoves(PieceColor.White);
                    if (whiteMoves.Count == 0)
                    {
                        return PieceColor.Black;
                    }

                    break;
            }

            return PieceColor.None;
        }
    }

    public class Move
    {
        public Piece Piece;
        public TilePair Target;

        public Move(Piece piece, TilePair target)
        {
            Piece = piece;
            Target = target;
        }
    }
}