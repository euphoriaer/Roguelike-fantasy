using Battle;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    [ParadoxNotion.Design.Name("MoveToPlayerAcion")]
    [ParadoxNotion.Design.Category("My Nodes/Monster")]
    public class MonsterMoveAction : ActionTask<Transform>
    {
        public float Allowance;
        private GameObject _player;
        protected override void OnExecute()
        {
            _player = GameObject.Find("Player");
            if (_player == null)
            {
                Debug.LogError("未找到玩家，无法移动");
            }
           
        }

        protected override string OnInit()
        {
            
            return base.OnInit();
        }

        protected override void OnStop()
        {
           
        }

        protected override void OnUpdate()
        {
            var moveSystem = agent.gameObject.GetComponent<MoveSystem>();
            moveSystem.Direction = _player.transform.position - agent.position;
            if (Vector3.Distance(_player.transform.position, agent.position)<Allowance)
            {
                moveSystem.Direction = Vector3.zero;
                EndAction();
            }
        }
    }
}
