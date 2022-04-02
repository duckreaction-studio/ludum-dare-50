using System;
using System.Collections;
using System.Collections.Generic;
using DuckReaction.Common.Container;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class ChessPiece : MonoBehaviour
    {
        [Inject] private Chessboard _chessboard;

        [SerializeField] private string[] _startPositionList;

        void Start()
        {
            InitRandomPosition();
        }

        [ContextMenu("Init random position")]
        private void InitRandomPosition()
        {
            transform.position = _chessboard.GetSquareWorldPosition(_startPositionList.GetRandom());
        }
    }
}