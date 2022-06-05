using Battle;
using UnityEngine;

public class FPS : MonoBehaviour
{
    /// <summary>
    /// 上一次更新帧率的时间
    /// </summary>
    private float m_lastUpdateShowTime = 0f;
    /// <summary>
    /// 更新显示帧率的时间间隔
    /// </summary>
    private readonly float m_updateTime = 0.05f;
    /// <summary>
    /// 帧数
    /// </summary>
    private int m_frames = 0;
    /// <summary>
    /// 帧间间隔
    /// </summary>
    private float m_frameDeltaTime = 0;
    private float m_FPS = 0;
    private Rect m_fps, m_dtime, m_Unitydtime;
    private GUIStyle m_style = new GUIStyle();

    void Awake()
    {
        Application.targetFrameRate =60;
    }

    void Start()
    {
        m_lastUpdateShowTime = Time.realtimeSinceStartup;
        m_fps = new Rect(0, 0, 10, 10);
        m_dtime = new Rect(0, 20, 10, 10);
        m_Unitydtime = new Rect(0, 40, 10, 10);
        m_style.fontSize =20;
        m_style.normal.textColor = Color.white;
    }

    void Update()
    {
        m_frames++;
        if (Time.realtimeSinceStartup - m_lastUpdateShowTime >= m_updateTime)
        {
            m_FPS = m_frames / (Time.realtimeSinceStartup - m_lastUpdateShowTime);
            m_frameDeltaTime = (Time.realtimeSinceStartup - m_lastUpdateShowTime) / m_frames;
            m_frames = 0;
            m_lastUpdateShowTime = Time.realtimeSinceStartup;
            PropertySystem.MathfDeltatime = m_frameDeltaTime;
        }
    }

    void OnGUI()
    {
        GUI.Label(m_fps, "FPS: " + m_FPS, m_style);
        GUI.Label(m_dtime, "间隔: " + m_frameDeltaTime, m_style);
        GUI.Label(m_Unitydtime, "Unitydtime " + Time.deltaTime, m_style);
    }
}
