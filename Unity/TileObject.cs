using System;
using Lean.Gui;
using SimpleChess.Game;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SimpleChess.Unity
{
    public class TileObject : MonoBehaviour
    {
        [FormerlySerializedAs("Rank")] public int Y;
        [FormerlySerializedAs("File")] public int X;

        private GameObject _heldPiece;

        public void SetPiece(ChessPiece piece)
        {
            _heldPiece = Instantiate(DrawBoard.Piece, transform);
            PieceObject pieceData = _heldPiece.GetComponent<PieceObject>();
            pieceData.Piece = piece;
            pieceData.Piece.Tile = new TilePair(X, Y);
            pieceData.Draw();

            if (GameManager.Instance.GameBoard.PlayerTurn != pieceData.Piece.Color)
            {
                _heldPiece.GetComponent<LeanDrag>().enabled = false;
            }
        }

        public void DeletePiece()
        {
            if (_heldPiece == null) return;
            Destroy(_heldPiece);
            _heldPiece = null;
        }

        public override string ToString()
        {
            return new TilePair(X, Y).ToString();
        }

        public PieceColor GetTileColor()
        {
            int colorOffset = 0;
            if (X % 2 == 0) colorOffset = 1;
            if ((Y + colorOffset) % 2 == 0)
            {
                return PieceColor.White;
            }
            else
            {
                return PieceColor.Black;
            }
        }
    }
}