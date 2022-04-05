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
    Animator _animator;

    void Start()
    {
        _startPosition = transform.position;
        _animator = GetComponent<Animator>();
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
        if (isActiveAndEnabled)
            _animator.Play("CharacterIdle", -1, 0f);
    }

    void Update()
    {
        _aimingCamera.Priority = _player.isAiming ? 20 : 0;
    }
}