using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class E_Spawner : MonoBehaviour
{
    public float minSpawnDistance;
    public float maxSpawnDistance;
    public int spawnRandomRange;
    //Lista de intervalo de numeros sorteados para o spawn
    public List<int> spawnRangeForMobs;
    public int minChanceSpawn;
    public int maxChanceSpawn;
    public int spawnIfLessThan;
    public float spawnEach_Sec;
    //public float spawnVelVar;
    public float xScreenMin;
    public float xScreenMax;
    
    
    private Transform egg;
    private Transform blackHole;
    private Transform planet;
    private List<Transform> constelations = new List<Transform>();
    private Transform container;
    private GameObject camera;
    private int nMobsSpawned;
    private int forLoop;
    //public List<GameObject> enemies = new List<GameObject>();
    private int objectN;
    private bool firstTime;
    
    //For 3d space calculating
    public float playerZPos;
    public Tools _tools;
    
    //For spawning only when camera moves
    public int planetsSpawnedWhenCameraMoves;
    public float planetMinYDistance;
    
    [FormerlySerializedAs("planetMinXDistance")] public float planetMaxYDistance;
    private Vector3 lastCamPos;
    private P_Movement _pMovement;
    private List<Vector3> listBounds = new List<Vector3>();
    private float lastYCamSpawnedPlanets;
    
    public float distanceFromCamToSpawnNewWorlds;
    public float yIncreaseInAcumulo;

    private int planetsSpawned;
    private float initVelocity;

    //Collider
    public float preActiveTime;

    //respiro para posição de planetas na tela
    public float respiro;

    public float camYDeslocation;


    private void Start()
    {
        _pMovement = GameObject.Find("Player").GetComponent<P_Movement>();
        
        //Objetos
        egg = transform.Find("Egg");
        blackHole = transform.Find("BlackHole");
        planet = transform.Find("Planet");
        constelations.Add(transform.Find("Ursa"));
        constelations.Add(transform.Find("Sagitario"));
        constelations.Add(transform.Find("Pegasus"));
        container = transform.Find("SpawnContainer");
        //Camera
        camera = GameObject.Find("Camera");
        //Controle do spawn
        forLoop = 1;

        listBounds = _tools.ReturnWorldPos(playerZPos);
        xScreenMin = listBounds[0].x;
        xScreenMax = listBounds[1].x;

        initVelocity = spawnEach_Sec;
        StartCoroutine("SpawnMobs");
    }

    private void Update()
    {
        //camera.transform.position += new Vector3(0, camYDeslocation);
    }

    private IEnumerator SpawnMobs()
    {
        //if (_pMovement.camLerping)
        //{
            //yield return new WaitForSeconds(spawnEach_Sec);
            //StopAllCoroutines();
            //StartCoroutine("SpawnMobs");
            //yield break;
        //}
        //if(planetsSpawned > 4)
        //{
            //if (camera.transform.position == lastCamPos)
            //{
                //yield return new WaitForSeconds(spawnVelocity);
                //planetsSpawned = 0;
                //StopAllCoroutines();
                //StartCoroutine("SpawnMobs");
                //yield break;
            //}
        //}

        //if (!firstTime)
        //{
            //StartCoroutine("SpawnPlanets");
        //}
        //if(camera.transform.position.y > lastYCamSpawnedPlanets)
        //{
            //StartCoroutine("SpawnPlanets");
            //planetsSpawned = 0;
        //}
        //else
        //{
            //if (planetsSpawned < 4)
            //{
                //yield return new WaitForSeconds(spawnEach_Sec);
                //lastYCamSpawnedPlanets = camera.transform.position.y;
                //StopAllCoroutines();
                //StartCoroutine("SpawnMobs");
                //yield break;
            //}
        //}
        
        StartCoroutine("SpawnPlanets");
        //For para spawnar mais de um inimigo ao mesmo tempo
        for (int i = 0; i < forLoop; i++)
        {
            //Chance para spawnar em porcentagem
            int spawnMob = Random.Range(minChanceSpawn, maxChanceSpawn);
            //se o numero sorteado for maior que a porcentagem de spawn
            if (spawnMob > spawnIfLessThan/forLoop * forLoop)
            {
                //reseta o spawn
                //spawnVelocity -= spawnVelocity/spawnVelVar;
                yield return new WaitForSeconds(spawnEach_Sec);
                forLoop = 1;
                StopAllCoroutines();
                StartCoroutine("SpawnMobs");
                yield break;
            }

            //Randomiza o mob
            int mobNumb = Random.Range(0, spawnRandomRange);
            //Distancia ainda mais os mobs se spawnar mais de 1
            float spawnRangeDistance = Random.Range(minSpawnDistance*forLoop, maxSpawnDistance*forLoop);
            if (mobNumb > spawnRangeForMobs[0] && mobNumb <= spawnRangeForMobs[1])
            {
                //Posição X do objeto
                var posX = Random.Range(xScreenMin, xScreenMax);
                
                //Checa os objetos spawnados
                //foreach (var go in enemies)
                //{
                    //Se o objeto na lista for do mesmo tipo do spawnado
                    //var enemyInfo = go.GetComponent<E_Geral>();
                    //if (enemyInfo.name == "Egg")
                    //{
                        //Checa a distancia
                        //var distance = new Vector3(go.transform.position.x, go.transform.position.y, playerZPos) -
                                      // new Vector3(posX, camera.transform.position.x + spawnRangeDistance, playerZPos);
                        //Se a distancia for menor que a estipulada
                        //if (distance.magnitude < minEggDistance)
                        //{
                            //Cancela o spawn e reseta a função
                            //spawnVelocity -= spawnVelocity/spawnVelVar;
                            //yield return new WaitForSeconds(spawnVelocity);
                            //forLoop = 1;
                            //StartCoroutine("SpawnMobs");
                            //yield break;
                        //}
                    //}
                //}
                
                //Spawna o objeto
                var newEgg = Instantiate(egg, container);

                newEgg.name = egg.name;
                //Seta infos
                var newEggInfo = newEgg.Find("EggObj").GetComponent<E_Geral>();
                newEggInfo.name = egg.name;
                newEggInfo.number = objectN;
                objectN += 1;

                //Seta posição
                newEgg.transform.position = new Vector3(posX, camera.transform.position.y + spawnRangeDistance, playerZPos);
                
                
                //Seta ativo
                yield return new WaitForSeconds(preActiveTime);
                newEgg.GetChild(0).gameObject.SetActive(true);
                
                //Diminui as chances de spawnar
                forLoop += 1;
            }
            else if(mobNumb > spawnRangeForMobs[2] && mobNumb <= spawnRangeForMobs[3])
            {
                var posX = Random.Range(xScreenMin, xScreenMax);
                //foreach (var go in enemies)
                //{
                    //var enemyInfo = go.GetComponent<E_Geral>();
                    //if (enemyInfo.name == "BlackHole")
                    //{
                        //var distance = new Vector3(go.transform.position.x, go.transform.position.y, playerZPos) -
                                       //new Vector3(posX, camera.transform.position.x + spawnRangeDistance, playerZPos);
                        //if (distance.magnitude < minDistBlackHole)
                        //{
                            //spawnVelocity -= spawnVelocity/spawnVelVar;
                            //yield return new WaitForSeconds(spawnVelocity);
                            //forLoop = 1;
                            //StartCoroutine("SpawnMobs");
                            //yield break;
                        //}
                    //}
                //}
                var newBlackHole = Instantiate(blackHole, container);

                newBlackHole.name = blackHole.name;
                
                var newBlackHoleInfo = newBlackHole.Find("BlackHoleObj").GetComponent<E_Geral>();
                newBlackHoleInfo.name = blackHole.name;
                newBlackHoleInfo.number = objectN;
                objectN += 1;
                
                newBlackHole.transform.position = new Vector3(posX, camera.transform.position.y + spawnRangeDistance, playerZPos);
                yield return new WaitForSeconds(preActiveTime);
                newBlackHole.GetChild(0).gameObject.SetActive(true);
                forLoop += 1;
            }
            else if(mobNumb > spawnRangeForMobs[4] && mobNumb <= spawnRangeForMobs[5])
            {
                var posX = Random.Range(xScreenMin, xScreenMax);
                //foreach (Transform go in container)
                //{
                    //var enemyInfo = go.GetComponent<E_Geral>();
                    //if (enemyInfo.name == "Constelation")
                    //{
                        //var distance = new Vector3(go.transform.position.x, go.transform.position.y, playerZPos) -
                                       //new Vector3(posX, camera.transform.position.x + spawnRangeDistance, playerZPos);
                        //if (distance.magnitude < minsDistConstelation)
                        //{
                            //spawnVelocity -= spawnVelocity/spawnVelVar;
                            //yield return new WaitForSeconds(spawnVelocity);
                            //forLoop = 1;
                            //StartCoroutine("SpawnMobs");
                           // yield break;
                        //}
                    //}
                //}

                var ranConstelation = Random.Range(0, constelations.Count);
                var newConstelation = Instantiate(constelations[ranConstelation], container);

                newConstelation.name = "Constelation";
                
                var newConstelationInfo = newConstelation.Find("ConstelationObj").GetComponent<E_Geral>();
                newConstelationInfo.name = "Constelation";
                newConstelationInfo.number = objectN;
                objectN += 1;

                newConstelation.transform.position = new Vector3(posX, camera.transform.position.y + spawnRangeDistance, playerZPos);
                yield return new WaitForSeconds(preActiveTime);
                newConstelation.GetChild(0).gameObject.SetActive(true);
                forLoop += 1;
            }
            
            spawnEach_Sec = initVelocity;
            yield return new WaitForSeconds(spawnEach_Sec);
            StopAllCoroutines();
            StartCoroutine("SpawnMobs");
        }
    }
    
    private IEnumerator SpawnPlanets()
    {
        var acumuloY = 1;
        if (!firstTime)
        {
            for (int i = 0; i < planetsSpawnedWhenCameraMoves*5; i++)
            {
                var newPlanet = Instantiate(planet, container);
                var ranX = Random.Range(xScreenMin + respiro, xScreenMax - respiro);
                var increaseY = yIncreaseInAcumulo * acumuloY;
                var ranY = Random.Range(planetMinYDistance, planetMaxYDistance);
                newPlanet.transform.position = new Vector3(ranX, camera.transform.position.y + ranY + increaseY, playerZPos);
                newPlanet.name = planet.name;
                var planetInfo = newPlanet.Find("PlanetObj").GetComponent<E_Geral>();
                planetInfo.name = planet.name;
                planetInfo.number = objectN;
                objectN += 1;
                acumuloY += 1;
                planetsSpawned += 1;
                newPlanet.GetChild(0).gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < planetsSpawnedWhenCameraMoves; i++)
            {
                var newPlanet = Instantiate(planet, container);
                var ranX = Random.Range(xScreenMin + respiro, xScreenMax - respiro);
                var increaseY = yIncreaseInAcumulo * acumuloY;
                var ranY = Random.Range(planetMinYDistance, planetMaxYDistance);
                newPlanet.transform.position = new Vector3(ranX,
                    camera.transform.position.y + listBounds[1].y / .8f + ranY + increaseY, playerZPos);
                newPlanet.name = planet.name;
                var planetInfo = newPlanet.Find("PlanetObj").GetComponent<E_Geral>();
                planetInfo.name = planet.name;
                planetInfo.number = objectN;
                objectN += 1;
                acumuloY += 1;
                planetsSpawned += 1;
                newPlanet.GetChild(0).gameObject.SetActive(true);
            }
        }
        firstTime = true;
        yield break;
    }
}
