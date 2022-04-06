using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform FollowTarget;

    public Vector2 xGroundRange;
    public Vector2 zGroundRange;

    // Start is called before the first frame update
    private void Start()
    {
    }

    private void OnValidate()
    {
        if (this.GetComponent<Camera>() == null)
        {
            Debug.LogError("CameraFollow ������������");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (FollowTarget == null)
        {
            return;
        }

        //��ȡ��ǰ��Ļ���
        float width = Screen.width;
        float height = Screen.height;
        //�������Ͷ������ ,�ܹ������ķ�Χ
        float rotateXCam = this.transform.rotation.eulerAngles.x;
        float sin = Mathf.Sin(Mathf.PI / 180 * rotateXCam);
        float halfViewZ = this.GetComponent<Camera>().orthographicSize / sin;

        float halfViewX = (width / height) * this.GetComponent<Camera>().orthographicSize;
        //�жϵ�ǰ��ɫ ���ڿ���x��Χ��λ��
        float compareXmin = Mathf.Abs(FollowTarget.position.x - xGroundRange.x);
        float compareXmax = Mathf.Abs(FollowTarget.position.x - xGroundRange.y);

        float compareZmin = Mathf.Abs(FollowTarget.position.z - zGroundRange.x);
        float compareZmax = Mathf.Abs(FollowTarget.position.z - zGroundRange.y);

        float xTransform;
        float zTransform;
        if (compareXmin > halfViewX && compareXmax > halfViewX)//������Ļһ����ӿ�ȣ�����
        {
            xTransform = FollowTarget.position.x;
        }
        else//����Ҫ����
        {
            xTransform = this.transform.position.x;
        }

        if (compareZmin > halfViewZ && compareZmax > halfViewZ)
        {
            //������Ļ���ӵģ����棬Zmin�±߽磬Zmax�ϱ߽�
            float tan = Mathf.Tan(Mathf.PI / 180 * rotateXCam);
            float offset = this.transform.position.y / tan;
            //ʹ���ﴦ�����м�
            zTransform = FollowTarget.position.z - offset;
        }
        else// ����Ҫ����
        {
            zTransform = this.transform.position.z;
        }

        this.transform.position = new Vector3(xTransform, this.transform.position.y, zTransform);
    }
}