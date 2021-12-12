using UnityEngine;

public class FsmManager : MonoBehaviour
{
    public float speed;
    public FSM fsm;

    public GameObject player;
    // Start is called before the first frame update
    private void Start()
    {
        Run run = new Run(player, speed, fsm);
        //整点花活
        Dodge dodge = new Dodge(player, new Dodge.DodgeDate()
        {
            distacne = 2f,
            speed = 2f
        }, fsm);
        fsm = new FSM(FSM.State.run, run);
        fsm.AddState(FSM.State.dogge, dodge);
    }

    // Update is called once per frame
    private void Update()
    {
        fsm.StateUpdate();
    }
}