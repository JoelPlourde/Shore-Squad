using System;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem {
	[RequireComponent(typeof(Canvas))]
	public class DialogueHandler : MonoBehaviour {

		public static DialogueHandler Instance;

		private Text _text;
		private Canvas _canvas;
		private Node _currentNode;
		private OptionHandler _optionHandler;
		private ButtonComponent _next;
		private ButtonComponent[] _options;
		private ButtonComponent _exit;

		// Display text in a Zelda-like fashion.
		private string _displayText = "";
		private int _textIndex = 0;
		private bool _isDisplayed = false;

		public event Action OnDialogueEnd;

		private void Awake() {
			Instance = this;

			_next = GetComponentWithNullCheck<ButtonComponent>(transform.Find("Next"));
			_exit = GetComponentWithNullCheck<ButtonComponent>(transform.Find("Exit"));
			_text = GetComponentWithNullCheck<Text>(transform.Find("Text"));
			_canvas = GetComponentWithNullCheck<Canvas>(transform);

			_optionHandler = transform.Find("Options").GetComponent<OptionHandler>();
			_options = _optionHandler.GetComponentsInChildren<ButtonComponent>();
			if (ReferenceEquals(_options, null) || _options.Length == 0) {
				throw new UnityException("Please verify the hierarchy of DialogueHandler: Options is missing.");
			}
		}

		public void Initialize(DialogueData dialogueData) {
			// Reset interal variable
			_displayText = "";
			_textIndex = 0;
			_isDisplayed = false;

			DialogueData = dialogueData;

			_currentNode = DialogueData.Entry;

			InitializeListeners();

			EnableNextButton(false);

			EnableOptions(false);

			UpdateComponents();

			_canvas.enabled = true;
		}

		private void OnNext() {
			if (_currentNode.NodeType == NodeType.ACTION) {
				ExecuteAction();
			}

			if (_currentNode.NodeType == NodeType.QUIT || _currentNode.Nodes.Length == 0) {
				OnExit();
				return;
			}

			_currentNode = _currentNode.Nodes[0];

			_isDisplayed = false;

			UpdateComponents();
		}

		private void OnOption(int index) {
			if (_currentNode.NodeType == NodeType.ACTION) {
				ExecuteAction();
			}

			_currentNode = _currentNode.Nodes[index];

			EnableNextButton(false);

			EnableOptions(false);

			OnNext();
		}

		/// <summary>
		/// Execute the ACTIVATE action on the QuestManager here.
		/// </summary>
		private void ExecuteAction() {
			QuestSystem.QuestManager.Instance.TriggerEvent(QuestSystem.ObjectiveType.TALK, DialogueData.Identifier);
		}

		/// <summary>
		/// Update the display of every components.
		/// </summary>
		private void UpdateComponents() {
			// Update the text.
			DisplayText(_currentNode.Content);

			EnableNextButton(false);

			if (_isDisplayed) {
				// Determine if the current node has some options.
				bool hasOptions = (_currentNode.Nodes.Length > 1);

				// If so, update the options text.
				if (hasOptions) {
					for (int i = 0; i < _currentNode.Nodes.Length; i++) {
						_options[i].UpdateText(_currentNode.Nodes[i].Content);
					}
				}

				EnableOptions(hasOptions);

				EnableNextButton(!hasOptions);
			}
		}

		/// <summary>
		/// Enable/Disable the Next Button
		/// </summary>
		/// <param name="enabled">True to enable, false to disable.</param>
		private void EnableNextButton(bool enabled) {
			_next.gameObject.SetActive(enabled);
		}

		/// <summary>
		/// Enable/Disable the Options
		/// </summary>
		/// <param name="enabled">True to enable, false to disable.</param>
		private void EnableOptions(bool enabled) {
			_optionHandler.gameObject.SetActive(enabled);
		}

		/// <summary>
		/// Start the routine to display the text one character at a time.
		/// </summary>
		/// <param name="displayText">The complete text to display</param>
		private void DisplayText(string displayText) {
			if (_isDisplayed == false) {
				_text.text = "";

				_displayText = displayText;

				// TODO put this value in the settings.
				InvokeRepeating(nameof(Routine), 0, 0.03f);
			}
		}

		/// <summary>
		/// Display a single character at a time.
		/// </summary>
		private void Routine() {
			_text.text += _displayText[_textIndex];

			_textIndex++;

			if (_textIndex >= _displayText.Length) {
				_textIndex = 0;

				_isDisplayed = true;

				CancelInvoke();

				UpdateComponents();
				return;
			}
		}

		/// <summary>
		/// Exit the dialogue.
		/// </summary>
		private void OnExit() {
			_canvas.enabled = false;

			EnableNextButton(false);

			EnableOptions(false);

			ResetListeners();

			OnDialogueEnd?.Invoke();

			CancelInvoke();
		}

		#region Listeners
		/// <summary>
		/// Initialize non-persistent listeners with their appropriate callback.
		/// </summary>
		private void InitializeListeners() {
			_next.SubscribeListener(OnNext);
			_exit.SubscribeListener(OnExit);
			_options[0].SubscribeListener(delegate { OnOption(0); });
			_options[1].SubscribeListener(delegate { OnOption(1); });
		}

		/// <summary>
		/// Reset non-persistent listeners.
		/// </summary>
		private void ResetListeners() {
			_next.UnsubscribeListeners();
			_exit.UnsubscribeListeners();
			for (int i = 0; i < _options.Length; i++) {
				_options[i].UnsubscribeListeners();
			}
		}
		#endregion

		private T GetComponentWithNullCheck<T>(Transform transform) {
			T component = transform.GetComponent<T>();
			if (ReferenceEquals(component, null)) {
				throw new UnityException("Please verify the hierarchy of DialogueHandler.");
			}
			return component;
		}

		public DialogueData DialogueData { get; private set; }
	}
}
