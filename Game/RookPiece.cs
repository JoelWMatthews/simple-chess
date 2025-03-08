using System;
using System.Collections.Generic;

namespace SimpleChess.Game
{
    public class RookPiece : ChessPiece
    {
        public override PieceType Type => PieceType.Rook;

        private PieceType CastleSide = PieceType.None;

        public RookPiece(PieceColor _color, TilePair _tile) : base(_color, _tile)
        {
            switch (_color)
            {
                case PieceColor.White:

                    if (Tile.X == 0 && Tile.Y == 7)
                    {
                        CastleSide = PieceType.Queen;
                    } 
                    else if (Tile.X == 7 && Tile.Y == 7)
                    {
                        CastleSide = PieceType.King;
                    }

                    break;

                case PieceColor.Black:

                    if (Tile.X == 0 && Tile.Y == 0)
                    {
                        CastleSide = PieceType.Queen;
                    }
                    else if (Tile.X == 7 && Tile.Y == 0)
                    {
                        CastleSide = PieceType.King;
                    }

                    break;
            }
        }

        public override char GetFen()
        {
            String fen = "r";
            if (Color == PieceColor.White)
            {
                fen = fen.ToUpper();
            }

            return fen.ToCharArray()[0];
        }

        public override List<TilePair> GetMoves(ChessBoard context, bool pruneCheck = false)
        {
            List<TilePair> moves = new List<TilePair>();

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

        public override void OnMove(ChessBoard context)
        {
            base.OnMove(context);
            context.DisableCastling(Color, CastleSide == PieceType.King);
        }

        public override void OnCapture(ChessBoard context)
        {
            base.OnCapture(context);
            context.DisableCastling(Color, CastleSide == PieceType.King);
        }
    }
}