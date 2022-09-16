using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private Renderer textureRenderer;
    [SerializeField] private int size;
    [SerializeField] private float scale;

    [SerializeField] private GameObject stoneBlock;
    [SerializeField] private GameObject waterBlock;
    [SerializeField] private GameObject sandBlock;
    [SerializeField] private GameObject dirtBlock;

    [SerializeField] private float waterValue;
    [SerializeField] private float sandValue;
    [SerializeField] private float dirtValue;
    [SerializeField] private float rockValue;
    
    private void Start()
    {
        float[,] noiseMap = new float[size, size];

        for(int y = 0; y < size; y++) 
        {
            for(int x = 0; x < size; x++) 
            {
                float noiseValue = Mathf.PerlinNoise(x * scale , y * scale );
                noiseMap[x, y] = noiseValue;
            }
        }
        
        for(int y = 0; y < size; y++) 
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = noiseMap[x, y];

                bool waterValueBool = noiseValue < waterValue;
                bool sandValueBool =  noiseValue > waterValue && noiseValue < sandValue ;
                bool dirtValueBool =  noiseValue > sandValue && noiseValue < rockValue;
                bool rockValueBool =   noiseValue > rockValue;
            
            
                if (waterValueBool)
                {
                    Instantiate(waterBlock, new Vector3(x, 0, y), transform.rotation,transform);
                }else if (sandValueBool)
                {
                    Instantiate(sandBlock, new Vector3(x, 0,y), transform.rotation,transform);
                }else if (dirtValueBool)
                {
                    Instantiate(dirtBlock, new Vector3(x, 0, y), transform.rotation,transform);
                }else if (rockValueBool)
                {
                    Instantiate(stoneBlock, new Vector3(x, 0,y), transform.rotation,transform);
                }
            }
        }
        
        DrawNoiseMap(noiseMap);
    }

    public void DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);
        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }
        
        texture.SetPixels(colourMap);
        texture.Apply();

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(width/10, 1, height/10);
        textureRenderer.transform.rotation = Quaternion.Euler(0,180,0);
    }
}
