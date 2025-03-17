using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace SimpleChess.Game
{
    public class ChessBoard
    {
        public static readonly String StartingPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 0";

        public ChessPiece[][] Layout;

        public PieceColor PlayerTurn = PieceColor.White;
        public PieceColor Win = PieceColor.None;

        public bool WhiteCanCastleKingside = true;
        public bool WhiteCanCastleQueenside = true;
        public bool BlackCanCastleKingside = true;
        public bool BlackCanCastleQueenside = true;

        public TilePair EnPassantTile = null;
        public TilePair EnPassantQueue = null;

        public int HalfMoveClock = 0;
        public int FullMoveClock = 0;

        public bool PieceWasCaptured = false;

        public PieceColor AIControlled = PieceColor.None;

        public List<ChessBoardListener> Listeners = new List<ChessBoardListener>();

        public ChessBoard()
        {
            SetFromString(StartingPosition);
        }

        public ChessBoard(String fen)
        {
            SetFromString(fen);
        }

        public void ClearBoard()
        {
            Layout = new ChessPiece[8][];
            for (int i = 0; i < Layout.Length; i++)
            {
                Layout[i] = new ChessPiece[8];
            }
        }

        public void SetFromString(String fen)
        {
            ClearBoard();

            String[] parts = fen.Split(' ');
            char[] newBoard = parts[0].ToCharArray();

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
                    Layout[x][y] = ChessPieceFactory.GetPieceFromFen(newBoard[i], new TilePair(x, y));
                    x++;
                    continue;
                }

                if (char.IsNumber(newBoard[i]))
                {
                    x += int.Parse(newBoard[i].ToString());
                    continue;
                }
            }

            this.SetTurnFromChar(parts[1].ToCharArray()[0]);

            this.SetCastlingRules(parts[2]);

            this.SetEnPassantTile(parts[3]);

            if (parts.Length < 5)
            {
                this.HalfMoveClock = 0;
                this.FullMoveClock = 0;
                return;
            }

            try
            {
                this.HalfMoveClock = int.Parse(parts[4]);
            }
            catch (Exception e)
            {
                this.HalfMoveClock = 0;
            }

            try
            {
                this.FullMoveClock = int.Parse(parts[5]);
            }
            catch (Exception e)
            {
                this.FullMoveClock = 0;
            }
        }

        public String GetFen()
        {
            StringBuilder fen = new StringBuilder();
            for (int y = 7; y >= 0; y--) // Start from rank 8 (y = 7) to rank 1 (y = 0)
            {
                int emptySquare = 0;
                for (int x = 0; x < 8; x++) // File 'a' to file 'h'
                {
                    if (Layout[x][y] == null) // Empty square
                    {
                        emptySquare++;
                    }
                    else
                    {
                        if (emptySquare > 0)
                        {
                            fen.Append(emptySquare);
                            emptySquare = 0;
                        }
                        fen.Append(Layout[x][y].GetFen()); // Append piece FEN notation
                    }
                }
                if (emptySquare > 0)
                {
                    fen.Append(emptySquare); // Append remaining empty squares
                }
                if (y > 0)
                {
                    fen.Append("/"); // Add rank separator
                }
            }

            fen.Append(" "); // Add space to separate board from turn
            
            if (PlayerTurn == PieceColor.White)
            {
                fen.Append("w ");
            }
            else
            {
                fen.Append("b ");
            }

            String castlingString = "";
            if (WhiteCanCastleKingside)
            {
                castlingString += "K";
            }
            if (WhiteCanCastleQueenside)
            {
                castlingString += "Q";
            }
            if (BlackCanCastleKingside)
            {
                castlingString += "k";
            }
            if (BlackCanCastleQueenside)
            {
                castlingString += "q";
            }
            fen.Append(castlingString);

            fen.Append(" ");

            fen.Append(EnPassantTile == null ? "-" : EnPassantTile.ToString());

            fen.Append(" ");
            fen.Append(HalfMoveClock.ToString());
            fen.Append(" ");
            fen.Append(FullMoveClock.ToString());

            return fen.ToString();
        }

        public List<ChessMove> GetAllPossibleMoves(bool pruneCheck = false)
        {
            List<ChessMove> allMoves = new List<ChessMove>();

            foreach (var line in Layout)
            {
                foreach (var piece in line)
                {
                    if (Equals(null, piece)) continue;

                    List<TilePair> moves = piece.GetMoves(this, pruneCheck);
                    foreach (var move in moves)
                    {
                        allMoves.Add(new ChessMove(piece, move));
                    }
                }
            }

            return allMoves;
        }

        public List<ChessMove> GetAllPossibleMoves(PieceColor color, bool pruneCheck = false)
        {
            List<ChessMove> colorMoves = new List<ChessMove>();

            List<ChessMove> moves = GetAllPossibleMoves(pruneCheck);

            foreach (var move in moves)
            {
                if (move.Piece.Color == color)
                {
                    colorMoves.Add(move);
                }
            }

            return colorMoves;
        }

        public void MovePiece(TilePair original, TilePair destination, bool force = false)
        {
            if (this.IsOccupied(original) == PieceColor.None)
            {
                return;
            }

            if (original.Y == destination.Y && original.X == destination.X)
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

            // Handle castling
            if (Layout[original.X][original.Y].Type == PieceType.King && Math.Abs(original.X - destination.X) == 2)
            {
                bool kingside = destination.X > original.X;
                Castle(Layout[original.X][original.Y].Color, kingside);
                return;
            }

            //En Passant
            if (Layout[original.X][original.Y].Type == PieceType.Pawn && Math.Abs(original.Y - destination.Y) == 2)
            {
                if (Layout[original.X][original.Y].Color == PieceColor.Black)
                {
                    EnPassantQueue = new TilePair(destination.X, destination.Y + 1);
                }
                else
                {
                    EnPassantQueue = new TilePair(destination.X, destination.Y - 1);
                }
            }

            if (Layout[destination.X][destination.Y] != null)
            {
                Layout[destination.X][destination.Y].OnCapture(this);
            }

            Layout[destination.X][destination.Y] = Layout[original.X][original.Y];
            Layout[original.X][original.Y] = null;
            Layout[destination.X][destination.Y].Tile = new TilePair(destination.X, destination.Y);

            Layout[destination.X][destination.Y].OnMove(this);

            //Check En Passant
            if (EnPassantTile != null && destination.ToString().Equals(EnPassantTile.ToString()))
            {
                if (EnPassantTile.Y == 2)
                {
                    Layout[EnPassantTile.X][EnPassantTile.Y + 1].OnCapture(this);
                    Layout[EnPassantTile.X][EnPassantTile.Y + 1] = null;
                }
                else
                {
                    Layout[EnPassantTile.X][EnPassantTile.Y - 1].OnCapture(this);
                    Layout[EnPassantTile.X][EnPassantTile.Y - 1] = null;
                }
            }

            if (PieceWasCaptured || Layout[destination.X][destination.Y].Type == PieceType.Pawn)
            {
                HalfMoveClock = 0;
                PieceWasCaptured = false;
            }
            else
            {
                HalfMoveClock++;
            }

            FullMoveClock++;

            if (!force)
            {
                this.ChangeTurn();
                Win = this.CheckForWin();
            }
        }
        public void Castle(PieceColor color, bool kingside)
        {
            int y = (color == PieceColor.Black) ? 7 : 0; // White pieces are on row 7, black pieces are on row 0
            int kingX = 4;
            int rookX = kingside ? 7 : 0;
            int newKingX = kingside ? 6 : 2;
            int newRookX = kingside ? 5 : 3;

            // Move the king
            Layout[newKingX][y] = Layout[kingX][y];
            Layout[kingX][y] = null;
            Layout[newKingX][y].Tile = new TilePair(newKingX, y);

            // Move the rook
            Layout[newRookX][y] = Layout[rookX][y];
            Layout[rookX][y] = null;
            Layout[newRookX][y].Tile = new TilePair(newRookX, y);

            // Update castling rights
            this.DisableCastling(color, true);
            this.DisableCastling(color, false);

            // Change turn
            this.ChangeTurn();
        }
    }
}
