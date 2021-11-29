using UnityEngine;
using UI;
using System.Collections.Generic;
using static UnityEngine.UI.Image;

namespace CraftingSystem {
	namespace UI {
		[RequireComponent(typeof(Canvas))]
		public class ProcessorHandler : MonoBehaviour, IUpdatable {
			public static ProcessorHandler Instance;

			[Tooltip("The Padding")]
			public float Padding = 25f;

			[Tooltip("The size of the cells")]
			public float CellSize = 50f;

			private Canvas _canvas;
			private RectTransform _rectTransform;
			private RectTransform _leftSection;
			private RectTransform _middleSection;
			private RectTransform _rightSection;

			private PowerBar _powerBar;
			private PowerBar _progressBar;

			private Dictionary<HandlerType, BaseHandler> _handlers;

			private void Start() {
				Instance = this;

				_canvas = GetComponent<Canvas>();
				_rectTransform = GetComponent<RectTransform>();
				_leftSection = transform.Find("Left").GetComponent<RectTransform>();
				_middleSection = transform.Find("Middle").GetComponent<RectTransform>();
				_rightSection = transform.Find("Right").GetComponent<RectTransform>();

				_powerBar = _leftSection.Find("Power").GetComponentInChildren<PowerBar>();
				_powerBar.Initialize(FillMethod.Vertical);

				_progressBar = _middleSection.GetComponentInChildren<PowerBar>();
				_progressBar.Initialize(FillMethod.Horizontal);

				_handlers = new Dictionary<HandlerType, BaseHandler> {
					[HandlerType.Input]  = _leftSection.Find("Input").GetComponent<BaseHandler>(),
					[HandlerType.Fuel]	 = _leftSection.Find("Fuel").GetComponent<FuelHandler>(),
					[HandlerType.Output] = _rightSection.Find("Output").GetComponent<BaseHandler>()
				};
			}

			public void DisplayProcessor(ProcessorBehaviour processorBehaviour) {
				// If its not the same Processor
				if (!ReferenceEquals(ProcessorBehaviour, null) && ProcessorBehaviour != processorBehaviour) {
					return;
				}

				// Store local variable.
				ProcessorBehaviour = processorBehaviour;

				_handlers[HandlerType.Input].Initialize(ProcessorBehaviour.Processor.Inputs);
				_handlers[HandlerType.Output].Initialize(ProcessorBehaviour.Processor.Outputs);

				if (ProcessorBehaviour.Processor.ProcessorData.RequiresPower) {
					_powerBar.transform.parent.gameObject.SetActive(true);
					_handlers[HandlerType.Fuel].gameObject.SetActive(true);
					_handlers[HandlerType.Fuel].Initialize(ProcessorBehaviour.Processor.Fuels);
				} else {
					_powerBar.transform.parent.gameObject.SetActive(false);
					_handlers[HandlerType.Fuel].gameObject.SetActive(false);
				}

				// Force the display of the Progress
				_progressBar.ForceUpdateValue(ProcessorBehaviour.Processor.CurrentProgress);
				_powerBar.ForceUpdateValue(ProcessorBehaviour.Processor.CurrentPower);

				// Subscribe to all the events for this processor.
				ProcessorBehaviour.Processor.OnPowerEvent += _powerBar.UpdateValue;
				ProcessorBehaviour.Processor.OnProgressEvent += _progressBar.UpdateValue;

				_handlers[HandlerType.Input].OnItemBeginDragEvent += ProcessorBehaviour.Processor.OnInputBeginDrag;
				_handlers[HandlerType.Input].OnItemEndDragEvent += ProcessorBehaviour.Processor.OnInputEndDrag;
				_handlers[HandlerType.Output].OnItemEndDragEvent += ProcessorBehaviour.Processor.OnOutputEndDrag;

				ProcessorBehaviour.Processor.Inputs.OnDirtyItemsEvent	+= _handlers[HandlerType.Input].OnDirtyItemsEvent;
				ProcessorBehaviour.Processor.Fuels.OnDirtyItemsEvent	+= _handlers[HandlerType.Fuel].OnDirtyItemsEvent;
				ProcessorBehaviour.Processor.Outputs.OnDirtyItemsEvent	+= _handlers[HandlerType.Output].OnDirtyItemsEvent;
				ProcessorBehaviour.Processor.Outputs.OnRedrawEvent += _handlers[HandlerType.Output].RedrawEvent;

				// Redraw all the slots.
				_handlers[HandlerType.Input].RedrawEvent(ProcessorBehaviour.Processor.Inputs.Items);
				_handlers[HandlerType.Fuel].RedrawEvent(ProcessorBehaviour.Processor.Fuels.Items);
				_handlers[HandlerType.Output].RedrawEvent(ProcessorBehaviour.Processor.Outputs.Items);

				ResizeCanvas(ProcessorBehaviour.Processor);

				GameController.Instance.RegisterLateUpdatable(this);

				_canvas.enabled = true;
			}

