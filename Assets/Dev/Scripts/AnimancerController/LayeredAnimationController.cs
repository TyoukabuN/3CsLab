using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using TinyGame;

public class LayeredAnimationController : MonoBehaviour
{
    public List<LayeredOverrideConfigAsset> ConfigAssets = new List<LayeredOverrideConfigAsset>();

    private AnimancerComponent animancerComponent;

    private void Start()
    {
        var compRef = new ComponentReference(gameObject);
        
        for (int i = 0; i < ConfigAssets.Count; i++)
        {
            var configAsset = ConfigAssets[i];
            var layerIndex = configAsset.LayerIndex;
            var configs = configAsset.Configs;
            if (layerIndex < 0)
                continue;
            var transition = new LayeredMixerTransition(layerIndex, configs, compRef);
            var mixer = compRef.animancerComponent.Layers[layerIndex].Play(transition);
        }
    }
}

public class ComponentReference
{
    public GameObject attachedGobj;

    public TPlayerEntity playerEntity;
    public AnimancerComponent animancerComponent;

    public ComponentReference(GameObject gobj)
    {
        attachedGobj = gobj;
        CollectComponent();
    }

    public void CollectComponent()
    {
        playerEntity = attachedGobj.GetComponentInParent<TPlayerEntity>();
        animancerComponent = attachedGobj.GetComponent<AnimancerComponent>();
    }
}

namespace Animancer
{
    public class LayeredMixerTransition : AnimancerTransition<LayeredMixerState>
    {
        public override float MaximumDuration => throw new System.NotImplementedException();

        private List<AnimationClip> _Animation;
        public ref List<AnimationClip> Animations => ref _Animation;

        private List<LayeredOverrideConfig> _Configs;

        public ref List<LayeredOverrideConfig> Configs => ref _Configs;

        private ComponentReference _CompRef;
        public ComponentReference CompRef => _CompRef;

        private int _LayerIndex;
        private int LayerIndex => _LayerIndex;

        public LayeredMixerTransition(int layerIndex,List<LayeredOverrideConfig> configs,ComponentReference compRef)
        {
            _LayerIndex = layerIndex;
            _Configs = configs;
            _CompRef = compRef;

        }
        public void InitializeAnimationClip()
        {
            _Animation = new List<AnimationClip>(_Configs.Count);
            for (int i = 0; i < _Configs.Count; i++)
            {
                var config = _Configs[i];
                _Animation.Add(config.Clip);
            }
        }

        public override LayeredMixerState CreateState()
        {
            State = new LayeredMixerState();
            InitializeState();
            return State;
        }

        public virtual void InitializeState()
        {
            InitializeAnimationClip();

            var mixer = State;
            
            try
            {
                mixer.Initialize(_Animation);
            }
            catch
            {
            }
        }

        public override void Apply(AnimancerState state)
        {
            base.Apply(state);

            var mixer = (LayeredMixerState)state;
            if (mixer == null)
                return;
            mixer._Configs = _Configs;
            mixer._CompRef = _CompRef;
            mixer._LayerIndex = _LayerIndex;
        }
    }

    public class LayeredMixerState : ManualMixerState
    {
        public List<LayeredOverrideConfig> _Configs;
        public ComponentReference _CompRef;
        public int _LayerIndex;
        public void Initialize(List<AnimationClip> clips)
        {
#if UNITY_ASSERTIONS
            if (clips == null)
                throw new ArgumentNullException(nameof(clips));
#endif

            var count = clips.Count;
            Initialize(count);

            for (int i = 0; i < count; i++)
            {
                var clip = clips[i];
                if (clip != null)
                    CreateChild(i, clip);
            }

        }
        protected override void Update(out bool needsMoreUpdates)
        {
            base.Update(out needsMoreUpdates);
            needsMoreUpdates = true;


        }
    }

    public class LogicTriggerBase
    {
        public bool _isOn = false;
        public bool isOn
        {
            get {
                return _isOn;
            }
            set {
                bool before = _isOn;
                if (before == value)
                    return;
                _isOn = value;

                if (OnValueChanged != null)
                    OnValueChanged(before, _isOn);
            }
        }
        public event Action<bool,bool> OnValueChanged;

        public virtual bool OnTriggerEnter() { return false; }
    }
    public class InputTrigger : LogicTriggerBase
    {
    }
}
