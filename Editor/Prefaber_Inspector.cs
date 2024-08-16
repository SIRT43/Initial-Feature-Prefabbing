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
                target.data = EditorGUILayout.ObjectField("����", target.data, typeof(Prefab), false) as Prefab;

                EditorGUILayout.Space();

                EditorGUILayout.HelpBox("Before you can do this, you must select a Prefab data.", MessageType.Info);

                return;
            }

            base.OnInspectorGUI();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("LoadPrefab"))
            {
                if (EditorUtility.DisplayDialog("��ȡѡ���ļ��Ҽ������ݣ�", $"{target.FileLocation.FullName}\n\n���޷������˲�����", "ȷ��", "ȡ��")) target.LoadPrefab();
            }

            if (GUILayout.Button("ToPrefab"))
            {
                if (EditorUtility.DisplayDialog("д��ѡ���ļ���", $"{target.FileLocation.FullName}\n\n���޷������ļ�д�������", "ȷ��", "ȡ��")) target.ToPrefab();
            }

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Instantiate"))
                target.SingleInstantiate();
        }
    }
}
#endif