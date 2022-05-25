using Battle;
using Battle.Resource;
using Battle.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card
{
    public bool isEnable = false;
    private GameObject cardObj;

    private Image icon;
    private TextMeshProUGUI text;
    private Button btn;

    private float lerpSpeed = 2f;

    private float LerpTime = 0;
    private int[] Text = new int[5];
    private float y;

    public Card(int cardId, GameObject parent)
    {
        //读取Json,获取ID;
        var cardJson = CardConfig.GetCard(cardId);
        string iconPath = cardJson[CardConfig.IconPath].ToString();
        string description = cardJson[CardConfig.Description].ToString();
        int buffId = int.Parse(cardJson[CardConfig.BuffID].ToString());

        cardObj = UIManager.Instate.CreateGizmos("Card",parent);
        icon = cardObj.transform.GetChild(1).GetComponent<Image>();
        text = cardObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        btn = cardObj.transform.GetChild(3).GetComponent<Button>();

        // 通过ID 读取图片路径，替换
        icon.sprite = ResourceManager.Instate.Load<Sprite>($"UI/GUI PRO Kit - Fantasy RPG/Sprites/Component/Icon_ItemIcons_(Original)/{iconPath}");
        text.text = description;
        //读取描述
        //增加按下事件 buff
        btn.onClick.AddListener(() =>
        {
            //给主角添加Buff
            BuffHelper.AddBuff(GameObject.FindGameObjectWithTag("Player"), buffId);
            //销毁
            UIManager.Instate.GetPanel("CardPanel").DestoryPanel();
           
        });
    }

    public void Destory()
    {
        cardObj.SetActive(false);
        isEnable = false;
    }

    public void Show()
    {
        cardObj.SetActive(true);
        y = cardObj.transform.rotation.eulerAngles.y;
        isEnable = true;
    }

    public void Update()
    {
        LerpTime += lerpSpeed * Time.deltaTime;
        float cury = Mathf.Lerp(y, 0, LerpTime);
        cardObj.transform.rotation = Quaternion.Euler(new Vector3(0, cury, 0));
        //cardObj.transform.rotation = Quaternion.EulerAngles(new Vector3(0, cury, 0)); 已过时不要使用
    }
}