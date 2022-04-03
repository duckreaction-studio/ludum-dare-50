using Enemies;
using ModestTree.Util;
using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [Preserve]
    public override void InstallBindings()
    {
        Debug.Log("Install level");
        Container.Bind<Player>().FromComponentInHierarchy(false).AsSingle();
        Container.Bind<EnemySpawner>().FromComponentInHierarchy(false).AsSingle();
    }
}