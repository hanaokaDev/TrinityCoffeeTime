
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    // {{{
    [SerializeField]
    private Sprite[] playerItemSprites; // 플레이어 아이템 스프라이트 배열
    // }}}

    public Sprite GetItemSprite(PlayerItem item)
    {
        return playerItemSprites[(int)item];
    }

}
