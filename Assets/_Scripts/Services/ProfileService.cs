using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ReviewData
{
    public string locationId;
    public string text;
    public int avatarIndex;
}

public class ProfileService
{
    private const string NameKey = "Profile_Name";
    private const string AvatarKey = "Profile_Avatar";
    private const string PlacesKey = "Profile_Places";
    private const string ReviewsKey = "Profile_Reviews";

    public string Name => PlayerPrefs.GetString(NameKey, "Guest");
    public int AvatarIndex => PlayerPrefs.GetInt(AvatarKey, 0);
    public string Places => PlayerPrefs.GetString(PlacesKey, "");

    public List<string> VisitedPlaces { get; } = new();
    public List<ReviewData> Reviews { get; private set; } = new();

    public void SetName(string name) => PlayerPrefs.SetString(NameKey, name);
    public void SetAvatar(int index)
    {
        PlayerPrefs.SetInt(AvatarKey, index);
        UpdateAvatarInAllReviews(index); // 👈 сохраняем новый индекс во всех отзывах
    }
    public void SetPlaces(string text) => PlayerPrefs.SetString(PlacesKey, text);

    public void AddVisitedPlace(string place)
    {
        if (!VisitedPlaces.Contains(place))
            VisitedPlaces.Add(place);
    }

    public void AddReview(string locationId, string text, int avatarIndex)
    {
        Reviews.Add(new ReviewData { locationId = locationId, text = text, avatarIndex = avatarIndex });
        SaveReviews();
    }

    public void UpdateAvatarInAllReviews(int newIndex)
    {
        foreach (var review in Reviews)
        {
            review.avatarIndex = newIndex;
        }
        SaveReviews();
    }


    public List<ReviewData> GetReviewsFor(string locationId)
    {
        return Reviews.FindAll(r => r.locationId == locationId);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(ReviewsKey))
        {
            string json = PlayerPrefs.GetString(ReviewsKey);
            var wrapper = JsonUtility.FromJson<ReviewWrapper>(json);
            Reviews = wrapper?.items ?? new List<ReviewData>();
        }
    }

    private void SaveReviews()
    {
        var wrapper = new ReviewWrapper { items = Reviews };
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(ReviewsKey, json);
    }

    [System.Serializable]
    private class ReviewWrapper
    {
        public List<ReviewData> items;
    }
}
