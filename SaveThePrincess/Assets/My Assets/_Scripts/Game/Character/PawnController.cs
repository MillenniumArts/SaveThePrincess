using UnityEngine;
using System.Collections;

public class PawnController : MonoBehaviour {
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

	public Transform playerHead;

	public Transform playerBackShoulder;

	public Transform playerFrontShoulder;

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
	/// <summary>
	/// The inventory of the player.
	/// </summary>
	public InventoryGUIController inventory;

	#endregion Inventory

	#region STATS
	/// <summary>
	/// The Maximum Mana for this player
	/// </summary>
	public string playerName ;

	/// <summary>
	/// The Maximum health for this player
	/// </summary>
	public int totalHealth ;
	
	/// <summary>
	/// The Remaining health for this player
	/// </summary>
	public  int remainingHealth;
	
	/// <summary>
	/// The Maximum Mana for this player
	/// </summary>
	public int totalMana ;
	
	/// <summary>
	/// The Remaining Mana for this player
	/// </summary>
	public int remainingMana ;
	
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
	public int damageToTake;
	
	#endregion STATS

	#region Public Functions
	/// <summary>
	/// Used to pass a custom combination to this script from an outside source.
	/// </summary>
	/// <param name="incomingDamage">int Incoming damage applied to the player</param>
	public void TakeDamage(){
		if (damageToTake > 0) {
			if ((damageToTake - this.armor) > Mathf.FloorToInt(this.totalHealth * 0.1f)) {		// if damage is more than 10% of player health
				if (this.remainingHealth - (damageToTake - this.armor) > 0){	// and it doesn't kill the player
					this.remainingHealth -= (damageToTake - this.armor);		// take damage
				}else{
					this.remainingHealth = 0;									// die
				}
				PerformDamageBehaviour ();									// only reacts if damage applies over 10%
			} else if ((damageToTake - this.armor) <= Mathf.FloorToInt(this.totalHealth * 0.1f)){	// if damage is less than 10% of player health
				if (this.remainingHealth - (damageToTake - this.armor) > 0){	// and it doesn't kill the player
						this.remainingHealth -= (damageToTake - this.armor);		// take damage
				}else{
					this.remainingHealth = 0;									// die
				}
				PerformBlockBehaviour ();									// block animation under 10% damage
			}
			Debug.Log (this.name + " laughs at the lack of damage!");
		}
		this.damageToTake = 0;
	}

	/// <summary>
	/// Sets the damage to be applied next turn.
	/// </summary>
	/// <param name="damage">Damage.</param>
	public void SetDamage(int damage){
		this.damageToTake = damage;
	}

	/// <summary>
	/// Heals the player for specified amount.
	/// </summary>
	/// <param name="amount">Amount.</param>
	public virtual void HealForAmount(int amount){
		if (this.remainingHealth < this.totalHealth) {
			this.PerformPotionBehaviour ();
			if (this.remainingHealth + amount > this.totalHealth) {
				this.remainingHealth = this.totalHealth;
			} else {
				this.remainingHealth += amount;
			}
		}
	}
	/// <summary>
	/// Heals for percent.
	/// </summary>
	/// <param name="percent">Percent (from 0.01f to 0.99).</param>
	public virtual void HealForPercent(float percent){
		this.PerformHealMagicBehaviour ();
		if (this.remainingHealth + Mathf.FloorToInt(this.remainingHealth * percent) > this.totalHealth) {
			this.remainingHealth = this.totalHealth;
		} else {
			this.remainingHealth += Mathf.FloorToInt(this.remainingHealth * percent);
		}
	}

	public bool UseMana (int amount){
		if (this.remainingMana - amount <= 0)
			return false;
		else {
			this.remainingMana -= amount;
			return true;
		}
	}

	/// <summary>
	/// Determines whether this instance is dead.
	/// </summary>
	/// <returns><c>true</c> if this instance is dead; otherwise, <c>false</c>.</returns>
	public bool IsDead(){
		return this.remainingHealth <= 0;
	}

	/// <summary>
	/// Deals Damage to specified player.
	/// </summary>
	/// <param name="attackedPlayer">Attacked player.</param>
	public void PhysicalAttack(PawnController attackedPlayer){
		// apply damage to player
		attackedPlayer.damageToTake = this.physicalDamage;
		this.PerformAttackBehaviour ();
	}

