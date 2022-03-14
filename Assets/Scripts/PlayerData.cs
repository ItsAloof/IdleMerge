using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    List<SerializableItem> Weapons = new List<SerializableItem>();
    List<SerializableItem> Defense = new List<SerializableItem>();
    int Balance { get; set; } = 7000;
    int Diamonds { get; set; } = 0;
    int ForgeLevel = 1;
    int WeaponCost { get; set; } = 300;
    int DefenseCost { get; set; } = 300;


    public PlayerData()
    {

    }

    public PlayerData(int StartingBalance)
    {
        Balance = StartingBalance;
    }

    public PlayerData(int StartingBalance, int startingWeaponCost, int startingDefenseCost)
    {
        Balance = StartingBalance;
        this.WeaponCost = startingWeaponCost;
        this.DefenseCost = startingDefenseCost;
    }


    public void setWeapons(List<SerializableItem> items)
    {
        this.Weapons = items;
    }

    public void setDefenses(List<SerializableItem> items)
    {
        this.Defense = items;
    }

    public List<SerializableItem> getWeapons()
    {
        return Weapons;
    }

    public List<SerializableItem> getDefenses()
    {
        return Defense;
    }

    public void setBalance(int amount)
    {
        Balance = amount;
    }

    public void setDiamonds(int amount)
    {
        Diamonds = amount;
    }

    public int getDiamonds()
    {
        return Diamonds;
    }

    public void setForgeLevel(int level)
    {
        this.ForgeLevel = level;
    }

    public int getForgeLevel()
    {
        return ForgeLevel;
    }

    public void depositDiamonds(int amount)
    {
        this.Diamonds += amount;
    }

    public void withdrawDiamonds(int amount)
    {
        this.Diamonds -= amount;
    }

    public void deposit(int amount)
    {
        this.Balance += amount;
    }

    public void withdraw(int amount)
    {
        Balance -= amount;
    }

    public int getBalance()
    {
        return this.Balance;
    }

    public void setWeaponCost(int cost)
    {
        this.WeaponCost = cost;
    }

    public int getWeaponCost()
    {
        return WeaponCost;
    }

    public void setDefenseCost(int cost)
    {
        this.DefenseCost = cost;
    }

    public int getDefenseCost()
    {
        return DefenseCost;
    }

    public string getBalanceFormatted()
    {
        string coins = string.Format("{0:n}", Balance);
        return coins.Substring(0, coins.Length - 3);
    }
    public string getDiamondsFormatted()
    {
        string diamonds = string.Format("{0:n}", Diamonds);
        return diamonds.Substring(0, diamonds.Length - 3);
    }


}
