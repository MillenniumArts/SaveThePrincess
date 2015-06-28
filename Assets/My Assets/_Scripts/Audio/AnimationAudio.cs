using UnityEngine;
using System.Collections;

public class AnimationAudio : MonoBehaviour {
    public void PlaySondEffect(string name)
    {
        AudioManager.Instance.PlaySFX(name);
    }
}
