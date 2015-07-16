using UnityEngine;
using System.Collections;

/// <summary>
/// This class required in the scene to create new weapons.  Stat Slect Scene, Store Scene, Battle Scene.
/// </summary>
public class SwapWeaponSprites : MonoBehaviour {
    /// <summary>
    /// These arrays are populated in the inspector.
    /// </summary>
    [SerializeField]
    private Sprite[] SpearHeads, SpearGuards, HammerHeads, HammerGuards,
                     AxeHeads, AxeGuards, DaggerBlades, DaggerGuards, SwordBlades, SwordGuards;

    public Sprite GetSprite(string spriteType)
    {
        Sprite s = null;
        string _spriteType = spriteType.ToLower();
        switch (_spriteType)
        {
            case "spearhead":
                s = SpearHeads[RandomIndex(SpearHeads)];
                break;
            case "spearguard":
                s = SpearGuards[RandomIndex(SpearGuards)];
                break;
            case "hammerhead":
                s = HammerHeads[RandomIndex(HammerHeads)];
                break;
            case "hammerguard":
                s = HammerGuards[RandomIndex(HammerGuards)];
                break;
            case "axehead":
                s = AxeHeads[RandomIndex(AxeHeads)];
                break;
            case "axeguard":
                s = AxeGuards[RandomIndex(AxeGuards)];
                break;
            case "daggerblade":
                s = DaggerBlades[RandomIndex(DaggerBlades)];
                break;
            case "daggerguard":
                s = DaggerGuards[RandomIndex(DaggerGuards)];
                break;
            case "swordblade":
                s = SwordBlades[RandomIndex(SwordBlades)];
                break;
            case "swordguard":
                s = SwordGuards[RandomIndex(SwordGuards)];
                break;
            default:
                Debug.Log("No Sprite to return.");
                break;
        }
        return s;
    }

    public Sprite SearchForSprites(string _weaponPart, string partName)
    {
        Sprite s = null;
        string _spriteType = _weaponPart.ToLower();
        switch (_spriteType)
        {
            case "spearhead":
                s = SearchArray(SpearHeads, partName);
                break;
            case "spearguard":
                s = SearchArray(SpearGuards, partName);
                break;
            case "hammerhead":
                s = SearchArray(HammerHeads, partName);
                break;
            case "hammerguard":
                s = SearchArray(HammerGuards, partName);
                break;
            case "axehead":
                s = SearchArray(AxeHeads, partName);
                break;
            case "axeguard":
                s = SearchArray(AxeGuards, partName);
                break;
            case "daggerblade":
                s = SearchArray(DaggerBlades, partName);
                break;
            case "daggerguard":
                s = SearchArray(DaggerGuards, partName);
                break;
            case "swordblade":
                s = SearchArray(SwordBlades, partName);
                break;
            case "swordguard":
                s = SearchArray(SwordGuards, partName);
                break;
            default:
                Debug.Log("No Sprite to return.");
                break;
        }
        return s;
    }

    private Sprite SearchArray(Sprite[] spriteArray, string name)
    {
        for (int i = 0; i < spriteArray.Length; i++)
        {
            if (spriteArray[i].name == name)
            {
                return spriteArray[i];
            }
        }
        Debug.Log("No sprite of that name.");
        return null;
    }

    private int RandomIndex(Sprite[] sArray)
    {
        int r = Random.Range(0, sArray.Length);
        return r;
    }
}
