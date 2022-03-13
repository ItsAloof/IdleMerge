using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class SerializableItem
{
    int Slot { get; set; }

    int Level { get; set; }

    bool Weapon { get; set; }

    public SerializableItem(int slot, int level, bool isWeapon)
    {
        Slot = slot;
        Level = level;
        Weapon = isWeapon;
    }

    public void setSlot(int slot)
    {
        this.Slot = slot;
    }

    public int getSlot()
    {
        return Slot;
    }

    public void setLevel(int level)
    {
        this.Level = level;
    }

    public int getLevel()
    {
        return Level;
    }

    public void setIsWeapon(bool IsWeapon)
    {
        Weapon = IsWeapon;
    }

    public bool IsWeapon()
    {
        return Weapon;
    }


}
