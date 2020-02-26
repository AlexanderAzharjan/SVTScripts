using UnityEngine;
public class SupplyController : MonoBehaviour/*供应体控制器*/
{
    public enum SupplyType { health, magic };//供给种类
    public SupplyType supplytype;//本身砖块的供给种类

    public float rotate_speed;//旋转速度
    private GameObject player;//补给需要移动到的位置
    public float move_speed;//移动速度
    private bool move = false;//是否进行移动

    /*初始化*/
    private void Start()
    {
        player = GameObject.FindWithTag("Player");//找到玩家
    }

    /*每帧更新的部分*/
    private void Update()
    {
        transform.Rotate(Vector3.up, rotate_speed);//方块自身进行旋转

        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, move_speed);//移动至玩家位置
            transform.localScale = Vector3.one * 0.3f;//缩小方块
        }
        if ((transform.position - player.transform.position).sqrMagnitude < 0.1f)//如果方块到达指定位置
        {
            Destroy(gameObject);//销毁方块
        }
    }

    /*当其他碰撞体接触*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")//如果是玩家接触
        {
            if (supplytype == SupplyType.health && other.GetComponent<PlayerStateChecker>().health_point < other.GetComponent<PlayerStateChecker>().max_health_point)//如果玩家生命值没有到达最大值
            {
                move = true;//设定进行移动
                other.GetComponent<PlayerStateChecker>().health_point++;//玩家生命值自增
            }
            else if (supplytype == SupplyType.magic && other.GetComponent<PlayerStateChecker>().magic_point < other.GetComponent<PlayerStateChecker>().max_magic_point)//如果玩家法力值没有到达最大值
            {
                move = true;//设定进行移动
                other.GetComponent<PlayerStateChecker>().magic_point++;//玩家法力值自增
            }
        }
    }
}