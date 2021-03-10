using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{

    private GameObject player;
    private float maxYReached;
    private P_Movement PMovement;
    private Tools _tools;
    private List<Vector3> screenBounds;
    private P_Vars _pVars;
    public float modY;
    
    void Start()
    {
        player = GameObject.Find("Player");
        PMovement = player.GetComponent<P_Movement>();
        _tools = GameObject.Find("GameInfo").GetComponent<Tools>();
        screenBounds = _tools.ReturnWorldPos(player.transform.position.z);
        maxYReached = player.transform.position.y;
        _pVars = _tools.GetComponent<P_Vars>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_pVars.playing)
        {
            if (player.transform.position.y > maxYReached)
            {
                maxYReached = player.transform.position.y;
            }

            transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
            if (player.transform.position.y < maxYReached - screenBounds[1].y * 3)
            {
                PMovement.menuScreen = true;
            }
        }
        else
        {
            transform.position += new Vector3(0, modY, 0);
        }
    }
}
