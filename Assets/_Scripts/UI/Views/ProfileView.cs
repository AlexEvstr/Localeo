using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ProfileView : MonoBehaviour
{
    [Header("Основное")]
    [SerializeField] private Text nameText;
    [SerializeField] private Button editBtn;
    [SerializeField] private Text numberOfPlacesLabel;
    [SerializeField] private GameObject editPanel;

    [Header("Edit Profile UI")]
    [SerializeField] private InputField inputName;
    [SerializeField] private Transform avatarChooseRoot;
    [SerializeField] private InputField inputPlaces;
    [SerializeField] private Button saveBtn;
    [SerializeField] private Button closeBtn;

    [Header("Отзывы")]
    [SerializeField] private Transform reviewsContainer;
    [SerializeField] private GameObject reviewItemPrefab;

    private ProfileService _profile;
    private int _selectedAvatarIndex = 0;

    [Inject]
    public void Construct(ProfileService profile)
    {
        _profile = profile;
    }

    private void Start()
    {
        editBtn.onClick.AddListener(() =>
        {
            editPanel.SetActive(true);
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

            Refresh();
            editPanel.SetActive(false);
        });

        closeBtn.onClick.AddListener(() =>
        {
            editPanel.SetActive(false);
        });

        // Аватары
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

    private void Refresh()
    {
        if (_profile == null)
        {
            Debug.LogError("ProfileService not injected!");
            return;
        }

        nameText.text = _profile.Name;
        numberOfPlacesLabel.text = _profile.Places;

        foreach (Transform child in reviewsContainer)
            Destroy(child.gameObject);

        foreach (var review in _profile.Reviews)
        {
            var item = Instantiate(reviewItemPrefab, reviewsContainer);
            var texts = item.GetComponentsInChildren<Text>();
            if (texts.Length >= 2)
            {
                texts[0].text = _profile.Name;
                texts[1].text = review;
            }
        }
    }

    private void UpdateAvatarUI()
    {
        for (int i = 0; i < avatarChooseRoot.childCount; i++)
        {
            avatarChooseRoot.GetChild(i).gameObject.SetActive(i == _selectedAvatarIndex);
        }
    }
}
