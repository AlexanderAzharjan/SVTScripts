using UnityEngine;
public class CameraController : MonoBehaviour/*使相机追踪玩家*/
{
    private GameObject player;//玩家
    public Transform camera_transform;//相机位置

    /*初始化*/
    private void Start()
    {
        Cursor.visible = false;//关闭指针显示
        player = GameObject.FindGameObjectWithTag("Player");//找到玩家
    }

    /*每帧更新的部分*/
    private void Update()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)//玩家没有在移动或旋转
        {
            transform.RotateAround(player.transform.position, Vector3.up, Input.GetAxis("Mouse X"));//摄像机随鼠标进行水平旋转
        }
        else//如果玩家在移动或旋转
        {
            if(transform.localEulerAngles.y > 1 && transform.localEulerAngles.y < 180)
            {
                transform.RotateAround(player.transform.position, Vector3.up, -1);//摄像机随鼠标进行水平旋转
            }
            else if (transform.localEulerAngles.y < 364 && transform.localEulerAngles.y > 180)
            {
                transform.RotateAround(player.transform.position, Vector3.up, 1);//摄像机随鼠标进行水平旋转
            }
        }
    }
}