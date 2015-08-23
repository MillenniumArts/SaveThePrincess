using UnityEngine;
using System.Collections;

public class BaseEnemyController : PawnController {

	
	#region Variables
	/// <summary>
	/// The chosen class for this player
	/// </summary>
	public string classType ;
		
	/// <summary>
	/// The number of usable items for this player
	/// </summary>
	public int numUsableItems ;

    public bool isAnimating;

    private PlayerController player;

    private int healthEnergy;

	#endregion Variables
	
	#region Public functions

	/// <summary>
	/// Transfers the purchased weapon to the player's hand.
	/// </summary>
	/// <param name="w">The weapon to be transfered.</param>
	public void TransferPurchasedWeapon(Item w){
		this.playerWeapon.SwapTo(w);							// Swaps all the stats.
		this.playerWeapon.SetCombination(w.GetComponentInChildren<CreateCombination>().GetCurrentComboArray()); // Sets a combination.
		this.playerWeapon.GiveCombination(w.GetItemSubClass());	// Swaps all the sprites to the new weapon.
		this.playerAnimator.SetBool(w.idleAnimParameter, w.idleState);
	}
	
	/// <summary>
	/// Transfers the purchased armor to the player's body.
	/// </summary>
	/// <param name="a">The armour to be transfered.</param>
	public void TransferPurchasedArmor(Item a){
		this.playerArmor.SwapTo(a);
		if(this.playerArmor.gameObject.GetComponentInChildren<SpriteRenderer>().enabled == false){
			this.playerArmor.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
		}
	}

    public void SetStats(int remainingHP, int totalHP, int remainingNRG, int totalNRG, int ATK, int DEF)
    {
        this.remainingHealth = remainingHP;
        this.totalHealth = totalHP;
        this.remainingEnergy = remainingNRG;
        this.totalEnergy = totalNRG;
        this.physicalDamage = ATK;
        this.armor = DEF;
       /* Debug.Log(" EnemyTotalHealth: " + totalHealth +
            " EnemyTotalEnergy: " + totalEnergy +
            " EnemyPhysicalDamage: " + physicalDamage +
            " EnemyArmor: " + armor);*/
    }

	#endregion Public functions

	#region Protected Functions


	#endregion Protected Functions

	#region Private functions

    /// <summary>
    /// Creates the ememy's stats based on the player stats.
    /// </summary>
    private void CreateStats()
    {
        EnemyStats.GetInstance().GetEnemyBaseStats(this, player);
    }

	private void EnemyStart(){
        this.dollarBalance = 35 + (BattleCounter.GetInstance().GetCurrentBattleCount() * 10);
        if (this.dollarBalance > 150)
        {
            this.dollarBalance = 150;
        }
        CreateStats();
        //remainingEnergy = totalEnergy;
        //remainingHealth = totalHealth;
	}

	/// <summary>
	/// Drops a random amount of money after enemy dies.
	/// </summary>
	public int DropMoney(){
		return Random.Range (Mathf.FloorToInt(this.dollarBalance/2) + 1 , this.dollarBalance);
	}

	#endregion Private functions
	
	#region MonoBehaviour 
	/// <summary>
	///Initialize the Enemy with values	
	/// </summary>
	void Start(){
		EnemyStart();
		PawnControllerStart();
	}

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        isAnimating = false;
    }

    bool weaponMod = true;

    void Update()
    {
        UpdateHealth();
        if (playerArmor.GetDefMod() > 0 && playerWeapon.GetAtkMod() > 0)
        {
            if (weaponMod)
            {
                if (playerArmor != null)
                {
                    //Debug.Log("Armor " + Mathf.FloorToInt(this.playerArmor.GetDefMod()));
                    this.playerArmor.SetArmor(Mathf.FloorToInt(this.playerArmor.GetDefMod() * 0.75f));
                   // Debug.Log("Armor " + this.playerArmor.GetDefMod());
                }
                if (playerWeapon != null)
                {
                    //Debug.Log("Damage " + Mathf.FloorToInt(this.playerWeapon.GetAtkMod()));
                    this.playerWeapon.SetDamage(Mathf.FloorToInt(this.playerWeapon.GetAtkMod() * 0.75f));
                    //Debug.Log("Damage " + this.playerWeapon.GetAtkMod());
                }
                weaponMod = false;
            }
        }
	}
	#endregion MonoBehaviour
}
