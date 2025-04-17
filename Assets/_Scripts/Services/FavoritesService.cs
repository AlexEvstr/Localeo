using System.Collections.Generic;
using UnityEngine;

public class FavoritesService
{
    private const string FavoritesKey = "Favorites";
    private readonly HashSet<string> _favorites = new();

    public FavoritesService()
    {
        Load();
    }

    public void Add(string id)
    {
        _favorites.Add(id);
        Save();
    }

    public void Remove(string id)
    {
        _favorites.Remove(id);
        Save();
    }

    public bool IsFavorite(string id) => _favorites.Contains(id);

    public List<string> GetAll() => new(_favorites);

    private void Save()
    {
        string json = JsonUtility.ToJson(new FavoritesWrapper { ids = new List<string>(_favorites) });
        PlayerPrefs.SetString(FavoritesKey, json);
    }

    private void Load()
    {
        _favorites.Clear();
        if (PlayerPrefs.HasKey(FavoritesKey))
        {
            string json = PlayerPrefs.GetString(FavoritesKey);
            var wrapper = JsonUtility.FromJson<FavoritesWrapper>(json);
            if (wrapper?.ids != null)
            {
                foreach (var id in wrapper.ids)
                    _favorites.Add(id);
            }
        }
    }

    [System.Serializable]
    private class FavoritesWrapper
    {
        public List<string> ids;
    }
}
