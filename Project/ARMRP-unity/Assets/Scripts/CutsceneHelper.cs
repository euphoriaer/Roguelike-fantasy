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

    /// <summary>
    /// 同名Cutscene 同时只播放一个
    /// </summary>
    /// <param name="player"></param>
    /// <param name="cutscene"></param>
    /// <param name="isLoop"></param>
    /// <returns></returns>
    public static Cutscene InstateInChildren(GameObject player, Cutscene cutscene, out bool isLoop)
    {
        if (cutscene != null)
        {
            var _cutscene = CutsceneHelper.Instate(cutscene, player);

            GameObject RoleActionCutscene = player.transform.Find("Cutscene")?.gameObject;
            if (RoleActionCutscene == null)
            {
                RoleActionCutscene = new GameObject("Cutscene");
                RoleActionCutscene.transform.SetParent(player.transform, false);
            }
            else
            {
                //查找同名Cutscene ,
                var lastCutscene = RoleActionCutscene.transform.FindInChildren(_cutscene.gameObject.name, true);

                if (lastCutscene != null)
                {
                    GameObject.Destroy(lastCutscene.gameObject);
                }
            }

            _cutscene.transform.SetParent(RoleActionCutscene.transform, false);

            //修改Loop 防止拉回原点
            if (_cutscene.defaultWrapMode == Cutscene.WrapMode.Loop)
            {
                isLoop = true;
            }
            else
            {
                isLoop = false;
            }
            _cutscene.updateMode = Cutscene.UpdateMode.Manual;
            return _cutscene;
        }
        isLoop = false;
        return null;
    }

    /// <summary>
    /// 唯一Cutscene
    /// </summary>
    /// <returns></returns>
    public static Cutscene InstateAction(out bool isLoop, Cutscene cutscene, params GameObject[] actors)
    {
        if (cutscene != null)
        {
            var _cutscene = CutsceneHelper.Instate(cutscene, actors);

            GameObject RoleActionCutscene = actors[0].transform.Find("ActionCutscene")?.gameObject;
            if (RoleActionCutscene == null)
            {
                RoleActionCutscene = new GameObject("ActionCutscene");
                RoleActionCutscene.transform.SetParent(actors[0].transform, false);
            }
            else
            {
                //销毁原本播放的Cutscene
                var cutsceneObj = RoleActionCutscene.transform.GetChild(0).gameObject;
                cutsceneObj.GetComponent<Cutscene>().Stop();
                UnityEngine.GameObject.Destroy(cutsceneObj);
            }

            _cutscene.transform.SetParent(RoleActionCutscene.transform, false);

            if (_cutscene.defaultWrapMode == Cutscene.WrapMode.Loop)
            {
                isLoop = true;
            }
            else
            {
                isLoop = false;
            }
            _cutscene.updateMode = Cutscene.UpdateMode.Manual;
            return _cutscene;
        }
        isLoop = false;
        return null;
    }

    public static Cutscene Instate(Cutscene inCutscene, params GameObject[] player)
    {
        Cutscene slate = GameObject.Instantiate(inCutscene);

        for (int i = 1; i < slate.groups.Count; i++)
        {
            var cutsceneGroup = slate.groups[i];
            cutsceneGroup.actor = player[i - 1];
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