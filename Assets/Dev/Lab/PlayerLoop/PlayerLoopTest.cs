using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.LowLevel;

public class PlayerLoopTest : MonoBehaviour
{
    [Button("ShowPlayerLoop")]
    public void ShowPlayerLoop()
    {
        Init();
    }

    public static void Init()
    {
        StringBuilder sb = new();
        ShowPlayerLoop(PlayerLoop.GetDefaultPlayerLoop(), sb, 0);
        Debug.Log(sb);
    }

    private static void ShowPlayerLoop(PlayerLoopSystem playerLoopSystem, StringBuilder text, int inline)
    {
        if (playerLoopSystem.type != null)
        {
            for (var i = 0; i < inline; i++)
            {
                text.Append("\t");
            }
            text.AppendLine(playerLoopSystem.type.Name);
        }

        if (playerLoopSystem.subSystemList != null)
        {
            inline++;
            foreach (var s in playerLoopSystem.subSystemList)
            {
                ShowPlayerLoop(s, text, inline);
            }
        }
    }
    private static PlayerLoopSystem AddSystem<T>(in PlayerLoopSystem loopSystem, PlayerLoopSystem systemToAdd) where T : struct
    {
        PlayerLoopSystem newPlayerLoop = new()
        {
            loopConditionFunction = loopSystem.loopConditionFunction,
            type = loopSystem.type,
            updateDelegate = loopSystem.updateDelegate,
            updateFunction = loopSystem.updateFunction
        };

        List<PlayerLoopSystem> newSubSystemList = new();

        foreach (var subSystem in loopSystem.subSystemList)
        {
            newSubSystemList.Add(subSystem);

            if (subSystem.type == typeof(T))
                newSubSystemList.Add(systemToAdd);
        }

        newPlayerLoop.subSystemList = newSubSystemList.ToArray();
        return newPlayerLoop;
    }
    public void Update()
    {
        Debug.Log("[Update] PlayerLoopTest");
    }
    public class CustomSystem
    {
        public void Update()
        {
            Debug.Log("[Update] CustomSystem");
        }
    }
}
