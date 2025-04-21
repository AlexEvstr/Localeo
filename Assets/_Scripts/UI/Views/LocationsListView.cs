using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class LocationsListView : MonoBehaviour
{
    [SerializeField] private LocationItemView itemPrefab;
    [SerializeField] private Transform container;

    private LocationRepository _locationRepo;
    private FavoritesService _favorites;
    private System.Action<string> _onOpenDetails;
    public Transform Container => container;
    public LocationItemView GetPrefab() => itemPrefab;


    [Inject]
    public void Construct(LocationRepository locationRepo, FavoritesService favorites)
    {
        _locationRepo = locationRepo;
        _favorites = favorites;
    }

    public void Show(LocationCategory category, System.Action<string> onOpenDetails)
    {
        _onOpenDetails = onOpenDetails;
        //Clear();

        List<LocationSO> locations = new(_locationRepo.GetByCategory(category));
        foreach (var loc in locations)
        {
            var item = Instantiate(itemPrefab, container);
            item.Setup(loc, _onOpenDetails, ToggleFavorite, LocationItemMode.Default);
        }
    }

    private void Clear()
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
    }

    private void ToggleFavorite(string id)
    {
        if (_favorites.IsFavorite(id))
            _favorites.Remove(id);
        else
            _favorites.Add(id);
    }

    private bool IsFavorite(string id) => _favorites.IsFavorite(id);
}