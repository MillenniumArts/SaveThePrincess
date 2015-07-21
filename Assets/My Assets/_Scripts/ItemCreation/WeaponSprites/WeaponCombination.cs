using UnityEngine;
using System.Collections;

public class WeaponCombination : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer[] spriteRenderers;
    [SerializeField]
    private bool swapNow = false;
    private bool[] onOff;
    [SerializeField]
    private bool dagger, hammer, axe, sword, spear, club, hook;
    [SerializeField]
    private bool oneHandGrip = true;
    [SerializeField]
    private string[] spriteSheetNames;
    private bool isSpawned = true;

    #region Monobehaviour
    void Start()
    {
        onOff = new bool[18];
    }

    void LateUpdate()
    {
        if (swapNow)
        {
            NewWeaponSprites child = GetComponentInChildren<NewWeaponSprites>();
            if (dagger)
            {
                IsDagger();
                if (isSpawned)
                    child.SwapSprites(spriteSheetNames[0]);
                //sprites[11].sortingLayerName = "WeaponHandleMiddle";
                oneHandGrip = true;
            }
            else if (hammer)
            {
                IsHammer();
                if (isSpawned)
                    child.SwapSprites(spriteSheetNames[1]);
                //sprites[11].sortingLayerName = "WeaponHandleMiddle_TwoHand";
                oneHandGrip = false;
            }
            else if (axe)
            {
                if (isSpawned)
                    child.SwapSprites(spriteSheetNames[2]);
                IsAxe();
                //sprites[11].sortingLayerName = "WeaponHandleMiddle_TwoHand";
                oneHandGrip = false;
            }
            else if (sword)
            {
                IsSword();
                if (isSpawned)
                    child.SwapSprites(spriteSheetNames[3]);
                ///sprites[11].sortingLayerName = "WeaponHandleMiddle";
                oneHandGrip = true;
            }
            else if (spear)
            {
                if (isSpawned)
                    child.SwapSprites(spriteSheetNames[4]);
                IsSpear();
                //sprites[11].sortingLayerName = "WeaponHandleMiddle_TwoHand";
                oneHandGrip = false;
            }
            else if (club)
            {
                if (isSpawned)
                    child.SwapSprites(spriteSheetNames[5]);
                IsClub();
                //sprites[11].sortingLayerName = "WeaponHandleMiddle_TwoHand";
                oneHandGrip = false;
            }
            else if (hook)
            {
                if (isSpawned)
                    child.SwapSprites(spriteSheetNames[6]);
                IsHook();
                //sprites[11].sortingLayerName = "WeaponHandleMiddle_TwoHand";
                oneHandGrip = false;
            }
            else
            {
                IsOff();
            }
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].enabled = onOff[i];
            }
            ClearUnusedRenderers();
            isSpawned = false;
            swapNow = false;
        }
    }
    #endregion Monobehaviour

    #region Public Methods
    /// <summary>
    /// Swaps the weapon to the weapon of the passed in name.
    /// </summary>
    /// <param name="name">The name of the weapon to swap to. "Dagger", "Axe", "Hammer", "Sword", "Spear".</param>
    public void SwapWeapon(string name)
    {
        AllOff();
        switch (name)
        {
            case "Dagger": dagger = true;
                break;
            case "Axe": axe = true;
                break;
            case "Hammer": hammer = true;
                break;
            case "Sword": sword = true;
                break;
            case "Spear": spear = true;
                break;
            case "Club": club = true;
                break;
            case "Hook": hook = true;
                break;
            default:
                Debug.Log("No weapon selected.  Or mispelled");
                break;
        }
        SwapNow();
    }

    /// <summary>
    /// If the return is true then the weapon is onehanded, else it is two handed.
    /// </summary>
    /// <returns>If the weapon is onehanded, true or false.</returns>
    public bool GetWeaponGrip()
    {
        return oneHandGrip;
    }

    /// <summary>
    /// Changes the weapon handle sorting layer depending if the pkayer is in town or not.
    /// </summary>
    /// <param name="calm">Bool stating if the player is in town.</param>
    public void SwitchHandleLayer(bool calm)
    {
        if (calm == false)
            spriteRenderers[11].sortingLayerName = "WeaponHandleMiddle_TwoHand";
        else
            spriteRenderers[11].sortingLayerName = "WeaponHandleMiddle";
    }

    /// <summary>
    /// Turns "off" all the weapon combinations for a weapon prefab.
    /// </summary>
    public void AllOff()
    {
        dagger = axe = hammer = sword = spear = club = hook = false;
    }

    /// <summary>
    /// Sets swapNow to true.  Sets the swap train in motion.
    /// </summary>
    public void SwapNow()
    {
        swapNow = true;
    }
    #endregion Public Methods

    #region Private Methods
    private void IsDagger()
    {
        onOff[0] = true;
        onOff[1] = true;
        onOff[2] = false;
        onOff[3] = false;
        onOff[4] = false;
        onOff[5] = false;
        onOff[6] = false;
        onOff[7] = false;
        onOff[8] = false;
        onOff[9] = false;
        onOff[10] = false;
        onOff[11] = true;
        onOff[12] = false;
        onOff[13] = true;
        onOff[14] = false;
        onOff[15] = false;
        onOff[16] = false;
        onOff[17] = false;
        oneHandGrip = true;
    }

    private void IsAxe()
    {
        onOff[0] = false;
        onOff[1] = false;
        onOff[2] = false;
        onOff[3] = false;
        onOff[4] = true;
        onOff[5] = true;
        onOff[6] = false;
        onOff[7] = false;
        onOff[8] = false;
        onOff[9] = false;
        onOff[10] = true;
        onOff[11] = true;
        onOff[12] = false;
        onOff[13] = true;
        onOff[14] = false;
        onOff[15] = false;
        onOff[16] = false;
        onOff[17] = false;
        oneHandGrip = false;
    }

    private void IsHammer()
    {
        onOff[0] = false;
        onOff[1] = false;
        onOff[2] = true;
        onOff[3] = true;
        onOff[4] = false;
        onOff[5] = false;
        onOff[6] = false;
        onOff[7] = false;
        onOff[8] = false;
        onOff[9] = false;
        onOff[10] = true;
        onOff[11] = true;
        onOff[12] = false;
        onOff[13] = true;
        onOff[14] = false;
        onOff[15] = false;
        onOff[16] = false;
        onOff[17] = false;
        oneHandGrip = false;
    }

    private void IsSword()
    {
        onOff[0] = false;
        onOff[1] = false;
        onOff[2] = false;
        onOff[3] = false;
        onOff[4] = false;
        onOff[5] = false;
        onOff[6] = true;
        onOff[7] = true;
        onOff[8] = false;
        onOff[9] = false;
        onOff[10] = false;
        onOff[11] = true;
        onOff[12] = false;
        onOff[13] = true;
        onOff[14] = false;
        onOff[15] = false;
        onOff[16] = false;
        onOff[17] = false;
        oneHandGrip = true;
    }

    private void IsSpear()
    {
        onOff[0] = false;
        onOff[1] = false;
        onOff[2] = false;
        onOff[3] = false;
        onOff[4] = false;
        onOff[5] = false;
        onOff[6] = false;
        onOff[7] = false;
        onOff[8] = true;
        onOff[9] = true;
        onOff[10] = true;
        onOff[11] = true;
        onOff[12] = true;
        onOff[13] = false;
        onOff[14] = false;
        onOff[15] = false;
        onOff[16] = false;
        onOff[17] = false;
        oneHandGrip = false;
    }

    private void IsClub()
    {
        onOff[0] = false;
        onOff[1] = false;
        onOff[2] = false;
        onOff[3] = false;
        onOff[4] = false;
        onOff[5] = false;
        onOff[6] = false;
        onOff[7] = false;
        onOff[8] = false;
        onOff[9] = false;
        onOff[10] = true;
        onOff[11] = true;
        onOff[12] = false;
        onOff[13] = true;
        onOff[14] = true;
        onOff[15] = true;
        onOff[16] = false;
        onOff[17] = false;
        oneHandGrip = false;
    }

    private void IsHook()
    {
        onOff[0] = false;
        onOff[1] = false;
        onOff[2] = false;
        onOff[3] = false;
        onOff[4] = false;
        onOff[5] = false;
        onOff[6] = false;
        onOff[7] = false;
        onOff[8] = false;
        onOff[9] = false;
        onOff[10] = false;
        onOff[11] = true;
        onOff[12] = false;
        onOff[13] = true;
        onOff[14] = false;
        onOff[15] = false;
        onOff[16] = true;
        onOff[17] = true;
        oneHandGrip = true;
    }

    private void IsOff()
    {
        onOff[0] = false;
        onOff[1] = false;
        onOff[2] = false;
        onOff[3] = false;
        onOff[4] = false;
        onOff[5] = false;
        onOff[6] = false;
        onOff[7] = false;
        onOff[8] = false;
        onOff[9] = false;
        onOff[10] = false;
        onOff[11] = false;
        onOff[12] = false;
        onOff[13] = false;
        onOff[14] = false;
        onOff[15] = false;
        onOff[16] = false;
        onOff[17] = false;
    }

    private void ClearUnusedRenderers()
    {
        for (int i = 0; i < onOff.Length; i++)
        {
            if (i < 10 || i > 13)
            {
                if (onOff[i] == false)
                {
                    spriteRenderers[i].sprite = null;
                }
            }
        }
    }
    #endregion Private Methods
}
