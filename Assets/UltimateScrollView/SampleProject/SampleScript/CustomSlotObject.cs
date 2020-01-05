using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomSlotObject : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private Button button;

    public void SetText(string p_content) {
        if (text != null)
            text.text = p_content;
    }

    public void SetButtonEvent(System.Action p_action) {
        if (button != null) {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(()=> { p_action(); });
        }
    }

}
