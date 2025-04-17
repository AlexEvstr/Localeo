using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class GuideInstaller : MonoInstaller
{
    [SerializeField] private List<LocationSO> locations;
    [SerializeField] private ProfileView profileView;

    public override void InstallBindings()
    {
        Container.Bind<List<LocationSO>>().FromInstance(locations).AsSingle();
        Container.Bind<LocationRepository>().AsSingle().NonLazy();
        Container.Bind<FavoritesService>().AsSingle();
        Container.Bind<ProfileService>().AsSingle();
    }

}
