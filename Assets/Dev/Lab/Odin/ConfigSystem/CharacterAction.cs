using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LS.Game
{
    [HunterClassLabelText("角色动作")]
    public enum CharacterAction
    {
        [LabelText("无")] [HideInInspector] None = 0,
        [LabelText("冲刺")] Run = 1,
        [LabelText("跳跃")] Jump = 2,
        [LabelText("翻滚")] Roll = 3,
        [LabelText("攻击")] Attack = 4,
        [LabelText("瞄准")] Aim = 5,
        [LabelText("取消瞄准")] AimCancel = 6,
        [LabelText("发射")] Shoot = 7,
        [LabelText("攻击蓄力")] AttackCharge = 8,
        [LabelText("攻击松开瞬间")] AttackJustRelease = 9,
        [LabelText("攻击松开持续")] AttackRelease = 10,
        [LabelText("防御")] Defend = 21,
        [LabelText("使用道具")] UseItem = 70,
        [LabelText("交互")] Interact = 80,
        
        [LabelText("测试用临时按键")] TestTemp = 99,
        
    }

    [HunterClassLabelText("敌怪动作")]
    public enum EnemyAction
    {
        [LabelText("无")] [HideInInspector] None = 0,
        
        #region MOVE
        [HunterSelectionPath("移动")][LabelText("行走")] Walk = 1,
        [HunterSelectionPath("移动")][LabelText("冲刺")] Run = 2,
        [HunterSelectionPath("移动")][LabelText("跳跃")] Jump = 3,
        [HunterSelectionPath("移动")][LabelText("翻滚")] Roll = 4,
        // [LabelText("倒退")] WalkReverse = 5,
        #endregion
        
        #region IDLE
        [HunterSelectionPath("闲置")][LabelText("闲置动作")] Idle_Pose = 10,
        [HunterSelectionPath("闲置")][LabelText("睡觉")] Idle_Sleep = 11,
        [HunterSelectionPath("闲置")][LabelText("醒来")] Idle_Wake = 12,
        [HunterSelectionPath("闲置")][LabelText("坐下")] Idle_Sit = 13,
        [HunterSelectionPath("闲置")][LabelText("坐起")] Idle_SitUp = 14,
        [HunterSelectionPath("闲置")][LabelText("登场")] Idle_Entrance = 18,
        #endregion
        
        #region VIGILANT
        [HunterSelectionPath("警戒")][LabelText("警觉动作")] Vigilant_Alert = 20,
        [HunterSelectionPath("警戒")][LabelText("警觉环视")] Vigilant_LookAround = 21,
        [HunterSelectionPath("警戒")][LabelText("目标确认动作")] Vigilant_ConfirmTarget = 22,
        [HunterSelectionPath("警戒")][LabelText("威吓动作")] Vigilant_ThreatenTarget = 23,
        #endregion
        
        


        #region ATTACK
        [HunterSelectionPath("攻击")][LabelText("普通攻击1")] Attack_Normal1_普通攻击1 = 100,
        [HunterSelectionPath("攻击")][LabelText("普通攻击2")] Attack_Normal2_普通攻击2 = 101,
        [HunterSelectionPath("攻击")][LabelText("普通攻击3")] Attack_Normal3_普通攻击3 = 102,

        [HunterSelectionPath("攻击")][LabelText("重攻击1")] Attack_Heavy1_重攻击1 = 110,
        [HunterSelectionPath("攻击")][LabelText("重攻击2")] Attack_Heavy2_重攻击2 = 111,
        [HunterSelectionPath("攻击")][LabelText("重攻击3")] Attack_Heavy3_重攻击3 = 112,

        [HunterSelectionPath("攻击")][LabelText("两连击")] Attack_Combo2_两连击 = 120,
        [HunterSelectionPath("攻击")][LabelText("三连击")] Attack_Combo3_三连击 = 121,
        [HunterSelectionPath("攻击")][LabelText("四连击")] Attack_Combo4_四连击 = 122,
        
        [HunterSelectionPath("攻击")][LabelText("特殊招式")] Attack_Special_特殊招式 = 131,
        [HunterSelectionPath("攻击")][LabelText("技能招式")] Attack_Skill_技能招式 = 132,
        [HunterSelectionPath("攻击")][LabelText("终极招式")] Attack_Final_终极招式 = 133,
        
        [HunterSelectionPath("攻击")][LabelText("冲刺攻击1")] Attack_Dash1_冲刺攻击1 = 140,
        [HunterSelectionPath("攻击")][LabelText("冲刺攻击2")] Attack_Dash2_冲刺攻击2 = 141,
        #endregion

        #region 交互

        [HunterSelectionPath("交互")][LabelText("交互动作1")] Interaction1_交互1 = 200,
        [HunterSelectionPath("交互")][LabelText("交互动作2")] Interaction2_交互2 = 201,
        [HunterSelectionPath("交互")][LabelText("交互动作3")] Interaction3_交互3 = 202,
        [HunterSelectionPath("交互")][LabelText("交互动作4")] Interaction4_交互4 = 203,

        #endregion
    }

    public enum SailAction
    {
        [LabelText("无"), HideInInspector]
        None = 0,
        [LabelText("交互")]
        Interact,
        [LabelText("船只启动开关")]
        EngineStartStop,
        [LabelText("抛锚开关")]
        Anchor,
        [LabelText("退出驾驶船只")]
        ExitSail,
    }
}