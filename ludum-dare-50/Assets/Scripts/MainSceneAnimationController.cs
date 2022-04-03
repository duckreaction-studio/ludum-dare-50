using System;
using DuckReaction.Common;
using DuckReaction.Common.Container;
using Enemies;
using ModestTree.Util;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Zenject;

public class MainSceneAnimationController : MonoBehaviour
{
    [SerializeField] GameObject _onSnapKill;
    [SerializeField] GameObject _onSnapDeath;

    [SerializeField] PlayableDirector _director;
    [SerializeField] SerializableDictionary<Score.Type, TimelineAsset> _timelines;

    [Inject] EnemySpawner _spawner;
    [Inject] SignalBus _signalBus;

    ChessPieceAnimationController CurrentEnemyAnimation =>
        _spawner.CurrentEnemy.GetComponent<ChessPieceAnimationController>();

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

    [Preserve]
    [ContextMenu("Trigger dead")]
    public void TriggerDead()
    {
        CurrentEnemyAnimation.TriggerDead();
    }

    [Preserve]
    [ContextMenu("Move on kill snap")]
    public void MoveOnKillSnap()
    {
        _onSnapKill.transform.position = CurrentEnemyAnimation.snapKill.transform.position;
    }

    [Preserve]
    [ContextMenu("Move on death snap")]
    public void MoveOnDeathSnap()
    {
        _onSnapDeath.transform.position = CurrentEnemyAnimation.snapDeath.transform.position;
    }

    [Preserve]
    [ContextMenu("Enable blood 1")]
    public void EnableBlood1()
    {
        CurrentEnemyAnimation.SetBloodActive(0, true);
    }

    [Preserve]
    [ContextMenu("Enable blood 2")]
    public void EnableBlood2()
    {
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