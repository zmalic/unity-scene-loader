using System;
namespace SLoader
{
    [Serializable]
    public class TipList
    {
        public Tip[] tips;


        /// <summary>
        /// Random Tip item
        /// </summary>
        /// <returns>Random Tip</returns>
        public Tip GetRandom()
        {
            if (IsEmpty())
            {
                throw new System.Exception("The tip list is empty");
            }

            return tips[UnityEngine.Random.Range(0, tips.Length)];
        }

        /// <summary>
        /// Check if list is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return tips == null || tips.Length == 0;
        }
    }

    /// <summary>
    /// Simple tip
    /// </summary>
    [Serializable]
    public class Tip
    {
        public string title;
        public string description;
    }
}
