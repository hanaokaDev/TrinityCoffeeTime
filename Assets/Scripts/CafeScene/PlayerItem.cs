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

public class PlayerItem : MonoBehaviour
{
    public PlayerItemEnum itemType; // 아이템 종류
    public PlayerItemEnum[] Ingredients; // 추가적으로 들어간 재료들(딸기시럽 등)

    // 맛 레벨 (0~10) - 각 맛에 대한 레벨을 저장하는 배열
    public TasteLevels tasteLevel
    {
        get { return CalculateTasteLevel(itemType, Ingredients); }
        set { /* setter는 필요 없으므로 비워둠 */ }
    }
    // public TasteLevels tasteLevel = { get { return CalculateTasteLevel(); } set {};}

    // 각 아이템의 기본 맛 레벨을 저장하는 배열
    static public TasteLevels[] defaultTasteLevel = new TasteLevels[Enum.GetNames(typeof(PlayerItemEnum)).Length];
    public void SetDefaultTasteLevel()
    {
        for (int itemType = 0; itemType < PlayerItemEnum.GetNames(typeof(PlayerItemEnum)).Length; itemType++)
        {
            // null 체크
            if (defaultTasteLevel[itemType] == null)
            {
                Debug.LogWarning("defaultTasteLevel is null for itemType: " + itemType);
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
    }

    // 생성자 선언
    public PlayerItem(PlayerItemEnum itemTypeArg, PlayerItemEnum[] ingredientsArgs)
    {
        itemType = itemTypeArg;
        Ingredients = ingredientsArgs;
        SetDefaultTasteLevel(); // 기본 맛 레벨 설정        
    }

    public TasteLevels CalculateTasteLevel(PlayerItemEnum baseIngredient, PlayerItemEnum[] additionalIngredients)
    {
        TasteLevels totalTasteLevel = new TasteLevels();
        totalTasteLevel.tasteLevels = (int[])defaultTasteLevel[(int)baseIngredient].tasteLevels.Clone(); // 기본 맛 레벨 복사

        foreach (var ingredient in additionalIngredients)
        {
            // // 각 재료의 맛 레벨을 계산하여 합산
            // var ingredientTasteLevel = ingredient.GetComponent<PlayerItem>().tasteLevel;
            // for (int i = 0; i < tasteLevel.Length; i++)
            // {
            //     tasteLevel[i] += ingredientTasteLevel[i];
            // }
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

    public PlayerMover owner; // 이 아이템을 소유한 플레이어
    public void SetOwner(PlayerMover player)
    {
        owner = player;
    }
    public void RemoveThisItem()
    {
        if (owner != null)
        {
            owner.RemoveItem(itemType);
        }
        Destroy(gameObject); // 아이템 오브젝트 제거
    }



}