using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events; // for UnityAction

public class HudManager : MonoBehaviour
{
    public static HudManager Instance;
    
    [SerializeField]
    private Button _UseButton;
    
    [SerializeField]
    private Sprite _OriginUseButtonSprite;

    [SerializeField]
    public Image[] trayItems = new Image[PlayerMover.MAXIMUM_TRAY_SIZE]; // 트레이에 아이템을 담을 배열
    public Sprite[] trayItemImages = new Sprite[PlayerMover.MAXIMUM_ITEM_NUM]; // 트레이에 아이템 이미지를 담을 배열
    private void Awake()
    {
        Instance = this;
    }

    public void SetUseButton(Sprite sprite, UnityAction action)
    {
        Debug.Log("SetUseButton called");
        _UseButton.image.sprite = sprite;
        _UseButton.onClick.AddListener(action);
        _UseButton.interactable = true;
    }
    
    public void UnsetUseButton()
    {
        Debug.Log("UnsetUseButton called");
        _UseButton.image.sprite = _OriginUseButtonSprite;
        _UseButton.onClick.RemoveAllListeners();
        _UseButton.interactable = false;
    }

    public void SetItemToTray(PlayerItemData item, int index)
    {
        // 아이템을 트레이에 추가하는 로직
        Debug.Log("Added " + item + " to tray at index " + index);
        if(item == null)
        {
            Debug.LogWarning("Item is null at index " + index);
            return;
        }

        if (item.itemType == PlayerItemEnum.NONE)
        {
            trayItems[index].sprite = null;
            trayItems[index].gameObject.SetActive(false); // 트레이에 아이템 비활성화
            return;
        }
        else if (item.itemType == PlayerItemEnum.WATER)
        {
            trayItems[index].sprite = trayItemImages[(int)item.itemType]; // 트레이에 아이템 활성화
            trayItems[index].SetNativeSize(); // 트레이에 아이템 크기 조정
            trayItems[index].gameObject.GetComponent<RectTransform>().localScale = new Vector3(.3f, .3f, 1f);
            trayItems[index].gameObject.SetActive(true); // 트레이에 아이템 활성화
            return;
        }
        else if (item.itemType == PlayerItemEnum.ESPRESSO)
        { // same with WATER
            trayItems[index].sprite = trayItemImages[(int)item.itemType]; // 트레이에 아이템 활성화
            trayItems[index].SetNativeSize(); // 트레이에 아이템 크기 조정
            trayItems[index].gameObject.GetComponent<RectTransform>().localScale = new Vector3(.3f, .3f, 1f);
            trayItems[index].gameObject.SetActive(true); // 트레이에 아이템 활성화
        }
        else if (item.itemType == PlayerItemEnum.AMERICANO)
        { // same with WATER
            trayItems[index].sprite = trayItemImages[(int)item.itemType]; // 트레이에 아이템 활성화
            trayItems[index].SetNativeSize(); // 트레이에 아이템 크기 조정
            trayItems[index].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            trayItems[index].gameObject.SetActive(true); // 트레이에 아이템 활성화
        }
        else
        {
            Debug.Log("Not Implemented item: " + item);
            return;
        }
    }
}
