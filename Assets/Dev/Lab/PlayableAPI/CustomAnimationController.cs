using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Assertions;
#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

[RequireComponent(typeof(Animator))]
public class CustomAnimationController : MonoBehaviour
{
    public PlayableGraph Graph => m_Graph;

    private PlayableGraph m_Graph;
    private AnimationPlayableOutput m_Output;
    private AnimationMixerPlayable m_Mixer;

    private Animator m_Animator;

    public AnimationClip clip0;
    public AnimationClip clip1;

    public AnimationClip[] clipsToPlay;

    private void Start()
    {
        Reset();
        m_Animator = GetComponent<Animator>();
        Assert.IsNotNull(m_Animator, "Find not <Animator> attached");
        Example01_Start();
    }

    public float transition = 2f;
    private float leftTime;
    private void Update()
    {
        Example01_Update();
    }

    #region Example01: Simple blending animation clip
    private void Example01_Start() 
    {
        m_Graph = PlayableGraph.Create(typeof(CustomAnimationController).Name);
        m_Graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
        m_Output = AnimationPlayableOutput.Create(m_Graph, "Animation", m_Animator);

        m_Mixer = AnimationMixerPlayable.Create(m_Graph, 2);
        m_Output.SetSourcePlayable(m_Mixer);

        AnimationClipPlayable clipPlayable0 = AnimationClipPlayable.Create(m_Graph, clip0);
        AnimationClipPlayable clipPlayable1 = AnimationClipPlayable.Create(m_Graph, clip1);

        m_Graph.Connect(clipPlayable0, 0, m_Mixer, 0);
        m_Graph.Connect(clipPlayable1, 0, m_Mixer, 1);

        m_Mixer.SetInputWeight(0, 1);
        m_Mixer.SetInputWeight(1, 0);

        m_Graph.Play();
    }

    [Range(0f, 1f)] public float blendWeight = 0;
    private bool valueChanged = false;
    private void Example01_Update() {
        m_Mixer.SetInputWeight(0, blendWeight);
        m_Mixer.SetInputWeight(1, 1 - blendWeight);
        if (valueChanged)
        {
            m_Graph.Evaluate();
            valueChanged = false;
        }
        //if (leftTime > 0)
        //{
        //    leftTime = Mathf.Clamp(leftTime - Time.deltaTime, 0, 1);
        //    float weight = leftTime / transition;
        //    m_Mixer.SetInputWeight(0, weight);
        //    m_Mixer.SetInputWeight(1, 1 - weight);
        //}
        //m_Graph.Evaluate(Time.deltaTime);
    }
    private void OnValidate()
    {
        valueChanged = true;
    }
    #endregion

    #region Example02: using PlayableBehaviour to loop animation clips
    void Example02_Start()
    {
        m_Graph = PlayableGraph.Create();

        var custPlayable = ScriptPlayable<CustomAnimationControllerPlayable>.Create(m_Graph);

        var playQueue = custPlayable.GetBehaviour();

        playQueue.Initialize(clipsToPlay, custPlayable, m_Graph);

        var playableOutput = AnimationPlayableOutput.Create(m_Graph, "Animation", m_Animator);

        playableOutput.SetSourcePlayable(custPlayable, 0);

        m_Graph.Play();
    }
    void Example02_Update() 
    {

    }
    #endregion

    public void Reset()
    {
        leftTime = transition;
    }
    void OnDisable()
    {
        m_Graph.Destroy();

    }
}

/// <summary>
/// reference: https://zhuanlan.zhihu.com/p/144851154
/// </summary>
public class CustomAnimationControllerPlayable : PlayableBehaviour 
{
    private int m_CurrentClipIndex = -1;
    private float m_TimeToNextClip = -1;
    private Playable mixer;
    public void Initialize(AnimationClip[] clipsToPlay, Playable owner, PlayableGraph graph)
    {
        owner.SetInputCount(1);
        mixer = AnimationMixerPlayable.Create(graph, clipsToPlay.Length);
        graph.Connect(mixer, 0, owner, 0);
        owner.SetInputWeight(0, 1);
        for (int clipIndex = 0; clipIndex < mixer.GetInputCount(); clipIndex++) 
        {
            graph.Connect(AnimationClipPlayable.Create(graph, clipsToPlay[clipIndex]), 0, mixer, clipIndex);
            mixer.SetInputWeight(clipIndex, 1.0f);
        }
    }
    private float currentClipLength = 1;
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if (mixer.GetInputCount() == 0)
            return;

        m_TimeToNextClip -= (float)info.deltaTime;
        if (m_TimeToNextClip <= 0f)
        {
            m_CurrentClipIndex++;
            if (m_CurrentClipIndex >= mixer.GetInputCount())
                m_CurrentClipIndex = 0;
            var currentClip = (AnimationClipPlayable)mixer.GetInput(m_CurrentClipIndex);

            currentClip.SetTime(0);
            m_TimeToNextClip = currentClip.GetAnimationClip().length;
            currentClipLength = m_TimeToNextClip;
        }

        float weight = m_TimeToNextClip / currentClipLength;
        for (int clipIndex = 0; clipIndex < mixer.GetInputCount(); ++clipIndex) 
        {
            if (clipIndex == m_CurrentClipIndex)
                mixer.SetInputWeight(clipIndex, 1f - weight);
            else
                mixer.SetInputWeight(clipIndex, weight);
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(CustomAnimationController))]
public class CustomAnimatomControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var inst = target as CustomAnimationController;
        if (GUILayout.Button("Show on Visualizer")) 
        {
            if (inst != null && inst.Graph.IsValid()) 
            {
                GraphVisualizerClient.Show(inst.Graph);
            }
        }       
        if (GUILayout.Button("Reset")) 
        {
            if (inst != null && inst.Graph.IsValid()) 
            {
                inst.Reset();
            }
        }
    }
}

#endif //UNITY_EDITOR
