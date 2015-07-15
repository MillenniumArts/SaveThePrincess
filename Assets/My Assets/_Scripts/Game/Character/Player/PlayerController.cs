using UnityEngine;
using System.Collections;


public class PlayerController : PawnController
{

    #region Variables
    /// <summary>
    /// The chosen class for this player
    /// </summary>
    public string classType;

    /// <summary>
    /// The number of usable items for this player
    /// </summary>
    public int numUsableItems;

    /// <summary>
    /// The player's Inventory
    /// </summary>
    public PlayerInventory inventory;
    #endregion Variables

    #region Public functions

    /// <summary>
    /// Uses the item at specified index.
    /// </summary>
    /// <param name="index">Index.</param>
    public void UseItem(int index)
    {
       /* bool ret = false;
        if (this.inventory == null)
        {
            this.inventory = GameObject.FindObjectOfType<InventoryGUIController>();
        }

        // if player has enough mana
        bool canUseMana = this.CanUseMana(10);

        if (canUseMana)
        {
            ret = true;
            // checks to see if item can be used on self (false if attack or full stats)
            bool canApplyToSelf = this.inventory._items[index].CanApplyEffectsToSelf(this);

            if (canApplyToSelf)
            {
                // apply item effect to self
                this.inventory._items[index].ApplyEffect(this);
                this.TriggerAnimation(this.inventory._items[index].GetItemSubClass());
                this.inventory.DisableButtonsIfUsed();
            }
            else
            {
                //apply effects to enemy
                BaseEnemyController e = FindObjectOfType<BaseEnemyController>();
                if (e != null)
                {
                    this.inventory._items[index].ApplyEffect(e);
                    this.remainingEnergy -= 10;
                    this.TriggerAnimation(this.inventory._items[index].GetItemSubClass());
                    this.inventory.DisableButtonsIfUsed();
                }

            }
            //this.remainingMana -= 10;
        }
        else
        {
            Debug.Log("Now is not the time to use that!");
        }
        return ret;
       */
    }

    /// <summary>
    /// Purchases the item and returns true if player has enough money, returns false if not.
    /// </summary>
    /// <returns><c>true</c>, if item was purchased, <c>false</c> otherwise.</returns>
    /// <param name="itemCost">Item cost.</param>
    public bool PurchaseItem(int itemCost)
    {
        if (this.dollarBalance - itemCost >= 0)
        {
            this.dollarBalance -= itemCost;
            return true;
        }
        else
        {
            //Debug.Log("Not enough money for that!");
            return false;
        }
    }

    /// <summary>
    /// Transfers the purchased weapon to the player's hand.
    /// </summary>
    /// <param name="w">The weapon to be transfered.</param>
    public void TransferPurchasedWeapon(Item w)
    {
        this.physicalDamage -= damageMod;
        this.playerWeapon.SwapTo(w);							// Swaps all the stats.
        this.playerWeapon.SetCombination(w.GetComponentInChildren<CreateCombination>().GetCurrentComboArray()); // Sets a combination.
        this.playerWeapon.GiveCombination(w.GetItemSubClass());	// Swaps all the sprites to the new weapon.
        this.playerAnimator.SetBool(w.idleAnimParameter, w.idleState);
        if (playerWeapon.GetItemSubClass() == "Spear")
        {
            playerAnimator.SetBool("IsSpearAttack", true);
        }
        else
        {
            playerAnimator.SetBool("IsSpearAttack", false);
        }
        this.damageMod = w.GetAtkMod();
        this.physicalDamage += damageMod;
        SetWeaponHands(w.gameObject.GetComponentInChildren<CreateWeaponCombination>());
    }

