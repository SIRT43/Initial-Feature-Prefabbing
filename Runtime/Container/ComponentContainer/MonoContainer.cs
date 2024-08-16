using FTGAMEStudio.InitialFramework.ExtensionMethods;
using System;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    /// <summary>
    /// ∏®÷˙¿‡£¨”√”⁄¥¢¥Ê <see cref="MonoBehaviour"/>°£
    /// </summary>
    [Serializable]
    public class MonoContainer : ComponentContainer<MonoBehaviour>
    {
        public MonoContainer(MonoBehaviour mono) : base(mono) { }

        public override void Credited(MonoBehaviour target)
        {
            name = target.name;

            uniqueName = target.GetType().GetUniqueName();
            data = IFJson.ToJson(target);
        }

        public override void Fetched(MonoBehaviour target)
        {
            target.name = name;

            IFJson.FromJson(data, target);
        }
    }
}
