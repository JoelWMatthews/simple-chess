using System;
using System.Collections.Generic;

namespace SimpleChess.Game
{
    public class PawnPiece : ChessPiece
    {
        public PawnPiece(PieceColor _color, TilePair _tile) : base(_color, _tile)
        {
        }

        public override PieceType Type => PieceType.Pawn;
        public override char GetFen()
        {
            String fen = "p";
            if (Color == PieceColor.White)
            {
                fen = fen.ToUpper();
            }

            return fen.ToCharArray()[0];
        }

        public override List<TilePair> GetMoves(ChessBoard context, bool pruneCheck = false)
        {
            List<TilePair> moves = new List<TilePair>();

            int offset = 1;
            if (Color == PieceColor.Black) offset *= -1;

            bool HasMoved = (Tile.Y != 1 && Color == PieceColor.White) || (Tile.Y != 6 && Color == PieceColor.Black);

            if (context.IsOccupied(new TilePair(Tile.X, Tile.Y+offset)) == PieceColor.None)
            {
                ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X, Tile.Y + offset), context, this, pruneCheck);
                
                if (!HasMoved && context.IsOccupied(new TilePair(Tile.X, Tile.Y+offset+offset)) == PieceColor.None)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X, Tile.Y + offset + offset), context, this, pruneCheck);
                }
            }

            if (Tile.X != 0)
            {
                PieceColor check = context.IsOccupied(new TilePair(Tile.X-1, Tile.Y+offset));
                if (check != PieceColor.None && check != Color)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X - 1, Tile.Y + offset), context, this, pruneCheck);
                }
                else if (context.EnPassantTile != null && TilePair.CoordinatesToString(Tile.X - 1, Tile.Y + offset).Equals(context.EnPassantTile.ToString()))
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X - 1, Tile.Y + offset), context, this, pruneCheck);
                }
            }

            if (Tile.X != 7)
            {
                PieceColor check = context.IsOccupied(new TilePair(Tile.X+1, Tile.Y+offset));
                if (check != PieceColor.None && check != Color)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X + 1, Tile.Y + offset), context, this, pruneCheck);
                }
                else if (context.EnPassantTile != null && TilePair.CoordinatesToString(Tile.X + 1, Tile.Y + offset).Equals(context.EnPassantTile.ToString()))
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X + 1, Tile.Y + offset), context, this, pruneCheck);
                }
            }
            return moves;
        }

        public override void OnMove(ChessBoard context)
        {
            base.OnMove(context);

            // TODO En Passant Logic
        }
    }
}