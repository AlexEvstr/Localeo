using UnityEngine;
using UnityEngine.UI;

public class LocationDetailsView : MonoBehaviour
{
    [Header("Основное")]
    //[SerializeField] private GameObject root; // сам LocationDetailsView
    [SerializeField] private Image locationsImage;
    [SerializeField] private Image featureImage;
    [SerializeField] private Text titleText;
    [SerializeField] private Text subtitleText;
    [SerializeField] private Text descriptionText;

    [Header("Избранное")]
    [SerializeField] private Button heartButton;
    [SerializeField] private GameObject heartFull; // иконка "в избранном"

    [Header("Отзывы")]
    [SerializeField] private Transform reviewsContainer;
    [SerializeField] private GameObject reviewItemPrefab;
    [SerializeField] private Button writeReviewBtn;

    [Header("Форма отзыва")]
    [SerializeField] private GameObject reviewFormRoot;
    [SerializeField] private InputField reviewInput;
    [SerializeField] private Button sendBtn;
    [SerializeField] private GameObject feedbackSent;

    [Header("Прочее")]
    [SerializeField] private Button closeBtn;

    private LocationSO _location;
    private FavoritesService _favorites;
    private ProfileService _profile;

    public void Init(FavoritesService favorites, ProfileService profile)
    {
        _favorites = favorites;
        _profile = profile;
    }

    public void Show(LocationSO location)
    {
        _location = location;
        Debug.Log($"[LocationDetailsView] Showing: {location.title}");
        gameObject.SetActive(true);
        //root.SetActive(true);
        reviewFormRoot.SetActive(false);
        feedbackSent.SetActive(false);

        locationsImage.sprite = location.detailImage;
        featureImage.sprite = location.featureImage;

        titleText.text = location.title;
        descriptionText.text = location.description;

        subtitleText.gameObject.SetActive(location.category == LocationCategory.Restaurant);
        subtitleText.text = location.category == LocationCategory.Restaurant ? "Top rated restaurant 🍽" : "";

        UpdateHeartIcon();
        UpdateReviews();

        heartButton.onClick.RemoveAllListeners();
        heartButton.onClick.AddListener(() =>
        {
            if (_favorites.IsFavorite(location.id))
                _favorites.Remove(location.id);
            else
                _favorites.Add(location.id);

            UpdateHeartIcon();
        });

        writeReviewBtn.onClick.RemoveAllListeners();
        writeReviewBtn.onClick.AddListener(() =>
        {
            reviewFormRoot.SetActive(true);
            feedbackSent.SetActive(false);
        });

        sendBtn.onClick.RemoveAllListeners();
        sendBtn.onClick.AddListener(() =>
        {
            if (!string.IsNullOrWhiteSpace(reviewInput.text))
            {
                _profile.AddReview(reviewInput.text);
                reviewInput.text = "";
                feedbackSent.SetActive(true);
                UpdateReviews();
            }
        });

        closeBtn.onClick.RemoveAllListeners();
        //closeBtn.onClick.AddListener(() => root.SetActive(false));
        closeBtn.onClick.AddListener(() => gameObject.SetActive(false));
    }

    private void UpdateHeartIcon()
    {
        bool isFav = _favorites.IsFavorite(_location.id);
        heartFull.SetActive(isFav);
    }

    private void UpdateReviews()
    {
        foreach (Transform child in reviewsContainer)
            Destroy(child.gameObject);

        foreach (var review in _profile.Reviews)
        {
            var item = Instantiate(reviewItemPrefab, reviewsContainer);
            var texts = item.GetComponentsInChildren<Text>();
            if (texts.Length >= 2)
            {
                texts[0].text = "👤 You";
                texts[1].text = review;
            }
        }
    }
}
