using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class Chessboard : MonoBehaviour
    {
        public static readonly int size = 8;

        [SerializeField] float _squareSize = 1f;
        [SerializeField] float _gizmoLineLength = 1f;

        void OnDrawGizmos()
        {
            for (var column = 0; column < size; column++)
            {
                for (var row = 0; row < size; row++)
                {
                    var position = GetSquareWorldPosition(new Position(column, row));
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(position, position + Vector3.up * _gizmoLineLength);
                }
            }
        }

        public Vector3 GetSquareWorldPosition(Position position)
        {
            if (!position.IsValid())
                throw new InvalidChessboardPosition();
            return transform.position + Vector3.right * _squareSize * position.column +
                   Vector3.forward * _squareSize * position.row;
        }

        public struct Position
        {
            public int column;
            public int row;
            public static readonly Position Invalid = new(-1, -1);

            public static Position CreateFromName(string squareName)
            {
                if (squareName.Length != 2)
                    throw new InvalidChessboardPosition();
                squareName = squareName.ToUpper();
                var result = new Position(squareName[0] - 'A', squareName[1] - '1');
                if (!result.IsValid())
                    throw new InvalidChessboardPosition();
                return result;
            }

            public Position(int column, int row)
            {
                this.column = column;
                this.row = row;
            }

            public bool IsValid()
            {
                return column >= 0 && row >= 0 && column < size && row < size;
            }

            public static Position operator +(Position a, Position b)
            {
                return new Position(a.column + b.column, a.row + b.row);
            }
        }
    }

    public class InvalidChessboardPosition : Exception
    {
    }
}