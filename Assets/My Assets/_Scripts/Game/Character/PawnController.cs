using UnityEngine;
using System.Collections;

public class PawnController : MonoBehaviour
{
    #region Rendering
    /// <summary>
    /// Reference to the CreateCombination that is attached to this player, holds reference to Sprite
    /// </summary>
    public CreateCombination body;

    /// <summary>
    /// The weapon combo script.
    /// </summary>
    public WeaponCombination weaponComboScript;

    /// <summary>
    /// The player's body.
    /// </summary>
    public Transform playerBody;

    public GameObject playerHead;

    public GameObject playerBackShoulder;

    public GameObject playerFrontShoulder;

    /// <summary>
    /// The player's hand.
    /// </summary>
    public Transform playerHand;

    /// <summary>
    /// The player animator.
    /// </summary>
    public Animator playerAnimator;

    ///<summary>
    ///Player Armor - An array of Item objects that represents all armor objects for the player
    ///</summary>
    public BodyArmor playerArmor = null;

    ///<summary>
    ///Player Weapons - An array of Item objects that represents all weapon objects for the player
    ///</summary>
    public Weapon playerWeapon = null;

    /// <summary>
    /// whether or not player will spawn with weapon.
    /// </summary>
    public bool spawnWithWeapon;

    /// <summary>
    /// whether or not player will spawn with weapon.
    /// </summary>
    public bool spawnWithArmor;

    public PlayerPositionController posController;
    #endregion Rendering

    #region STATS
    /// <summary>
    /// The Player's name
    /// </summary>
    public string playerName;

    /// <summary>
    /// The Maximum health for this player
    /// </summary>
    public int totalHealth;

    /// <summary>
    /// The Remaining health for this player
    /// </summary>
    public int remainingHealth;

    /// <summary>
    /// The Maximum Mana for this player
    /// </summary>
    public int totalEnergy;

    /// <summary>
    /// The Remaining Mana for this player
    /// </summary>
    public int remainingEnergy;

    /// <summary>
    /// The speed of the player, used for dodge probablility
    /// </summary>
    public int speed;

    /// <summary>
    /// The Armor for this player, used for damage reduction on hit
    /// </summary>
    public int armor;

    /// <summary>
    /// The armor stat modifier.
    /// </summary>
   // public int armorMod;

    /// <summary>
    /// The base physical damage for this player (BEFORE ENCHANTMENTS) 
    /// </summary>
    public int physicalDamage;

    /// <summary>
    /// physicalDamage stat modifier.
    /// </summary>
    //public int damageMod;

    /// <summary>
    /// The base magic damage for this player (BEFORE ENCHANTMENTS) 
    /// </summary>
    public int magicalDamage;

    /// <summary>
    /// The heal per turn.
    /// </summary>
    public int healPerTurn;

    /// <summary>
    /// The player's dollar balance usable at the store.
    /// </summary>
    public int dollarBalance;

    /// <summary>
    /// The damage to take next turn.
    /// </summary>
    public int physicalDamageToTake;

    /// <summary>
    /// The magical damage to take next turn
    /// </summary>
    public int magicalDamageToTake;

    /// <summary>
    /// Base Energy Cost for any attack
    /// </summary>
    public int ATTACK_ENERGY_COST = 30;

    /// <summary>
    /// Food heals per turn, specifies how many turns are left before heal stops
    /// </summary>
    public int numTurnsLeftToHeal;

    public bool inBattle;

    #endregion STATS

    #region Public Functions

    /// <summary>
    /// Sets the number of turns to heal, adds to number if more than 0.
    /// </summary>
    /// <param name="numTurns"></param>
    public void SetNumTurnsToHeal(int numTurns)
    {
        if (this.numTurnsLeftToHeal == 0)
        {
            this.numTurnsLeftToHeal = numTurns;
        }
        else
        {
            this.numTurnsLeftToHeal += numTurns;
        }
    }

