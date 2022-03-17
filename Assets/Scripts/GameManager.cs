using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    bool dragging = false;
    GameObject draggedObject;

    [Tooltip("Items that players can purchase")]
    [SerializeField]
    GameObject Item;

    [Tooltip("Inventory Slots where items go")]
    [SerializeField]
    public List<GameObject> slotGroup;

    [SerializeField, Tooltip("The starting balance for the player")]
    int StartingBalance;

    [SerializeField, Tooltip("The starting balance for the player")]
    int StartingDiamonds;

    List<GameObject> Weapons = new List<GameObject>();

    List<GameObject> Defenses = new List<GameObject>();

    // List of current inventory
    [SerializeField]
    public List<GameObject> items = new List<GameObject>();

    [SerializeField, Tooltip("Set the starting level for items")]
    int startingLevel = 1;

    [SerializeField, Tooltip("Forge Upgrade state")]
    GameObject ForgeUpgradeState;

    [SerializeField, Tooltip("Normal anvil state")]
    GameObject ForgeNormalState;

    [SerializeField, Tooltip("List of all weapon sprites")]
    List<Sprite> WeaponSprites = new List<Sprite>();

    [SerializeField, Tooltip("List of all defense sprites")]
    List<Sprite> DefenseSprites = new List<Sprite>();

    [SerializeField]
    Image InventoryBackground;

    [SerializeField, Tooltip("Background image used when in weapon inventory")]
    Sprite SwordBackground;

    [SerializeField, Tooltip("Background image used when in defense inventory")]
    Sprite ShieldBackground;

    [SerializeField]
    GameObject WeaponBtn;

    [SerializeField]
    GameObject ActiveWeaponBtn;

    [SerializeField]
    GameObject DefenseBtn;

    [SerializeField]
    GameObject ActiveDefenseBtn;

    public PlayerData playerData;

    [SerializeField, Tooltip("Text to display current coin balance in-game")]
    Text CoinBalanceText;

    [SerializeField, Tooltip("Text to display current diamond balance in-game")]
    Text DiamondBalanceText;

    bool Saving = false;

    public bool CurrentlyWeapons = true;

    string savePath = "playerData.dat";

    Enemies enemies;

    [SerializeField]
    Forge Forge;

    private void Awake()
    {
        savePath = $"{Application.persistentDataPath}/{savePath}";
        Load();
        enemies = new Enemies();
    }

    private void OnApplicationQuit()
    {
        if (Saving)
            return;
        Save();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            if (Saving)
                return;
            Save();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 worldPos = new Vector2(vector3.x, vector3.y);
            if (touch.phase == TouchPhase.Moved)
                draggedObject.GetComponent<Transform>().position = worldPos;
            if (touch.phase == TouchPhase.Ended)
            {
                GameObject closestSlot = slotGroup[0];
                float closestDistance = Vector2.Distance(worldPos, closestSlot.transform.position);
                for (int i = 1; i < slotGroup.Count; i++)
                {
                    float distance = Vector2.Distance(slotGroup[i].transform.position, worldPos);
                    if (closestDistance > distance)
                    {
                        closestDistance = distance;
                        closestSlot = slotGroup[i];
                    }
                }
                int slot = slotGroup.IndexOf(closestSlot);
                GameObject itemInSlot = SlotTaken(slot, draggedObject);
                if (itemInSlot != null)
                {
                    if (itemInSlot.GetComponent<Item>().canMerge(draggedObject))
                    {
                        items.Remove(draggedObject);
                        Destroy(draggedObject);
                        int index = items.IndexOf(itemInSlot);
                        items[index] = itemInSlot.GetComponent<Item>().mergeItems(draggedObject, (CurrentlyWeapons) ? WeaponSprites : DefenseSprites);
                    }
                    else
                    {
                        draggedObject.GetComponent<Item>().setSlot(slot, closestSlot, false);
                        int newSlot = getFirstAvailableSlot(items);
                        itemInSlot.GetComponent<Item>().setSlot(newSlot, slotGroup[newSlot], false);
                    }
                }
                else
                {
                    draggedObject.GetComponent<Item>().setSlot(slot, closestSlot, false);
                }
                dragging = false;
            }
            return;
        }
        else
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
    float time = 0;
    bool Fighting = false;
    private void FixedUpdate()
    {
        if(time >= 5)
        {
            StartCoroutine(EnemyLoop());
            Fighting = true;
            time = 0;
        }else if(!Fighting)
            time += Time.deltaTime;
    }

    public GameObject GenerateItem(bool IsWeapon, List<GameObject> items, bool Active)
    {
        GameObject go = Instantiate(Item);
        Debug.Log($"Creating Weapon={IsWeapon} thats Active={Active}");
        int firstAvailableSlot = getFirstAvailableSlot(items);

        if (IsWeapon)
        {
            go.GetComponent<Item>().setSlot(firstAvailableSlot, slotGroup[firstAvailableSlot], true);
            go.GetComponent<Item>().setLevel(startingLevel, WeaponSprites);
            playerData.setWeapons(SerializeItems(Weapons));
        }
        else
        {
            go.GetComponent<Item>().setSlot(firstAvailableSlot, slotGroup[firstAvailableSlot], true);
            go.GetComponent<Item>().setIsWeapon(false);
            go.GetComponent<Item>().setLevel(startingLevel, DefenseSprites);
            playerData.setDefenses(SerializeItems(Defenses));
        }

        go.SetActive(Active);

        return go;
    }

    public GameObject SlotTaken(int slot, GameObject movedItem)
    {
        foreach (GameObject go in items)
        {
            if (go != movedItem)
            {
                if (go.GetComponent<Item>().getSlot() == slot)
                {
                    return go;
                }
            }
        }
        return null;
    }

    public void Sort()
    {
        int length = items.Count;
        for (int i = 0; i < length - 1; i++)
        {
            for (int j = 0; j < length - i - 1; j++)
            {
                if (items[j].GetComponent<Item>().getLevel() < items[j + 1].GetComponent<Item>().getLevel())
                {
                    GameObject tmp = items[j];
                    items[j] = items[j + 1];
                    items[j + 1] = tmp;
                }
            }
        }
        for (int i = 0; i < items.Count; i++)
        {
            items[i].GetComponent<Item>().setSlot(i, slotGroup[i], false);
        }
    }

    public int getFirstAvailableSlot(List<GameObject> items)
    {
        if (items.Count == 0)
            return 0;
        List<int> takenSlots = new List<int>();
        foreach (GameObject go in items)
        {
            takenSlots.Add(go.GetComponent<Item>().getSlot());
        }
        for (int i = 0; i < slotGroup.Count; i++)
        {
            if (!takenSlots.Contains(i))
                return i;
        }
        return -1;
    }

    public void swapInventories()
    {
        CurrentlyWeapons = !CurrentlyWeapons;
        WeaponBtn.SetActive(!CurrentlyWeapons);
        ActiveWeaponBtn.SetActive(CurrentlyWeapons);
        DefenseBtn.SetActive(CurrentlyWeapons);
        ActiveDefenseBtn.SetActive(!CurrentlyWeapons);
        foreach (GameObject go in items)
            go.SetActive(false);
        if (CurrentlyWeapons)
        {
            Defenses = items;
            items = Weapons;
            InventoryBackground.sprite = SwordBackground;
        }
        else
        {
            Weapons = items;
            items = Defenses;
            InventoryBackground.sprite = ShieldBackground;
        }
        foreach (GameObject go in items)
            go.SetActive(true);
    }


    public void Save()
    {
        Saving = true;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        if (CurrentlyWeapons)
            Weapons = items;
        else
            Defenses = items;
        playerData.setWeapons(SerializeItems(Weapons));
        playerData.setDefenses(SerializeItems(Defenses));
        playerData.setForgeLevel(startingLevel);

        bf.Serialize(file, playerData);
        file.Close();
        Saving = false;
    }

    public List<SerializableItem> SerializeItems(List<GameObject> list)
    {
        List<SerializableItem> items = new List<SerializableItem>();
        foreach (GameObject go in list)
        {
            items.Add(go.GetComponent<Item>().MakeSerializable());
        }
        return items;
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            this.playerData = data;
            List<GameObject> weapons = createGear(CurrentlyWeapons, true, playerData.getWeapons());
            List<GameObject> defenses = createGear(!CurrentlyWeapons, false, playerData.getDefenses());
            enemies = new Enemies(playerData.getCurrentStage(), playerData.getCurrentLevel());
        }
        else
        {
            items.Add(GenerateItem(true, items, true));
            Defenses.Add(GenerateItem(false, Defenses, false));
            this.playerData = new PlayerData(StartingBalance, StartingDiamonds, Forge.BaseAttackItemCost, Forge.BaseDefenseItemCost, SerializeItems(items), SerializeItems(Defenses));
        }
        updateTextDisplays();
    }

    public void updateTextDisplays()
    {
        CoinBalanceText.text = playerData.getBalanceFormatted();
        DiamondBalanceText.text = playerData.getDiamondsFormatted();
    }

    public List<GameObject> createGear(bool Hide, bool Weapons, List<SerializableItem> scripts)
    {
        List<GameObject> list = new List<GameObject>();
        foreach (SerializableItem script in scripts)
        {
            //Debug.Log($"Creating a {(script.IsWeapon() ? "Weapon" : "Shield")} @ Level {script.getLevel()}");
            GameObject go = Instantiate(Item);
            go.GetComponent<Item>().setIsWeapon(Weapons);
            go.GetComponent<Item>().setLevel(script.getLevel(), (script.IsWeapon()) ? WeaponSprites : DefenseSprites);
            go.GetComponent<Item>().setSlot(script.getSlot(), slotGroup[script.getSlot()], true);
            go.SetActive(Hide);
            if (CurrentlyWeapons && Weapons)
                items.Add(go);
            if (script.IsWeapon())
                this.Weapons.Add(go);
            else
                this.Defenses.Add(go);
        }
        return list;
    }

    public void MoveToSlots(List<GameObject> gameObjects)
    {
        foreach (GameObject go in gameObjects)
        {
            int slot = go.GetComponent<Item>().getSlot();
            go.GetComponent<RectTransform>().anchoredPosition = slotGroup[slot].GetComponent<RectTransform>().anchoredPosition;
        }
    }

    public void DeleteSaveData()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/playerData.dat");
        }
    }

    public List<Item> getItemScripts(List<GameObject> list)
    {
        List<Item> items = new List<Item>();
        foreach (GameObject go in list)
        {
            items.Add(go.GetComponent<Item>());
        }
        return items;
    }

    IEnumerator EnemyLoop()
    {
        Enemy enemy = null;
        playerData.setHealth(playerData.getMaxHealth());
        if (enemies.getEnemyCount() == 0 || enemies.getEnemyCount() < 10 && enemies.getEnemyCount() != 5)
        {
            enemy = new Enemy(enemies.getEnemies().Where((e) => e.getName() == "Minion").FirstOrDefault());
            if (enemy != null)
            {
                StartCoroutine(AttackLoop(enemy));
            }
        }
        else if (enemies.getEnemyCount() == 5)
        {
            enemy = enemies.getEnemies().Where((e) => e.getName() == "Mini-Boss").FirstOrDefault();
            if (enemy != null)
            {
                StartCoroutine(AttackLoop(enemy));
            }
        }
        else if (enemies.getEnemyCount() == 10)
        {
            enemy = enemies.getEnemies().Where((e) => e.getName() == "Boss").FirstOrDefault();
            if (enemy != null)
            {
                StartCoroutine(AttackLoop(enemy));
            }
            if (enemies.getEnemyCount() > 10)
            {
                enemies.setEnemyCount(0);
            }
        }
        yield return new WaitUntil(() => enemy.getHealth() == 0 || playerData.getHealth() == 0);
    }

    IEnumerator AttackLoop(Enemy enemy)
    {
        bool FoeDead = false;
        bool SelfDead = false;
        while (!FoeDead && !SelfDead)
        {
            float PlayerDamage = 0;
            float EnemyDamage = 0;
            FoeDead = enemy.Attack(playerData.CalculateDamage(), out PlayerDamage);
            SelfDead = playerData.Attack(enemy.getDamage(), out EnemyDamage);
            Debug.Log($"Player delt {PlayerDamage} damage, Enemy Delt {EnemyDamage} damage\nPlayer Health: {playerData.getHealth()}, Enemy Health: {enemy.getHealth()}");
            yield return new WaitForSeconds(1f);
        }
        if (FoeDead)
        {
            int coins = enemies.calculateGold(enemy);
            playerData.deposit(coins);
            updateTextDisplays();
            enemies.setEnemyCount(enemies.getEnemyCount() + 1);
            Debug.Log($"Killed {enemy.getName()} which dropped {coins} Gold Coins");
        }
        else
        {
            Debug.Log($"You have died to a {enemy.getName()}");
            enemies.setEnemyCount(0);
        }
        Fighting = false;
    }
}