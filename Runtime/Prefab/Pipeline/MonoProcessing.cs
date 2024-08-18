using FTGAMEStudio.InitialFramework.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    public static class MonoProcessing
    {
        /// <summary>
        /// ����ȡ�� <see cref="MonoInfo"/>��
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
        /// ��ȡָ�� <see cref="MonoBehaviour"/> ��������� <see cref="MonoBehaviour"/> ����� <see cref="MonoInfo"/>��
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
