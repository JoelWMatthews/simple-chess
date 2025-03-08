using System;
using System.Collections.Generic;

namespace SimpleChess.Game
{
    public abstract class ChessPiece
    {
        public abstract PieceType Type { get; }
        public PieceColor Color { get; private set; }

        public TilePair Tile;

        public ChessPiece(PieceColor _color, TilePair _tile)
        {
            Color = _color;
            Tile = _tile;
        }

        public override string ToString()
        {
            return Color + " " + Type;
        }

        public abstract char GetFen();

        public abstract List<TilePair> GetMoves(ChessBoard context, bool pruneCheck = false);

        public virtual void OnMove(ChessBoard context) { }

        public virtual void OnCapture(ChessBoard context) { context.PieceWasCaptured = true; }
    }

    public enum PieceType
    {
        None,
        Pawn,
        Knight,
        Rook,
        Bishop,
        Queen,
        King
    }

    public enum PieceColor
    {
        White,
        Black,
        None,
        Both
    }
    
    public class TilePair
    {
        public int Y;
        public int X;

        public TilePair(int x, int y)
        {
            Y = y;
            X = x;
        }

        public TilePair(String tile)
        {
            X = tile[0] - 'A';
            Y = int.Parse(tile[1].ToString()) - 1;
        }

        public static String CoordinatesToString(int x, int y)
        {
            String result = "";

            switch (x)
            {
                case 0:
                    result = "A";
                    break;
                case 1:
                    result = "B";
                    break;
                case 2:
                    result = "C";
                    break;
                case 3:
                    result = "D";
                    break;
                case 4:
                    result = "E";
                    break;
                case 5:
                    result = "F";
                    break;
                case 6:
                    result = "G";
                    break;
                case 7:
                    result = "H";
                    break;
            }

            result += (y + 1).ToString();
            return result;
        }
        
        public override string ToString()
        {
            String result = "";

            switch (X)
            {
                case 0:
                    result = "A";
                    break;
                case 1:
                    result = "B";
                    break;
                case 2:
                    result = "C";
                    break;
                case 3:
                    result = "D";
                    break;
                case 4:
                    result = "E";
                    break;
                case 5:
                    result = "F";
                    break;
                case 6:
                    result = "G";
                    break;
                case 7:
                    result = "H";
                    break;
            }

            result += (Y + 1).ToString();
            return result;
        }
    }
}