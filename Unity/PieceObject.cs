using System.Collections.Generic;
using SimpleChess.Game;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleChess.Unity
{
    public class PieceObject : MonoBehaviour
    {
        public Piece Piece;

        public Image Front;
        public Image Back;

        public List<GameObject> Markers = new List<GameObject>();
        public void OnPickUp()
        {
            List<TilePair> moves = Piece.GetMoves(GameManager.Board);

            foreach (var move in moves)
            {
                GameObject marker = Instantiate(DrawBoard.Marker, DrawBoard.Board[move.X][move.Y].transform);
                PieceColor tileColor = DrawBoard.Board[move.X][move.Y].GetTileColor();
                if (tileColor == PieceColor.White)
                {
                    marker.GetComponent<Image>().color = DrawBoard.DarkColor;
                }
                else
                {
                    marker.GetComponent<Image>().color = DrawBoard.LightColor;
                }
                Markers.Add(marker);
            }
        }

        public void OnPutDown()
        {
            //transform.position = new Vector3(_cachedX, _cachedY, transform.position.z);

            foreach (var marker in Markers)
            {
                Destroy(marker);
            }
            Markers.Clear();

            TileObject closestTile = DrawBoard.Board[0][0];
            float closestDistance =
                Vector2.Distance(transform.position, new Vector2(closestTile.transform.position.x, closestTile.transform.position.y));

            for (int x = 0; x < DrawBoard.Board.Length; x++)
            {
                for (int y = 0; y < DrawBoard.Board[x].Length; y++)
                {
                    float checkDistance = Vector2.Distance(transform.position, new Vector2(DrawBoard.Board[x][y].transform.position.x, DrawBoard.Board[x][y].transform.position.y));
                    if (closestDistance > checkDistance)
                    {
                        closestDistance = checkDistance;
                        closestTile = DrawBoard.Board[x][y];
                    }
                }
            }
            
            
            DrawBoard.MovePiece(new TilePair(Piece.Tile.X, Piece.Tile.Y), new TilePair(closestTile.X, closestTile.Y));
        }

        public void Draw()
        {
            Front.sprite = DrawBoard.FrontSprites[Piece.Type];
            Back.sprite = DrawBoard.BackSprites[Piece.Type];

            if (Piece.Color == PieceColor.White)
            {
                Front.color = DrawBoard.LightColor;
                Back.color = DrawBoard.DarkColor;
            }
            else
            {
                Front.color = DrawBoard.DarkColor;
                Back.color = DrawBoard.LightColor;
            }
        }
    }
}