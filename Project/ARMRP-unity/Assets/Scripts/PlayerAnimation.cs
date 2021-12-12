using Slate;
using UnityEngine;

public static class PlayerAnimation
{
    public static Cutscene Play(GameObject player, string AnimationName)
    {
        //播放动画
        GameObject slateRes = Resources.Load<GameObject>($"Slate/Player/{AnimationName}");
        var slate = GameObject.Instantiate(slateRes);
        slate.name = "SlateRun";
        var cutscene = slate.GetComponent<Cutscene>();
        foreach (var cutsceneGroup in cutscene.groups)
        {
            cutsceneGroup.actor = player;
        }

        return cutscene;
    }
}