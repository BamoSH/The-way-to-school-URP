using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

// [System.Serializable]
// public class SplinePathData
// {
//     public SliceData[] slices;
// }
//
// [System.Serializable]
// public class SliceData
// {
//     public int splineIndex;
//     public SplineRange range;
//     
//     //Can store more useful information
//     public bool isEnabled = true;
//     public float sliceLength;
//     public float distanceFromStart;
// }
public class PlayerFollow : MonoBehaviour
{
    [SerializeField] private SplineContainer container;
    [SerializeField] private float speed = 0.04f;

    [SerializeField] private SplinePathData pathData;

    private SplinePath path;

    [SerializeField] private float progressRatio;
    private float progress;
    private float totalLength;

    void Start()
    {
        path = new SplinePath(CalculatePath());
        
        // // Create a SplinePath from a subset of Splines(Slices)
        // path = new SplinePath(new[]
        // {
        //     new SplineSlice<Spline>(container.Splines[0], new SplineRange(0, 5), localToWorldMatrix),
        //     new SplineSlice<Spline>(container.Splines[1], new SplineRange(0, 4), localToWorldMatrix),
        //     new SplineSlice<Spline>(container.Splines[2], new SplineRange(4, 6), localToWorldMatrix),
        // });
        
        StartCoroutine(FollowCoroutine());
    }

    private List<SplineSlice<Spline>> CalculatePath()
    {
        // Get the Container's transform  matrix
        var localToWorldMatrix = container.transform.localToWorldMatrix;
        
        // Get all the enabled Slices using LINQ
        var enabledSlices= pathData.slices.Where(slice => slice.isEnabled).ToList();

        var slices = new List<SplineSlice<Spline>>();

        totalLength = 0f;
        foreach (var sliceData in enabledSlices)
        {
            var spline = container.Splines[sliceData.splineIndex];
            var slice = new SplineSlice<Spline>(spline, sliceData.range, localToWorldMatrix);
            slices.Add(slice);
            
            // Calculate the slice details
            sliceData.distanceFromStart = totalLength;
            sliceData.sliceLength = slice.GetLength();
            totalLength += sliceData.sliceLength;
        }

        return slices;
    }

    IEnumerator FollowCoroutine()
    {
        for (var n = 0;; ++n)
        {
            progressRatio = 0f;

            while (progressRatio <= 1f)
            {
                // Get the position on the path
                var pos = path.EvaluatePosition(progressRatio);
                var direction = path.EvaluateTangent(progressRatio);
                
                transform.position = pos;
                transform.LookAt(pos + direction);
                
                // Increment the progress ratio
                progressRatio += speed * Time.deltaTime;
                
                // Calculate the current distance travelled
                progress = progressRatio * totalLength;
                yield return null;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
