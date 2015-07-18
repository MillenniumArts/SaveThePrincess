using UnityEngine;
using System.Collections;

public class WeaponCombination : MonoBehaviour {
    public SpriteRenderer[] sprites;
    public bool swapNow = false;
    private bool[] onOff;
    public bool dagger, hammer, axe, sword, spear;
    public bool oneHandGrip = true;

    void Start()
    {
        onOff = new bool[13];
    }

    void Update()
    {
        if (swapNow)
        {
            if (dagger)
            {
                IsDagger();
                //sprites[11].sortingLayerName = "WeaponHandleMiddle";
                oneHandGrip = true;
            }
            else if (hammer)
            {
                IsHammer();
                //sprites[11].sortingLayerName = "WeaponHandleMiddle_TwoHand";
                oneHandGrip = false;
            }
            else if (axe)
            {
                IsAxe();
                //sprites[11].sortingLayerName = "WeaponHandleMiddle_TwoHand";
                oneHandGrip = false;
            }
            else if (sword)
            {
                IsSword();
                ///sprites[11].sortingLayerName = "WeaponHandleMiddle";
                oneHandGrip = true;
            }
            else if (spear)
            {
                IsSpear();
                //sprites[11].sortingLayerName = "WeaponHandleMiddle_TwoHand";
                oneHandGrip = false;
            }
            else
            {
                IsOff();
            }
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].enabled = onOff[i];
            }
            swapNow = false;
        }
    }

    public void SwapNow()
    {
        swapNow = true;
    }

    public void AllOff()
    {
        dagger = axe = hammer = sword = spear = false;
    }

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
            default:
                Debug.Log("No weapon selected.  Or mispelled");
                break;
        }
        SwapNow();
    }

    public void IsDagger()
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
        oneHandGrip = true;
    }

    public void IsAxe()
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
        oneHandGrip = false;
    }

    public void IsHammer()
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
        oneHandGrip = false;
    }

    public void IsSword()
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
        oneHandGrip = true;
    }

    public void IsSpear()
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
        oneHandGrip = false;
    }

    public void IsOff()
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
    }

    /// <summary>
    /// If the return is true then the weapon is onehanded, else it is two handed.
    /// </summary>
    /// <returns>If the weapon is onehanded, true or false.</returns>
    public bool GetWeaponGrip()
    {
        return oneHandGrip;
    }

    public void SwitchHandleLayer(bool calm)
    {
        if(false)
            sprites[11].sortingLayerName = "WeaponHandleMiddle_TwoHand";
        else
            sprites[11].sortingLayerName = "WeaponHandleMiddle";
    }
}
