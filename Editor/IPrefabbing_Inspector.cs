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
                if (EditorUtility.DisplayDialog("Enprefab?", $"您无法撤销 Enprefab 与它带来的文件读写操作。", "确定", "取消")) target.Enprefab();
            }

            if (GUILayout.Button("Deprefab"))
            {
                if (EditorUtility.DisplayDialog("Deprefab?", $"您无法撤销 Deprefab 与它带来的文件读写操作。", "确定", "取消")) target.Deprefab();
            }

            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (target.DeepOperation) EditorGUILayout.HelpBox("启用 Deep Operation 可能导致性能问题或堆栈溢出。", MessageType.Warning);
        }
    }
}
#endif