using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem {
    [Serializable]
    public class SquadDto {

        [SerializeField]
        public List<string> ActorGuids = new List<string>();
    }
}