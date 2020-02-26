using UnityEngine;
using UnityEngine.UI;
public class Magic_UIController : MonoBehaviour/*魔法值UI控制器*/
{
    private GameObject player;//玩家

    /*每帧更新的部分*/
    private void Update()
    {
        if (!player)//如果没有找到玩家
        {
            player = GameObject.FindWithTag("Player");//找到玩家
        }
        else if (player.tag == "Player")//如果找到了玩家并且玩家没有死亡
        {
            GetComponent<Image>().fillAmount = player.GetComponent<PlayerStateChecker>().magic_point / player.GetComponent<PlayerStateChecker>().max_magic_point;//让魔方值UI随着玩家魔法值进行变化
        }
    }
}
