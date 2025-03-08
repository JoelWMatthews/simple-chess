using System.Collections.Generic;
using SimpleChess.Game;
using UnityEngine;

namespace SimpleChess.Unity
{
    public class DebugComponent : MonoBehaviour
    {
        public void PrintAllMoves()
        {
            List<ChessMove> moves = GameManager.Instance.GameBoard.GetAllPossibleMoves();

            foreach (var move in moves)
            {
                Debug.Log($"{move.Piece} at {move.Piece.Tile} to {move.Target}");
            }
        }

        public void PrintAllWhiteMoves()
        {
            List<ChessMove> moves = GameManager.Instance.GameBoard.GetAllPossibleMoves(PieceColor.White, false);

            foreach (var move in moves)
            {
                Debug.Log($"{move.Piece} at {move.Piece.Tile} to {move.Target}");
            }
        }
        
        public void PrintAllBlackMoves()
        {
            List<ChessMove> moves = GameManager.Instance.GameBoard.GetAllPossibleMoves(PieceColor.Black, false);

            foreach (var move in moves)
            {
                Debug.Log($"{move.Piece} at {move.Piece.Tile} to {move.Target}");
            }
        }


        public void PrintFen()
        {
            Debug.Log(GameManager.Instance.GameBoard.GetFen());
        }
    }
}