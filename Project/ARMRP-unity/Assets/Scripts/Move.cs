using Slate;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Cutscene cutscene;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject slateRes = Resources.Load<GameObject>("Slate/run");
        var slate = GameObject.Instantiate(slateRes);
        cutscene = slate.GetComponent<Cutscene>();
        foreach (var cutsceneGroup in cutscene.groups)
        {
            cutsceneGroup.actor = this.gameObject;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        cutscene.Play();
    }
}