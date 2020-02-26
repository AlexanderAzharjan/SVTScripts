using UnityEngine;
public class AlarmController : MonoBehaviour/*敌人的警惕控制器*/
{
    [HideInInspector] public bool saw_player = false;//是否看见过玩家
    [HideInInspector] public bool stay_alramed = false;//是否保持警惕
    private float alarming_time = 3;//警惕时长

    /*每帧更新的部分*/
    private void Update()
    {
        if (saw_player && !GetComponent<DetectObjects>().detected_player)//如果之前见过玩家并且目前没有检测到玩家
        {
            stay_alramed = true;//设定保持警惕
            saw_player = false;//设定没有看到玩家
        }
        if (stay_alramed)//如果敌人保持警惕
        {
            GetComponent<Actions>().Aiming();//瞄准敌人
            GetComponent<Rigidbody>().velocity = Vector3.zero;//让自己停下来
            if (GetComponent<EnermyMoveController>().enabled)//如果自己可以移动
            {
                GetComponent<EnermyMoveController>().enabled = false;//设定自己无法移动
            }
            if (GetComponent<EnermyRotateController>().enabled)//如果自己可以旋转
            {
                GetComponent<EnermyRotateController>().enabled = false;//设定自己无法旋转
            }

            if (alarming_time < 0 || GetComponent<DetectObjects>().detected_player)//如果计时结束或者警惕过程中发现了敌人
            {
                if (!GetComponent<EnermyMoveController>().enabled)//如果自己无法移动
                {
                    GetComponent<EnermyMoveController>().enabled = true;//设定自己可以移动
                }
                if (!GetComponent<EnermyRotateController>().enabled)//如果自己无法旋转
                {
                    GetComponent<EnermyRotateController>().enabled = true;//设定自己可以旋转
                }
                stay_alramed = false;//设定放下警惕或处于攻击状态
                alarming_time = 3;//重置警惕时长
            }
            else//如果处于警惕状态
            {
                alarming_time -= Time.deltaTime;//警惕计时
            }
        }
    }
}