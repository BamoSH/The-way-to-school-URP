using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParachuteLines : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public GameObject pointA;
    public GameObject pointB;
    public GameObject pointC;

    private LineRenderer lineRendererAB; 
    private LineRenderer lineRendererBC; 

    public float lineWidthAB = 0.1f; 
    public float lineWidthBC = 0.1f; 

    void Start()
    {
        lineRendererAB = CreateLineRenderer(lineWidthAB, "Line_AB");
        
        lineRendererBC = CreateLineRenderer(lineWidthBC, "Line_BC");
    }

    void Update()
    {
        if (pointA != null && pointB != null && pointC != null)
        {
            lineRendererAB.SetPosition(0, pointA.transform.position);
            lineRendererAB.SetPosition(1, pointB.transform.position);

            lineRendererBC.SetPosition(0, pointB.transform.position);
            lineRendererBC.SetPosition(1, pointC.transform.position);
        }
    }

    private LineRenderer CreateLineRenderer(float lineWidth, string name)
    {
        GameObject lineObj = new GameObject(name);
        lineObj.transform.SetParent(transform); 
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.black; 
        lr.endColor = Color.black;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.positionCount = 2; 
        return lr;
    }
}
