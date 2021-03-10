using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class P_Color : MonoBehaviour
{
     private ParticleSystem _particleAlive;
     private ParticleSystem _particleShockOne;
     private ParticleSystem _particleShockTwo;
     private ParticleSystem _particlesEating;
     private ParticleSystem _particlesDying;
     public List<Gradient> colorlist;

     private P_Vars _pVars;

     public bool testing;
     public int degradeInt;

     private void Start()
     {
          _pVars = GameObject.Find("GameInfo").GetComponent<P_Vars>();
          _particleAlive = transform.Find("Principal").Find("Movement").GetComponent<ParticleSystem>();
          _particleShockOne = transform.Find("Principal").Find("Lightning").GetComponent<ParticleSystem>();
          _particleShockTwo = transform.Find("Principal").Find("Lightning2").GetComponent<ParticleSystem>();
          _particlesEating = transform.Find("Eating").GetComponent<ParticleSystem>();
          _particlesDying = transform.Find("Dying").GetComponent<ParticleSystem>();
          if(!testing)
          {
               SetDegradeStart(_pVars.degradeInt);
          }
          else
          {
               SetDegradeStart(degradeInt);
          }
     }

     public void ChangeDegrade()
     {
          int ranGradient = 0;
          if(!testing)
          {
               ranGradient = Random.Range(0, colorlist.Count);
          }
          else
          {
               ranGradient = degradeInt;
          }
          
          var colorOverMove = _particleAlive.colorOverLifetime;
          colorOverMove.color = colorlist[ranGradient];

          var colorOverLight = _particleShockOne.colorOverLifetime;
          colorOverLight.color = colorlist[ranGradient];

          var colorOverLightTwo = _particleShockTwo.colorOverLifetime;
          colorOverLightTwo.color = colorlist[ranGradient];

          var colorOverEating = _particlesEating.colorOverLifetime;
          colorOverEating.color = colorlist[ranGradient];

          var colorOverDying = _particlesDying.colorOverLifetime;
          colorOverDying.color = colorlist[ranGradient];
          
          PlayerPrefs.SetInt("Degrade", ranGradient);
          _pVars.degradeInt = ranGradient;
     }

     public void SetDegradeStart(int ranGradient)
     {
          var colorOverMove = _particleAlive.colorOverLifetime;
          colorOverMove.color = colorlist[ranGradient];

          var colorOverLight = _particleShockOne.colorOverLifetime;
          colorOverLight.color = colorlist[ranGradient];

          var colorOverLightTwo = _particleShockTwo.colorOverLifetime;
          colorOverLightTwo.color = colorlist[ranGradient];

          var colorOverEating = _particlesEating.colorOverLifetime;
          colorOverEating.color = colorlist[ranGradient];

          var colorOverDying = _particlesDying.colorOverLifetime;
          colorOverDying.color = colorlist[ranGradient];
     }

     private void Update()
     {
          if (Input.GetKeyDown(KeyCode.A))
          {
               if (degradeInt == colorlist.Count-1)
               {
                    degradeInt = 0;
               }
               else
               {
                    degradeInt += 1;
               }
               ChangeDegrade();
          }

          if (Input.GetKeyDown(KeyCode.S))
          {
               if (degradeInt == 0)
               {
                    degradeInt = colorlist.Count-1;
               }
               else
               {
                    degradeInt -= 1;    
               }
               ChangeDegrade();
          }
     }
}
