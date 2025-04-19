using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationDetailsView : MonoBehaviour
{
    [Header("Основное")]
    [SerializeField] private Image locationsImage;
    [SerializeField] private Image featureImage;
    [SerializeField] private Text titleText;
    [SerializeField] private Text subtitleText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Sprite[] avatarSprites;

    [Header("Избранное")]
    [SerializeField] private Button heartButton;
    [SerializeField] private GameObject heartFull;

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
    [SerializeField] private Button backBtn;

    private LocationSO _location;
    private FavoritesService _favorites;
    private ProfileService _profile;
    private ProfileView _profileView;

    public void Init(FavoritesService favorites, ProfileService profile, ProfileView profileView)
    {
        _favorites = favorites;
        _profile = profile;
        _profileView = profileView;
    }

    public void Show(LocationSO location)
    {
        _location = location;
        gameObject.SetActive(true);
        reviewFormRoot.SetActive(false);
        feedbackSent.SetActive(false);
        // Изначально выключаем кнопку
        sendBtn.interactable = false;

        // Слушаем изменения текста
        reviewInput.onValueChanged.RemoveAllListeners();
        reviewInput.onValueChanged.AddListener(text =>
        {
            sendBtn.interactable = !string.IsNullOrWhiteSpace(text);
        });
        
        locationsImage.sprite = location.detailImage;
        featureImage.sprite = location.featureImage;

        titleText.text = location.title;
        descriptionText.text = location.description;

        subtitleText.gameObject.SetActive(!string.IsNullOrWhiteSpace(location.subtitle));
        subtitleText.text = location.subtitle;


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
            SFXManager.Instance.PlayClickSound();
        });

        writeReviewBtn.onClick.RemoveAllListeners();
        writeReviewBtn.onClick.AddListener(() =>
        {
            reviewFormRoot.SetActive(true);
            feedbackSent.SetActive(false);
            SFXManager.Instance.PlayClickSound();
        });

        sendBtn.onClick.RemoveAllListeners();
        sendBtn.onClick.AddListener(() =>
        {
            if (!string.IsNullOrWhiteSpace(reviewInput.text))
            {
                _profile.AddReview(_location.id, reviewInput.text, _profile.AvatarIndex);

                reviewInput.text = "";
                feedbackSent.SetActive(true);
                UpdateReviews();
                _profileView?.Refresh(); // 👈 ОБНОВЛЯЕМ PROFILE VIEW
                SFXManager.Instance.PlayClickSound();
            }
        });

        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            SFXManager.Instance.PlayClickSound();
        });
        
        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            SFXManager.Instance.PlayClickSound();
        }
        );
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

        // 1. Добавляем фейковые
        foreach (var fake in _location.fakeReviews)
        {
            var item = Instantiate(reviewItemPrefab, reviewsContainer);
            var texts = item.GetComponentsInChildren<Text>();
            var images = item.transform.GetChild(0).GetComponentsInChildren<Image>();

            if (texts.Length >= 2)
            {
                texts[0].text = fake.authorName;
                texts[1].text = fake.text;
            }
            if (images.Length > 0)
            {
                images[0].sprite = avatarSprites[fake.avatarIndex];
            }
        }

        // 2. Добавляем настоящие отзывы пользователя
        var reviews = _profile.GetReviewsFor(_location.id);
        foreach (var review in reviews)
        {
            var item = Instantiate(reviewItemPrefab, reviewsContainer);
            var texts = item.GetComponentsInChildren<Text>();
            var images = item.transform.GetChild(0).GetComponentsInChildren<Image>();

            if (texts.Length >= 2)
            {
                texts[0].text = "👤 You";
                texts[1].text = review.text;
            }
            if (images.Length > 0)
            {
                images[0].sprite = avatarSprites[review.avatarIndex];
            }
        }
    }

}
