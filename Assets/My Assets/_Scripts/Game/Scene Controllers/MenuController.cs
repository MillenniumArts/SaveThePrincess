using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {
	// used to handle menu options
	public string firstSceneToLoad;

	public PlayerController player;
	public MenuController menuController;

	[SerializeField]
	private Button healthUp = null, 
				   healthDown = null,
				   damageUp = null,
				   damageDown = null,
				   armorUp = null, 
				   armorDown= null,
                   energyUp= null,
                   energyDown = null, 
				   confirm = null,
                   backButton ;

	private int baseHealth, baseArmor, baseDamage, baseEnergy, 
				healthInc, armorInc, damageInc, energyInc,
				newHealth, newArmor, newDamage, newEnergy;

	public Text healthAmt, damageAmt, armorAmt, energyAmt, creditText;

	public int numCredits;
    public int MAX_CREDITS = 5;

	public ItemFactory itemFactory;
    Vector3 newPos;

	// Use this for initialization
	void Start () {
        EscapeHandler.instance.GetButtons();
		GameObject menuControllerObject = GameObject.FindWithTag ("MenuController");
		
		if (menuControllerObject != null) {
			menuController = menuControllerObject.GetComponent<MenuController> ();
		} 
		if (menuController == null) {
			Debug.Log("Cannot find MenuObject!");
			return;
		}
	
        this.player = FindObjectOfType<PlayerController>();

		// value per credit:
		this.healthInc = 10;
		this.damageInc = 5;
		this.armorInc = 5;
        this.energyInc = 10;

		// base stats
		this.baseHealth = this.player.totalHealth;
		this.baseDamage = this.player.physicalDamage;
		this.baseArmor = this.player.armor;
        this.baseEnergy = this.player.totalEnergy;

		// variables to be changed
		this.newHealth = this.baseHealth;
		this.newDamage = this.baseDamage;
		this.newArmor = this.baseArmor;
        this.newEnergy = this.baseEnergy;

		this.healthAmt.text = this.newHealth.ToString();
		this.armorAmt.text = this.newArmor.ToString();
        this.energyAmt.text = this.newEnergy.ToString();
		this.damageAmt.text = this.newDamage.ToString ();
		this.creditText.text = "CREDITS: " + numCredits;


		// button handling - INCREASE
		this.healthUp.onClick.AddListener (()=>{
			if (this.numCredits > 0){
				numCredits--;
				this.newHealth += this.healthInc;
			}
		});
		this.damageUp.onClick.AddListener (()=>{
			if (this.numCredits > 0){
				numCredits--;
				this.newDamage += this.damageInc;
			}
		});
		this.armorUp.onClick.AddListener (()=>{
			if (this.numCredits > 0){
				numCredits--;
				this.newArmor += this.armorInc;
			}
		});
        this.energyUp.onClick.AddListener(() =>
        {
            if (this.numCredits > 0)
            {
                numCredits--;
                this.newEnergy += this.energyInc;
            }
        });


		// DECREASE STATS
		this.healthDown.onClick.AddListener (()=>{
			if (this.numCredits < MAX_CREDITS){
				if (this.newHealth - this.healthInc < this.baseHealth)	// make sure they can't go below base stats
					this.newHealth = this.baseHealth;
				else{
					this.newHealth -= this.healthInc;
					numCredits++;
				}
			}
		});
		this.damageDown.onClick.AddListener (()=>{
			if (this.numCredits < 10){
				if (this.newDamage - this.damageInc < this.baseDamage)	// make sure they can't go below base stats
					this.newDamage = this.baseDamage;
				else{
					this.newDamage -= this.damageInc;
					numCredits++;
				}
			}
		});
		this.armorDown.onClick.AddListener (()=>{
			if (this.numCredits < 10){
				if (this.newArmor - this.armorInc < this.baseArmor)	// make sure they can't go below base stats
					this.newArmor = this.baseArmor;
				else{
					this.newArmor -= this.armorInc;
					numCredits++;
				}
			}
		});

        this.energyDown.onClick.AddListener(() =>
        {
            if (this.numCredits < 10)
            {
                if (this.newEnergy - this.energyInc < this.baseEnergy)	// make sure they can't go below base stats
                    this.newEnergy = this.baseEnergy;
                else
                {
                    this.newEnergy -= this.energyInc;
                    numCredits++;
                }
            }
        });
        
		// LOAD NEXT SCENE WITH THIS PLAYER
		this.confirm.onClick.AddListener (()=>{
			if (numCredits == 0){
				this.player.totalHealth = this.newHealth;
				this.player.remainingHealth = this.newHealth;
				this.player.physicalDamage = this.newDamage;
                int _defaultDamageMod = Random.Range(0, 6); // Damage modifier for default weapon.
                this.player.physicalDamage += _defaultDamageMod;
                this.player.damageMod = _defaultDamageMod;
                this.player.playerWeapon.SetDmgArm(_defaultDamageMod, 0);
                this.player.totalEnergy = this.newEnergy;
                this.player.remainingEnergy = this.newEnergy;
				this.player.armor = this.newArmor;

                EnemyStats.GetInstance().SetEnemyBaseStats(player.remainingHealth, player.remainingEnergy, player.physicalDamage, player.armor);

				DontDestroyOnLoad(this.player);

				Application.LoadLevel (firstSceneToLoad);
			}else{

			}
		});

	}

    public void GoBack()
    {
        EscapeHandler.instance.ClearButtons();
        Application.LoadLevel("StartMenu_LVP");
    }

	void UpdateText(){
		this.healthAmt.text = this.newHealth.ToString();
		this.armorAmt.text =  this.newArmor.ToString();
		this.damageAmt.text = this.newDamage.ToString ();
        this.energyAmt.text = this.newEnergy.ToString();
		this.creditText.text = "CREDITS: " + numCredits;
	}

	// Update is called once per frame
	void Update () {
		UpdateText ();
	}
}
