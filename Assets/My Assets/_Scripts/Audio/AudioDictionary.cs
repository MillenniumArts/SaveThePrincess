using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioDictionary : MonoBehaviour{
    private Dictionary<string, string> SoundFiles;
    private Dictionary<string, string> MusicFiles;
    void Awake()
    {
        SoundFiles = new Dictionary<string, string>();

        SoundFiles.Add("Button1", "Audio_SoundEffect_Temp_Button_Select_WAV");
        SoundFiles.Add("Button2", "Audio_SoundEffect_Temp_Battle_OpenInventory_WAV");
        SoundFiles.Add("Hit1", "Audio_SoundEffect_Temp_Battle_Hit_WAV");
        SoundFiles.Add("SelectSmall", "Audio_SoundFX_Select_Small");
        SoundFiles.Add("SelectLarge", "Audio_SoundFX_Select_Large");
        SoundFiles.Add("Return", "Audio_SoundFX_Return");
        SoundFiles.Add("Inventory", "Audio_SoundFX_Backpack");
        SoundFiles.Add("AcceptPurchase", "Audio_SoundFX_Coins");
        SoundFiles.Add("OpenShop", "Audio_SoundFX_Open_Shop");
        SoundFiles.Add("OpenTavern", "Audio_SoundFX_Open_Tavern");
        SoundFiles.Add("Hit", "Audio_SoundFX_Battle_Hit");

        MusicFiles = new Dictionary<string, string>();
        
        MusicFiles.Add("ForestOverworld", "Audio_Music_Overworld_Forest_MP3");
        MusicFiles.Add("ForestBattle", "Audio_Music_Battle_Forest_MP3");
        MusicFiles.Add("Death", "Audio_Music_Death_MP3");
        MusicFiles.Add("Main_Menu", "AUDIO_BAR_VER2_WAV");
        MusicFiles.Add("Shop", "Audio_Music_Shop_MP3");
    }

    public string GetSound(string s)
    {
        string sound;
        if (SoundFiles.TryGetValue(s, out sound))
        {
            return sound;
        }
        else
        {
            Debug.Log("There is no SFX with the name " + s + " in the Dictionary(SoundFiles).");
            return null;
        }
    }

    public string GetMusic(string s)
    {
        string music;
        if (MusicFiles.TryGetValue(s, out music))
        {
            return music;
        }
        else
        {
            Debug.Log("There is no MUSIC with the name " + s + " in the Dictionary(MusicFiles).");
            return null;
        }
    }

}
