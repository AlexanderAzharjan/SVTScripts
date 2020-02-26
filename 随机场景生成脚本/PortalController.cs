using UnityEngine;
using UnityEngine.SceneManagement;
public class PortalController : MonoBehaviour/*传送门控制器*/
{
    /*当玩家在传送门当中*/
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyUp(KeyCode.T))//如果玩家按下T键
        {
            SceneManager.LoadScene("SVT");//重新载入关卡
        }
    }
}