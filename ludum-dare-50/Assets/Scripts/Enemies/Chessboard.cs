using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class Chessboard : MonoBehaviour
    {
        public static readonly int size = 8;

        [SerializeField] private float _squareSize = 1f;
        [SerializeField] private float _gizmoLineLength = 1f;

        private void OnDrawGizmos()
        {
            for (var column = 0; column < size; column++)
            {
                for (var row = 0; row < size; row++)
                {
                    var position = GetSquareWorldPosition(column, row);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(position, position + Vector3.up * _gizmoLineLength);
                }
            }
        }

        public Vector3 GetSquareWorldPosition(string squareName)
        {
            if (squareName.Length != 2)
                throw new InvalidChessboardPosition();
            squareName = squareName.ToUpper();
            return GetSquareWorldPosition(squareName[0] - 'A', squareName[1] - '1');
        }

        public Vector3 GetSquareWorldPosition(int column, int row)
        {
            EnsurePositionIsValid(column, row);
            return transform.position + Vector3.right * _squareSize * column + Vector3.forward * _squareSize * row;
        }

        private static void EnsurePositionIsValid(int column, int row)
        {
            if (column < 0 || row < 0 || column >= size || row >= size)
                throw new InvalidChessboardPosition();
        }
    }

    public class InvalidChessboardPosition : Exception
    {
    }
}