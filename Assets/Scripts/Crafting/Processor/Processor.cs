using CraftingSystem.SaveSystem;
using ItemSystem;
using SaveSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CraftingSystem {
	public class Processor {

		public event Action<float> OnPowerEvent;				// Event with the percentage (%) of power
		public event Action<float> OnProgressEvent;             // Event with the percentage (%) of progress
		public event Action OnInputBeginDragEvent;              // Event triggered whenever the Item(s) in Input is begin drag.
		public event Action OnInputEvent;                       // Event triggered whenever there is an item changes
		public event Action OnFuelEvent;                        // Event triggered whenever there is an fuel changes
		public event Action OnOutputEvent;						// Event triggered whenever the output is removed.

		private HashSet<RecipeData> _possibleRecipes;				// List of Potential recipes based on the current inputs.

		public Processor(ProcessorData processorData) {
			ProcessorData = processorData;

			Inputs = new Inventory(ProcessorData.InputCount);
			Inputs.OnDirtyItemsEvent += OnInputEventListener;

			Fuels = new Inventory(ProcessorData.FuelCount);
			Fuels.OnDirtyItemsEvent += OnFuelEventListener;

			Outputs = new Inventory(ProcessorData.OutputCount);

			_possibleRecipes = new HashSet<RecipeData>();
		}

		public void LoadProcessor(ProcessorDto processorDto) {
			Inputs.Initialize(processorDto.Inputs);
			Outputs.Initialize(processorDto.Outputs);
			Fuels.Initialize(processorDto.Fuels);
			Power = processorDto.Power;
			Progress = processorDto.Progress;
			MaxPower = processorDto.MaxPower;
			IsProcessing = processorDto.IsProcessing;
		}

		/// <summary>
		/// Method that needs to be called whenever the Processor is not needed anymore.
		/// </summary>
		public void Destroy() {
			Inputs.OnDirtyItemsEvent -= OnInputEventListener;
			Fuels.OnDirtyItemsEvent -= OnFuelEventListener;
		}

		#region Drag
		public void OnInputBeginDrag() {
			OnInputBeginDragEvent?.Invoke();
		}

		public void OnInputEndDrag() {
			OnInputEvent?.Invoke();
		}

		public void OnOutputEndDrag() {
			OnOutputEvent?.Invoke();
		}

		public void OnInputEventListener(List<int> indexes, Item[] items) {
			OnInputEvent?.Invoke();
		}

		public void OnFuelEventListener(List<int> indexes, Item[] items) {
			OnFuelEvent?.Invoke();
		}
		#endregion

		#region Process
		/// <summary>
		/// Start the Process by scanning the possible recipes until a single recipe is left.
		/// </summary>
		/// <returns>Whether or not the process can be started or not.</returns>
		public bool StartProcess() {
			// Reset the possible recipes.
			_possibleRecipes.Clear();

			// Check all the recipes which are possible.
			foreach (RecipeData recipeData in ProcessorData.Recipes) {
				if (recipeData.CheckIfRecipeIsPossible(Inputs.Items)) {
					_possibleRecipes.Add(recipeData);
				}
			}

			// Check if there is only 1 recipe left AND that is it valid.
			if (_possibleRecipes.Count == 1 && _possibleRecipes.First().CheckIfRecipeIsValid(Inputs.Items)) {

				RecipeData recipe = _possibleRecipes.First();

				if (ProcessorData.RequiresPower && Power == 0) {
					if (!HasFuel()) {
						IsProcessing = false;
						return false;
					}
				}

				Dictionary<string, int> currentOutputs = InventoryUtils.GroupItemsByID(Outputs.Items);

				// Verify if you will be able to add the outputs once the operation is completed.
				foreach (Item ingredient in recipe.Outputs) {
					if (currentOutputs.ContainsKey(ingredient.ItemData.ID)) {
						currentOutputs[ingredient.ItemData.ID] += ingredient.Amount;
						if (currentOutputs[ingredient.ItemData.ID] > Inventory.MAX_STACK) {
							IsProcessing = false;
							return IsProcessing;
						}
					} else {
						currentOutputs.Add(ingredient.ItemData.ID, ingredient.Amount);
						if (currentOutputs.Count > ProcessorData.OutputCount) {
							IsProcessing = false;
							return IsProcessing;
						}
					}
				}

				Progress = 0;

				IsProcessing = true;
				return IsProcessing;
			}

			IsProcessing = false;
			return IsProcessing;
		}

		/// <summary>
		/// Routine Process, stop whenever the Progress is achieved or run out of fuel.
		/// </summary>
		/// <returns>Return True if you can continue, False if not.</returns>
		public bool ProcessRoutine() {
			return IncreaseProgress() && Progress <= _possibleRecipes.First().RequiredProgress;
		}

		public bool StopProgress() {
			RecipeData recipeData = _possibleRecipes.First();
			if (Progress < _possibleRecipes.First().RequiredProgress) {
				IsProcessing = false;
				return false;
			}

			List<Item> remainingItems = new List<Item>();

			Inputs.RemoveItemsFromInventory(recipeData.Inputs, out remainingItems);
			if (remainingItems.Count > 0) {
				throw new UnityException("THIS SHOULD NEVER HAPPEN.");
			}

			Outputs.AddItemsToInventory(recipeData.Outputs, out remainingItems);
			if (remainingItems.Count > 0) {
				throw new UnityException("THIS SHOULD NEVER HAPPEN.");
			}

			// Reset progress
			ResetProgress();

			return true;
		}

		/// <summary>
		/// Increase the progress by consuming power if needed.
		/// </summary>
		/// <returns></returns>
		private bool IncreaseProgress() {
			if (ProcessorData.RequiresPower) {
				if (Power > 0) {
					UpdateProgress(1);

					UpdatePower(-1);
					return true;
				} else if (ConsumeFuel()) {
					return IncreaseProgress();
				} else {
					return false;
				}
			} else {
				UpdateProgress(1);
				return true;
			}
		}

		/// <summary>
		/// Reset the progress.
		/// </summary>
		public void ResetProgress() {
			Progress = 0;
			IsProcessing = false;

			OnProgressEvent?.Invoke(Progress);
		}

		/// <summary>
		/// Update the Progress and send its newest Percentage to the UI.
		/// </summary>
		private void UpdateProgress(int value) {
			if (!IsProcessing) {
				throw new UnityException("You cannot update progress without StartProcess() == true first");
			}

			Progress += value;
			float percentage = (float) Progress / (float) _possibleRecipes.First().RequiredProgress;
			OnProgressEvent?.Invoke(percentage);
		}
		#endregion

		#region Fuel
		/// <summary>
		/// Determine whether or not it has fuel.
		/// </summary>
		/// <returns>Whether or not it has fuel</returns>
		public bool HasFuel() {
			for (int i = 0; i < Fuels.Items.Length; i++) {
				if (!ReferenceEquals(Fuels.Items[i], null)) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Consume Fuel and increase the Power of the processor.
		/// </summary>
		/// <returns></returns>
		public bool ConsumeFuel() {
			for (int i = 0; i < Fuels.Items.Length; i++) {
				if (!ReferenceEquals(Fuels.Items[i], null)) {
					MaxPower = Mathf.Max(0, Fuels.Items[i].ItemData.Power);
					if (Fuels.RemoveItemFromInventoryAtPosition(i, 1)) {
						UpdatePower(MaxPower);
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Update the Power and send its newest Percentage to the UI.
		/// </summary>
		/// <param name="value">The value to update the power by.</param>
		private void UpdatePower(int value) {
			Power += value;
			OnPowerEvent?.Invoke(CurrentPower);
		}
		#endregion

		public bool IsProcessing { get; private set; }

		public Inventory Inputs { get; private set; }
		public Inventory Outputs { get; private set; }
		public Inventory Fuels { get; private set; }

		public int Power { get; private set; } = 0;
		public int Progress { get; private set; } = 0;
		public int MaxPower { get; private set; } = 0;

		public float CurrentPower { get {return (MaxPower == 0) ? 0 : Power / (float)MaxPower;}}
		public float CurrentProgress { get { return (_possibleRecipes.Count == 1) ? Progress / (float)_possibleRecipes.First().RequiredProgress : 0; } }

		public ProcessorData ProcessorData { get; private set; }
	}
}
