using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class DetailedEyeBlinking : MonoBehaviour
{
    [Header("Eye Sprites")]
    [SerializeField] private Image eyeImage; // 하나의 이미지 컴포넌트만 사용
    
    [Header("Sprites in Sequence")]
    [SerializeField] private Sprite eyeOpenImage;       // 완전히 뜬 눈
    [SerializeField] private Sprite eyeOpenImage_0;     // 약간 감기 시작
    [SerializeField] private Sprite eyeClosingImage_0;  // 절반 감김
    [SerializeField] private Sprite eyeClosingImage_1;  // 거의 감김
    [SerializeField] private Sprite eyeClosedImage;     // 완전히 감김
    [SerializeField] private Sprite eyeOpeningImage_0;  // 약간 열림
    [SerializeField] private Sprite eyeOpeningImage_1;  // 거의 열림
    
    [Header("Blink Settings")]
    [SerializeField] private float blinkDuration = 0.3f;    // 한 번 깜빡임의 총 시간
    [SerializeField] private float minBlinkInterval = 5f;   // 최소 깜빡임 간격
    [SerializeField] private float maxBlinkInterval = 10f;  // 최대 깜빡임 간격
    
    private List<Sprite> blinkSequenceSprites = new List<Sprite>();
    private Sequence blinkSequence;
    
    void Start()
    {
        // 깜빡임 순서대로 스프라이트 배열에 추가
        blinkSequenceSprites.Add(eyeOpenImage);       // 0: 시작 (눈 뜸)
        blinkSequenceSprites.Add(eyeOpenImage_0);     // 1: 감기 시작
        blinkSequenceSprites.Add(eyeClosingImage_0);  // 2: 감는 중
        blinkSequenceSprites.Add(eyeClosingImage_1);  // 3: 거의 감김
        blinkSequenceSprites.Add(eyeClosedImage);     // 4: 완전히 감김
        blinkSequenceSprites.Add(eyeOpeningImage_0);  // 5: 열기 시작
        blinkSequenceSprites.Add(eyeOpeningImage_1);  // 6: 거의 열림
        // 마지막에는 다시 eyeOpenImage(첫 번째)로 돌아갑니다
        
        // 초기 이미지 설정
        eyeImage.sprite = eyeOpenImage;
        
        // 깜빡임 시작
        StartBlinking();
    }
    
    void StartBlinking()
    {
        // 기존 시퀀스가 있으면 정리
        if (blinkSequence != null) blinkSequence.Kill();
        
        // 새 시퀀스 생성
        blinkSequence = DOTween.Sequence();
        
        // 랜덤한 시간 간격으로 깜빡임 시작
        float waitTime = Random.Range(minBlinkInterval, maxBlinkInterval);
        blinkSequence.AppendInterval(waitTime);
        
        // 각 프레임 사이의 시간 간격 계산 (동일한 간격)
        float frameDuration = blinkDuration / blinkSequenceSprites.Count;
        
        // 각 스프라이트로 순차적으로 변경
        for (int i = 0; i < blinkSequenceSprites.Count; i++)
        {
            int frameIndex = i; // 클로저 문제 방지를 위한 로컬 변수
            blinkSequence.AppendCallback(() => {
                eyeImage.sprite = blinkSequenceSprites[frameIndex];
            });
            
            // 마지막 프레임이 아니면 대기 시간 추가
            if (i < blinkSequenceSprites.Count - 1)
            {
                blinkSequence.AppendInterval(frameDuration);
            }
        }
        
        // 마지막에 다시 눈 뜬 상태로
        blinkSequence.AppendCallback(() => {
            eyeImage.sprite = eyeOpenImage;
        });
        
        // 시퀀스 완료 후 다시 시작
        blinkSequence.OnComplete(StartBlinking);
    }
    
    void OnDestroy()
    {
        // 시퀀스 정리
        if (blinkSequence != null) blinkSequence.Kill();
    }
}