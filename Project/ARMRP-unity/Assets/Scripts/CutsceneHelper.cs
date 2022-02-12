using Slate;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CutsceneHelper
{
    public static Cutscene InstateAndPlay(GameObject player, string CutsceneName)
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

    public static Cutscene Instate(GameObject player, string CutsceneName)
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

        return cutscene;
    }

    public static Cutscene Instate(GameObject player, Cutscene inCutscene)
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
        Cutscene slate = GameObject.Instantiate(inCutscene);
        slate.transform.position = RoleActionCutscene.transform.position;
        slate.transform.SetParent(RoleActionCutscene.transform, false);
        foreach (var cutsceneGroup in slate.groups)
        {
            cutsceneGroup.actor = player;
        }

        return slate;
    }
    /// <summary>
    /// 得到指定类型，指定名字的Cutscene Clip
    /// </summary>
    /// <typeparam name="T">Clip 类型</typeparam>
    /// <param name="cutscene"></param>
    /// <param name="CutsceneClipName">Clip名字，Cutscene中填写</param>
    /// <returns></returns>
    public static T GetCutsceneClip<T>(this Cutscene cutscene, string CutsceneClipName) where T : CutsceneClipBase
    {
        //通过cutscene 对象找到所有的Clips，
        foreach (var group in cutscene.groups)
        {
            foreach (var track in group.tracks)
            {
                var clips = track.clips.ToList();

                for (int i = 0; i < clips.Count; i++)
                {
                    var curClip = clips[i];
                    var cutsceneClip = curClip as T;
                    if (cutsceneClip == null)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(cutsceneClip.CutsceneClipName))
                    {
                        continue;
                    }

                    if (cutsceneClip.CutsceneClipName == CutsceneClipName)
                    {
                        return cutsceneClip as T;
                    }
                }
            }
        }

        return null;
    }
    /// <summary>
    /// 得到Cutscene 所有对应的Clip
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cutscene"></param>
    /// <returns></returns>
    public static List<T> GetCutsceneClip<T>(this Cutscene cutscene) where T : CutsceneClipBase
    {
        List<T> cutsceneClips = new List<T>();
        //通过cutscene 对象找到所有的Clips
        foreach (var group in cutscene.groups)
        {
            foreach (var track in group.tracks)
            {
                var clips = track.clips.ToList();

                for (int i = 0; i < clips.Count; i++)
                {
                    var curClip = clips[i];
                    if (curClip is T)
                    {
                        cutsceneClips.Add(curClip as T);
                    }
                }
            }
        }

        return cutsceneClips;
    }
}