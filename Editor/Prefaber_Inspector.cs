#if UNITY_EDITOR
using FTGAMEStudio.InitialFramework.Replicator;
using UnityEditor;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    [CustomEditor(typeof(Prefaber))]
    public class Prefaber_Inspector : IPrefabbing_Inspector
    {
        public override void OnInspectorGUI()
        {
            Prefaber target = this.target as Prefaber;

            if (target.replicator == null || target.prefabs == null)
            {
                target.replicator = EditorGUILayout.ObjectField("Replicator", target.replicator, typeof(GameObjectReplicator), true) as GameObjectReplicator;
                target.prefabs = EditorGUILayout.ObjectField("预制件", target.prefabs, typeof(Prefabs), true) as Prefabs;

                EditorGUILayout.Space();

                EditorGUILayout.HelpBox("在此之前，您必须完成以上字段。", MessageType.Warning);

                return;
            }

            base.OnInspectorGUI();

            if (GUILayout.Button("Instantiate"))
            {
                if (EditorUtility.DisplayDialog("Instantiate?", $"您无法撤销 Instantiate 与它带来的文件读写操作。", "确定", "取消")) target.Instantiate();
            }
        }
    }
}
#endif
