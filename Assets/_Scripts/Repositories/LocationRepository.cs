using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class LocationRepository : IInitializable
{
    [Inject] private List<LocationSO> _locations;

    public IReadOnlyList<LocationSO> GetAll() => _locations;

    public IReadOnlyList<LocationSO> GetByCategory(LocationCategory category) =>
        _locations.Where(l => l.category == category).ToList();

    public LocationSO GetById(string id) =>
        _locations.FirstOrDefault(l => l.id == id);

    public void Initialize()
    {
        Debug.Log($"LocationRepository initialized. Total: {_locations.Count}");
    }
}
