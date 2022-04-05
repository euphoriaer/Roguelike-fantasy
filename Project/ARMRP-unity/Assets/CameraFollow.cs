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
        //��ȡ��ǰ��Ļ���
        float width = Screen.width;
        float height = Screen.height;
        //�������Ͷ������ ,�ܹ������ķ�Χ
        float rotateXCam = this.transform.rotation.eulerAngles.x;
        float sin = Mathf.Sin(Mathf.PI / 180 * rotateXCam);
        float halfViewZ = this.GetComponent<Camera>().orthographicSize/ sin;
        
        float halfViewX = (width/ height)* this.GetComponent<Camera>().orthographicSize;
        //�жϵ�ǰ��ɫ ���ڿ���x��Χ��λ��
        float compareXmin = Mathf.Abs(FollowTarget.position.x - xGroundRange.x);
        float compareXmax = Mathf.Abs(FollowTarget.position.x - xGroundRange.y);

        float compareZmin = Mathf.Abs(FollowTarget.position.x - zGroundRange.x);
        float compareZmax = Mathf.Abs(FollowTarget.position.x - zGroundRange.y);

        float xTransform;
        float zTransform;
        if (compareXmin > halfViewX)//������Ļһ����ӿ�ȣ�����
        {
             xTransform = FollowTarget.position.x;
        }
        else// ���ڿ��������
        {
             xTransform = xGroundRange.x + halfViewX;
        }

        if (compareZmin > halfViewZ)//������Ļһ��߶ȣ�����
        {

            float tan= Mathf.Tan(Mathf.PI / 180 * rotateXCam);
            float offset = this.transform.position.y / tan;
            //ʹ���ﴦ�����м�
            zTransform = FollowTarget.position.z-offset; 
        }
        else// ���ڿ������²�
        {
             zTransform = zGroundRange.x + halfViewZ;
        }

        this.transform.position = new Vector3(xTransform, this.transform.position.y, zTransform);
    }

    private void OnValidate()
    {
        if (this.GetComponent<Camera>()==null)
        {
            Debug.LogError("CameraFollow ������������");

        }
     
    }

    // Update is called once per frame
    void Update()
    {
        //if (FollowTarget==null)
        //{
        //    return;
        //}
        ////��ȡ��ǰ��Ļ��ߣ���߱ȣ��������ӷ�Χ
        //float width = Screen.width;
        //float height = Screen.height;
        ////�������Ͷ������ ,�ܹ������ķ�Χ
        ////�������Ͷ������ ,�ܹ������ķ�Χ
        //float rotateXCam = this.transform.rotation.x;
        //float halfViewZ = this.GetComponent<Camera>().orthographicSize / Mathf.Sin(rotateXCam);
        //float halfViewX = (width / height) * this.GetComponent<Camera>().orthographicSize;
        //float groundy = width;
        ////float groundMinx=
        ////float groundManx=
        ////float groundMiny=
        ////float groundMany=



        ////��Ȧ����Χ�Ƚϣ����ɳ����߽�
        //float xOffset = 0;
        //float zOffset = 0;
        ////�±߽� �����ƶ���������Ļһ�룬ƫ��
        //if (Mathf.Abs(FollowTarget.position.x- xGroundRange.x)> halfViewX)
        //{
        //     xOffset = Mathf.Abs(FollowTarget.position.x - xGroundRange.x) ;
        //}


        //if (Mathf.Abs(FollowTarget.position.z - zGroundRange.x) > halfViewZ)
        //{
        //     zOffset = Mathf.Abs(FollowTarget.position.z- zGroundRange.x);
        //}

        ////�ϱ߽� ����ֹͣ�ƶ���ʣ����ӷ�Χ С����Ļ���һ�룬ֹͣƫ��
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
