using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool dragging = false;
    GameObject draggedObject;

    [Tooltip("Items that players can purchase")]
    [SerializeField]
    GameObject Item;

    [Tooltip("Inventory Slots where items go")]
    [SerializeField]
    List<GameObject> slotGroup;

    List<GameObject> items = new List<GameObject>();
    [SerializeField, Tooltip("Set the starting level for items")]
    int startingLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(dragging && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 worldPos = new Vector2(vector3.x, vector3.y);
            if (touch.phase == TouchPhase.Moved)
                draggedObject.GetComponent<Transform>().position = worldPos;
            if(touch.phase == TouchPhase.Ended)
            {
                GameObject closestSlot = slotGroup[0];
                float closestDistance = Vector2.Distance(worldPos, closestSlot.transform.position);
                for(int i = 1; i < slotGroup.Count; i++)
                {
                    float distance = Vector2.Distance(slotGroup[i].transform.position, worldPos);
                    if(closestDistance > distance)
                    {
                        closestDistance = distance;
                        closestSlot = slotGroup[i];
                    }
                }
                int slot = slotGroup.IndexOf(closestSlot);
                GameObject itemInSlot = SlotTaken(slot, draggedObject);
                if (itemInSlot != null)
                {
                    if (itemInSlot.GetComponent<Item>().canMerge(draggedObject))
                    {
                        items.Remove(draggedObject);
                        Destroy(draggedObject);
                        int index = items.IndexOf(itemInSlot);
                        items[index] = itemInSlot.GetComponent<Item>().mergeItems(draggedObject);
                    }
                    else
                    {
                        draggedObject.GetComponent<Item>().setSlot(slot, closestSlot);
                        int newSlot = getFirstAvailableSlot();
                        itemInSlot.GetComponent<Item>().setSlot(newSlot, slotGroup[newSlot]);
                    }
                }else
                {
                    draggedObject.GetComponent<Item>().setSlot(slot, closestSlot);
                }
                dragging = false;
            }
            return;
        }else
        {
            dragging = false;
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 twoDWorldPos = new Vector2(worldPos.x, worldPos.y);
            RaycastHit2D hit = Physics2D.Raycast(twoDWorldPos, Camera.main.transform.forward);
            if (hit.collider != null && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                dragging = true;
                draggedObject = hit.collider.gameObject;
            }
        }
    }

    public void PurchaseItem()
    {
        if (items.Count == slotGroup.Count)

            return;
        GameObject go = Instantiate(Item);
        int firstAvailableSlot = getFirstAvailableSlot();
        go.GetComponent<Item>().setSlot(firstAvailableSlot, slotGroup[firstAvailableSlot]);
        go.GetComponent<Item>().setLevel(startingLevel);
        items.Add(go);
    }

    public GameObject SlotTaken(int slot, GameObject movedItem)
    {
        foreach(GameObject go in items)
        {
            if(go != movedItem)
            {
                if(go.GetComponent<Item>().getSlot() == slot)
                {
                    return go;
                }
            }
        }
        return null;
    }

    public void Sort()
    {
        int length = items.Count;
        for(int i = 0; i < length - 1; i++)
        {
            for(int j = 0; j < length - i - 1; j++)
            {
                if(items[j].GetComponent<Item>().getLevel() < items[j + 1].GetComponent<Item>().getLevel())
                {
                    GameObject tmp = items[j];
                    items[j] = items[j + 1];
                    items[j + 1] = tmp;
                }
            }
        }
        for(int i = 0; i < items.Count; i++)
        {
            items[i].GetComponent<Item>().setSlot(i, slotGroup[i]);
        }
    }

    public int getFirstAvailableSlot()
    {
        if (items.Count == 0)
            return 0;
        List<int> takenSlots = new List<int>();
        foreach (GameObject go in items)
        {
            takenSlots.Add(go.GetComponent<Item>().getSlot());
        }
        for(int i = 0; i < slotGroup.Count; i++)
        {
            if (!takenSlots.Contains(i))
                return i;
        }
        return -1;
    }
}
