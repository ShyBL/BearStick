using System.Collections;
using UnityEngine;

public class SpawnBackgroundNPC : MonoBehaviour
{
    [SerializeField] int i_MaxBackgroundNPC, i_CurrentNumBackgroundNPC;
    [SerializeField] GameObject backgroundNPC;
    float spawnDelay = 5f;
    private bool bIsSpawning = false;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (i_CurrentNumBackgroundNPC < i_MaxBackgroundNPC && !bIsSpawning)
        {
            
            StartCoroutine(SpawnDelay());
        }
        else if (i_CurrentNumBackgroundNPC >= i_MaxBackgroundNPC)
        {
            StopCoroutine(SpawnDelay());
        }
    }

    void SpawnNPC()
    {
        GameObject newNPC = Instantiate(backgroundNPC);
        i_CurrentNumBackgroundNPC++;
    }

    IEnumerator SpawnDelay()
    {
        bIsSpawning = true;

        yield return new WaitForSeconds(spawnDelay);

        SpawnNPC();

        bIsSpawning = false;
    }
}