	/// <summary>
	/// Performs Magic attack.
	/// </summary>
	/// <returns><c>true</c>, if magic attack was cast, <c>false</c> otherwise.</returns>
	/// <param name="attackedPlayer">Attacked player.</param>
	public bool MagicAttack(PawnController attackedPlayer){
		// animate sprites
		if (this.remainingMana - 10 >= 0) {
			this.remainingMana -= 10;
			attackedPlayer.damageToTake = this.magicalDamage;
			this.PerformMagicBehaviour ();
			return true;
		} else {
			Debug.Log ("Not Enough Mana For That!");
			return false;
		}
	}

	/// <summary>
	/// Gives the specified amount of mana.
	/// </summary>
	/// <param name="amount">Amount.</param>
	public void GiveMana(int amount){
		if (this.remainingMana + amount >= this.totalMana) {
			this.remainingMana = this.totalMana;
		} else {
			this.remainingMana += amount;
		}
	}

	/// <summary>
	/// Calls the SetWeapon method to set the weapon at the player's hand.
	/// </summary>
	/// <param name="name">Determines the weapon created. Sword, Axe, Bow, Hammer, Dagger, Spear.</param>
	public void CallSetWeapon(string name){
		SetWeapon(name);
	}
	
	/// <summary>
	/// Calls the SetArmor coroutine to set the armor at the player's body.
	/// </summary>
	/// <param name="name">Determines the armor created. Armor.</param>
	public void CallSetArmor(string name){
		SetArmor (name);
	}

	#endregion Public Functions

	#region protected functions
	protected void SetWeapon(string name){
		if(playerHand.GetComponentInChildren<Weapon>() == true){				// If there is a weapon in hand..
			Destroy(playerHand.GetComponentInChildren<Weapon>().gameObject);	// .. Destroy it.
		}
		this.playerWeapon = ItemFactory.instance.CreateWeapon(playerHand, name);// Spawn specified weapon.
		this.playerWeapon.transform.parent = playerHand;										// Make it the child of the hand.
		this.playerWeapon.transform.localScale = new Vector3(1,1,1);							// Fix the scale.
		this.weaponComboScript = playerWeapon.GetComponentInChildren<CreateWeaponCombination>();// Sets a reference to the weapon's script.
	}
	
	protected void SetArmor(string name){
		// JAKE I CHANGED THIS LINE HERE.
		// THANKS Carlo! :)
		GameObject body = playerBody.gameObject;	// Gets a reference for the body to see if there..
													// .. is a an armor on the character's body.
		if(body.GetComponentInChildren<Armor>() == true){				// If there is Armor on the body..
			Destroy(body.GetComponentInChildren<Armor>().gameObject);	// .. Destroy it.
		}
		this.playerArmor = ItemFactory.instance.CreateArmor(playerBody, name);	// Spawn specified weapon.
		this.playerArmor.transform.parent = playerBody;								// Make it the child of the hand.
		this.playerArmor.transform.localScale = new Vector3(1,1,1);							// Fix the scale.
		if(spawnWithArmor){
			this.playerHead.gameObject.GetComponent<SpriteRenderer>().sprite = playerArmor.helmetArmorOptionsSprites[playerArmor.typeIndex];
			this.playerFrontShoulder.gameObject.GetComponent<SpriteRenderer>().sprite = playerArmor.frontShoulderArmorOptionsSprites[playerArmor.typeIndex];
			this.playerBackShoulder.gameObject.GetComponent<SpriteRenderer>().sprite = playerArmor.backShoulderArmorOptionsSprites[playerArmor.typeIndex];
		}
	}

