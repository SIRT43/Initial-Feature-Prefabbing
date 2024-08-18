using FTGAMEStudio.InitialFramework.Classifying;
using System;
using System.Collections.Generic;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    public static class ExpressionProcessing
    {
        /// <summary>
        /// 将 <see cref="ObjectExpression"/> 按 <see cref="ObjectExpression.uniqueName"/> 分类。
        /// </summary>
        public static Dictionary<string, List<T>> ClassifyExpressions<T>(T[] expressions) where T : ObjectExpression
        {
            FilterableClassifier<string, T> classifier = new((T expression, out string key) =>
            {
                key = expression.uniqueName;
                return true;
            });

            return classifier.Classify(expressions);
        }



        /// <summary>
        /// 记入宏，用于批量记入。
        /// </summary>
        public static void CreditedMacro(UnityEngine.Object[] objects, ObjectExpression[] expressions)
        {
            if (objects.Length != expressions.Length)
                throw new ArgumentException($"The parameters are asymmetrical and this cannot be done. objects: {objects.Length}, expressions: {expressions.Length}");

            for (int index = 0; index < objects.Length; index++) expressions[index].Credited(objects[index]);
        }

        /// <summary>
        /// 取出宏，用于批量取出。
        /// </summary>
        public static void FetchedMacro(UnityEngine.Object[] objects, ObjectExpression[] expressions)
        {
            if (objects.Length != expressions.Length)
                throw new ArgumentException($"The parameters are asymmetrical and this cannot be done. objects: {objects.Length}, expressions: {expressions.Length}");

            for (int index = 0; index < objects.Length; index++) expressions[index].Fetched(objects[index]);
        }
    }
}
