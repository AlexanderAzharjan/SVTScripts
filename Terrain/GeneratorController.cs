using UnityEngine;
using System.Collections.Generic;
public class GeneratorController : MonoBehaviour/*生成控制器*/
{
    private enum BlockType { vertical, horizontal };//阻碍种类的枚举

    public float interval;//间隔长度

    public int min__number;//障碍物生成的最小数(行或列)
    public GameObject block;//障碍物预设

    private GameObject bound_block;//边界阻碍物
    public Transform vertical_start;//纵向阻碍的起始生成点
    public Transform horizontal_start;//横向阻碍的起始生成点

    public GameObject enermy;//敌人预设
    public Transform enermy_start;//敌人生成启动位置
    private Vector2 possible_position;//敌人可以生成的位置
    private List<Vector2> exist_position = new List<Vector2>();//已经记录生成的坐标位置
    private int enermy_number;//敌人生成的数量

    public GameObject portal;//传送门
    private Vector2 portal_position;//传送门生成位置

    public GameObject player;//玩家

    /*初始化*/
    void Start()
    {
        InstantiateBlocks(BlockType.vertical, vertical_start.position);//生成纵向阻碍
        InstantiateBlocks(BlockType.horizontal, horizontal_start.position);//生成横向阻碍
        InstantiateEnermies();//生成敌人
        InstantiatePortal();//生成传送门
        InstantiatePlayer();//生成玩家
    }

    /*生成矩阵*/
    private void InstantiateBlocks(BlockType block_type, Vector3 generate_position)//BlockType为阻碍的种类，min_blocktype_number为阻碍的最小个数(行或列),generate_position为生成点
    {
        if (block_type == BlockType.vertical)//如果要生成纵向阻碍
        {
            for (int i = 0; i < min__number + 1; i++)//遍历矩阵行数
            {
                for (int j = 0; j < min__number; j++)//遍历矩阵纵数
                {
                    if (i == 0 || i == min__number)//如果是边界
                    {
                        bound_block = Instantiate(block, generate_position, Quaternion.identity);//生成纵向阻碍
                        bound_block.tag = "UnthroughableBlock";//改变障碍物标签为"UnthroughableBlock"
                    }
                    else if (Random.Range(0, 2) == 0)//如果不是边界，约50%几率生成阻碍物
                    {
                        Instantiate(block, generate_position, Quaternion.identity);//生成纵向阻碍
                    }
                    generate_position += new Vector3(0, 0, interval);//生成下一个阻碍之前更新生成点纵坐标
                }
                generate_position += new Vector3(interval, 0, 0);//生成下一个阻碍之前更新生成点横坐标
                generate_position = new Vector3(generate_position.x, generate_position.y, vertical_start.position.z);//重置纵坐标
            }
        }
        else//如果要生成横向阻碍
        {
            for (int i = 0; i < min__number; i++)//遍历矩阵行数
            {
                for (int j = 0; j < min__number + 1; j++)//遍历矩阵纵数
                {
                    if (j == 0 || j == min__number)//如果是边界
                    {
                        bound_block = Instantiate(block, generate_position, Quaternion.Euler(0, 90, 0));//生成横向阻碍
                        bound_block.tag = "UnthroughableBlock";//改变障碍物标签为"UnthroughableBlock"
                    }
                    else if (Random.Range(0, 3) == 0)//如果不是边界，约33%几率生成阻碍物
                    {
                        Instantiate(block, generate_position, Quaternion.Euler(0, 90, 0));//生成横向阻碍
                    }
                    generate_position += new Vector3(0, 0, interval);//生成下一个阻碍之前更新生成点纵坐标
                }
                generate_position += new Vector3(interval, 0, 0);//生成下一个阻碍之前更新生成点横坐标
                generate_position = new Vector3(generate_position.x, generate_position.y, horizontal_start.position.z);//重置纵坐标
            }
        }
    }

    /*生成敌人*/
    private void InstantiateEnermies()//generate_position为生成点
    {
        for (enermy_number = 0; enermy_number != interval; enermy_number++)//一个个生成敌人直到规定数量
        {
            while (true)//循环以下过程
            {
                possible_position = new Vector2(Random.Range(-(min__number / 2), min__number / 2 + 1), Random.Range(-(min__number / 2), min__number / 2 + 1));//随机生成一个可能的位置
                if (!exist_position.Contains(possible_position) && !(possible_position.x == 0 && possible_position.y == 0) && !(Mathf.Abs(possible_position.x) == 1) && !(Mathf.Abs(possible_position.y) == 1))//如果这个位置没有被记录过并且不是生成起点
                {
                    exist_position.Add(possible_position);//记录该位置
                    break;//跳出循环
                }
            }
            Instantiate(enermy, new Vector3(enermy_start.position.x + (possible_position.x * interval), enermy_start.position.y, enermy_start.position.z + (possible_position.y * interval)), Quaternion.identity);//生成敌人
        }
    }

    /*生成传送门*/
    private void InstantiatePortal()
    {
        /*在地图的四个角生成传送门*/
        if (Random.Range(0, 2) == 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                portal_position = new Vector2(-88, -82.5f);
            }
            else
            {
                portal_position = new Vector2(80, -82.5f);
            }
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                portal_position = new Vector2(-88, 85.5f);
            }
            else
            {
                portal_position = new Vector2(80, 85.5f);
            }
        }
        Instantiate(portal, new Vector3(portal_position.x, 0, portal_position.y), Quaternion.identity);//生成传送门
    }

    /*生成玩家*/
    private void InstantiatePlayer()
    {
        Instantiate(player);//实例化玩家
    }
}