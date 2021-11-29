using CraftingSystem;
using ItemSystem;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace UnitTest {
	public class ProcessorTest : IPrebuildSetup {

		public void Setup() {
			var gameObject = new GameObject();
			ItemManager itemManager = gameObject.AddComponent<ItemManager>();
		}

		[Test]
		[PrebuildSetup(typeof(ProcessorTest))]
		public void AddFuel_test() {
			Processor processor = new Processor(GetProcessorData());

			Item branch = new Item(GetItemDataFixture(0), 15);
			Item remainder = null;

			Debug.Log("Adding Fuel: branch");
			Assert.True(processor.Fuels.AddItemInInventoryAtPosition(branch, 0, ref remainder));
			Assert.That(processor.Fuels.Items[0].ItemData.ID, Is.EqualTo("branch"));
			Assert.That(processor.Fuels.Items[0].Amount, Is.EqualTo(15));
			Assert.Null(remainder);
		}

		[Test]
		[PrebuildSetup(typeof(ProcessorTest))]
		public void AddFuel_with_remainder_test() {
			Processor processor = new Processor(GetProcessorData());

			Item branch1 = new Item(GetItemDataFixture(0), 15);
			Item branch2 = new Item(GetItemDataFixture(0), 6);
			Item remainder = null;

			Debug.Log("Adding Fuel: branch x15");
			Assert.True(processor.Fuels.AddItemInInventoryAtPosition(branch1, 0, ref remainder));
			Assert.That(processor.Fuels.Items[0].ItemData.ID, Is.EqualTo("branch"));
			Assert.That(processor.Fuels.Items[0].Amount, Is.EqualTo(15));
			Assert.Null(remainder);

			Debug.Log("Adding Fuel: branch x6");
			Assert.True(processor.Fuels.AddItemInInventoryAtPosition(branch2, 0, ref remainder));
			Assert.That(processor.Fuels.Items[0].ItemData.ID, Is.EqualTo("branch"));
			Assert.That(processor.Fuels.Items[0].Amount, Is.EqualTo(20));

			Debug.Log("Checking the remainder, should be branch x1");
			Assert.NotNull(remainder);
			Assert.That(remainder.ItemData.ID, Is.EqualTo("branch"));
			Assert.That(remainder.Amount, Is.EqualTo(1));
		}

		[Test]
		[PrebuildSetup(typeof(ProcessorTest))]
		public void AddFuel_swap_test() {
			Processor processor = new Processor(GetProcessorData());

			Item branch = new Item(GetItemDataFixture(0), 15);
			Item log = new Item(GetItemDataFixture(1), 6);

			Item remainder = null;

			Debug.Log("Adding Fuel: branch");
			Assert.True(processor.Fuels.AddItemInInventoryAtPosition(branch, 0, ref remainder));
			Assert.That(processor.Fuels.Items[0].ItemData.ID, Is.EqualTo("branch"));
			Assert.That(processor.Fuels.Items[0].Amount, Is.EqualTo(15));
			Assert.Null(remainder);

			Debug.Log("Adding Fuel: log");
			Assert.True(processor.Fuels.AddItemInInventoryAtPosition(log, 0, ref remainder));
			Assert.That(processor.Fuels.Items[0].ItemData.ID, Is.EqualTo("log"));
			Assert.That(processor.Fuels.Items[0].Amount, Is.EqualTo(6));
			Assert.NotNull(remainder);

			Debug.Log("Checking the results of the Swap.");
			Assert.That(remainder.ItemData.ID, Is.EqualTo(branch.ItemData.ID));
			Assert.That(remainder.Amount, Is.EqualTo(branch.Amount));
		}

		[Test]
		[PrebuildSetup(typeof(ProcessorTest))]
		public void ConsumeFuel_test() {
			Processor processor = new Processor(GetProcessorData());

			Item branch = new Item(GetItemDataFixture(0), 15);
			Item remainder = null;

			processor.Fuels.AddItemInInventoryAtPosition(branch, 0, ref remainder);

			Assert.True(processor.ConsumeFuel());

			Assert.That(processor.Power, Is.EqualTo(branch.ItemData.Power));
			Assert.That(processor.Fuels.Items[0].Amount, Is.EqualTo(14));
		}

		[Test]
		[PrebuildSetup(typeof(ProcessorTest))]
		public void ConsumeFuel_with_no_fuel_test() {
			Processor processor = new Processor(GetProcessorData());

			Assert.Null(processor.Fuels.Items[0]);
			Assert.False(processor.ConsumeFuel());
		}

		[Test]
		[PrebuildSetup(typeof(ProcessorTest))]
		public void ConsumeFuel_with_empty_fuel_test() {
			Processor processor = new Processor(GetProcessorData());

			Item branch = new Item(GetItemDataFixture(0), 1);
			Item remainder = null;

			processor.Fuels.AddItemInInventoryAtPosition(branch, 0, ref remainder);

			Assert.True(processor.ConsumeFuel());
			Assert.Null(processor.Fuels.Items[0]);
			Assert.That(processor.Power, Is.EqualTo(branch.ItemData.Power));
		}

		[Test]
		[PrebuildSetup(typeof(ProcessorTest))]
		public void IncreaseProgress_requires_power_without_power_test() {
			Processor processor = new Processor(GetPowerableProcessorData());

			Assert.False(processor.ProcessRoutine());
		}

		[Test]
		[PrebuildSetup(typeof(ProcessorTest))]
		public void IncreaseProgress_requires_power_with_power_test() {
			Processor processor = new Processor(GetPowerableProcessorData());

			Item branch = new Item(GetItemDataFixture(0), 1);
			Item logs = new Item(GetItemDataFixture(1), 2);
			Item remainder = null;

			// Fill-in the fuel.
			Assert.True(processor.Fuels.AddItemInInventoryAtPosition(branch, 0, ref remainder));

			// Add the items for the recipe.
			Assert.True(processor.Inputs.AddItemInInventoryAtPosition(logs, 0, ref remainder));
			Assert.True(processor.StartProcess());
			Assert.True(processor.ProcessRoutine());

			Assert.That(processor.Power, Is.GreaterThan(1));

			// Verify that the fuel is consumed.
			Assert.Null(processor.Fuels.Items[0]);
		}

		[Test]
		[PrebuildSetup(typeof(ProcessorTest))]
		public void IncreaseProgress_without_starting_process_should_throw_test() {
			Processor processor = new Processor(GetPowerableProcessorData());

			Item branch = new Item(GetItemDataFixture(0), 1);
			Item remainder = null;

			Assert.True(processor.Fuels.AddItemInInventoryAtPosition(branch, 0, ref remainder));
			Assert.Throws<UnityException>(() => processor.ProcessRoutine());
		}

		[Test]
		[PrebuildSetup(typeof(ProcessorTest))]
		public void IncreaseProgress_not_requires_power_without_power_test() {
			Processor processor = new Processor(GetUnpowerableProcessorData());

			Item logs = new Item(GetItemDataFixture(1), 2);
			Item remainder = null;

			// Fill-in the fuel.
			Assert.True(processor.Inputs.AddItemInInventoryAtPosition(logs, 0, ref remainder));

			Assert.True(processor.StartProcess());
			Assert.True(processor.ProcessRoutine());
		}

		[Test]
		[PrebuildSetup(typeof(ProcessorTest))]
		public void StartProcess_test() {
			Processor processor = new Processor(GetUnpowerableProcessorData());

			Item log = new Item(GetItemDataFixture(1), 6);
			Item remainder = null;

			Assert.True(processor.Inputs.AddItemInInventoryAtPosition(log, 0, ref remainder));
			Assert.Null(remainder);

			Assert.True(processor.StartProcess());
		}

		[Test]
		[PrebuildSetup(typeof(ProcessorTest))]
		public void StartProcess_with_not_enough_inputs_test() {
			Processor processor = new Processor(GetUnpowerableProcessorData());

			Debug.Log(processor.ProcessorData.Recipes[0].Inputs[0].ItemData.ID);
			Debug.Log(processor.ProcessorData.Recipes[0].Inputs[0].Amount);

			Item log = new Item(GetItemDataFixture(1), 1);
			Item remainder = null;

			Assert.True(processor.Inputs.AddItemInInventoryAtPosition(log, 0, ref remainder));
			Assert.Null(remainder);

			Assert.False(processor.StartProcess());
		}

		[Test]
		[PrebuildSetup(typeof(ProcessorTest))]
		public void StartProgress_with_wrong_items_test() {
			Processor processor = new Processor(GetUnpowerableProcessorData());

			Item boots = new Item(GetItemDataFixture(2), 2);
			Item remainder = null;

			Assert.True(processor.Inputs.AddItemInInventoryAtPosition(boots, 0, ref remainder));
			Assert.Null(remainder);

			Assert.False(processor.StartProcess());
		}

		[Test]
		[PrebuildSetup(typeof(ProcessorTest))]
		public void StartProcess_with_enough_inputs_in_different_slot_test() {

			ProcessorData processorData = GetUnpowerableProcessorData();
			processorData.InputCount = 2;

			Processor processor = new Processor(processorData);

			Item log1 = new Item(GetItemDataFixture(1), 1);
			Item log2 = new Item(GetItemDataFixture(1), 1);
			Item remainder = null;

			Assert.True(processor.Inputs.AddItemInInventoryAtPosition(log1, 0, ref remainder));
			Assert.True(processor.Inputs.AddItemInInventoryAtPosition(log2, 1, ref remainder));
			Assert.Null(remainder);

			Assert.True(processor.StartProcess());
		}

		[Test]
		[PrebuildSetup(typeof(ProcessorTest))]
		public void Process_test() {
			Processor processor = GetPoweredProcessorFixture(GetPowerableProcessorData());

			// Start the Process.
			Assert.True(processor.StartProcess());

			// Call IncreaseProgress 5 times
			for (int i = 1; i < 6; i++) {
				Assert.True(processor.ProcessRoutine());
				Assert.That(processor.Progress, Is.EqualTo(i));
			}

			Assert.False(processor.ProcessRoutine());

			// TODO Verify the output.
			Assert.True(processor.StopProgress());

			Assert.That(processor.Outputs.Items[0].ItemData.ID, Is.EqualTo("branch"));
			Assert.That(processor.Outputs.Items[0].Amount, Is.EqualTo(3));
		}

		[Test]
		[PrebuildSetup(typeof(ProcessorTest))]
		public void Process_running_out_of_power_test() {
			ProcessorData processorData = GetPowerableProcessorData();
			processorData.Recipes[0].RequiredProgress = 15;

			Processor processor = GetPoweredProcessorFixture(processorData);

			// Start the Process.
			Assert.True(processor.StartProcess());

			// Call IncreaseProgress 10 times so that the Power runs out.
			for (int i = 0; i < 10; i++) {
				Assert.True(processor.ProcessRoutine());
				Assert.That(processor.Progress, Is.EqualTo(i + 1));
			}

			Assert.False(processor.ProcessRoutine());
			Assert.That(processor.Power, Is.EqualTo(0));
			Assert.That(processor.Progress, Is.EqualTo(10));
		}

		private Processor GetPoweredProcessorFixture(ProcessorData processorData) {
			Processor processor = new Processor(processorData);

			Item branch = new Item(GetItemDataFixture(0), 1);
			Item remainder = null;

			// Add Fuels to the Processor.
			Assert.True(processor.Fuels.AddItemInInventoryAtPosition(branch, 0, ref remainder));
			Assert.Null(remainder);

			// Add enough Inputs to the Processor
			Item logs = new Item(GetItemDataFixture(1), 3);
			Assert.True(processor.Inputs.AddItemInInventoryAtPosition(logs, 0, ref remainder));
			Assert.Null(remainder);

			return processor;
		}

		private ProcessorData GetProcessorData() {
			return new ProcessorData {
				RequiresPower = false,
				InputCount = 2,
				OutputCount = 1,
				FuelCount = 1
			};
		}

		private ProcessorData GetUnpowerableProcessorData() {
			return new ProcessorData {
				RequiresPower = false,
				InputCount = 1,
				OutputCount = 1,
				FuelCount = 1,
				Recipes = new List<RecipeData> {
					GetSimpleRecipeDataFixture()
				}
			};
		}

		private ProcessorData GetPowerableProcessorData() {
			return new ProcessorData {
				RequiresPower = true,
				InputCount = 1,
				OutputCount = 1,
				FuelCount = 1,
				Recipes = new List<RecipeData> {
					GetSimpleRecipeDataFixture()
				}
			};
		}

		private RecipeData GetSimpleRecipeDataFixture() {
			return new RecipeData {
				Inputs = new List<Item> {
					new Item (GetItemDataFixture(1), 2)
				},
				Outputs = new List<Item> {
					new Item (GetItemDataFixture(0), 3)
				},
				RequiredProgress = 5
			};
		}

		private ItemData GetItemDataFixture(int index) {
			switch (index) {
				case 0:
					return ItemManager.Instance.GetItemData("branch");
				case 1:
					return ItemManager.Instance.GetItemData("log");
				case 2:
					return ItemManager.Instance.GetItemData("leather_boots");
				default:
					throw new System.Exception("Unknown index: " + index);
			}
		}
	}
}
