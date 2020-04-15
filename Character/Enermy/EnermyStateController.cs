using UnityEngine;

/*敌人状态控制器*/
public class EnermyStateController : MonoBehaviour {

    private GameObject player; //玩家
    [HideInInspector] public float health_point = 1; //生命值
    private bool dead = false; //是否死亡

    public GameObject health_supply, magic_supply; //生命供给或法力供给

    /*初始化*/
    private void Start () {
        GetComponent<PlayerController> ().SetArsenal ("Rifle"); //设置玩家的枪
        player = GameObject.FindGameObjectWithTag ("Player"); //找到玩家
    }

    /*每帧更新的部分*/
    private void Update () {
        if (health_point <= 0) //如果敌人生命值降为0以下
        {
            GetComponentInChildren<Light> ().enabled = false; //关闭灯光
            if (!dead) //如果敌人没有确认死亡
            {
                GetComponent<AlarmController> ().saw_player = false; //设定没有看到敌人
                foreach (MonoBehaviour scripts in GetComponentsInChildren<MonoBehaviour> ()) //对于其中物体，找到它的所有脚本
                {
                    scripts.enabled = false; //禁用这些脚本
                }
                Destroy (GetComponent<Rigidbody> ()); //销毁刚体组件
                Destroy (GetComponent<Collider> ()); //销毁碰撞体
                GetComponent<Actions> ().Death (); //播放死亡动画
                dead = true; //设定敌人已经死亡

                if (Random.Range (0, 2) == 0) //随机生成一个生命或法力补给
                {
                    Instantiate (health_supply, transform.position + new Vector3 (0, 1, 0), Quaternion.Euler (new Vector3 (45, 45, 0)));
                } else {
                    Instantiate (magic_supply, transform.position + new Vector3 (0, 1, 0), Quaternion.Euler (new Vector3 (45, 45, 0)));
                }
            }
            Destroy (gameObject, 10); //10s后敌人消失
        }
    }

    /*被杀死*/
    public void Killed () {
        health_point--; //生命值减一
    }
}