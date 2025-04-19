
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CafeSceneManager : MonoBehaviour
{
    public void Start()
    {
        AudioManager.instance.PlayBgm(true, AudioManager.BGM.CoffeeCats); // BGM 재생
    }

    public void OnTestButtonClick()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonSelect); // 버튼 클릭 사운드 재생
        Debug.Log("Test Button Clicked"); // 테스트 버튼 클릭 시 로그 출력
    }
}
