#if UNITY_EDITOR
using FTGAMEStudio.InitialSolution.Persistence;
using UnityEditor;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    public abstract class IPrefabbing_Inspector : IPersistableObject_Inspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            IPrefabbing target = this.target as IPrefabbing;

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Enprefab"))
            {
                if (EditorUtility.DisplayDialog("Enprefab?", $"���޷����� Enprefab �����������ļ���д������", "ȷ��", "ȡ��")) target.Enprefab();
            }

            if (GUILayout.Button("Deprefab"))
            {
                if (EditorUtility.DisplayDialog("Deprefab?", $"���޷����� Deprefab �����������ļ���д������", "ȷ��", "ȡ��")) target.Deprefab();
            }

            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (target.DeepOperation) EditorGUILayout.HelpBox("���� Deep Operation ���ܵ�������������ջ�����", MessageType.Warning);
        }
    }
}
#endif