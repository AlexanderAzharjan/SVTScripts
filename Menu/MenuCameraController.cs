using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuCameraController : MonoBehaviour/*控制主菜单的相机移动*/
{
    public GameObject plane_light;//地板灯光

    /*每帧更新的部分*/
    private void Update()
    {
        transform.localEulerAngles = new Vector3(0, (Input.mousePosition.x - Screen.width / 2) / 50, 0);//随鼠标进行视角旋转
        if(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)//如果动画播放完毕
        {
            SceneManager.LoadScene("SVT");//切换场景
        }
    }

    /*播放动画*/
    public void PlayAnimation()
    {
        GetComponent<Animator>().enabled = true;//播放动画
        plane_light.GetComponent<PlaneLightController>().start_game = true;//灯光逐渐变暗
    }
}