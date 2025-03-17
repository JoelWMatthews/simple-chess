using System.Collections.Generic;
using System.Linq;
using SimpleChess.Game;
using TMPro;
using UnityEngine;

namespace SimpleChess.Unity
{
    public class DrawBoard : ChessBoardListener
    {
        public static DrawBoard Instance;
        
        public static GameObject Piece;
        public static GameObject Marker;

        public TMP_Text TurnText;
        
        public static Color DarkColor = Color.black;
        public static Color LightColor = Color.white;

        public SpritePair[] FrontSprite;
        public SpritePair[] BackSprite;

        public static Dictionary<PieceType, Sprite> FrontSprites = new Dictionary<PieceType, Sprite>();
        public static Dictionary<PieceType, Sprite> BackSprites = new Dictionary<PieceType, Sprite>();

        public static TileObject[][] Board;
        void Start()
        {
            Instance = this;
            
            Piece = Resources.Load<GameObject>("Prefabs/Piece");
            Marker = Resources.Load<GameObject>("Prefabs/Marker");
            
            foreach (var pairs in FrontSprite)
            {
                FrontSprites.Add(pairs.Type, pairs.Sprite);
            }

            foreach (var pairs in BackSprite)
            {
                BackSprites.Add(pairs.Type, pairs.Sprite);
            }

            Board = new TileObject[8][];
            for (int i = 0; i < 8; i++)
            {
                Board[i] = new TileObject[8];
            }
            
            TileObject[] pieces = transform.GetComponentsInChildren<TileObject>();
            foreach (TileObject piece in pieces)
            {
                Board[piece.X][piece.Y] = piece;
            }
            
            GameManager.Instance.Initialize();
            //GameManager.SetFromString(GameManager.TestingString);
            Draw();
        }

        public void StartOver()
        {
            GameManager.Instance.Initialize();
            Instance.TurnText.text = "White's Turn!";
            Draw();
        }

        public static void MovePiece(TilePair original, TilePair destination)
        {
            GameManager.Instance.GameBoard.MovePiece(original, destination);
            if (GameManager.Instance.GameBoard.PlayerTurn == PieceColor.White)
            {
                Instance.TurnText.text = "White's Turn!";
            }
            else
            {
                Instance.TurnText.text = "Black's Turn!";
            }

            if (GameManager.Instance.GameBoard.Win != PieceColor.None)
            {
                if (GameManager.Instance.GameBoard.Win == PieceColor.Black)
                {
                    Instance.TurnText.text = "BLACK WINS!!";
                }
                else if (GameManager.Instance.GameBoard.Win == PieceColor.White)
                {
                    Instance.TurnText.text = "WHITE WINS!!";
                }
                else
                {
                    Instance.TurnText.text = "DRAW!!";
                }
            }

            Draw();
        }

        public static void Draw()
        {
            for (int x = 0; x < Board.Length; x++)
            {
                for (int y = 0; y < Board[x].Length; y++)
                {
                    Board[x][y].DeletePiece();
                    if (!Equals(GameManager.Instance.GameBoard.Layout[x][y], null))
                    {
                        Board[x][y].SetPiece(GameManager.Instance.GameBoard.Layout[x][y]);
                    }
                }
            }
        }

        public override void OnAIMove()
        {
            Debug.Log("OnMove");
            if (GameManager.Instance.GameBoard.PlayerTurn == PieceColor.White)
            {
                Instance.TurnText.text = "White's Turn!";
            }
            else
            {
                Instance.TurnText.text = "Black's Turn!";
            }

            if (GameManager.Instance.GameBoard.Win != PieceColor.None)
            {
                if (GameManager.Instance.GameBoard.Win == PieceColor.Black)
                {
                    Instance.TurnText.text = "BLACK WINS!!";
                }
                else if (GameManager.Instance.GameBoard.Win == PieceColor.White)
                {
                    Instance.TurnText.text = "WHITE WINS!!";
                }
                else
                {
                    Instance.TurnText.text = "DRAW!!";
                }
            }

            Draw();
        }
    }
}
