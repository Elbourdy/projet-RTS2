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

        Undo.RecordObject(inspectedObject, "Modified Night Attack Scriptable");

        //NUMBER OF SPAWNERS BY NIGHT INSPECTOR

        int[] tab0 = new int[inspectedObject.numSpawnerActive.Length];
        tab0 = inspectedObject.numSpawnerActive;
        GUILayout.Label("Nombres de portails actifs par nuit");
        int cellWidth = 50;

        GUILayout.BeginHorizontal();
        for(int i = 0; i < tab0.Length; i++)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Night" + i, GUILayout.Width(cellWidth));
            int newValue = EditorGUILayout.IntField("", tab0[i], GUILayout.Width(cellWidth));
            tab0[i] = newValue;

            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();

        inspectedObject.numSpawnerActive = tab0;

        GUILayout.Label("\n");

        //COST BY NIGHT INSPECTOR

        int[] tab1 = new int[inspectedObject.costByNight.Length];
        tab1 = inspectedObject.costByNight;
        GUILayout.Label("Coût par portail par nuit");
        cellWidth = 50;

        GUILayout.BeginHorizontal();
        for (int i = 0; i < tab1.Length; i++)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Night" + i, GUILayout.Width(cellWidth));
            int newValue = EditorGUILayout.IntField("", tab1[i], GUILayout.Width(cellWidth));
            tab1[i] = newValue;

            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();

        inspectedObject.costByNight = tab1;

        GUILayout.Label("\n");

        //NIGHT BEFORE UNIT SPAWN INSPECTOR

        int[] tab2 = new int[inspectedObject.nightBeforeUnitSpawn.Length];
        tab2 = inspectedObject.nightBeforeUnitSpawn;
        GUILayout.Label("Blocage d'unité avant une certaine nuit");
        cellWidth = 100;

        GUILayout.BeginHorizontal();
        for (int i = 0; i < tab2.Length; i++)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Unit" + i, GUILayout.Width(cellWidth));
            int newValue = EditorGUILayout.IntField("", tab2[i], GUILayout.Width(cellWidth));
            tab2[i] = newValue;

            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();

        inspectedObject.nightBeforeUnitSpawn = tab2;

        GUILayout.Label("\n");

        //CUSTOM WAVES INSPECTOR

        int height = 10;
        inspectedObject.height = height;
        int width = 5;
        inspectedObject.width = width;
        int[] tab = new int[50];
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
        for (int i = 0; i < height; i++)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Night" + i, GUILayout.Width(70));

            for (int j = 0; j < width; j++)
            {
                int newValue = EditorGUILayout.IntField("", tab[i * width + j],GUILayout.Width(70));
                tab[i * width + j] = newValue;
            }

            GUILayout.EndHorizontal();
        }

        inspectedObject.customWaves = tab;

        bool userDidSomething = EditorGUI.EndChangeCheck();

        if (userDidSomething) EditorUtility.SetDirty(target);
    }

}
