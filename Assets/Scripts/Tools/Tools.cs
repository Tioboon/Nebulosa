using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    public List<Vector3> ReturnWorldPos(float zPos)
    {
        Camera _camera = Camera.main;
        List<Vector3> listPos = new List<Vector3>();
        var worldMaxPoint = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, zPos));
        var worldMinPoint = _camera.ScreenToWorldPoint(new Vector3(0, 0, zPos));
        worldMaxPoint += new Vector3(0.2f, 0, 0);
        listPos.Add(worldMinPoint);
        listPos.Add(worldMaxPoint);
        return listPos;
    }
}

