using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        Hostiles.Add(new Enemy("Minion", 25, 5, 2, 30));
        Hostiles.Add(new Enemy("Mini-Boss", 75, 20, 10, 60));
        Hostiles.Add(new Enemy("Boss", 150, 60, 25, 120));
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
