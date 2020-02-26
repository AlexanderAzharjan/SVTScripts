using UnityEngine;
public class AbilityController : ShotController/*玩家能力控制器，继承ShotController*/
{
    public GameObject[] bullets;//子弹
    private GameObject current_bullet;//当前的子弹
    private int current_bullet_number = 0;//当前子弹序号

    /*每帧更新的部分*/
    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0)//如果鼠标上滚
        {
            current_bullet_number = (current_bullet_number == 0) ? (bullets.Length - 1) : (current_bullet_number - 1);//如果当前子弹为第一个，则选择最后一种子弹，否则选择上一种
        }
        else if (Input.mouseScrollDelta.y < 0)//如果鼠标下滚
        {
            current_bullet_number = (current_bullet_number == (bullets.Length - 1)) ? 0 : (current_bullet_number + 1);//如果当前子弹为最后一个，则选择第一种子弹，否则选择下一种
        }
        if (Input.GetMouseButtonUp(0) && !GameObject.FindGameObjectWithTag("Bullets"))//如果鼠标左键按下后抬起并且没有子弹存在
        {
            if (GetComponent<PlayerStateChecker>().OutofMagic())//如果玩家没有有法力值
            {
                if (GetComponent<AudioSource>().clip.name != "OutofBullet")//如果声音名字不是OutofBullet
                {
                    GetComponent<AudioSource>().clip = (AudioClip)Resources.Load("Audio/Attack/OutofBullet");//将声音替换为OutofBullet
                }
                if (!GetComponent<AudioSource>().isPlaying)//如果声音没有在播放
                {
                    GetComponent<AudioSource>().Play();//播放声音
                }
            }
            else//如果玩家还有法力值
            {
                if (GetComponent<AudioSource>().clip.name != "Shot")//如果声音名字不是Shot
                {
                    GetComponent<AudioSource>().clip = (AudioClip)Resources.Load("Audio/Attack/Shot");//将声音替换为Shot
                }
                if (!GetComponent<AudioSource>().isPlaying)//如果声音没有在播放
                {
                    GetComponent<AudioSource>().Play();//播放声音
                }
                GetComponent<PlayerStateChecker>().magic_consume = true;//设置消耗魔法值
                Shot(bullets[current_bullet_number]);//进行射击
            }
        }
        if (Input.GetMouseButtonUp(1) && (current_bullet = GameObject.FindGameObjectWithTag("Bullets")) && current_bullet.GetComponent<MonoBehaviour>().enabled)//如果按下鼠标右键并且发现了子弹并且子弹没有被时停
        {
            current_bullet.GetComponent<BulletsAbilityController>().ActivateAbility();//启用子弹的能力
        }
    }
}