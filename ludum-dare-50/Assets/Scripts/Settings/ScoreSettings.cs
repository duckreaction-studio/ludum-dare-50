using System;
using DuckReaction.Common.Container;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "ScoreSettings", menuName = "Game/Create score settings", order = 0)]
    public class ScoreSettings : ScriptableObject
    {
        [SerializeField]
        public float killTimeout = 0.15f; // Temps maximum pour tuer l'enemie lorsqu'il arrive sur la position de fin

        [SerializeField] [InfoBox("Order matter. Higher score should be first in the list")]
        public ScoreLevelSettings[] scoreLevelSettings;

        [Serializable]
        public struct ScoreLevelSettings
        {
            [SerializeField] public Score.Type type;
            [SerializeField] public bool needPerfectHit;
            [SerializeField] public float maxDistance;
        }

        public Score CalculatePlayerScore(bool perfectHit, float distanceFromSquare)
        {
            Score result;
            result.perfectHit = perfectHit;
            result.enemyDistanceFromSquare = distanceFromSquare;
            result.type = Score.Type.Fail;
            foreach (var scoreLevel in scoreLevelSettings)
            {
                if (scoreLevel.needPerfectHit && !result.perfectHit)
                    continue;
                if (result.enemyDistanceFromSquare > scoreLevel.maxDistance)
                    continue;
                result.type = scoreLevel.type;
                break;
            }

            return result;
        }
    }
}