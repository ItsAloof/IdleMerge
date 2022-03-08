using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    bool dragging = false;
    GameObject draggedObject;
    [SerializeField]
    List<GameObject> slotGroup;

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
                if(closestDistance < 1)
                    draggedObject.GetComponent<RectTransform>().anchoredPosition = closestSlot.GetComponent<RectTransform>().anchoredPosition;
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


    //void Update()
    //{
    //    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
    //    if ((hit != null) && hit.collider != null)
    //    {
    //        Debug.Log("I'm hitting " + hit.collider.name);
    //    }
    //}

    //void Update()
    //{
    //    // Code for OnMouseDown in the iPhone. Unquote to test.
    //    RaycastHit hit = new RaycastHit();
    //    for (int i = 0; i < Input.touchCount; ++i)
    //    {
    //        if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
    //        {
    //            // Construct a ray from the current touch coordinates
    //            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
    //            if (Physics.Raycast(ray, out hit))
    //            {
    //                hit.transform.gameObject.SendMessage("OnMouseDown");
    //            }
    //        }
    //    }
    //}
}
