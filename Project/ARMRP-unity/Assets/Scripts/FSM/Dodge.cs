using Slate;
using UnityEngine;

public class Dodge : IState
{
    private GameObject _player;
    private DodgeDate _dodgeDate;
    private Cutscene cutscene;

    private Vector3 enterPoint;
    private Vector3 curPosition;

    private FSM fsm;

    public class DodgeDate
    {
        /// <summary>
        /// 闪避距离
        /// </summary>
        public float distacne;

        /// <summary>
        /// 闪避速度
        /// </summary>
        public float speed;
    }

    public Dodge(GameObject player, DodgeDate dodgeDate, FSM stateFsm)
    {
        fsm = stateFsm;
        _dodgeDate = dodgeDate;
        _player = player;
    }

    public void Enter()
    {
        Debug.Log("进入闪避");
        cutscene = PlayerAnimation.Play(_player, "dodge");
        //记录刚进入闪避的 point
        enterPoint = _player.transform.position;

        //处于无敌进入闪避
    }

    public void Update()
    {
        //播放动画
        cutscene.Play();
        bool isDodge = cutscene.isActive;
        if (!isDodge)
        {
            return;
        }
        //产生效果
        if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("shanbia");
            _player.transform.Translate(Vector3.left * _dodgeDate.speed);
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("shanbib");
            _player.transform.Translate(Vector3.back * _dodgeDate.speed);
        }

        if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("shanbic");
            _player.transform.Translate(Vector3.right * _dodgeDate.speed);
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("shanbid");
            _player.transform.Translate(Vector3.forward * _dodgeDate.speed);
        }

        //得到闪避距离
        curPosition = _player.transform.position;
        var dodgeDis = Vector3.Distance(enterPoint, curPosition);

        if (dodgeDis >= _dodgeDate.distacne)
        {
            //切换模式
            fsm.TransformState(FSM.State.run);
        }
    }

    public void Exit()
    {
        //退出无敌
    }
}