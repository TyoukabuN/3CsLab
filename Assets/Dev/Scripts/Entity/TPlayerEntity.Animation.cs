using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TinyGame
{
    [Serializable]
    public partial class TPlayerEntity : TEntity, INumericalControl, IActionControl
    {
        public const string CNAME_IDLE = "Idle";

        public const string CNAME_JUMP_START = "Jump_Start";
        public const string CNAME_JUMP_KEEP = "Jump_Keep";
        public const string CNAME_JUMP_LAND_W = "Jump_Land_Wait";
        public const string CNAME_JUMP_LAND_M = "Jump_Land_Move";

        struct DirectionNameSet
        {
            public string F, FL, FR, B, BL, BR;
        }

        private static DirectionNameSet Walk = new DirectionNameSet()
        {
            F = "Walk_Front",
            FL = "Walk_Front_L",
            FR = "Walk_Front_R",
            B = "Walk_Back",
            BL = "Walk_Back_L",
            BR = "Walk_Back_R"
        };
        private static DirectionNameSet Dash = new DirectionNameSet()
        {
            F = "Dash_Front",
            FL = "Dash_Front_L",
            FR = "Dash_Front_R",
            B = "Dash_Back",
            BL = "Dash_Back_L",
            BR = "Dash_Back_R"
        };

        protected override void Init_Animation()
        {
            base.Init_Animation();

            Animation_InitIK();
        }
        private bool CanPlayMovementAnima()
        {
            return Animation_CanExitJump() && Animation_CanExitAttack();
        }
        protected void Update_Animation()
        {
            if (OnGround && CanPlayMovementAnima())
            {
                Animation_ClearJump();
                Animation_ClearAttack();
                //
                DirectionNameSet moveSet = Walk;
                if (IsRunning())
                    moveSet = Dash;

                if (horizontalInput.magnitude > 0)
                {
                    if (Mathf.Abs(horizontalInput.x) <= 0.001f)
                    {
                        Animancer_Play(horizontalInput.y > 0 ? moveSet.F : moveSet.B);
                    }
                    else
                    {
                        if (horizontalInput.y > 0)
                            Animancer_Play(horizontalInput.x > 0 ? moveSet.FR : moveSet.FL);
                        else
                            Animancer_Play(horizontalInput.x > 0 ? moveSet.BR : moveSet.BL);
                    }
                }
                else
                {
                    Animancer_Play(CNAME_IDLE);
                }
            }
            else
            {
            }
        }
        protected void FixedUpdate_Animation()
        {
            if (!Animation_Attacking())
            {
                if (upperarm_l)
                    Animatiom_HandWallInteractiveIK(AvatarIKGoal.LeftHand, upperarm_l);
                if (upperarm_r)
                    Animatiom_HandWallInteractiveIK(AvatarIKGoal.RightHand, upperarm_r);
            }
            else
            {
                Animatiom_SetIKActive(false);
            }
        }

        #region Jump

        private AnimancerState jumpAnimaState;
        protected bool Animation_CanExitJump()
        {
            if (jumpAnimaState == null)
                return true;
            return jumpAnimaState.NormalizedTime > 0.75f;
        }
        protected void Animation_OnJumpStart()
        {
            jumpAnimaState = Animancer_Play(CNAME_JUMP_START, 0, FadeMode.FromStart);
            jumpAnimaState.Events.OnEnd = Animation_OnJumpStartEnd;
        }
        protected void Animation_OnJumpStartEnd()
        {
            if (!OnGround)
            {
                Animancer_Play(CNAME_JUMP_KEEP, 0, FadeMode.FromStart);
            }
        }
        protected void Animation_OnJumpLand()
        {
            if (jumpAnimaState == null)
                return;
            jumpAnimaState = Animancer_Play(IsMoving() ? CNAME_JUMP_LAND_M : CNAME_JUMP_LAND_W);
        }
        private void Animation_ClearJump()
        {
            if (jumpAnimaState != null)
                jumpAnimaState = null;
        }
        #endregion

        #region Attack Combo

        private string[] attackAnimaNameSet_Hand = new string[] { "Attack_Hand_1", "Attack_Hand_2", "Attack_Hand_3" };
        private float[] exitTimeSet_Hand = new float[] { 0.30f, 0.30f, 0.40f };
        private AttackState attackState;
        private void Animation_Attack()
        {
            //start
            if (attackState == null || attackState.Done())
            {
                attackState = attackState == null ? new AttackState() : attackState;
                attackState.Init(this, attackAnimaNameSet_Hand, exitTimeSet_Hand, Animation_OnComboEnd);
            }
            //combo
            attackState.Next();
        }
        private void Animation_OnComboEnd()
        {
            Animation_ClearAttack();
        }
        private bool Animation_CanExitAttack()
        {
            return attackState == null || attackState.CanExit();
        }
        private void Animation_ClearAttack()
        {
            if (attackState != null)
                attackState.Clear();
            attackState = null;
        }
        private bool Animation_Attacking()
        {
            return !Animation_CanExitAttack();
        }
        public override void OnAnimatorMove()
        {
            if (mainAnimator.applyRootMotion)
            {
                _rigidbody.MovePosition(_rigidbody.position + mainAnimator.deltaPosition);
                _rigidbody.MoveRotation(_rigidbody.rotation * mainAnimator.deltaRotation);
            }
        }

        #endregion

        #region IK

        [SerializeField] private IKPuppetTarget[] _IKTargets;

        public string path_upperarm_l = "root/pelvis/spine_01/spine_02/spine_03/clavicle_l/upperarm_l";
        public string path_upperarm_r = "root/pelvis/spine_01/spine_02/spine_03/clavicle_r/upperarm_r";
        public Transform upperarm_l;
        public Transform upperarm_r;

        private void Animation_InitIK()
        {
            animancer.Layers[0].ApplyAnimatorIK = true;
            Animatiom_SetIKActive(false);
            //
            var model = mainAnimator.transform;
            upperarm_l = model.transform.Find(path_upperarm_l);
            upperarm_r = model.transform.Find(path_upperarm_r);
        }
        public float[] rayDistance = new float[3] { 0.72f, 0.65f, 0.5f };
        public float IKInteractivePointOffset = -0.1f;

        public Vector3[] rayDirections = new Vector3[3];
        private void Animation_OnDrawGizmos()
        {

        }
        /// <summary>
        /// �ֲ���ǽ�彻����IK�߼�
        /// </summary>
        /// <param name="avatarIKGoal"></param>
        /// <param name="shoulder"></param>
        public void Animatiom_HandWallInteractiveIK(AvatarIKGoal avatarIKGoal, Transform shoulder)
        {
            if (shoulder == null)
                return;

            bool found = false;
            Vector3 interactivePoint = default;
            Vector3 side = transform.right;
            if (avatarIKGoal == AvatarIKGoal.LeftHand)
                side = -side;

            RaycastHit raycastHit;

            rayDirections[0] = transform.forward;
            rayDirections[1] = (forward + side).normalized;
            rayDirections[2] = side;

            for (int i = 0; i < rayDirections.Length; i++)
            {
                var direction = rayDirections[i];
                if (Physics.Raycast(shoulder.position, direction, out raycastHit, rayDistance[i], 1 << LayerMask.NameToLayer("Wall")))
                {
                    //Gizmos.color = Color.yellow;
                    Vector3 normal = -raycastHit.normal;
                    Vector3 dir = (raycastHit.point - shoulder.position);
                    dir = normal * (Vector3.Dot(dir, normal) + IKInteractivePointOffset);
                    //Gizmos.DrawLine(shoulder.position, shoulder.position + dir);
                    interactivePoint = shoulder.position + dir;
                    found = true;
                    break;
                }
                else
                {
                    //Gizmos.color = Color.red;
                    //Gizmos.DrawLine(shoulder.position, shoulder.position + direction * rayDistance[i]);
                }
            }

            if (found)
            {
                Animatiom_IKSwitch(avatarIKGoal, true, interactivePoint, IKPuppetTarget.ValueChangeApproach.Tween);
            }
            else
            {
                Animatiom_IKSwitch(avatarIKGoal, false);
            }
        }
        public override void OnAnimatorIK()
        {
            base.OnAnimatorIK();


            for (int i = 0; i < _IKTargets.Length; i++)
            {
                _IKTargets[i].UpdateAnimatorIK(animancer.Animator);
            }
        }
        /// <summary>
        /// IK����
        /// </summary>
        /// <param name="enable"></param>
        public void Animatiom_SetIKActive(bool enable)
        {
            for (int i = 0; i < _IKTargets.Length; i++)
            {
                _IKTargets[i].SetAnimatorIK(animancer.Animator, enable ? 1 : 0, IKPuppetTarget.ValueChangeApproach.Immediately);
            }
        }
        public void Animatiom_IKSwitch(AvatarIKGoal _type, bool enable, Vector3 worldPosition = default, IKPuppetTarget.ValueChangeApproach approach = IKPuppetTarget.ValueChangeApproach.Tween)
        {
            for (int i = 0; i < _IKTargets.Length; i++)
            {
                if (_IKTargets[i].AvatarIKGoal != _type)
                    continue;
                _IKTargets[i].SetAnimatorIK(animancer.Animator, enable ? 1 : 0, approach);
                if (worldPosition != default)
                    _IKTargets[i].transform.position = worldPosition;
            }
        }
        #endregion
    }

    class AttackState
    {
        public TEntity entity;
        public AnimancerState attackAnimaState;
        public string[] animaNameSet;
        public float[] exitTimeSet;
        public int index = -1;
        public Action onEnd = null;

        public bool Done()
        {
            return (index > -1 && index >= animaNameSet.Length - 1) && CanExit();
        }
        public bool CanExit()
        {
            if (attackAnimaState == null)
                return true;
            else if (attackAnimaState.IsPlaying == false)
                return true;
            else if (attackAnimaState.NormalizedTime > 0.75f)
                return true;
            return false;
        }
        public void Init(TEntity entity, string[] animaNameSet, float[] exitTimeSet, Action onEnd)
        {
            this.entity = entity;
            this.attackAnimaState = null;
            this.animaNameSet = animaNameSet;
            this.exitTimeSet = exitTimeSet;
            this.onEnd = onEnd;
            index = -1;
        }
        public float GetExitTime()
        {
            if (exitTimeSet == null || exitTimeSet.Length <= 0 || index >= exitTimeSet.Length)
                return 0.5f;
            return exitTimeSet[index];
        }
        public bool CanNext()
        {
            if (animaNameSet.Length <= 0) return false;
            if (animaNameSet == null) return false;
            //
            if (attackAnimaState != null && attackAnimaState.NormalizedTime < GetExitTime()) return false;
            //
            if (Done()) return false;
            return true;
        }
        public void Next()
        {
            if (!CanNext())
                return;
            try
            {
                attackAnimaState = entity.Animancer_Play(animaNameSet[++index % animaNameSet.Length]);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            entity.animancer.Animator.applyRootMotion = true;
            if (onEnd != null)
                attackAnimaState.Events.OnEnd = onEnd;
        }

        public void Clear()
        {
            entity.animancer.Animator.applyRootMotion = false;
        }
    }
}