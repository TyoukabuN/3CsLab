using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;

public class XSimpleTypeEffect : MonoBehaviour
{
    private TextMeshProUGUI DialogContentText;

    public string Content = string.Empty;
    [Tooltip("打字机效果每秒的打字速度")] public float secondPerChar = 5;
    [Tooltip("支持色码")] public bool UseColorCode = false;

    private float _timeRecorder;
    private string _curContent = string.Empty;
    private bool _colorChange;

    private bool m_onCompleteTrigger = false;

    public Action onComplete;

    // Start is called before the first frame update
    void Awake()
    {
        DialogContentText = GetComponent<TextMeshProUGUI>();
    }
    public void Play(string context)
    {
        Content = context;
        if (!DialogContentText)
            return;
        DialogContentText.text = string.Empty;
        _curContent = string.Empty;
        _timeRecorder = 0.0f;
        m_onCompleteTrigger = true;
    }
    public void Play()
    {
        Play(Content);
    }

    public bool isDone
    {
        get { return _curContent.Length >= Content.Length; }
    }

    public void Finish()
    {
        _curContent = Content;
        UpdateDialogText();
    }

    public void CheckComplete()
    {
        if (!isDone)
            return;
        if (!m_onCompleteTrigger)
            return;
        m_onCompleteTrigger = false;
        if (onComplete != null)
        {
            try
            {
                onComplete.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckComplete();

        if (_curContent.Length >= Content.Length)
            return;

        if (!DialogContentText)
            return;

        if (DialogContentText.text.Length >= Content.Length)
            return;

        _timeRecorder += Time.unscaledDeltaTime;
        if (_timeRecorder >= secondPerChar)
        {
            if (!UseColorCode)
            {
                //没有对色码提供打字机效果的支持
                DialogContentText.text = DialogContentText.text.Insert(DialogContentText.text.Length, Content[DialogContentText.text.Length].ToString());
                _timeRecorder = 0;
                return;
            }

            //对色码提供打字机效果的支持
            if (Content[_curContent.Length].ToString() == "<")
            {
                if (_colorChange)
                {
                    _colorChange = false;
                    _curContent = _curContent.Insert(_curContent.Length, "</color>");
                    UpdateDialogText();
                    return;
                }

                //检验正则
                //Regex regexColorCode = new Regex(@"<[cC][oO][lL][oO][rR]=\#[0-f]{6}>|<[cC][oO][lL][oO][rR]=\#[0-f]{8}>|<[cC][oO][lL][oO][rR]=\w*>");
                Regex regexColorCode =
                    new Regex(
                        @"^<[cC][oO][lL][oO][rR]=\#[0-f]{6}>$|^<[cC][oO][lL][oO][rR]=\#[0-f]{8}>$|^<[cC][oO][lL][oO][rR]=\w*>$");

                int originIndex = DialogContentText.text.Length;

                string colorCodeHead = "<";
                bool isFull = true;
                while (!regexColorCode.IsMatch(colorCodeHead) && colorCodeHead.Length <= 18)
                {
                    if (originIndex + colorCodeHead.Length > Content.Length)
                    {
                        isFull = false;
                        //防越界检测
                        break;
                    }
                    colorCodeHead = colorCodeHead.Insert(colorCodeHead.Length,
                        Content[originIndex + colorCodeHead.Length].ToString());
                }
                if (isFull)
                {
                    _curContent = _curContent.Insert(_curContent.Length, colorCodeHead);
                    _curContent = _curContent.Insert(_curContent.Length,
                        Content[_curContent.Length].ToString());
                    _colorChange = true;
                    _timeRecorder = 0;
                    UpdateDialogText();
                    return;
                }

            }
            _curContent = _curContent.Insert(_curContent.Length, Content[_curContent.Length].ToString());
            _timeRecorder = 0;
            UpdateDialogText();
            return;
        }
    }

    public void UpdateDialogText()
    {
        if (!DialogContentText)
            return;
        DialogContentText.text = _curContent + (_colorChange ? "</color>" : "");
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(XSimpleTypeEffect))]
public class XSimpleTypeEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        XSimpleTypeEffect instance = target as XSimpleTypeEffect;

        if (GUILayout.Button("Play"))
        {
            instance.Play();
        }
    }

}
#endif
