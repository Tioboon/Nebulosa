using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nebulosa : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Vector3 initPos;
    public float yMax;
    private Vector3 finalPos;
    private float lerpCont;
    public float lerpMod;
    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        finalPos = _rectTransform.anchoredPosition;
        _rectTransform.anchoredPosition += new Vector2(0, yMax);
        initPos = _rectTransform.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (lerpCont < 1)
        {
            lerpCont += lerpMod;
        }
        else
        {
            lerpCont = 1;
        }
        _rectTransform.anchoredPosition = Vector2.Lerp(initPos, finalPos, lerpCont);
    }
}
