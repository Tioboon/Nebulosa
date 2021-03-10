using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class P_Movement : MonoBehaviour
{
    private Vector3 initMousePos;
    private Vector3 lastMousePos;
    private Vector3 vecForce;
    private Camera mainCam;
    private List<Vector3> screenBounds;
    private GameObject _player;
    private Rigidbody _playerRB;

    private bool eatingPlanet;
    private bool goToPlanet;
    private float lerpPlanet;

    private bool isMoving;
    private bool clickedOnPlayer;

    private Vector3 planetPos;
    private Vector3 initNebulaPos;

    public float timeForBounce;
    public float lerpMod;
    public float forceMod;
    public float lerpCamMod;
    public Tools _tools;

    private LineRenderer _lineRenderer;

    private bool kicked;

    private Vector3 initialCamPos;
    private Vector3 finalCamPos;
    public bool camLerping;
    private float lerpCam;

    private LightController _lightController;

    private GameObject bounce;
    private float bounceTimer;
    public float wallAnimTime;
    private bool bounceObj;

    private bool isDead;
    private int planetsEated;
    private GameObject planetBeingEated;

    private Pontuation _pontuation;
    private P_Vars _pVars;

    private bool blackHoleLeft;
    private bool blackHoleRight;
    public float varBlackHole;

    private E_Spawner _eSpawner;
    private E_Geral planetInfo;

    private bool launched;

    public bool menuScreen;
    private Transform inMenu;
    private bool reseted;

    private bool tuto;
    public float gravityMod;
    
    private Transform dyingAnim;
    private Transform normalAnim;
    private Transform eatingAnim;

    private P_Color _pColor;

    private AudioSource audioSource;
    public AudioClip dyingClip;
    private void Start()
    {
        _pColor = GetComponent<P_Color>();
        
        dyingAnim = transform.Find("Dying");
        normalAnim = transform.Find("Principal");
        eatingAnim = transform.Find("Eating");
        
        bounce = GameObject.Find("BounceObject");
        bounce.SetActive(false);

        _pontuation = GameObject.Find("Canvas").transform.Find("InGame").Find("Pontuation").GetComponent<Pontuation>();
        
        mainCam = Camera.main;
        screenBounds = _tools.ReturnWorldPos(transform.position.z);
        _playerRB = GetComponent<Rigidbody>();
        
        //Remove gravity;
        _playerRB.useGravity = false;
        _lineRenderer = transform.Find("Line").GetComponent<LineRenderer>();

        _lightController = GameObject.Find("DeadSpace").GetComponent<LightController>();

        _eSpawner = GameObject.Find("Generator").GetComponent<E_Spawner>();

        _pVars = GameObject.Find("GameInfo").GetComponent<P_Vars>();
        inMenu = GameObject.Find("Canvas").transform.Find("InMenu");

        audioSource = GetComponent<AudioSource>();
        
        _lineRenderer.SetPosition(0, transform.position);

        if (_pVars.musicMuted == 0)
        {
            audioSource.Play();
        }
    }

    void Update()
    {
        //Die if out of window
        if (transform.position.y > mainCam.transform.position.y + screenBounds[1].y * 2f)
        {
            menuScreen = true;
            Destroy(bounce);
        }
        
        if (menuScreen)
        {
            if(reseted) return;
            _pVars.timesDeadToSeeNewAd -= 1;
            PlayerPrefs.SetInt("Ads", _pVars.timesDeadToSeeNewAd);
            _playerRB.velocity = Vector3.zero;
            normalAnim.gameObject.SetActive(false);
            audioSource.clip = dyingClip;
            audioSource.loop = false;
            audioSource.volume = 0.2f;
            if (_pVars.musicMuted == 0)
            {
                audioSource.Play();
            }
            dyingAnim.gameObject.SetActive(true);
            inMenu.gameObject.SetActive(true);
            _pVars.playing = false;
            reseted = true;
        }
        //Camera
        //if(camLerping)
        //{
            //mainCam.transform.position = Vector3.Lerp(initialCamPos, finalCamPos, lerpCam); 
            //lerpCam += lerpCamMod;
        //}
        //if (lerpCam >= 1)
        //{
           // camLerping = false;
            //lerpCam = 0;
           // _lightController.CancelRangeChange();
        //}

        //LineTracing
        if (!isMoving)
        {
            var mousePos =
                mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    transform.position.z - mainCam.transform.position.z));
            _lineRenderer.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 50));
        }

        //Bouncing
        if(isMoving)
        {
            //_playerRB.velocity -= new Vector3(0, gravityMod, 0);
            //if(kicked) return;
            if (transform.position.x < screenBounds[0].x + transform.localScale.x/2)
            {
                //print("PosX: " + transform.position.x + "ScreenBound: " + screenBounds[0].x + " to " + screenBounds[1].x +
                      //"Raio: " + transform.localScale.x / 2);
                if(_playerRB.velocity.x < 0)
                {
                    bounce.transform.position = new Vector3(screenBounds[0].x + bounce.transform.localScale.x*5,
                        transform.position.y, transform.position.z);
                    bounce.transform.eulerAngles = new Vector3(0, 0, 0);
                    bounce.gameObject.SetActive(true);
                    bounceTimer = wallAnimTime;
                    Bounce();
                }
            }
            else if(transform.position.x > screenBounds[1].x - transform.localScale.x/2)
            {
                //print("PosX: " + transform.position.x + "ScreenBound: " + screenBounds[0].x + " to " + screenBounds[1].x +
                //"Raio: " + transform.localScale.x / 2);
                if(_playerRB.velocity.x > 0)
                {
                    bounce.transform.position = new Vector3(screenBounds[1].x - bounce.transform.localScale.x*5,
                        transform.position.y, transform.position.z);
                    bounce.transform.eulerAngles = new Vector3(0, 180, 0);
                    bounce.gameObject.SetActive(true);
                    bounceTimer = wallAnimTime;
                    Bounce();
                }
            }
        }

        if(bounceObj)
        {
            if (bounceTimer > 0)
            {
                bounceTimer -= Time.deltaTime;
            }
            else
            {
                bounce.gameObject.SetActive(false);
                bounceObj = false;
            }
        }
        
        //Eat Planet (Lerp Into it)
        if (eatingPlanet)
        {
            if(lerpPlanet < 1)
            {
                lerpPlanet += lerpMod;
                transform.position = Vector3.Lerp(initNebulaPos, planetPos, lerpPlanet);
            }
            else
            {
                Destroy(planetBeingEated);
                planetsEated += 1;
                _pontuation.UpdateScore(planetsEated);
                transform.position = planetPos;
                launched = false;
                eatingPlanet = false;
                eatingAnim.gameObject.SetActive(false);
                goToPlanet = false;
                lerpPlanet = 0;
            }
        }
        
        //Input
        if(!isMoving)
        {
            if(!_pVars.playing) return;
            if (Input.GetMouseButtonDown(0))
            {
                if(!isMoving)
                {
                    initMousePos =
                        mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                           transform.position.z - mainCam.transform.position.z));
                    if (initMousePos.x < transform.position.x + transform.localScale.x/2 &&
                        initMousePos.x > transform.position.x - transform.localScale.x/2)
                    {
                        clickedOnPlayer = true;
                        _lineRenderer.gameObject.SetActive(true);
                        _lineRenderer.SetPosition(0, new Vector3(initMousePos.x, initMousePos.y, 50));
                    }
                }
            }

            if (!Input.GetMouseButton(0))
            {
                _lineRenderer.gameObject.SetActive(false);
                launched = false;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if(launched) return;
                if(clickedOnPlayer)
                {
                    lastMousePos =
                        mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                            transform.position.z - mainCam.transform.position.z));
                    _lineRenderer.gameObject.SetActive(false);
                    vecForce = initMousePos - lastMousePos;
                    if(vecForce.magnitude > 0.1f)
                    {
                        _playerRB.useGravity = true;
                        _playerRB.AddForce(vecForce * forceMod);
                        isMoving = true;
                        if (!tuto)
                        {
                            tuto = true;
                        }
                    }

                    launched = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Planet"))
        {
            planetBeingEated = other.gameObject;
            planetInfo = planetBeingEated.GetComponent<E_Geral>();
            _playerRB.useGravity = false;
            planetPos = other.transform.position;
            initNebulaPos = transform.position;
            _playerRB.velocity = Vector3.zero;
            clickedOnPlayer = false;
            eatingPlanet = true;
            eatingAnim.gameObject.SetActive(true);
            isMoving = false;
            initialCamPos = mainCam.transform.position;
            finalCamPos = mainCam.transform.position + new Vector3(0, transform.position.y - mainCam.transform.position.y, 0);
            camLerping = true;
            _lineRenderer.SetPosition(0, transform.position);
            _lightController.CallRangeChange();
        }

        if (other.CompareTag("Constelation"))
        {
            menuScreen = true;
            Destroy(bounce);
        }

        if (other.CompareTag("Egg"))
        {
            _pColor.ChangeDegrade();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("BlackHole"))
        {
            if (other.transform.position.x < transform.position.x)
            {
                blackHoleLeft = true;
            }
            else
            {
                blackHoleRight = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BlackHole"))
        {
            blackHoleLeft = false;
            blackHoleRight = false;
        }

        if (other.name == "DeadSpace")
        {
            menuScreen = true;
            Destroy(bounce);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BlackHole"))
        {
            if (!isMoving) return;
            var vecDistance = other.transform.position - transform.position;
            var angle = Vector3.Angle(_playerRB.velocity, vecDistance);
            if (other.transform.position.x < transform.position.x)
            {
                blackHoleLeft = true;
            }
            else
            {
                blackHoleRight = true;
            }
            //BlackHole
            if (blackHoleLeft)
            {
                _playerRB.velocity += new Vector3(-varBlackHole, -varBlackHole/10, 0);
            }

            if (blackHoleRight)
            {
                _playerRB.velocity += new Vector3(varBlackHole, -varBlackHole/10, 0);
            }
        }
    }

    private IEnumerator ResetBounce()
    {
        yield return new WaitForSeconds(timeForBounce);
        kicked = false;
    }

    private void Bounce()
    {
        _playerRB.velocity = new Vector3(-_playerRB.velocity.x, _playerRB.velocity.y, _playerRB.velocity.z);
        //kicked = true;
        bounceObj = true;
        //StartCoroutine("ResetBounce");
    }
}
