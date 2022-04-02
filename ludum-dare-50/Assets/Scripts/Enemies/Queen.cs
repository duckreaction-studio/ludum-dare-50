using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class Queen : ChessPiece
    {
        protected override List<Chessboard.Position> GetAllBackwardPositionFrom(Chessboard.Position currentPosition)
        {
            List<Chessboard.Position> result = new();
            for (var i = 1; i < Chessboard.size; i++)
            {
                result.Add(currentPosition + new Chessboard.Position(-i, i));
                result.Add(currentPosition + new Chessboard.Position(i, i));
            }

            for (var i = 1; i < Chessboard.size; i++)
            {
                result.Add(currentPosition + new Chessboard.Position(0, i));
            }

            return result;
        }

        protected override List<Chessboard.Position> GetAllForwardPositionFrom(Chessboard.Position currentPosition)
        {
            List<Chessboard.Position> result = new();
            for (var i = 1; i < Chessboard.size; i++)
            {
                result.Add(currentPosition + new Chessboard.Position(-i, -i));
                result.Add(currentPosition + new Chessboard.Position(i, -i));
            }

            for (var i = -Chessboard.size + 1; i < Chessboard.size; i++)
            {
                if (i != 0)
                {
                    result.Add(currentPosition + new Chessboard.Position(i, 0));
                }

                if (i < 0)
                {
                    result.Add(currentPosition + new Chessboard.Position(0, i));
                }
            }

            return result;
        }
    }
}