using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class GuideInstaller : MonoInstaller
{
    [SerializeField] private List<LocationSO> locations;
    [SerializeField] private ProfileView profileView;
    [SerializeField] private LocationDetailsView detailsView;

    public override void InstallBindings()
    {
        Container.Bind<List<LocationSO>>().FromInstance(locations).AsSingle();
        Container.Bind<LocationRepository>().AsSingle().NonLazy();
        Container.Bind<FavoritesService>().AsSingle();
        Container.Bind<ProfileService>().AsSingle();

        var favorites = Container.Resolve<FavoritesService>();
        var profile = Container.Resolve<ProfileService>();

        profile.Load();

        profileView.InjectManually(profile);

        // 👇 передаём и profileView
        detailsView.Init(favorites, profile, profileView);
    }
}
