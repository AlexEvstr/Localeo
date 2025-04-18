using UnityEngine;
using UnityEngine.UI;

public enum LocationItemMode
{
    Default,    // Только кнопка Open
    Favorites   // Только кнопка Delete
}

public class LocationItemView : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text titleText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Button detailsButton;

    [Header("Favorites mode only")]
    [SerializeField] private Button deleteButton;

    private string locationId;
    private System.Action<string> onOpenDetails;
    private System.Action<string> onRemoveFavorite;

    public void Setup(LocationSO data, System.Action<string> onOpen, System.Action<string> onRemove = null, LocationItemMode mode = LocationItemMode.Default)
    {
        locationId = data.id;
        titleText.text = data.title;
        descriptionText.text = data.description;
        image.sprite = data.thumbnailImage;

        onOpenDetails = onOpen;
        onRemoveFavorite = onRemove;

        detailsButton.onClick.RemoveAllListeners();
        detailsButton.onClick.AddListener(() =>
        {
            onOpenDetails?.Invoke(locationId);
        });


        // Режимы
        deleteButton.gameObject.SetActive(mode == LocationItemMode.Favorites);

        if (mode == LocationItemMode.Favorites && deleteButton != null)
        {
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() => onRemoveFavorite?.Invoke(locationId));
        }
    }
}
