using UnityEngine;
public class EnermyMoveController : MonoBehaviour/*敌人移动控制器*/
{
    public float move_speed;//敌人移动的速度
    public float back_speed;//敌人的后退速度

    /*每帧更新的部分*/
    private void Update()
    {
        if (!GetComponent<DetectObjects>().detected_player)//如果没有发现玩家
        {
            GetComponent<Rigidbody>().velocity = transform.forward * move_speed;//以move_speed向前方移动
            GetComponent<Actions>().Walk();//播放走路动画
        }
        else
        {
            if(GetComponent<EnermyRotateController>().delta_vector.sqrMagnitude != 0)//如果玩家与敌人之间距离不为0
            {
                GetComponent<Rigidbody>().velocity = (-GetComponent<EnermyRotateController>().delta_vector.normalized) * (back_speed / GetComponent<EnermyRotateController>().delta_vector.sqrMagnitude);//向远离玩家方向移动，并且距离越远，速度越慢
            }
        }
    }
}