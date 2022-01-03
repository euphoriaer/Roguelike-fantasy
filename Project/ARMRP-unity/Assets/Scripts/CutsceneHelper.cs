using Slate;
using UnityEngine;

public static class CutsceneHelper
{
    public static Cutscene Play(GameObject player, string CutsceneName)
    {
        GameObject RoleActionCutscene = player.transform.Find("RoleActionCutscene")?.gameObject;
        if (RoleActionCutscene == null)
        {
            RoleActionCutscene = new GameObject("RoleActionCutscene");
            RoleActionCutscene.transform.SetParent(player.transform, false);
        }
        else
        {
            GameObject.Destroy(RoleActionCutscene.transform.GetChild(0).gameObject);
        }
        //播放动画
        GameObject slateRes = Resources.Load<GameObject>($"Slate/Player/{CutsceneName}");
        if (slateRes == null)
        {
            Debug.LogError("找不到对应的Cutscene");
        }
        var slate = GameObject.Instantiate(slateRes);

        slate.transform.position = RoleActionCutscene.transform.position;
        slate.transform.SetParent(RoleActionCutscene.transform, false);
        var cutscene = slate.GetComponent<Cutscene>();
        foreach (var cutsceneGroup in cutscene.groups)
        {
            cutsceneGroup.actor = player;
        }
        cutscene.Play();
        return cutscene;
    }
}