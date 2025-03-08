using System;
using System.Collections.Generic;

namespace SimpleChess.Game
{
    public class KingPiece : ChessPiece
    {
        public KingPiece(PieceColor _color, TilePair _tile) : base(_color, _tile)
        {
        }

        public override PieceType Type => PieceType.King;
        public override char GetFen()
        {
            String fen = "k";
            if (Color == PieceColor.White)
            {
                fen = fen.ToUpper();
            }

            return fen.ToCharArray()[0];
        }

        public override List<TilePair> GetMoves(ChessBoard context, bool pruneCheck = false)
        {
            List<TilePair> moves = new List<TilePair>();

            // Standard king moves
            if (Tile.X < 7)
            {
                ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X + 1, Tile.Y), context, this, pruneCheck);
            }

            if (Tile.X > 0)
            {
                ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X - 1, Tile.Y), context, this, pruneCheck);
            }

            if (Tile.Y < 7)
            {
                ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X, Tile.Y + 1), context, this, pruneCheck);
            }

            if (Tile.Y > 0)
            {
                ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X, Tile.Y - 1), context, this, pruneCheck);
            }

            // Castling moves
            if (context.CanCastle(Color, true))
            {
                if (context.IsOccupied(new TilePair(Tile.X + 1, Tile.Y)) == PieceColor.None &&
                    context.IsOccupied(new TilePair(Tile.X + 2, Tile.Y)) == PieceColor.None)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X + 2, Tile.Y), context, this, pruneCheck);
                }
            }
            if (context.CanCastle(Color, false))
            {
                if (context.IsOccupied(new TilePair(Tile.X - 1, Tile.Y)) == PieceColor.None &&
                    context.IsOccupied(new TilePair(Tile.X - 2, Tile.Y)) == PieceColor.None &&
                    context.IsOccupied(new TilePair(Tile.X - 3, Tile.Y)) == PieceColor.None)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X - 2, Tile.Y), context, this, pruneCheck);
                }
            }

            return moves;
        }

        public override void OnMove(ChessBoard context)
        {
            base.OnMove(context);
            context.DisableCastling(Color, true);
            context.DisableCastling(Color, false);
        }
    }
}