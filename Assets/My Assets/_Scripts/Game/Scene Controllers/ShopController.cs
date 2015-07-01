using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public Button[] buttons;
    public Button buyButton;
    public Button exitStore;
    public Text[] buttonText;
    public Text playerBalance;
    public Item[] shopItems;
    public int selectedItem = -1;
    public Transform spawn1, spawn2, spawn3, spawn4, spawn5, spawn6;
    public int HI_DOLLAR_VALUE;
    public int LO_DOLLAR_VALUE;
    
    private bool firstTick;
    //private string[] itemsToSpawn = { "Sword", "Hammer", "Spear" /*,"Axe","Bow","Dagger"*/ };


    public Text currentStatDisplay;
    public Text selectedItemStats;

    private ItemFactory factory;
    private PlayerController player;
    private Vector3 prevPos;

    //private bool start;


    private void ItemNameUpdate()
    {
        for (int i = 0; i < buttonText.Length; i++)
        {
            buttonText[i].text = "$" + shopItems[i].dollarCost + "\n" + shopItems[i].GetName();
        }
    }

    public void ExitStore()
    {
        AudioManager.Instance.PlaySFX("Select");
        this.player.gameObject.transform.localPosition = prevPos;
        EscapeHandler.instance.ClearButtons();
        DontDestroyOnLoad(this.player);
        Application.LoadLevel("Town_LVP");
    }

    #region purchasing items
    public void SelectItem(int buttonNum)
    {
        AudioManager.Instance.PlaySFX("Select");
        selectedItemStats.text = shopItems[buttonNum].GetStatsString();
        selectedItem = buttonNum;
        buyButton.enabled = true;
        buyButton.image.color = Color.white;

        // if player cant afford item
        if (player.dollarBalance < shopItems[buttonNum].GetDollarCost())
        {
            // turn button red
            buyButton.image.color = Color.red;
        }
        else if (player.dollarBalance >= shopItems[buttonNum].GetDollarCost())
        {
            // turn button gren
            buyButton.image.color = Color.green;
        }
    }

    public void BuyItem()
    {
        AudioManager.Instance.PlaySFX("Select");
        if (this.player.PurchaseItem(shopItems[selectedItem].dollarCost))
        {	// can afford
            if (shopItems[selectedItem].GetItemClass() == "Armor")
            {	// Armor
                player.TransferPurchasedArmor(shopItems[selectedItem]);
            }
            else if (shopItems[selectedItem].GetItemClass() == "Weapon")
            {	// Weapon
                player.TransferPurchasedWeapon(shopItems[selectedItem]);
            }
            else if (shopItems[selectedItem].GetItemClass() == "Magic")
            {	// Magic

            }
            else
            {	// Other
                Debug.Log(shopItems[selectedItem].GetItemClass() + " is not a recognized Item Class!");
            }
            DestroyItem(selectedItem);
        }
        selectedItemStats.text = "";
        selectedItem = -1;
    }

    private void DestroyItem(int n)
    {
        Destroy(buttons[n].gameObject);
        Destroy(shopItems[n].gameObject);
    }
    #endregion purchasing items
    private void PopulateShop()
    {
        // NEW ALGORITHM
        // (Player level / 3 * Random.Range(0,10)) * prev.enemy.stat
        
        GetRandomArmor();
        shopItems[0] = factory.CreateWeapon(spawn1, "Sword");
        shopItems[0].transform.parent = spawn1.transform;
        shopItems[0].SetDmgArm(GetRandomDamage(), GetRandomArmor());

        shopItems[1] = factory.CreateWeapon(spawn2, "Hammer");
        shopItems[1].transform.parent = spawn2.transform;
        shopItems[1].SetDmgArm(GetRandomDamage(), GetRandomArmor());

        shopItems[2] = factory.CreateWeapon(spawn3, "Spear");
        shopItems[2].transform.parent = spawn3.transform;
        shopItems[2].SetDmgArm(GetRandomDamage(), GetRandomArmor());

        shopItems[3] = factory.CreateArmor(spawn4, "LightArmor");
        shopItems[3].transform.parent = spawn4.transform;
        shopItems[3].SetDmgArm(GetRandomDamage(), GetRandomArmor());

        shopItems[4] = factory.CreateArmor(spawn5, "MediumArmor");
        shopItems[4].transform.parent = spawn5.transform;
        shopItems[4].SetDmgArm(GetRandomDamage(), GetRandomArmor());

        shopItems[5] = factory.CreateArmor(spawn6, "HeavyArmor");
        shopItems[5].transform.parent = spawn6.transform;
        shopItems[5].SetDmgArm(GetRandomDamage(), GetRandomArmor());

    }

    private int GetRandomDamage()
    {
        return Mathf.RoundToInt(Random.Range(1.0f, 1.1f) 
            * (DifficultyLevel.GetInstance().GetDifficultyMultiplier() / 3) 
            * EnemyStats.GetInstance().GetNewEnemyATK());
    }

    private int GetRandomArmor()
    {
        return Mathf.RoundToInt(Random.Range(1.0f, 1.1f) 
            * (DifficultyLevel.GetInstance().GetDifficultyMultiplier() / 3) 
            * EnemyStats.GetInstance().GetNewEnemyDEF());
    }

    /// <summary>
    /// Randomizes the cost of item.
    /// </summary>
    private void RandomizeCost()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            int totalStats = shopItems[i].GetDefMod() + shopItems[i].GetAtkMod();
            /*+ shopItems[i].GetHpMod() + shopItems[i].GetManaMod() + shopItems[i].GetSpdMod();  // UNCOMMENT AFTER LVP!*/

            // IF WEAPON STATS ARE NOT INITIATED HERE WE GET THE FIRESALE

           


            this.LO_DOLLAR_VALUE = totalStats + DifficultyLevel.GetInstance().GetDifficultyMultiplier();

            int score = PlayerPrefs.GetInt("score");

            if (score > 0)
            {
                this.HI_DOLLAR_VALUE = (int)((Mathf.FloorToInt(score * 1.5f) / 2) + this.LO_DOLLAR_VALUE);
            }
            else
            {
                this.HI_DOLLAR_VALUE = (DifficultyLevel.GetInstance().GetDifficultyMultiplier() + this.LO_DOLLAR_VALUE);
            }
            shopItems[i].dollarCost = Random.Range(LO_DOLLAR_VALUE, HI_DOLLAR_VALUE);
        }
    }

    /// <summary>
    /// Updates Text
    /// </summary>
    public void UpdateText()
    {
        this.playerBalance.text = "Remaining Balance: $" + this.player.dollarBalance.ToString();
        this.currentStatDisplay.text = "Weapon: \n" + player.playerWeapon.GetName() + "\n" +
            "DMG: " + player.playerWeapon.GetAtkMod() + " | " +
            "AMR: " + player.playerWeapon.GetDefMod() + "\n" +
            "Armor: \n" + player.playerArmor.GetName() + "\n" +
            "DMG: " + player.playerArmor.GetAtkMod() + " | " +
            "AMR: " + player.playerArmor.GetDefMod();
    }

    private void DoOnFirstTick()
    {
        firstTick = true;
        Invoke("RandomizeCost", 0.01f);
    }

    #region monobehaviour

    void Start()
    {
        AudioManager.Instance.PlayNewSong("ForestOverworld");
        EscapeHandler.instance.GetButtons();

        firstTick = false;
        this.player = FindObjectOfType<PlayerController>();
        this.prevPos = this.player.gameObject.transform.localPosition;

        // relocate player
        Vector3 newSpot = new Vector3(-5.7f, -2f);
        this.player.gameObject.transform.localPosition = newSpot;

        this.playerBalance.text = this.player.dollarBalance.ToString();

        factory = FindObjectOfType<ItemFactory>();

        PopulateShop();
    }

    public void Update()
    {
        ItemNameUpdate();
        UpdateText();
        if (!firstTick)
            DoOnFirstTick();
        if (selectedItem < 0)
        {
            buyButton.enabled = false;
        }
    }
}
#endregion monobehaviour
