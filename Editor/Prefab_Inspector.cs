#if UNITY_EDITOR
using UnityEditor;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    [CustomEditor(typeof(Prefab))]
    public class Prefab_Inspector : IPrefabbing_Inspector { }
}
#endif
