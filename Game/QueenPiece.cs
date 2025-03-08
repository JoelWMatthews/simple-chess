using System;
using System.Collections.Generic;

namespace SimpleChess.Game
{
    public class QueenPiece : ChessPiece
    {
        public QueenPiece(PieceColor _color, TilePair _tile) : base(_color, _tile)
        {
        }

        public override PieceType Type => PieceType.Queen;
        public override char GetFen()
        {
            String fen = "q";
            if (Color == PieceColor.White)
            {
                fen = fen.ToUpper();
            }

            return fen.ToCharArray()[0];
        }

        public override List<TilePair> GetMoves(ChessBoard context, bool pruneCheck = false)
        {
            List<TilePair> moves = new List<TilePair>();

            // Move diagonally up and right
            for (int newX = Tile.X + 1, newY = Tile.Y + 1; newX < 8 && newY < 8; newX++, newY++)
            {
                if (context.IsOccupied(new TilePair(newX, newY)) != PieceColor.None)
                {
                    if (context.IsOccupied(new TilePair(newX, newY)) != Color)
                    {
                        ChessBoardUtilities.AddMove(moves, new TilePair(newX, newY), context, this, pruneCheck);
                    }

                    break;
                }
                ChessBoardUtilities.AddMove(moves, new TilePair(newX, newY), context, this, pruneCheck);
            }

            // Move diagonally up and left
            for (int newX = Tile.X - 1, newY = Tile.Y + 1; newX >= 0 && newY < 8; newX--, newY++)
            {
                if (context.IsOccupied(new TilePair(newX, newY)) != PieceColor.None)
                {
                    if (context.IsOccupied(new TilePair(newX, newY)) != Color)
                    {
                        ChessBoardUtilities.AddMove(moves, new TilePair(newX, newY), context, this, pruneCheck);
                    }

                    break;
                }
                
                ChessBoardUtilities.AddMove(moves, new TilePair(newX, newY), context, this, pruneCheck);
            }

            // Move diagonally down and right
            for (int newX = Tile.X + 1, newY = Tile.Y - 1; newX < 8 && newY >= 0; newX++, newY--)
            {
                if (context.IsOccupied(new TilePair(newX, newY)) != PieceColor.None)
                {
                    if (context.IsOccupied(new TilePair(newX, newY)) != Color)
                    {
                        ChessBoardUtilities.AddMove(moves, new TilePair(newX, newY), context, this, pruneCheck);
                    }

                    break;
                }
                ChessBoardUtilities.AddMove(moves, new TilePair(newX, newY), context, this, pruneCheck);
            }

            // Move diagonally down and left
            for (int newX = Tile.X - 1, newY = Tile.Y - 1; newX >= 0 && newY >= 0; newX--, newY--)
            {
                if (context.IsOccupied(new TilePair(newX, newY)) != PieceColor.None)
                {
                    if (context.IsOccupied(new TilePair(newX, newY)) != Color)
                    {
                        ChessBoardUtilities.AddMove(moves, new TilePair(newX, newY), context, this, pruneCheck);
                    }

                    break;
                }
                ChessBoardUtilities.AddMove(moves, new TilePair(newX, newY), context, this, pruneCheck);
            }
            
            //Up
            for (int up = Tile.Y + 1; up < 8; up++)
            {
                PieceColor testColor = context.IsOccupied(new TilePair(Tile.X, up));
                if (testColor == Color)
                {
                    up = 8;
                }
                else if (testColor == PieceColor.None)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X, up), context, this, pruneCheck);
                }
                else
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X, up), context, this, pruneCheck);
                    up = 8;
                }
            }
            
            //Down
            for (int down = Tile.Y - 1; down >= 0; down--)
            {
                PieceColor testColor = context.IsOccupied(new TilePair(Tile.X, down));
                if (testColor == Color)
                {
                    down = -1;
                }
                else if (testColor == PieceColor.None)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X, down), context, this, pruneCheck);
                }
                else
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X, down), context, this, pruneCheck);
                    down = -1;
                }
            }
            
            //Left
            for (int left = Tile.X - 1; left >= 0; left--)
            {
                PieceColor testColor = context.IsOccupied(new TilePair(left, Tile.Y));
                if (testColor == Color)
                {
                    left = -1;
                }
                else if (testColor == PieceColor.None)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(left, Tile.Y), context, this, pruneCheck);
                }
                else
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(left, Tile.Y), context, this, pruneCheck);
                    left = -1;
                }
            }
            
            //Right
            for (int right = Tile.X + 1; right < 8; right++)
            {
                PieceColor testColor = context.IsOccupied(new TilePair(right, Tile.Y));
                if (testColor == Color)
                {
                    right = 8;
                }
                else if (testColor == PieceColor.None)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(right, Tile.Y), context, this, pruneCheck);
                }
                else
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(right, Tile.Y), context, this, pruneCheck);
                    right = 8;
                }
            }

            return moves;
        }
    }
}