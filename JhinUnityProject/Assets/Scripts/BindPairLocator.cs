using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

    /// <summary>
    /// Component that adds the arcing motion seen on Dancing Grenade bounces
    /// </summary>
    public class BindPairLocator : MonoBehaviour
    {
        [Serializable]
        public struct BindPair
        {
            public string name;
            public Transform transform;
            public string bindTarget;
        }

        [SerializeField]
        public BindPair[] bindPairs;
    }
