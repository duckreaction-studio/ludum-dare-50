using UnityEngine;
using Zenject;

namespace Enemies
{
    public class ChessboardInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("Install chessboard");
            Container.Bind<Chessboard>().FromComponentInHierarchy(false).AsSingle();
        }
    }
}