using UnityEngine;
public class FollowSmokeController : MonoBehaviour /*跟随烟雾控制器*/ {
    /*初始化*/
    private void Start () {
        transform.parent = GameObject.FindWithTag ("Player").transform; //成为玩家的子物体
    }

    /*每帧更新的部分*/
    private void Update () {
        if (transform.parent) //如果与玩家进行了绑定
        {
            if (SpaceBulletController.effected_by_time) //如果时间弹消失
            {
                transform.parent = null; //脱离父子关系
            }
        } else //如果与玩家解除了绑定
        {
            if (transform.Find ("Smoke").GetComponent<ParticleSystem> ().particleCount == 1) //如果烟雾效果消失
            {
                Destroy (gameObject); //销毁该特效
            }
        }
    }
}