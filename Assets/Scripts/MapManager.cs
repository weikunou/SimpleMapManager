using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    // 地图数据文件
    public TextAsset mapAsset;
    public int mapIndex;

    // 设定地图道具的符号
    private const char TREE = '#'; // 树
    private const char MUSHROOM = '*'; // 蘑菇

    // 地图道具对应的3D素材
    public GameObject tree;
    public GameObject mushroom;
    public GameObject grass;

    // 地图数据的结构体
    struct MapData
    {
        public int length; // 地图长度
        public int width; // 地图宽度

        public char[,] data; // 地图数据
    };

    // 读取到的地图数据
    private MapData mapData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        // 创建地图
        CreateMap();
    }

    // 创建地图
    public void CreateMap()
    {
        DeleteMap();

        // 读取地图 txt 文件
        mapAsset = Resources.Load<TextAsset>("Maps/map_" + mapIndex);

        // 读取地图数据
        LoadMapData(mapAsset);

        // 生成场景
        GenerateScene();
    }

    // 读取地图数据
    void LoadMapData(TextAsset asset)
    {
        if(asset != null)
        {
            string txtMapData = asset.text;

            // 一个枚举类型，用于将分割后的字符数组中的空元素删除
            System.StringSplitOptions option = System.StringSplitOptions.RemoveEmptyEntries;

            // 以 \r \n 为分割符，并删除空元素
            string[] lines = txtMapData.Split(new char[] { '\r', '\n' }, option);
            string[] chars = lines[0].Split(new char[] { ',' }, option);

            mapData.length = chars.Length;
            mapData.width = lines.Length;

            // 二维字符数组保存分割后的地图数据，数组的尺寸由地图尺寸决定
            char[,] mapPropData = new char[mapData.width, mapData.length];

            for (int row = 0; row < mapData.width; row++)
            {
                string[] tempData = lines[row].Split(new char[] { ',' }, option);

                for (int col = 0; col < mapData.length; col++)
                {
                    mapPropData[row, col] = tempData[col][0];
                }
            }

            mapData.data = mapPropData;

            UIManager.instance.tip.text = "生成地图成功！";
            UIManager.instance.text.text = "当前地图索引：" + mapIndex;
        }
        else
        {
            Debug.LogWarning("地图文件为空");
            UIManager.instance.tip.text = "地图文件为空...";
        }
    }

    // 生成场景
    void GenerateScene()
    {
        // 生成环境空物体
        GameObject obj_parent = new GameObject("Environment");

        obj_parent.tag = "Environment";

        for (int i = 0; i < mapData.width; i++)
        {
            for(int j = 0; j< mapData.length; j++)
            {
                // 计算每个道具的位置
                Vector3 pos = new Vector3(j, 0.0f, -i);

                // 生成草地
                GameObject obj_grass = Instantiate(grass, pos - new Vector3(0, 0.5f, 0), grass.transform.rotation);
                obj_grass.transform.parent = obj_parent.transform;

                // 根据地图数据生成相应的道具模型
                switch (mapData.data[i, j])
                {
                    // 生成树
                    case TREE:
                        GameObject obj_tree = Instantiate(tree, pos, Quaternion.identity);
                        obj_tree.transform.parent = obj_parent.transform;
                        break;
                    // 生成蘑菇
                    case MUSHROOM:
                        GameObject obj_mushroom = Instantiate(mushroom, pos, Quaternion.identity);
                        obj_mushroom.transform.parent = obj_parent.transform;
                        break;
                }
            }
        }
    }

    // 删除地图
    public void DeleteMap()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Environment");

        if (obj != null)
        {
            Destroy(obj);
        }
    }
}
