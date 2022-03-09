using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField]
    int slot;

    [SerializeField]
    int level = 1;

    bool weapon = true;

    [SerializeField]
    Text levelDisplay;

    [SerializeField, Tooltip("Set the text format for the level display on items")]
    string levelTextFormat = "Lvl. {0}";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSlot(int slot, GameObject slotGo)
    {
        this.slot = slot;
        GetComponent<RectTransform>().anchoredPosition = slotGo.GetComponent<RectTransform>().anchoredPosition;
    }

    public int getSlot()
    {
        return slot;
    }

    public void setLevel(int level)
    {
        this.level = level;
        this.levelDisplay.text = string.Format(levelTextFormat, level);
    }

    public void levelUp()
    {
        this.level++;
        this.levelDisplay.text = string.Format(levelTextFormat, level);
    }

    public int getLevel()
    {
        return level;
    }

    public void setIsWeapon(bool IsWeapon)
    {
        weapon = IsWeapon;
    }

    public bool IsWeapon()
    {
        return weapon;
    }

    public bool canMerge(GameObject item)
    {
        if(item.GetComponent<Item>() != null)
        {
            if(item.GetComponent<Item>().getLevel() == this.level)
            {
                return true;
            }
        }
        return false;
    }

    public GameObject mergeItems(GameObject item)
    {
        Item itemInfo = item.GetComponent<Item>();
        setLevel(this.level + 1);
        return this.gameObject;
    }


}
