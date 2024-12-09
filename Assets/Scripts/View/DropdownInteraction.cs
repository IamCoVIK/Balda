using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropdownInteraction : MonoBehaviour
{
    private TMP_Dropdown Dropdown;

    private void Start()
    {
        Dropdown = GetComponent<TMP_Dropdown>();
    }
    public void SetInteraction()
    {
        Dropdown.interactable = !Dropdown.interactable;
    }
}
