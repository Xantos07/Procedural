using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    public float waterLevel = .4f;
    public float sandLevel = .6f;
    public float dirtLevel = .6f;
    public float rockLevel = .6f;
    public float snowLevel = .9f;
    [Range(0f,1f)]
    public float scale = .1f;
    public int size = 100;

    public float lacunarity;
    [Range(0f, 5f)] public float persistancy;
    public float octave = 1f;
    public float xOffset,yOffset;
    
    Cell[,] grid;

    public GameObject tileWater;
    public GameObject tileGrass;
    public GameObject tileSand;
    public GameObject tileRock;
    public GameObject tileSnow;
    private float[,] noiseMap;
    [SerializeField] private Renderer textureRenderer;
    private List<GameObject> cellObj =new List<GameObject>();

    private GameObject obj;

    public Land[] land;
    private Color[] floatColor;
    void Start()
    {
        BuildMap();
    }

    public void BuildMap()
    {
        if (cellObj.Count != 0)
        {
            for (int i = 0; i < cellObj.Count; i++)
            {
                Destroy(cellObj[i].gameObject);
            }
            cellObj.Clear();
        }
        
        noiseMap = new float[size, size];
        floatColor = new Color[size * size];
        (xOffset, yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        
        for(int y = 0; y < size; y++) 
        {
            for(int x = 0; x < size; x++) 
            {
                float amplitude = 1f;
                float frenquency= 1f;
                float noiseHeight = 0f;

                for (int i = 0; i < octave; i++)
                {
                    float noiseValue = Mathf.PerlinNoise(x * frenquency * scale + xOffset, y * frenquency* scale + yOffset) * 2-1;
                    //float noiseValue = Mathf.PerlinNoise(sampleX + xOffset, sampleY + yOffset) * 2-1;
                    noiseHeight += noiseValue * amplitude;
                    amplitude *= persistancy;
                    frenquency *= lacunarity;
                }
                
                noiseMap[x, y] = noiseHeight;

                for (int i = 0; i < land.Length; i++)
                {
                    if (noiseMap[x, y] <= land[i].valueRegion)
                    {
                        floatColor[y *size + x] = land[i].colorRegion;
                        Debug.Log(land[i].colorRegion);
                        break;   
                    }
                }
            }
        }

        /*
        for(int y = 0; y < size; y++) 
        {
            for(int x = 0; x < size; x++) 
            {
                float noiseValue = noiseMap[x, y];

                bool isLand = noiseValue > sandLevel && noiseValue < rockLevel;
                bool isWater = noiseValue < waterLevel;
                bool isSand = noiseValue > waterLevel && noiseValue < sandLevel;
                bool isRock = noiseValue > rockLevel && noiseValue < snowLevel;
                bool isSnow = noiseValue > snowLevel;

                Cell cell = new Cell(isLand, isWater, isSand,isRock, isSnow);

                if (cell.isWater){  obj = Instantiate(tileWater,new Vector3(x, 0, y), transform.rotation,transform); }
                if (cell.isLand) {  obj = Instantiate(tileGrass,new Vector3(x, 0, y), transform.rotation,transform); }
                if (cell.isSand) {  obj = Instantiate(tileSand, new Vector3(x, 0, y), transform.rotation,transform); }
                if (cell.isRock) {  obj = Instantiate(tileRock, new Vector3(x, 0, y), transform.rotation,transform); }
                if (cell.isSnow) {  obj = Instantiate(tileSnow, new Vector3(x, 0, y), transform.rotation,transform); }
                
                cellObj.Add(obj);
            }
        }*/

        //DrawNoiseMap(noiseMap);
        DrawColorMap(floatColor);
    }

    public void DrawColorMap(Color[] colorMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(colorMap);
        texture.Apply();
        
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(width/10, 1, height/10);
        textureRenderer.transform.rotation = Quaternion.Euler(0,180,0);
    }
    public void DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);
        Color[] colorMap = new Color[width * height];
        
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }
        
        texture.SetPixels(colorMap);
        texture.Apply();

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(width/10, 1, height/10);
        textureRenderer.transform.rotation = Quaternion.Euler(0,180,0);
    }

    [Serializable]
    public struct Land
    {
        public string nameRegion;
        public Color colorRegion;
        public float valueRegion;
    }
}