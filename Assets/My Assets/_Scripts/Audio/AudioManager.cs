using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    #region Game Object Singleton
    /// <summary>
    /// This manager's sole instance (singleton).
    /// </summary>
    private static AudioManager instance = null;
    // Static read-only property that makes AudioManager accessible throughout the code.
    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }
    #endregion Game Object Singleton

    [SerializeField] private GameObject[] sfx;
    [SerializeField] private GameObject[] songs;
    private AudioSource oldSong;
    private bool isSongPlaying = false;
    public bool audioMuted = false;

    public void ToggleAudioMute()
    {
        if (this.audioMuted)
        {
            this.audioMuted = false;
        }
        else
        {
            this.audioMuted = true;
        }
    }

    public void PlaySFX(string n)
    {
        if (!audioMuted)
        {
            PlaySound(SearchAudioSources(sfx, n));
        }
    }

    public void PlayNewSong(string n)
    {
        if (!audioMuted) { 
            if (isSongPlaying)
            {
                if (n != oldSong.gameObject.name)
                {
                    StopSound(oldSong);
                    AudioSource newSource = SearchAudioSources(songs, n);
                    PlaySound(newSource);
                    oldSong = newSource;
                }
            }
            else
            {
                AudioSource newSource = SearchAudioSources(songs, n);
                PlaySound(newSource);
                oldSong = newSource;
                isSongPlaying = true;
            }
        }
    }

    private void PlaySound(AudioSource clip)
    {
        if (!audioMuted)
        {
            // Play the clip.
            clip.Play();
        }
    }

    private void StopSound(AudioSource clip)
    {
        // Play the clip.
        clip.Stop();
    }

    private AudioSource SearchAudioSources(GameObject[] a, string n)
    {
        AudioSource source = null;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i].name == n)
            {
                source = a[i].GetComponent<AudioSource>();
                return source;
            }
        }
        Debug.Log("No GameObject called " + n + " with an Audio Source in the Audio Manager's array.");
        return source;
    }
}