using Enemies;
using ModestTree.Util;
using UnityEngine;
using Zenject;

public class MainSceneAnimationController : MonoBehaviour
{
    [SerializeField] GameObject _onSnapKill;
    [SerializeField] GameObject _onSnapDeath;

    [Inject] EnemySpawner _spawner;

    ChessPieceAnimationController CurrentEnemyAnimation =>
        _spawner.CurrentEnemy.GetComponent<ChessPieceAnimationController>();

    [Preserve]
    [ContextMenu("Prepare targets")]
    public void PrepareTargets()
    {
        _onSnapKill.transform.position = CurrentEnemyAnimation.snapKill.transform.position;
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