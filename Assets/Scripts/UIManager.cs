﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public InputField inputField;
    public Text tip;
    public Text text;

    /// <summary>
    /// 偏移量阈值
    /// </summary>
    public int threshold = 150;

    /// <summary>
    /// 点击的开始位置
    /// </summary>
    Vector3 startPos;

    /// <summary>
    /// 偏移量
    /// </summary>
    Vector3 offset;

    /// <summary>
    /// 触摸的开始位置
    /// </summary>
    Vector2 touchStartPos;

    /// <summary>
    /// 触摸偏移量
    /// </summary>
    Vector2 touchOffset;

    /// <summary>
    /// 地图移动速度
    /// </summary>
    float speed = 5f;

    /// <summary>
    /// 地图物体
    /// </summary>
    GameObject environment;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE

        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;

            environment = GameObject.Find("Environment");
        }

        if (Input.GetMouseButton(0))
        {
            offset = Input.mousePosition - startPos;

            if (offset.x != 0 && Mathf.Abs(offset.x) > threshold)
            {
                environment.transform.position += (offset.x < 0 ? Vector3.left : Vector3.right) * speed * Time.deltaTime;
            }

            if(offset.y != 0 && Mathf.Abs(offset.y) > threshold)
            {
                environment.transform.position += (offset.y < 0 ? Vector3.back : Vector3.forward) * speed * Time.deltaTime;
            }
        }
#else

        if (Input.touchCount == 1)
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                touchStartPos = Input.touches[0].position;

                environment = GameObject.Find("Environment");
            }

            if(Input.touches[0].phase == TouchPhase.Moved)
            {
                touchOffset = Input.touches[0].position - touchStartPos;

                if (touchOffset.x != 0 && Mathf.Abs(touchOffset.x) > threshold)
                {
                    environment.transform.position += (touchOffset.x < 0 ? Vector3.left : Vector3.right) * speed * Time.deltaTime;
                }

                if (touchOffset.y != 0 && Mathf.Abs(touchOffset.y) > threshold)
                {
                    environment.transform.position += (touchOffset.y < 0 ? Vector3.back : Vector3.forward) * speed * Time.deltaTime;
                }
            }
        }

#endif
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
