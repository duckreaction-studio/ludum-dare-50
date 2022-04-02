using DuckReaction.Common;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [Inject] SignalBus _signalBus;

    Camera _shotCamera;

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

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && ShotCamera)
        {
            var (hitEnemy, perfectHit) = TestHitEnemy();

            _signalBus.Fire(new GameEvent(GameEventType.PlayerShot, hitEnemy));
            if (hitEnemy)
            {
                _signalBus.Fire(new GameEvent(GameEventType.PlayerHitEnemy, perfectHit));
            }
        }
    }

    (bool hitEnemy, bool perfectHit) TestHitEnemy()
    {
        var ray = ShotCamera.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray);
        bool hitEnemy = false, perfectHit = false;
        foreach (var hit in hits)
        {
            perfectHit = perfectHit || hit.collider.CompareTag("PerfectCollider");
            hitEnemy = hitEnemy || hit.collider.CompareTag("LargeCollider");
        }

        return (hitEnemy, perfectHit);
    }
}