﻿using Sirenix.OdinInspector;
using Slate;
using System;
using System.Collections.Generic;
using Battle;
using UnityEngine;
using UnityEngine.Serialization;

[Name("伤害盒子")]
[Attachable(typeof(HurtTrack))]
public class HurtClip : CutsceneClip<Transform>, IDirectable
{
    [LabelText("伤害")] public int hurt;

    [FormerlySerializedAs("colliders")] [LabelText("碰撞框")]
    public List<Shape> Colliders; //todo 绘制时需要 根据当前Time 显示/隐藏

    public override float length
    {
        get { return _length; }
        set { _length = value; }
    }

    [HideInInspector]
    [SerializeField] private float _length = 1 / 30f;

    private List<BoxCollider> _colliders = new List<BoxCollider>();
    private Action<Collider> triggerAction;
    private float time;

    public override void Refresh()
    {
    }


    protected override void OnEnter()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        foreach (var collider in Colliders)
        {
            BoxCollider boxCollider = actor.AddComponent<BoxCollider>();
            boxCollider.center = new Vector3(collider.offset.x, collider.offset.y + 0.5f, collider.offset.z);
            boxCollider.size = collider.size;
            boxCollider.isTrigger = true;
            _colliders.Add(boxCollider);
        }

        //需要设置碰撞事件
        triggerAction = (Collider collider) => { Debug.Log("碰撞到了" + collider.gameObject.name); };
        actor.GetComponent<Property>().CollisionEnterAction += triggerAction;
    }

    protected override void OnExit()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        actor.GetComponent<Property>().CollisionEnterAction -= triggerAction;
        foreach (var item in _colliders)
        {
            Destroy(item);
        }
    }

#if UNITY_EDITOR //逻辑绘制


    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.black;    
    //     Gizmos.matrix = transform.localToWorldMatrix;    
    //     Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    // }
    
    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        
        Gizmos.matrix = actor.transform.localToWorldMatrix;//Gizmos 配合Collider 旋转
        foreach (var collider in Colliders)
        {
            //绘制collider 到屏幕上

            switch (collider.Type)
            {
                //根据形状不同去绘制

                case Shape.ShapeType.Box:
                    //绘制在 演员所在位置
                    var boxCenter = actor.transform.position +
                                    new Vector3(collider.offset.x-actor.transform.position.x, collider.offset.y-actor.transform.position.y, collider.offset.z-actor.transform.position.z) +
                                    new Vector3(0, collider.size.y * 0.5f, 0);
                    Gizmos.DrawWireCube(boxCenter, new Vector3(collider.size.x, collider.size.y, collider.size.z));
                    
                    break;

                //case Shape.ShapeType.Circle:
                //    float angle = collider.size.x;
                //    float radius = collider.size.y;
                //    float height = collider.size.z;
                //    var arcCenter1 = actor.transform.position + new Vector3(collider.offset.x, collider.offset.y, collider.offset.z);

                //    GizmosHelper.DrawWireArc(arcCenter1,
                //            actor.transform.forward,
                //            radius,
                //            angle,
                //            0);
                //    if (height > 0)
                //    {
                //        var arcCenter2 = arcCenter1 + new Vector3(0, height, 0);
                //        var direction = actor.transform.forward;
                //        GizmosHelper.DrawWireArc(arcCenter2,
                //            direction,
                //            radius,
                //            angle,
                //            0);

                //        if (angle < 360f)
                //        {
                //            Gizmos.DrawLine(arcCenter1, arcCenter2);
                //        }
                //        Gizmos.DrawLine(arcCenter1 + direction * radius, arcCenter2 + direction * radius);

                //        var normal = Vector3.up;
                //        var leftDir = Quaternion.AngleAxis(-angle / 2f, normal) * direction;
                //        var rightDir = Quaternion.AngleAxis(angle / 2f, normal) * direction;
                //        var currentP = arcCenter1 + leftDir * radius;
                //        var currentP1 = arcCenter2 + leftDir * radius;
                //        Gizmos.DrawLine(currentP, currentP1);

                //        var currentP2 = arcCenter1 + rightDir * radius;
                //        var currentP3 = arcCenter2 + rightDir * radius;
                //        Gizmos.DrawLine(currentP2, currentP3);
                //    }
                //    break;

                default:
                    break;
            }
        }
    }

#endif

    [System.Serializable]
    public class Shape
    {
        public enum ShapeType
        {
            Box //, Circle
        }

        
        [LabelText("碰撞盒类型")] public ShapeType Type;
        
        [LabelText("偏移")] public Vector3 offset = new Vector3();
        
        [LabelText("尺寸/角度,半径,高度")] public Vector3 size = new Vector3();
    }
}