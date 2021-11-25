using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class TaskTest : MonoBehaviour
{
    private Task _task;
    

    // Start is called before the first frame update
    private void Start()
    {
        GameObject t = new GameObject("12");

        StartCoroutine("Test");

        //var _task=  Task.Run((() =>
        //{
            
        //        Thread.Sleep(1000);
        //        Debug.Log("T2");
        //        t.name = "778";
        //        Instantiate(t,Vector3.zero,Quaternion.identity);
        //        Debug.Log("T3");
        //}));

      //–≠≥Ã≤‚ ‘
        Debug.Log("T1");
    }

    private IEnumerator Test()
    {
        
        GameObject t = new GameObject("12");
        Debug.Log("T2");
        t.name = "778";
        Instantiate(t, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Debug.Log("T3");
    }
    // Update is called once per frame
    private void Update()
    {
       
    }

    private void OnDisable()
    {
       
    }


    private void OnDestroy()
    {
        
    }



}