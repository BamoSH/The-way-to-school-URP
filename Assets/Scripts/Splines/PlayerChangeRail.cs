using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using UnityEngine.UI;

[System.Serializable]
public class SplinePathData
{
    public SliceData[] slices;
}

[System.Serializable]
public class SliceData
{
    public int splineIndex;
    public SplineRange range;
    
    //Can store more useful information
    public bool isEnabled = true;
    public float sliceLength;
    public float distanceFromStart;

    public int[] possibleSlices;

    public OptionData optionData;
}

[System.Serializable]
public class OptionData
{
    public int optionIndex;
    public bool optionAscend;
}
public class PlayerChangeRail : MonoBehaviour
{
    public AudioClip trafficClip;
    public AudioClip popDown2;
    public AudioClip popUp1;
    public AudioSource audioSource;
    public AudioSource trainSoundSource;
    public float interval = 10f;
    public Animator death;
    public Animator pass;
    public Canvas deathCanvas;
    public Canvas passCanvas;
    [SerializeField] private SplineContainer container;
    [SerializeField] private float speed = 6f;

    [SerializeField] private SplinePathData pathData;

    // All Slices Index
    [SerializeField] private int sliceDataIndex = 0;
    [SerializeField] private int optionIndex;
    [SerializeField] private int optionNum;
    [SerializeField] private bool ascend = true;

    public Sprite[] lightImage2;
    public Sprite[] lightImage3;
    public Image displayImage2;
    public Image displayImage3;
    
    public SplineDataAsset splineDataAsset;
    // private List<SplinePath> pathA;

    private SplinePath pathForAll;
    private SplinePath path;

    [SerializeField] private float progressRatio = 0f;
    private float progress;
    private float totalLength = 0f;
    private float totalLengthForAll = 0f;

    private List<SplineSlice<Spline>> slices;
    private List<SplineSlice<Spline>> slicesForAll;

    private List<SliceData> enabledSlices;

