using UnityEngine;

public class VibrationBulletController : BulletsMoveController, BulletsAbilityController /*振动弹控制器，继承BulletsMoveController，实现BulletsAbilityController*/ {

    private GameObject copy_left; //本体的左复制体
    private GameObject copy_right; //本体的右复制体
    private bool copied = false; //是否已经进行了复制
    private bool copy_left_active; //做复制体是否被激活

    public float vibrate_range; //振动幅度
    public float vibrate_frequence; //振动频率
    private float timer; //计时器
    public GameObject attach_place; //附着点

    private bool activable = false; //是否可被激活
    private bool actived = false; //是否被激活

    private GameObject vibrate_object; //需要振动的物体

    public float vibrate_time; //可以振动的最长时间

    /*初始化*/
    private void Start () {
        timer = vibrate_frequence; //设定计时器
    }

    /*脚本被启用时*/
    private void OnEnable () {
        Move (); //移动
    }

    /*每帧更新的部分*/
    private void Update () {
        if (!actived) //如果没有被激活
        {
            Vibrate (gameObject); //自己进行振动
        } else //如果被激活
        {
            Vibrate (vibrate_object); //振动附着上的物体的根物体
            transform.position = vibrate_object.transform.position; //始终跟随附着上的物体
            if (vibrate_time > 0) //若计时未结束
            {
                vibrate_time -= Time.deltaTime; //计时vibrate_times
            } else //若计时结束
            {
                foreach (Renderer renders in vibrate_object.GetComponentsInChildren<Renderer> ()) //对于振动的物体及其子物体的渲染器
                {
                    renders.enabled = true; //设定可见
                }
                foreach (Collider colliders in vibrate_object.GetComponentsInChildren<Collider> ()) //对于振动的物体及其子物体的碰撞器
                {
                    colliders.isTrigger = false; //关闭触发器
                }
                Destroy (copy_left); //销毁左复制体
                Destroy (copy_right); //销毁右复制体
                Destroy (attach_place); //销毁附着点
                Destroy (gameObject); //销毁子弹
            }
        }
    }

    /*振动弹与碰撞体相遇*/
    private void OnCollisionEnter (Collision collision) {
        if (GetComponent<MonoBehaviour> ().enabled && collision.gameObject.tag != "UnthroughableBlock" && collision.gameObject.tag != "EnermyBullet" && collision.gameObject.tag != "Enermy") //如果振动弹脚本存在，除去不可穿越的阻碍物以及敌人的子弹，以及玩家和爹
        {
            Destroy (GetComponent<Rigidbody> ()); //销毁刚体组件，使子弹停下
            GetComponent<Collider> ().enabled = false; //子弹碰撞器禁用
            vibrate_object = collision.transform.root.gameObject; //获取被碰撞的对象的根物体
            attach_place = Instantiate (attach_place, collision.contacts[0].point, Quaternion.identity); //在碰撞点实例化一个附着点
            attach_place.transform.parent = vibrate_object.transform; //设定附着点为被碰撞对象根物体的子物体
            transform.parent = attach_place.transform; //设定子弹是附着点的子物体
            activable = true; //设定可以被激活
        }
    }

    /*振动*/
    private void Vibrate (GameObject game_object) //game_object为要振动的物体
    {
        if (!copied) //如果没有进行过复制
        {
            Copy (game_object); //复制物体
            copied = true; //设定已经复制了物体
        }
        if (!copy_left_active) //如果左复制体不可见
        {
            if (Timing ()) //进行计时，如果计时结束
            {
                foreach (Renderer renderer in copy_left.GetComponentsInChildren<Renderer> ()) //对于左复制体及其子物体
                {
                    renderer.enabled = true; //设置为可见
                }
                foreach (Renderer renderer in copy_right.GetComponentsInChildren<Renderer> ()) //对于右复制体及其子物体
                {
                    renderer.enabled = false; //设置为不可见
                }
                copy_left_active = true; //设定左复制体可见
            }
        } else //如果右复制体不可见
        {
            if (Timing ()) //进行计时，如果计时结束
            {
                foreach (Renderer renderer in copy_left.GetComponentsInChildren<Renderer> ()) //对于左复制体及其子物体
                {
                    renderer.enabled = false; //设置为不可见
                }
                foreach (Renderer renderer in copy_right.GetComponentsInChildren<Renderer> ()) //对于右复制体及其子物体
                {
                    renderer.enabled = true; //设置为可见
                }
                copy_left_active = false; //设定左复制体不可见
            }
        }
    }

