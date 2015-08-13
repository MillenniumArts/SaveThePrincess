using UnityEngine;
using System.Collections;

/// <summary>
/// Body armor class.
/// </summary>
public class BodyArmor : Armor {
	// Sub-Class of the BodyArmor Sub-Class.
	[SerializeField] private string subClassName;
	// Minimums and maximums of the stats.
	[SerializeField] private int atkMin;
	[SerializeField] private int atkMax;
	[SerializeField] private int defMin;
	[SerializeField] private int defMax;
	[SerializeField] private int spdMin;
	[SerializeField] private int spdMax;
	[SerializeField] private int hpMin;
	[SerializeField] private int hpMax;
	[SerializeField] private int manaMin;
	[SerializeField] private int manaMax;
	
	/// <summary>
	/// Sets up this instance of the class.
	/// </summary>
	void Start(){
		factory = FindObjectOfType<ItemFactory>();
		//GetBodyArmorType();
        //StartCoroutine("RenderArmorAtEoF");
        ArmourStart();
	}
    /*
	/// <summary>
	/// Gets the complete armor sprites and renders them on the player.
	/// </summary>
	/// <param name="backS">Back shoulder.</param>
	/// <param name="frontS">Front shoulder.</param>
	/// <param name="head">Head/Helmet.</param>
	public void RenderCompleteArmor(GameObject backS, GameObject frontS, GameObject head, BodyArmor body){
		body.gameObject.GetComponent<SpriteRenderer>().sprite = bodyArmorOptionsSprites[typeIndex];
		backS.GetComponent<SpriteRenderer>().sprite = backShoulderArmorOptionsSprites[typeIndex];
		frontS.GetComponent<SpriteRenderer>().sprite = frontShoulderArmorOptionsSprites[typeIndex];
		head.GetComponent<SpriteRenderer>().sprite = helmetArmorOptionsSprites[typeIndex];
	}
	
	/// <summary>
	/// Gets the type of the body armor.
	/// </summary>
	private void GetBodyArmorType(){
		typeIndex = Random.Range(0, armorOptionsTypes.Length);
	}*/

   // protected IEnumerator RenderArmorAtEoF()
    private void ArmourStart()
    {
        //yield return new WaitForEndOfFrame();
        RandomSetNum();
        DisplayArmourInStore();
        SetItem(className, NameRandomizer.instance.GetPart1() + /*armorOptionsTypes[typeIndex]*/ GetName() + NameRandomizer.instance.GetPart2(),
            /*bodyArmorOptionsSprites[typeIndex]*/null, animationParameter, idleAnimParameter, subClassName, "None" /*armorOptionsTypes[typeIndex]*/,
                "none", 0, factory.GetModPwr(atkMin, atkMax), factory.GetModPwr(defMin, defMax),
                factory.GetModPwr(spdMin, spdMax), factory.GetModPwr(hpMin, hpMax), factory.GetModPwr(manaMin, manaMax));
        SendArmourSetToRenderer();
    }

    private string GetName()
    {
        string name = " ";
        if (this.gameObject.GetComponentInChildren<SpriteRenderer>().sprite != null)
        {
            name = this.gameObject.GetComponentInChildren<SpriteRenderer>().sprite.name;
            string[] _name = name.Split('_');
            if (_name.Length > 2)
                name = _name[2];
            else
                name = "None";
        }
        return name;
    }
    //For Testing
    /*public bool yee = false;
    public string testSaveString;
    void Update()
    {
        if (yee)
        {
            LoadArmourSprites(testSaveString);
        }
    }*/
}
