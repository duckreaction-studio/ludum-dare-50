using DuckReaction.Common;
using ModestTree.Util;
using Settings;
using UnityEngine;
using Zenject;

public class CommonInstaller : MonoInstaller
{
    [Preserve]
    public override void InstallBindings()
    {
        Debug.Log("Install common");
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<GameEvent>();
        Container.Bind<SceneService>().FromComponentInHierarchy(false).AsSingle();
        Container.Bind<ScoreSettings>().FromScriptableObjectResource("ScoreSettings").AsSingle();
    }
}