using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using ModestTree.Util;
using UnityEngine;
using Zenject;

public class MainSceneAnimationController : MonoBehaviour
{
    [SerializeField] GameObject _onSnapKill;
    [SerializeField] GameObject _onSnapDeath;

    [Inject] EnemySpawner _spawner;

    [Preserve]
    [ContextMenu("Prepare targets")]
    public void PrepareTargets()
    {
        _onSnapKill.transform.position = GetEnemySnapPosition("SpawnKill");
        _onSnapDeath.transform.position = GetEnemySnapPosition("SpawnDeath");
    }

    [Preserve]
    [ContextMenu("Enable blood 1")]
    public void EnableBlood1()
    {
        SetEnemyBloodActive("Blood1", true);
    }

    [Preserve]
    [ContextMenu("Enable blood 2")]
    public void EnableBlood2()
    {
        SetEnemyBloodActive("Blood2", true);
    }

    [Preserve]
    [ContextMenu("Reset")]
    public void Reset()
    {
        SetEnemyBloodActive("Blood1", false);
        SetEnemyBloodActive("Blood2", false);
    }

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
}