using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DuckReaction.Common;
using DuckReaction.Common.Container;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class ChessPiece : MonoBehaviour
    {
        [Inject] private Chessboard _chessboard;

        [SerializeField] private string[] _startPositionList;

        [SerializeField] [MinMaxSlider(0, 10, true)]
        private Vector2Int _rangeStepBackward = new(0, 2);

        [SerializeField] [MinMaxSlider(0, 10, true)]
        private Vector2Int _rangeStepForward = new(2, 4);

        [SerializeField] [MinMaxSlider(0, 5, true)]
        private Vector2 _rangeMoveDuration = new(0.2f, 2f);

        [SerializeField] [MinMaxSlider(0, 5, true)]
        private Vector2 _rangeWaitDuration = new(0.2f, 2f);

        [SerializeField] private int _trajectoryCount = 4;

        private List<Trajectory> _trajectories;
        private int _trajectoryIndex;
        private bool _isInited = false;

        private Trajectory CurrentTrajectory
        {
            get { return _trajectories[_trajectoryIndex]; }
        }

        void Start()
        {
            Init();
        }

        [ContextMenu("Init")]
        private void Init()
        {
            InitRandomTrajectories();
            ChooseRandomTrajectory();
            transform.position = _chessboard.GetSquareWorldPosition(CurrentTrajectory.Positions.First());
            _isInited = true;
        }

        private void ChooseRandomTrajectory()
        {
            _trajectoryIndex = Random.Range(0, _trajectories.Count);
        }

        private void InitRandomTrajectories()
        {
            _trajectories = new(_trajectoryCount);
            for (int i = 0; i < _trajectoryCount; i++)
                _trajectories.Add(CreateRandomTrajectory());
        }

        private Trajectory CreateRandomTrajectory()
        {
            List<int> steps = new();
            int stepBackwardCount = _rangeStepBackward.GetRandom();
            int stepForwardCount = _rangeStepForward.GetRandom();
            steps.AddRange(Enumerable.Repeat(-1, stepBackwardCount));
            steps.AddRange(Enumerable.Repeat(1, stepForwardCount));
            steps.Shuffle();

            var result = new Trajectory();
            result.Positions = new();
            result.Positions.Add(GetRandomStartPosition());

            for (var i = 0; i < steps.Count; i++)
            {
                var step = steps[i];
                var previousPosition = result.Positions.Last();
                Chessboard.Position nextPosition;
                if (step < 0)
                    nextPosition = GetRandomBackwardPositionFrom(previousPosition);
                else
                    nextPosition = GetRandomForwardPositionFrom(previousPosition);
                if (nextPosition.IsValid())
                    result.Positions.Add(nextPosition);
            }

            return result;
        }

        private Chessboard.Position GetRandomStartPosition()
        {
            return Chessboard.Position.CreateFromName(_startPositionList.GetRandom());
        }

        protected virtual Chessboard.Position GetRandomBackwardPositionFrom(Chessboard.Position previousPosition)
        {
            throw new NotImplementedException();
        }

        protected virtual Chessboard.Position GetRandomForwardPositionFrom(Chessboard.Position previousPosition)
        {
            throw new NotImplementedException();
        }

        private void OnDrawGizmosSelected()
        {
            if (_isInited)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(_chessboard.GetSquareWorldPosition(CurrentTrajectory.Positions.Last()),
                    Vector3.one * 0.2f);
            }
        }

        private struct Trajectory
        {
            public List<Chessboard.Position> Positions;
            public int CurrentIndex;
        }
    }
}