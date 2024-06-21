using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private Button _selectButton;

    private void Awake()
    {
        _selectButton.onClick.AddListener(ButtonClick);
    }

    private void ButtonClick()
    {
        
    }
}
