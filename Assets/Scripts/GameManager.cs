using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [Header("SpawnRoad")]
    [SerializeField] private Transform player;
    [SerializeField] private Road[] roadPrefabs;
    [SerializeField] private Road firstRoad;
    [Header("SpawnAsteroid")]
    [SerializeField] private GameObject asteroid;
    [SerializeField] private int asteroidCount = 1;
    [SerializeField] private float createRate = 10f;

    
    private Road newRoad;

    private List<Road> spawnedRoads = new List<Road>();

    private void Start()
    {
        spawnedRoads.Add(firstRoad);
        StartCoroutine(RaiseAsteroidCount(createRate));
    }

    private void Update()
    {
        if(player.position.z > spawnedRoads[spawnedRoads.Count -1].end.position.z -170f)
        {
            SpawnedRoad();
        }
    }

    private void SpawnedRoad()
    {
        newRoad = Instantiate(roadPrefabs[Random.Range(0, roadPrefabs.Length)]);
        newRoad.transform.position = spawnedRoads[spawnedRoads.Count - 1].end.position - newRoad.begin.localPosition;
        SpawnedAsteroid();
        spawnedRoads.Add(newRoad);
        if (spawnedRoads.Count < 4) return;
        Destroy(spawnedRoads[0].gameObject);
        spawnedRoads.RemoveAt(0);
    }
    private void SpawnedAsteroid()
    {
        //create asteroids on a new section of the road, depending on the complexity of the game
        for (var i = 0; i < asteroidCount;i++)
        {
            var getPositionIndex = Random.Range(0, newRoad.createPositions.Count - 1);
            Instantiate(asteroid, newRoad.createPositions[getPositionIndex].position,Quaternion.identity);
            newRoad.createPositions.RemoveAt(getPositionIndex);
        }
    }
    private IEnumerator RaiseAsteroidCount(float rate)
    {
        //increasing the number of asteroids on the road
        yield return new WaitForSeconds(1f);
        while(asteroidCount < newRoad.createPositions.Count -1)
        {
            yield return new WaitForSeconds(rate);
            asteroidCount++;
        }
    }
   
}
