using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Collections.Generic;

public class ProfileView : MonoBehaviour
{
    [Header("Основное")]
    [SerializeField] private Text nameText;
    [SerializeField] private Button editBtn;
    [SerializeField] private GameObject editPanel;

    [Header("Edit Profile UI")]
    [SerializeField] private InputField inputName;
    [SerializeField] private Transform avatarChooseRoot;
    [SerializeField] private InputField inputPlaces;
    [SerializeField] private Text _placesNumber;
    [SerializeField] private Button saveBtn;
    //[SerializeField] private Button closeBtn;

    [Header("Отзывы")]
    [SerializeField] private Transform reviewsContainer;
    [SerializeField] private GameObject reviewItemPrefab;

    [Header("Аватарки")]
    [SerializeField] private Image avatarImage;
    [SerializeField] private Sprite[] avatarSprites;

    private ProfileService _profile;
    private int _selectedAvatarIndex = 12;

    [Inject]
    public void Construct(ProfileService profile)
    {
        _profile = profile;
        _profile.Load();
        Refresh();
    }

    private void Start()
    {
        inputPlaces.contentType = InputField.ContentType.IntegerNumber;
        inputPlaces.onValidateInput += (text, charIndex, addedChar) =>
        {
            return char.IsDigit(addedChar) ? addedChar : '\0';
        };

        editBtn.onClick.AddListener(() =>
        {
            editPanel.SetActive(true);
            //closeBtn.gameObject.SetActive(true);
            //saveBtn.gameObject.SetActive(false);
            inputName.text = _profile.Name;
            inputPlaces.text = _profile.Places;
            _selectedAvatarIndex = _profile.AvatarIndex;
            UpdateAvatarUI();
        });

        saveBtn.onClick.AddListener(() =>
        {
            _profile.SetName(inputName.text);
            _profile.SetPlaces(inputPlaces.text);
            _profile.SetAvatar(_selectedAvatarIndex);
            _profile.UpdateAvatarInAllReviews(_selectedAvatarIndex);

            Refresh();
            editPanel.SetActive(false);
        });

        //closeBtn.onClick.AddListener(() =>
        //{
        //    editPanel.SetActive(false);
        //    saveBtn.gameObject.SetActive(true);
        //    closeBtn.gameObject.SetActive(false);
        //});

        // Обработка кнопок аватаров
        for (int i = 0; i < avatarChooseRoot.childCount; i++)
        {
            int index = i;
            var btn = avatarChooseRoot.GetChild(i).GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() =>
                {
                    _selectedAvatarIndex = index;
                    UpdateAvatarUI();
                });
            }
        }

        editPanel.SetActive(false);
        Refresh();
    }

    public void Refresh()
    {
        if (_profile == null)
        {
            Debug.LogError("ProfileService not injected!");
            return;
        }

        nameText.text = _profile.Name;

        if (_placesNumber != null)
            _placesNumber.text = _profile.Places;

        if (avatarSprites.Length > _profile.AvatarIndex)
        {
            avatarImage.sprite = avatarSprites[_profile.AvatarIndex];
        }

        foreach (Transform child in reviewsContainer)
            Destroy(child.gameObject);

        foreach (var review in _profile.Reviews)
        {
            var item = Instantiate(reviewItemPrefab, reviewsContainer);
            var texts = item.GetComponentsInChildren<Text>();
            var avatar = item.transform.GetChild(0).GetComponent<Image>();

            if (texts.Length >= 2)
            {
                texts[0].text = _profile.Name;
                texts[1].text = review.text;
            }

            if (avatar != null && avatarSprites.Length > review.avatarIndex)
            {
                avatar.sprite = avatarSprites[review.avatarIndex];
            }
        }
    }

    private void UpdateAvatarUI()
    {
        for (int i = 0; i < avatarChooseRoot.childCount; i++)
        {
            var marker = avatarChooseRoot.GetChild(i).GetChild(0).gameObject;
            marker.SetActive(i == _selectedAvatarIndex);
        }

        if (avatarSprites.Length > _selectedAvatarIndex)
        {
            avatarImage.sprite = avatarSprites[_selectedAvatarIndex];
        }

        foreach (Transform child in reviewsContainer)
        {
            var image = child.GetComponentInChildren<Image>();
            if (image != null && avatarSprites.Length > _selectedAvatarIndex)
            {
                image.sprite = avatarSprites[_selectedAvatarIndex];
            }
        }

        _profile.SetAvatar(_selectedAvatarIndex);
        _profile.UpdateAvatarInAllReviews(_selectedAvatarIndex);
    }



    public void InjectManually(ProfileService profile)
    {
        _profile = profile;
        Refresh();
        editPanel.SetActive(false);
    }
}
