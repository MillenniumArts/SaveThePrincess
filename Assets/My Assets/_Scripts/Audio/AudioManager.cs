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
        // TODO: Finish singleton implementation.
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

    public void PlaySFX(string n)
    {
        /*for(int i = 0; i < sfx.Length; i++){
            if(sfx[i].name == n){
                AudioSource source = sfx[i].GetComponent<AudioSource>();
                PlaySound(source);
                break;
            }
        }*/
        PlaySound(SearchAudioSources(sfx, n));
    }

    public void PlayNewSong(string n)
    {
        if (isSongPlaying)
        {
            StopSound(oldSong);
            /*for (int i = 0; i < songs.Length; i++)
            {
                if (songs[i].name == n)
                {
                    AudioSource source = songs[i].GetComponent<AudioSource>();
                    PlaySound(source);
                    oldSong = source;
                    break;
                }
            }*/
            AudioSource newSource = SearchAudioSources(songs, n);
            PlaySound(newSource);
            oldSong = newSource;
        }
        else
        {
            /*for (int i = 0; i < songs.Length; i++)
            {
                if (songs[i].name == n)
                {
                    AudioSource source = songs[i].GetComponent<AudioSource>();
                    PlaySound(source);
                    oldSong = source;
                    break;
                }
            }*/
            AudioSource newSource = SearchAudioSources(songs, n);
            PlaySound(newSource);
            oldSong = newSource;
            isSongPlaying = true;
        }
    }

    private void PlaySound(AudioSource clip)
    {
        // Play the clip.
        clip.Play();
    }

    private void StopSound(AudioSource clip)
    {
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
        return source;
    }
}
