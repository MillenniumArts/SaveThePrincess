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
	public Armor playerArmor = null;

	///<summary>
	///Player Weapons - An array of Item objects that represents all weapon objects for the player
	///</summary>
	public Weapon playerWeapon = null;
	
	/// <summary>
	/// whether or not player will spawn with weapon.
	/// </summary>
	public bool spawnWithWeapon;
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
	/// The base physical damage for this player (BEFORE ENCHANTMENTS) 
	/// </summary>
	public int physicalDamage ;
	
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
	
	
	#endregion STATS

	#region Public Functions
	/// <summary>
	/// Used to pass a custom combination to this script from an outside source.
	/// </summary>
	/// <param name="incomingDamage">int Incoming damage applied to the player</param>
	public void TakeDamage(int incomingDamage){
		if ((incomingDamage - Mathf.FloorToInt(this.armor*0.5f)) > 0) {		// if damage is going to apply after armor
			if (this.remainingHealth - (incomingDamage - Mathf.FloorToInt(this.armor*0.5f)) > 0)
				this.remainingHealth -= (incomingDamage - Mathf.FloorToInt(this.armor*0.5f));
			else
				this.remainingHealth = 0;
		}
	}
	/// <summary>
	/// Heals the player for specified amount.
	/// </summary>
	/// <param name="amount">Amount.</param>
	public virtual void HealForAmount(int amount){
		if (this.remainingHealth + amount > this.totalHealth) {
			this.remainingHealth = this.totalHealth;
		} else {
			this.remainingHealth += amount;
		}
		this.PerformPotionBehaviour ();
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
		this.PerformAttackBehaviour ();
		attackedPlayer.TakeDamage (this.physicalDamage);
		// animate sprites		
		this.PerformAttackBehaviour ();
	}
	
	public bool MagicAttack(PawnController attackedPlayer){
		// animate sprites
		this.PerformMagicBehaviour ();

		if (this.remainingMana - 10 >= 0) {
			this.remainingMana -= 10;
			attackedPlayer.TakeDamage (this.magicalDamage);
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
		GameObject body = GetBodyTransform().gameObject;	// Gets a reference for the body to see if there..
		//Debug.Log (body);												// .. is a an armor on the character's body.
		if(body.GetComponentInChildren<Armor>() == true){				// If there is Armor on the body..
			Destroy(body.GetComponentInChildren<Armor>().gameObject);	// .. Destroy it.
		}
		this.playerArmor = ItemFactory.instance.CreateArmor(playerBody, name);	// Spawn specified weapon.
		this.playerArmor.transform.parent = playerBody;								// Make it the child of the hand.
		this.playerArmor.transform.localScale = new Vector3(1,1,1);							// Fix the scale.
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


	//set up default parameters here for all characters in game
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
