using UnityEngine;
using UnityEngine.UI;
public class Letter_RotateController : MonoBehaviour/*字母旋转控制器*/
{
    public Transform[] letter_transform;//字母可能的位置
    public enum LetterPosition { left, middle, right };//字母位置枚举
    public LetterPosition current_position;//当前的位置

    /*每帧更新的部分*/
    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0)//如果鼠标上滚
        {
            /*切换位置*/
            switch (current_position)
            {
                case LetterPosition.left:
                    current_position = LetterPosition.middle;
                    break;
                case LetterPosition.middle:
                    current_position = LetterPosition.right;
                    break;
                case LetterPosition.right:
                    current_position = LetterPosition.left;
                    break;
            }
        }
        else if(Input.mouseScrollDelta.y < 0)//如果鼠标下滚
        {
            /*切换位置*/
            switch (current_position)
            {
                case LetterPosition.left:
                    current_position = LetterPosition.right;
                    break;
                case LetterPosition.middle:
                    current_position = LetterPosition.left;
                    break;
                case LetterPosition.right:
                    current_position = LetterPosition.middle;
                    break;
            }
        }

        if(letter_transform.Length == 3)//如果已经记录好数组的长度
        {
            transform.position = Vector3.MoveTowards(transform.position, letter_transform[(int)current_position].position, 3);//移动字母的位置
        }
        if(current_position == LetterPosition.middle)//如果字母在中间位置
        {
            GetComponent<Image>().enabled = true;//设定字母可见
        }
        else//如果字母不在中间位置
        {
            GetComponent<Image>().enabled = false;//设定字母不可见
        }
    }
}