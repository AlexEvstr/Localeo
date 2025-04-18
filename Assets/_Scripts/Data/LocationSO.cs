using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FakeReview
{
    public string authorName;
    public int avatarIndex;
    [TextArea(2, 5)] public string text;
}

[CreateAssetMenu(menuName = "Location")]
public class LocationSO : ScriptableObject
{
    public string id;
    public string title;
    public string description;
    public string subtitle;
    public LocationCategory category;

    public Sprite thumbnailImage;    // Для списка
    public Sprite detailImage;       // Для деталей
    public Sprite featureImage;     // Для окна отзыва
    public List<FakeReview> fakeReviews;
}