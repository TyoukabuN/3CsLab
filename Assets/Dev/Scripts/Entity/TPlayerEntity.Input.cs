using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TinyGame
{
    [RequireComponent(typeof(PlayerInput))]
    public partial class TPlayerEntity : TEntity, INumericalControl, IActionControl
    {
        protected bool m_lastCanUp = false;
        public Action<bool> onCanUpChange;

        protected float rotationPower = 0.15f;

        protected Vector2 horizontalInput;
        protected Vector2 _look;
        protected float _run;

        #region Input System Message
        protected void OnMove(InputValue value)
        {
            horizontalInput = value.Get<Vector2>();
        }

        protected void OnLook(InputValue value)
        {
            _look = value.Get<Vector2>();
        }

        protected void OnRun(InputValue value)
        {
            _run = value.Get<float>();
        }

        protected void OnJump(InputValue value)
        {
            if (CanJump())
            {
                Input_Jump();
            }
            else if (IsHanging())
            {
                Input_UnHang();
            }
        }
        protected void OnDash(InputValue value)
        {
            //TODO:先禁掉冲刺
            if (true)
                return;
            Input_Dash();
        }
        protected void OnFire(InputValue value)
        {
            Input_Attack();
        }
        #endregion

        protected void Update_Input() {

            if (!CanOperate())
            {
                m_isDownArrowClick = false;
                m_isLeftControlClick = false;
                m_isSpaceClick = false;
                m_isUpArrowClick = false;
                return;
            }

            //计算cameraSpot的旋转
            cameraSpot.transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);
            cameraSpot.transform.rotation *= Quaternion.AngleAxis(-_look.y * rotationPower, Vector3.right);

            var angles = cameraSpot.transform.localEulerAngles;
            angles.z = 0;

            var angle = cameraSpot.transform.localEulerAngles.x;

            //限制仰/俯角
            if (angle > 180 && angle < 340)
            {
                angles.x = 340;
            }
            else if (angle < 180 && angle > 40)
            {
                angles.x = 40;
            }

            cameraSpot.transform.localEulerAngles = angles;

            //因为cameraSpot跟transfrom的Parent关系，将cameraSpot的y轴旋转应用到transfrom
            transform.rotation = Quaternion.Euler(0, cameraSpot.transform.rotation.eulerAngles.y, 0);
            //cameraSpot只需要关注x轴旋转
            cameraSpot.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

        }
        public bool canOperate = true;

        public bool CanOperate()
        {
            //TODO:CanOperate
            //if (!IsVaild())
            //    return false;
            //if (!TinyGameManager.instance.isRunning)
            //    return false;
            //if (!TinyGameManager.playerControllable)
            //    return false;
            return canOperate;
        }

        public void Input_Attack()
        {
            if (!CanOperate())
                return;
            if(!CanAttack())
                return;
            Animation_Attack();
        }
        public void Input_UnHang()
        {
            if (!CanOperate())
                return;
            m_isUpArrowClick = true;
        }
        public void Input_Dash()
        {
            if (!CanOperate())
                return;
            if (!CanDash())
                return;
            m_isLeftControlClick = true;
        }
        public void Input_Jump()
        {
            if (!CanOperate())
                return;
            if (!CanJump())
                return;
            m_isSpaceClick = true;
        }
        public bool IsRunning()
        {
            return _run > 0;
        }
        public bool IsMoving()
        {
            return horizontalInput.magnitude > 0;
        }

        public bool CanAttack() 
        {
            return OnGround;
        }
        public bool CanDash()
        {
            if (IsJumping())
                return false;
            return !IsDashing();
        }
        public bool IsDashing()
        {
            return dashCounter > 0;
        }
        public void StopDash()
        {
            dashCounter = 0;
        }
        public bool IsJumping()
        {
            //return rig.velocity.y < -0.05f || rig.velocity.y > 0.05f;
            return !OnGround;
        }
        public bool CanJump()
        {
            return jumpCounter < jumpableTimes && !IsHanging();
        }

        public bool CanHang()
        {
            return false;// IsGrounded() && IsEdgeCollider2DGround();
        }

        private bool m_hanging = false;
        public bool IsHanging()
        {
            return m_hanging;
        }


    }
}
