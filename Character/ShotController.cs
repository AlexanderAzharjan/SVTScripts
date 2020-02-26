using UnityEngine;
public class ShotController : MonoBehaviour/*射击控制*/
{
    public Transform bullet_start;//子弹初始位置

    /*射击*/
    public void Shot(GameObject bullet)//bullet为具体子弹
    {
        Instantiate(bullet, bullet_start.position, bullet_start.rotation);//在初始位置实例化子弹
    }
}