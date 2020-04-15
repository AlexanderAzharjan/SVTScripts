using UnityEngine;
using UnityEngine.UI;

/*状态控制器，为玩家和敌人给予状态*/
public class StateController : MonoBehaviour {
    private GameObject player; //玩家

    [SerializeField] public static float player_max_health_point = 10; //玩家的最大生命值
    [SerializeField] public static float player_health_point = 10; //玩家的生命值

    [SerializeField] public static float player_max_magic_point = 9; //玩家的最大魔法值
    [SerializeField] public static float magic_point = 9; //玩家的魔法值

    [SerializeField] public static int enermy_attack_point = 1; //敌人的攻击值

    private static bool start_game = false; //是否第一次开始游戏

    public Image black_transition; //黑色渐变

    public Image[] count; //计时图片
    private float timer = 2; //计时器
    private bool show_3 = false; //是否展示数字3

    public Text level; //关卡
    private static int level_count = 0; //关卡号初始为0

    public Image[] state; //状态

    /*初始化*/
    private void Start () {
        level_count++; //关卡数自增

        if (!start_game) //除去第一次开始游戏
        {
            start_game = true;
            DontDestroyOnLoad (gameObject); //设定不随场景销毁
        } else {
            enermy_attack_point++; //每过一关敌人攻击值加一
            if (Random.Range (0, 2) == 0) //50%几率提高生命值或魔法值
            {
                player_max_health_point++; //每过一关玩家的最大生命值加一
            } else {
                player_max_magic_point++; //每过一关玩家的最大魔法值加一
            }
        }
        Time.timeScale = 0; //时间暂停
        black_transition.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Screen.width, Screen.height); //使黑色渐变满屏
    }

    /*每帧更新的部分*/
    private void Update () {
        if (black_transition && black_transition.color.a > 0) //如果黑色渐变
        {
            black_transition.color -= new Color (0, 0, 0, 0.02f); //黑色逐渐褪去
        } else {
            if (timer < 0) //计时结束
            {
                timer = 2; //计时器重置
                if (!show_3) //如果数字3未展示
                {
                    count[0].enabled = true; //展示数字3
                    show_3 = true; //设置数字3已经显示
                } else if (count[0] && count[0].enabled) //如果数字3显示
                {
                    count[1].enabled = !(count[0].enabled = false); //关闭数字3，开启数字2
                } else if (count[1] && count[1].enabled) //如果数字2显示
                {
                    count[2].enabled = !(count[1].enabled = false); //关闭数字2，开启数字1
                } else if (count[2] && count[2].enabled) //如果数字1显示
                {
                    count[2].enabled = false; //关闭数字1

                    timer = 10; //设定进入一个较长计时

                    Time.timeScale = 1; //时间恢复
                    foreach (MonoBehaviour scripts in player.GetComponentsInChildren<MonoBehaviour> ()) {
                        scripts.enabled = true; //开启玩家所有脚本
                    }
                }
            } else //计时中
            {
                timer -= 0.02f; //进行计时
                foreach (Image states in state) {
                    if (states && states.enabled && states.color.a < 1) //如果状态UI开启
                    {
                        states.color += new Color (0, 0, 0, Time.deltaTime);
                    }
                }
            }
        }

        if (player) //如果玩家存在
        {
            if (!player.GetComponent<PlayerStateChecker> ()) //如果玩家死亡
            {
                if (player.transform.position.y > -2.2f) {
                    player.transform.position -= new Vector3 (0, Time.deltaTime, 0); //减低玩家的高度
                }
            } else if (player.GetComponent<PlayerStateChecker> ().update) //如果玩家未死亡
            {
                magic_point = player.GetComponent<PlayerStateChecker> ().magic_point; //记录玩家的魔法值
                player_health_point = player.GetComponent<PlayerStateChecker> ().health_point; //记录玩家的生命值
            }
        } else //如果玩家不存在
        {
            player = GameObject.FindWithTag ("Player"); //找到玩家
            foreach (MonoBehaviour scripts in player.GetComponentsInChildren<MonoBehaviour> ()) {
                scripts.enabled = false; //关闭玩家所有脚本
            }
        }

        if (level) {
            level.text = "Level: " + level_count; //关卡数自增
        }

        if (Input.GetKeyUp (KeyCode.Escape)) //按下ESC退出游戏
        {
            Application.Quit (); //退出游戏
        }
    }
}