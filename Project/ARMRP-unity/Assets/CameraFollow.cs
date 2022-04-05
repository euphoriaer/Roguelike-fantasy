using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform FollowTarget;

    public Vector2 xGroundRange;
    public Vector2 zGroundRange;

  
    // Start is called before the first frame update
    void Start()
    {
        //获取当前屏幕宽高
        float width = Screen.width;
        float height = Screen.height;
        //计算地面投射坐标 ,能够看到的范围
        float rotateXCam = this.transform.rotation.eulerAngles.x;
        float sin = Mathf.Sin(Mathf.PI / 180 * rotateXCam);
        float halfViewZ = this.GetComponent<Camera>().orthographicSize/ sin;
        
        float halfViewX = (width/ height)* this.GetComponent<Camera>().orthographicSize;
        //判断当前角色 处于可视x范围的位置
        float compareXmin = Mathf.Abs(FollowTarget.position.x - xGroundRange.x);
        float compareXmax = Mathf.Abs(FollowTarget.position.x - xGroundRange.y);

        float compareZmin = Mathf.Abs(FollowTarget.position.x - zGroundRange.x);
        float compareZmax = Mathf.Abs(FollowTarget.position.x - zGroundRange.y);

        float xTransform;
        float zTransform;
        if (compareXmin > halfViewX)//大于屏幕一半可视宽度，跟随
        {
             xTransform = FollowTarget.position.x;
        }
        else// 置于可视最左侧
        {
             xTransform = xGroundRange.x + halfViewX;
        }

        if (compareZmin > halfViewZ)//大于屏幕一半高度，跟随
        {

            float tan= Mathf.Tan(Mathf.PI / 180 * rotateXCam);
            float offset = this.transform.position.y / tan;
            //使人物处于最中间
            zTransform = FollowTarget.position.z-offset; 
        }
        else// 置于可视最下侧
        {
             zTransform = zGroundRange.x + halfViewZ;
        }

        this.transform.position = new Vector3(xTransform, this.transform.position.y, zTransform);
    }

    private void OnValidate()
    {
        if (this.GetComponent<Camera>()==null)
        {
            Debug.LogError("CameraFollow 必须挂在相机上");

        }
     
    }

    // Update is called once per frame
    void Update()
    {
        //if (FollowTarget==null)
        //{
        //    return;
        //}
        ////获取当前屏幕宽高，宽高比，决定可视范围
        //float width = Screen.width;
        //float height = Screen.height;
        ////计算地面投射坐标 ,能够看到的范围
        ////计算地面投射坐标 ,能够看到的范围
        //float rotateXCam = this.transform.rotation.x;
        //float halfViewZ = this.GetComponent<Camera>().orthographicSize / Mathf.Sin(rotateXCam);
        //float halfViewX = (width / height) * this.GetComponent<Camera>().orthographicSize;
        //float groundy = width;
        ////float groundMinx=
        ////float groundManx=
        ////float groundMiny=
        ////float groundMany=



        ////与圈定范围比较，不可超过边界
        //float xOffset = 0;
        //float zOffset = 0;
        ////下边界 决定移动，大于屏幕一半，偏移
        //if (Mathf.Abs(FollowTarget.position.x- xGroundRange.x)> halfViewX)
        //{
        //     xOffset = Mathf.Abs(FollowTarget.position.x - xGroundRange.x) ;
        //}


        //if (Mathf.Abs(FollowTarget.position.z - zGroundRange.x) > halfViewZ)
        //{
        //     zOffset = Mathf.Abs(FollowTarget.position.z- zGroundRange.x);
        //}

        ////上边界 决定停止移动，剩余可视范围 小于屏幕宽度一半，停止偏移
        //if (Mathf.Abs(FollowTarget.position.x - xGroundRange.y) <= halfViewX)
        //{
        //    xOffset = 0;
        //}


        //if (Mathf.Abs(FollowTarget.position.z - zGroundRange.y) <= halfViewZ)
        //{
        //    zOffset =0;
        //}
        //this.transform.position += new Vector3(xOffset, 0, zOffset);
    }
}
