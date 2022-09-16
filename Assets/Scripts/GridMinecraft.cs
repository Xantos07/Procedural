using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridMinecraft : MonoBehaviour
{
    [SerializeField] private bool underground;
    [SerializeField] private MapView mapView;
    
    [Header("LandScape")]
    [SerializeField] private int width;
    [SerializeField] private int length;
    [SerializeField] private int height;
    [SerializeField] private float lacunarity;
    [Range(0f, 1f)] [SerializeField] private float persistancy;
    [SerializeField] private float octave = 1f;
    [SerializeField] private int xOffset,yOffset;
    [SerializeField] private float   scale = .1f;

    
    [Header("BlockValue")] [SerializeField] private float heightWater =3f;
    [SerializeField] private GameObject stoneBlock;
    [SerializeField] private GameObject waterBlock;
    [SerializeField] private GameObject sandBlock;
    [SerializeField] private GameObject dirtBlock;

    [SerializeField] private float waterValue;
    [SerializeField] private float sandValue;
    [SerializeField] private float dirtValue;
    [SerializeField] private float rockValue;
    
    [Header("Underground")]
    [SerializeField] private GameObject coalBlock;
    [SerializeField] private GameObject ironBlock;
    [SerializeField] private GameObject goldBlock;
    [SerializeField] private GameObject diamondBlock;
    
    [Range(0f,1f)] [SerializeField] private float coalValue;
    [Range(0f,1f)] [SerializeField] private float ironValue;
    [Range(0f,1f)] [SerializeField] private float goldValue;
    [Range(0f,1f)] [SerializeField] private float diamondValue;
    
     private int maxHeight = 0;
     private float[,] noiseMap;

     private Dictionary<Vector3,BoardBlock> BoardBlockList = new Dictionary<Vector3,BoardBlock>();
     
     [SerializeField] private List<Vector3> listRock = new List<Vector3>();

     private void Start()
    {
        SetLand();
        SetUnderground(coalValue, coalBlock, Block.coal);
        SetUnderground(ironValue, ironBlock,Block.iron);
        SetUnderground(goldValue, goldBlock,Block.gold);
        SetUnderground(diamondValue, diamondBlock,Block.diamond);

        for (int i = 0; i < listRock.Count; i++)
        {
            if (listRock[i].x == 0 || listRock[i].x == length - 1 || listRock[i].z == 0 || listRock[i].z == width - 1)
            { 
                GameObject obj=   Instantiate(stoneBlock, new Vector3(listRock[i].x,listRock[i].y,listRock[i].z), transform.rotation);
                BoardBlockList.Add(listRock[i], SetBoardBlock(Block.stone,points[i],obj));
                obj.name = "" + listRock[i];
            }
            else
            {
                GameObject obj=   Instantiate(stoneBlock, new Vector3(listRock[i].x,listRock[i].y,listRock[i].z), transform.rotation);
                BoardBlockList.Add(listRock[i], SetBoardBlock(Block.stone,points[i],obj));
                obj.SetActive(false);
                obj.name = "" + listRock[i];
            }
        }
    }

    BoardBlock SetBoardBlock(Block _enum, Vector3 _position ,GameObject _object)
    {
        BoardBlock newBoardBlock = new BoardBlock();
        newBoardBlock.enumBlock = _enum;
        newBoardBlock.positionBlock = _position;
        newBoardBlock.gameObjectBlock = _object;
        
        return newBoardBlock;
    }
    
    [ContextMenu("GenerateMap")]
    public void SetLand()
    {
        GenerateNoise(width,length,scale);
        GenerateLandScape(width,length,octave);
        mapView.DrawNoiseMap(noiseMap);
    }

    public void SetUnderground(float _value, GameObject _block, Block _enumBlock)
    {
        PerlinNoise.SetRamdomList();
        InstantiatePoints();
        GenerateUnderground(_value,_block, _enumBlock);
    }
    
    void GenerateNoise(int _width, int _length, float _scale)
    {
         noiseMap = new float[_width, _length];
        (xOffset, yOffset) = (Random.Range(-10000, 10000), Random.Range(-10000, 10000));
        for(int y = 0; y < _width; y++) 
        {
            for(int x = 0; x < _length; x++) 
            {
                float amplitude = 1f;
                float frenquency= 1f;
                float noiseHeight = 0f;
                
                for (int i = 0; i < octave; i++)
                {
                    float noiseValue = Mathf.PerlinNoise(x * frenquency * _scale + xOffset, y * frenquency* _scale + yOffset);
                
                    noiseHeight += noiseValue * amplitude;
                    amplitude *= persistancy;
                    frenquency *= lacunarity;
                }
                noiseMap[x, y] = noiseHeight;   
            }
        }
    }

    void GenerateLandScape(int _width, int _length,float _octave)
    {
        for(int y = 0; y < width; y++) 
        {
            for (int x = 0; x < length; x++)
            {
                float noiseValue = noiseMap[x, y];

                bool waterValueBool = noiseValue < waterValue;
                bool sandValueBool =  noiseValue > waterValue && noiseValue < sandValue ;
                bool dirtValueBool =  noiseValue > sandValue && noiseValue < rockValue;
                bool rockValueBool =   noiseValue > rockValue;
            
            
                if (waterValueBool)
                {
                    Instantiate(waterBlock, new Vector3(x, Mathf.Round(octave*noiseValue), y), transform.rotation,transform);
                }else if (sandValueBool)
                {
                    Instantiate(sandBlock, new Vector3(x, Mathf.Round(octave*noiseValue), y), transform.rotation,transform);
                }else if (dirtValueBool)
                {
                    Instantiate(dirtBlock, new Vector3(x, Mathf.Round(octave*noiseValue), y), transform.rotation,transform);
                }else if (rockValueBool)
                {
                    Instantiate(stoneBlock, new Vector3(x, Mathf.Round(octave*noiseValue), y), transform.rotation,transform);
                }
                
                if (Mathf.Round(octave*noiseValue) > maxHeight)
                {
                    maxHeight = (int)Mathf.Round(octave*noiseValue);
                }
            }
        }
    }

    void GenerateUnderground(float _value, GameObject _block, Block _enumBlock)
    {
        if (!underground) 
            return;
        for(int y = 0 , i = 0; y < width; y++) 
        {
            for (int x = 0; x < length; x++)
            {
                float noiseValue = noiseMap[x, y];
                
                for (int z = -height; z < height + maxHeight; z++)
                {     
                    float sample = PerlinNoise.get3DPerlinNoise(points[i], frequency);
                    sample = (sample + 1f) / 2f;


                    if (points[i].y < Mathf.Round(octave * noiseValue))
                    {
                        if (sample < _value)
                        {
                            GameObject rockInstantiate =  Instantiate(_block,  points[i], transform.rotation,transform);

                            if (!BoardBlockList.ContainsKey(new Vector3(x, z, y)))
                            {
                                BoardBlockList.Add(points[i] , SetBoardBlock(_enumBlock,points[i], rockInstantiate));
                                
                                //BoardBlockList[new Vector3(x,y,z)].gameObjectBlock.SetActive(false);
                                
                                
                                if(listRock.Contains(new Vector3(x,z,y)))
                                {
                                    listRock.Remove(new Vector3(x, z, y));
                                }   
                            }
                        }
                        else
                        {
                            if (!BoardBlockList.ContainsKey(new Vector3(x, z, y)))
                            {
                                BoardBlockList.Add(points[i] , SetBoardBlock(_enumBlock,points[i],null));
                                listRock.Add(new Vector3(x,z,y));
                            }
                        }
                    }
                    i++;
                }
            } 
        }
    }
    
    [Range(-1f,0f)] public float frequency = 1.01f;
    private Vector3[] points;
    
    void InstantiatePoints()
    {
        points = new Vector3[(height*2 + maxHeight) * width * length];

        for (int z = 0, i = 0; z < width; z++)
        {
            for (int x = 0; x < length; x++)
            {
                for (int y = -height; y < height + maxHeight; y++)
                {
                    points[i] = new Vector3(x, y, z);
                    i++;
                }
            }
        }
    }
    
    [ContextMenu("ResetAllMap")]
    public void ResetAllMap()
    {
        Transform[] allChild = GetComponentsInChildren<Transform>();
        
        foreach (Transform child in allChild)
        {
            if (child.transform.name != "Grid")
            {
                Destroy(child.gameObject);   
            }
        }
    }
}

public class BoardBlock
{
    public Block enumBlock;
    public Vector3 positionBlock;
    public GameObject gameObjectBlock;
}

public enum Block
{
    dirt = 0,
    stone = 1,
    coal = 2,
    iron = 3,
    gold = 4,
    diamond = 5,
}
