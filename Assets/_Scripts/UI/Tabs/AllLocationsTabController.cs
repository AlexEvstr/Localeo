using UnityEngine;
using UnityEngine.UI;

public class AllLocationsTabController : MonoBehaviour
{
    [Header("Секции локаций")]
    [SerializeField] private GameObject restaurantsSection;
    [SerializeField] private GameObject parksSection;
    [SerializeField] private GameObject museumsSection;

    [Header("Кнопки открытия секций")]
    [SerializeField] private Button openRestaurantsButton;
    [SerializeField] private Button openParksButton;
    [SerializeField] private Button openMuseumsButton;

    [Header("Кнопки возврата")]
    [SerializeField] private Button backFromRestaurantsButton;
    [SerializeField] private Button backFromParksButton;
    [SerializeField] private Button backFromMuseumsButton;

    private void Start()
    {
        openRestaurantsButton.onClick.AddListener(() => ShowSection(restaurantsSection));
        openParksButton.onClick.AddListener(() => ShowSection(parksSection));
        openMuseumsButton.onClick.AddListener(() => ShowSection(museumsSection));

        backFromRestaurantsButton.onClick.AddListener(ShowMainMenu);
        backFromParksButton.onClick.AddListener(ShowMainMenu);
        backFromMuseumsButton.onClick.AddListener(ShowMainMenu);

        ShowMainMenu();
    }

    private void ShowSection(GameObject sectionToShow)
    {
        restaurantsSection.SetActive(sectionToShow == restaurantsSection);
        parksSection.SetActive(sectionToShow == parksSection);
        museumsSection.SetActive(sectionToShow == museumsSection);
    }

    private void ShowMainMenu()
    {
        restaurantsSection.SetActive(false);
        parksSection.SetActive(false);
        museumsSection.SetActive(false);
    }
}
