using UnityEngine;
using System.Collections;

public class PlayerInventory : MonoBehaviour
{
    #region variables
    /// <summary>
    /// Apples.
    /// </summary>
    [SerializeField]
    private int _apples;
    public int Apples
    {
        get { return _apples; }
        set { _apples = value; }
    }
    /// <summary>
    /// Bread.
    /// </summary>
    [SerializeField]
    private int _bread;

    public int Bread
    {
        get { return _bread; }
        set { _bread = value; }
    }
    /// <summary>
    /// Cheese.
    /// </summary>
    [SerializeField]
    private int _cheese;

    public int Cheese
    {
        get { return _cheese; }
        set { _cheese = value; }
    }
    /// <summary>
    /// Health Potions
    /// </summary>
    [SerializeField]
    private int _healthPotions;

    public int HealthPotions
    {
        get { return _healthPotions; }
        set { _healthPotions = value; }
    }
    /// <summary>
    /// Energy Potions
    /// </summary>
    [SerializeField]
    private int _energyPotions;

    public int EnergyPotions
    {
        get { return _energyPotions; }
        set { _energyPotions = value; }
    }

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

    /// <summary>
    /// COUNTERS FOR ITEMS
    /// </summary>

    //
    /// <summary>
    /// Camp Kit Counters
    /// </summary>
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
        if (itemType == "Food"){
            return TotalFood < MaxFood;
        }
        else if (itemType == "Potion")
        {
            return TotalPotions < MaxPotions;
        }
        else if (itemType == "campKit")
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

    // Use this for initialization
	void Start () {
        this.Apples = 0;
        this.Bread = 0;
        this.Cheese = 0;
        this.HealthPotions = 0;
        this.EnergyPotions = 0;
        this.TotalFood = 0;
        this.TotalPotions = 0;
        this.TotalCampKits = 0;
        this.MaxFood = 3;
        this.MaxPotions = 3;
	}
	
    

	// Update is called once per frame
	void Update () {
        SetCounts();
	}
}
