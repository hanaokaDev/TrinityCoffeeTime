
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;

    void Start()
    {
        AudioManager.instance.PlayBgm(true); // BGM 재생
    }

    void OnNewGameButtonClicked()
    {
        SceneManager.LoadScene("GameScene"); // 게임 씬으로 전환
    }

    void OnExitButtonClicked()
    {
        if (Application.isEditor) // 에디터에서 실행중일 때
        {
            UnityEditor.EditorApplication.isPlaying = false; // 에디터 종료
        }
        else // 빌드된 애플리케이션에서 실행중일 때
        {
            Application.Quit(); // 애플리케이션 종료
        }
    }
}
