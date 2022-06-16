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

      
        [LabelText("��ǰ�ƶ��ٶ�")]
        public float CurSpeed;

        

        private void FixedUpdate() 
        {
            //todo �ٶȿ�������ƽ����Ϊ���ɶ������ʱ�� learp
            //CurSpeed = Mathf.Lerp(0, MaxSpeed, LerpTime);
            //LerpTime += LerpSpeed * GetComponent<PropertySystem>().DeltaTime;
            this.transform.position = this.transform.position + CurSpeed * Direction.normalized *
                this.transform.GetComponent<PropertySystem>().FixedDeltaTime;
            //ʹ�ñ�֡���� �ƶ���ײҪ�Լ�д
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