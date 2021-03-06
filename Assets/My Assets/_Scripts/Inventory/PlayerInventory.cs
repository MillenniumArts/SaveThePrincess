using UnityEngine;
using System.Collections;

public class PlayerInventory : MonoBehaviour
{
    #region variables
    #region Player
    private PlayerController _player;

    public PlayerController Player
    {
        get { return _player; }
        set { _player = value; }
    }
    
    #endregion Player

    #region inventoryLevel
    /// <summary>
    /// Inventory Level, used for resizing
    /// </summary>
    [SerializeField]
    private int _inventoryLevel;

    public int InventoryLevel
    {
        get { return _inventoryLevel; }
        set { _inventoryLevel = value; }
    }
    #endregion inventoryLevel

    #region Apple
    /// <summary>
    /// Apples.
    /// </summary>
    [SerializeField]
    private int _appleCount;
    public int Apples
    {
        get { return _appleCount; }
        set { _appleCount = value; }
    }
    /// <summary>
    /// Percent of Player's health healed by Apple
    /// </summary>
    private int _appleTurns;

    public int AppleTurns
    {
        get { return _appleTurns; }
        set { _appleTurns = value; }
    }
    #endregion Apple

    #region Bread
    /// <summary>
    /// Bread.
    /// </summary>
    [SerializeField]
    private int _breadCount;

    public int Bread
    {
        get { return _breadCount; }
        set { _breadCount = value; }
    }
    /// <summary>
    /// Percent of health healed by bread
    /// </summary>
    private int _breadTurns;

    public int BreadTurns
    {
        get { return _breadTurns; }
        set { _breadTurns = value; }
    }
    #endregion Bread

    #region Cheese
    /// <summary>
    /// Cheese.
    /// </summary>
    [SerializeField]
    private int _cheeseCount;

    public int Cheese
    {
        get { return _cheeseCount; }
        set { _cheeseCount = value; }
    }
    /// <summary>
    /// Percent of health restored from Cheese
    /// </summary>
    private int _cheeseTurns;

    public int CheeseTurns
    {
        get { return _cheeseTurns; }
        set { _cheeseTurns = value; }
    }
    #endregion Cheese

    #region HealthPotions
    /// <summary>
    /// Health Potions
    /// </summary>
    [SerializeField]
    private int _healthPotionCount;

    public int HealthPotions
    {
        get { return _healthPotionCount; }
        set { _healthPotionCount = value; }
    }

    private int _healthPotionAmount;

    public int HealthPotionPercent
    {
        get { return _healthPotionAmount; }
        set { _healthPotionAmount = value; }
    }
    
    #endregion HealthPotions

    #region EnergyPotions
    /// <summary>
    /// Energy Potions
    /// </summary>
    [SerializeField]
    private int _energyPotionCount;

    public int EnergyPotions
    {
        get { return _energyPotionCount; }
        set { _energyPotionCount = value; }
    }
    private int _energyPotionAmount;

    public int EnergyPotionPercent
    {
        get { return _energyPotionAmount; }
        set { _energyPotionAmount = value; }
    }
    
    #endregion EnergyPotions
    
    #region CampKits

    /// <summary>
    /// Camp Kits.
    /// </summary>
    [SerializeField]
    private int _campKits;

    public int CampKits
    {
        get { return _campKits; }
        set { _campKits = value; }
    }
    #endregion CampKits

    #region item counters
    /// <summary>
    /// COUNTERS FOR ITEMS
    /// </summary>

    //
    /// <summary>
    /// Camp Kit Counters
    /// </summary>
    [SerializeField]

    private int _totalCampKits;
    public int TotalCampKits
    {
        get { return _totalCampKits; }
        set { _totalCampKits = value; }
    }

    private int _maxCampKits;

    public int MaxCampKits
    {
        get { return _maxCampKits; }
        set { _maxCampKits = value; }
    }

    /// <summary>
    /// Food Counters
    /// </summary>
    [SerializeField]
    private int _totalFood;

    public int TotalFood
    {
        get { return _totalFood; }
        set { _totalFood = value; }
    }
    private int _maxFood;

    public int MaxFood
    {
        get { return _maxFood; }
        set { _maxFood = value; }
    }

    /// <summary>
    /// Potions Counters
    /// </summary>
    [SerializeField]
    private int _totalPotions;

    public int TotalPotions
    {
        get { return _totalPotions; }
        set { _totalPotions = value; }
    }
    private int _maxPotions;

    public int MaxPotions
    {
        get { return _maxPotions; }
        set { _maxPotions = value; }
    }
    #endregion item counters

    #region percentToHealPerTurn
    private int _percentToHealPerTurn;

    public int PercentToRegenPerTurn
    {
        get { return _percentToHealPerTurn; }
        set { _percentToHealPerTurn = value; }
    }

    #endregion percentToHealPerTurn

    #endregion variables

    #region buying items

