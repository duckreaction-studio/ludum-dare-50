using System;
using System.Collections;
using System.Collections.Generic;
using DuckReaction.Common;
using DuckReaction.Common.Container;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] SerializableDictionary<ChessPiece.Type, GameObject> _enemies;

        [Inject] SignalBus _signalBus;

        void DisableAllEnemies()
        {
            foreach (var enemy in _enemies.Values)
                enemy.SetActive(false);
        }

        void Start()
        {
            DisableAllEnemies();
            _signalBus?.Subscribe<GameEvent>(OnGameEventReceived);
        }

        void OnGameEventReceived(GameEvent gameEvent)
        {
            if (gameEvent.Is(GameEventType.SpwanEnemy))
                Spawn(gameEvent.GetParam<ChessPiece.Type>());
        }

        void Spawn(ChessPiece.Type type)
        {
            DisableAllEnemies();
            var enemy = _enemies[type];
            enemy.transform.position = transform.position;
            enemy.SetActive(true);
        }
    }
}