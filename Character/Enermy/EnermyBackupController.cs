using System.Collections.Generic;
using UnityEngine;

/*敌人增援控制器*/
public class EnermyBackupController : MonoBehaviour {

    private bool call_for_backup = false; //是否请求增援
    private List<GameObject> enermies = new List<GameObject> (); //记录被呼叫的敌人
    public float backup_radius; //求助半径
    private Vector3 delta_vector; //该敌人与其他敌人的向量差
    private Vector3 move_direction; //敌人需要旋转的角度
    private GameObject player; //玩家
    [HideInInspector] public bool found_player; //是否已经发现了玩家

    /*初始化*/
    private void Start () {
        enermies.AddRange (GameObject.FindGameObjectsWithTag ("Enermy")); //记录所有敌人
        enermies.Remove (gameObject); //除去自己
        player = GameObject.FindGameObjectWithTag ("Player"); //找到玩家
    }

    /*每帧更新的部分*/
    private void Update () {
        if (GetComponent<DetectObjects> ().detected_player && !call_for_backup) //如果检测到了玩家并且没有申请过支援
        {
            call_for_backup = true; //设置请求支援
        }
        if (call_for_backup) //如果请求支援
        {
            foreach (GameObject enermy in enermies) //遍历所有记录的敌人
            {
                if (enermy == null) //如果找不到该敌人，即敌人已经被消灭
                {
                    enermies.Remove (enermy); //将此敌人移除
                    break; //跳过此敌人
                }
                if (enermy.GetComponent<MonoBehaviour> ().enabled && (enermy.transform.position - transform.position).sqrMagnitude < Mathf.Pow (backup_radius, 2)) //如果其他敌人与该敌人距离较近且这些敌人没有被时停
                {
                    enermy.GetComponent<EnermyBackupController> ().found_player = true; //设定这些敌人发现了玩家
                }
            }
            call_for_backup = false; //关闭请求支援
        }
        if (found_player) //如果发现了玩家
        {
            LookAtPlayer (player); //看向玩家
        }
        if (GetComponent<DetectObjects> ().detected_player) //如果看到了玩家
        {
            found_player = false; //设定不看向玩家
        }
    }

    /*看向玩家*/
    public void LookAtPlayer (GameObject player) {
        delta_vector = player.transform.position - transform.position; //找到其他敌人与该敌人之间的向量差
        move_direction = new Vector3 (0, (delta_vector.x >= 0 ? 90 : -90) - Mathf.Atan (delta_vector.z / delta_vector.x) * Mathf.Rad2Deg, 0); //获取需要旋转的角度：如果玩家位于敌人右侧，右侧需要旋转的角度范围从0到180，左侧需要旋转的角度范围从0到-180，由arctan(dy/dx)推出，需要由弧度制转换为角度制
        transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (move_direction), GetComponent<EnermyRotateController> ().rotate_speed * 3); //以rotate_speed * 3的速度，转向玩家
    }
}