using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[ExecuteInEditMode()]
public class CompanionDisplay : MonoBehaviour
{
    public static CompanionDisplay Instance { get; set; }

    public float alpha = 0;

    CanvasGroup group;
    void OnEnable()
    {
        group = GetComponent<CanvasGroup>();
        Instance = this;
        Hide();
    }
    public void Show()
    {
        alpha = 1;
    }
    public void Hide()
    {
        alpha = 0;
    }

    void Update()
    {
        if (Mathf.Abs(group.alpha - alpha) > 0.01f)
        {
            group.alpha = Mathf.MoveTowards(group.alpha, alpha, Time.deltaTime * 2);
            if(Mathf.Abs(group.alpha - alpha) < 0.01f)
                group.alpha = alpha;
        }
        
    }
}
