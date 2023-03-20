using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BoxGene : MonoBehaviour
{
    public GameObject boxPrefab;
    public float horizon_distance;
    public float vertical_distance;
    public int InstantiateCount;
 
    private void Start()
    {
        // 反復（繰り返し文）
        for (int i = 0; i < InstantiateCount; i++)
        {
            for(int j = 1; j < InstantiateCount + 1; j++)
            {
                Instantiate(boxPrefab, new Vector3(i * horizon_distance, 0f, j * vertical_distance), Quaternion.identity);
            }
        }
    }
}