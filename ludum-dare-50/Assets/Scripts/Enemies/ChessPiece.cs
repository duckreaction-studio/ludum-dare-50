using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DuckReaction.Common;
using DuckReaction.Common.Container;
using ModestTree;
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

        [SerializeField] [MinMaxSlider(0, 2, true)] [Tooltip("Speed for one square move")]
        private Vector2 _rangeMoveDuration = new(0.2f, 0.6f);

        [SerializeField] [MinMaxSlider(0, 5, true)]
        private Vector2 _rangeWaitDuration = new(0.2f, 3f);

        [SerializeField] private int _trajectoryCount = 4;

        private List<Trajectory> _trajectories;
        private int _trajectoryIndex;
        private bool _isInitialized = false;
        Chessboard.Position _startPosition;
        Sequence _sequence;

        private Trajectory CurrentTrajectory => _trajectories[_trajectoryIndex];

        void Start()
        {
            Init();
        }

        [ContextMenu("Init")]
        void Init()
        {
            _sequence?.Kill();
            InitRandomTrajectories();
            ChooseRandomTrajectory();
            transform.position = _chessboard.GetSquareWorldPosition(CurrentTrajectory.Positions.First());
            _isInitialized = true;
        }

        [ContextMenu("Start move")]
        public void StartMove()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            for (int i = 1; i < CurrentTrajectory.Length; i++)
            {
                var previousPosition = CurrentTrajectory.Positions[i - 1];
                var position = CurrentTrajectory.Positions[i];
                var worldPosition = _chessboard.GetSquareWorldPosition(position);
                var distance = (worldPosition - _chessboard.GetSquareWorldPosition(previousPosition)).magnitude;

                _sequence.Join(transform.DOMove(worldPosition, distance * _rangeMoveDuration.GetRandom())
                    .SetEase(Ease.InOutCubic));
                _sequence.AppendInterval(_rangeWaitDuration.GetRandom());
            }

            _sequence.Play();
        }

        void ChooseRandomTrajectory()
        {
            _trajectoryIndex = Random.Range(0, _trajectories.Count);
        }

        void InitRandomTrajectories()
        {
            _startPosition = GetRandomStartPosition();
            _trajectories = new(_trajectoryCount);
            for (int i = 0; i < _trajectoryCount; i++)
                _trajectories.Add(CreateRandomTrajectory());
        }

        Trajectory CreateRandomTrajectory()
        {
            var steps = CreateRandomSteps();
            var result = new Trajectory
            {
                Positions = new() {_startPosition}
            };

            foreach (var step in steps)
                AppendRandomPositionIntoTrajectory(result, step);

            return result;
        }

        private void AppendRandomPositionIntoTrajectory(Trajectory trajectory, int step)
        {
            var previousPosition = trajectory.Positions.Last();
            var nextPosition = step < 0
                ? GetRandomBackwardPositionFrom(previousPosition)
                : GetRandomForwardPositionFrom(previousPosition);
            if (nextPosition.IsValid())
                trajectory.Positions.Add(nextPosition);
        }

        private List<int> CreateRandomSteps()
        {
            List<int> steps = new();
            var stepBackwardCount = _rangeStepBackward.GetRandom();
            var stepForwardCount = _rangeStepForward.GetRandom();
            steps.AddRange(Enumerable.Repeat(-1, stepBackwardCount));
            steps.AddRange(Enumerable.Repeat(1, stepForwardCount));
            steps.Shuffle();
            return steps;
        }

        Chessboard.Position GetRandomStartPosition()
        {
            return Chessboard.Position.CreateFromName(_startPositionList.GetRandom());
        }

        protected virtual Chessboard.Position GetRandomBackwardPositionFrom(Chessboard.Position previousPosition)
        {
            var positionList = GetAllBackwardPositionFrom(previousPosition);
            positionList.Shuffle();
            return ShuffleAndGetFirstValidPosition(positionList);
        }

        protected virtual Chessboard.Position GetRandomForwardPositionFrom(Chessboard.Position previousPosition)
        {
            var positionList = GetAllForwardPositionFrom(previousPosition);
            return ShuffleAndGetFirstValidPosition(positionList);
        }

        static Chessboard.Position ShuffleAndGetFirstValidPosition(List<Chessboard.Position> positionList)
        {
            if (positionList.IsEmpty()) return Chessboard.Position.Invalid;
            positionList.Shuffle();
            foreach (var position in positionList)
            {
                if (position.IsValid())
                    return position;
            }

            return Chessboard.Position.Invalid;
        }

        protected virtual List<Chessboard.Position> GetAllBackwardPositionFrom(Chessboard.Position previousPosition)
        {
            throw new NotImplementedException();
        }

        protected virtual List<Chessboard.Position> GetAllForwardPositionFrom(Chessboard.Position previousPosition)
        {
            throw new NotImplementedException();
        }

        void OnDrawGizmosSelected()
        {
            if (!_isInitialized) return;
            for (var i = 0; i < _trajectories.Count; i++)
            {
                var trajectory = _trajectories[i];
                Gizmos.color = i == _trajectoryIndex ? Color.green : Color.red;
                Gizmos.DrawCube(_chessboard.GetSquareWorldPosition(trajectory.Positions.Last()),
                    Vector3.one * 0.2f);
            }
        }

        struct Trajectory
        {
            public List<Chessboard.Position> Positions;
            public int CurrentIndex;

            public int Length => Positions.Count;
        }
    }
}