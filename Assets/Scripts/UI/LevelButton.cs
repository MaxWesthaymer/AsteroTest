using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private Text levelNumTxt;

    public void SetupLevelButton(int levelId, bool isLocked, Action<int> onClick)
    {
        var button = GetComponent<Button>();
        button.interactable = !isLocked;
        lockIcon.SetActive(isLocked);
        levelNumTxt.text = isLocked ? "" : $"{levelId + 1}";
        button.onClick.AddListener(() =>
        {
            onClick?.Invoke(levelId);
        });
    }
}
