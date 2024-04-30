using System;
using System.Collections.Generic;
using SimpleChess.Game;
using UnityEngine;

namespace SimpleChess.Unity
{
    [Serializable]
    public class SpritePair
    {
        public PieceType Type;
        public Sprite Sprite;

        public SpritePair(PieceType _type, Sprite _sprite)
        {
            Type = _type;
            Sprite = _sprite;
        }
    }
}