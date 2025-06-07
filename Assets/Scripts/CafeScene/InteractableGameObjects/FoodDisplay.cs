
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodDisplay : InteractableItem
{
    public FoodDisplayUI foodDisplayUI; // FoodDisplayUI 스크립트 참조

    [SerializeField]
    private int[] numberOfFoodItems = new int[Enum.GetNames(typeof(PlayerItemEnum)).Length]; // 0: Strawberry Syrup, 1: Chocolate Syrup, 2: Whipped Cream


    public int[] NumberOfFoodItems
    {
        get { return numberOfFoodItems; }
        set
        {
            Debug.Log($"Food items Number changed from {string.Join(",", numberOfFoodItems)} to {string.Join(",", value)}");
            numberOfFoodItems = value;
        }
    }
    protected override void Start()
    {
        base.Start();

        maxTime = 1;    // FoodDisplay는 즉시 사용 가능하므로 maxTime을 1로 설정
        if (progressBar != null) // FoodDisplay는 progressBar가 필요하지 않음
        {
            progressBar.gameObject.SetActive(false);
        }

        for (int i = 0; i < numberOfFoodItems.Length; i++)
        {
            numberOfFoodItems[i] = 0; // 초기화
        }
    }

    public override void OnClickInteractableGameObject()
    {
        Debug.Log("FoodDisplay Clicked");
        foodDisplayUI.gameObject.SetActive(true);

    }
    public int GetFoodItem(PlayerItemEnum itemType)
    {   
        Debug.Log($"Requesting food item of type {itemType}.");
        // 플레이어가 음식 아이템을 선택했을 때 호출되는 메서드
        if (NumberOfFoodItems[(int)itemType] > 0)
        {
            NumberOfFoodItems[(int)itemType]--;
            chosenPlayer.AddItem(new PlayerItemData(itemType, new PlayerItemEnum[] { })); // 플레이어에게 음식 아이템 추가
            Debug.Log($"Food item {itemType} given to player. Remaining: {NumberOfFoodItems[(int)itemType]}");
            return NumberOfFoodItems[(int)itemType]; // 성공적으로 아이템을 줬을 때 남은갯수 반환
        }
        else
        {
            Debug.LogWarning($"No food items of type {itemType} available.");
            return 0;
        }
    }
    public void PutFoodItem(PlayerItemEnum itemType)
    {
        // 플레이어가 음식 아이템을 FoodDisplay에 넣었을 때 호출되는 메서드
        NumberOfFoodItems[(int)itemType]++;
        Debug.Log($"Food item {itemType} added to display. Total: {NumberOfFoodItems[(int)itemType]}");
    }

}
