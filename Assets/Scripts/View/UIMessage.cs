using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button button;

    public void Show(string m, UnityAction act)
    {
        button.onClick.RemoveAllListeners();
        text.text = m;
        gameObject.SetActive(true);
        button.onClick.AddListener(act);
    }
}
