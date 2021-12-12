using Slate;
using UnityEngine;

public class Run : IState
{
    private GameObject player;
    private Cutscene cutscene;
    private float speed;
    private FSM fsm;

    public Run(GameObject player, float runSpeed, FSM stateFsm)
    {
        this.player = player;
        speed = runSpeed;
        fsm = stateFsm;
    }

    public void Enter()
    {
        //Debug.Log("进入Run");
        //////播放动画
        cutscene = PlayerAnimation.Play(player, "run");
    }

    public void Update()
    {
        //在run的时候 控制移动，同时播放移动动画

        //播放动画
        cutscene.Play();

        //控制移动
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("a");
            player.transform.Translate(Vector3.left * speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("s");
            player.transform.Translate(Vector3.back * speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("d");
            player.transform.Translate(Vector3.right * speed);
        }

        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("w");
            player.transform.Translate(Vector3.forward * speed);
        }
        
    }

    public void Exit()
    {
        Debug.Log("退出Run");
    }
}