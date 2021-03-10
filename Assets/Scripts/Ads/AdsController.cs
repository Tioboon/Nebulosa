using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsController : MonoBehaviour
{
    private static AdsController adsInstance;
    
    public string gameId = "3974431";
    public bool testMode = true;
    private P_Vars _pVars;
    public int timesDeadToSeeAd = 4;
    private Ads_Inter _adsInter;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (adsInstance == null)
        {
            adsInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start () {
        Advertisement.Initialize (gameId, testMode);
        _pVars = GameObject.Find("GameInfo").GetComponent<P_Vars>();
        _adsInter = _pVars.GetComponent<Ads_Inter>();
    }

    private void Update()
    {
        //if (_pVars.timesDeadToSeeNewAd <= 0)
        //{
            //_adsInter.ShowInterstitialAd();
            //PlayerPrefs.SetInt("Ads", timesDeadToSeeAd);
            //_pVars.timesDeadToSeeNewAd = timesDeadToSeeAd;
        //}
    }
}
