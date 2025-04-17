using UnityEngine;

[CreateAssetMenu(menuName = "Location")]
public class LocationSO : ScriptableObject
{
    public string id;
    public string title;
    public string description;
    public LocationCategory category;

    public Sprite thumbnailImage;    // Для списка
    public Sprite detailImage;       // Для деталей
    public Sprite featureImage;     // Для окна отзыва
}