    /// <summary>
    /// Used to apply damage to player stored in damageToTake;
    /// </summary>
    public void TakeDamage()
    {
        int defecit=0;
        if (this.physicalDamageToTake > 0)
        {
            if (this.physicalDamageToTake > this.armor) // if true damage will apply after armor
            {
            // only amount of damage over armor amount is true damage
                int trueDamage = this.physicalDamageToTake - this.armor;
                int reducedDamage = Mathf.RoundToInt((this.physicalDamageToTake - trueDamage)/2f);
                defecit = trueDamage + reducedDamage;
            }
            else
            {
                defecit = Mathf.RoundToInt(this.physicalDamageToTake / 2);
            }

            if (this.remainingHealth - defecit > 0)
            {	// and it doesn't kill the player
                this.remainingHealth -= defecit;		// take damage
            }
            else
            {
                this.remainingHealth = 0;				// die
            }

            //handle animation for damage
            if (defecit > Mathf.FloorToInt(this.totalHealth * 0.1f))
            {		// if damage is more than 10% of player health
                PerformDamageBehaviour();
            }
            else if (defecit <= Mathf.FloorToInt(this.totalHealth * 0.1f))
            {
                PerformBlockBehaviour();
            }
            
            this.physicalDamageToTake = 0;
        }
        else
        {
            //PerformVictoryBehaviour();
        }
        // MAGICAL DAMAGE
        if (this.magicalDamageToTake > 0)
        {
            if (this.remainingHealth - this.magicalDamageToTake >= 0)
            {
                this.remainingHealth -= this.magicalDamageToTake;
            }
            else
            {
                this.remainingHealth = 0;
            }

            //handle animation for damage
            if (magicalDamageToTake > Mathf.FloorToInt(this.totalHealth * 0.1f))
            {		// if damage is more than 10% of player health
                PerformDamageBehaviour();
            }
            else if (magicalDamageToTake <= Mathf.FloorToInt(this.totalHealth * 0.1f))
            {
                PerformBlockBehaviour();
            }
            this.magicalDamageToTake = 0;
            PerformDamageBehaviour();
        }
        else
        {
            //PerformVictoryBehaviour();
        }
    }

    /// <summary>
    /// Sets the damage to be applied next turn.
    /// </summary>
    /// <param name="damage">Damage.</param>
    public void SetDamage(int damage, bool physical)
    {
        if (physical)
            this.physicalDamageToTake = damage;
        else
            this.magicalDamageToTake = damage;
    }

    /// <summary>
    /// Heals the player for specified amount.
    /// </summary>
    /// <param name="amount">Amount.</param>
    public virtual void GiveHealthAmount(int amount)
    {
        if (this.remainingHealth < this.totalHealth)
        {
            this.PerformPotionBehaviour();
            if (this.remainingHealth + amount > this.totalHealth)
            {
                this.remainingHealth = this.totalHealth;
            }
            else
            {
                this.remainingHealth += amount;
            }
        }
    }
    
    /// <summary>
    /// Heals for percent specified by a float (0.01f - 0.99f)
    /// </summary>
    /// <param name="percent">Percent (from 0.01f to 0.99f).</param>
    public virtual void GiveHealthPercent(float percent)
    {
        if (this.remainingHealth + Mathf.FloorToInt(this.totalHealth * percent) > this.totalHealth)
        {
            this.remainingHealth = this.totalHealth;
        }
        else
        {
            this.remainingHealth += Mathf.FloorToInt(this.totalHealth * percent);
        }
    }

    /// <summary>
    /// Heals for percent specified by Integer value (must be less than 100)
    /// </summary>
    /// <param name="percent"></param>
    public virtual void GiveHealthPercent(int percent)
    {        
        // set to float to use
        float newPercent = percent * 0.01f;
        // can't be over 100% heal
        if (newPercent > 1)
            newPercent = 1.0f;

        if (this.remainingHealth + Mathf.FloorToInt(this.totalHealth * newPercent) > this.totalHealth)
        {
            this.remainingHealth = this.totalHealth;
        }
        else
        {
            this.remainingHealth += Mathf.FloorToInt(this.totalHealth * newPercent);
        }
    }

