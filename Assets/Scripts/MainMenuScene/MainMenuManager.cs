
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public Button newGameButton;
    public Button quitButton;
    public Button settingButton;


    void Start()
    {
        AudioManager.instance.PlayBgm(true); // BGM 재생
    }

    private void PlayButtonClickSound()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonSelect); // 버튼 클릭 사운드 재생
    }

    public void OnNewGameButtonClicked()
    {
        PlayButtonClickSound();
        SceneManager.LoadScene("GameScene"); // 게임 씬으로 전환
    }

    public void OnQuitButtonClicked()
    {
        PlayButtonClickSound();
        if (Application.isEditor) // 에디터에서 실행중일 때
        {
            UnityEditor.EditorApplication.isPlaying = false; // 에디터 종료
        }
        else // 빌드된 애플리케이션에서 실행중일 때
        {
            Application.Quit(); // 애플리케이션 종료
        }
    }

    // TODO: 설정 버튼 클릭 시 동작 추가
    public void OnSettingButtonClicked()
    {
        PlayButtonClickSound();
        Debug.Log("Setting Button Clicked"); // 설정 버튼 클릭 시 로그 출력
    }
}
