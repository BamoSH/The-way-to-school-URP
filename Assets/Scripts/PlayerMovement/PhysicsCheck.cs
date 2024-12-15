using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    public Transform groundCheck;
    public bool isGround { get; private set; }
    public float groundDistance = 0.5f;
    public LayerMask groundLayer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGround = Physics.CheckSphere(groundCheck.position,groundDistance,groundLayer);
    }
    
    // 绘制Gizmos以可视化地面监测
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;  // 设置Gizmos颜色为红色
        if (isGround)
        {
            Gizmos.color = Color.green;  // 如果在地面上，设置Gizmos颜色为绿色
        }

        // 绘制一个球体表示地面检测区域
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
