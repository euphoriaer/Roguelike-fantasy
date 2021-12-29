using Sirenix.OdinInspector;
using Slate;
using UnityEngine;

public class Move : MonoBehaviour
{  
    public float speed;

    // Start is called before the first frame update
    private void Start()
    {
    
    }

    // Update is called once per frame
    private void Update()
    {
        //¿ØÖÆÒÆ¶¯
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("a");
            this.transform.Translate(Vector3.left * speed);
        }
    }
}