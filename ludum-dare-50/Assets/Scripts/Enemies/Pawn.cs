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

        protected override Chessboard.Position GetRandomForwardPositionFrom(Chessboard.Position previousPosition)
        {
            List<Chessboard.Position> forwardPositionList = GetAllForwardPositionFrom(previousPosition);
            forwardPositionList.Shuffle();
            foreach (var position in forwardPositionList)
            {
                if (position.IsValid())
                    return position;
            }

            return new();
        }

        private static List<Chessboard.Position> GetAllForwardPositionFrom(Chessboard.Position currentPosition)
        {
            return new()
            {
                currentPosition + new Chessboard.Position(0, -1)
            };
        }
    }
}