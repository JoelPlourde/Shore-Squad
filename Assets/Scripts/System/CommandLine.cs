using ItemSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(InputField))]
public class CommandLine : MonoBehaviour, IUpdatable {

	private readonly char DELIMITER = ' ';

	private Canvas _canvas;
	private InputField _inputField;

	private void Start() {
		if (Debug.isDebugBuild) {
			_canvas = GetComponent<Canvas>();
			_inputField = GetComponent<InputField>();

			_inputField.onEndEdit.AddListener(value => {
				if (Input.GetKeyDown(KeyCode.Return)) {
					ParseCommand();
				}
			});

			GameController.Instance.RegisterUpdatable(this);
		}
	}

	public void OnUpdate() {
		if (Input.GetKeyUp(KeyCode.BackQuote)) {
			_canvas.enabled = !_canvas.isActiveAndEnabled;
			if (_canvas.enabled) {
				EventSystem.current.SetSelectedGameObject(gameObject);
				GameController.Instance.DeregisterLateUpdatable(CameraSystem.CameraController.Instance);
				UserInputs.Instance.DisableInput();
			} else {
				EventSystem.current.SetSelectedGameObject(null);
				GameController.Instance.RegisterLateUpdatable(CameraSystem.CameraController.Instance);
				UserInputs.Instance.EnableInput();
			}
		}
	}

	/// <summary>
	/// Parse the input command.
	/// </summary>
	private void ParseCommand() {
		string[] words = _inputField.text.Split(DELIMITER);
		switch (ValidateArgument(words[0])) {
			case "spawn":
				SpawnItems(words);
				break;
			case "h":
			case "help":
				Help();
				break;
			default:
				throw new UnityException("Unknown command: " + words[0]);
		}

		ClearCommand();
	}

	/// <summary>
	/// Validate the argument that is a string that is not null, empty or whitespace.
	/// </summary>
	/// <param name="argument">The argument to verify.</param>
	/// <returns>The verified argument.</returns>
	private string ValidateArgument(string argument) {
		if (string.IsNullOrEmpty(argument) || string.IsNullOrWhiteSpace(argument)) {
			throw new UnityException("Please verify the following argument: " + argument);
		}

		return argument.ToLower();
	}

	/// <summary>
	/// Validate the argument as a number.
	/// </summary>
	/// <param name="argument">The argument which should represent a number.</param>
	/// <returns>The parsed number.</returns>
	private int ValidateNumber(string argument) {
		return int.Parse(ValidateArgument(argument));
	}

	/// <summary>
	/// Spawn Items in the first selected Actor or any actor if none selected.
	/// </summary>
	/// <param name="arguments">The arguments to spawn the items:spawn <item> <amount></param>
	private void SpawnItems(string[] arguments) {
		if (Squad.FirstSelected(out Actor actor)) {
		} else if (Squad.Any(out actor)) {
		} else {
			throw new UnityException("There is no character defined in the squad, please verify.");
		}

		actor.Inventory.AddItemsToInventory(new List<Item>() { new Item(ItemManager.Instance.GetItemData(ValidateArgument(arguments[1])), ValidateNumber(arguments[2])) }, out List<Item> _);
	}

	/// <summary>
	/// Outpus the Help for this command line in the Console.
	/// </summary>
	private void Help() {
		Debug.Log("Supported command: \n" +
			"- spawn <item name> <amount>\n" + 
			"- help");
	}

	/// <summary>
	/// Reset the Input field
	/// </summary>
	private void ClearCommand() {
		_inputField.text = "";
	}
}