    // private Quaternion lastRotation;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayLoopWithInterval());
        pathData = splineDataAsset.PathData;
        InitPath();
        optionIndex = 0;
        optionNum = pathData.slices[0].possibleSlices.Length;
        TrafficLightDisplay();
    }

    private void TrafficLightDisplay()
    {
        
        displayImage2.enabled = false;
        displayImage3.enabled = false;
        if (optionNum == 2)
        {
            displayImage2.enabled = true;
            displayImage3.enabled = false;
            displayImage2.sprite = lightImage2[optionIndex];
            // print("optionIndex: " + optionIndex);
        }
        else if (optionNum == 3)
        {
            displayImage2.enabled = false;
            displayImage3.enabled = true;
            displayImage3.sprite = lightImage3[optionIndex];
            // print("optionIndex: " + optionIndex);
        }
        else if (optionNum == 1)
        {
            displayImage2.enabled = false;
            displayImage3.enabled = false;
        }
    }

    private void Update()
    {           
        UpdateSlicesData();
        TrafficLightDisplay();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.PlayOneShot(trafficClip);
            CircleInput(optionNum);
        }
    }

    private void InitPath()
    {
        slices = new List<SplineSlice<Spline>>();
        slicesForAll = new List<SplineSlice<Spline>>();
        path = new SplinePath(CalculateSlice());
        pathForAll = new SplinePath(CalculatePath());
        StartCoroutine(FollowCoroutine());
    }

    private List<SplineSlice<Spline>> CalculatePath(SliceData tempSlice)
    {
        var localToWorldMatrix = container.transform.localToWorldMatrix;
        var spline = container.Splines[tempSlice.splineIndex];
        var slice = new SplineSlice<Spline>(spline, tempSlice.range, localToWorldMatrix);
        slices.Add(slice);
        tempSlice.distanceFromStart = totalLength;
        tempSlice.sliceLength = slice.GetLength();
        totalLength += tempSlice.sliceLength;
        
        return slices;
    }
    
    private List<SplineSlice<Spline>> CalculatePath()
    {
        // Get the Container's transform  matrix
        var localToWorldMatrix = container.transform.localToWorldMatrix;
        
        // Get all the enabled Slices using LINQ
        
        totalLengthForAll = 0f;
        foreach (var sliceData in pathData.slices)
        {
            var spline = container.Splines[sliceData.splineIndex];
            var slice = new SplineSlice<Spline>(spline, sliceData.range, localToWorldMatrix);
            // print("For each time: " + sliceData.splineIndex);
            // print("Path data slices' length: " + pathData.slices.Length);
            // print("Slice length: " + slice.GetLength());
            slicesForAll.Add(slice);
            
            // Calculate the slice details
            sliceData.distanceFromStart = totalLengthForAll;
            sliceData.sliceLength = slice.GetLength();
            totalLengthForAll += sliceData.sliceLength;
        }
        return slicesForAll;
    }

    private List<SplineSlice<Spline>> CalculateSlice()
    {
        var localToWorldMatrix = container.transform.localToWorldMatrix;
        var sliceData = pathData.slices[sliceDataIndex];
        var spline = container.Splines[sliceData.splineIndex];
        var slice = new SplineSlice<Spline>(spline, sliceData.range, localToWorldMatrix);
        slices.Add(slice);
        sliceData.distanceFromStart = totalLength;
        sliceData.sliceLength = slice.GetLength();
        totalLength += sliceData.sliceLength;
        return slices;
    }
    
    private void CircleInput(int maxIndex)
    {
        if (maxIndex == 2)
        {
            displayImage3.enabled = false;
            displayImage2.enabled = true;
            displayImage2.sprite = lightImage2[optionIndex];
            switch (optionIndex)
            {
                case 0:
                    optionIndex = 1;
                    displayImage2.sprite = lightImage2[optionIndex];
                    print("Right!");
                    break;
                case 1:
                    optionIndex = 0;
                    displayImage2.sprite = lightImage2[optionIndex];
                    print("Left!");
                    break;
            }
        }
        else if (maxIndex == 3)
        {
            displayImage2.enabled = false;
            displayImage3.enabled = true;
            displayImage3.sprite = lightImage3[optionIndex];
            switch (optionIndex, ascend)
            {
                case (0,true):
                    optionIndex++;
                    displayImage3.sprite = lightImage3[optionIndex];
                    print("Mid!");
                    break;
                case (1,true):
                    optionIndex++;
                    displayImage3.sprite = lightImage3[optionIndex];
                    ascend = false;
                    print("Right!");
                    break;
                case (2,false):
                    optionIndex--;
                    displayImage3.sprite = lightImage3[optionIndex];
                    print("Mid!");
                    break;
                case (1,false):
                    optionIndex--;
                    displayImage3.sprite = lightImage3[optionIndex];
                    print("Left");
                    ascend = true;
                    break;
            }
        }
        else
        {
            displayImage2.enabled = false;
            displayImage3.enabled = false;
            print("You only have 1 way!");
        }
    }

    private void UpdatePath()
    {
        progressRatio = 0f;
        slices.Clear();
        print("SliceDataIndex: " + sliceDataIndex);
        var temp = sliceDataIndex;
        print("temp: " + temp);
        print("optionIndex: " + optionIndex);
        print(pathData.slices[temp].possibleSlices[0]);
        sliceDataIndex = pathData.slices[temp].possibleSlices[optionIndex];

        optionIndex = pathData.slices[sliceDataIndex].optionData.optionIndex;
        ascend = pathData.slices[sliceDataIndex].optionData.optionAscend;
        print("Update SliceDataIndex: " + sliceDataIndex);
        print("UpdatePath OptionIndex: " + optionIndex);
        path = new SplinePath(CalculateSlice());
    }
    private void UpdateSlicesData()
    {
        optionNum = pathData.slices[sliceDataIndex].possibleSlices.Length;
    }

    IEnumerator FollowCoroutine()
    {
        for (var n = 0;; ++n)
        {
            while (progressRatio <= 1f)
            {
                // print("ProgressRatio less than 1f!");
                // Get the position on the path
                var pos = path.EvaluatePosition(progressRatio);
                var direction = path.EvaluateTangent(progressRatio);
                var upVector = path.EvaluateUpVector(progressRatio);
                // var ratioForAll = 1 - path.GetLength()/ totalLengthForAll;
    
                transform.position = pos;
                transform.LookAt(pos + direction, upVector);
                progressRatio += speed * Time.deltaTime / path.GetLength();
                progress = progressRatio * totalLength;

                // lastRotation = transform.rotation;
                
                yield return null;
            }
            UpdatePath();
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        print("Trigger Enter!");
        if (other.CompareTag("Failure"))
        {
            trainSoundSource.Stop();
            StartCoroutine(StartTransitionDelay());
        }
    }
    
    public void OnTriggerStay(Collider other)
    {
        // print("Trigger Stay!");
        if (other.CompareTag("Acceleration"))
        {
            print("Acceleration!");
            speed = Mathf.Lerp(speed, 10, 2 * Time.deltaTime);
        }
        else if (other.CompareTag("Deceleration"))
        {
            print("Deceleration!");
            speed = Mathf.Lerp(speed, 4, 1 * Time.deltaTime);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        print("Trigger Exit!");
    }
    
    IEnumerator StartTransitionDelay()
    {
        if (MainBGM.instance != null)
        {
            MainBGM.instance.SwitchAudioWithFade();
        }
        speed = 0;
        deathCanvas.sortingOrder = 2;
        audioSource.PlayOneShot(popDown2);
        death.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        StartCoroutine(EndTransitionDelay());

    }

    IEnumerator EndTransitionDelay()
    {
        death.SetTrigger("End");
        audioSource.PlayOneShot(popUp1);
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        deathCanvas.sortingOrder = -1;
    }
    
    private IEnumerator PlayLoopWithInterval()
    {
        while (true)  
        {
            trainSoundSource.Play(); 
            yield return new WaitForSeconds(interval);
        }
    }
}
