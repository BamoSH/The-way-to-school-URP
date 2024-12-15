using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCircleInput : MonoBehaviour
{
    public int minIndex = 0;
    public int tempIndex;
    public bool ascend = true;
    public bool twoWay = false;
    public bool threeWay = false;

    public int maxIndex;
    // Start is called before the first frame update
    void Start()
    {
        tempIndex = minIndex;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CircleInput();
            print(tempIndex);
        }
    }

    private void CircleInput()
    {
        if (twoWay)
        {
            switch (tempIndex)
            {
                case 0:
                    tempIndex = 1;
                    print("Right!");
                    break;
                case 1:
                    tempIndex = 0;
                    print("Left!");
                    break;
            }
        }
        else if (threeWay)
        {
            switch ((tempIndex,ascend))
            {
                case (0,true):
                    tempIndex++;
                    print("Mid!");
                    break;
                case (1,true):
                    tempIndex++;
                    ascend = false;
                    print("Right!");
                    break;
                case (2,false):
                    tempIndex--;
                    print("Mid!");
                    break;
                case (1,false):
                    tempIndex--;
                    print("Left");
                    ascend = true;
                    break;
            }
        }

        // if (ascend)
        // {
        //     if (tempIndex < maxIndex)
        //         tempIndex++;
        //     else
        //     {
        //         tempIndex--;
        //         ascend = false;
        //     }
        // }else
        // {
        //     if (tempIndex>0)
        //     {
        //         tempIndex--;
        //     }
        //     else
        //     {
        //         tempIndex++;
        //         ascend = true;
        //     }
        // }
    }
}
