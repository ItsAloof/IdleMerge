using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Forge : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    [SerializeField, Tooltip("Set the starting level for items")]
    int StartingLevel;

    [SerializeField]
    int StartingLevelInc = 0;

    [SerializeField, Tooltip("How much the next purchase will cost")]
    Text PurchaseItemCostText;

    [SerializeField, Tooltip("Cost to purchase next item for attack")]
    public int BaseAttackItemCost;

    [SerializeField, Tooltip("Cost to purchase next item for defense")]
    public int BaseDefenseItemCost;

    int CurrentAttackCost { get; set; }

    int CurrentDefenseCost { get; set; }

    [SerializeField, Tooltip("Text field for when there is no room left for items and trying to purchase")]
    GameObject NoRoomText;

    [SerializeField]
    Sprite ActiveForge;

    [SerializeField]
    Sprite InactiveForge;

    [SerializeField, Tooltip("Amount of gold to add per unit of damage to CurrentCost for item type")]
    int CostPerUnitOfDamage;

    [SerializeField, Tooltip("Total Damage divided by this value to get one unit of damage")]
    int UnitOfDamage;

    bool Active = true;

    bool NoRoom = false;

    bool CurrentlyWeapons = true;

    // Start is called before the first frame update
    void Start()
    {
        CalculateCost(true);
        CalculateCost(false);
        PurchaseItemCostText.text = $"{formatCost(CurrentAttackCost)}";
    }

    // Update is called once per frame
    void Update()
    {
        bool ActiveChange = Active;
        if(gameManager.CurrentlyWeapons)
        {
            if(!CurrentlyWeapons)
            {
                CurrentlyWeapons = true;
                UpdateCostText();
            }
        }else if(!gameManager.CurrentlyWeapons)
        {
            if(CurrentlyWeapons)
            {
                CurrentlyWeapons = false;
                UpdateCostText();
            }
        }
        Active = canAfford();
        if(ActiveChange != Active)
        {
            if(Active)
            {
                this.GetComponent<Image>().sprite = ActiveForge;
            }else
            {
                this.GetComponent<Image>().sprite = InactiveForge;
            }
        }
    }

    public int getCurrentAttackCost()
    {
        return CurrentAttackCost;
    }

    public int getCurrentDefenseCost()
    {
        return CurrentDefenseCost;
    }

    public bool canAfford()
    {
        if(CurrentlyWeapons)
        {
            if (CurrentAttackCost > gameManager.playerData.getBalance())
                return false;
            else
                return true;
        }else
        {
            if (CurrentDefenseCost > gameManager.playerData.getBalance())
                return false;
            else
                return true;
        }
    }

    public string formatCost(int amount)
    {
        return amount.ToString("N0");
    }

    public void UpdateCostText()
    {
        if(CurrentlyWeapons)
        {
            CalculateCost(true);
            PurchaseItemCostText.text = $"{formatCost(CurrentAttackCost)}";
        }
        else
        {
            CalculateCost(false);
            PurchaseItemCostText.text = $"{formatCost(CurrentDefenseCost)}";
        }
    }

    public void ForgeItem()
    {
        if (gameManager.items.Count == gameManager.slotGroup.Count)
        {
            if (NoRoom)
                return;
            else
                StartCoroutine(NoRoomMsg());
            return;
        }
        else if (!canAfford())
            return;
        GameObject go = gameManager.GenerateItem(gameManager.CurrentlyWeapons, gameManager.items, true);
        gameManager.items.Add(go);
        StartingLevel += StartingLevelInc;
        gameManager.playerData.withdraw((CurrentlyWeapons) ? CurrentAttackCost : CurrentDefenseCost);
        UpdateCostText();
        gameManager.updateTextDisplays();
    }

    public void CalculateCost(bool WeaponCost)
    {
        if(WeaponCost)
        {
            Debug.Log($"Current Damage: {gameManager.playerData.CalculateDamage()}");
            CurrentAttackCost = BaseAttackItemCost + ((int)(gameManager.playerData.CalculateDamage() / UnitOfDamage) * CostPerUnitOfDamage);
        }else
        {
            Debug.Log($"Current Defense: {gameManager.playerData.CalculateDefense()}");
            CurrentDefenseCost = BaseDefenseItemCost + ((int)((gameManager.playerData.CalculateDefense()) / UnitOfDamage) * CostPerUnitOfDamage);
        }
    }

    IEnumerator NoRoomMsg()
    {
        NoRoom = true;
        NoRoomText.SetActive(true);
        yield return new WaitForSeconds(3f);
        NoRoomText.SetActive(false);
        NoRoom = false;
    }
}
