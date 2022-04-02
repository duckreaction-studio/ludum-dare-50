using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class Knight : ChessPiece
    {
        protected override List<Chessboard.Position> GetAllBackwardPositionFrom(Chessboard.Position currentPosition)
        {
            List<Chessboard.Position> result = new();
            result.Add(currentPosition + new Chessboard.Position(-2, 1));
            result.Add(currentPosition + new Chessboard.Position(2, 1));
            result.Add(currentPosition + new Chessboard.Position(-1, 2));
            result.Add(currentPosition + new Chessboard.Position(1, 2));

            return result;
        }

        protected override List<Chessboard.Position> GetAllForwardPositionFrom(Chessboard.Position currentPosition)
        {
            List<Chessboard.Position> result = new();
            result.Add(currentPosition + new Chessboard.Position(-2, -1));
            result.Add(currentPosition + new Chessboard.Position(2, -1));
            result.Add(currentPosition + new Chessboard.Position(-1, -2));
            result.Add(currentPosition + new Chessboard.Position(1, -2));

            return result;
        }
    }
}