    /// <summary>
    /// Gives the specified amount of energy.
    /// </summary>
    /// <param name="amount">Amount.</param>
    public virtual void GiveEnergyAmount(int amount)
    {
        if (this.remainingEnergy + amount >= this.totalEnergy)
        {
            this.remainingEnergy = this.totalEnergy;
        }
        else
        {
            this.remainingEnergy += amount;
        }
    }
    /// <summary>
    /// Gives specified percentage to player as defined by float value (0.01f - 0.99f)
    /// </summary>
    /// <param name="percent"></param>
    public virtual void GiveEnergyPercent(float percent)
    {
        if (this.remainingEnergy + Mathf.FloorToInt(this.totalEnergy * percent) > this.totalEnergy)
        {
            this.remainingEnergy = this.totalEnergy;
        }
        else
        {
            this.remainingEnergy += Mathf.FloorToInt(this.totalEnergy * percent);
        }
    }
    /// <summary>
    /// Gives specified percentage to player as defined by Int value (1-99)
    /// </summary>
    /// <param name="percent"></param>
    public virtual void GiveEnergyPercent(int percent)
    {
        // set to float to use
        float newPercent = percent * 0.01f;

        // can't be over 100% heal
        if (newPercent > 1)
            newPercent = 1;

        if (this.remainingEnergy + Mathf.FloorToInt(this.totalEnergy * newPercent) > this.totalEnergy)
        {
            this.remainingEnergy = this.totalEnergy;
        }
        else
        {
            this.remainingEnergy += Mathf.FloorToInt(this.totalEnergy * newPercent);
        }

    }

    /// <summary>
    /// Determines whether this instance is dead.
    /// </summary>
    /// <returns><c>true</c> if this instance is dead; otherwise, <c>false</c>.</returns>
    public bool IsDead()
    {
        return this.remainingHealth <= 0;
    }

    /// <summary>
    /// Uses amount of energy if possible, bottoms out otherwise (sets to 0). 
    /// DAMAGE REDUCTION ON 'BOTTOMING OUT' IS HANDLED IN THE GAME CONTROLLER IN CASE MAGIC AND PHYS HAVE DIFFERENT BEHAVIOURS
    /// </summary>
    /// <param name="amount"></param>
    public void UseEnergy(int amount)
    {
        if (CanUseEnergy(amount)){
            this.remainingEnergy -= amount;
        }
        else
        {
            this.remainingEnergy = 0;
        }
    }
    /// <summary>
    /// Determines if player has enough mana
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool CanUseEnergy(int amount)
    {
        if (remainingEnergy <= 0)
            return false;
        int defecit = this.remainingEnergy - amount;
        return defecit >= 0;
    }

    /// <summary>
    /// Attacks provided pawncontroller, passing in the amount of damage calculated from the attack bar in the Battle
    /// </summary>
    /// <param name="attackedPlayer">Player Being attacked</param>
    /// <param name="appliedDamage">the actual percent of damage applied from the attack bar in the Battle</param>
    public void Attack(PawnController attackedPlayer, int appliedDamage, int energyUsed)
    {
        // Check Weapon type for behavior
        if (this.playerWeapon.GetItemClass() == "Weapon"){
            PhysicalAttack(attackedPlayer, appliedDamage);
        }
        else if (this.playerWeapon.GetItemClass() == "Magic")
        {
            MagicAttack(attackedPlayer, appliedDamage);

        }else{
            // other attack types?
        }
        // all attacks always use energy
        this.UseEnergy(energyUsed);
    }

    
    /// <summary>
    /// Deals Damage to specified player.
    /// </summary>
    /// <param name="attackedPlayer">Attacked player.</param>
    /// /// <param name="damageToApply">Amount on the AttackBar in the Battle Sequence.</param>
    public void PhysicalAttack(PawnController attackedPlayer, int damageToApply)
    {
        // apply damage to player
        attackedPlayer.physicalDamageToTake = damageToApply;
        this.PerformAttackBehaviour();
    }

