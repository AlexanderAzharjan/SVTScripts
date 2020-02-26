using UnityEngine;
public class PlayerStateChecker : MonoBehaviour/*玩家状态检查器，继承StateChecker*/
{
    [HideInInspector] public float max_health_point;//最大生命值
    [HideInInspector] public float health_point;//生命值

    [HideInInspector] public float max_magic_point;//最大法力值
    [HideInInspector] public float magic_point;//玩家法力值
    [HideInInspector] public bool magic_consume;//是否消耗法力值
    [HideInInspector] public bool dead = false;//玩家是否死亡

    public bool update = false;//是否更新了数值

    /*每帧更新的部分*/
    private void Update()
    {
        if (!update)
        {
            max_health_point = StateController.player_max_health_point;//初始化最大生命值
            health_point = StateController.player_health_point;//初始化生命值
            max_magic_point = StateController.player_max_magic_point;//初始化最大法力值
            magic_point = StateController.magic_point;//初始化魔法值
            update = true;
        }

        if (magic_consume)//如果消耗法力值
        {
            magic_point--;//法力值被消耗
            magic_consume = false;//设定法力值不被消耗
        }
        if (!dead && health_point <= 0)//如果玩家生命值小于0并且未死亡
        {
            foreach (MonoBehaviour scripts in GetComponentsInChildren<MonoBehaviour>())//获取玩家的所有脚本
            {
                Destroy(scripts);//销毁所有脚本
            }
            if (tag != "Untagged")
            {
                tag = "Untagged";//改变标签
            }
            Destroy(GetComponent<Rigidbody>());//销毁刚体
            GetComponent<Animator>().SetBool("dead", true);//播放死亡的动画
            /*关闭其他动画*/
            GetComponent<Animator>().SetBool("forward", false);
            GetComponent<Animator>().SetBool("backward", false);
            GetComponent<Animator>().SetBool("left", false);
            GetComponent<Animator>().SetBool("right", false);
            dead = true;//设定玩家未死亡
        }
    }

    /*法力值是否用完*/
    public bool OutofMagic()
    {
        return magic_point <= 0;//法力值小于等于0即用完
    }
}