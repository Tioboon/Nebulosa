
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Geral : MonoBehaviour
{
    public string name;
    public int number;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "DeadSpace")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (CompareTag("Planet"))
        {
            if (other.CompareTag("Constelation") || other.CompareTag("BlackHole") || other.CompareTag("Egg"))
            {
                if (other.transform.position.x < transform.position.x + transform.localScale.x/2 &&
                    other.transform.position.x > transform.position.x - transform.localScale.x/2)
                {
                    if(other.transform.position.y < transform.position.y + transform.localScale.y/2 &&
                       other.transform.position.y > transform.position.y - transform.localScale.y/2)
                    {
                        var otherInfo = other.GetComponent<E_Geral>();
                        if (otherInfo.number > number)
                        {
                            Destroy(other.transform.parent.gameObject);
                        }
                    }
                }
            }
        }
    }
}