    /// <summary>
    /// Performs Magic attack.
    /// </summary>
    /// <returns><c>true</c>, if magic attack was cast (had enough mana), <c>false</c> otherwise.</returns>
    /// <param name="attackedPlayer">Attacked player.</param>
    public bool MagicAttack(PawnController attackedPlayer, int damageToApply)
    {
        if (this.CanUseEnergy(ATTACK_ENERGY_COST))
        {
            attackedPlayer.magicalDamageToTake = damageToApply;
            this.PerformMagicBehaviour();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Calls the SetWeapon method to set the weapon at the player's hand.
    /// </summary>
    /// <param name="name">Determines the weapon created. Sword, Axe, Bow, Hammer, Dagger, Spear.</param>
    public void CallSetWeapon(string name)
    {
        SetWeapon(name);
    }

    /// <summary>
    /// Calls the SetArmor coroutine to set the armor at the player's body.
    /// </summary>
    /// <param name="name">Determines the armor created. Armor.</param>
    public void CallSetArmor(string name)
    {
        SetArmor(name);
    }

    /// <summary>
    /// Triggers the animation.
    /// </summary>
    /// <param name="state">State.</param>
    public void TriggerAnimation(string state)
    {
        state = state.ToLower();
        switch (state)
        {
            case "enterbattle":
                //his.playerAnimator.SetTrigger("Enter");
                break;
            case "attack":
                this.playerAnimator.SetTrigger("One_Hand_Attack");
                break;
            case "attackmagic":
                this.playerAnimator.SetTrigger("Magic_Attack");
                break;
            case "energypotion":
                this.playerAnimator.SetTrigger("Magic_Heal");
                break;
            case "healpotion":
                this.playerAnimator.SetTrigger("Use_Item");
                break;
            case "death":
                this.playerAnimator.SetTrigger("Death");
                break;
            case "healmagic":
                this.playerAnimator.SetTrigger("Magic_Up");
                break;
            case "damage":
                if ((float)this.physicalDamageToTake / (float)this.totalHealth > 0.65f)
                {
                    this.playerAnimator.SetTrigger("Take_Damage_2");
                }
                else if ((float)this.physicalDamageToTake / (float)this.totalHealth <= 0.65f
                    && (float)this.physicalDamageToTake / (float)this.totalHealth >= 0.25f)
                {
                    this.playerAnimator.SetTrigger("Take_Damage_1");
                }
                else if ((float)this.physicalDamageToTake / (float)this.totalHealth < 0.25)
                {
                    this.playerAnimator.SetTrigger("Take_Damage_0");
                }
                break;
            case "block":
                this.playerAnimator.SetTrigger("Block");
                break;
            case "victory":
                if ((float)this.remainingHealth / (float)this.totalHealth > 0.75)
                {
                    this.playerAnimator.SetTrigger("human_winHigh");
                }
                else if ((float)this.remainingHealth / (float)this.totalHealth <= 0.75f
                      && (float)this.remainingHealth / (float)this.totalHealth >= 0.25f)
                {
                    this.playerAnimator.SetTrigger("human_winMid");
                }
                else if ((float)this.remainingHealth / (float)this.totalHealth < 0.25f)
                {
                    this.playerAnimator.SetTrigger("human_winLow");
                }
                break;
        }
    }

    /// <summary>
    /// Is the pawn in battle? True or False.  Sets the Idle animation.
    /// </summary>
    /// <param name="b">Boolean</param>
    public void InBattle(bool b)
    {
        this.playerAnimator.SetBool("InBattle", b);
        SetWeaponHands(playerWeapon.gameObject.GetComponentInChildren<WeaponCombination>(), !b);
    }


    /// <summary>
    /// Get the total armor
    /// </summary>
    /// <returns></returns>
    public int GetTotalArmor()
    {
        return this.armor + this.playerArmor.GetDefMod();// + this.playerWeapon.GetDefMod();
    }
    /// <summary>
    /// Gets the total damage.
    /// </summary>
    /// <returns>The total damage.</returns>
    public int GetTotalDamage()
    {
        return this.physicalDamage + this.playerWeapon.GetAtkMod();// + this.playerArmor.GetAtkMod() ;
    }
    /// <summary>
    /// returns a sum of player's health and energy
    /// </summary>
    /// <returns>sum of player's health and energy</returns>
    public int GetTotalHeatlhEnergyStats()
    {
        int total = totalEnergy + totalHealth;
        return total;
    }

    /// <summary>
    /// returns a string in format STAT:###;STAT:###;... for Load/Save
    /// </summary>
    /// <returns>string in format STAT:###;STAT:###</returns>
    public string GetPlayerStatString()
    {
        /* ORDER:
         * remHP
         * totHP
         * remNRG
         * totNRG
            * [Magic]
         * DMG
         * ARM
         * wDMG
         * wARM
         * aDMG
         * aARM
         */

        string ret = "";
        // HP
        ret += "rHP:" + this.remainingHealth + ";";
        ret += "tHP:" + this.totalHealth + ";";
        // NRG
        ret += "rNRG:" + this.remainingEnergy + ";";
        ret += "tNRG:" + this.totalEnergy + ";";
        //Magic
        //ret += "MAG:" + this.player.magicalDamage = ";";  
        // DMG
        ret += "DMG:" + this.physicalDamage + ";";
        // ARM
        ret += "ARM:" + this.armor + ";";

        // weapon stats
        ret += "wDMG:" + this.playerWeapon.GetAtkMod() + ";";
        ret += "wARM:" + this.playerWeapon.GetDefMod() + ";";
        // armor stats
        ret += "aDMG:" + this.playerArmor.GetAtkMod() + ";";
        ret += "aARM:" + this.playerArmor.GetDefMod() + ";";
        return ret;
    }

    /// <summary>
    /// returns a primitive string in format "STAT:###;" with base total stats to recreate enemies
    /// </summary>
    /// <returns>primitive string in format "STAT:###;" with base total stats to recreate enemies</returns>
    public string GetEnemyStatString()
    {
        /* rHP
         * tHP
         * eNRG
         * tNRG
         * DMG
         * ARM
         */

        string ret = "";
        ret += "erHP:" + this.remainingHealth + ";";
        ret += "etHP:" + this.totalHealth + ";";
        ret += "erNRG:" + this.remainingEnergy + ";";
        ret += "etNRG:" + this.totalEnergy+ ";";
        ret += "eDMG:" + this.physicalDamage + ";";
        ret += "eARM:" + this.armor + ";";

        return ret;
    }

    #endregion Public Functions

    #region protected functions
    protected void SetWeapon(string name)
    {
        if (playerHand.GetComponentInChildren<Weapon>() == true)
        {				// If there is a weapon in hand..
            Destroy(playerHand.GetComponentInChildren<Weapon>().gameObject);	// .. Destroy it.
        }
        this.playerWeapon = ItemFactory.instance.CreateWeapon(playerHand, name);// Spawn specified weapon.
        this.playerWeapon.transform.parent = playerHand;										// Make it the child of the hand.
        this.playerWeapon.transform.localScale = new Vector3(1, 1, 1);							// Fix the scale.
        this.weaponComboScript = playerWeapon.GetComponentInChildren<WeaponCombination>();// Sets a reference to the weapon's script.
    }

    // Variables for Swap WeaponHands
    public GameObject OneHandGrip;
    public GameObject TwoHandGrip;

    public void SetWeaponHands(WeaponCombination _w, bool calm)
    {
        if (!calm) // In battle.
        {
            if (_w.GetWeaponGrip() == true) 
            {
                // Set hands to one handed weapon
                OneHandGrip.SetActive(true);
                TwoHandGrip.SetActive(false);
                playerHand.transform.localPosition = new Vector3(-0.08f, -0.36f, 0f);
                playerWeapon.GetComponentInChildren<WeaponCombination>().SwitchHandleLayer(true);
                //Debug.Log("In Battle, One Hand");
            }
            else
            {
                // Set hands to two handed weapon
                OneHandGrip.SetActive(false);
                TwoHandGrip.SetActive(true);
                playerHand.transform.localPosition = new Vector3(-0.08f, 0.17f, 0f);
                playerWeapon.GetComponentInChildren<WeaponCombination>().SwitchHandleLayer(false);
                //Debug.Log("In Battle, Two Hand");
            }
        }
        else // Out of battle.
        {
            if (_w.GetWeaponGrip() == true)
            {
                // Set hands to one handed weapon
                OneHandGrip.SetActive(true);
                TwoHandGrip.SetActive(false);
                playerHand.transform.localPosition = new Vector3(-0.08f, -0.36f, 0f);
                playerWeapon.GetComponentInChildren<WeaponCombination>().SwitchHandleLayer(true);
                //Debug.Log("Out Battle, One Hand");
            }
            else
            {
                // Set hands to two handed weapon
                OneHandGrip.SetActive(true);
                TwoHandGrip.SetActive(false);
                playerHand.transform.localPosition = new Vector3(-0.08f, -0.36f, 0f);
                playerWeapon.GetComponentInChildren<WeaponCombination>().SwitchHandleLayer(true);
                //Debug.Log("Out Battle, Two Hand");
            }
        }
    }

    protected void SetArmor(string name)
    {
        GameObject body = playerBody.gameObject;	// Gets a reference for the body to see if there..
        if (body.GetComponentInChildren<Armor>() == true){				// If there is Armor on the body..
            Destroy(body.GetComponentInChildren<Armor>().gameObject);	// .. Destroy it.
        }
        this.playerArmor = ItemFactory.instance.CreateArmor(playerBody, name);	// Spawn specified Armor.
        this.playerArmor.transform.parent = playerBody;							// Make it the child of the body.
        this.playerArmor.transform.localScale = new Vector3(1, 1, 1);			// Fix the scale.
        /*if (spawnWithArmor)
        {
            // Calls coroutine to that renders the armor.  Coroutine is the method below this one.
            StartCoroutine("RenderArmorAtEoF");
        }*/
    }
    /*
    /// <summary>
    /// Wait until the end of the frame before rendering the armor.
    /// </summary>
    /// <returns>WaitForEndOfFrame()</returns>
    protected IEnumerator RenderArmorAtEoF()
    {
        yield return new WaitForEndOfFrame();
        // Renders the armor.
        this.playerArmor.RenderCompleteArmor(playerBackShoulder, playerFrontShoulder, playerHead, playerArmor);
    }*/

    /// <summary>
    /// Sets the animation parameter that determines if the idle animation is "tired" or not.
    /// </summary>
    protected void UpdateHealth()
    {
        float healthPercent = (float)remainingHealth / (float)totalHealth;
        playerAnimator.SetFloat("Health", healthPercent);
    }

    #region perform behaviours

    /// <summary>
    /// Performs the potion behaviour.
    /// </summary>
    protected virtual void PerformPotionBehaviour()
    {
        TriggerAnimation("potion");
    }
    /// <summary>
    /// Performs the attack behaviour.
    /// </summary>
    protected virtual void PerformAttackBehaviour()
    {
        TriggerAnimation("attack");
    }
    /// <summary>
    /// Performs the death behaviour.
    /// </summary>
    protected virtual void PerformDeathBehaviour()
    {
        TriggerAnimation("death");
    }
    /// <summary>
    /// Performs the magic behaviour.
    /// </summary>
    protected virtual void PerformMagicBehaviour()
    {
        TriggerAnimation("AttackMagic");
    }
    /// <summary>
    /// Performs the heal magic behaviour.
    /// </summary>
    protected virtual void PerformHealMagicBehaviour()
    {
        TriggerAnimation("HealMagic");
    }
    /// <summary>
    /// Performs the damage behaviour.
    /// </summary>
    protected virtual void PerformDamageBehaviour()
    {
        TriggerAnimation("damage");
    }
    /// <summary>
    /// Performs the block behaviour.
    /// </summary>
    protected virtual void PerformBlockBehaviour()
    {
        TriggerAnimation("block");
    }

    protected virtual void PerformVictoryBehaviour()
    {
        TriggerAnimation("victory");
    }
    #endregion perform behaviours
    #endregion protected Functions

    #region skeletonTransform
    /// <summary>
    /// Gets the back hand transform.
    /// </summary>
    /// <returns>The back hand transform.</returns>
    public Transform GetBackHandTransform()
    {
        //body.spriteSheetElements[5] = shield
        // get all spriterenderers
        SpriteRenderer[] s = this.body.character.GetComponentsInChildren<SpriteRenderer>();
        // get sword name
        string n = this.body.character.sprites[5].name;
        // find spriteRenderer
        Transform t = null;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i].sprite.name == n)
            {
                t = s[i].transform;
                return t;
            }
        }
        return t;
    }

    /// <summary>
    /// Gets the front hand transform.
    /// </summary>
    /// <returns>The front hand transform.</returns>
    public Transform GetFrontHandTransform()
    {
        //body.spriteSheetElements[7] = sword
        // get all spriterenderers
        SpriteRenderer[] s = this.body.character.GetComponentsInChildren<SpriteRenderer>();

        // get sword name
        string n = this.body.character.sprites[7].name;
        // find spriteRenderer
        Transform t = null;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i].sprite.name == n)
            {
                t = s[i].transform;
                return t;
            }
        }
        return t;
    }

    /// <summary>
    /// Gets the body transform.
    /// </summary>
    /// <returns>The body transform.</returns>
    public Transform GetBodyTransform()
    {
        //body.spriteSheetElements[0] = body
        // get all spriterenderers
        SpriteRenderer[] s = this.body.character.GetComponentsInChildren<SpriteRenderer>();
        // get body name
        string n = this.body.character.sprites[0].name;
        // find spriteRenderer
        Transform t = null;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i].sprite.name == n)
            {
                t = s[i].transform;
                return t;
            }
        }
        return t;
    }
    #endregion skeletonTransform

    #region SpriteCustomization



    #endregion SpriteCustomization

    #region Constructors

    public void PawnControllerStart()
    {
        this.body = GameObject.FindWithTag(this.tag).GetComponentInChildren<CreateCombination>();
        this.playerAnimator = GetComponentInChildren<PlayerMoveAnim>().gameObject.GetComponent<Animator>();
        this.inBattle = false;
        this.posController = this.gameObject.GetComponent<PlayerPositionController>();
        // initialize weapon if player is supposed to have one
        CallSetWeapon("Sword");
        if (!spawnWithWeapon)
        {			//
            this.weaponComboScript.AllOff();	// Creates a weapon, sets it to the player's hand and makes it invisible.
            this.weaponComboScript.SwapNow();
        }

        if (spawnWithArmor)
        {
            int randomNum = Random.Range(0, 3);
            switch (randomNum)
            {
                case 0: CallSetArmor("HeavyArmor");
                    break;
                case 1: CallSetArmor("MediumArmor");
                    break;
                case 2: CallSetArmor("LightArmor");
                    break;
            }
        }
        else
        {
            CallSetArmor("BlankArmor");
            this.playerArmor.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
    }

    #endregion Constructors
   
    //set up default parameters here for all characters in game
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
