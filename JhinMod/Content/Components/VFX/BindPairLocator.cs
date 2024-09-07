using System;
using UnityEngine;
using RoR2;
using System.Collections.Generic;

namespace JhinMod.Content.Components
{
    /// <summary>
    /// Component that adds the arcing motion seen on Dancing Grenade bounces
    /// </summary>
    public class BindPairLocator : MonoBehaviour
    {
        [Serializable]
        public class BindPair
        {
            public string selfLocatorString;
            public string targetLocatorString;
            public Transform selfTransform;
            public Transform targetTransform;
        }

        [SerializeField]
        public GameObject target;

        [SerializeField]
        public List<BindPair> bindPairs = new List<BindPair>();

        public void SetTransforms( BindPair bindPair )
        {
            Transform child1 = null;
            Transform child2 = null;

            var selfComp = this.gameObject.GetComponent<ChildLocator>();
            if ( selfComp )
            {
                child1 = selfComp.FindChild(bindPair.selfLocatorString);
            }

            ModelLocator tarComp = target.GetComponent<ModelLocator>();
            if (tarComp && tarComp.modelTransform)
            {
                var tarComp2 = tarComp.modelTransform.GetComponent<ChildLocator>();
                if (tarComp2)
                {
                    child2 = tarComp2.FindChild(bindPair.targetLocatorString);
                }
            }

            if ( child1 != null ) 
            { 
                bindPair.selfTransform = child1;
            }
            if ( child2 != null )
            {
                bindPair.targetTransform = child2;
            }
        }

        public void SetTransforms()
        {
            // Bind pairs to respective bones or attachments
            foreach (var pair in this.bindPairs)
            {
                SetTransforms(pair);
            }
        }

        public BindPair CreateBindPair(string selfLocatorString, string targetLocatorString)
        {
            return new BindPair { selfLocatorString = selfLocatorString, targetLocatorString = targetLocatorString };
        }

        public void AddBindPair(string selfLocatorString, string targetLocatorString)
        {
            var newBindPair = CreateBindPair(selfLocatorString, targetLocatorString);
            this.bindPairs.Add( newBindPair );
        }

        public void BindPairs()
        {
            this.SetTransforms();
            // Bind pairs to respective bones or attachments
            foreach (var pair in this.bindPairs)
            {
                if ( pair.selfTransform != null && pair.targetTransform != null )
                {
                    pair.selfTransform.SetParent(pair.targetTransform);
                    pair.selfTransform.position = pair.targetTransform.position;
                    pair.selfTransform.rotation = pair.targetTransform.rotation;
                    pair.selfTransform.localScale = pair.targetTransform.localScale;
                }
            }
        }
    }
}

