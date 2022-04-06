using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(NightAttackScriptable))]
public class NightAttackScriptableInspector : Editor
{
    NightAttackScriptable inspectedObject;

    public void OnEnable()
    {
        inspectedObject = target as NightAttackScriptable;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();

        int[,] tab = new int[inspectedObject.customWaves.GetLength(0), inspectedObject.customWaves.GetLength(1)];
        tab = inspectedObject.customWaves;

        GUILayout.Label("Custom Waves");

        GUILayout.BeginHorizontal();
        GUILayout.Label("", GUILayout.Width(70));
        GUILayout.Label("IsScripted", GUILayout.Width(70));
        GUILayout.Label("Unit0", GUILayout.Width(70));
        GUILayout.Label("Unit1", GUILayout.Width(70));
        GUILayout.Label("Unit2", GUILayout.Width(70));
        GUILayout.Label("Unit3", GUILayout.Width(70));

        GUILayout.EndHorizontal();
        for (int i = 0; i < tab.GetLength(0); i++)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Night" + i, GUILayout.Width(70));

            for (int j = 0; j < tab.GetLength(1); j++)
            {
                int newValue = EditorGUILayout.IntField("", tab[i, j],GUILayout.Width(70));
                tab[i, j] = newValue;
            }

            GUILayout.EndHorizontal();
        }
        inspectedObject.customWaves = tab;

        bool userDidSomething = EditorGUI.EndChangeCheck();

        if (userDidSomething) EditorUtility.SetDirty(target);
    }
}
