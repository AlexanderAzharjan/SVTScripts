using UnityEngine;
public class PlayerAnimatorController : MonoBehaviour /*玩家动画器控制器*/ {
    private float input_horizontal, input_vertical; //玩家的键盘横向或纵向输入
    private Animator animator; //玩家的动画控制器
    private enum InputCondition { forward, forward_right, right, backward_right, backward, backward_left, left, forward_left, none } //输入情况枚举
 private InputCondition current_state; //当前状态

 /*初始化*/
 private void Start () {
 animator = GetComponent<Animator> (); //获取玩家的动画器
    }

    /*每帧更新的部分*/
    private void Update () {
        current_state = InputState (); //获取当前输入状态

        if (current_state == InputCondition.forward) /*关闭除了前进的所有动画*/ {
            if (!animator.GetBool ("forward")) {
                animator.SetBool ("forward", true);
            }
            if (animator.GetBool ("backward")) {
                animator.SetBool ("backward", false);
            }
            if (animator.GetBool ("left")) {
                animator.SetBool ("left", false);
            }
            if (animator.GetBool ("right")) {
                animator.SetBool ("right", false);
            }
        } else if (current_state == InputCondition.backward) /*关闭除了后退的所有动画*/ {
            if (animator.GetBool ("forward")) {
                animator.SetBool ("forward", false);
            }
            if (!animator.GetBool ("backward")) {
                animator.SetBool ("backward", true);
            }
            if (animator.GetBool ("left")) {
                animator.SetBool ("left", false);
            }
            if (animator.GetBool ("right")) {
                animator.SetBool ("right", false);
            }
        } else if (current_state == InputCondition.left || current_state == InputCondition.forward_left || current_state == InputCondition.backward_right) /*关闭除了向左行进的所有动画*/ {
            if (animator.GetBool ("forward")) {
                animator.SetBool ("forward", false);
            }
            if (animator.GetBool ("backward")) {
                animator.SetBool ("backward", false);
            }
            if (!animator.GetBool ("left")) {
                animator.SetBool ("left", true);
            }
            if (animator.GetBool ("right")) {
                animator.SetBool ("right", false);
            }

            /*同时按下多个按键时改变动画速度*/
            switch (current_state) {
                case InputCondition.left:
                    if (animator.GetFloat ("speed") != 1) {
                        animator.SetFloat ("speed", 1);
                    }
                    break;
                case InputCondition.forward_left:
                    if (animator.GetFloat ("speed") != 1.5f) {
                        animator.SetFloat ("speed", 1.5f);
                    }
                    break;
                case InputCondition.backward_right:
                    if (animator.GetFloat ("speed") != -1.5f) {
                        animator.SetFloat ("speed", -1.5f);
                    }
                    break;
            }
        } else if (current_state == InputCondition.right || current_state == InputCondition.forward_right || current_state == InputCondition.backward_left) /*关闭除了向右行进的所有动画*/ {
            if (animator.GetBool ("forward")) {
                animator.SetBool ("forward", false);
            }
            if (animator.GetBool ("backward")) {
                animator.SetBool ("backward", false);
            }
            if (animator.GetBool ("left")) {
                animator.SetBool ("left", false);
            }
            if (!animator.GetBool ("right")) {
                animator.SetBool ("right", true);
            }

            /*同时按下多个按键时改变动画速度*/
            switch (current_state) {
                case InputCondition.right:
                    if (animator.GetFloat ("speed") != 1) {
                        animator.SetFloat ("speed", 1);
                    }
                    break;
                case InputCondition.forward_right:
                    if (animator.GetFloat ("speed") != 1.5f) {
                        animator.SetFloat ("speed", 1.5f);
                    }
                    break;
                case InputCondition.backward_left:
                    if (animator.GetFloat ("speed") != -1.5f) {
                        animator.SetFloat ("speed", -1.5f);
                    }
                    break;
            }
        } else //关闭所有动画
        {
            if (animator.GetBool ("forward")) {
                animator.SetBool ("forward", false);
            }
            if (animator.GetBool ("backward")) {
                animator.SetBool ("backward", false);
            }
            if (animator.GetBool ("left")) {
                animator.SetBool ("left", false);
            }
            if (animator.GetBool ("right")) {
                animator.SetBool ("right", false);
            }
        }
    }

    /*输入情况*/
    private InputCondition InputState () {
        input_horizontal = Input.GetAxis ("Horizontal"); //获取键盘的横向输入
        input_vertical = Input.GetAxis ("Vertical"); //获取键盘的纵向输入

        if (input_vertical > 0 && input_horizontal == 0) //如果仅按下前进键
        {
            return InputCondition.forward; //设定前进
        } else if (input_vertical < 0 && input_horizontal == 0) //如果仅按下后退键
        {
            return InputCondition.backward; //设定后退
        } else if (input_horizontal > 0 && input_vertical == 0) //如果仅按下右键
        {
            return InputCondition.right; //设定右走
        } else if (input_horizontal < 0 && input_vertical == 0) //如果仅按下左键
        {
            return InputCondition.left; //设定左走
        } else if (input_vertical > 0 && input_horizontal > 0) //如果同时按上键与右键
        {
            return InputCondition.forward_right; //设定右上方向走
        } else if (input_vertical > 0 && input_horizontal < 0) //如果同时按上键与左键
        {
            return InputCondition.forward_left; //设定左上方向走
        } else if (input_vertical < 0 && input_horizontal > 0) //如果同时按下键与右键
        {
            return InputCondition.backward_right; //设定右下方向走
        } else if (input_vertical < 0 && input_horizontal < 0) //如果同时按下键与左键
        {
            return InputCondition.backward_left; //设定左下方向走
        } else {
            return InputCondition.none; //设定没有按下任何按键
        }
    }
}