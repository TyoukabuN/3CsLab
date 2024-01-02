using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

namespace TinyGame
{
    public partial class TEntity: MonoBehaviour, IAnimatorEventReceiver
    {
        [SerializeField] public Animator mainAnimator;
        [SerializeField] public AnimancerComponent animancer;
        [SerializeField] public AnimatorAgency animatorAgency;
        //
        //[SerializeField] public AnimatiomClipSet animationClipSet;
        [SerializeField] public AnimatiomClipTransitionSet animationClipTransitionSet;
        protected virtual void Init_Animation()
        {
            mainAnimator = GetComponentInChildren<Animator>();
            animancer = GetComponentInChildren<AnimancerComponent>();
            //
            animatorAgency = GetComponentInChildren<AnimatorAgency>();
            animatorAgency.animatorEventReceiver = this;
        }
        public void Animator_SetEnabled(bool enabled)
        {
            if (mainAnimator)//|| !animator.HasState(layerIndex, stateId)
                mainAnimator.enabled = enabled;
            foreach (var animator in subAnimators)
                if (animator)
                    animator.enabled = enabled;
        }
        protected void CheckAniamtor()
        {
            if (TinyGameManager.instance.isPause)
                Animator_SetEnabled(false);
            else
                Animator_SetEnabled(true);
        }

        #region Animancer
        public bool TryGetClip(string clipName,out ClipTransition clip)
        {
            clip = null;
            if (animationClipTransitionSet == null || animationClipTransitionSet.clips == null)
                return false;
            return animationClipTransitionSet.clips.TryGetValue(clipName, out clip);
        }
        public AnimancerState Animancer_Play(string clipName)
        {
            if (animancer == null) return null;
            if (TryGetClip(clipName, out ClipTransition clip)) { 
                return animancer.Play(clip);
            }
            return null;
        }
        public AnimancerState Animancer_Play(string clipName, float transition = 0.25f, FadeMode mode = default)
        {
            if (animancer == null) return null;
            if (TryGetClip(clipName, out ClipTransition clip))
            {
                return animancer.Play(clip, transition, mode);
            }
            return null;
        }

        #endregion


        #region Animator
        public void Animator_Play(string stateName, int layerIndex = -1, float normalizedTime = 0)
        {
            Animator_Play(Animator.StringToHash(stateName), layerIndex, normalizedTime);
        }
        public void Animator_Play(int stateId, int layerIndex = -1, float normalizedTime = 0)
        {
            if (mainAnimator)//|| !animator.HasState(layerIndex, stateId)
                mainAnimator.Play(stateId, layerIndex, normalizedTime);

            foreach (var animator in subAnimators)
                if (animator)
                    animator.Play(stateId, layerIndex, normalizedTime);

        }
        public void Animator_SetTrigger(string name)
        {
            if (mainAnimator)
                mainAnimator.SetTrigger(name);

            foreach (var animator in subAnimators)
                if (animator)
                    animator.SetTrigger(name);
        }
        public void Animator_SetTrigger(int nameHash)
        {
            if (mainAnimator)
                mainAnimator.SetTrigger(nameHash);

            foreach (var animator in subAnimators)
                if (animator)
                    animator.SetTrigger(nameHash);
        }
        public void Animator_SetInt(string name, int intValue)
        {
            if (mainAnimator)
                mainAnimator.SetInteger(name, intValue);

            foreach (var animator in subAnimators)
                if (animator)
                    animator.SetInteger(name, intValue);
        }
        public void Animator_SetInt(int nameHash, int intValue)
        {
            if (mainAnimator)
                mainAnimator.SetInteger(nameHash, intValue);

            foreach (var animator in subAnimators)
                if (animator)
                    animator.SetInteger(nameHash, intValue);
        }
        public void Animator_SetFloat(string name, float floatValue)
        {
            if (mainAnimator)
                mainAnimator.SetFloat(name, floatValue);

            foreach (var animator in subAnimators)
                if (animator)
                    animator.SetFloat(name, floatValue);
        }
        public void Animator_SetFloat(int nameHash, float floatValue)
        {
            if (mainAnimator)
                mainAnimator.SetFloat(nameHash, floatValue);

            foreach (var animator in subAnimators)
                if (animator)
                    animator.SetFloat(nameHash, floatValue);
        }
        public void Animator_SetSpeed(float value = 1)
        {
            if (mainAnimator)
                mainAnimator.speed = value;

            foreach (var animator in subAnimators)
            {
                if (animator)
                {
                    animator.speed = value;
                }
            }
        }
        public void Animator_AddSubAnimator(Animator animator)
        {
            if (subAnimators.Contains(animator))
                return;
            subAnimators.Add(animator);
        }
        public virtual void OnAnimatorIK()
        {

        }
        public virtual void OnAnimatorMove()
        {

        }
        #endregion
    }
}
