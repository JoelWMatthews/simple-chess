using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleChess.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public ChessBoard GameBoard { get; private set; }

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
        }
    }
}
