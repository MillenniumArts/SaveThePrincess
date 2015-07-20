using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour
{
    // used to handle menu options
    public string firstSceneToLoad;

    public PlayerController player;

    [SerializeField]
    private Button healthUp = null,
                   healthDown = null,
                   damageUp = null,
                   damageDown = null,
                   armorUp = null,
                   armorDown = null,
                   energyUp = null,
                   energyDown = null,
                   confirm = null,
                   backButton = null;

    public Text healthAmt,
                damageAmt,
                armorAmt,
                energyAmt,
                creditText,
                numHpCredits,
                numNrgCredits,
                numDmgCredits,
                numArmCredits,
                prevHP,
                prevNRG,
                prevDmg,
                prevArm;

    private int baseHealth, baseArmor, baseDamage, baseEnergy,
                healthInc, armorInc, damageInc, energyInc,
                newHealth, newArmor, newDamage, newEnergy;

    public int numCredits, numDMG, numARM, numNRG, numHP;
    public int MAX_CREDITS = 3;

    public ItemFactory itemFactory;
    Vector3 newPos, prevPos;

    public void StatUp(int index)
    {
        if (this.numCredits > 0)
        {
            AudioManager.Instance.PlaySFX("Button1");
            switch (index)
            {
                case 0: // HP
                    numCredits--;
                    this.numHP++;
                    this.newHealth += this.healthInc;
                    break;
                case 1:// NRG
                    numCredits--;
                    this.numNRG++;
                    this.newEnergy += this.energyInc;
                    break;
                case 2:// DMG
                    numCredits--;
                    this.numDMG++;
                    this.newDamage += this.damageInc;
                    break;
                case 3:// ARM
                    numCredits--;
                    this.numARM++;
                    this.newArmor += this.armorInc;
                    break;
                default:
                    break;
            }
        }
    }

    public void StatDown(int index)
    {
        AudioManager.Instance.PlaySFX("Button1");
        if (this.numCredits <= this.MAX_CREDITS)
        {
            switch (index)
            {
                case 0://HP
                    if (this.newHealth - this.healthInc < this.baseHealth)	// make sure they can't go below base stats
                        this.newHealth = this.baseHealth;
                    else
                    {
                        this.newHealth -= this.healthInc;
                        numCredits++;
                        this.numHP--;
                    }
                    break;
                case 1://NRG
                    if (this.newEnergy - this.energyInc < this.baseEnergy)	// make sure they can't go below base stats
                        this.newEnergy = this.baseEnergy;
                    else
                    {
                        this.newEnergy -= this.energyInc;
                        numCredits++;
                        this.numNRG--;
                    }
                    break;
                case 2://DMG
                    if (this.newDamage - this.damageInc < this.baseDamage)	// make sure they can't go below base stats
                        this.newDamage = this.baseDamage;
                    else
                    {
                        this.newDamage -= this.damageInc;
                        numCredits++;
                        this.numDMG--;
                    }
                    break;
                case 3://ARM
                    if (this.newArmor - this.armorInc < this.baseArmor)	// make sure they can't go below base stats
                        this.newArmor = this.baseArmor;
                    else
                    {
                        this.newArmor -= this.armorInc;
                        numCredits++;
                        this.numARM--;
                    }
                break;
                default:
                break;
            }
        }
    }


    public void Confirm()
    {
        AudioManager.Instance.PlaySFX("Button1");
        if (numCredits == 0)
        {
            this.player.totalHealth = this.newHealth;
            //only set health to full if 1st time
            if (PlayerPrefs.GetInt("midgame") == 0)
                this.player.remainingHealth = this.newHealth;
            this.player.physicalDamage = this.newDamage;
            this.player.totalEnergy = this.newEnergy;
            this.player.remainingEnergy = this.newEnergy;
            this.player.armor = this.newArmor;

            LevelLoadHandler.Instance.LoadLevel("Town_LVP");
        }
    }
    public void GoBack()
    {
        AudioManager.Instance.PlaySFX("Button1");
        LevelLoadHandler.Instance.LoadLevel("LoadSave_LVP");
        //EscapeHandler.instance.ClearButtons();
        //Application.LoadLevel();
    }

    public void FirstEnemy()
    {
        if (EnemyStats.GetInstance().IsFirstEnemy())
        {
            EnemyStats.GetInstance().SetEnemyBaseStats(100, 100, 15, 10);
            int _defaultDamageMod = Random.Range(0, 6);                 // Damage modifier for default weapon.
            this.player.playerWeapon.SetDmgArm(_defaultDamageMod, 0);   //
        }
    }


    void UpdateText()
    {
        this.healthAmt.text = this.newHealth.ToString();
        this.armorAmt.text = this.newArmor.ToString();
        this.damageAmt.text = this.newDamage.ToString();
        this.energyAmt.text = this.newEnergy.ToString();

        this.prevArm.text = this.baseArmor.ToString();
        this.prevDmg.text = this.baseDamage.ToString();
        this.prevNRG.text = this.baseEnergy.ToString();
        this.prevHP.text = this.baseHealth.ToString();

        this.creditText.text = "CREDITS: " + numCredits;
        this.numArmCredits.text = this.numARM.ToString();
        this.numDmgCredits.text = this.numDMG.ToString();
        this.numHpCredits.text = this.numHP.ToString();
        this.numNrgCredits.text = this.numNRG.ToString();
    }
    #region monobehaviour
    // Use this for initialization
    void Start()
    {
        SceneFadeHandler.Instance.levelStarting = true;
        AudioManager.Instance.PlayNewSong("ForestOverworld");

        // initialize references
        this.player = FindObjectOfType<PlayerController>();
        this.prevPos = this.player.transform.localPosition;
        newPos = new Vector3(-14f, -2.5f);
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

        //first Enemy handling
        Invoke("FirstEnemy", 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }
    #endregion monobehaviour
}
