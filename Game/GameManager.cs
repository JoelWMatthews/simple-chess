using SimpleChess.Unity;
using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace SimpleChess.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public ChessBoard GameBoard { get; private set; }

        public PieceColor AIControlledQueue = PieceColor.None;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Initialize()
        {
            GameBoard = new ChessBoard();
            GameBoard.AIControlled = AIControlledQueue;
            GameManager.Instance.GameBoard.Listeners.Add(DrawBoard.Instance);
        }

        public static void SetAIControlWhite() => SetAIControl(PieceColor.White);

        public static void SetAIControlBlack() => SetAIControl(PieceColor.Black);

        public static void SetAIControl(PieceColor color)
        {
            Instance.AIControlledQueue = color;
        }

        public static void ClearSettings()
        {
            Instance.AIControlledQueue = PieceColor.None;
        }
    }
}
