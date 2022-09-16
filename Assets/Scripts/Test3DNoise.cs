using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3DNoise : MonoBehaviour
{
   [SerializeField] private float x;
   [SerializeField]   private float y;
   [SerializeField]  private float z;
    private void Update()
    {
        Perlin3D(x,y,z);
    }

    public static float Perlin3D(float _x, float _y, float _z) {
        float ab = Mathf.PerlinNoise(_x, _y);
        float bc = Mathf.PerlinNoise(_y, _z);
        float ac = Mathf.PerlinNoise(_x, _z);

        float ba = Mathf.PerlinNoise(_y, _x);
        float cb = Mathf.PerlinNoise(_z, _y);
        float ca = Mathf.PerlinNoise(_z, _x);

        float abc = ab + bc + ac + ba + cb + ca;
        Debug.Log("abc / 6" + abc / 6f);
        return abc / 6f;
    }
}
