using Zenject;
using System.ComponentModel;
using Enemies;
using UnityEngine;
using ModestTree.Util;

namespace Tests
{
    public class TestShotInstaller : MonoInstaller
    {
        [Preserve]
        public override void InstallBindings()
        {
            Debug.Log("Install test shot");
            Container.Bind<ChessPiece>().FromComponentInHierarchy(false).AsSingle();
        }
    }
}