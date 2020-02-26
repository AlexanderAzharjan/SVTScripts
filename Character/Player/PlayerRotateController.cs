using UnityEngine;
public class PlayerRotateController : MonoBehaviour/*控制玩家旋转*/
{
    public float sensitivity;//人物旋转的敏感度

    /*初始化*/
    private void Start()
    {
        Cursor.visible = false;//关闭指针显示
    }

    /*每帧更新的部分*/
    private void Update()
    {
        transform.localEulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * sensitivity, 0);//人物随着鼠标进行旋转
    }
}