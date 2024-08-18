using FTGAMEStudio.InitialFramework.ExtensionMethods;
using System;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    /// <summary>
    /// <see cref="MonoBehaviour"/> �����࣬���ڴ��������Ϣ�򸲸Ƕ�����Ϣ��
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
