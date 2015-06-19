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
    public CreateWeaponCombination weaponComboScript;

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
    #endregion Rendering

    #region Inventory
    

    #endregion Inventory

    #region STATS
    /// <summary>
    /// The Maximum Mana for this player
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
    public int armorMod;

    /// <summary>
    /// The base physical damage for this player (BEFORE ENCHANTMENTS) 
    /// </summary>
    public int physicalDamage;

    /// <summary>
    /// physicalDamage stat modifier.
    /// </summary>
    public int damageMod;

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
    /// The Damage Reduction Factor for armor
    /// </summary>
    public float DMG_REDUCTION_FACTOR;

    /// <summary>
    /// The Base Damage if armor nullifies attack.
    /// </summary>
    public int BASE_DMG;

    /// <summary>
    /// Base Energy Cost for any attack
    /// </summary>
    public int ATTACK_ENERGY_COST = 30;

    #endregion STATS

    #region Public Functions
    /// <summary>
    /// Used to apply damage to player stored in damageToTake;
    /// </summary>
    public void TakeDamage()
    {
        int defecit;
        if (this.physicalDamageToTake > 0)
        {
            if (this.physicalDamageToTake > this.armor) // if true damage will apply after armor
            {
            // only amount of damage over armor amount is true damage
                int trueDamage = this.physicalDamageToTake - this.armor;
                if (trueDamage < 0)
                    trueDamage = 0;
                int reducedDamage = Mathf.FloorToInt((this.physicalDamageToTake - trueDamage) / 2);
            //amount of damage to take is the equivalent of:
                // 50% of damage for each 'matched' point of armor
                // 100% of damage for any left over points of armor
            defecit = trueDamage + reducedDamage;
            }
            else
            {
                defecit = Mathf.FloorToInt(this.physicalDamage / 2);
            }

            
            // ensure min damage is always applied
            if (defecit < BASE_DMG)
                defecit = BASE_DMG;

            //handle animation for damage
            if (defecit > Mathf.FloorToInt(this.totalHealth * 0.1f))
            {		// if damage is more than 10% of player health
                if (this.remainingHealth - defecit > 0)
                {	// and it doesn't kill the player
                    this.remainingHealth -= defecit;		// take damage
                }
                else
                {
                    this.remainingHealth = 0;									// die
                }
                PerformDamageBehaviour();									// only reacts if damage applies over 10%
            }
            else if (defecit <= Mathf.FloorToInt(this.totalHealth * 0.1f) && defecit >= 0)
            {	// if damage is less than 10% of player health
                if (this.remainingHealth - defecit > 0)
                {	// and it doesn't kill the player
                    this.remainingHealth -= defecit;		// take damage
                }
                else
                {
                    this.remainingHealth = 0;									// die
                }
                PerformBlockBehaviour();									// block animation under 10% damage
                Debug.Log(this.name + " laughs at the lack of damage!");
            } if (defecit <= 0)
            {
                Debug.Log("BASE DMG APPLIED");
                // apply base damage since armor nullified
                if (this.remainingHealth - BASE_DMG > 0)
                {	// and it doesn't kill the player
                    this.remainingHealth -= BASE_DMG;		// take damage
                }
                else
                {
                    this.remainingHealth = 0;									// die
                }
                PerformBlockBehaviour();									// block animation under 10% damage
                Debug.Log(this.name + " laughs at the lack of damage!");
            }
        this.physicalDamageToTake = 0;
        }
        if (this.magicalDamageToTake > 0)
        {
            // IF WE WANT MAGIC RESIST!!!
            /*defecit = this.magicalDamageToTake - this.magicResist */

            // just take straight damage
            if (this.remainingHealth - this.magicalDamageToTake >= 0)
            {
                this.remainingHealth -= this.magicalDamageToTake;
            }
            else
            {
                this.remainingHealth = 0;
            }
            this.magicalDamageToTake = 0;
            PerformDamageBehaviour();
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
    public virtual void HealForAmount(int amount)
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
    public virtual void HealForPercent(float percent)
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
    public virtual void HealForPercent(int percent)
    {        
        // set to float to use
        float newPercent = percent * 0.01f;
        Debug.Log("Healing for " + newPercent + " percent");
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
        Debug.Log(this.name + " Receieved " + amount + " Energy.");
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
    /// Uses amount of energy if possible
    /// </summary>
    /// <param name="amount"></param>
    public void UseEnergy(int amount)
    {
        if (CanUseEnergy(amount)){
            this.remainingEnergy -= amount;
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
    /// Attacks provided pawncontroller, passing in the amount from the attack bar in the Battle
    /// </summary>
    /// <param name="attackedPlayer">Player Being attacked</param>
    /// <param name="attackAmount">the amount from the attack bar in the Battle</param>
    public void Attack(PawnController attackedPlayer, float attackAmount)
    {
        // Check Weapon type for behavior
        if (this.playerWeapon.GetItemClass() == "Weapon"){
            PhysicalAttack(attackedPlayer, attackAmount);
        }
        else if (this.playerWeapon.GetItemClass() == "Magic")
        {
            MagicAttack(attackedPlayer, attackAmount);

        }else{
            // other attack types?
        }
        // all attacks always use energy
        this.UseEnergy(ATTACK_ENERGY_COST);
    }

    
    /// <summary>
    /// Deals Damage to specified player.
    /// </summary>
    /// <param name="attackedPlayer">Attacked player.</param>
    /// /// <param name="attackAmount">Amount on the AttackBar in the Battle Sequence.</param>
    public void PhysicalAttack(PawnController attackedPlayer, float attackAmount)
    {
        // apply damage to player
        attackedPlayer.physicalDamageToTake = this.physicalDamage;
        this.PerformAttackBehaviour();
    }

    /// <summary>
    /// Performs Magic attack.
    /// </summary>
    /// <returns><c>true</c>, if magic attack was cast (had enough mana), <c>false</c> otherwise.</returns>
    /// <param name="attackedPlayer">Attacked player.</param>
    public bool MagicAttack(PawnController attackedPlayer, float attackAmount)
    {
        if (this.CanUseEnergy(ATTACK_ENERGY_COST))
        {
            attackedPlayer.magicalDamageToTake = this.magicalDamage;
            this.PerformMagicBehaviour();
            return true;
        }
        else
        {
            Debug.Log("Not Enough Mana For That!");
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
                this.playerAnimator.SetTrigger("Take_Damage");
                break;
            case "block":
                this.playerAnimator.SetTrigger("Block");
                break;
            case "victory":
                if (this.remainingHealth / this.totalHealth > 0.75)
                {
                    this.playerAnimator.SetTrigger("human_winHigh");
                }
                else if (this.remainingHealth / this.totalHealth < 0.75
                      && this.remainingHealth / this.totalHealth < 0.75)
                {
                    this.playerAnimator.SetTrigger("human_winMid");
                }
                else if (this.remainingHealth / this.totalHealth < 0.25)
                {
                    this.playerAnimator.SetTrigger("human_winLow");
                }
                break;
        }
    }
    /// <summary>
    /// Get the total armor
    /// </summary>
    /// <returns></returns>
    public int GetTotalArmor()
    {
        return this.armor;// + this.playerArmor.GetDefMod() + this.playerWeapon.GetDefMod();
    }
    /// <summary>
    /// Gets the total damage.
    /// </summary>
    /// <returns>The total damage.</returns>
    public int GetTotalDamage()
    {
        return this.physicalDamage;// + this.playerArmor.GetAtkMod() + this.playerWeapon.GetAtkMod();
    }

    public int GetTotalHeatlhEnergyStats()
    {
        int total = totalEnergy + totalHealth;
        return total;
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
        this.weaponComboScript = playerWeapon.GetComponentInChildren<CreateWeaponCombination>();// Sets a reference to the weapon's script.
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
        if (spawnWithArmor)
        {
            // Calls coroutine to that renders the armor.  Coroutine is the method below this one.
            StartCoroutine("RenderArmorAtEoF");
        }
    }

    /// <summary>
    /// Wait until the end of the frame before rendering the armor.
    /// </summary>
    /// <returns>WaitForEndOfFrame()</returns>
    protected IEnumerator RenderArmorAtEoF()
    {
        yield return new WaitForEndOfFrame();
        // Renders the armor.
        this.playerArmor.RenderCompleteArmor(playerBackShoulder, playerFrontShoulder, playerHead, playerArmor);
    }

    /// <summary>
    /// Sets the animation parameter that determines if the idle animation is "tired" or not.
    /// </summary>
    protected void UpdateHealth()
    {
        float healthPercent = (float)remainingHealth / (float)totalHealth;
        playerAnimator.SetFloat("Health", healthPercent);
    }

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
