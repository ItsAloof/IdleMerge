using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Enemies
{
    int Stage { get; set; } = 1;
    int Level { get; set; } = 1;
    int EnemyCount { get; set; } = 0;

    List<Enemy> Hostiles = new List<Enemy>();  

    float time = 5f;

    public Enemies()
    {
        PopulateEnemies();
    }

    public Enemies(int Stage, int Level)
    {
        this.Stage = Stage;
        this.Level = Level;
        PopulateEnemies();
    }

    public void setEnemyCount(int count)
    {
        this.EnemyCount = count;
    }

    public int getEnemyCount()
    {
        return this.EnemyCount;
    }

    public int getLevel()
    {
        return Level;
    }

    public int getStage()
    {
        return Stage;
    }

    public void setLevel(int level)
    {
        this.Level = level;
        LevelUpEnemies();
    }

    public void LevelUpEnemies()
    {
        foreach (Enemy enemy in Hostiles)
        {
            enemy.setHealth((float) Math.Round(enemy.getHealth() * ((Level*1.15f) * (Stage*1.5)), 2));
            enemy.setDamage((int) Math.Round(enemy.getDamage() * (Level * 1.15) * (Stage * 1.5), 2));
            enemy.setDefense((int) Math.Round(enemy.getDefense() * (Level * 1.15) * (Stage * 1.5), 2));
        }
    }

    public void setStage(int stage)
    {
        this.Stage = stage;
    }

    public void setTimeDelay(float time)
    {
        this.time = time;
    }

    public float getTimeDelay()
    {
        return time;
    }

    public List<Enemy> getEnemies()
    {
        return Hostiles;
    }

    public void PopulateEnemies()
    {
        // Passive Income Calculations
        // Totals for Baase Level enemies
        // Health, Damage, Defense, Coin Drop
        // 250     120     60       210
        // Level 4 Weapon Damage: 32
        // Attack Damage Calculation: PlayerDamage / (EnemyDefense * 0.75f)
        // Damage: 0.71
        // Time To Kill @ 1 Hit/sec. = 352 seconds or 5.87 minutes
        // Gold Per Hour = (60 / 5.87 Minutes) * 210 = (10.22) * 210 = 2146 Gold/Hr
        Hostiles.Add(new Enemy("Minion",    25,  10, 5,  30));
        Hostiles.Add(new Enemy("Mini-Boss", 75,  30, 15, 60));
        Hostiles.Add(new Enemy("Boss",      150, 80, 40, 120));
    }

    public float CalculateMultiplier()
    {
        float multiplier = 1;
        multiplier += (0.01f * Level) + (2 * Stage);
        return multiplier;
    }

    public int calculateGold(Enemy enemy)
    {
        float drop = enemy.getCoinDrop() * CalculateMultiplier();
        return (int)drop;
    }
}
