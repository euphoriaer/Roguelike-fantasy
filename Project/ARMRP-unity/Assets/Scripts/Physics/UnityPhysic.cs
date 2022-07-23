using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace UnityPhysics
{
    public class UnityPhysic
    {
        #region Actions

        public Action<Collision> CollisionEnterAction
        {
            get { return unityAction.CollisionEnterAction; }
            set { unityAction.CollisionEnterAction = value; }
        }

        public Action<Collision> CollisionStayAction
        {
            get { return unityAction.CollisionStayAction; }
            set { unityAction.CollisionStayAction = value; }
        }

        public Action<Collision> CollisionExitAction
        {
            get { return unityAction.CollisionExitAction; }
            set { unityAction.CollisionExitAction = value; }
        }

        public Action<Collider> TriggerEnterAction
        {
            get { return unityAction.TriggerEnterAction; }
            set { unityAction.TriggerEnterAction = value; }
        }

        public Action<Collider> TriggerStayAction
        {
            get { return unityAction.TriggerStayAction; }
            set { unityAction.TriggerStayAction = value; }
        }

        public Action<Collider> TriggerExitAction
        {
            get { return unityAction.TriggerExitAction; }
            set { unityAction.TriggerExitAction = value; }
        }

        #endregion Actions

        public GameObject PhysicObj
        {
            get { return _physicObj; }
        }

        private GameObject _physicObj;

        private UnityPhysicAction unityAction;

        /// <summary>
        /// Trigger Use TriggerAction,other use CollisionAction
        /// </summary>
        /// <param name="isTrigger"></param>
        /// <param name="shapes"></param>
        /// <param name="position"></param>
        public UnityPhysic(bool isTrigger, Shape[] shapes, Vector3 position)
        {
            //todo shapeType
            _physicObj = new GameObject("Physic");
            _physicObj.transform.position = position;
            unityAction = _physicObj.AddComponent<UnityPhysicAction>();

            for (int i = 0; i < shapes.Length; i++)
            {
                var shape = shapes[i];
                BoxCollider boxCollider = _physicObj.AddComponent<BoxCollider>();
                boxCollider.center = new Vector3(shape.offset.x, shape.offset.y +shape.size.y/2, shape.offset.z);
                boxCollider.size = shape.size;
                boxCollider.isTrigger = isTrigger;
            }
        }

        public void Destory()
        {
            GameObject.Destroy(_physicObj);
        }

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

        private class UnityPhysicAction : MonoBehaviour
        {
            public Action<Collision> CollisionEnterAction;
            public Action<Collision> CollisionStayAction;
            public Action<Collision> CollisionExitAction;

            public Action<Collider> TriggerEnterAction;
            public Action<Collider> TriggerStayAction;
            public Action<Collider> TriggerExitAction;

            private void OnCollisionEnter(Collision collision)
            {
                if (CollisionEnterAction == null)
                {
                    return;
                }
                CollisionEnterAction(collision);
            }

            private void OnCollisionExit(Collision other)
            {
                if (CollisionExitAction == null)
                {
                    return;
                }
                CollisionExitAction(other);
            }

            private void OnCollisionStay(Collision collisionInfo)
            {
                if (CollisionStayAction == null)
                {
                    return;
                }
                CollisionStayAction(collisionInfo);
            }

            private void OnTriggerEnter(Collider other)
            {
                if (TriggerEnterAction == null)
                {
                    return;
                }
                TriggerEnterAction(other);
            }

            private void OnTriggerExit(Collider other)
            {
                if (TriggerExitAction == null)
                {
                    return;
                }
                TriggerExitAction(other);
            }

            private void OnTriggerStay(Collider other)
            {
                if (TriggerStayAction == null)
                {
                    return;
                }
                TriggerStayAction(other);
            }
        }
    }
}
