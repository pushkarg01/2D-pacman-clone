using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameEvents>().AsSingle().NonLazy();

        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.Bind<Movement>().FromComponentInHierarchy().AsSingle().NonLazy();
        //  Container.Bind<Ghost>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.Bind<Pellets>()
    .FromComponentInHierarchy()
    .AsCached();
    }
}