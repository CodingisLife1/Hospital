using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnQuest : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] quests;
    [SerializeField] private Transform questsParent;
    private List<GameObject> existQuests = new List<GameObject>();
    private bool[] emptySpawnPoints;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (existQuests.Count > 0)
        {
            foreach (var item in existQuests)
            {
                Destroy(item);
            }
            existQuests.Clear();
        }
        

        
        emptySpawnPoints = new bool[spawnPoints.Length];
        for (int i = 0; i < 7; i++)
        {
            int r = Random.Range(0, spawnPoints.Length);
            while (emptySpawnPoints[r] != false)
            {
                r = Random.Range(0, spawnPoints.Length);
            }
            
            GameObject q = Instantiate(quests[Random.Range(0, quests.Length)], spawnPoints[r].position, Quaternion.identity, questsParent);
            existQuests.Add(q); 
            emptySpawnPoints[r] = true;
        }
    }
}
