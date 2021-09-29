using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IPointerClickHandler
{
    private bool isFollowing = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFollowing)
        {
            isFollowing = false;
            InsertInContainer();
        }
        else
        {
            isFollowing = true;
            ExtractFromContainer();
        }
    }
    public void Update()
    {
        FollowMouse();
    }
    private void FollowMouse()
    {
        if (!isFollowing) return;

        transform.position = Input.mousePosition;
    }

    private void ExtractFromContainer()
    {
        transform.SetParent(GameController.Instance.Inventory.InventorySlots);
    }

    private void InsertInContainer()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.tag == "ItemContainer")
                {
                    foreach (InventoryManager.ItemContent item in GameController.Instance.Inventory.prefabContents)
                    {
                        if (item.reference == gameObject)
                        {
                            item.placement = result.gameObject.transform.GetSiblingIndex();
                        }
                    }
                    transform.SetParent(result.gameObject.transform);
                    transform.localPosition = new Vector3(50f, 0f, 0f);
                }
            }
        }
    }

}
