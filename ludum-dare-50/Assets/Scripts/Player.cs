using System;
using DuckReaction.Common;
using Enemies;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [Inject] SignalBus _signalBus;

    Camera _shotCamera;
    bool _hasShot;

    public Camera ShotCamera
    {
        get
        {
            if (_shotCamera == null)
            {
                _shotCamera = GameObject.FindWithTag("ShotCamera")?.GetComponent<Camera>();
            }

            return _shotCamera;
        }
    }

    public bool isHolding { get; private set; }

    void Start()
    {
        _signalBus.Subscribe<GameEvent>(OnGameEventReceived);
    }

    void OnGameEventReceived(GameEvent gameEvent)
    {
        if (gameEvent.Is(GameEventType.Reset) || gameEvent.Is(GameEventType.LevelRestart))
            Reset();
    }

    void Reset()
    {
        _hasShot = false;
        _shotCamera = null;
    }

    void Update()
    {
        if (!_hasShot)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isHolding = true;
                _signalBus.Fire(new GameEvent(GameEventType.PlayerPrepareShot));
            }


            if (Input.GetMouseButtonUp(0) && ShotCamera)
            {
                isHolding = false;
                Shot();
                _hasShot = true;
            }
        }
    }

    void Shot()
    {
        var hitInfo = TestHitEnemy();
        Debug.Log("Shot " + hitInfo);
        _signalBus.Fire(new GameEvent(GameEventType.PlayerShot, hitInfo));
    }

    HitInfo TestHitEnemy()
    {
        var ray = ShotCamera.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray);
        HitInfo hitInfo = new(null, false);
        foreach (var hit in hits)
        {
            if (hitInfo.enemy == null &&
                (hit.collider.CompareTag("PerfectCollider") || hit.collider.CompareTag("LargeCollider")))
            {
                hitInfo.enemy = hit.collider.GetComponentInParent<ChessPiece>();
            }

            hitInfo.perfectHit = hitInfo.perfectHit || hit.collider.CompareTag("PerfectCollider");
        }

        return hitInfo;
    }

    public struct HitInfo
    {
        public ChessPiece enemy;
        public bool perfectHit;
        public bool HasHitEnemy => enemy != null;

        public HitInfo(ChessPiece enemy, bool perfectHit)
        {
            this.enemy = enemy;
            this.perfectHit = perfectHit;
        }
    }
}