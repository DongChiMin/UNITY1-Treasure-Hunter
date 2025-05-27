using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentState;
    [SerializeField] PlayerStateMachine player;

    private void Update()
    {
        string ans = player.GetCurrentState().ToString();
        currentState.text = ans.Substring(6);
    }
}
