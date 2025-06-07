// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using UnityEngine.Events;
// using System; // for UnityAction


// public class FoodDisplayUIButton : MonoBehaviour
// {
//     // {{{ To set
//     // Inspector에서 설정할 변수들
//     public PlayerItemEnum itemType;
//     public FoodDisplay foodDisplay; // FoodDisplay 스크립트 참조
//     public Text foodCountText; // 음식 개수를 표시할 Text 컴포넌트


//     // Script에서 초기화
//     public Button button; // 버튼 컴포넌트
//     public FoodDisplayUI foodDisplayUI;
//     // }}}

//     void Awake()
//     {
//         foodDisplayUI = GetComponentInParent<FoodDisplayUI>();
//         if (foodDisplayUI == null)
//         {
//             Debug.LogError("FoodDisplayUI not found in parent!");
//         }
//         button = GetComponent<Button>();
//         if (button == null)
//         {
//             Debug.LogError("Button component not found on FoodDisplayUIButton!");
//         }
//     }

//     // void OnEnable()
//     // {
//     //     foodCountText.text = foodDisplay.NumberOfFoodItems[(int)itemType].ToString();
//     //     foodImage.sprite = HudManager.Instance.GetItemImage(itemType);
//     //     if(foodDisplay.NumberOfFoodItems[(int)itemType] <= 0)
//     //     {
//     //         button.interactable = false; // 음식이 없으면 버튼 비활성화
//     //     }
//     //     else
//     //     {
//     //         foodDisplayUI.UpdateText(itemType); // 음식 설명 업데이트
//     //         HudManager.Instance.SetUseButton(foodImage.sprite, OnClickButton); // Use 버튼 설정
//     //     }
//     // }

//     // public void OnClickButton()
//     // {
//     //     bool isSuccess = AddResultToTray(new PlayerItemData(itemType, new PlayerItemEnum[] { }));
//     //     if (isSuccess)
//     //     {
//     //         foodDisplay.NumberOfFoodItems[(int)itemType]--; // 음식 디스플레이에 추가
//     //     }
//     //     UpdateText(itemType);
//     // }

//     // private bool AddResultToTray(PlayerItemData playerItemData)
//     // {
//     //     bool isSuccess = player.AddItem(playerItemData); // 플레이어에게 아이템 추가
//     //     if (isSuccess)
//     //     {
//     //         Debug.Log("Added " + playerItemData.itemType.ToString() + " to inventory!");
//     //     }
//     //     return isSuccess;
//     // }
// }
