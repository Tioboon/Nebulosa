using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    private P_Vars _pVars;

    private void Awake()
    {
        _pVars = GameObject.Find("GameInfo").GetComponent<P_Vars>();
    }

    void Start()
    {
        if (_pVars.playing)
        {
            transform.Find("InGame").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("InMenu").gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
