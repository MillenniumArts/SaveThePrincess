using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {
	// used to handle menu options
	public string firstSceneToLoad;

	public PlayerController player;

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
                   backButton = null ;

	private int baseHealth, baseArmor, baseDamage, baseEnergy, 
				healthInc, armorInc, damageInc, energyInc,
				newHealth, newArmor, newDamage, newEnergy;

	public Text healthAmt, damageAmt, armorAmt, energyAmt, creditText;

	public int numCredits;
    public int MAX_CREDITS = 3;

	public ItemFactory itemFactory;
    Vector3 newPos, prevPos;


    void ButtonInit()
    {
        this.healthUp.onClick.AddListener(() =>
        {
            if (this.numCredits > 0)
            {
                AudioManager.Instance.PlaySFX("Select");
                numCredits--;
                this.newHealth += this.healthInc;
            }
        });
        this.damageUp.onClick.AddListener(() =>
        {
            if (this.numCredits > 0)
            {
                AudioManager.Instance.PlaySFX("Select");
                numCredits--;
                this.newDamage += this.damageInc;
            }
        });
        this.armorUp.onClick.AddListener(() =>
        {
            if (this.numCredits > 0)
            {
                AudioManager.Instance.PlaySFX("Select");
                numCredits--;
                this.newArmor += this.armorInc;
            }
        });
        this.energyUp.onClick.AddListener(() =>
        {
            if (this.numCredits > 0)
            {
                AudioManager.Instance.PlaySFX("Select");
                numCredits--;
                this.newEnergy += this.energyInc;
            }
        });


        // DECREASE STATS
        this.healthDown.onClick.AddListener(() =>
        {
            if (this.numCredits < MAX_CREDITS)
            {
                AudioManager.Instance.PlaySFX("Select");
                if (this.newHealth - this.healthInc < this.baseHealth)	// make sure they can't go below base stats
                    this.newHealth = this.baseHealth;
                else
                {
                    this.newHealth -= this.healthInc;
                    numCredits++;
                }
            }
        });
        this.damageDown.onClick.AddListener(() =>
        {
            if (this.numCredits < MAX_CREDITS)
            {
                AudioManager.Instance.PlaySFX("Select");
                if (this.newDamage - this.damageInc < this.baseDamage)	// make sure they can't go below base stats
                    this.newDamage = this.baseDamage;
                else
                {
                    this.newDamage -= this.damageInc;
                    numCredits++;
                }
            }
        });
        this.armorDown.onClick.AddListener(() =>
        {
            if (this.numCredits < MAX_CREDITS)
            {
                AudioManager.Instance.PlaySFX("Select");
                if (this.newArmor - this.armorInc < this.baseArmor)	// make sure they can't go below base stats
                    this.newArmor = this.baseArmor;
                else
                {
                    this.newArmor -= this.armorInc;
                    numCredits++;
                }
            }
        });

        this.energyDown.onClick.AddListener(() =>
        {
            if (this.numCredits < MAX_CREDITS)
            {
                AudioManager.Instance.PlaySFX("Select");
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
        this.confirm.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX("Select");
            Confirm();
        });
    }

    public void Confirm()
    {
        if (numCredits == 0)
        {
            this.player.totalHealth = this.newHealth;
            //only set health to full if 1st time
            if(PlayerPrefs.GetInt("midgame") == 0)
                this.player.remainingHealth = this.newHealth;
            this.player.physicalDamage = this.newDamage;

            int _defaultDamageMod = Random.Range(0, 6); // Damage modifier for default weapon.
            this.player.physicalDamage += _defaultDamageMod;
            this.player.damageMod = _defaultDamageMod;
            this.player.playerWeapon.SetDmgArm(_defaultDamageMod, 0);
            this.player.totalEnergy = this.newEnergy;
            this.player.remainingEnergy = this.newEnergy;
            this.player.armor = this.newArmor;

            if (EnemyStats.GetInstance().GetFirstEnemyBool())
            {
                EnemyStats.GetInstance().SetEnemyBaseStats(100, 100, 15, 10);
            }

            //DontDestroyOnLoad(this.player);
            LevelLoadHandler.Instance.LoadLevel("Town_LVP");
        }
    }
    public void GoBack()
    {
        AudioManager.Instance.PlaySFX("Select");
        LevelLoadHandler.Instance.LoadStartGame();
        //EscapeHandler.instance.ClearButtons();
        //Application.LoadLevel();
    }


    #region monobehaviour
	void UpdateText(){
		this.healthAmt.text = this.newHealth.ToString();
		this.armorAmt.text =  this.newArmor.ToString();
		this.damageAmt.text = this.newDamage.ToString ();
        this.energyAmt.text = this.newEnergy.ToString();
		this.creditText.text = "CREDITS: " + numCredits;
	}
    // Use this for initialization
    void Start()
    {
        AudioManager.Instance.PlayNewSong("ForestOverworld");
        //EscapeHandler.instance.GetButtons();

        // initialize references
        this.player = FindObjectOfType<PlayerController>();
        this.prevPos = this.player.transform.localPosition;
        newPos = new Vector3(-7.5f, -2.5f);
        this.player.gameObject.transform.localPosition = newPos;

        if (PlayerPrefs.GetInt("midgame") == 1)
        {
            this.backButton.gameObject.SetActive(false);
            this.numCredits = 1;
        }
        if (PlayerPrefs.GetInt("score") == 0)
        {
            EnemyStats.GetInstance().SetVeryFirstEnemy(true);
        }
        else
        {
            EnemyStats.GetInstance().SetVeryFirstEnemy(false);
        }
        if (BattleCounter.GetInstance().GetCurrentBattleCount() == 0)
        {
            EnemyStats.GetInstance().SetStartOfRun(true);
        }

        // get stats from player
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
        this.damageAmt.text = this.newDamage.ToString();
        this.creditText.text = "CREDITS: " + numCredits;

        // value per credit:
        this.healthInc = 10;
        this.damageInc = 5;
        this.armorInc = 5;
        this.energyInc = 10;

        // button handling
        ButtonInit();
    }

	// Update is called once per frame
	void Update () {
		UpdateText ();
	}
    #endregion monobehaviour
}
