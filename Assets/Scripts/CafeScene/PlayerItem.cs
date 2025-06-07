using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TasteEnum
{
    None,
    SWEET,
    BITTER,
    SOUR,
    SALTY,
    SPICY,
    UMAMI,
    STRANGE,
}

public class TasteLevels
{
    public int[] tasteLevels = new int[Enum.GetNames(typeof(TasteEnum)).Length];
}

[System.Serializable]
public class PlayerItemData
{
    public PlayerItemEnum itemType;
    public PlayerItemEnum[] Ingredients;
    public string uniqueId;

    // Empty 객체 캐싱
    public static readonly PlayerItemData Empty = new PlayerItemData(PlayerItemEnum.NONE, new PlayerItemEnum[] { })
    {
        uniqueId = "empty"
    };
    
    // 각 아이템의 기본 맛 레벨을 저장하는 배열. 배열이 readonly라도 배열내용자체는 변경가능.
    static public readonly TasteLevels[] defaultTasteLevel = new TasteLevels[Enum.GetNames(typeof(PlayerItemEnum)).Length];

    static PlayerItemData()
    {
        // 기본 맛 레벨 초기화
        for (int itemType = 0; itemType < PlayerItemEnum.GetNames(typeof(PlayerItemEnum)).Length; itemType++)
        {
            // null 체크
            if (defaultTasteLevel[itemType] == null)
            {
                defaultTasteLevel[itemType] = new TasteLevels();
            }

            // 기본 맛 레벨을 초기화
            for (int i = 0; i < defaultTasteLevel[itemType].tasteLevels.Length; i++)
            {
                defaultTasteLevel[itemType].tasteLevels[i] = 0;
            }

            // 아이템 종류에 따라 기본 맛 레벨 설정
            switch (itemType)
            {
                case (int)PlayerItemEnum.WATER:
                    defaultTasteLevel[itemType].tasteLevels[(int)TasteEnum.SWEET] = 1;
                    break;
                case (int)PlayerItemEnum.ESPRESSO:
                    defaultTasteLevel[itemType].tasteLevels[(int)TasteEnum.BITTER] = 5;
                    break;
                case (int)PlayerItemEnum.AMERICANO:
                    defaultTasteLevel[itemType].tasteLevels[(int)TasteEnum.BITTER] = 3;
                    defaultTasteLevel[itemType].tasteLevels[(int)TasteEnum.SWEET] = 1;
                    break;
                // 다른 아이템에 대해서도 기본 맛 레벨 설정
                default:
                    break;
            }

        }

        for (int i = 0; i < defaultTasteLevel.Length; i++)
        {
            // 디버그용 출력
            Debug.Log($"Item: {((PlayerItemEnum)i).ToString()}, Taste Levels: {string.Join(", ", defaultTasteLevel[i].tasteLevels)}");
        }

    }

    public PlayerItemData(PlayerItemEnum itemTypeArg, PlayerItemEnum[] ingredientsArgs)
    {
        itemType = itemTypeArg;
        Ingredients = ingredientsArgs;

        // 고유 ID 생성
        uniqueId = System.Guid.NewGuid().ToString();
    }

    // 데이터 관련 로직
    public TasteLevels CalculateTasteLevel()
    {
        TasteLevels totalTasteLevel = new TasteLevels();
        totalTasteLevel.tasteLevels = (int[])defaultTasteLevel[(int)this.itemType].tasteLevels.Clone(); // 기본 맛 레벨 복사

        foreach (var ingredient in this.Ingredients)
        {
            if (ingredient == PlayerItemEnum.ESPRESSO)
            {
                totalTasteLevel.tasteLevels[(int)TasteEnum.BITTER] += 5; // 에스프레소는 쓴맛이 강함
            }
            else if (ingredient == PlayerItemEnum.STRAWBERRY_SYRUP)
            {
                totalTasteLevel.tasteLevels[(int)TasteEnum.SWEET] += 3; // 딸기 시럽은 단맛이 강함
            }
        }
        return totalTasteLevel;
    }
}

public class PlayerItem : MonoBehaviour
{
    public PlayerItemData data;

    // 맛 레벨 (0~10) - 각 맛에 대한 레벨을 저장하는 배열
    public TasteLevels tasteLevel
    {
        get { return data.CalculateTasteLevel(); }
        set { /* setter는 필요 없으므로 비워둠 */ }
    }    

    public bool IsEmpty() => data == PlayerItemData.Empty || data.itemType == PlayerItemEnum.NONE;

    public void Initialize(PlayerItemData itemData)
    {
        // data = PlayerItemData.Empty;
        data = itemData ?? PlayerItemData.Empty;
    }
    
}