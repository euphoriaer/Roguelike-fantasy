using Slate;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Cutscene cutscene;

    public GameObject babala;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject slateRes = Resources.Load<GameObject>("Slate/dodge");
        var slate = GameObject.Instantiate(slateRes);
        cutscene = slate.GetComponent<Cutscene>();
        foreach (var cutsceneGroup in cutscene.groups)
        {
            cutsceneGroup.actor = babala;
        }

        //cutscene.playingWrapMode = Cutscene.WrapMode.Loop;

        cutscene.Play();
    }

    // Update is called once per frame
    private void Update()
    {
    }
}