    /*复制本体*/
    private void Copy (GameObject game_object) {
        copy_left = Instantiate (game_object); //复制本体到左复制体
        copy_right = Instantiate (game_object); //复制本体到右复制体
        if (game_object.tag == "Enermy") //如果复制对象是敌人
        {
            copy_left.GetComponent<PlayerController> ().SetArsenal ("Rifle"); //设置左复制体玩家的枪
            copy_right.GetComponent<PlayerController> ().SetArsenal ("Rifle"); //设置右复制体玩家的枪
        }
        if (game_object == gameObject) //对于子弹本身
        {
            GetComponent<Renderer> ().enabled = false; //不进行渲染
        } else //对于要振动的物体
        {
            foreach (Renderer renderer in game_object.GetComponentsInChildren<Renderer> ()) //对于本体及其子物体
            {
                renderer.enabled = false; //设置为不可见
            }
        }
        RemoveComponents (copy_left); //移除左复制体组件
        RemoveComponents (copy_right); //移除右复制体组件
        copy_left.transform.position += new Vector3 (vibrate_range, 0, 0); //移动左复制体到正确位置
        copy_right.transform.position -= new Vector3 (vibrate_range, 0, 0); //移动右复制体到正确位置
        copy_right.transform.parent = copy_left.transform.parent = game_object.transform; //设定左、右复制体为本体子对象
        foreach (Renderer renderer in copy_left.GetComponentsInChildren<Renderer> ()) //对于左复制体及其子物体
        {
            renderer.enabled = false; //设置为不可见
        }
        copy_left_active = false; //设定左复制体没有被激活
    }

    /*移除组件*/
    private void RemoveComponents (GameObject game_object_copy) //game_object_copy是复制体
    {
        foreach (Component components in game_object_copy.GetComponentsInChildren<Component> ()) //对于复制体及其在同一个树分支下的所有组件
        {
            if (!(components.GetType ().ToString () == "UnityEngine.Transform") && !(components.GetType ().ToString () == "UnityEngine.MeshRenderer") && !(components.GetType ().ToString () == "UnityEngine.SkinnedMeshRenderer") && !(components.GetType ().ToString () == "UnityEngine.MeshFilter") && !(components.GetType ().ToString () == "UnityEngine.Animator") && !(components.GetType ().ToString () == "PlayerAnimatorController")) //除去变换组件、网格过滤器与渲染器以及人物动画控制器
            {
                Destroy (components); //销毁这个组件
            }
        }
    }

    /*计时*/
    private bool Timing () {
        if (timer > 0) //进行计时
        {
            timer -= Time.deltaTime; //计时vibrate_frequence秒
            return false;
        } else //计时结束
        {
            timer = vibrate_frequence; //重置计时器
            return true;
        }
    }

    /*激活能力*/
    void BulletsAbilityController.ActivateAbility () {
        if (activable) //如果可被激活
        {
            foreach (Component components in GetComponentsInChildren<Component> ()) //对于子弹本体及其子物体
            {
                if (!(components.GetType ().ToString () == "UnityEngine.Transform") && !(components.GetType ().ToString () == "VibrationBulletController")) //除去变换组件与VibrationBulletController脚本
                {
                    Destroy (components); //销毁这个组件
                }
            }
            transform.tag = "Untagged"; //设定标签为Untagged
            transform.parent = null; //解除与物体的父子关系
            Destroy (copy_left); //销毁左复制体
            Destroy (copy_right); //销毁右复制体
            copied = false; //设定还没有复制物体
            actived = true; //设定已经激活
            activable = false; //设定子弹不可再被激活
            foreach (Collider colliders in vibrate_object.GetComponentsInChildren<Collider> ()) //对于被附着物体以及其子物体的碰撞体
            {
                colliders.isTrigger = true; //使碰撞体变为触发器
            }
        }
    }
}