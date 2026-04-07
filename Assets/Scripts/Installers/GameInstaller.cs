using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameEvents>().AsSingle().NonLazy();

        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}