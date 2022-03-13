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


    public PlayerData()
    {

    }

    public PlayerData(int StartingBalance)
    {
        Balance = StartingBalance;
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

    public string getBalanceFormatted()
    {
        return Balance.ToString("C");
    }


}
