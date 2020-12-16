using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SimpleInventorySystem;
using UnityEngine.UI;
using TMPro;

public class UI_InventoryItem_Controller : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler
{
    public int OriginSlot = 0;
    public Inventory OriginInventory;
    public bool PickedUp { get; private set; } = false;
    public int ItemID = -1;
    public int ItemQuantity = 0;
    public Sprite ItemSprite => Resources.Load<Sprite>(ItemDatabase.FetchItemByID(ItemID).SpritePath);
    private Vector3 StartPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            UpdateOriginSlot();
            PickedUp = true;
            StartPos = transform.position;
            GetComponent<LayoutElement>().ignoreLayout = true;
            GetComponentInParent<CanvasGroup>().blocksRaycasts = false;
            transform.SetAsLastSibling();
        }
        else
        {
            return;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(PickedUp)
        {
            transform.position = eventData.position;
        }
        else
        {
            return;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (PickedUp)
        {
            transform.position = StartPos;
            PickedUp = false;
            GetComponentInParent<LayoutElement>().ignoreLayout = false;
            GetComponentInParent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            return; 
        }
    }

    void Start()
    {
        UpdatePortrait();
        UpdateOriginSlot();
    }

    private void UpdateOriginSlot()
    {
        OriginSlot = transform.parent.GetSiblingIndex();
    }

    public void SetOriginInventory(Inventory inv)
    {
        OriginInventory = inv;
    }

    public void UpdatePortrait()
    {
        transform.GetChild(0).GetComponent<Image>().sprite = ItemSprite;
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{ItemQuantity}";
    }
}