    /// <summary>
    /// Returns whether or not the player has room in their inventory 
    /// for a type of item at specified quantity.
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public bool HasRoomInInventoryFor(string itemType, int quantity)
    {
        itemType = itemType.ToLower();
        if (itemType == "food"){
            return quantity < ( MaxFood - TotalFood );
        }
        else if (itemType == "potion")
        {
            return quantity < ( MaxPotions - TotalPotions );
        }
        else if (itemType == "campkit")
        {
            return TotalCampKits < MaxCampKits;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Adds food to inventory. MAKE SURE YOU CALL HasRoomInInventoryFor() before adding.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="quantity"></param>
    public void AddFood(string type, int quantity)
    {
        if (type == "Apple")
        {
            Apples += quantity;
        }
        else if (type == "Bread")
        {
            Bread += quantity;
        }
        else if (type == "Cheese")
        {
            Cheese += quantity;
        }
        else
        {
            Debug.Log("Unknow type of food! " + type);
        }
    }

    public void AddPotion(string type, int quantity)
    {
        if (type == "HealthPotion")
        {
            HealthPotions += quantity;
        }
        else if (type == "EnergyPotion")
        {
            EnergyPotions += quantity;
        }
        else
        {
            Debug.Log("Unknown type of potion! " + type);
        }
    }

    public void AddCampKit(string type, int quantity)
    {
        CampKits += quantity;
    }
    /// <summary>
    /// updates total food and potion counters
    /// </summary>
    void SetCounts()
    {
        this.TotalFood = Apples + Bread + Cheese;
        this.TotalPotions = HealthPotions + EnergyPotions;
    }

    #endregion buying items

    #region using items
    /// <summary>
    /// behaviour called on food use
    /// </summary>
    /// <param name="food">type of food</param>
    /// <returns>if food was eaten successfully</returns>
    public bool EatFood(string food)
    {
        if (food == "Apple")
        {
            if (Apples > 0)
            {
                //Debug.Log(this.Player.name + " Has restored "+ AppleTurns + " health and energy!");
                this.Player.SetNumTurnsToHeal(AppleTurns);
                this.Player.TriggerAnimation("potion");
                this.Apples--;
                return true;
            }

        }else if (food == "Bread"){
            if (Bread > 0)
            {
                //Debug.Log(this.Player.name + " Has restored " + BreadTurns + " health and energy!");
                this.Player.SetNumTurnsToHeal(BreadTurns);
                this.Player.TriggerAnimation("potion");
                this.Bread--;
                return true;
            }
        }else if (food == "Cheese"){
            if (Cheese > 0) {
                //Debug.Log(this.Player.name + " Has restored " + CheeseTurns + " health and energy!");
                this.Player.SetNumTurnsToHeal(CheeseTurns);
                this.Player.TriggerAnimation("potion");
                this.Cheese--;
                return true;
            }
        }
        else
        {
            Debug.Log("Unknown food type "+ food);
            return false;
        }
        return false;
    }

    /// <summary>
    /// behaviour called on potion use
    /// </summary>
    /// <param name="potion">potion type</param>
    /// <returns></returns>
    public bool UsePotion(string potion)
    {
        if (this.HealthPotions > 0)
        {
            if (potion == "Health")
            {
                if (this.Player.remainingHealth < this.Player.totalHealth)
                {
                    this.Player.GiveHealthPercent(HealthPotionPercent);
                    this.HealthPotions--;
                    this.Player.TriggerAnimation("healpotion");
                    return true;
                }
            }
        }
        else if (potion == "Energy")
        {
            if (this.EnergyPotions > 0)
            {
                if (this.Player.remainingEnergy < this.Player.totalEnergy)
                {
                    this.Player.GiveEnergyPercent(EnergyPotionPercent);
                    this.EnergyPotions--;
                    this.Player.TriggerAnimation("energypotion");
                    return true;
                }
            }
        }
        else
        {
            Debug.Log("Unknown potion type " + potion);
            return false;
        }
        return false;
    }

    public void UseCampKit()
    {

    }

    #endregion using items
    #region monobehaviour
    // Use this for initialization
	void Awake () {
        // get player
        this.Player = FindObjectOfType<PlayerController>();
        // inventory level 
        this.InventoryLevel = 1;
        // item counts
        this.Apples = 0;
        this.Bread = 0;
        this.Cheese = 0;
        this.HealthPotions = 0;
        this.EnergyPotions = 0;
        //max counts
        this.MaxFood = 3;
        this.MaxPotions = 3;
        this.MaxCampKits = 1;
        // totals
        this.TotalFood = 0;
        this.TotalPotions = 0;
        this.TotalCampKits = 0;
        // max counts
        this.MaxFood = 3;
        this.MaxPotions = 3;
        // number of turns to heal for a percent per food item
        this.AppleTurns = 2;
        this.BreadTurns = 3;
        this.CheeseTurns = 4;
        // percent to heal per turn
        this.PercentToRegenPerTurn = 10;
        // amount per potion
        this.HealthPotionPercent = 50;
        this.EnergyPotionPercent = 30;
	}
	
	// Update is called once per frame
	void Update () {
        SetCounts();
	}
}
#endregion monobehaviour
