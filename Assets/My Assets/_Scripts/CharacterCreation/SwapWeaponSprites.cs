using UnityEngine;
using System.Collections;

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
        }
        return s;
    }

    private int RandomIndex(Sprite[] sArray)
    {
        int r = Random.Range(0, sArray.Length);
        return r;
    }
}
