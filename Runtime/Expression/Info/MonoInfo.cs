using FTGAMEStudio.InitialFramework.ExtensionMethods;
using System;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    /// <summary>
    /// <see cref="MonoBehaviour"/> 表达基类，用于储存对象信息或覆盖对象信息。
    /// </summary>
    [Serializable]
    public class MonoInfo : ComponentExpression<MonoBehaviour>
    {
        public MonoInfo(MonoBehaviour mono) : base(mono) { }

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
