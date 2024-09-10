using System;
using UnityEngine;
using RoR2;
using System.Collections.Generic;

namespace JhinMod.Content.Components
{
    /// <summary>
    /// Component that allows binding multiple transforms, instead of just one
    /// </summary>
    [RequireComponent(typeof(ChildLocator))]
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

        public GameObject target;

        [SerializeField]
        public List<BindPair> bindPairs = new List<BindPair>();

        private bool enableDebug = false;

        public void SetTransforms( BindPair bindPair )
        {
            Transform child1 = null;
            Transform child2 = null;

            var selfComp = this.gameObject.GetComponent<ChildLocator>();
            if ( selfComp )
            {
                child1 = selfComp.FindChild(bindPair.selfLocatorString);
            }

            ChildLocator tarComp = target.GetComponentInChildren<ChildLocator>();
            if ( tarComp )
            {
                    child2 = tarComp.FindChild(bindPair.targetLocatorString);
            }

            if ( child1 != null ) 
            {
                bindPair.selfTransform = child1;
                DebugPrint($"child1 found {child1}");
            }
            if ( child2 != null )
            {
                bindPair.targetTransform = child2;
                DebugPrint($"child2 found {child2}");
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

        public void AddBindPair(string locatorString)
        {
            var newBindPair = CreateBindPair(locatorString, locatorString);
            this.bindPairs.Add(newBindPair);
        }

        public void AddBindPairs( string[] locatorStrings)
        {
            foreach (var locatorString in locatorStrings)
            {
                var newBindPair = CreateBindPair(locatorString, locatorString);
                this.bindPairs.Add(newBindPair);
            }
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

        public void DebugPrint(string text)
        {
            if (enableDebug)
            {
                Log.Warning(text);
            }
        }
    }
}

