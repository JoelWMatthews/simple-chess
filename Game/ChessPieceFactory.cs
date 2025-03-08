namespace SimpleChess.Game
{
    public static class ChessPieceFactory
    {
        public static ChessPiece GetPieceFromFen(char fen, TilePair tile)
        {
            PieceColor color = char.IsUpper(fen) ? PieceColor.White : PieceColor.Black;

            switch (fen.ToString().ToUpper())
            {
                case "R":
                    return new RookPiece(color, tile);
                case "N":
                    return new KnightPiece(color, tile);
                case "B":
                    return new BishopPiece(color, tile);
                case "Q":
                    return new QueenPiece(color, tile);
                case "K":
                    return new KingPiece(color, tile);
                default:
                    return new PawnPiece(color, tile);
            }
        }
    }
}
