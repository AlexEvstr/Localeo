using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TabsController : MonoBehaviour
{
    [SerializeField] private GameObject allLocationsTab;
    [SerializeField] private GameObject favoritesTab;
    [SerializeField] private GameObject profileTab;

    [SerializeField] private Button allButton;
    [SerializeField] private Button favoritesButton;
    [SerializeField] private Button profileButton;

    [SerializeField] private LocationsListView restaurantsList;
    [SerializeField] private LocationsListView parksList;
    [SerializeField] private LocationsListView museumsList;
    [SerializeField] private FavoritesView favoritesView;
    [SerializeField] private ProfileView profileView;
    [SerializeField] private LocationDetailsView detailsViewPrefab;
    private LocationDetailsView detailsViewInstance;

    private FavoritesService _favorites;
    private ProfileService _profile;

    private readonly Color selectedColor = Color.white;
    private readonly Color unselectedColor = new Color(0.9f, 0.6f, 0.9f, 1f);

    private LocationRepository _locationRepo;

    [Inject]
    public void Construct(LocationRepository repo, FavoritesService favorites, ProfileService profile)
    {
        _locationRepo = repo;
        _favorites = favorites;
        _profile = profile;
    }


    private void Start()
    {
        allButton.onClick.AddListener(ShowAllTab);
        favoritesButton.onClick.AddListener(ShowFavoritesTab);
        profileButton.onClick.AddListener(ShowProfileTab);

        ShowAllTab();
    }

    private void ShowAllTab()
    {
        allLocationsTab.SetActive(true);
        favoritesTab.SetActive(false);
        profileTab.SetActive(false);

        restaurantsList.Show(LocationCategory.Restaurant, ShowDetails);
        parksList.Show(LocationCategory.Park, ShowDetails);
        museumsList.Show(LocationCategory.Museum, ShowDetails);

        SetTabColors(allButton, favoritesButton, profileButton);
    }

    private void ShowFavoritesTab()
    {
        allLocationsTab.SetActive(false);
        favoritesTab.SetActive(true);
        profileTab.SetActive(false);

        favoritesView.Show(ShowDetails);

        SetTabColors(favoritesButton, allButton, profileButton);
    }

    private void ShowProfileTab()
    {
        allLocationsTab.SetActive(false);
        favoritesTab.SetActive(false);
        profileTab.SetActive(true);

        SetTabColors(profileButton, allButton, favoritesButton);
    }

    private void SetTabColors(Button active, Button other1, Button other2)
    {
        active.GetComponent<Image>().color = selectedColor;
        other1.GetComponent<Image>().color = unselectedColor;
        other2.GetComponent<Image>().color = unselectedColor;
    }


    private void ShowDetails(string locationId)
    {
        var location = _locationRepo.GetById(locationId);
        if (location == null)
        {
            Debug.LogWarning("Location not found!");
            return;
        }

        // Если ещё не создан — создаём инстанс из префаба
        if (detailsViewInstance == null)
        {
            // Предполагаем, что твой Canvas находится в сцене и ты хочешь добавить в него
            var canvas = FindObjectOfType<Canvas>();
            detailsViewInstance = Instantiate(detailsViewPrefab, canvas.transform);
        }

        detailsViewInstance.Init(_favorites, _profile, profileView);
        detailsViewInstance.Show(location);
    }


}
