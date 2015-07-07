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
    private AudioSource songSource;
    public AudioSource currentlyPlayingSong;
    public AudioSource currentlyPlayingSound = null;
    private bool isSongPlaying = false;

    public float volumeFactor = 1.0f;

    public void SetAudioVolume(float vol)
    {
        Mathf.Clamp(vol,0,1);
        this.volumeFactor = vol;
    }

    public void PlaySFX(string n)
    {
        this.currentlyPlayingSound = SearchAudioSources(sfx, n);
        PlaySound(currentlyPlayingSound);
    }

    /*public void PlayNewSong(string n)
    {
        if (isSongPlaying)
        {
            if (n != oldSong.clip.name)
            {
                StopSound(oldSong);
                AudioSource newSource = SearchAudioSources(songs, n);
                this.currentlyPlayingSong = newSource;
                PlaySound(newSource);
                oldSong = newSource;
            }
        }
        else
        {
            AudioSource newSource = SearchAudioSources(songs, n);
            this.currentlyPlayingSong = newSource;
            PlaySound(newSource);
            oldSong = newSource;
            isSongPlaying = true;
        }
    }*/

    public void PlayNewSong(string n)
    {
        if (isSongPlaying)
        {
            if (n != songSource.clip.name)  // If the current song is different from the song that is being called.
            {
                StopSound(songSource);      // Stop the previous song.
                LoadPlaySongClip(n);        // And load and play the new song.
            }
            else
            {
                // Old song keeps playing.
            }
        }
        else
        {
            // Load and play the first song.
            LoadPlaySongClip(n);
            isSongPlaying = true; // A song is now playing.
        }
    }

    private void LoadPlaySongClip(string name)
    {
        AudioClip newClip = Resources.Load("_Audio/Music/" + name) as AudioClip;
        songSource.clip = newClip;
        PlaySound(songSource);
    }

    private void PlaySound(AudioSource clip)
    {
        // Play the clip.
        clip.Play();
    }

    private void StopSound(AudioSource clip)
    {
        this.currentlyPlayingSound = null;
        // Stop the clip.
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



    void Start()
    {
        this.currentlyPlayingSound = null;
        songSource = GameObject.Find("Audio_Music").GetComponent<AudioSource>();
    }
    void Update()
    {
        // set volume of clips as needed
        if (this.currentlyPlayingSong != null)
            currentlyPlayingSong.volume = this.volumeFactor;
        if (this.currentlyPlayingSound != null)
            currentlyPlayingSound.volume = this.volumeFactor;
    }
}