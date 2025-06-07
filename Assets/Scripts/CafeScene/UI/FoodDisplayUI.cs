using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using Unity.VisualScripting; // for UnityAction


public class FoodDisplayUI : MonoBehaviour
{
    // {{{ To set
    public PlayerMover chosenPlayer;

    public FoodDisplay foodDisplay; // Data관리용
    public Button[] foodButtons = new Button[Enum.GetNames(typeof(PlayerItemEnum)).Length]; // Button관리용
    public Text DescriptionText; // 음식 설명 텍스트
    public string[] FoodDescriptions = new string[Enum.GetNames(typeof(PlayerItemEnum)).Length]; // 음식 설명 텍스트

    // }}}

    void Awake()
    {
        for (int itemType = 0; itemType < Enum.GetNames(typeof(PlayerItemEnum)).Length; itemType++)
        {
            // 현재 itemType 값을 새 변수에 복사
            int capturedItemType = itemType;
            
            if (foodButtons[itemType] == null)
            {
                Debug.LogError($"Food button at index {itemType} is not assigned!");
                continue;
            }
            foodButtons[itemType].onClick.AddListener(() => OnClickButton_FoodItem(capturedItemType)); // 각 버튼에 클릭 이벤트 추가
            Debug.Log($"Food button at index {itemType}, name {foodButtons[itemType].name} assigned with listener.");
        }
    }

    void OnEnable()
    {
        DescriptionText.text = "음식을 선택하세요"; // 초기 설명 텍스트
        for(int itemType = 0; itemType < Enum.GetNames(typeof(PlayerItemEnum)).Length; itemType++)
        {
            if(foodButtons[itemType] == null)
            {
                continue;
            }
            if (foodDisplay.NumberOfFoodItems[itemType] <= 0)
            {
                foodButtons[itemType].interactable = false; // 음식이 없으면 버튼 비활성화
            }
        }
    }


    void UpdateText(PlayerItemEnum itemType)
    {
        // 음식 설명 텍스트를 업데이트
        DescriptionText.text = FoodDescriptions[(int)itemType];
    }

    public void OnClickButton_FoodItem(int itemType)
    {
        // 음식 아이템 버튼 클릭 시 처리
        int remains = foodDisplay.GetFoodItem((PlayerItemEnum)itemType);
        if (remains <= 0)
        {
            foodButtons[itemType].interactable = false; // 음식이 없으면 버튼 비활성화
        }
        DescriptionText.text = FoodDescriptions[itemType]; // 음식 설명 업데이트
    }

    public void OnClickButton_Close()
    {
        gameObject.SetActive(false); // FoodDisplayUI를 비활성화
        HudManager.Instance.UnsetUseButton(); // Use 버튼 초기화
    }
}
