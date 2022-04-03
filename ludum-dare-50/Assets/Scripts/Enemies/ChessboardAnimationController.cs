using System.Collections.Generic;
using System.Linq;
using DuckReaction.Common;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class ChessboardAnimationController : MonoBehaviour
    {
        [Inject] SignalBus _signalBus;
        [Inject] EnemySpawner _spawner;

        List<GameObject> _emitters = new();

        void Start()
        {
            _signalBus.Subscribe<GameEvent>(OnGameEventReceived);
        }

        void OnGameEventReceived(GameEvent gameEvent)
        {
            if (gameEvent.Is(GameEventType.EnemyReady))
                OnEnemyReady();
        }

        void OnEnemyReady()
        {
            ClearEmitters();

            var enemy = _spawner.CurrentEnemy.GetComponent<ChessPiece>();
            EnableEmitters(enemy.trajectories);
        }

        void EnableEmitters(List<ChessPiece.Trajectory> trajectories)
        {
            foreach (var trajectory in trajectories)
            {
                var target = FindEmitter(trajectory.positions.Last());
                if (target)
                {
                    target.SetActive(true);
                    _emitters.Add(target);
                }
            }
        }

        GameObject FindEmitter(Chessboard.Position position)
        {
            var squareName = position.Name.ToLower();
            var target = GameObject.Find(squareName);
            foreach (Transform child in target.transform)
                if (child.gameObject.name == "SquareEmit")
                    return child.gameObject;
            return null;
        }

        void ClearEmitters()
        {
            foreach (var emitter in _emitters)
                emitter.SetActive(false);
            _emitters.Clear();
        }
    }
}