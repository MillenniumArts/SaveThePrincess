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
    public Transform spawn1, spawn2, spawn3;//, spawn4, spawn5, spawn6;
    public GameObject[] receipt;
    public int HI_DOLLAR_VALUE;
    public int LO_DOLLAR_VALUE;
    public Image[] priceTags;
    
    private bool firstTick;
    //private string[] itemsToSpawn = { "Sword", "Hammer", "Spear" /*,"Axe","Bow","Dagger"*/ };


    public Text currentStatDisplay, curStatTitle;
    public Text selectedItemStats, selItemTitle;
    public Text curBalanceText, remBalanceText, costText;

    private ItemFactory factory;
    private PlayerController player;
    private Vector3 prevPos;

    public GameObject armourPrefab;

    //private bool start;


    private void ItemNameUpdate()
    {
        for (int i = 0; i < buttonText.Length; i++)
        {
            if (buttonText[i] != null)
                buttonText[i].text = shopItems[i].dollarCost.ToString();// +"\n" + shopItems[i].GetName();
        }
    }

    public void ExitStore()
    {
        this.player.gameObject.transform.localScale = new Vector3(-1.25f, 1.25f, 1f);
        AudioManager.Instance.PlaySFX("SelectSmall");
      //  this.player.gameObject.transform.localPosition = prevPos;
        //EscapeHandler.instance.ClearButtons();
        //DontDestroyOnLoad(this.player);
        LevelLoadHandler.Instance.LoadLevel("Town_LVP", false);
    }

    public void RefreshShop()
    {
        AudioManager.Instance.PlaySFX("Inventory");
        for (int i = 0; i < shopItems.Length; i++)
        {
            if(shopItems[i] != null)
                Destroy(shopItems[i].gameObject);
            buttons[i + 3].interactable = true;
        }
        selectedItemStats.text = "";
        selectedItem = -1;
        PopulateShop();
        Invoke("RandomizeCost", 0.01f);
        for (int i = 0; i < priceTags.Length; i++)
        {
            priceTags[i].color = Color.white;
        }
    }

    #region purchasing items
    public void SelectItem(int buttonNum)
    {
        AudioManager.Instance.PlaySFX("SelectSmall");
        selectedItemStats.text = shopItems[buttonNum].GetStatsString();
        selectedItem = buttonNum;
        buyButton.enabled = true;
        //buyButton.image.color = Color.white;
        //buyButton.gameObject.GetComponent<ButtonPulse>().PulseOff();

        // if player cant afford item
        if (player.dollarBalance < shopItems[buttonNum].GetDollarCost())
        {
            // turn button red
            ///buyButton.image.color = Color.red;  
            //buyButton.gameObject.GetComponent<ButtonPulse>().colourName = "red";
            //buyButton.gameObject.GetComponent<ButtonPulse>().PulseOn();
            buyButton.gameObject.SetActive(false);
        }
        else if (player.dollarBalance >= shopItems[buttonNum].GetDollarCost())
        {
            // turn button gren
            ///buyButton.image.color = Color.green;
            //buyButton.gameObject.GetComponent<ButtonPulse>().colourName = "black";
            //buyButton.gameObject.GetComponent<ButtonPulse>().PulseOn();
            buyButton.gameObject.SetActive(true);
        }

        for (int i = 0; i < priceTags.Length; i++)
        {
            if (i == buttonNum)
            {
                priceTags[i].color = Color.green;
            }
            else
            {
                priceTags[i].color = Color.white;
            }
        }
    }

    public void BuyItem()
    {
        AudioManager.Instance.PlaySFX("SelectSmall");
        AudioManager.Instance.PlaySFX("AcceptPurchase");
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
            buttons[selectedItem + 3].interactable = false;
            //receipt[selectedItem].SetActive(true);
        }
        //buyButton.gameObject.GetComponent<ButtonPulse>().PulseOff();
        buyButton.gameObject.SetActive(false);
        for (int i = 0; i < priceTags.Length; i++)
        {
            priceTags[i].color = Color.white;
        }
        selectedItemStats.text = "";
        selectedItem = -1;
    }

    private void DestroyItem(int n)
    {
        if (buttons[n] != null)
        {
            Destroy(buttons[n].gameObject);
        }
        Destroy(shopItems[n].gameObject);
    }
    #endregion purchasing items
    private void PopulateShop()
    {
        // NEW ALGORITHM
        // (Player level / 3 * Random.Range(0,10)) * prev.enemy.stat

        int rand3 = Random.Range(0, 16);
        //GetRandomArmor();
        if (rand3 < 5)
            shopItems[0] = factory.CreateWeapon(spawn1, "Sword");
        else if (rand3 < 9)
            shopItems[0] = factory.CreateWeapon(spawn1, "Hook");
        else if (rand3 < 13)
            shopItems[0] = factory.CreateWeapon(spawn1, "Club");
        else
            shopItems[0] = factory.CreateWeapon(spawn1, "Dagger");
        shopItems[0].transform.parent = spawn1.transform;
        shopItems[0].SetDmgArm(GetRandomDamage(), GetRandomArmor());

        int rand1 = Random.Range(0, 12);
        if(rand1 < 5)
            shopItems[1] = factory.CreateWeapon(spawn2, "Hammer");
        else if(rand1 < 9)
            shopItems[1] = factory.CreateWeapon(spawn2, "Axe");
        else
            shopItems[1] = factory.CreateWeapon(spawn2, "Spear");
        shopItems[1].transform.parent = spawn2.transform;
        shopItems[1].SetDmgArm(GetRandomDamage(), GetRandomArmor());

        int rand2 = Random.Range(0, 10);
        if (rand2 < 4)
            shopItems[2] = factory.CreateArmor(spawn3, "LightArmor");
        else if(rand2 < 7)
            shopItems[2] = factory.CreateArmor(spawn3, "MediumArmor");
        else
            shopItems[2] = factory.CreateArmor(spawn3, "HeavyArmor");
        shopItems[2].transform.parent = spawn3.transform;
        Vector3 tempScale = shopItems[2].transform.localScale;
        tempScale *= 1.2f;
        shopItems[2].transform.localScale = tempScale;
        shopItems[2].SetDmgArm(GetRandomDamage(), GetRandomArmor());
        GameObject newArmourPrefab = Instantiate(armourPrefab, shopItems[2].transform.position, Quaternion.identity) as GameObject;
        newArmourPrefab.transform.parent = shopItems[2].gameObject.transform;

        /*shopItems[3] = factory.CreateArmor(spawn4, "LightArmor");
        shopItems[3].transform.parent = spawn4.transform;
        shopItems[3].SetDmgArm(GetRandomDamage(), GetRandomArmor());

        shopItems[4] = factory.CreateArmor(spawn5, "MediumArmor");
        shopItems[4].transform.parent = spawn5.transform;
        shopItems[4].SetDmgArm(GetRandomDamage(), GetRandomArmor());

        shopItems[5] = factory.CreateArmor(spawn6, "HeavyArmor");
        shopItems[5].transform.parent = spawn6.transform;
        shopItems[5].SetDmgArm(GetRandomDamage(), GetRandomArmor());*/

    }

    private int GetRandomDamage()
    {
        return Mathf.RoundToInt(Random.Range(1.0f, 1.1f) 
            * (DifficultyLevel.GetInstance().GetDifficultyMultiplier() / 3) 
            * EnemyStats.GetInstance().GetCurrentEnemyATK());
    }

    private int GetRandomArmor()
    {
        return Mathf.RoundToInt(Random.Range(1.0f, 1.1f) 
            * (DifficultyLevel.GetInstance().GetDifficultyMultiplier() / 3) 
            * EnemyStats.GetInstance().GetCurrentEnemyDEF());
    }

    /// <summary>
    /// Randomizes the cost of item.
    /// </summary>
    private void RandomizeCost()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            int totalStats = shopItems[i].GetDefMod() + shopItems[i].GetAtkMod();

            this.LO_DOLLAR_VALUE = totalStats - (int)(totalStats * 0.2) + DifficultyLevel.GetInstance().GetDifficultyMultiplier();

            int score = PlayerPrefs.GetInt("score");

            if (score > 0)
            {
                this.HI_DOLLAR_VALUE = (int)((Mathf.FloorToInt(score * 1.5f) / 2) + this.LO_DOLLAR_VALUE);
            }
            else
            {
                this.HI_DOLLAR_VALUE = (DifficultyLevel.GetInstance().GetDifficultyMultiplier() + this.LO_DOLLAR_VALUE);
            }
            shopItems[i].dollarCost = (int)(Random.Range(LO_DOLLAR_VALUE, HI_DOLLAR_VALUE) / 2);
        }
    }

    /// <summary>
    /// Updates Text
    /// </summary>
    public void UpdateText()
    {
        this.playerBalance.text = this.player.dollarBalance.ToString();

        if (selectedItem == -1)
        {
            this.selItemTitle.text = "";
            this.curStatTitle.text = "";
            this.currentStatDisplay.text = "";
        }
        else if (selectedItem == 0 || selectedItem == 1)
        {
            this.curStatTitle.text = this.player.playerName + "'s Weapon stats";
            this.selItemTitle.text = "Selected Weapon Stats";
            //weapon
            this.currentStatDisplay.text = player.playerWeapon.GetName() + "\n" +
                "DMG: " + player.playerWeapon.GetAtkMod();
        }
        else if (selectedItem == 2)
        {
            this.curStatTitle.text = this.player.playerName + "'s Armor stats";
            this.selItemTitle.text = "Selected Weapon Stats";
            // armor
            this.currentStatDisplay.text = player.playerArmor.GetName() + "\n" +
                "ARM: " + player.playerArmor.GetDefMod();
        }


        this.curBalanceText.text = this.playerBalance.text;
        if (selectedItem >= 0){
            this.costText.text = this.shopItems[selectedItem].GetDollarCost().ToString();
            this.remBalanceText.text = (this.player.dollarBalance - this.shopItems[selectedItem].GetDollarCost()).ToString();
        }
        else
        {
            this.costText.text = "0";
            this.remBalanceText.text = this.playerBalance.text;
        }
    }

    private void DoOnFirstTick()
    {
        firstTick = true;
        Invoke("RandomizeCost", 0.01f);
    }

    #region monobehaviour

    void Start()
    {
        SceneFadeHandler.Instance.levelStarting = true;
        AudioManager.Instance.PlayNewSong("Shop");
        AudioManager.Instance.PlaySFX("OpenShop");
        EscapeHandler.instance.GetButtons();

        firstTick = false;
        this.player = FindObjectOfType<PlayerController>();
      //  this.prevPos = this.player.gameObject.transform.localPosition;

        // relocate player
      //  Vector3 newSpot = new Vector3(-5.7f, -2f);
      //  this.player.gameObject.transform.localPosition = newSpot;
        this.player.posController.MovePlayer(28, 27);

        this.player.gameObject.transform.localScale = new Vector3(-1.5f, 1.5f, 1f);

        this.playerBalance.text = this.player.dollarBalance.ToString();

        factory = FindObjectOfType<ItemFactory>();
        this.selectedItemStats.text = "";
        this.currentStatDisplay.text = "";
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
