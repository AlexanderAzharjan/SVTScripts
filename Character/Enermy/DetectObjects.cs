using UnityEngine;

/*检测物体*/
public class DetectObjects : MonoBehaviour {

    public Transform detect_start; //检测起点
    public float block_max_distance; //检测阻碍物的最大长度
    public float player_max_distance; //检测玩家的最大长度
    public float max_degree; //检测的最大角度
    [HideInInspector] public bool detected_block = false; //是否检测到障碍物
    [HideInInspector] public bool detected_player = false; //是否检测到玩家
    private RaycastHit hit_info; //射线击中信息
    private bool player_hit = false; //玩家是否撞上来

    /*每帧更新的部分*/
    private void Update () {
        if (!player_hit) //如果玩家没有主动撞上来
        {
            detected_player = detected_block = false; //假定什么都没有检测到
        }
        for (detect_start.localEulerAngles = new Vector3 (0, 360 - max_degree, 0); detect_start.localEulerAngles.y >= (360 - max_degree) || detect_start.localEulerAngles.y <= max_degree; detect_start.localEulerAngles += new Vector3 (0, 1, 0)) //在敌人左右70度范围内
        {
            if (Physics.Raycast (detect_start.position, detect_start.forward, out hit_info, player_max_distance)) //向检测方向前方player_max_distance长度发射射线，如果检测到任何碰撞体
            {
                if (hit_info.collider.tag == "Player") //如果检测到的是玩家
                {
                    if (!GetComponent<EnermyMoveController> ().enabled) //如果自己无法移动
                    {
                        GetComponent<EnermyMoveController> ().enabled = true; //设定自己可以移动
                    }
                    if (!GetComponent<EnermyRotateController> ().enabled) //如果自己无法旋转
                    {
                        GetComponent<EnermyRotateController> ().enabled = true; //设定自己可以旋转
                    }
                    detected_player = true; //设定检测到玩家
                    player_hit = false; //设定玩家没有碰到敌人
                    GetComponent<AlarmController> ().saw_player = true; //设定见过玩家
                    break;
                }
            }
            if (Physics.Raycast (detect_start.position, detect_start.forward, out hit_info, block_max_distance)) //向检测方向前方block_max_distance长度发射射线，如果检测到任何碰撞体
            {
                if (hit_info.collider.tag == "Block" || hit_info.collider.tag == "UnthroughableBlock") //如果检测到的是障碍物
                {
                    detected_block = true; //设定检测到障碍物
                    break;
                }
            }
        }
    }

    /*如果与任何碰撞体碰撞*/
    private void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.transform.root.tag == "Player") //如果玩家撞上来
        {
            detected_player = true; //设定检测到玩家
            player_hit = true; //设定玩家撞了上来
        }
    }
}