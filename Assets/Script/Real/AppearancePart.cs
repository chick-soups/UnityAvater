using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Real
{
    public struct AppearancePart
    {
        public string Name {
            get;
            private set;
        }
        public List<Material> Materials {
            private set;
            get;
        }
        public List<Transform> Bones {
            get;
            private set;
        }
        public List<CombineInstance> CombineInstances
        {
            get;
            private set;
        }

        public AppearancePart(string partName)
        {
            Name = partName;
            Materials = new List<Material>();
            Bones = new List<Transform>();
            CombineInstances = new List<CombineInstance>();

        }
    }
}

