using System;
using UnityEngine;

namespace ElementalRift {
    [Serializable]
	[CreateAssetMenu(fileName = "EmoteData", menuName = "ScriptableObjects/Elemental Rift/Element Data")]
    public class ElementData : ScriptableObject {

        [Tooltip("The type of the Element")]
		public ElementType ElementType;

        [Tooltip("The layerMask associated to the Element")]
        public LayerMask LayerMask;

		[Tooltip("The primary color associated to the Element")]
        public Color PrimaryColor;

		[Tooltip("The secondary color associated to the Element")]
        public Color SecondaryColor;
    }
}