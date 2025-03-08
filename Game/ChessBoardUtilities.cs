using System.Collections.Generic;
using System.IO;

namespace SimpleChess.Game
{
    public static class ChessBoardUtilities
    {
        public static void AddMove(List<TilePair> pairs, TilePair pair, ChessBoard context, ChessPiece piece, bool pruneCheck)
        {
            if (pair.X is > 7 or < 0 || pair.Y is > 7 or < 0)
            {
                return;
            }

            if (context.IsOccupied(pair) == piece.Color)
            {
                return;
            }

            if (!pruneCheck)
            {
                ChessBoard checkBoard = new ChessBoard(context.GetFen());

                checkBoard.MovePiece(new TilePair(piece.Tile.X, piece.Tile.Y), pair, true);

                PieceColor check = checkBoard.IsInCheck();
                if (check == PieceColor.Both || check == piece.Color)
                {
                    return;
                }
            }

            pairs.Add(pair);
        }

        public static PieceColor IsInCheck(this ChessBoard board)
        {
            bool whiteCheck = false;
            bool blackCheck = false;

            List<ChessMove> allMoves = board.GetAllPossibleMoves(true);

            var sr = File.CreateText("Output.txt");

            foreach (var move in allMoves)
            {
                sr.WriteLine(move.Piece + " at " + move.Piece.Tile + " to " + move.Target);
            }
            
            sr.Close();

            foreach (var move in allMoves)
            {
                if (Equals(null, board.Layout[move.Target.X][move.Target.Y])) continue;
                if (board.Layout[move.Target.X][move.Target.Y].Type == PieceType.King)
                {
                    switch (board.Layout[move.Target.X][move.Target.Y].Color)
                    {
                        case PieceColor.White:
                            whiteCheck = true;
                            break;
                        case PieceColor.Black:
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

        public static PieceColor IsOccupied(this ChessBoard board, TilePair tile)
        {
            if (tile.X > 7 || tile.X < 0 || tile.Y > 7 || tile.Y < 0) return PieceColor.None;
            if (Equals(null, board.Layout[tile.X][tile.Y]))
            {
                return PieceColor.None;
            }

            return board.Layout[tile.X][tile.Y].Color;
        }

        public static PieceColor CheckForWin(this ChessBoard board)
        {
            PieceColor colorCheck = board.IsInCheck();

            if (colorCheck == PieceColor.None) return PieceColor.None;

            switch (colorCheck)
            {
                case PieceColor.Black:
                    List<ChessMove> blackMoves = board.GetAllPossibleMoves(PieceColor.Black);
                    if (blackMoves.Count == 0)
                    {
                        return PieceColor.White;
                    }

                    break;
                case PieceColor.White:
                    List<ChessMove> whiteMoves = board.GetAllPossibleMoves(PieceColor.White);
                    if (whiteMoves.Count == 0)
                    {
                        return PieceColor.Black;
                    }

                    break;
            }

            if (board.HalfMoveClock >= 50)
            {
                return PieceColor.Both;
            }

            return PieceColor.None;
        }

        public static void SetTurnFromChar(this ChessBoard board, char turn)
        {
            switch (turn)
            {
                case 'w':
                    board.PlayerTurn = PieceColor.White;
                    break;
                case 'b':
                    board.PlayerTurn = PieceColor.Black;
                    break;
            }
        }

        public static void ChangeTurn(this ChessBoard board)
        {
            if (board.PlayerTurn == PieceColor.White)
            {
                board.PlayerTurn = PieceColor.Black;
            }
            else
            {
                board.PlayerTurn = PieceColor.White;
            }

            board.EnPassantTile = board.EnPassantQueue;
            board.EnPassantQueue = null;
        }

        public static void SetCastlingRules(this ChessBoard board, string castling)
        {
            board.WhiteCanCastleKingside = false;
            board.WhiteCanCastleQueenside = false;
            board.BlackCanCastleKingside = false;
            board.BlackCanCastleQueenside = false;

            if (castling.Contains("K"))
            {
                board.WhiteCanCastleKingside = true;
            }
            if (castling.Contains("Q"))
            {
                board.WhiteCanCastleQueenside = true;
            }
            if (castling.Contains("k"))
            {
                board.BlackCanCastleKingside = true;
            }
            if (castling.Contains("q"))
            {
                board.BlackCanCastleQueenside = true;
            }
        }

        public static bool CanCastle(this ChessBoard board, PieceColor color, bool kingside)
        {
            if (color == PieceColor.White)
            {
                if (kingside) return board.WhiteCanCastleKingside;
                else return board.WhiteCanCastleQueenside;
            }
            else
            {
                if (kingside) return board.BlackCanCastleKingside;
                else return board.BlackCanCastleQueenside;
            }
        }

        public static void DisableCastling(this ChessBoard board, PieceColor color, bool kingside)
        {
            switch (color)
            {
                case PieceColor.White:
                    if (kingside) board.WhiteCanCastleKingside = false;
                    else board.WhiteCanCastleQueenside = false;
                    break;
                case PieceColor.Black:
                    if (kingside) board.BlackCanCastleKingside = false;
                    else board.BlackCanCastleQueenside = false;
                    break;
            }
        }

        public static void SetEnPassantTile(this ChessBoard board, TilePair tile)
        {
            board.EnPassantTile = tile;
        }

        public static void SetEnPassantTile(this ChessBoard board, string tile)
        {
            if (tile == "-")
            {
                board.EnPassantTile = null;
                return;
            }
            board.EnPassantTile = new TilePair(tile);
        }

    }
}
