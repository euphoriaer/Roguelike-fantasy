using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle
{
    [RequireComponent(typeof(BehitSystem))]
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.MoveSystem)]
    [UnityEngine.AddComponentMenu("System/MoveSystem")]
    internal class MoveSystem : SystemMonoBehaviour
    {
        public Vector3 Direction;

        [LabelText("�����ƶ��ٶ�")]
        public float MaxSpeed = 10;

        [LabelText("��ǰ�ƶ��ٶ�")]
        public float CurSpeed=10;

        private void FixedUpdate()
        {
            this.transform.position = this.transform.position + CurSpeed * Direction.normalized * this.transform.GetComponent<PropertySystem>().DeltaTime;
            //���������ײʱ  �������
        }

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}