using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGeneration : MonoBehaviour
{
    public bool cubeBoundries = false;
    public int size = 10;
    public int height = 20;
    public int width = 20;
    public int depth = 20;
    [Range(0f,1f)]
    public float rockValue = 0.6f;
    
    public Object block ;

    [Range(-1f,0f)] public float frequency = 1.01f;
    private Vector3[] points;

    void Start()
    {
        InstantiatePoints();
        GenerateBlock();
    }
    
    void InstantiatePoints()
    {
        points = new Vector3[height * width * depth];

        for (int z = 0, i = 0; z < depth; z++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    points[i] = new Vector3(x, y, z);
                    i++;
                }
            }
        }
    }

    private void GenerateBlock()
    {
        if (points != null)
        {
            for (int i = 0; i < points.Length; i++)
            {

                float sample = PerlinNoise.get3DPerlinNoise(points[i], frequency);

                sample = (sample + 1f) / 2f;
                Gizmos.color = new Color(sample, sample, sample, 1);
                
                 Debug.Log("value : " + sample);

                 if (sample > rockValue)
                 {
                     Instantiate(block,points[i], transform.rotation);
                 }
            }
        }
    }
}
