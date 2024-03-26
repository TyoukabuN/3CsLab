using UnityEngine;
using System.Collections;
using System;

namespace RootMotion.Dynamics {
	
	// Switching and blending between Modes
	public partial class PuppetMaster: MonoBehaviour
	{
		// public event UpdateDelegate OnNextModeSwitchToActive;
        /// <summary>
        /// Switches this PuppetMaster to PuppetMaster.Mode.Active.
        /// </summary>
        public void SwitchToActiveMode()
        {
            mode = Mode.Active;
        }

        public void SwitchToActiveModeImmediate()
        {
	        mode = Mode.Active;
	        SwitchModes(true);
        }
        
        /// <summary>
        /// Switches this PuppetMaster to PuppetMaster.Mode.Kinematic.
        /// </summary>
        public void SwitchToKinematicMode()
        {
            mode = Mode.Kinematic;
        }

        /// <summary>
        /// Switches this PuppetMaster to PuppetMaster.Mode.Disabled.
        /// </summary>
        public void SwitchToDisabledMode()
        {
            mode = Mode.Disabled;
        }

		/// <summary>
		/// Returns true if the PuppetMaster is in the middle of blending from a mode to mode.
		/// </summary>
		public bool isSwitchingMode { get; private set; }

		#region Modified By Hunter (jb) -- 2022年6月28日

		public bool isSwitchingOrAboutToSwitchMode
		{
			get => isSwitchingMode || mode != lastMode;
		}

		#endregion

		#region Modified By Hunter (jb) -- 2023年4月13日

		//卡了一个bug原来是这边协程会把根节点的刚体设为simulated
		/// <summary>
		/// switch mode的时候不设置root
		/// </summary>
		// public bool setRootMuscleMappingMaxOnNextActivation { get; set; }

		// public bool NextActivationIsImmdiate { get; set; }
		
		#endregion
		
		private Mode activeMode;
		private Mode lastMode;

		private float mappingBlend;
		// {
		// 	get => __mappingBlend;
		// 	set
		// 	{
		// 		// Debug.Log($"setting mapping blend :{value}");
		// 		__mappingBlend = value;
		// 	}
		// }
		// private float __mappingBlend;

		/// <summary>
		/// Disables the Puppet immediately without waiting for normal mode switching procedures.
		/// </summary>
		public void DisableImmediately() {
			mappingBlend = 0f;
			isSwitchingMode = false;
			mode = Mode.Disabled;
			activeMode = mode;
			lastMode = mode;

			foreach (Muscle m in muscles) {
				if (m.rigidbody == null)
				{
					Debug.LogWarning("muscle missing", this);
					continue;
				}
				m.rigidbody.gameObject.SetActive(false);
			}
		}

		// Master controller for switching modes. Mode switching is done by simply changing PuppetMaster.mode and can not be interrupted.
		protected virtual void SwitchModes(bool immediate = false) {
			if (!initiated) return;
			if (isKilling) mode = Mode.Active;
			if (!isAlive) mode = Mode.Active;
			

			foreach (BehaviourBase behaviour in behaviours) {
				if (behaviour.forceActive) {
					mode = Mode.Active;
					break;
				}
			}

			if (mode == lastMode) return;
			if (isSwitchingMode) return;
			if (isKilling && mode != Mode.Active) return;
			if (state != State.Alive && mode != Mode.Active) return;

            // Enable state switching here or else mapping won't be blended correctly

            isSwitchingMode = true;
			
			if (lastMode == Mode.Disabled) {
                if (mode == Mode.Kinematic) DisabledToKinematic();
                else if (mode == Mode.Active)
                {
	                if (immediate)
	                {
		                DisableToActive_Immediate();
	                }
	                else
	                {
		                StartCoroutine(DisabledToActive());
	                }
	                
                }
			}
			
			else if (lastMode == Mode.Kinematic) {
				if (mode == Mode.Disabled) KinematicToDisabled();
				else if (mode == Mode.Active)
				{
					if (immediate)
					{
						KinematicToActive_Immediate();
					}
					else
					{
						StartCoroutine(KinematicToActive());
					}
					
				}
			}
			
			else if (lastMode == Mode.Active) {
				if (immediate)
				{
					throw new NotImplementedException();
				}
				if (mode == Mode.Disabled) StartCoroutine(ActiveToDisabled());
				else if (mode == Mode.Kinematic) StartCoroutine(ActiveToKinematic());
			}
			
			lastMode = mode;
		}

		// Switch from Disabled to Kinematic mode
		private void DisabledToKinematic() {
			foreach (Muscle m in muscles) {
                if (!m.state.isDisconnected) m.Reset();
			}
			
			foreach (Muscle m in muscles) {
                if (!m.state.isDisconnected)
                {
                    m.rigidbody.gameObject.SetActive(true);
                    m.SetKinematic(true);
                }
			}

            FlagInternalCollisionsForUpdate();

            foreach (Muscle m in muscles) {
                if (!m.state.isDisconnected) m.MoveToTarget();
			}
			
			activeMode = Mode.Kinematic;
			isSwitchingMode = false;
		}

