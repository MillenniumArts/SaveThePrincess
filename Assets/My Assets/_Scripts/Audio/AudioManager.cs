using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    #region Singleton
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
    #endregion Singleton

    #region Variables
    /// <summary>
    /// GameObject that holds the Audio Sources for the Sound Effects.
    /// </summary>
    [SerializeField]
    private GameObject SFXParent;
    /// <summary>
    /// List of Audio Sources for the Sound Effects.
    /// </summary>
    private List<AudioSource> sources = new List<AudioSource>();
    /// <summary>
    /// Maximum amount of Audio Sources that can be on SFXParent.
    /// </summary>
    [SerializeField]
    private int maxAudioSourceCount = 10;
    /// <summary>
    /// The Audio Mixer that outputs the Sound Effects.
    /// Used to assign an output to newly created AudioSources.
    /// </summary>
    [SerializeField]
    private AudioMixerGroup SFXmixer;
    /// <summary>
    /// The Audio Source for the Music.
    /// </summary>
    private AudioSource musicAudioSource;
    /// <summary>
    /// Is a song playing?
    /// </summary>
    private bool isSongPlaying = false;
    /// <summary>
    /// Dictionary containing all of the file names for the Audio Clips.
    /// </summary>
    private AudioDictionary _dictionary;
    /// <summary>
    /// Volume of the audio mix.
    /// </summary>
    public float volumeFactor = 1.0f;
    #endregion Variables

    #region Public Methods
    /// <summary>
    /// Sets the volume of the audio.
    /// </summary>
    /// <param name="vol">The volume, between 0 and 1.</param>
    public void SetAudioVolume(float vol)
    {
        Mathf.Clamp01(vol);
        this.volumeFactor = vol;
    }

    /// <summary>
    /// Plays a Sound Effect.
    /// </summary>
    /// <param name="n">The Dictionary key to the file name.</param>
    public void PlaySFX(string n)
    {
        if (n != null)
        {
            AudioClip newClip = Resources.Load("_Audio/Sounds/" + _dictionary.GetSound(n)) as AudioClip;
            if (newClip != null)
            {
                AudioSource newSource = GetAvailableSource();
                newSource.clip = newClip;
                PlaySound(newSource);
            }
        }
    }

    /// <summary>
    /// Plays a new song.
    /// </summary>
    /// <param name="n">The Dictionary key to the file name.</param>
    public void PlayNewSong(string n)
    {
        if (n != null)
        {
            if (isSongPlaying)
            {
                if (_dictionary.GetMusic(n) != musicAudioSource.clip.name)  // If the current song is different from the song that is being called.
                {
                    StopSound(musicAudioSource);      // Stop the previous song.
                    LoadPlaySongClip(n);        // And load and play the new song.
                }
            }
            else
            {
                // Load and play the first song.
                LoadPlaySongClip(n);
                isSongPlaying = true; // A song is now playing.
            }
        }
    }
    #endregion Public Methods

    #region Private Methods
    /// <summary>
    /// Loads and plays an song.  The dictionary key is passed in.
    /// </summary>
    /// <param name="name">The dictionary key to the song's file name.</param>
    private void LoadPlaySongClip(string name)
    {
        Debug.Log("Load/PlaySongClip, start.  Passed in string is: " + name);
        if (name != null)
        {
            Debug.Log(name + " is found.");
            AudioClip newClip = Resources.Load("_Audio/Music/" + _dictionary.GetMusic(name)) as AudioClip;
            if (newClip != null)
            {
                Debug.Log("The name of the clip is: " + newClip.name);
                musicAudioSource.clip = newClip;
                PlaySound(musicAudioSource);
            }
            else
            {
                Debug.Log("Cannot load the audio clip from resources. Passed in name is : " + name);
            }
        }
        else
        {
            Debug.Log("No Song Name: " + name);
        }
    }

    /// <summary>
    /// Plays the Audio Source that is passed in.
    /// </summary>
    /// <param name="clip">An Audio Source.</param>
    private void PlaySound(AudioSource clip)
    {
        // Play the clip.
        clip.Play();
    }

    /// <summary>
    /// Stops the audio source that is passed in.
    /// </summary>
    /// <param name="clip">An Audio Source.</param>
    private void StopSound(AudioSource clip)
    {
        // Stop the clip.
        clip.Stop();
    }

    /// <summary>
    /// Looks for an available AudioSource. An available AudioSource is one that is not playing an AudioClip.
    /// If no available AudioSources exist, it permanently creates a new AudioSource.
    /// </summary>
    /// <returns>The free source.</returns>
    private AudioSource GetAvailableSource()
    {
        // If the list of AudioSources has not been created yet, create it.
        if (this.sources == null)
        {
            this.sources = new List<AudioSource>();
        }

        // If there are no AudioSources created, add the first AudioSource.
        if (this.sources.Count == 0)
        {
            AudioSource firstSource = SFXParent.AddComponent<AudioSource>();
            firstSource.outputAudioMixerGroup = SFXmixer;
            this.sources.Add(firstSource);
        }

        // Search for an available AudioSource.
        // If one is found, it returns it. 
        for (int i = 0; i < this.sources.Count; i++)
        {
            AudioSource source = this.sources[i];
            if (source.isPlaying == false)
            {
                return source;
            }
        }

        // If we have not reached our maximum number of AudioSources for this game object, then:
        // Add a new AudioSource component to the SFXParent object, save it in the list of sources, and return it.
        if (this.sources.Count < this.maxAudioSourceCount)
        {
            AudioSource newSource = SFXParent.AddComponent<AudioSource>();
            this.sources.Add(newSource);
            newSource.outputAudioMixerGroup = SFXmixer;
            return newSource;
        }

        // We have no available AudioSources to return.
        return null;
    }
    #endregion Private Methods

    #region Monobehaviour
    void Start()
    {
        musicAudioSource = GameObject.Find("Audio_Music").GetComponent<AudioSource>();  // Finds the music Audio Source.
        _dictionary = this.gameObject.GetComponent<AudioDictionary>();                  // Finds the Dictionary.
    }
    void Update()
    {
        // set volume of clips as needed
        if (this.musicAudioSource != null)
        {
            musicAudioSource.volume = this.volumeFactor;
        }
        if (this.sources.Count != 0)
            foreach (AudioSource a in sources)
                a.volume = this.volumeFactor;
    }
    #endregion Monobehaviour
}