			public void HideProcessor(ProcessorBehaviour processorBehaviour) {
				if (ProcessorBehaviour != processorBehaviour) {
					return;
				}

				if (!ReferenceEquals(ProcessorBehaviour, null)) {
					// Unsubscribe to all the events for this processor.
					ProcessorBehaviour.Processor.OnPowerEvent -= _powerBar.UpdateValue;
					ProcessorBehaviour.Processor.OnProgressEvent -= _progressBar.UpdateValue;

					_handlers[HandlerType.Input].OnItemBeginDragEvent -= ProcessorBehaviour.Processor.OnInputBeginDrag;
					_handlers[HandlerType.Input].OnItemEndDragEvent -= ProcessorBehaviour.Processor.OnInputEndDrag;
					_handlers[HandlerType.Output].OnItemEndDragEvent -= ProcessorBehaviour.Processor.OnOutputEndDrag;

					ProcessorBehaviour.Processor.Inputs.OnDirtyItemsEvent	-= _handlers[HandlerType.Input].OnDirtyItemsEvent;
					ProcessorBehaviour.Processor.Fuels.OnDirtyItemsEvent	-= _handlers[HandlerType.Fuel].OnDirtyItemsEvent;
					ProcessorBehaviour.Processor.Outputs.OnDirtyItemsEvent	-= _handlers[HandlerType.Output].OnDirtyItemsEvent;
					ProcessorBehaviour.Processor.Outputs.OnRedrawEvent -= _handlers[HandlerType.Output].RedrawEvent;
				}

				_progressBar.ForceUpdateValue(0);
				_powerBar.ForceUpdateValue(0);

				_canvas.enabled = false;

				GameController.Instance.DeregisterLateUpdatable(this);

				// Reset stored variable.
				ProcessorBehaviour = null;
			}

			public void OnUpdate() {
				Vector2 viewportPoint = Camera.main.WorldToViewportPoint(ProcessorBehaviour.WorldPosition);
				_rectTransform.anchorMin = viewportPoint;
				_rectTransform.anchorMax = viewportPoint;
			}

			#region Resize
			private void ResizeCanvas(Processor processor) {

				int inputCount = 3;
				if (!processor.ProcessorData.RequiresPower) {
					inputCount = 1;
				}

				int maxX = Mathf.Max(processor.Inputs.Items.Length, processor.Fuels.Items.Length);
				int maxY = Mathf.Max(processor.Outputs.Items.Length, inputCount);

				float leftSizeX = (maxX * CellSize) + Padding;
				float middleSizeX = CellSize;
				float rightSizeX = CellSize + Padding;

				float height = (maxY * CellSize) + Padding;

				_leftSection.sizeDelta = new Vector2(leftSizeX, CellSize);
				_middleSection.sizeDelta = new Vector2(middleSizeX, CellSize);
				_rightSection.sizeDelta = new Vector2(rightSizeX, CellSize);

				_rectTransform.sizeDelta = new Vector2(leftSizeX + middleSizeX + rightSizeX, height);
			}
			#endregion

			public bool IsActive { get { return _canvas.enabled; } }
			public ProcessorBehaviour ProcessorBehaviour { get; private set; }

			private enum HandlerType {
				Input = 0,
				Fuel = 1,
				Output = 2
			}
		}
	}
}
