using UnityEngine;
public class PlaneLightController : MonoBehaviour/*平面灯光控制器*/
{
    public bool start_game = false;//是否开始了游戏

    /*每帧更新的部分*/
    private void Update()
    {
        transform.position = new Vector3((Input.mousePosition.x - Screen.width / 2) / 30, (Input.mousePosition.y - Screen.height / 2) / 15, 45);//灯光随着鼠标进行移动
        if (start_game)
        {
            GetComponent<Light>().intensity -= Time.deltaTime;//灯光强度递减
        }
    }
}