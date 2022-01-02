using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        CutsceneHelper.Play(this.gameObject, "Dodge");
      
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
