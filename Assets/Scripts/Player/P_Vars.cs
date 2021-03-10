using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Vars : MonoBehaviour
{
    private static P_Vars _pVars;
    
    public bool playing;
    public int degradeInt;
    public int timesDeadToSeeNewAd;
    public int initialized;
    public int musicMuted;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (_pVars == null)
        {
            _pVars = this;
            initialized = PlayerPrefs.GetInt("Initialized");
            if(initialized == 1)
            {
                degradeInt = PlayerPrefs.GetInt("Degrade");
                timesDeadToSeeNewAd = PlayerPrefs.GetInt("Ads");
                musicMuted = PlayerPrefs.GetInt("Music");
            }
            else
            {
                PlayerPrefs.SetInt("Degrade", 0);
                PlayerPrefs.SetInt("Ads", 5);
                PlayerPrefs.SetInt("Initialized", 1);
                PlayerPrefs.SetInt("Music", 0);
                degradeInt = PlayerPrefs.GetInt("Degrade");
                timesDeadToSeeNewAd = PlayerPrefs.GetInt("Ads");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeSoundState()
    {
        if (musicMuted == 0)
        {
            musicMuted = 1;
            PlayerPrefs.SetInt("Music", musicMuted);
        }
        else
        {
            musicMuted = 0;
            PlayerPrefs.SetInt("Music", musicMuted);
        }
    }
}
