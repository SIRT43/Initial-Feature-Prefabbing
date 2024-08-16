#if UNITY_EDITOR
using FTGAMEStudio.InitialSolution.Persistence;
using UnityEditor;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    [CustomEditor(typeof(Prefab))]
    public class Prefab_Inspector : IPersistableObject_Inspector { }
}
#endif