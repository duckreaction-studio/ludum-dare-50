using System.Collections;
using System.Collections.Generic;
using DuckReaction.Common.Container;
using UnityEngine;

namespace Enemies
{
    public class Pawn : ChessPiece
    {
        protected override Chessboard.Position GetRandomBackwardPositionFrom(Chessboard.Position previousPosition)
        {
            // Pawn can not backward
            return previousPosition;
        }

        protected override List<Chessboard.Position> GetAllForwardPositionFrom(Chessboard.Position currentPosition)
        {
            return new()
            {
                currentPosition + new Chessboard.Position(0, -1)
            };
        }
    }
}