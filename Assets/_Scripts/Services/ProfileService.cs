using UnityEngine;
using System.Collections.Generic;

public class ProfileService
{
    private const string NameKey = "Profile_Name";
    private const string AvatarKey = "Profile_Avatar";
    private const string PlacesKey = "Profile_Places";

    public string Name => PlayerPrefs.GetString(NameKey, "Guest");
    public int AvatarIndex => PlayerPrefs.GetInt(AvatarKey, 0);
    public string Places => PlayerPrefs.GetString(PlacesKey, "");

    public List<string> VisitedPlaces { get; } = new();
    public List<string> Reviews { get; } = new();

    public void SetName(string name)
    {
        PlayerPrefs.SetString(NameKey, name);
    }

    public void SetAvatar(int index)
    {
        PlayerPrefs.SetInt(AvatarKey, index);
    }

    public void SetPlaces(string text)
    {
        PlayerPrefs.SetString(PlacesKey, text);
    }

    public void AddVisitedPlace(string place)
    {
        if (!VisitedPlaces.Contains(place))
            VisitedPlaces.Add(place);
    }

    public void AddReview(string text)
    {
        Reviews.Add(text);
    }
}
