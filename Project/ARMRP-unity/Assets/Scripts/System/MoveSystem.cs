using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : MonoBehaviour
{
    public float Speed;
    public Vector3 Direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        this.transform.position = this.transform.position + Speed * Direction.normalized * Time.deltaTime;
        //避免持续碰撞时  相机抖动
    }
}
