using UnityEngine;

[RequireComponent(typeof(Camera))]
[UnityEngine.DisallowMultipleComponent]
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
            Debug.LogError("CameraFollow 必须挂在相机上");
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void LateUpdate()
    {
        //使用LateUpdate 方式碰撞时相机抖动
        if (FollowTarget == null)
        {
            return;
        }

        //获取当前屏幕宽高
        float width = Screen.width;
        float height = Screen.height;
        //计算地面投射坐标 ,能够看到的范围
        float rotateXCam = this.transform.rotation.eulerAngles.x;
        float sin = Mathf.Sin(Mathf.PI / 180 * rotateXCam);
        float halfViewZ = this.GetComponent<Camera>().orthographicSize / sin;

        float halfViewX = (width / height) * this.GetComponent<Camera>().orthographicSize;
        //判断当前角色 处于可视x范围的位置
        float compareXmin = Mathf.Abs(FollowTarget.position.x - xGroundRange.x);
        float compareXmax = Mathf.Abs(FollowTarget.position.x - xGroundRange.y);

        float compareZmin = Mathf.Abs(FollowTarget.position.z - zGroundRange.x);
        float compareZmax = Mathf.Abs(FollowTarget.position.z - zGroundRange.y);

        float xTransform;
        float zTransform;
        if (compareXmin > halfViewX && compareXmax > halfViewX)//大于屏幕一半可视宽度，跟随
        {
            xTransform = FollowTarget.position.x;
        }
        else//不需要跟随
        {
            xTransform = this.transform.position.x;
        }

        if (compareZmin > halfViewZ && compareZmax > halfViewZ)
        {
            //大于屏幕可视的，跟随，Zmin下边界，Zmax上边界
            float tan = Mathf.Tan(Mathf.PI / 180 * rotateXCam);
            float offset = this.transform.position.y / tan;
            //使人物处于最中间
            zTransform = FollowTarget.position.z - offset;
        }
        else// 不需要跟随
        {
            zTransform = this.transform.position.z;
        }

        this.transform.position = new Vector3(xTransform, this.transform.position.y, zTransform);
    }

    private void FixedUpdate()
    {
    }
}