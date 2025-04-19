using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    AudioSource bgmPlayer; // BGM은 동시 1개만 재생되므로 AudioSource 1개만 필요하다.

    // 1. AudioHighPassFilter 는 ListenerEffect 계열이다. 
    // 2. 따라서 AudioHighPassFilter 는 MainCamera에 넣어야 한다. 
    // 3. 또한, Effect 영향을 받는 AudioSource에는 BypassListenerEffects 가 false 되어있어야 한다.
    // 4. 따라서, Effect 영향을 받아야 하는 BGM은 false로, Sfx는 true로 해야한다.
    AudioHighPassFilter bgmEffect;
    public enum BGM {LuminousMemory=0, CoffeeCats}


    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels; // channel 개수.
    AudioSource[] sfxPlayers;
    int channelIndex; // 현재 재생중인 channel 의 index

    // public enum Sfx {Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win}
    public enum Sfx {ButtonSelect, ButtonCancel}

    void Awake()
    {
        instance = this; // 어디서든 호출가능하게.
        Init();
        DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 AudioManager는 파괴되지 않도록.
    }

    void Init(){
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.clip = bgmClips[(int)BGM.LuminousMemory]; // 초기값은 null
        bgmPlayer.playOnAwake = false; // 기본값이 true
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.loop = true;
        bgmPlayer.bypassListenerEffects = false; // ListenerEffect 영향을 받는다.
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];
        for(int index=0; index<channels; index++){
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
            sfxPlayers[index].loop = false;
            sfxPlayers[index].bypassListenerEffects = true; // ListenerEffect 영향을 받지 않는다.
        } 
    }
    public void PlaySfx(Sfx sfx)
    {
        for(int index=0; index<sfxPlayers.Length; index++){
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;
            if(sfxPlayers[loopIndex].isPlaying) continue;

            // 랜덤으로 음성을 재생하고자 하면 사용할것.
            /*int ranIndex=0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee){
                ranIndex = UnityEngine.Random.Range(0,2);
            }*/

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void PlayBgm(bool isPlay, BGM bgm = BGM.LuminousMemory)
    {
        if(isPlay){
            bgmPlayer.clip = bgmClips[(int)bgm];
            bgmPlayer.Play();
        } 
        else{
            bgmPlayer.Stop();
        }
    }

    
    public void EffectBgm(bool isEffectOn)
    {
        if(isEffectOn) bgmEffect.enabled = true;
        else bgmEffect.enabled = false;
    }


}
