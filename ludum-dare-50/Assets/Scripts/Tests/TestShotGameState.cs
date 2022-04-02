using System.Collections;
using DuckReaction.Common;
using Enemies;
using UnityEngine;
using Zenject;

namespace Tests
{
    public class TestShotGameState : MonoBehaviour
    {
        [Inject] SignalBus _signalBus;
        [Inject] ChessPiece _chessPiece;

        [SerializeField] float _waitBeforeStart = 2f;

        void Start()
        {
            StartCoroutine(StartGame());
            _signalBus.Subscribe<GameEvent>(OnReceiveGameEvent);
        }

        void OnReceiveGameEvent(GameEvent gameEvent)
        {
            if (gameEvent.Is(GameEventType.PlayerHitEnemy))
            {
                Debug.Log("Player hit enemy " + gameEvent.GetParam<bool>());
            }

            if (gameEvent.Is(GameEventType.PlayerShot))
                Debug.Log("Player has shot");
        }

        IEnumerator StartGame()
        {
            yield return new WaitForSeconds(_waitBeforeStart);
            Debug.Log("Start");
            _chessPiece.StartMove();
        }
    }
}