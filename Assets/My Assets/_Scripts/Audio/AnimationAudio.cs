using UnityEngine;
using System.Collections;

public class AnimationAudio : MonoBehaviour {
    public void PlaySoundEffect(string name)
    {
        AudioManager.Instance.PlaySFX(name);
    }
}
