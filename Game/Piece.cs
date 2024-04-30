using System;
using System.Collections.Generic;

namespace SimpleChess.Game
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public PieceColor Color { get; private set; }

        public bool HasMoved = false;

        public TilePair Tile;

        public Piece(PieceColor _color)
        {
            Color = _color;
        }

        public override string ToString()
        {
            return Color + " " + Type;
        }

        public abstract char GetFen();

        public abstract List<TilePair> GetMoves(Board context, bool pruneCheck = false);
    }
    public class RookPiece : Piece
    {
        public override PieceType Type => PieceType.Rook;

        public RookPiece(PieceColor _color) : base(_color)
        {
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

        public override List<TilePair> GetMoves(Board context, bool pruneCheck = false)
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
                    moves.AddMove(new TilePair(Tile.X, up), context, this, pruneCheck);
                }
                else
                {
                    moves.AddMove(new TilePair(Tile.X, up), context, this, pruneCheck);
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
                    moves.AddMove(new TilePair(Tile.X, down), context, this, pruneCheck);
                }
                else
                {
                    moves.AddMove(new TilePair(Tile.X, down), context, this, pruneCheck);
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
                    moves.AddMove(new TilePair(left, Tile.Y), context, this, pruneCheck);
                }
                else
                {
                    moves.AddMove(new TilePair(left, Tile.Y), context, this, pruneCheck);
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
                    moves.AddMove(new TilePair(right, Tile.Y), context, this, pruneCheck);
                }
                else
                {
                    moves.AddMove(new TilePair(right, Tile.Y), context, this, pruneCheck);
                    right = 8;
                }
            }

            return moves;
        }
    }

    public class KnightPiece : Piece
    {
        public KnightPiece(PieceColor _color) : base(_color)
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

        public override List<TilePair> GetMoves(Board context, bool pruneCheck = false)
        {
            List<TilePair> moves = new List<TilePair>();

            if (Tile.Y <= 5)
            {
                if (Tile.X >= 1)
                {
                    moves.AddMove(new TilePair(Tile.X - 1,Tile.Y + 2), context, this, pruneCheck);
                }

                if (Tile.X <= 6)
                {
                    moves.AddMove(new TilePair(Tile.X + 1, Tile.Y + 2), context, this, pruneCheck);
                }
            }

            if (Tile.Y >= 2)
            {
                if (Tile.X >= 1)
                {
                    moves.AddMove(new TilePair(Tile.X - 1, Tile.Y - 2), context, this, pruneCheck);
                }

                if (Tile.X <= 6)
                {
                    moves.AddMove(new TilePair(Tile.X + 1, Tile.Y - 2), context, this, pruneCheck);
                }
            }

            if (Tile.X <= 5)
            {
                if (Tile.Y >= 1)
                {
                    moves.AddMove(new TilePair(Tile.X + 2, Tile.Y - 1), context, this, pruneCheck);
                }

                if (Tile.Y <= 6)
                {
                    moves.AddMove(new TilePair(Tile.X + 2, Tile.Y + 1), context, this, pruneCheck);
                }
            }
            
            if (Tile.X >= 2)
            {
                if (Tile.Y >= 1)
                {
                    moves.AddMove(new TilePair(Tile.X - 2, Tile.Y - 1), context, this, pruneCheck);
                }

                if (Tile.Y <= 6)
                {
                    moves.AddMove(new TilePair(Tile.X - 2, Tile.Y + 1), context, this, pruneCheck);
                }
            }

            return moves;
        }
    }

    public class BishopPiece : Piece
    {
        public BishopPiece(PieceColor _color) : base(_color)
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

        public override List<TilePair> GetMoves(Board context, bool pruneCheck = false)
        {
            List<TilePair> moves = new List<TilePair>();

            // Move diagonally up and right
            for (int newX = Tile.X + 1, newY = Tile.Y + 1; newX < 8 && newY < 8; newX++, newY++)
            {
                moves.AddMove(new TilePair(newX, newY), context, this, pruneCheck);
                if (context.IsOccupied(new TilePair(newX, newY)) != PieceColor.None)
                    break;
            }

            // Move diagonally up and left
            for (int newX = Tile.X - 1, newY = Tile.Y + 1; newX >= 0 && newY < 8; newX--, newY++)
            {
                moves.AddMove(new TilePair(newX, newY), context, this, pruneCheck);
                if (context.IsOccupied(new TilePair(newX, newY)) != PieceColor.None)
                    break;
            }

            // Move diagonally down and right
            for (int newX = Tile.X + 1, newY = Tile.Y - 1; newX < 8 && newY >= 0; newX++, newY--)
            {
                moves.AddMove(new TilePair(newX, newY), context, this, pruneCheck);
                if (context.IsOccupied(new TilePair(newX, newY)) != PieceColor.None)
                    break;
            }

            // Move diagonally down and left
            for (int newX = Tile.X - 1, newY = Tile.Y - 1; newX >= 0 && newY >= 0; newX--, newY--)
            {
                moves.AddMove(new TilePair(newX, newY), context, this, pruneCheck);
                if (context.IsOccupied(new TilePair(newX, newY)) != PieceColor.None)
                    break;
            }

            return moves;
        }
    }

    public class KingPiece : Piece
    {
        public KingPiece(PieceColor _color) : base(_color)
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

        public override List<TilePair> GetMoves(Board context, bool pruneCheck = false)
        {
            List<TilePair> moves = new List<TilePair>();

            if (Tile.X < 7)
            {
                moves.AddMove(new TilePair(Tile.X+1, Tile.Y), context, this, pruneCheck);
            }

            if (Tile.X > 0)
            {
                moves.AddMove(new TilePair(Tile.X-1, Tile.Y), context, this, pruneCheck);
            }

            if (Tile.Y < 7)
            {
                moves.AddMove(new TilePair(Tile.X, Tile.Y+1), context, this, pruneCheck);
            }

            if (Tile.Y > 0)
            {
                moves.AddMove(new TilePair(Tile.X, Tile.Y-1), context, this, pruneCheck);
            }

            return moves;
        }
    }

    public class QueenPiece : Piece
    {
        public QueenPiece(PieceColor _color) : base(_color)
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

        public override List<TilePair> GetMoves(Board context, bool pruneCheck = false)
        {
            List<TilePair> moves = new List<TilePair>();

            // Move diagonally up and right
            for (int newX = Tile.X + 1, newY = Tile.Y + 1; newX < 8 && newY < 8; newX++, newY++)
            {
                if (context.IsOccupied(new TilePair(newX, newY)) != PieceColor.None)
                {
                    if (context.IsOccupied(new TilePair(newX, newY)) != Color)
                    {
                        moves.AddMove(new TilePair(newX, newY), context, this, pruneCheck);
                    }

                    break;
                }
                moves.AddMove(new TilePair(newX, newY), context, this, pruneCheck);
            }

            // Move diagonally up and left
            for (int newX = Tile.X - 1, newY = Tile.Y + 1; newX >= 0 && newY < 8; newX--, newY++)
            {
                if (context.IsOccupied(new TilePair(newX, newY)) != PieceColor.None)
                {
                    if (context.IsOccupied(new TilePair(newX, newY)) != Color)
                    {
                        moves.AddMove(new TilePair(newX, newY), context, this, pruneCheck);
                    }

                    break;
                }
                
                moves.AddMove(new TilePair(newX, newY), context, this, pruneCheck);
            }

            // Move diagonally down and right
            for (int newX = Tile.X + 1, newY = Tile.Y - 1; newX < 8 && newY >= 0; newX++, newY--)
            {
                if (context.IsOccupied(new TilePair(newX, newY)) != PieceColor.None)
                {
                    if (context.IsOccupied(new TilePair(newX, newY)) != Color)
                    {
                        moves.AddMove(new TilePair(newX, newY), context, this, pruneCheck);
                    }

                    break;
                }
                moves.AddMove(new TilePair(newX, newY), context, this, pruneCheck);
            }

            // Move diagonally down and left
            for (int newX = Tile.X - 1, newY = Tile.Y - 1; newX >= 0 && newY >= 0; newX--, newY--)
            {
                if (context.IsOccupied(new TilePair(newX, newY)) != PieceColor.None)
                {
                    if (context.IsOccupied(new TilePair(newX, newY)) != Color)
                    {
                        moves.AddMove(new TilePair(newX, newY), context, this, pruneCheck);
                    }

                    break;
                }
                moves.AddMove(new TilePair(newX, newY), context, this, pruneCheck);
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
                    moves.AddMove(new TilePair(Tile.X, up), context, this, pruneCheck);
                }
                else
                {
                    moves.AddMove(new TilePair(Tile.X, up), context, this, pruneCheck);
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
                    moves.AddMove(new TilePair(Tile.X, down), context, this, pruneCheck);
                }
                else
                {
                    moves.AddMove(new TilePair(Tile.X, down), context, this, pruneCheck);
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
                    moves.AddMove(new TilePair(left, Tile.Y), context, this, pruneCheck);
                }
                else
                {
                    moves.AddMove(new TilePair(left, Tile.Y), context, this, pruneCheck);
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
                    moves.AddMove(new TilePair(right, Tile.Y), context, this, pruneCheck);
                }
                else
                {
                    moves.AddMove(new TilePair(right, Tile.Y), context, this, pruneCheck);
                    right = 8;
                }
            }

            return moves;
        }
    }

    public class PawnPiece : Piece
    {
        public PawnPiece(PieceColor _color) : base(_color)
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

        public override List<TilePair> GetMoves(Board context, bool pruneCheck = false)
        {
            List<TilePair> moves = new List<TilePair>();

            int offset = 1;
            if (Color == PieceColor.Black) offset *= -1;

            if (context.IsOccupied(new TilePair(Tile.X, Tile.Y+offset)) == PieceColor.None)
            {
                moves.AddMove(new TilePair(Tile.X, Tile.Y+offset), context, this, pruneCheck);
                
                if (!HasMoved && context.IsOccupied(new TilePair(Tile.X, Tile.Y+offset+offset)) == PieceColor.None)
                {
                    moves.AddMove(new TilePair(Tile.X, Tile.Y+offset+offset), context, this, pruneCheck);
                }
            }

            if (Tile.X != 0)
            {
                PieceColor check = context.IsOccupied(new TilePair(Tile.X-1, Tile.Y+offset));
                if (check != PieceColor.None && check != Color)
                {
                    moves.AddMove(new TilePair(Tile.X-1, Tile.Y+offset), context, this, pruneCheck);
                }
            }

            if (Tile.X != 7)
            {
                PieceColor check = context.IsOccupied(new TilePair(Tile.X+1, Tile.Y+offset));
                if (check != PieceColor.None && check != Color)
                {
                    moves.AddMove(new TilePair(Tile.X+1, Tile.Y+offset), context, this, pruneCheck);
                }
            }
            return moves;
        }
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