using System;
using System.Collections;
using System.Collections.Generic;

public class Enemy
{
    string Name { get; set; }
    float Health { get; set; }
    int Damage { get; set; }
    int Defense { get; set; }
    int Coins { get; set; }

    public Enemy(string Name, float Health, int Damage, int Defense, int CoinDrop)
    {
        this.Name = Name;
        this.Health = Health;
        this.Damage = Damage;
        this.Defense = Defense;
        this.Coins = CoinDrop;
    }

    public Enemy(Enemy enemy)
    {
        Name = enemy.Name;
        Health = enemy.Health;
        Damage = enemy.Damage;
        Defense = enemy.Defense;
        Coins = enemy.Coins;
    }

    public float getHealth()
    {
        return Health;
    }

    public void setHealth(float Health)
    {
        this.Health = Health;
    }

    public string getName()
    {
        return Name;
    }

    public void setName(string Name)
    {
        this.Name = Name;
    }

    public int getDamage()
    {
        return Damage;
    }

    public void setDamage(int Damage)
    {
        this.Damage = Damage;
    }

    public int getDefense()
    {
        return Defense;
    }

    public void setDefense(int Defense)
    {
        this.Defense = Defense;
    }

    public int getCoinDrop()
    {
        return Coins;
    }

    public void setCoinDrop(int CoinDrop)
    {
        this.Coins = CoinDrop;
    }

    public bool Attack(float damage, out float DamageDelt)
    {
        DamageDelt = (float)Math.Round(damage / (float)Math.Round((Defense * 0.9f), 2), 2);
        Health -= DamageDelt;
        return (Health <= 0);
    }
}
