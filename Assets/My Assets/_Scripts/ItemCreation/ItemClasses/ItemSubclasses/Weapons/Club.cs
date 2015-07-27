using UnityEngine;
using System.Collections;

/// <summary>
/// Club class.
/// </summary>
public class Club : Weapon
{
    /// <summary>
    /// The club type options.
    /// </summary>
    public string[] clubOptionsTypes;
    /// <summary>
    /// The club type options sprites.
    /// </summary>
    public Sprite[] clubOptionsSprites;
    /// <summary>
    /// The index of the type.
    /// </summary>
    private int typeIndex;
    // The minimums and maximums of stats.
    public int atkMin;
    public int atkMax;
    public int defMin;
    public int defMax;
    public int spdMin;
    public int spdMax;
    public int hpMin;
    public int hpMax;
    public int manaMin;
    public int manaMax;

    /// <summary>
    /// Sets up this instance of the class.
    /// </summary>
    void Start()
    {
        factory = FindObjectOfType<ItemFactory>();
        GetClubType();
        SetItem(className, NameRandomizer.instance.GetPart1() + clubOptionsTypes[typeIndex] + NameRandomizer.instance.GetPart2(),
                clubOptionsSprites[typeIndex], animationParameter, idleAnimParameter, "Club", clubOptionsTypes[typeIndex],
                factory.GetStatusEffect(), 0, factory.GetModPwr(atkMin, atkMax), factory.GetModPwr(defMin, defMax),
                factory.GetModPwr(spdMin, spdMax), factory.GetModPwr(hpMin, hpMax), factory.GetModPwr(manaMin, manaMax));
    }

    /// <summary>
    /// Gets the type of the club.
    /// </summary>
    private void GetClubType()
    {
        typeIndex = Random.Range(0, clubOptionsTypes.Length);
    }
}
