using UnityEngine;
public class PlayerMoveController : MonoBehaviour
{
    private float input_horizontal,input_vertical;//玩家横向与纵向的按键返回值

    public float move_speed;//玩家的移动速度
    public float x_min, x_max, z_min, z_max;//玩家的移动边界
    private enum OutBound { x_min, x_max, z_min, z_max, none };//超出边界的枚举
    [HideInInspector] public bool hit_when_transfer = false;//玩家是否在瞬移时撞到了任何碰撞体

    /*每帧更新的部分*/
    private void Update()
    {
        Move();
    }

    /*玩家移动*/
    private void Move()
    {
        if (!SpaceBulletController.transfer || SpaceBulletController.effected_by_time)//如果玩家没有在瞬移或者空间弹受到时停影响
        {
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))) * move_speed;//玩家进行移动
        }
        switch (IsMovable())//查看是否超出了边界，超出时进行限制
        {
            case OutBound.x_max:
                transform.position = new Vector3(x_max, transform.position.y, transform.position.z);
                break;
            case OutBound.x_min:
                transform.position = new Vector3(x_min, transform.position.y, transform.position.z);
                break;
            case OutBound.z_max:
                transform.position = new Vector3(transform.position.x, transform.position.y, z_max);
                break;
            case OutBound.z_min:
                transform.position = new Vector3(transform.position.x, transform.position.y, z_min);
                break;
        }
    }

    /*是否在规定范围*/
    private OutBound IsMovable()
    {
        if (transform.position.x < x_min)//超出左边界
        {
            return OutBound.x_min;
        }
        else if (transform.position.x > x_max)//超出右边界
        {
            return OutBound.x_max;
        }
        else if (transform.position.z < z_min)//超出下边界
        {
            return OutBound.z_min;
        }
        else if (transform.position.z > z_max)//超出上边界
        {
            return OutBound.z_max;
        }
        return OutBound.none;
    }

    /*当玩家撞到了任何碰撞体*/
    private void OnCollisionEnter(Collision collision)
    {
        if (SpaceBulletController.transfer && collision.collider.tag != "Plane")//如果玩家正在瞬移并且撞到的不是地面
        {
            hit_when_transfer = true;//设定瞬移时撞到了碰撞体
        }
    }
}