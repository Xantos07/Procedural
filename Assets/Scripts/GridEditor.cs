using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grid))] 
public class GridEditor : Editor
{
    public float waterLevel = .4f;
    public float sandLevel = .6f;
    public float dirtLevel = .6f;
    public float rockLevel = .6f;
    public float snowLevel = .9f;
    public float scale = .1f;
    public float size = 100;
    public float lacunarity;
    public float persistancy;
    public float octave = 1f;

    public override void OnInspectorGUI()
    {
        Grid _grid = (Grid) target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generation"))
        {
            _grid.BuildMap();
        }

        EditorGUI.BeginChangeCheck();

        waterLevel = EditorGUILayout.Slider(waterLevel, 0, 1);
        sandLevel = EditorGUILayout.Slider(sandLevel, 0, 1);
        dirtLevel = EditorGUILayout.Slider(dirtLevel, 0, 1);
        rockLevel = EditorGUILayout.Slider(rockLevel, 0, 1);
        snowLevel = EditorGUILayout.Slider(snowLevel, 0, 1);
        scale = EditorGUILayout.Slider(scale, 0, 1);
        size = EditorGUILayout.Slider(size, 0, 100);
        lacunarity = EditorGUILayout.Slider(lacunarity, 0, 100);
        persistancy = EditorGUILayout.Slider(persistancy, 0, 1f);
        octave = EditorGUILayout.Slider(octave, 0, 100);
        
        if (EditorGUI.EndChangeCheck())
        {
            _grid.waterLevel = waterLevel;
            _grid.sandLevel = sandLevel;
            _grid.dirtLevel = dirtLevel;
            _grid.rockLevel = rockLevel;
            _grid.snowLevel = snowLevel;
            _grid.scale = scale;
            _grid.size = (int)size;
            _grid.lacunarity = lacunarity;
            _grid.persistancy = persistancy;
            _grid.octave = octave;
            
            _grid.BuildMap();
        }
    }
}
