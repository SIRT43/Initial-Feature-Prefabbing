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
                target.prefabs = EditorGUILayout.ObjectField("Ԥ�Ƽ�", target.prefabs, typeof(Prefabs), true) as Prefabs;

                EditorGUILayout.Space();

                EditorGUILayout.HelpBox("�ڴ�֮ǰ����������������ֶΡ�", MessageType.Warning);

                return;
            }

            base.OnInspectorGUI();

            if (GUILayout.Button("Instantiate"))
            {
                if (EditorUtility.DisplayDialog("Instantiate?", $"���޷����� Instantiate �����������ļ���д������", "ȷ��", "ȡ��")) target.Instantiate();
            }
        }
    }
}
#endif
