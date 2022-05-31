using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Samling av grupper (prefabs) som spawner kan skapa
    public GameObject[] grupper;

    void Start()
    {
        SpawnNext();   
    }

    void Update()
    {
        
    }

    public void SpawnNext()
    {
        // Ta en slumpmassig grupp i spawner listan
        var i = Random.Range(0, grupper.Length);

        // Rita upp den på skärmen
        Instantiate(grupper[i], transform.position, Quaternion.identity);
    }
}
