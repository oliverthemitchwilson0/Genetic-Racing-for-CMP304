using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public GameObject carPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 30; i++)
        {
            Instantiate(carPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
