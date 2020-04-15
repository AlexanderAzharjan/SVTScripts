using UnityEngine;
public class BulletsMoveController : MonoBehaviour /*子弹移动控制*/ {

    public float move_speed; //移动速度

    /*子弹移动*/
    public void Move () {
        if (GetComponent<Rigidbody> ()) //如果子弹刚体存在
        {
            GetComponent<Rigidbody> ().AddForce (transform.forward * move_speed); //给子弹正前方向一个力
        }
    }
}