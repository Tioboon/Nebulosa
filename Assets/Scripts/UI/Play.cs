using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Vector3 initPos;
    public float xMax;
    private Vector3 finalPos;
    private float lerpCont;
    public float lerpMod;
    private P_Vars _pVars;
    private AudioSource _audioSource;
    void Awake()
    {
        _pVars = GameObject.Find("GameInfo").GetComponent<P_Vars>();
        _rectTransform = GetComponent<RectTransform>();
        finalPos = _rectTransform.anchoredPosition;
        _rectTransform.anchoredPosition += new Vector2(xMax, 0);
        initPos = _rectTransform.anchoredPosition;
        _audioSource = GetComponent<AudioSource>();
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

    public IEnumerator RestartScene()
    {
        _pVars.playing = true;
        if(_pVars.musicMuted == 0)
        {
            _audioSource.Play();
        }
        yield return new WaitForSeconds(_audioSource.clip.length);
        SceneManager.LoadScene("GameScene");
    }

    public void RestartSceneButtuonHandler()
    {
        StartCoroutine("RestartScene");
    }
}
