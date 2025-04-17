using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class EnterSceneLoader : MonoBehaviour
{
    private async void Start()
    {
        await UniTask.Delay(3000);
        SceneManager.LoadScene("MainScene");
    }
}