
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class CafeSceneManager : MonoBehaviour
{
    public static CafeSceneManager Instance; // Singleton instance
    private void Awake()
    {
        Instance = this;
    }

    public IntroUI introUI;

    public Transform npcSpawnPosition;

    public void Start()
    {
        AudioManager.instance.PlayBgm(true, AudioManager.BGM.CoffeeCats); // BGM 재생
        introUI.Open(); // 대사출력 후 자동으로 Close됨.
    }

    public void OnTestButtonClick()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonSelect); // 버튼 클릭 사운드 재생
        Debug.Log("Test Button Clicked"); // 테스트 버튼 클릭 시 로그 출력
    }
}
