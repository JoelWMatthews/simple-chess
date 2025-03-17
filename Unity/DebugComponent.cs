using System.Collections.Generic;
using SimpleChess.Game;
using UnityEditor;
using UnityEngine;

namespace SimpleChess.Unity
{
    public class DebugComponent : MonoBehaviour
    {
        public Object executable;
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

        public async void PrintBestMove()
        {
            string fen = GameManager.Instance.GameBoard.GetFen();
            string bestMove = await StockfishCommunicator.Instance.GetBestMove(fen);
            Debug.Log(bestMove);
        }

        public void PrintExeName()
        {
            Debug.Log(AssetDatabase.GetAssetPath(executable));
        }
    }
}