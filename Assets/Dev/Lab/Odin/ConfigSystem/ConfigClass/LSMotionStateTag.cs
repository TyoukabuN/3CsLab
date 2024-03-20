using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LS
{
    #region Hero
    [MotionStateTagPreset("角色行为标签", true)]
    public static class LSBehaviourTag_Character
    {
        [LabelText("闲置行为"), TagIcon("Icon_State_Idle")]
        public const string IDLE = "BEHAVIOUR_IDLE";
        [LabelText("移动行为"), TagIcon("Icon_State_Walk")]
        public const string MOVE = "BEHAVIOUR_MOVE";
        [LabelText("攻击行为"), TagIcon("Icon_State_Attack")]
        public const string ATTACK = "BEHAVIOUR_ATTACK";
        [LabelText("防御行为"), TagIcon("Icon_State_Defense")]
        public const string DEFENSE = "BEHAVIOUR_DEFENSE";
        [LabelText("受击行为"), TagIcon("Icon_State_Hurt")]
        public const string HURT = "BEHAVIOUR_HURT";

        [LabelText("游泳行为"), TagIcon("Icon_State_Swim")]
        public const string SWIM = "BEHAVIOUR_SWIM";
    }
    [MotionStateTagPreset("角色状态标签", false)]
    public static class LSStateTag_Character
    {
        #region Idle
        [LabelText("静息"), TagIcon("Icon_State_Idle")]
        public const string IDLE = "STATE_IDLE";
        #endregion

        #region Move
        [LabelText("走"), TagIcon("Icon_State_Move_Walk")]
        public const string WALK = "STATE_WALK";
        [LabelText("冲刺"), TagIcon("Icon_State_Move_Run")]
        public const string RUN = "STATE_RUN";

        [LabelText("跳跃"), TagIcon("Icon_State_Jump")]
        public const string JUMP = "STATE_JUMP";
        [LabelText("跳跃滞空"), TagIcon("Icon_State_Jump_Airborne")]
        public const string JUMP_AIRBORNE = "STATE_JUMP_AIRBORNE";
        [LabelText("跳跃"), TagIcon("Icon_State_Jump_Land")]
        public const string JUMP_LAND = "STATE_JUMP_LAND";
        #endregion

        #region Attack
        #region 空手
        [LabelText("赤手_拳击1"), TagIcon("Icon_State_Attack")]
        public const string PUNCH_ATTACK_1 = "STATE_PUNCH_ATTACK_1";
        #endregion

        #region 剑相关
        [LabelText("剑_普攻1"), TagIcon("Icon_State_Sword_Attack")]
        public const string SWORD_NORMAL_ATTACK_1 = "STATE_SWORD_NORMAL_ATTACK_1";
        [LabelText("剑_普攻2"), TagIcon("Icon_State_Sword_Attack")]
        public const string SWORD_NORMAL_ATTACK_2 = "STATE_SWORD_NORMAL_ATTACK_2";
        [LabelText("剑_普攻3"), TagIcon("Icon_State_Sword_Attack")]
        public const string SWORD_NORMAL_ATTACK_3 = "STATE_SWORD_NORMAL_ATTACK_3";

        [LabelText("剑_冲刺攻击"), TagIcon("Icon_State_Sword_Attack")]
        public const string SWORD_DASH_ATTACK = "STATE_SWORD_DASH_ATTACK";
        [LabelText("剑_蓄力"), TagIcon("Icon_State_Sword_Attack")]
        public const string SWORD_CHARGING = "STATE_SWORD_CHARGING";
        [LabelText("剑_蓄力攻击"), TagIcon("Icon_State_Sword_Attack")]
        public const string SWORD_CHARGE_ATTACK = "STATE_SWORD_CHARGE_ATTACK";
        #endregion

        #region 盾相关
        [LabelText("持盾_格挡反击"), TagIcon("Icon_State_Shield_Blocked_Attack")]
        public const string SHIELD_BLOCKED_ATTACK = "STATE_SHIELD_BLOCKED_ATTACK";
        [LabelText("持盾_盾击"), TagIcon("Icon_State_Shield_Slam")]
        public const string SHIELD_SLAM = "STATE_SHIELD_SLAM";
        #endregion
        #endregion

        #region Defense
        [LabelText("持盾_进入防御"), TagIcon("Icon_State_Shield_Enter_Defense")]
        public const string SHIELD_ENTER_DEFENSE = "STATE_SHIELD_ENTER_DEFENSE";
        [LabelText("持盾_防御中"), TagIcon("Icon_State_Shield_Defending")]
        public const string SHIELD_DEFENDING = "STATE_SHIELD_DEFENDING";
        [LabelText("持盾_格挡"), TagIcon("Icon_State_Shield_Block")]
        public const string SHIELD_BLOCK = "STATE_SHIELD_BLOCK";
        #endregion

        #region Hurt
        [LabelText("没受击硬直"), TagIcon("Icon_State_Hurt_No")]
        public const string HURT_NO = "STATE_HURT_NO";
        [LabelText("正面受击小硬直"), TagIcon("Icon_State_Hurt_Weak")]
        public const string HURT_WEAK_F = "STATE_HURT_WEAK_F";
        [LabelText("背面受击小硬直"), TagIcon("Icon_State_Hurt_Weak")]
        public const string HURT_WEAK_B = "STATE_HURT_WEAK_B";
        [LabelText("正面受击大硬直"), TagIcon("Icon_State_Hurt_Heavy")]
        public const string HURT_HEAVY_F = "STATE_HURT_HEAVY_F";
        [LabelText("背面受击大硬直"), TagIcon("Icon_State_Hurt_Heavy")]
        public const string HURT_HEAVY_B = "STATE_HURT_HEAVY_B";
        [LabelText("正面受击被击飞"), TagIcon("Icon_State_Hurt_KnockFly")]
        public const string HURT_KNOCKFLY_F = "STATE_HURT_KNOCKFLY_F";
        [LabelText("背面受击被击飞"), TagIcon("Icon_State_Hurt_KnockFly")]
        public const string HURT_KNOCKFLY_B = "STATE_HURT_KNOCKFLY_B";
        [LabelText("被击飞中"), TagIcon("Icon_State_Hurt_KnockFlying")]
        public const string HURT_KNOCKFLYING = "STATE_HURT_KNOCKFLYING";
        [LabelText("被砸倒"), TagIcon("Icon_State_Hurt_KnockDown")]
        public const string HURT_KNOCKDOWN = "STATE_HURT_KNOCKDOWN";
        [LabelText("受击击晕"), TagIcon("Icon_State_Hurt_Stun")]
        public const string HURT_STUN = "STATE_HURT_STUN";
        [LabelText("倒地站起"), TagIcon("Icon_State_Hurt_StandUp")]
        public const string HURT_STAND_UP = "STATE_HURT_STAND_UP";
        [LabelText("倒地死亡"), TagIcon("Icon_State_Hurt_Die")]
        public const string HURT_GROUND_DIE = "STATE_HURT_GROUND_DIE";
        [LabelText("死亡"), TagIcon("Icon_State_Hurt_Die")]
        public const string HURT_DIE = "STATE_HURT_DIE";
        [LabelText("全身布娃娃模拟"), TagIcon("Icon_Ragdoll_Full_Active")]
        public const string HURT_RAGDOLL_FULL_ACTIVE = "STATE_RD_FA";
        [LabelText("全身布娃娃死亡"), TagIcon("Icon_Ragdoll_Full_Dead")]
        public const string HURT_RAGDOLL_FULL_DEAD = "STATE_RD_FD";

        #endregion
    }
    #endregion



    #region Enemy 
    // [MotionTagPreset("敌人Behaviour标签")]
    [MotionStateTagPreset("敌怪行为标签", true)]
    public static class LSBehaviourTag_Enemy
    {
        [LabelText("闲置行为"), TagIcon("Icon_State_Idle")]
        public const string IDLE = "BEHAVIOUR_IDLE";
        [LabelText("移动行为"), TagIcon("Icon_State_Walk")]
        public const string MOVE = "BEHAVIOUR_MOVE";
        [LabelText("警戒行为"), TagIcon("Icon_State_Vigilant")]
        public const string VIGILANT = "BEHAVIOUR_VIGILANT";
        [LabelText("攻击行为"), TagIcon("Icon_State_Attack")]
        public const string ATTACK = "BEHAVIOUR_ATTACK";
        [LabelText("受击行为"), TagIcon("Icon_State_Hurt")]
        public const string HURT = "BEHAVIOUR_HURT";
        [LabelText("交互行为"), TagIcon("")]
        public const string INTERACTION = "BEHAVIOUR_INTERACTION";
    }


    [MotionStateTagPreset("敌怪状态标签", false)]
    // [MotionTagPreset("敌人State标签")]
    public static class LSStateTag_Enemy
    {
        #region IDLE Behaviour
        [LabelText("静息"), TagIcon("Icon_State_Idle")]
        public const string IDLE = "STATE_IDLE";
        [LabelText("原地转身"), TagIcon("Icon_State_TurnAround")]
        public const string TURN_AROUND = "STATE_TURN_AROUND";
        [LabelText("闲置POSE"), TagIcon("Icon_State_Pose")]
        public const string POSE = "STATE_POSE";
        [LabelText("睡觉"), TagIcon("Icon_State_Sleep")]
        public const string SLEEP = "STATE_SLEEP";
        [LabelText("醒来"), TagIcon("Icon_State_Wake")]
        public const string WAKE = "STATE_WAKE";

        [LabelText("坐下"), TagIcon("Icon_State_Sit")]
        public const string IDLE_SIT = "STATE_IDLE_SIT";
        [LabelText("坐着"), TagIcon("Icon_State_Sit")]
        public const string IDLE_SITTING = "STATE_IDLE_SITTING";
        [LabelText("坐起"), TagIcon("Icon_State_Sit")]
        public const string IDLE_SITUP = "STATE_IDLE_SITUP";

        [LabelText("登场"), TagIcon("Icon_State_Entrance")]
        public const string ENTRANCE = "STATE_ENTRANCE";
        #endregion

        #region MOVE Behaviour
        [LabelText("行走"), TagIcon("Icon_State_Walk")]
        public const string WALK = "STATE_WALK";
        // [LabelText("倒退走"), TagIcon("Icon_State_WalkReverse")]
        // public const string WALK_REVERSE = "STATE_WALK_REVERSE";
        [LabelText("奔跑"), TagIcon("Icon_State_Run")]
        public const string RUN = "STATE_RUN";
        [LabelText("猛冲"), TagIcon("Icon_State_Dash")]
        public const string DASH = "STATE_DASH";
        // [LabelText("跳跃"), TagIcon("Icon_State_Jump")]
        // public const string JUMP = "STATE_JUMP";
        [LabelText("滞空"), TagIcon("Icon_State_Aerial")]
        public const string AERIAL = "STATE_AERIAL";
        #endregion

        #region VIGILANT Behaviour
        [LabelText("引起警觉"), TagIcon("Icon_State_Alert")]
        public const string ALERT = "STATE_ALERT";
        [LabelText("警觉环视"), TagIcon("Icon_State_LookAround")]
        public const string LOOK_AROUND = "STATE_LOOK_AROUND";
        [LabelText("确认目标"), TagIcon("Icon_State_ConfirmTarget")]
        public const string CONFIRM_TARGET = "STATE_CONFIRM_TARGET";
        [LabelText("威吓目标"), TagIcon("Icon_State_ThreatenTarget")]
        public const string THREATEN_TARGET = "STATE_THREATEN_TARGET";
        #endregion

        #region ATTACK Behaviour
        [LabelText("普通攻击1"), TagIcon("Icon_State_NormalAttack1")]
        public const string NORMAL_ATTACK_1 = "STATE_NORMAL_ATTACK_1";
        [LabelText("普通攻击2"), TagIcon("Icon_State_NormalAttack2")]
        public const string NORMAL_ATTACK_2 = "STATE_NORMAL_ATTACK_2";
        [LabelText("普通攻击3"), TagIcon("Icon_State_NormalAttack3")]
        public const string NORMAL_ATTACK_3 = "STATE_NORMAL_ATTACK_3";
        [LabelText("重攻击1"), TagIcon("Icon_State_HeavyAttack1")]
        public const string HEAVY_ATTACK_1 = "STATE_HEAVY_ATTACK_1";
        [LabelText("重攻击2"), TagIcon("Icon_State_HeavyAttack2")]
        public const string HEAVY_ATTACK_2 = "STATE_HEAVY_ATTACK_2";
        [LabelText("重攻击3"), TagIcon("Icon_State_HeavyAttack3")]
        public const string HEAVY_ATTACK_3 = "STATE_HEAVY_ATTACK_3";
        [LabelText("两连击"), TagIcon("Icon_State_ComboAttack2")]
        public const string COMBO_ATTACK_2 = "STATE_COMBO_ATTACK_2";
        [LabelText("三连击"), TagIcon("Icon_State_ComboAttack3")]
        public const string COMBO_ATTACK_3 = "STATE_COMBO_ATTACK_3";
        [LabelText("四连击"), TagIcon("Icon_State_ComboAttack4")]
        public const string COMBO_ATTACK_4 = "STATE_COMBO_ATTACK_4";
        [LabelText("特殊招式"), TagIcon("Icon_State_SpecialAttack")]
        public const string SPECIAL_ATTACK = "STATE_SPECIAL_ATTACK";
        [LabelText("技能招式"), TagIcon("Icon_State_SkillAttack")]
        public const string SKILL_ATTACK = "STATE_SKILL_ATTACK";
        [LabelText("终极招式"), TagIcon("Icon_State_FinalAttack")]
        public const string FINAL_ATTACK = "STATE_FINAL_ATTACK";
        [LabelText("冲刺攻击1"), TagIcon("Icon_State_DashAttack1")]
        public const string DASH_ATTACK_1 = "STATE_DASH_ATTACK_1";
        [LabelText("冲刺攻击2"), TagIcon("Icon_State_DashAttack2")]
        public const string DASH_ATTACK_2 = "STATE_DASH_ATTACK_2";
        [LabelText("累了"), TagIcon("Icon_State_Tired")]
        public const string TIRED = "STATE_DASH_TIRED";
        #endregion

        #region HURT Behaviour
        [LabelText("正面受击小硬直"), TagIcon("Icon_State_Hurt_Weak")]
        public const string HURT_WEAK_F = "STATE_HURT_WEAK_F";
        [LabelText("背面受击小硬直"), TagIcon("Icon_State_Hurt_Weak")]
        public const string HURT_WEAK_B = "STATE_HURT_WEAK_B";
        [LabelText("正面受击大硬直"), TagIcon("Icon_State_Hurt_Heavy")]
        public const string HURT_HEAVY_F = "STATE_HURT_HEAVY_F";
        [LabelText("背面受击大硬直"), TagIcon("Icon_State_Hurt_Heavy")]
        public const string HURT_HEAVY_B = "STATE_HURT_HEAVY_B";
        [LabelText("正面受击被击飞"), TagIcon("Icon_State_Hurt_KnockFly")]
        public const string HURT_KNOCKFLY_F = "STATE_HURT_KNOCKFLY_F";
        [LabelText("背面受击被击飞"), TagIcon("Icon_State_Hurt_KnockFly")]
        public const string HURT_KNOCKFLY_B = "STATE_HURT_KNOCKFLY_B";
        [LabelText("被击飞中"), TagIcon("Icon_State_Hurt_KnockFlying")]
        public const string HURT_KNOCKFLYING = "STATE_HURT_KNOCKFLYING";
        [LabelText("被砸倒"), TagIcon("Icon_State_Hurt_KnockDown")]
        public const string HURT_KNOCKDOWN = "STATE_HURT_KNOCKDOWN";
        [LabelText("受击击晕"), TagIcon("Icon_State_Hurt_Stun")]
        public const string HURT_STUN = "STATE_HURT_STUN";
        [LabelText("倒地站起"), TagIcon("Icon_State_Hurt_StandUp")]
        public const string HURT_STAND_UP = "STATE_HURT_STAND_UP";
        [LabelText("倒地死亡"), TagIcon("Icon_State_Hurt_Die")]
        public const string HURT_GROUND_DIE = "STATE_HURT_GROUND_DIE";
        [LabelText("死亡"), TagIcon("Icon_State_Hurt_Die")]
        public const string HURT_DIE = "STATE_HURT_DIE";
        #endregion

        #region INTERACTION Behaviour
        [LabelText("交互动作1"), TagIcon("")]
        public const string INTERACTION1 = "STATE_INTERACTION1";
        [LabelText("交互动作2"), TagIcon("")]
        public const string INTERACTION2 = "STATE_INTERACTION2";
        [LabelText("交互动作3"), TagIcon("")]
        public const string INTERACTION3 = "STATE_INTERACTION3";
        [LabelText("交互动作4"), TagIcon("")]
        public const string INTERACTION4 = "STATE_INTERACTION4";
        #endregion
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class MotionStateTagPresetAttribute : Attribute
    {
        public readonly string Category;
        public readonly bool isBehaviourTag;
        public MotionStateTagPresetAttribute(string category, bool isBehaviourTag)
        {
            this.Category = category;
            this.isBehaviourTag = isBehaviourTag;
        }
    }


    // 必须存在于
    [AttributeUsage(AttributeTargets.Field)]
    public class TagIconAttribute : Attribute
    {
        public string fileName;

        public TagIconAttribute(string fileName)
        {
            this.fileName = fileName;
        }
    }
    #endregion

}