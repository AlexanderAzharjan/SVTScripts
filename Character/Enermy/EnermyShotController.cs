using UnityEngine;

/*敌人射击控制器，继承ShotController*/
public class EnermyShotController : ShotController {

    public GameObject enermy_bullet; //敌人子弹
    public float shot_speed; //敌人射速
    private float timer; //计时器

    /*每帧更新的部分*/
    void Update () {
        if (transform.root.GetComponent<DetectObjects> ().detected_player) //如果发现了玩家
        {
            if (timer < 0) //若计时结束
            {
                transform.root.GetComponent<Actions> ().Attack (); //攻击玩家
                Shot (enermy_bullet); //射击子弹
                timer = 1 / shot_speed; //计时器重置
            } else {
                timer -= Time.deltaTime; //开始计时
            }
        }
    }
}