    /// <summary>
    /// Transfers the purchased armor to the player's body.
    /// </summary>
    /// <param name="a">The armour to be transfered.</param>
    public void TransferPurchasedArmor(Item a)
    {
        this.armor -= armorMod;				// Remove the current defence stat modifier.
        Armor _a = (Armor)a;				// Casts the Item to Armor.  Used to access CopyTypeIndex().
        this.playerArmor.SwapTo(_a);		// Swaps all the stats from the new armor to the player's armor.
        this.armorMod = _a.GetDefMod();		// Gets the defence stat modifier.
        this.armor += armorMod;				// Add the defence stat modifier.
        if (this.playerArmor.gameObject.GetComponentInChildren<SpriteRenderer>().enabled == false)
        {
            this.playerArmor.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
            // Renders the new armor.
            this.playerArmor.SwapArmorSprites(_a); // Swaps armor sprites from the new armor to the player's armor.
            this.playerArmor.RenderCompleteArmor(playerBackShoulder, playerFrontShoulder, playerHead, playerArmor);
        }
        else
        {
            // Renders the new armor.
            this.playerArmor.SwapArmorSprites(_a); // Swaps armor sprites from the new armor to the player's armor.
            this.playerArmor.RenderCompleteArmor(playerBackShoulder, playerFrontShoulder, playerHead, playerArmor);
        }
    }

    #endregion Public functions

    #region Private functions
    private bool firstTick = true;
    /// <summary>
    /// Things to do on the first tick for the player.
    /// </summary>
    private void DoOnFirstTick()
    {
        if (firstTick)
        {
            if (spawnWithWeapon || spawnWithArmor)
            {
                if (spawnWithWeapon)
                {
                    if (playerWeapon != null)
                    {
                        damageMod = playerWeapon.GetAtkMod();
                        playerWeapon.itemName = "Wooden Sword";
                        physicalDamage += damageMod;
                    }
                }
                else
                {
                    playerWeapon.ClearStats();
                    playerWeapon.itemName = "Fist";
                }

                if (spawnWithArmor)
                {
                    if (playerArmor != null)
                    {
                        armorMod = playerArmor.GetDefMod();
                        playerArmor.itemName = "Cloth";
                        armor += armorMod;
                    }
                }
                else
                {
                    playerArmor.ClearStats();
                    playerArmor.itemName = "Naked Skin";
                }
            }
            else
            {
                this.damageMod = playerWeapon.GetAtkMod();
                //this.physicalDamage = physicalDamage - damageMod;
                this.physicalDamage += damageMod;
                this.armorMod = playerArmor.GetDefMod();
                //this.armor = armor - armorMod;
                this.armor += armorMod;
                playerWeapon.ClearStats();
                playerArmor.ClearStats();
                playerWeapon.itemName = "Fist";
                playerArmor.itemName = "Bare Skin";
                damageMod = 0;
                armorMod = 0;
                this.playerWeapon.animParameter = "OneHandAttack";
            }
            firstTick = false;
        }
    }

    /// <summary>
    /// Clean up player stats for restart on last tick.
    /// </summary>
    private void DoOnLastTick()
    {
        this.playerArmor.ClearStats();
        this.playerWeapon.ClearStats();
        //this.TriggerAnimation("death");
    }


    #endregion Private functions

    #region MonoBehaviour
    /// <summary>
    ///Initialize the player with values	
    /// </summary>
    void Start()
    {
        frontThumb = GameObject.Find("thumb_front_player");
        backThumb = GameObject.Find("thumb_back_player");
        backFingers = GameObject.Find("fingers_back_player");
        PawnControllerStart();
        this.playerAnimator = GetComponentInChildren<PlayerMoveAnim>().gameObject.GetComponent<Animator>();
        this.dollarBalance = 50;
        
        playerArmor.ClearStats();
        playerArmor.itemName = "";
        playerWeapon.itemName = "";
        //damageMod = 0;
        armorMod = 0;
    }

    void Update()
    {
        DoOnFirstTick();
        if (IsDead())
        {
            DoOnLastTick();
        }
        UpdateHealth();
    }
    #endregion MonoBehaviour
}

