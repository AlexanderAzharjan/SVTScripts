using UnityEngine;

/*敌人旋转控制器，继承ShotController*/
public class EnermyRotateController : ShotController {
    public float rotate_speed; //敌人旋转的速度
    public float waking_time; //敌人行走的时间
    private float timer; //计时器

    private Vector3 move_direction; //敌人移动的方向
    private bool turn_back = false; //是否应当转身
    private bool set_turn_direction = false; //是否设置好转向设置

    private GameObject player; //玩家
    [HideInInspector] public Vector3 delta_vector; //玩家与敌人之间的向量差值
    private bool turn_to_player = false; //是否已经转向玩家

    /*初始化*/
    private void Start () {
        player = GameObject.FindGameObjectWithTag ("Player"); //找到玩家
    }

    /*每帧更新的部分*/
    private void Update () {
        if (!GetComponent<DetectObjects> ().detected_player) //如果没有检测到玩家
        {
            if (turn_to_player) //如果之前看过玩家
            {
                turn_to_player = false; //设置未看到玩家
            }
            if (GetComponent<DetectObjects> ().detected_block && !turn_back) //如果检测到前方有阻碍物并且未设置转身
            {
                turn_back = true; //设定此时应当转身
            }
            if (!turn_back) //如果不需要转身
            {
                if (timer <= 0) //如果计时结束
                {
                    timer = waking_time; //重置计时器
                    move_direction = new Vector3 (0, Random.Range (0, 4) * 90, 0); //随机获得一个旋转角度
                } else //如果在计时
                {
                    timer -= Time.deltaTime; //进行计时
                    if (transform.eulerAngles != move_direction) //如果玩家的旋转角度不等于设定好的旋转角度
                    {
                        transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (move_direction), rotate_speed); //以rotate_speed角速度进行旋转
                    }
                }
            } else //如果需要转身
            {
                if (!set_turn_direction) //如果没有设置转向方向
                {
                    move_direction = new Vector3 (0, Random.Range (0, 4) * 90, 0); //随机获得一个旋转角度
                    set_turn_direction = true; //设定已经转向
                }
                if (transform.eulerAngles != move_direction) //如果玩家的旋转角度不等于设定好的旋转角度
                {
                    transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (move_direction), rotate_speed); //以rotate_speed角速度进行转身
                } else //玩家选择到正确角度
                {
                    set_turn_direction = false; //设定未设置转向
                    turn_back = false; //设置此时无需转身
                }
            }
        } else //如果检测到玩家
        {
            if (!turn_to_player) //如果未决定看向玩家，将之前的所有开关关闭
            {
                set_turn_direction = false; //设定未设置转向
                turn_back = false; //设置此时无需转身
                turn_to_player = true; //准备看向玩家
            }
            delta_vector = player.transform.position - transform.position; //找到玩家与敌人之间的向量差
            move_direction = new Vector3 (0, (delta_vector.x >= 0 ? 90 : -90) - Mathf.Atan (delta_vector.z / delta_vector.x) * Mathf.Rad2Deg, 0); //获取需要旋转的角度：如果玩家位于敌人右侧，右侧需要旋转的角度范围从0到180，左侧需要旋转的角度范围从0到-180，由arctan(dy/dx)推出，需要由弧度制转换为角度制
            transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (move_direction), rotate_speed * 3); //以rotate_speed * 3的速度，转向玩家
        }
    }
}