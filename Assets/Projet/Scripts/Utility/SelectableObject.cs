using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    private bool isSelected = false;


    public bool IsSelected 
    {
        get
        {
            return isSelected;
        }
        set
        {
            isSelected = value;
            if (IsSelected)
            {
                onSelected?.Invoke();
            }
            else
            {
                onDeselected?.Invoke();
            }
        }
    }

    public delegate void SelectionEvent();
    public SelectionEvent onSelected;
    public SelectionEvent onDeselected;
    public SelectionEvent onChangeIsSelected;

    public bool canBeMultiSelected = false;


    [Header("Feedbacks")]
    public GameObject visualFeedback;


    private void OnEnable()
    {
        HideVisualFeedback();
        NewSelectionManager.instance.selectableList.Add(this);
        onSelected += ShowVisualFeeback;
        onDeselected += HideVisualFeedback;
    }

    private void OnDisable()
    {
        NewSelectionManager.instance.selectableList.Remove(this);
        onSelected -= ShowVisualFeeback;
        onDeselected -= HideVisualFeedback;
    }


    public void ShowVisualFeeback()
    {
        if (visualFeedback)
        visualFeedback.SetActive(true);
    }

    public void HideVisualFeedback()
    {
        if(visualFeedback)
        visualFeedback.SetActive(false);
    }

}
