using UnityEngine;
public class SpaceBulletController : BulletsMoveController, BulletsAbilityController /*空间弹控制器，继承BulletsMoveController，实现BulletsAbilityController*/ {
    private GameObject player; //玩家
    public float transfer_speed; //瞬移速度
    public static bool transfer = false; //是否进行瞬移
    public static bool effected_by_time = false; //是否受到了时间弹影响
    public GameObject follow_smoke; //跟随的烟雾
    public GameObject space_bullet_vfx; //空间弹特效

    /*脚本被启用时*/
    private void OnEnable () {
        effected_by_time = false; //设定没有受到时间影响
        Move (); //移动
    }

    /*每帧更新的部分*/
    private void Update () {
        Transfer ();
    }

    /*脚本被禁用时*/
    private void OnDisable () {
        effected_by_time = true; //设定了受到了时间弹影响
    }

    /*瞬移*/
    private void Transfer () {
        if (transfer) //如果确认进行瞬移
        {
            if (GetComponent<AudioSource> ().clip.name != "Transfer") //如果声音名字不是Transfer
            {
                GetComponent<AudioSource> ().clip = (AudioClip) Resources.Load ("Audio/Bullets/Transfer"); //将声音替换为Transfer
                GetComponent<AudioSource> ().volume = 0.05f; //降低音量
                GetComponent<AudioSource> ().pitch = 1.5f; //提高音调
            }
            if (!GetComponent<AudioSource> ().isPlaying && GetComponent<AudioSource> ().loop) {
                GetComponent<AudioSource> ().loop = false; //关闭循环播放
                GetComponent<AudioSource> ().Play (); //播放声音
            }

            if (GetComponent<Rigidbody> ().velocity != Vector3.zero) //如果子弹没有停下
            {
                GetComponent<Rigidbody> ().velocity = Vector3.zero; //让子弹停下
            }
            player.GetComponent<Rigidbody> ().velocity = (transform.position - player.transform.position) * transfer_speed; //瞬移玩家

            if ((transform.position - player.transform.position).sqrMagnitude < 15 || player.GetComponent<PlayerMoveController> ().hit_when_transfer) //如果玩家与子弹之间距离足够小或者玩家撞到了任何碰撞体
            {
                transfer = false; //确认停止瞬移

                space_bullet_vfx.transform.parent = null; //脱离父子关系
                foreach (Transform space_bullet_vfxs in space_bullet_vfx.GetComponentsInChildren<Transform> ()) //对于粒子效果
                {
                    ParticleSystem.MainModule main_module = space_bullet_vfxs.GetComponent<ParticleSystem> ().main;
                    main_module.loop = false; //停止粒子循环
                }
                Destroy (space_bullet_vfx, 4.5f); //4.5s后销毁物体

                player.GetComponent<PlayerMoveController> ().hit_when_transfer = false; //设定现在玩家没有在瞬移
                Destroy (gameObject); //销毁子弹
            }
        }
    }

    /*激活能力*/
    void BulletsAbilityController.ActivateAbility () {
        transfer = true; //确认进行瞬移

        player = GameObject.FindGameObjectWithTag ("Player"); //找到玩家

        Instantiate (follow_smoke, player.transform.position, Quaternion.identity); //在玩家位置实例化烟雾

        GetComponent<Collider> ().enabled = false; //设定空间弹无碰撞效果
        GetComponent<Rigidbody> ().velocity = Vector3.zero; //让子弹停下
    }
}