using UnityEngine;
using Zenject;

public class FavoritesView : MonoBehaviour
{
    [SerializeField] private LocationsListView locationsList;

    private FavoritesService _favorites;
    private LocationRepository _repo;
    private System.Action<string> _onOpenDetails; // 👈 сохраняем

    [Inject]
    public void Construct(FavoritesService favorites, LocationRepository repo)
    {
        _favorites = favorites;
        _repo = repo;
    }

    public void Show(System.Action<string> onOpenDetails)
    {
        _onOpenDetails = onOpenDetails; // 👈 сохраняем для Redraw()
        Redraw();
    }

    private void Redraw()
    {
        Clear();

        foreach (var id in _favorites.GetAll())
        {
            var loc = _repo.GetById(id);
            if (loc != null)
            {
                var item = Instantiate(locationsList.GetPrefab(), locationsList.Container);
                item.Setup(loc, _onOpenDetails, ToggleFavorite, LocationItemMode.Favorites);
            }
        }
    }

    private void Clear()
    {
        foreach (Transform child in locationsList.Container)
            Destroy(child.gameObject);
    }

    private void ToggleFavorite(string id)
    {
        if (_favorites.IsFavorite(id))
            _favorites.Remove(id);

        Redraw();
    }

    private bool IsFavorite(string id) => _favorites.IsFavorite(id);
}

