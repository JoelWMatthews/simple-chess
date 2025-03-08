using System;
using System.Collections.Generic;

namespace SimpleChess.Game
{
    public class BishopPiece : ChessPiece
    {
        public BishopPiece(PieceColor _color, TilePair _tile) : base(_color, _tile)
        {
        }

        public override PieceType Type => PieceType.Bishop;
        public override char GetFen()
        {
            String fen = "b";
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
                ChessBoardUtilities.AddMove(moves, new TilePair(newX, newY), context, this, pruneCheck);
                if (context.IsOccupied(new TilePair(newX, newY)) != PieceColor.None)
                    break;
            }

            // Move diagonally up and left
            for (int newX = Tile.X - 1, newY = Tile.Y + 1; newX >= 0 && newY < 8; newX--, newY++)
            {
                ChessBoardUtilities.AddMove(moves, new TilePair(newX, newY), context, this, pruneCheck);
                if (context.IsOccupied(new TilePair(newX, newY)) != PieceColor.None)
                    break;
            }

            // Move diagonally down and right
            for (int newX = Tile.X + 1, newY = Tile.Y - 1; newX < 8 && newY >= 0; newX++, newY--)
            {
                ChessBoardUtilities.AddMove(moves, new TilePair(newX, newY), context, this, pruneCheck);
                if (context.IsOccupied(new TilePair(newX, newY)) != PieceColor.None)
                    break;
            }

            // Move diagonally down and left
            for (int newX = Tile.X - 1, newY = Tile.Y - 1; newX >= 0 && newY >= 0; newX--, newY--)
            {
                ChessBoardUtilities.AddMove(moves, new TilePair(newX, newY), context, this, pruneCheck);
                if (context.IsOccupied(new TilePair(newX, newY)) != PieceColor.None)
                    break;
            }

            return moves;
        }
    }
}