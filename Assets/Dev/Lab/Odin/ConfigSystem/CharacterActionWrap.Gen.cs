namespace LS.Game
{
	public class LSBehaviourTag_CharacterWrap
	{
		public static ActionTagWrap IDLE = new ActionTagWrap(LS.LSBehaviourTag_Character.IDLE);
		public static ActionTagWrap MOVE = new ActionTagWrap(LS.LSBehaviourTag_Character.MOVE);
		public static ActionTagWrap ATTACK = new ActionTagWrap(LS.LSBehaviourTag_Character.ATTACK);
		public static ActionTagWrap DEFENSE = new ActionTagWrap(LS.LSBehaviourTag_Character.DEFENSE);
		public static ActionTagWrap HURT = new ActionTagWrap(LS.LSBehaviourTag_Character.HURT);
		public static ActionTagWrap SWIM = new ActionTagWrap(LS.LSBehaviourTag_Character.SWIM);
	}
	public class LSStateTag_CharacterWrap
	{
		public static ActionTagWrap IDLE = new ActionTagWrap(LS.LSStateTag_Character.IDLE);
		public static ActionTagWrap WALK = new ActionTagWrap(LS.LSStateTag_Character.WALK);
		public static ActionTagWrap RUN = new ActionTagWrap(LS.LSStateTag_Character.RUN);
		public static ActionTagWrap JUMP = new ActionTagWrap(LS.LSStateTag_Character.JUMP);
		public static ActionTagWrap JUMP_AIRBORNE = new ActionTagWrap(LS.LSStateTag_Character.JUMP_AIRBORNE);
		public static ActionTagWrap JUMP_LAND = new ActionTagWrap(LS.LSStateTag_Character.JUMP_LAND);
		public static ActionTagWrap PUNCH_ATTACK_1 = new ActionTagWrap(LS.LSStateTag_Character.PUNCH_ATTACK_1);
		public static ActionTagWrap SWORD_NORMAL_ATTACK_1 = new ActionTagWrap(LS.LSStateTag_Character.SWORD_NORMAL_ATTACK_1);
		public static ActionTagWrap SWORD_NORMAL_ATTACK_2 = new ActionTagWrap(LS.LSStateTag_Character.SWORD_NORMAL_ATTACK_2);
		public static ActionTagWrap SWORD_NORMAL_ATTACK_3 = new ActionTagWrap(LS.LSStateTag_Character.SWORD_NORMAL_ATTACK_3);
		public static ActionTagWrap SWORD_DASH_ATTACK = new ActionTagWrap(LS.LSStateTag_Character.SWORD_DASH_ATTACK);
		public static ActionTagWrap SWORD_CHARGING = new ActionTagWrap(LS.LSStateTag_Character.SWORD_CHARGING);
		public static ActionTagWrap SWORD_CHARGE_ATTACK = new ActionTagWrap(LS.LSStateTag_Character.SWORD_CHARGE_ATTACK);
		public static ActionTagWrap SHIELD_BLOCKED_ATTACK = new ActionTagWrap(LS.LSStateTag_Character.SHIELD_BLOCKED_ATTACK);
		public static ActionTagWrap SHIELD_SLAM = new ActionTagWrap(LS.LSStateTag_Character.SHIELD_SLAM);
		public static ActionTagWrap SHIELD_ENTER_DEFENSE = new ActionTagWrap(LS.LSStateTag_Character.SHIELD_ENTER_DEFENSE);
		public static ActionTagWrap SHIELD_DEFENDING = new ActionTagWrap(LS.LSStateTag_Character.SHIELD_DEFENDING);
		public static ActionTagWrap SHIELD_BLOCK = new ActionTagWrap(LS.LSStateTag_Character.SHIELD_BLOCK);
		public static ActionTagWrap HURT_NO = new ActionTagWrap(LS.LSStateTag_Character.HURT_NO);
		public static ActionTagWrap HURT_WEAK_F = new ActionTagWrap(LS.LSStateTag_Character.HURT_WEAK_F);
		public static ActionTagWrap HURT_WEAK_B = new ActionTagWrap(LS.LSStateTag_Character.HURT_WEAK_B);
		public static ActionTagWrap HURT_HEAVY_F = new ActionTagWrap(LS.LSStateTag_Character.HURT_HEAVY_F);
		public static ActionTagWrap HURT_HEAVY_B = new ActionTagWrap(LS.LSStateTag_Character.HURT_HEAVY_B);
		public static ActionTagWrap HURT_KNOCKFLY_F = new ActionTagWrap(LS.LSStateTag_Character.HURT_KNOCKFLY_F);
		public static ActionTagWrap HURT_KNOCKFLY_B = new ActionTagWrap(LS.LSStateTag_Character.HURT_KNOCKFLY_B);
		public static ActionTagWrap HURT_KNOCKFLYING = new ActionTagWrap(LS.LSStateTag_Character.HURT_KNOCKFLYING);
		public static ActionTagWrap HURT_KNOCKDOWN = new ActionTagWrap(LS.LSStateTag_Character.HURT_KNOCKDOWN);
		public static ActionTagWrap HURT_STUN = new ActionTagWrap(LS.LSStateTag_Character.HURT_STUN);
		public static ActionTagWrap HURT_STAND_UP = new ActionTagWrap(LS.LSStateTag_Character.HURT_STAND_UP);
		public static ActionTagWrap HURT_GROUND_DIE = new ActionTagWrap(LS.LSStateTag_Character.HURT_GROUND_DIE);
		public static ActionTagWrap HURT_DIE = new ActionTagWrap(LS.LSStateTag_Character.HURT_DIE);
		public static ActionTagWrap HURT_RAGDOLL_FULL_ACTIVE = new ActionTagWrap(LS.LSStateTag_Character.HURT_RAGDOLL_FULL_ACTIVE);
		public static ActionTagWrap HURT_RAGDOLL_FULL_DEAD = new ActionTagWrap(LS.LSStateTag_Character.HURT_RAGDOLL_FULL_DEAD);
	}
	public class LSBehaviourTag_EnemyWrap
	{
		public static ActionTagWrap IDLE = new ActionTagWrap(LS.LSBehaviourTag_Enemy.IDLE);
		public static ActionTagWrap MOVE = new ActionTagWrap(LS.LSBehaviourTag_Enemy.MOVE);
		public static ActionTagWrap VIGILANT = new ActionTagWrap(LS.LSBehaviourTag_Enemy.VIGILANT);
		public static ActionTagWrap ATTACK = new ActionTagWrap(LS.LSBehaviourTag_Enemy.ATTACK);
		public static ActionTagWrap HURT = new ActionTagWrap(LS.LSBehaviourTag_Enemy.HURT);
		public static ActionTagWrap INTERACTION = new ActionTagWrap(LS.LSBehaviourTag_Enemy.INTERACTION);
	}
	public class LSStateTag_EnemyWrap
	{
		public static ActionTagWrap IDLE = new ActionTagWrap(LS.LSStateTag_Enemy.IDLE);
		public static ActionTagWrap TURN_AROUND = new ActionTagWrap(LS.LSStateTag_Enemy.TURN_AROUND);
		public static ActionTagWrap POSE = new ActionTagWrap(LS.LSStateTag_Enemy.POSE);
		public static ActionTagWrap SLEEP = new ActionTagWrap(LS.LSStateTag_Enemy.SLEEP);
		public static ActionTagWrap WAKE = new ActionTagWrap(LS.LSStateTag_Enemy.WAKE);
		public static ActionTagWrap IDLE_SIT = new ActionTagWrap(LS.LSStateTag_Enemy.IDLE_SIT);
		public static ActionTagWrap IDLE_SITTING = new ActionTagWrap(LS.LSStateTag_Enemy.IDLE_SITTING);
		public static ActionTagWrap IDLE_SITUP = new ActionTagWrap(LS.LSStateTag_Enemy.IDLE_SITUP);
		public static ActionTagWrap ENTRANCE = new ActionTagWrap(LS.LSStateTag_Enemy.ENTRANCE);
		public static ActionTagWrap WALK = new ActionTagWrap(LS.LSStateTag_Enemy.WALK);
		public static ActionTagWrap RUN = new ActionTagWrap(LS.LSStateTag_Enemy.RUN);
		public static ActionTagWrap DASH = new ActionTagWrap(LS.LSStateTag_Enemy.DASH);
		public static ActionTagWrap AERIAL = new ActionTagWrap(LS.LSStateTag_Enemy.AERIAL);
		public static ActionTagWrap ALERT = new ActionTagWrap(LS.LSStateTag_Enemy.ALERT);
		public static ActionTagWrap LOOK_AROUND = new ActionTagWrap(LS.LSStateTag_Enemy.LOOK_AROUND);
		public static ActionTagWrap CONFIRM_TARGET = new ActionTagWrap(LS.LSStateTag_Enemy.CONFIRM_TARGET);
		public static ActionTagWrap THREATEN_TARGET = new ActionTagWrap(LS.LSStateTag_Enemy.THREATEN_TARGET);
		public static ActionTagWrap NORMAL_ATTACK_1 = new ActionTagWrap(LS.LSStateTag_Enemy.NORMAL_ATTACK_1);
		public static ActionTagWrap NORMAL_ATTACK_2 = new ActionTagWrap(LS.LSStateTag_Enemy.NORMAL_ATTACK_2);
		public static ActionTagWrap NORMAL_ATTACK_3 = new ActionTagWrap(LS.LSStateTag_Enemy.NORMAL_ATTACK_3);
		public static ActionTagWrap HEAVY_ATTACK_1 = new ActionTagWrap(LS.LSStateTag_Enemy.HEAVY_ATTACK_1);
		public static ActionTagWrap HEAVY_ATTACK_2 = new ActionTagWrap(LS.LSStateTag_Enemy.HEAVY_ATTACK_2);
		public static ActionTagWrap HEAVY_ATTACK_3 = new ActionTagWrap(LS.LSStateTag_Enemy.HEAVY_ATTACK_3);
		public static ActionTagWrap COMBO_ATTACK_2 = new ActionTagWrap(LS.LSStateTag_Enemy.COMBO_ATTACK_2);
		public static ActionTagWrap COMBO_ATTACK_3 = new ActionTagWrap(LS.LSStateTag_Enemy.COMBO_ATTACK_3);
		public static ActionTagWrap COMBO_ATTACK_4 = new ActionTagWrap(LS.LSStateTag_Enemy.COMBO_ATTACK_4);
		public static ActionTagWrap SPECIAL_ATTACK = new ActionTagWrap(LS.LSStateTag_Enemy.SPECIAL_ATTACK);
		public static ActionTagWrap SKILL_ATTACK = new ActionTagWrap(LS.LSStateTag_Enemy.SKILL_ATTACK);
		public static ActionTagWrap FINAL_ATTACK = new ActionTagWrap(LS.LSStateTag_Enemy.FINAL_ATTACK);
		public static ActionTagWrap DASH_ATTACK_1 = new ActionTagWrap(LS.LSStateTag_Enemy.DASH_ATTACK_1);
		public static ActionTagWrap DASH_ATTACK_2 = new ActionTagWrap(LS.LSStateTag_Enemy.DASH_ATTACK_2);
		public static ActionTagWrap TIRED = new ActionTagWrap(LS.LSStateTag_Enemy.TIRED);
		public static ActionTagWrap HURT_WEAK_F = new ActionTagWrap(LS.LSStateTag_Enemy.HURT_WEAK_F);
		public static ActionTagWrap HURT_WEAK_B = new ActionTagWrap(LS.LSStateTag_Enemy.HURT_WEAK_B);
		public static ActionTagWrap HURT_HEAVY_F = new ActionTagWrap(LS.LSStateTag_Enemy.HURT_HEAVY_F);
		public static ActionTagWrap HURT_HEAVY_B = new ActionTagWrap(LS.LSStateTag_Enemy.HURT_HEAVY_B);
		public static ActionTagWrap HURT_KNOCKFLY_F = new ActionTagWrap(LS.LSStateTag_Enemy.HURT_KNOCKFLY_F);
		public static ActionTagWrap HURT_KNOCKFLY_B = new ActionTagWrap(LS.LSStateTag_Enemy.HURT_KNOCKFLY_B);
		public static ActionTagWrap HURT_KNOCKFLYING = new ActionTagWrap(LS.LSStateTag_Enemy.HURT_KNOCKFLYING);
		public static ActionTagWrap HURT_KNOCKDOWN = new ActionTagWrap(LS.LSStateTag_Enemy.HURT_KNOCKDOWN);
		public static ActionTagWrap HURT_STUN = new ActionTagWrap(LS.LSStateTag_Enemy.HURT_STUN);
		public static ActionTagWrap HURT_STAND_UP = new ActionTagWrap(LS.LSStateTag_Enemy.HURT_STAND_UP);
		public static ActionTagWrap HURT_GROUND_DIE = new ActionTagWrap(LS.LSStateTag_Enemy.HURT_GROUND_DIE);
		public static ActionTagWrap HURT_DIE = new ActionTagWrap(LS.LSStateTag_Enemy.HURT_DIE);
		public static ActionTagWrap INTERACTION1 = new ActionTagWrap(LS.LSStateTag_Enemy.INTERACTION1);
		public static ActionTagWrap INTERACTION2 = new ActionTagWrap(LS.LSStateTag_Enemy.INTERACTION2);
		public static ActionTagWrap INTERACTION3 = new ActionTagWrap(LS.LSStateTag_Enemy.INTERACTION3);
		public static ActionTagWrap INTERACTION4 = new ActionTagWrap(LS.LSStateTag_Enemy.INTERACTION4);
	}
	public class CharacterActionWrap
	{
		public static ActionTagWrap None = new ActionTagWrap(LS.Game.CharacterAction.None);
		public static ActionTagWrap Run = new ActionTagWrap(LS.Game.CharacterAction.Run);
		public static ActionTagWrap Jump = new ActionTagWrap(LS.Game.CharacterAction.Jump);
		public static ActionTagWrap Roll = new ActionTagWrap(LS.Game.CharacterAction.Roll);
		public static ActionTagWrap Attack = new ActionTagWrap(LS.Game.CharacterAction.Attack);
		public static ActionTagWrap Aim = new ActionTagWrap(LS.Game.CharacterAction.Aim);
		public static ActionTagWrap AimCancel = new ActionTagWrap(LS.Game.CharacterAction.AimCancel);
		public static ActionTagWrap Shoot = new ActionTagWrap(LS.Game.CharacterAction.Shoot);
		public static ActionTagWrap AttackCharge = new ActionTagWrap(LS.Game.CharacterAction.AttackCharge);
		public static ActionTagWrap AttackJustRelease = new ActionTagWrap(LS.Game.CharacterAction.AttackJustRelease);
		public static ActionTagWrap AttackRelease = new ActionTagWrap(LS.Game.CharacterAction.AttackRelease);
		public static ActionTagWrap Defend = new ActionTagWrap(LS.Game.CharacterAction.Defend);
		public static ActionTagWrap UseItem = new ActionTagWrap(LS.Game.CharacterAction.UseItem);
		public static ActionTagWrap Interact = new ActionTagWrap(LS.Game.CharacterAction.Interact);
		public static ActionTagWrap TestTemp = new ActionTagWrap(LS.Game.CharacterAction.TestTemp);
	}
	public class EnemyActionWrap
	{
		public static ActionTagWrap None = new ActionTagWrap(LS.Game.EnemyAction.None);
		public static ActionTagWrap Walk = new ActionTagWrap(LS.Game.EnemyAction.Walk);
		public static ActionTagWrap Run = new ActionTagWrap(LS.Game.EnemyAction.Run);
		public static ActionTagWrap Jump = new ActionTagWrap(LS.Game.EnemyAction.Jump);
		public static ActionTagWrap Roll = new ActionTagWrap(LS.Game.EnemyAction.Roll);
		public static ActionTagWrap Idle_Pose = new ActionTagWrap(LS.Game.EnemyAction.Idle_Pose);
		public static ActionTagWrap Idle_Sleep = new ActionTagWrap(LS.Game.EnemyAction.Idle_Sleep);
		public static ActionTagWrap Idle_Wake = new ActionTagWrap(LS.Game.EnemyAction.Idle_Wake);
		public static ActionTagWrap Idle_Sit = new ActionTagWrap(LS.Game.EnemyAction.Idle_Sit);
		public static ActionTagWrap Idle_SitUp = new ActionTagWrap(LS.Game.EnemyAction.Idle_SitUp);
		public static ActionTagWrap Idle_Entrance = new ActionTagWrap(LS.Game.EnemyAction.Idle_Entrance);
		public static ActionTagWrap Vigilant_Alert = new ActionTagWrap(LS.Game.EnemyAction.Vigilant_Alert);
		public static ActionTagWrap Vigilant_LookAround = new ActionTagWrap(LS.Game.EnemyAction.Vigilant_LookAround);
		public static ActionTagWrap Vigilant_ConfirmTarget = new ActionTagWrap(LS.Game.EnemyAction.Vigilant_ConfirmTarget);
		public static ActionTagWrap Vigilant_ThreatenTarget = new ActionTagWrap(LS.Game.EnemyAction.Vigilant_ThreatenTarget);
		public static ActionTagWrap Attack_Normal1_普通攻击1 = new ActionTagWrap(LS.Game.EnemyAction.Attack_Normal1_普通攻击1);
		public static ActionTagWrap Attack_Normal2_普通攻击2 = new ActionTagWrap(LS.Game.EnemyAction.Attack_Normal2_普通攻击2);
		public static ActionTagWrap Attack_Normal3_普通攻击3 = new ActionTagWrap(LS.Game.EnemyAction.Attack_Normal3_普通攻击3);
		public static ActionTagWrap Attack_Heavy1_重攻击1 = new ActionTagWrap(LS.Game.EnemyAction.Attack_Heavy1_重攻击1);
		public static ActionTagWrap Attack_Heavy2_重攻击2 = new ActionTagWrap(LS.Game.EnemyAction.Attack_Heavy2_重攻击2);
		public static ActionTagWrap Attack_Heavy3_重攻击3 = new ActionTagWrap(LS.Game.EnemyAction.Attack_Heavy3_重攻击3);
		public static ActionTagWrap Attack_Combo2_两连击 = new ActionTagWrap(LS.Game.EnemyAction.Attack_Combo2_两连击);
		public static ActionTagWrap Attack_Combo3_三连击 = new ActionTagWrap(LS.Game.EnemyAction.Attack_Combo3_三连击);
		public static ActionTagWrap Attack_Combo4_四连击 = new ActionTagWrap(LS.Game.EnemyAction.Attack_Combo4_四连击);
		public static ActionTagWrap Attack_Special_特殊招式 = new ActionTagWrap(LS.Game.EnemyAction.Attack_Special_特殊招式);
		public static ActionTagWrap Attack_Skill_技能招式 = new ActionTagWrap(LS.Game.EnemyAction.Attack_Skill_技能招式);
		public static ActionTagWrap Attack_Final_终极招式 = new ActionTagWrap(LS.Game.EnemyAction.Attack_Final_终极招式);
		public static ActionTagWrap Attack_Dash1_冲刺攻击1 = new ActionTagWrap(LS.Game.EnemyAction.Attack_Dash1_冲刺攻击1);
		public static ActionTagWrap Attack_Dash2_冲刺攻击2 = new ActionTagWrap(LS.Game.EnemyAction.Attack_Dash2_冲刺攻击2);
		public static ActionTagWrap Interaction1_交互1 = new ActionTagWrap(LS.Game.EnemyAction.Interaction1_交互1);
		public static ActionTagWrap Interaction2_交互2 = new ActionTagWrap(LS.Game.EnemyAction.Interaction2_交互2);
		public static ActionTagWrap Interaction3_交互3 = new ActionTagWrap(LS.Game.EnemyAction.Interaction3_交互3);
		public static ActionTagWrap Interaction4_交互4 = new ActionTagWrap(LS.Game.EnemyAction.Interaction4_交互4);
	}
	public class SailActionWrap
	{
		public static ActionTagWrap None = new ActionTagWrap(LS.Game.SailAction.None);
		public static ActionTagWrap Interact = new ActionTagWrap(LS.Game.SailAction.Interact);
		public static ActionTagWrap EngineStartStop = new ActionTagWrap(LS.Game.SailAction.EngineStartStop);
		public static ActionTagWrap Anchor = new ActionTagWrap(LS.Game.SailAction.Anchor);
		public static ActionTagWrap ExitSail = new ActionTagWrap(LS.Game.SailAction.ExitSail);
		public static ActionTagWrap TEST_70005 = new ActionTagWrap("TEST_70005");
	}
}
