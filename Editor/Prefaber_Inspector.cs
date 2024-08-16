#if UNITY_EDITOR
using FTGAMEStudio.InitialSolution.Persistence;
using UnityEditor;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    [CustomEditor(typeof(Prefaber))]
    public class Prefaber_Inspector : IPersistableObject_Inspector
    {
        public override void OnInspectorGUI()
        {
            Prefaber target = this.target as Prefaber;

            if (target.data == null)
            {
                target.data = EditorGUILayout.ObjectField("数据", target.data, typeof(Prefab), false) as Prefab;

                EditorGUILayout.Space();

                EditorGUILayout.HelpBox("Before you can do this, you must select a Prefab data.", MessageType.Info);

                return;
            }

            base.OnInspectorGUI();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("LoadPrefab"))
            {
                if (EditorUtility.DisplayDialog("读取选定文件且加载数据？", $"{target.FileLocation.FullName}\n\n您无法撤销此操作。", "确定", "取消")) target.LoadPrefab();
            }

            if (GUILayout.Button("ToPrefab"))
            {
                if (EditorUtility.DisplayDialog("写入选定文件？", $"{target.FileLocation.FullName}\n\n您无法撤销文件写入操作。", "确定", "取消")) target.ToPrefab();
            }

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Instantiate"))
                target.SingleInstantiate();
        }
    }
}
#endif