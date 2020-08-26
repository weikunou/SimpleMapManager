using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public InputField inputField;
    public Text tip;
    public Text text;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // 改变地图索引
    public void ChangeMapIndex()
    {
        // 判断文本框输入的是否为纯数字
        if (int.TryParse(inputField.text,out int number))
        {
            // 转换成功，修改地图索引
            MapManager.instance.mapIndex = int.Parse(inputField.text);
        }
        else
        {
            Debug.LogWarning("输入的字符串不是纯数字");
            tip.text = "输入的字符串不是纯数字...";
        }
    }
}
