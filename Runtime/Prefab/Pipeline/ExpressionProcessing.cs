using FTGAMEStudio.InitialFramework.Classifying;
using System;
using System.Collections.Generic;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    public static class ExpressionProcessing
    {
        /// <summary>
        /// �� <see cref="ObjectExpression"/> �� <see cref="ObjectExpression.uniqueName"/> ���ࡣ
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
        /// ����꣬�����������롣
        /// </summary>
        public static void CreditedMacro(UnityEngine.Object[] objects, ObjectExpression[] expressions)
        {
            if (objects.Length != expressions.Length)
                throw new ArgumentException($"The parameters are asymmetrical and this cannot be done. objects: {objects.Length}, expressions: {expressions.Length}");

            for (int index = 0; index < objects.Length; index++) expressions[index].Credited(objects[index]);
        }

        /// <summary>
        /// ȡ���꣬��������ȡ����
        /// </summary>
        public static void FetchedMacro(UnityEngine.Object[] objects, ObjectExpression[] expressions)
        {
            if (objects.Length != expressions.Length)
                throw new ArgumentException($"The parameters are asymmetrical and this cannot be done. objects: {objects.Length}, expressions: {expressions.Length}");

            for (int index = 0; index < objects.Length; index++) expressions[index].Fetched(objects[index]);
        }
    }
}
