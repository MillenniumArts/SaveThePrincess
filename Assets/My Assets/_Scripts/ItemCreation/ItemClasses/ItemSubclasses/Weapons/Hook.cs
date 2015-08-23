using UnityEngine;
using System.Collections;

/// <summary>
/// Hook class.
/// </summary>
public class Hook : Weapon
{
    /// <summary>
    /// The hook type options.
    /// </summary>
    public string[] hookOptionsTypes;
    /// <summary>
    /// The hook type options sprites.
    /// </summary>
    public Sprite[] hookOptionsSprites;
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
        GetHookType();
        SetItem(className, NameRandomizer.instance.GetPart1() + this.GetWeaponType() + NameRandomizer.instance.GetPart2(),
                hookOptionsSprites[typeIndex], animationParameter, idleAnimParameter, "Hook", hookOptionsTypes[typeIndex],
                factory.GetStatusEffect(), 0, factory.GetModPwr(atkMin, atkMax), factory.GetModPwr(defMin, defMax),
                factory.GetModPwr(spdMin, spdMax), factory.GetModPwr(hpMin, hpMax), factory.GetModPwr(manaMin, manaMax));
    }

    /// <summary>
    /// Gets the type of the hook.
    /// </summary>
    private void GetHookType()
    {
        typeIndex = Random.Range(0, hookOptionsTypes.Length);
    }
}