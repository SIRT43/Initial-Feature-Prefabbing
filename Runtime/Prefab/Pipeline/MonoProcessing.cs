using FTGAMEStudio.InitialFramework.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    public static class MonoProcessing
    {
        /// <summary>
        /// 批量取出 <see cref="MonoInfo"/>。
        /// </summary>
        public static void FetchedWithMonos(MonoBehaviour[] monos, MonoInfo[] monoInfos)
        {
            Dictionary<string, List<MonoBehaviour>> classifyMonos = ReflectionMacro.ClassifyWithUniqueName(monos);
            Dictionary<string, List<MonoInfo>> classifyMonoInfos = ExpressionProcessing.ClassifyExpressions(monoInfos);

            foreach (KeyValuePair<string, List<MonoBehaviour>> classifyMono in classifyMonos)
            {
                if (!classifyMonoInfos.ContainsKey(classifyMono.Key)) continue;

                ExpressionProcessing.FetchedMacro(classifyMono.Value.ToArray(), classifyMonoInfos[classifyMono.Key].ToArray());
            }
        }

        /// <summary>
        /// 获取指定 <see cref="MonoBehaviour"/> 数组的所有 <see cref="MonoBehaviour"/> 组件的 <see cref="MonoInfo"/>。
        /// </summary>
        public static MonoInfo[] GetInfos(MonoBehaviour[] monos)
        {
            MonoInfo[] monoInfos = new MonoInfo[monos.Length];

            for (int index = 0; index < monos.Length; index++)
                monoInfos[index] = new(monos[index]);

            return monoInfos;
        }
    }
}
