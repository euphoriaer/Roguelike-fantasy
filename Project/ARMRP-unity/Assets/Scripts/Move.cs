using Slate;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Cutscene cutscene;

    public GameObject babala;
    // Start is called before the first frame update
    private void Start()
    {
        GameObject slate = Resources.Load<GameObject>("Slate/run");
        var cutscene = slate.GetComponent<Cutscene>();
    }

    // Update is called once per frame
    private void Update()
    {
        cutscene.Play();
      
    }

    public void Cutscene_OnStop()
    {
        Debug.Log("12345");
    }
}