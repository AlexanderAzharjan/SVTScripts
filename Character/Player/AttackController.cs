using UnityEngine;
public class AttackController : MonoBehaviour/*攻击控制器*/
{
    [HideInInspector] public bool attack_enermy = false;//是否攻击到敌人
    public Transform detect_start;//检测起点
    public float max_degree;//检测的最大角度
    private RaycastHit hit_info;//射线击中信息
    public float attack_max_distance;//攻击的最大长度

    /*每帧更新的部分*/
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))//如果按下空格之后抬起
        {
            GetComponent<Animator>().SetBool("use_knife", true);
            if (GetComponent<AudioSource>().clip.name != "Knife")//如果音效不是Knife
            {
                GetComponent<AudioSource>().clip = (AudioClip)Resources.Load("Audio/Attack/Knife");//切换音效为Knife
            }
            if (!GetComponent<AudioSource>().isPlaying)//如果音效没有在播放
            {
                GetComponent<AudioSource>().Play();//播放音效
            }
            for (detect_start.localEulerAngles = new Vector3(0, 360 - max_degree, 0); detect_start.localEulerAngles.y >= (360 - max_degree) || detect_start.localEulerAngles.y <= max_degree; detect_start.localEulerAngles += new Vector3(0, 1, 0))//在玩家左右70度范围内
            {
                if (Physics.Raycast(detect_start.position, detect_start.forward, out hit_info, attack_max_distance) && hit_info.collider.tag == "Enermy")//如果探测到了敌人
                {
                    attack_enermy = true;//设定攻击到敌人
                    hit_info.collider.gameObject.GetComponent<EnermyStateController>().Killed();//敌人生命值减一
                    break;
                }
            }
        }
        if (GetComponent<Animator>().GetBool("use_knife") && GetComponent<Animator>().GetCurrentAnimatorClipInfo(1).Length == 1)//如果击打动画播放完毕
        {
            GetComponent<Animator>().SetBool("use_knife", false);//关闭攻击动画
        }
    }  
}