	/// <summary>
	/// Triggers the animation.
	/// </summary>
	/// <param name="state">State.</param>
	public void TriggerAnimation(string state){
		switch (state) {
		case "attack": 
			this.playerAnimator.SetTrigger(this.playerWeapon.GetAnimParameter());
			break;
		case "AtkMagic": 
			this.playerAnimator.Play ("magic up");
			break;
		case "potion": 
			this.playerAnimator.Play ("human_Use item");
			break;
		case "death": 
			this.playerAnimator.Play ("human_Knee death");
			break;
		case "HealMagic": 
			this.playerAnimator.Play ("heal magic");
			break;
		case "damage": 
			this.playerAnimator.Play ("human_Hit2");
			break;
		case "block":
			this.playerAnimator.Play ("human_Block");
			break;
		}
	}
	/// <summary>
	/// Performs the potion behaviour.
	/// </summary>
	protected virtual void PerformPotionBehaviour(){
		TriggerAnimation ("potion");
	}
	/// <summary>
	/// Performs the attack behaviour.
	/// </summary>
	protected virtual void PerformAttackBehaviour(){
		TriggerAnimation ("attack");
	}
	/// <summary>
	/// Performs the death behaviour.
	/// </summary>
	protected virtual void PerformDeathBehaviour(){
		TriggerAnimation ("death");
	}
	/// <summary>
	/// Performs the magic behaviour.
	/// </summary>
	protected virtual void PerformMagicBehaviour(){
		TriggerAnimation ("AtkMagic");
	}
	/// <summary>
	/// Performs the heal magic behaviour.
	/// </summary>
	protected virtual void PerformHealMagicBehaviour(){
		TriggerAnimation ("HealMagic");
	}
	/// <summary>
	/// Performs the damage behaviour.
	/// </summary>
	protected virtual void PerformDamageBehaviour(){
		TriggerAnimation ("damage");
	}
	/// <summary>
	/// Performs the block behaviour.
	/// </summary>
	protected virtual void PerformBlockBehaviour(){
		TriggerAnimation ("block");
	}
	#endregion protected Functions

	#region skeletonTransform
	/// <summary>
	/// Gets the back hand transform.
	/// </summary>
	/// <returns>The back hand transform.</returns>
	public Transform GetBackHandTransform(){
		//body.spriteSheetElements[5] = shield
		// get all spriterenderers
		SpriteRenderer[] s = this.body.character.GetComponentsInChildren<SpriteRenderer>();
		// get sword name
		string n = this.body.character.sprites [5].name;
		// find spriteRenderer
		Transform t=null;
		for (int i=0; i<s.Length; i++) {
			if (s[i].sprite.name == n){
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
	public Transform GetFrontHandTransform(){
		//body.spriteSheetElements[7] = sword
		// get all spriterenderers
		SpriteRenderer[] s = this.body.character.GetComponentsInChildren<SpriteRenderer>();
		
		// get sword name
		string n = this.body.character.sprites [7].name;
		// find spriteRenderer
		Transform t=null;
		for (int i=0; i<s.Length; i++) {
			if (s[i].sprite.name == n){
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
	public Transform GetBodyTransform(){
		//body.spriteSheetElements[0] = body
		// get all spriterenderers
		SpriteRenderer[] s = this.body.character.GetComponentsInChildren<SpriteRenderer>();
		// get body name
		string n = this.body.character.sprites [0].name;
		// find spriteRenderer
		Transform t=null;
		for (int i=0; i<s.Length; i++) {
			if (s[i].sprite.name == n){
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

	public void PawnControllerStart(){
		this.body = GameObject.FindWithTag (this.tag).GetComponentInChildren<CreateCombination> ();
		this.playerAnimator = GetComponentInChildren<PlayerMoveAnim>().gameObject.GetComponent<Animator>();

		// initialize weapon if player is supposed to have one
		CallSetWeapon("Sword");
		CallSetArmor("Armor");
		if (!spawnWithWeapon) {			//
			this.weaponComboScript.AllOff ();	// Creates a weapon, sets it to the player's hand and makes it invisible.
			this.weaponComboScript.SwapNow ();
		}
		if (!spawnWithArmor) {
			this.playerArmor.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
		}
	}

	public bool firstTick = true;
	
	public void DoOnFirstTick(){
		if(firstTick){
			if(spawnWithWeapon){							
				if(playerWeapon != null){
					damageMod = playerWeapon.GetAtkMod();
					physicalDamage += damageMod;
				}
				if(playerArmor != null){
					armorMod = playerWeapon.GetDefMod();
					armor += armorMod;
				}
			}else{
				this.damageMod = playerWeapon.GetAtkMod();	
				//this.physicalDamage = physicalDamage - damageMod;
				this.physicalDamage += damageMod;
				this.armorMod = playerArmor.GetDefMod();
				//this.armor = armor - armorMod;
				this.armor += armorMod;
				playerWeapon.ClearStats();
				playerArmor.ClearStats();
				playerWeapon.itemName = "None";
				playerArmor.itemName = "None";
				damageMod = 0;
				armorMod = 0;
				this.playerWeapon.animParameter = "OneHandAttack";
			}
			firstTick = false;
		}
	}

	#endregion Constructors

	//set up default parameters here for all characters in game
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