		// Blend from Disabled to Active mode
		private IEnumerator DisabledToActive()
		{
			ProcessMuscles_DisableToActive();

            if (blendTime > 0f)
            {
                while (mappingBlend < 1f)
                {
                    mappingBlend = Mathf.Clamp(mappingBlend + Time.deltaTime / blendTime, 0f, 1f);
                    yield return null;
                }
            } else
            {
                mappingBlend = 1f;
            }
			
			activeMode = Mode.Active;
			isSwitchingMode = false;
		}

		private void DisableToActive_Immediate()
		{
			ProcessMuscles_DisableToActive();
			mappingBlend = 1f;
			activeMode = Mode.Active;
			isSwitchingMode = false;
		}

		private void ProcessMuscles_DisableToActive()
		{
			foreach (Muscle m in muscles) {
				if (!m.state.isDisconnected) m.Reset();
			}

			foreach (Muscle m in muscles) {
				if (!m.state.isDisconnected)
				{
					m.rigidbody.gameObject.SetActive(true);
					// if (m.index == 0 && setRootMuscleMappingMaxOnNextActivation)
					// {
					// 	m.props.mappingWeight = 1f;
					// 	setRootMuscleMappingMaxOnNextActivation = false;
					// }
					m.SetKinematic(false);
					m.rigidbody.WakeUp();
					m.rigidbody.velocity = m.mappedVelocity;
					m.rigidbody.angularVelocity = m.mappedAngularVelocity;
				}
			}
            

			FlagInternalCollisionsForUpdate();

			foreach (Muscle m in muscles) {
				if (!m.state.isDisconnected) m.MoveToTarget();
			}

			Read();
		}

		// Switch from Kinematic to Disabled
		private void KinematicToDisabled() {
			foreach (Muscle m in muscles) {
                if (!m.state.isDisconnected)
                {
                    m.rigidbody.gameObject.SetActive(false);
                }
			}
			
			activeMode = Mode.Disabled;
			isSwitchingMode = false;
		}


		// Blend from Kinematic to Active mode
		private IEnumerator KinematicToActive()
		{
			ProcessMuscles_KinematicToActive();

            if (blendTime > 0f)
            {
                while (mappingBlend < 1f)
                {
                    mappingBlend = Mathf.Clamp(mappingBlend + Time.deltaTime / blendTime, 0f, 1f);
                    yield return null;
                }
            }
            else
            {
                mappingBlend = 1f;
            }

            activeMode = Mode.Active;
            isSwitchingMode = false;
		}

		private void KinematicToActive_Immediate()
		{
			ProcessMuscles_KinematicToActive();
			mappingBlend = 1f;
			activeMode = Mode.Active;
			isSwitchingMode = false;
		}
		
		private void ProcessMuscles_KinematicToActive()
		{
			
			foreach (Muscle m in muscles) {
				if (!m.state.isDisconnected)
				{
					// if (m.index == 0 && setRootMuscleMappingMaxOnNextActivation)
					// {
					// 	m.props.mappingWeight = 1f;
					// 	setRootMuscleMappingMaxOnNextActivation = false;
					// }
					m.SetKinematic(false);
					m.rigidbody.WakeUp();
					m.rigidbody.angularVelocity = m.mappedAngularVelocity;
					
				}
			}

			foreach (Muscle m in muscles) {
				if (!m.state.isDisconnected)
				{
					m.MoveToTarget();
				}
			}

			Read();
		}
		
		// Blend from Active to Disabled mode
		private IEnumerator ActiveToDisabled() {
            if (blendTime > 0f)
            {
                while (mappingBlend > 0f)
                {
                    mappingBlend = Mathf.Max(mappingBlend - Time.deltaTime / blendTime, 0f);
                    yield return null;
                }
            } else
            {
                mappingBlend = 0f;
            }
			
			foreach (Muscle m in muscles) {
                if (!m.state.isDisconnected)
                {
                    m.rigidbody.gameObject.SetActive(false);
                }
			}

			activeMode = Mode.Disabled;
			isSwitchingMode = false;
		}

		// Blend from Active to Kinematic mode
		private IEnumerator ActiveToKinematic() {
            if (blendTime > 0f)
            {
                while (mappingBlend > 0f)
                {
                    mappingBlend = Mathf.Max(mappingBlend - Time.deltaTime / blendTime, 0f);
                    yield return null;
                }
            } else
            {
                mappingBlend = 0f;
            }
			
			foreach (Muscle m in muscles) {
                if (!m.state.isDisconnected) m.SetKinematic(true);
			}
			
			foreach (Muscle m in muscles) {
                if (!m.state.isDisconnected) m.MoveToTarget();
			}

			activeMode = Mode.Kinematic;
			isSwitchingMode = false;
		}
	}
}