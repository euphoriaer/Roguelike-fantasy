using System;
using UnityEngine;

public static class GizmosHelper
{
    /// <summary>
    /// 绘制扇形
    /// </summary>
    /// <param name="origin">圆心点</param>
    /// <param name="direction">方向</param>
    /// <param name="radius">半径</param>
    /// <param name="angle">角度</param>
    /// <param name="ringWidth">环宽</param>
    public static void DrawWireArc(Vector3 origin, Vector3 direction, float radius, float angle, float ringWidth)
    {
        DrawWireArc(origin, direction, radius, angle, ringWidth, Vector3.up);
    }

    /// <summary>
    /// 绘制扇形
    /// </summary>
    /// <param name="origin">圆心点</param>
    /// <param name="direction">方向</param>
    /// <param name="radius">半径</param>
    /// <param name="angle">角度</param>
    /// <param name="ringWidth">环宽</param>
    /// <param name="normal">法线</param>
    private static void DrawWireArc(Vector3 origin, Vector3 direction, float radius, float angle, float ringWidth, Vector3 normal)
    {
        var leftDir = Quaternion.AngleAxis(-angle / 2f, normal) * direction;
        var rightDir = Quaternion.AngleAxis(angle / 2f, normal) * direction;

        var currentP = origin + leftDir * radius;
        Vector3 oldP;

        var currentP1 = origin + leftDir * (radius - ringWidth);
        Vector3 oldP1;
        if (Math.Abs(angle - 360) > 0.0001f)
        {
            Gizmos.DrawLine(ringWidth > 0.0001f ? currentP1 : origin, currentP);
        }
        for (var i = 0; i < angle / 10; i++)
        {
            var dir = Quaternion.AngleAxis(10 * i, normal) * leftDir;
            oldP = currentP;
            currentP = origin + dir * radius;
            Gizmos.DrawLine(oldP, currentP);
        }
        oldP = currentP;
        currentP = origin + rightDir * radius;
        Gizmos.DrawLine(oldP, currentP);

        if (ringWidth > 0.0001f)
        {
            // 内弧
            for (var i = 0; i < angle / 10; i++)
            {
                var dir = Quaternion.AngleAxis(10 * i, normal) * leftDir;
                oldP1 = currentP1;
                currentP1 = origin + dir * (radius - ringWidth);
                Gizmos.DrawLine(oldP1, currentP1);
            }
            oldP1 = currentP1;
            currentP1 = origin + rightDir * (radius - ringWidth);
            Gizmos.DrawLine(oldP1, currentP1);
        }
        if (Math.Abs(angle - 360) > 0.0001f)
        {
            Gizmos.DrawLine(ringWidth > 0.0001f ? currentP1 : origin, currentP);
        }
    }
}