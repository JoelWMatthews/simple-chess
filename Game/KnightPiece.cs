using System;
using System.Collections.Generic;

namespace SimpleChess.Game
{
    public class KnightPiece : ChessPiece
    {
        public KnightPiece(PieceColor _color, TilePair _tile) : base(_color, _tile)
        {
        }

        public override PieceType Type => PieceType.Knight;
        
        public override char GetFen()
        {
            String fen = "n";
            if (Color == PieceColor.White)
            {
                fen = fen.ToUpper();
            }

            return fen.ToCharArray()[0];
        }

        public override List<TilePair> GetMoves(ChessBoard context, bool pruneCheck = false)
        {
            List<TilePair> moves = new List<TilePair>();

            if (Tile.Y <= 5)
            {
                if (Tile.X >= 1)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X - 1, Tile.Y + 2), context, this, pruneCheck);
                }

                if (Tile.X <= 6)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X + 1, Tile.Y + 2), context, this, pruneCheck);
                }
            }

            if (Tile.Y >= 2)
            {
                if (Tile.X >= 1)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X - 1, Tile.Y - 2), context, this, pruneCheck);
                }

                if (Tile.X <= 6)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X + 1, Tile.Y - 2), context, this, pruneCheck);
                }
            }

            if (Tile.X <= 5)
            {
                if (Tile.Y >= 1)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X + 2, Tile.Y - 1), context, this, pruneCheck);
                }

                if (Tile.Y <= 6)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X + 2, Tile.Y + 1), context, this, pruneCheck);
                }
            }
            
            if (Tile.X >= 2)
            {
                if (Tile.Y >= 1)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X - 2, Tile.Y - 1), context, this, pruneCheck);
                }

                if (Tile.Y <= 6)
                {
                    ChessBoardUtilities.AddMove(moves, new TilePair(Tile.X - 2, Tile.Y + 1), context, this, pruneCheck);
                }
            }

            return moves;
        }
    }
}