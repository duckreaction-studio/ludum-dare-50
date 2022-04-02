using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class Bishop : ChessPiece
    {
        protected override List<Chessboard.Position> GetAllBackwardPositionFrom(Chessboard.Position currentPosition)
        {
            List<Chessboard.Position> result = new();
            for (var i = 1; i < Chessboard.size; i++)
            {
                result.Add(currentPosition + new Chessboard.Position(-i, i));
                result.Add(currentPosition + new Chessboard.Position(i, i));
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

            return result;
        }
    }
}