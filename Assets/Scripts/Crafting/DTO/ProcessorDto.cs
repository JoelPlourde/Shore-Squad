using System;
using SaveSystem;
using UnityEngine;

namespace CraftingSystem {
	namespace SaveSystem {
		[Serializable]
		public class ProcessorDto {

			[SerializeField]
			public InventoryDto Inputs;

			[SerializeField]
			public InventoryDto Outputs;

			[SerializeField]
			public InventoryDto Fuels;

			[SerializeField]
			public int Power;

			[SerializeField]
			public int Progress;

			[SerializeField]
			public int MaxPower;

			[SerializeField]
			public bool IsProcessing;

			public ProcessorDto(Processor processor) {
				Inputs = new InventoryDto(processor.Inputs);
				Outputs = new InventoryDto(processor.Outputs);
				Fuels = new InventoryDto(processor.Fuels);
				Power = processor.Power;
				Progress = processor.Progress;
				MaxPower = processor.MaxPower;
				IsProcessing = processor.IsProcessing;
			}
		}
	}
}
