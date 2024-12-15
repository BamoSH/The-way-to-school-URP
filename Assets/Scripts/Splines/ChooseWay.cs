using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class ChooseWay : MonoBehaviour
{
    public SplineContainer splineContainer;
    public int currentSplineIndex = 0;      
    private float t = 0f;                   

    private SplineAnimate splineAnimate;
    
    public void ChoosePath(int splineIndex)
    {
        if (splineIndex < splineContainer.Splines.Count)
        {
            currentSplineIndex = splineIndex;
            t = 0f; 
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (t < 1.0f)
        {
            Spline currentSpline = splineContainer.Splines[currentSplineIndex];
            Vector3 position = currentSpline.EvaluatePosition(t);
            transform.position = position;
            t += Time.deltaTime;

            if (t > 0.5f && t < 0.6f) 
            {
                t = 0.55f; 
                ChoosePath(1); 
            }
        }
    }
}
