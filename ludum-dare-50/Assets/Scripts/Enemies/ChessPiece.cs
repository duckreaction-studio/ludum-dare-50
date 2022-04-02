using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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

        [SerializeField] [MinMaxSlider(0, 5, true)] [Tooltip("Speed for one square move")]
        private Vector2 _rangeMoveDuration = new(0.2f, 2f);

        [SerializeField] [MinMaxSlider(0, 5, true)]
        private Vector2 _rangeWaitDuration = new(0.2f, 3f);

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
        void Init()
        {
            InitRandomTrajectories();
            ChooseRandomTrajectory();
            transform.position = _chessboard.GetSquareWorldPosition(CurrentTrajectory.Positions.First());
            _isInited = true;
        }

        [ContextMenu("Start move")]
        public void StartMove()
        {
            var sequence = DOTween.Sequence();
            for (int i = 1; i < CurrentTrajectory.Length; i++)
            {
                var previousPosition = CurrentTrajectory.Positions[i - 1];
                var position = CurrentTrajectory.Positions[i];
                var worldPosition = _chessboard.GetSquareWorldPosition(position);
                var distance = (worldPosition - _chessboard.GetSquareWorldPosition(previousPosition)).magnitude;

                sequence.Join(transform.DOMove(worldPosition, distance * _rangeMoveDuration.GetRandom())
                    .SetEase(Ease.InOutCubic));
                sequence.AppendInterval(_rangeWaitDuration.GetRandom());
            }

            sequence.Play();
        }

        void ChooseRandomTrajectory()
        {
            _trajectoryIndex = Random.Range(0, _trajectories.Count);
        }

        void InitRandomTrajectories()
        {
            _trajectories = new(_trajectoryCount);
            for (int i = 0; i < _trajectoryCount; i++)
                _trajectories.Add(CreateRandomTrajectory());
        }

        Trajectory CreateRandomTrajectory()
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

        Chessboard.Position GetRandomStartPosition()
        {
            return Chessboard.Position.CreateFromName(_startPositionList.GetRandom());
        }

        protected virtual Chessboard.Position GetRandomBackwardPositionFrom(Chessboard.Position previousPosition)
        {
            List<Chessboard.Position> forwardPositionList = GetAllBackwardPositionFrom(previousPosition);
            forwardPositionList.Shuffle();
            foreach (var position in forwardPositionList)
            {
                if (position.IsValid())
                    return position;
            }

            return new();
        }

        protected virtual Chessboard.Position GetRandomForwardPositionFrom(Chessboard.Position previousPosition)
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
            if (_isInited)
            {
                for (var i = 0; i < _trajectories.Count; i++)
                {
                    var trajectory = _trajectories[i];
                    Gizmos.color = i == _trajectoryIndex ? Color.green : Color.red;
                    Gizmos.DrawCube(_chessboard.GetSquareWorldPosition(trajectory.Positions.Last()),
                        Vector3.one * 0.2f);
                }
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