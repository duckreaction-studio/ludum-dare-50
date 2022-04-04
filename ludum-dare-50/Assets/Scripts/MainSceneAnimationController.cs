using System;
using DG.Tweening;
using DuckReaction.Common;
using DuckReaction.Common.Container;
using Enemies;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Zenject;
using Zenject.Internal;

public class MainSceneAnimationController : MonoBehaviour
{
    [SerializeField] float _moveDuration = 0.25f;
    [SerializeField] GameObject _onSnapKill;
    [SerializeField] GameObject _onSnapDeath;

    [SerializeField] PlayableDirector _director;
    [SerializeField] SerializableDictionary<Score.Type, TimelineAsset> _timelines;

    [Inject] EnemySpawner _spawner;
    [Inject] SignalBus _signalBus;

    ChessPieceAnimationController
        CurrentEnemyAnimation
    {
        get
        {
            if (_spawner.CurrentEnemy != null)
                _spawner.CurrentEnemy.GetComponent<ChessPieceAnimationController>();
            return null;
        }
    }

    void Start()
    {
        _signalBus.Subscribe<GameEvent>(OnGameEventReceived);
    }

    void OnGameEventReceived(GameEvent gameEvent)
    {
        if (gameEvent.Is(GameEventType.LevelWin))
        {
            PlayAnimation(gameEvent.GetParam<Score>().type);
        }
        else if (gameEvent.Is(GameEventType.LevelGameOver))
        {
            PlayAnimation(Score.Type.Fail);
        }
    }

    [Preserve]
    public void SignalEndOfAnimation()
    {
        _signalBus.Fire(new GameEvent(GameEventType.LevelAnimationEnd));
    }

    [ContextMenu("Test perfect")]
    public void TestPerfect()
    {
        PlayAnimation(Score.Type.Perfect);
    }

    public void PlayAnimation(Score.Type scoreType)
    {
        _director.playableAsset = _timelines[scoreType];
        _director.Play();
    }

    [ModestTree.Util.Preserve]
    [ContextMenu("Trigger dead")]
    public void TriggerDead()
    {
        if (CurrentEnemyAnimation != null)
            CurrentEnemyAnimation.TriggerDead();
    }

    [ModestTree.Util.Preserve]
    [ContextMenu("Move on kill snap")]
    public void MoveOnKillSnap()
    {
        if (CurrentEnemyAnimation != null)
            _onSnapKill.transform.DOMove(CurrentEnemyAnimation.snapKill.transform.position, _moveDuration);
    }

    [ModestTree.Util.Preserve]
    [ContextMenu("Move on death snap")]
    public void MoveOnDeathSnap()
    {
        if (CurrentEnemyAnimation != null)
            _onSnapDeath.transform.DOMove(CurrentEnemyAnimation.snapDeath.transform.position, _moveDuration);
    }

    [ModestTree.Util.Preserve]
    [ContextMenu("Enable blood 1")]
    public void EnableBlood1()
    {
        if (CurrentEnemyAnimation != null)
            CurrentEnemyAnimation.SetBloodActive(0, true);
    }

    [ModestTree.Util.Preserve]
    [ContextMenu("Enable blood 2")]
    public void EnableBlood2()
    {
        if (CurrentEnemyAnimation != null)
            CurrentEnemyAnimation.SetBloodActive(1, true);
    }

/*
    void SetEnemyBloodActive(string tagName, bool active)
    {
        var target = FindChildWithTag(_spawner.CurrentEnemy, tagName);
        if (target)
            target.gameObject.SetActive(active);
    }

    Vector3 GetEnemySnapPosition(string tagName)
    {
        var target = FindChildWithTag(_spawner.CurrentEnemy, tagName);
        return target ? target.position : Vector3.zero;
    }

    Transform FindChildWithTag(GameObject root, string tagName)
    {
        var children = root.GetComponentsInChildren<Transform>();
        return children.FirstOrDefault(x => x.CompareTag(tagName));
    }
    */
}