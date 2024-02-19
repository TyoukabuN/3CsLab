using System.Collections;
using System.Collections.Generic;
using TinyGame;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using UnityEngine.InputSystem;
using Animancer.FSM;
using Animancer;

namespace TinyGame
{
    public static class EPlayerState
    {
        public static int None = 0;        
        public static int Stand = 1;
        public static int Walk = 2;
        public static int Running = 3;
        public static int Jump_Begin = 4;
        public static int Jump_Falling = 5;
        public static int Jump_Land = 6;
        public static int Dushing = 7;
        public static int End = 8;
    }

    public abstract class State
    {
        public enum Phase
        { 
            Running,
            End,
        }
        public Phase phase;

        public StateMachineEntity entity;

        public State() {  OnInit(); }
        public virtual void HandleInput(StateMachineEntity entity) { }
        public virtual void Update(StateContext stateContext) { }
        public virtual void FixedUpdate(StateContext stateContext) { }
        public virtual bool CanChange(int from) { return true; }
        public virtual float normalizeTime {  get { return 1f; } }

        #region Phase
        protected void ToPhaseEnd() { phase = Phase.End; }
        public virtual void OnInit() { }
        public virtual void OnEnter(StateMachineEntity entity) { phase = Phase.Running; this.entity = entity; }
        public virtual void OnUpdate() { }
        public virtual void OnChange(int from) { }
        public virtual void OnFinish() { }
        #endregion

        #region Input System Message
        public virtual void OnMove(InputValue value) { }
        public virtual void OnLook(InputValue value) { }
        public virtual void OnRun(InputValue value) { }
        public virtual void OnJump(InputValue value) { }
        public virtual void OnDash(InputValue value) { }
        public virtual void OnFire(InputValue value) { }
        #endregion

        #region Entity Collision
        public virtual void OnGrounded() { }
        #endregion
    }

    public class StateMachineEntity : TEntity
    {
        [SerializeField]
        protected int currentEState = EPlayerState.None;

        protected State[] states;
        protected StateContext _stateContext;

        protected Dictionary<int, Transition[]> state2transition;

        public StateContext stateContext
        {
            get { 
                if (_stateContext == null)
                    _stateContext = new StateContext();
                return _stateContext; 
            }
        }
        protected virtual bool State_Change(int ePlayerState) { return true; }

        protected virtual void Update_State()
        {
            State_UpdateContext();
            if (currentEState != 0)
            {
                var state = states[currentEState];
                if (state != null)
                {
                    state.Update(stateContext);
                    if (CheckTransition(currentEState, out int toState))
                    {
                        State_Change(toState);
                    }
                }
            }
        }
        protected virtual void State_UpdateContext(){ }
        public bool CheckTransition(int state,out int toState)
        {
            toState = 0; 
            if (!state2transition.TryGetValue(state, out var transitions))
                return false;
            for (int i = 0; i < transitions.Length; i++)
            {
                if (transitions[i].Check(states[state])) {
                    toState = transitions[i].toState;
                    if (states[toState] == null)
                        continue;
                    return true;
                }
            }
            return false;
        }

    }

    public abstract class Transition
    {
        public int toState = -1;
        public bool inverse = false;
        public Transition(int toState)
        { 
            this.toState = toState;
        }
        public abstract bool Check(State state);
    }
    public class Trans_OnGrounded : Transition
    {
        private float checkableNormalizeTime = 0;
        public Trans_OnGrounded(int toState, float checkableNormalizeTime = 0f) : base(toState)
        {
            this.checkableNormalizeTime = checkableNormalizeTime;
        }
        public override bool Check(State state)
        {
            if (state.normalizeTime < checkableNormalizeTime)
                return false;
            return state.entity.stateContext.grounded > 0;
        }
    }
    public class Trans_OnStateFinish : Transition
    {
        private float checkableNormalizeTime = 0;
        public Trans_OnStateFinish(int toState,float checkableNormalizeTime = 0f) : base(toState) {
            this.checkableNormalizeTime = checkableNormalizeTime;
        }
        public override bool Check(State state)
        {
            if (state.normalizeTime < checkableNormalizeTime)
                return false;
            return state.phase == State.Phase.End;
        }
    }
    public class Trans_OnWalking : Transition
    {
        public Trans_OnWalking(int toState) : base(toState) { }

        public override bool Check(State state)
        {
            return state.entity.stateContext.inputAxi.magnitude > 0;
        }
    }
    public class Trans_OnRunning : Transition
    {
        public Trans_OnRunning(int toState,bool inverse = false) : base(toState) {
            this.inverse = inverse;
        }

        public override bool Check(State state)
        {
            var context = state.entity.stateContext;
            if (context.inputAxi.magnitude > 0)
            {
                return inverse ? (context.runValue <= 0) : context.runValue > 0;
            }
            return inverse;
        }
    }

    public class StateContext
    {
        public Vector2 inputAxi;
        public Vector2 mouseDelta;
        public float runValue;
        public int grounded;
        struct State { }
    }

    public struct DirectionNameSet { public string F, FL, FR, B, BL, BR; }

    public static class AnimationNameSet 
    {
        public const string IDLE = "Idle";

        public const string JUMP_START = "Jump_Start";
        public const string JUMP_KEEP = "Jump_Keep";
        public const string JUMP_LAND_W = "Jump_Land_Wait";
        public const string JUMP_LAND_M = "Jump_Land_Move";

        public const string WALK_F = "Walk_Front";
        public const string WALK_FL = "Walk_Front_L";
        public const string WALK_FR = "Walk_Front_R";
        public const string WALK_B = "Walk_Back";
        public const string WALK_BL = "Walk_Back_L";
        public const string WALK_BR = "Walk_Back_R";

        public const string DASH_F = "Dash_Front";
        public const string DASH_FL = "Dash_Front_L";
        public const string DASH_FR = "Dash_Front_R";
        public const string DASH_B = "Dash_Back";
        public const string DASH_BL = "Dash_Back_L";
        public const string DASH_BR = "Dash_Back_R";


        public static DirectionNameSet Walk = new DirectionNameSet()
        {
            F = WALK_F,
            FL = WALK_FL,
            FR = WALK_FR,
            B = WALK_B,
            BL = WALK_BL,
            BR = WALK_BR,
        };
        public static DirectionNameSet Dash = new DirectionNameSet()
        {
            F = DASH_F,
            FL = DASH_FL,
            FR = DASH_FR,
            B = DASH_B,
            BL = DASH_BL,
            BR = DASH_BR,
        };
    }
}