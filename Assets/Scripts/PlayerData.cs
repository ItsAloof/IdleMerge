using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    List<SerializableItem> Weapons = new List<SerializableItem>();
    List<SerializableItem> Defense = new List<SerializableItem>();
    Dictionary<int, int> DamageTable = new Dictionary<int, int>() { { 1, 1 }, { 2, 2 }, { 3, 4 }, { 4, 8 }, { 5, 16 }, { 6, 6 }, { 7, 7 }, { 8, 8 }, { 9, 9 }, { 10, 10 }, { 11, 11 }, { 12, 12 }, { 13, 13 }, { 14, 14 } };     
    int Balance { get; set; } = 0;
    int Diamonds { get; set; } = 0;
    int ForgeLevel = 1;
    int WeaponCost { get; set; } = 300;
    int DefenseCost { get; set; } = 300;

    int CurrentStage { get; set; } = 1;
    int CurrentLevel { get; set; } = 1;

    float Health = 100;
    float MaxHealth = 100;


    public PlayerData()
    {

    }

    public PlayerData(int StartingBalance)
    {
        Balance = StartingBalance;
    }

    public PlayerData(int StartingBalance, int StartingDiamonds, int startingWeaponCost, int startingDefenseCost)
    {
        Balance = StartingBalance;
        Diamonds = StartingDiamonds;
        this.WeaponCost = startingWeaponCost;
        this.DefenseCost = startingDefenseCost;
    }

    public PlayerData(int StartingBalance, int StartingDiamonds, int startingWeaponCost, int startingDefenseCost, List<SerializableItem> Weapons, List<SerializableItem> Defenses)
    {
        Balance = StartingBalance;
        Diamonds = StartingDiamonds;
        this.WeaponCost = startingWeaponCost;
        this.DefenseCost = startingDefenseCost;
        this.Weapons = Weapons;
        this.Defense = Defenses;
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

    public void setCurrentStage(int stage)
    {
        this.CurrentStage = stage;
    }

    public void setCurrentLevel(int level)
    {
        this.CurrentLevel = level;
    }

    public int getCurrentStage()
    {
        return CurrentStage;
    }

    public int getCurrentLevel()
    {
        return CurrentLevel;
    }

    public float getHealth()
    {
        return Health;
    }

    public void setHealth(float Health)
    {
        this.Health = Health;
    }

    public float getMaxHealth()
    {
        return MaxHealth;
    }

    public void setMaxHealth(float MaxHealth)
    {
        this.MaxHealth = MaxHealth;
    }

    public int CalculateDamage()
    {
        int Damage = 0;
        foreach(SerializableItem item in Weapons)
        {
            Damage += (int)Math.Pow(2, item.getLevel() - 1) * 2;
        }
        return Damage;
    }

    public int CalculateDefense()
    {
        int Defense = 0;
        foreach (SerializableItem item in this.Defense)
        {
            Defense += (int) Math.Pow(2, item.getLevel() - 1) * 2;
        }
        return Defense;
    }

    public bool Attack(float damage, out float DamageDelt)
    {
        DamageDelt = (float)Math.Round(damage / (float)Math.Round((CalculateDefense() * 0.9f), 2), 2);
        Health -= DamageDelt;
        return (Health <= 0);
    }

    public string getBalanceFormatted()
    {
        return Balance.ToString("N0");
    }
    public string getDiamondsFormatted()
    {
        return Diamonds.ToString("N0");
    }


}
