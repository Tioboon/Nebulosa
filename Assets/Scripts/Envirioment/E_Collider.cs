using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Collider : MonoBehaviour
{
    public E_Geral eGeral;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Constelation") || other.CompareTag("BlackHole") || other.CompareTag("Egg") || other.CompareTag("Planet"))
        {
            var infoOther = other.transform.GetComponent<E_Geral>();
            if(infoOther.name == eGeral.name)
            {
                if (infoOther.number > eGeral.number)
                {
                    //print("This " + transform.name + ": " + transform.position + " Destroied this " + other.transform.parent.GetChild(0).name + ": " + other.transform.position);
                    Destroy(other.transform.parent.gameObject);
                }
            }
        }
    }
    
}
