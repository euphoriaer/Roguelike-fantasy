using UnityEngine;


namespace MyEditor
{
public class DomainCameraCal : MonoBehaviour
{
    private DomainCamera targetDomainCamera;
    private Camera targetCamera;

    public Vector2 xGroundRange;
    public Vector2 zGroundRange;

    public DomainCamera TargetDomainCamera
    {
        get
        {
            if (targetDomainCamera == null)
                targetDomainCamera = GetComponent<DomainCamera>();
            return targetDomainCamera;
        }
    }

    public Camera TargetCamera
    {
        get
        {
            if (targetCamera == null)
                targetCamera = GetComponent<Camera>();
            return targetCamera;
        }
    }

    private void Update()
    {
        TargetDomainCamera.XRange = GetXRange(TargetCamera);
        TargetDomainCamera.ZRange = GetZRange(TargetCamera);
    }

    public Vector2 GetXRange(Camera targetCamera)
    {
        var radio = Screen.width / (float)Screen.height;

        var distance = Mathf.Abs(transform.position.y) / Mathf.Sin(transform.eulerAngles.x * Mathf.Deg2Rad);
        var planeForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        // 与地面的水平距离
        var forwardDistance = Vector3.Dot(transform.forward * distance, planeForward);
        // 相机视口与地面的切面的半长
        var offset = targetCamera.orthographicSize / Mathf.Cos((90 - transform.eulerAngles.x) * Mathf.Deg2Rad);

        var xv2 = new Vector2
        {
            x = xGroundRange.x + targetCamera.orthographicSize * radio,
            y = xGroundRange.y - targetCamera.orthographicSize * radio
        };
        return xv2;
    }

    public Vector2 GetZRange(Camera targetCamera)
    {
        var radio = Screen.width / (float)Screen.height;

        var distance = Mathf.Abs(transform.position.y) / Mathf.Sin(transform.eulerAngles.x * Mathf.Deg2Rad);
        var planeForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        // 与地面的水平距离
        var forwardDistance = Vector3.Dot(transform.forward * distance, planeForward);
        // 相机视口与地面的切面的半长
        var offset = targetCamera.orthographicSize / Mathf.Cos((90 - transform.eulerAngles.x) * Mathf.Deg2Rad);

        var zv2 = new Vector2
        {
            x = zGroundRange.x - forwardDistance + offset,
            y = zGroundRange.y - forwardDistance - offset
        };
        return zv2;
    }

#if UNITY_EDITOR

    [Header("屏幕分辨率")]
    public Vector2 size = new Vector2(1080, 1920);



#endif
}
}