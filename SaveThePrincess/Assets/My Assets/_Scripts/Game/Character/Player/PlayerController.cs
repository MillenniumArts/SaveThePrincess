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
    /// Purchases the item.
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
            Debug.Log("Not enough money for that!");
            return false;
        }
    }

    /// <summary>
    /// Transfers the purchased weapon to the player's hand.
    /// </summary>
    /// <param name="w">The weapon to be transfered.</param>
    public void TransferPurchasedWeapon(Item w)
    {
        this.physicalDamage = physicalDamage - damageMod;
        this.playerWeapon.SwapTo(w);							// Swaps all the stats.
        this.playerWeapon.SetCombination(w.GetComponentInChildren<CreateCombination>().GetCurrentComboArray()); // Sets a combination.
        this.playerWeapon.GiveCombination(w.GetItemSubClass());	// Swaps all the sprites to the new weapon.
        this.playerAnimator.SetBool(w.idleAnimParameter, w.idleState);
        this.damageMod = w.GetAtkMod();
        this.physicalDamage = physicalDamage + damageMod;
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
    /// <summary>
    /// Gets the total armor.
    /// </summary>
    /// <returns>The total armor.</returns>
    
    #endregion Public functions

    #region Private functions
    /// <summary>
    /// Clean up player stats for restart on last tick.
    /// </summary>
    private void DoOnLastTick()
    {
        this.playerArmor.ClearStats();
        this.playerWeapon.ClearStats();
    }


    #endregion Private functions

    #region MonoBehaviour
    /// <summary>
    ///Initialize the player with values	
    /// </summary>
    void Start()
    {
        PawnControllerStart();
        this.playerAnimator = GetComponentInChildren<PlayerMoveAnim>().gameObject.GetComponent<Animator>();
        this.dollarBalance = 50;
        this.armorMod = playerArmor.GetDefMod();
        //this.armor = armor - armorMod;
        this.armor += armorMod;
        playerArmor.ClearStats();
        playerArmor.itemName = "None";
        armorMod = 0;
    }

    void Update()
    {
        DoOnFirstTick();
        if (IsDead())
        {
            DoOnLastTick();
            this.TriggerAnimation("death");
        }
        playerAnimator.SetInteger("Health", remainingHealth);
    }
    #endregion MonoBehaviour
}

