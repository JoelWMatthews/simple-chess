namespace SimpleChess.Game
{
    public class ChessMove
    {
        public ChessPiece Piece;
        public TilePair Target;

        public ChessMove(ChessPiece piece, TilePair target)
        {
            Piece = piece;
            Target = target;
        }
    }
}
