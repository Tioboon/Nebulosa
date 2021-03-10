using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightController : MonoBehaviour
{
    public List<float> topRangeBound;
    public List<float> botRangeBound;
    public List<Color> topListColors;
    public List<Color> botListColors;

    public float rangeTimeCall;
    public float rangeMod;

    public float colorTimeCall;
    public float lerpColorMod;
        
    private Light botLight;
    private Light botSun;
    private Light topLight;
    private Light topSun;
    
    private Color actualColorTop;
    private Color actualColorBot;
    private bool changeColor;
    private int nextColorInt;
    private int actualColorInt;
    private float lerpColor;
    private bool stopColorChange;
    
    private float rangeTop;
    private bool topRangeUp;
    private float rangeBot;
    private bool botRangeUp;
    private bool stopRangeChange;

    private void Start()
    {
        botLight = transform.Find("Bottom").GetComponent<Light>();
        botSun = transform.Find("Bottom").Find("Sun").GetComponent<Light>();
        topLight = transform.Find("Top").GetComponent<Light>();
        topSun = transform.Find("Top").Find("Sun").GetComponent<Light>();
        
        
        rangeTop = topSun.range;
        rangeBot = botSun.range;
        botRangeUp = true;

        StartCoroutine(ChangeColor());

        actualColorInt = Random.Range(0, topListColors.Count);
        if(actualColorInt == 5)
        {
            nextColorInt = 0;
        }
        else
        {
            nextColorInt = actualColorInt + 1;
        }
    }

    private IEnumerator ChangeRange()
    {
        if (topRangeUp)
        {
            rangeTop += rangeMod;
            topSun.range = rangeTop;
            if (rangeTop >= topRangeBound[1])
            {
                topRangeUp = false;
            }
        }
        else
        {
            rangeTop -= rangeMod;
            topSun.range = rangeTop;
            if (rangeTop <= topRangeBound[0])
            {
                topRangeUp = true;
            }
        }
        
        if (botRangeUp)
        {
            rangeBot += rangeMod;
            botSun.range = rangeBot;
            if (rangeBot >= botRangeBound[1])
            {
                botRangeUp = false;
            }
        }
        else
        {
            rangeBot -= rangeMod;
            botSun.range = rangeBot;
            if (rangeBot <= botRangeBound[0])
            {
                botRangeUp = true;
            }
        }

        if (!stopRangeChange)
        {
            yield return new WaitForSeconds(rangeTimeCall);
            StartCoroutine(ChangeRange());
        }
    }

    private IEnumerator ChangeColor()
    {
        if (changeColor)
        {
            var ran = Random.Range(0, 1);
            actualColorInt = nextColorInt;
            if (ran == 0)
            {
                nextColorInt =  actualColorInt - 1;
            }
            else
            {
                nextColorInt = actualColorInt - 1;
            }

            if (nextColorInt > 5)
            {
                nextColorInt = 0;
            }

            if (nextColorInt < 0)
            {
                nextColorInt = 5;
            }

            lerpColor = 0;
            changeColor = false;
        }
        else
        {
            if (lerpColor < 1)
            {
                lerpColor += lerpColorMod;
                var botColorInitVec = new Vector3(botListColors[actualColorInt].r, botListColors[actualColorInt].g,
                    botListColors[actualColorInt].b);
                var botColorFinalVec = new Vector3(botListColors[nextColorInt].r, botListColors[nextColorInt].g,
                    botListColors[nextColorInt].b);
                var botLightColor = Vector3.Lerp(botColorInitVec, botColorFinalVec, lerpColor);
                var botActualColor = new Color(botLightColor.x, botLightColor.y, botLightColor.z);
                botLight.color = botActualColor;
                botSun.color = botActualColor;

                var topColorInitVec = new Vector3(topListColors[actualColorInt].r, topListColors[actualColorInt].g,
                    topListColors[actualColorInt].b);
                var topColorFinalVec = new Vector3(topListColors[nextColorInt].r, topListColors[nextColorInt].g,
                    topListColors[nextColorInt].b);
                var topLightColor = Vector3.Lerp(topColorInitVec, topColorFinalVec, lerpColor);
                var topActualColor = new Color(topLightColor.x, topLightColor.y, topLightColor.z);
                topLight.color = topActualColor;
                topSun.color = topActualColor;
            }
            else
            {
                changeColor = true;
            }
        }

        if (!stopColorChange)
        {
            yield return new WaitForSeconds(colorTimeCall);
            StartCoroutine(ChangeColor());
        }
    }

    public void CallRangeChange()
    {
        stopRangeChange = false;
        StartCoroutine(ChangeRange());
    }

    public void CancelRangeChange()
    {
        stopRangeChange = true;
    }
}
