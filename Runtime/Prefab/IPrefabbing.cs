using FTGAMEStudio.InitialFramework.Collections.Generic;
using System;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    public interface IPrefabbing
    {
        public bool DeepOperation { get; set; }

        /// <summary>
        /// 序列化。
        /// </summary>
        public void Enprefab(InquiryMachine<string, Type> containerTypes);
        public void Enprefab();

        /// <summary>
        /// 反序列化。
        /// </summary>
        public void Deprefab(InquiryMachine<string, Type> containerTypes);
        public void Deprefab();
    }
}
