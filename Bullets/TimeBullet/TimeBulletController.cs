using UnityEngine;
using System.Collections.Generic;
public class TimeBulletController : BulletsMoveController, BulletsAbilityController/*时间弹控制器，继承BulletsMoveController，实现BulletsAbilityController*/
{
    private bool active = false;//子弹是否被激活
    public float scale_speed;//子弹的放大速度
    public float max_size;//子弹的最大放大倍数
    public float color_speed;//子弹的颜色改变速度
    public float gray_level;//灰度
    public float alpha_level;//透明度
    private List<GameObject> game_objects_in_sphere = new List<GameObject>();//在场景中的所有物体
    private GameObject game_object_in_sphere;//在场景中的某个物体
    public float pause_time;//子弹效果持续时间
    private bool audio_changed = false;//是否改变了声音
    public GameObject time_bullet_vfx;//时间弹特效

    /*初始化*/
    private void Start()
    {
        GetComponent<Renderer>().material.color = new Color(gray_level, gray_level, gray_level, alpha_level);//设定初始颜色
    }

    /*脚本启用时*/
    private void OnEnable()
    {
        Move();//移动
    }

    /*每帧更新的部分*/
    private void Update()
    {
        if (active && pause_time > 0)//如果子弹被激活并且效果未结束
        {
            if (transform.position.y > -2.3f)//如果子弹高度大于2.3f
            {
                transform.position -= new Vector3(0, Time.deltaTime, 0);//降低子弹高度
            }
            if (tag != "Untagged")//如果子弹标签不是"Untagged"
            {
                tag = "Untagged";//设定子弹标签是"Untagged"
            }
            if (transform.localScale.y < max_size)//子弹放大未结束
            {
                transform.localScale += Vector3.one * scale_speed;//以每帧scale_speed的速度放大子弹
            }
            else//放大结束
            {
                foreach (Collider colliders in Physics.OverlapSphere(transform.position, max_size / 2))//找到球内所有碰撞体
                {
                    if (!game_objects_in_sphere.Contains(game_object_in_sphere = colliders.gameObject) && game_object_in_sphere != gameObject && game_object_in_sphere.tag != "Player")//除去时间弹、玩家，获取碰撞体对应的物体，如果没有被记录
                    {
                        game_objects_in_sphere.Add(game_object_in_sphere);//记录这个物体
                        if (game_object_in_sphere.GetComponent<Rigidbody>() && !game_object_in_sphere.GetComponent<Rigidbody>().isKinematic)//如果物体包含刚体且物体不是运动学的
                        {
                            game_object_in_sphere.GetComponent<Rigidbody>().isKinematic = true;//让物体停下来
                        }
                        if (game_object_in_sphere.GetComponent<Animator>())//如果物体包含动画组件
                        {
                            game_object_in_sphere.GetComponent<Animator>().SetFloat("speed", 0);//停止播放动画
                        }
                        foreach (MonoBehaviour scripts in game_object_in_sphere.GetComponents<MonoBehaviour>())//对于其中物体，找到它的所有脚本
                        {
                            scripts.enabled = false;//禁用这些脚本
                        }
                    }
                }
                pause_time -= Time.deltaTime;//子弹进行计时
            }
        }
        if(pause_time < 0)//如果子弹计时结束
        {
            if (!audio_changed)//如果音频没有改变
            {
                GetComponent<AudioSource>().clip = (AudioClip)Resources.Load("Audio/Bullets/TimeBulletReverse");//切换音频
                audio_changed = true;//设定改变完成
            }
            if (!GetComponent<AudioSource>().isPlaying)//如果音效没有在播放
            {
                GetComponent<AudioSource>().Play();//播放音效
            }

            if (transform.localScale.y > 0)//子弹缩小未结束
            {
                transform.localScale -= Vector3.one * scale_speed;//以每帧scale_speed的速度缩小子弹
            }
            else//子弹缩小结束
            {
                foreach (GameObject game_object_in_sphere in game_objects_in_sphere)//对于记录过的物体
                {
                    if (game_object_in_sphere.GetComponent<Rigidbody>())//如果物体包含刚体
                    {
                        game_object_in_sphere.GetComponent<Rigidbody>().isKinematic = false;//禁用物体的运动学
                    }
                    if (game_object_in_sphere.GetComponent<Animator>())//如果物体包含动画组件
                    {
                        game_object_in_sphere.GetComponent<Animator>().SetFloat("speed", 1);//继续播放动画
                    }
                    foreach (MonoBehaviour scripts in game_object_in_sphere.GetComponents<MonoBehaviour>())//对于其中物体，找到它的所有脚本
                    {
                        scripts.enabled = true;//开启这些脚本
                    }
                }
                Destroy(gameObject);//销毁子弹本身
            }
        }
    }

    /*激活能力*/
    void BulletsAbilityController.ActivateAbility()
    {
        if (!time_bullet_vfx.activeSelf)//如果时间弹特效没有被激活
        {
            time_bullet_vfx.SetActive(true);//激活时间弹特效
        }
        if (!active)//如果子弹没有被激活
        {
            active = true;//设定子弹被激活
            Destroy(GetComponent<Rigidbody>());//移除子弹的刚体组件
            Destroy(GetComponent<Collider>());//移除子弹的碰撞体组件
            if (!GetComponent<AudioSource>().isPlaying)//如果音频没在播放
            {
                GetComponent<AudioSource>().Play();//播放时停音频
            }
        }
    }
}