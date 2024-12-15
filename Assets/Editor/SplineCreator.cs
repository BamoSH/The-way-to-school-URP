using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

public class SplineCreator : EditorWindow
{
    [MenuItem("Window/Custom Spline Editor")]
    public static void ShowWindow()
    {
        GetWindow<SplineCreator>("Spline Editor");
    }

    private GameObject splineParent;

    private void OnGUI()
    {
        GUILayout.Label("Spline Editor", EditorStyles.boldLabel);

        splineParent = (GameObject)EditorGUILayout.ObjectField("Spline Parent", splineParent, typeof(GameObject), true);

        if (GUILayout.Button("Create Spline"))
        {
            CreateSpline();
        }
    }

    private void CreateSpline()
    {
        if (splineParent == null)
        {
            Debug.LogError("Spline Parent is not set.");
            return;
        }

        var container = splineParent.GetComponent<SplineContainer>();
        var spline = container.AddSpline();

        var knots = new BezierKnot[3];
        knots[0] = new BezierKnot(new float3(0, 0, 0));
        knots[1] = new BezierKnot(new float3(1, 1, 0), new float3(0,0,-2), new float3(0,0,2));
        knots[2] = new BezierKnot(new float3(2, -1, 0));
        spline.Knots = knots;
    }
}
