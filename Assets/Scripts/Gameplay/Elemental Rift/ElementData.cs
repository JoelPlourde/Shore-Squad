using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalRift {
    [Serializable]
	[CreateAssetMenu(fileName = "EmoteData", menuName = "ScriptableObjects/Element Data")]
    public class ElementData : ScriptableObject {

		[SerializeField]
		[Tooltip("The type of the Element")]
		public ElementType ElementType;

        [SerializeField]
        [Tooltip("The layerMask associated to the Element")]
        public LayerMask LayerMask;

		[SerializeField]
		[Tooltip("The primary color associated to the Element")]
        public Color PrimaryColor;

		[SerializeField]
		[Tooltip("The secondary color associated to the Element")]
        public Color SecondaryColor;
    
    }
}