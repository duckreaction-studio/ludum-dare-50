using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DuckReaction.Common;
using UnityEngine;
using Zenject;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _aimingCamera;
    [Inject] Player _player;
    [Inject] SignalBus _signalBus;

    Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
        _signalBus.Subscribe<GameEvent>(OnGameEventReceived);
    }

    void OnGameEventReceived(GameEvent gameEvent)
    {
        if (gameEvent.Is(GameEventType.LevelRestart))
        {
            Reset();
        }
    }

    void Reset()
    {
        transform.position = _startPosition;
    }

    void Update()
    {
        _aimingCamera.Priority = _player.isHolding ? 20 : 0;
    }
}