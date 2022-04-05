using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace MyEditor
{
    public class Camera : MonoBehaviour
    {
        [CustomEditor(typeof(CameraFollow))]
#if ODIN_INSPECTOR
        public class DomainCameraEditor : Sirenix.OdinInspector.Editor.OdinEditor
#else
    public class DomainCameraEditor : Editor
#endif
        {
            BoxBoundsHandle boxBoundsHandle;
            protected override void OnEnable()
            {
                base.OnEnable();
                boxBoundsHandle = new BoxBoundsHandle();
                boxBoundsHandle.axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Z;
                boxBoundsHandle.handleColor = Color.cyan;
                boxBoundsHandle.wireframeColor = Color.green;
                boxBoundsHandle.midpointHandleSizeFunction += _ => { return 1f; };
            }

            private void OnSceneGUI()
            {
                CameraFollow dCam = target as CameraFollow;

                var min = new Vector3(dCam.xGroundRange.x, 0, dCam.zGroundRange.x);
                var max = new Vector3(dCam.xGroundRange.y, 0, dCam.zGroundRange.y);

                boxBoundsHandle.center = (min + max) / 2;
                boxBoundsHandle.size = max - min;
                boxBoundsHandle.DrawHandle();
                min = boxBoundsHandle.center - boxBoundsHandle.size * 0.5f;
                max = boxBoundsHandle.center + boxBoundsHandle.size * 0.5f;
                dCam.xGroundRange = new Vector2(min.x, max.x);
                dCam.zGroundRange = new Vector2(min.z, max.z);

                Rect groundRange = new Rect();
                groundRange.xMin = dCam.xGroundRange.x;
                groundRange.xMax = dCam.xGroundRange.y;
                groundRange.yMin = dCam.zGroundRange.x;
                groundRange.yMax = dCam.zGroundRange.y;

                Matrix4x4 matrix = Handles.matrix;
                Handles.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(90, 0, 0), Vector3.one);
                Handles.DrawSolidRectangleWithOutline(groundRange, new Color(0, 1, 0, 0.3f), new Color(0, 1, 0, 1));
                Handles.matrix = matrix;
            }
        }
    }
}
