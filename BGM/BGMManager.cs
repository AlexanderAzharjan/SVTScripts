using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour /*背景音乐管理器*/ {
    private List<GameObject> enermies = new List<GameObject> (); //记录所有敌人
    private bool change_to_danger = false; //是否播放danger音乐
    private float timer = 0.2f; //计时器

    /*每帧更新的部分*/
    private void Update () {
        change_to_danger = false; //假设不播放Danger音乐
        if (enermies.Count == 0) //如果没有记录敌人
        {
            enermies.AddRange (GameObject.FindGameObjectsWithTag ("Enermy")); //记录所有敌人
        }
        foreach (GameObject enermy in enermies) //遍历所有记录的敌人
        {
            if (enermy == null) //如果找不到该敌人，即敌人已经被消灭
            {
                enermies.Remove (enermy); //将此敌人移除
                break; //跳过此敌人
            }
            if (enermy.GetComponent<AlarmController> ().saw_player) //如果敌人看到了敌人
            {
                change_to_danger = true; //改为播放Danger音乐
                break; //跳出本次循环
            }
        }
        ChangeBGM (change_to_danger);
    }

    /*播放BGM*/
    void ChangeBGM (bool change_to_danger) //change_to_danger为是否切换为Danger音乐
    {
        if (change_to_danger) //如果需要切换
        {
            if (GetComponent<AudioSource> ().clip.name != "Danger") //如果背景音效不是Danger的话
            {
                GetComponent<AudioSource> ().volume = 0.2f; //降低音量
                GetComponent<AudioSource> ().clip = (AudioClip) Resources.Load ("Audio/BackGround/Danger"); //替换背景音效为Danger
            }
        } else //如果不需要切换
        {
            if (timer > 0 && GetComponent<AudioSource> ().clip.name != "BGM") //计时未结束并且背景音乐不是BGM
            {
                GetComponent<AudioSource> ().volume -= Time.deltaTime * 0.05f; //逐步降低声音
                timer -= Time.deltaTime * 0.051f; //进行计时
            } else //计时结束
            {
                if (GetComponent<AudioSource> ().clip.name != "BGM") //如果背景音效不是BGM的话
                {
                    GetComponent<AudioSource> ().clip = (AudioClip) Resources.Load ("Audio/BackGround/BGM"); //替换背景音效为BGM
                }
                if (GetComponent<AudioSource> ().volume < 0.8f) //逐步提高音量
                {
                    GetComponent<AudioSource> ().volume += Time.deltaTime; //提高音量
                } else //音量提高结束
                {
                    timer = 0.2f; //重置计时器
                }
            }
        }
        if (!GetComponent<AudioSource> ().isPlaying) //如果音乐没有在播放
        {
            GetComponent<AudioSource> ().Play (); //播放音乐
        }
    }
}