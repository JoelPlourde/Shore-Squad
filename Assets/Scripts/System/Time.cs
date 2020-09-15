using System;
using System.Linq;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

namespace TimeSystem {
	public class Time : MonoBehaviour {

		public static Time Instance;

		private Timer _timer;
		private long _seconds;

		private TimeComparator _timeComparator = new TimeComparator();
		private List<TimedEvent> timedEvents = new List<TimedEvent>();

		private void Awake() {
			Instance = this;

			// TODO Load this from the save file.
			_seconds = 0;

			Initialize();
		}

		public void RegisterTimedEvent(TimedEvent timedEvent) {
			timedEvent.CalculateRelativeTime(_seconds);
			var index = timedEvents.BinarySearch(timedEvent, _timeComparator);
			if (index < 0) index = ~index;
			timedEvents.Insert(index, timedEvent);
		}

		private void Initialize() {
			_timer = new Timer(Constant.TICK_RATE);
			_timer.Elapsed += Routine;
			_timer.AutoReset = true;
			_timer.Enabled = true;
		}

		private void Routine(object sender, EventArgs e) {
			_seconds += Variable.TIME_PER_TICK;
			bool proceed = true;
			do {
				if (timedEvents.Count > 0) {
					TimedEvent timedEvent = timedEvents.First();
					if (timedEvent.RelativeTime <= _seconds) {
						timedEvent.Callback?.Invoke();
						timedEvents.RemoveAt(0);
					} else {
						proceed = false;
					}
				} else {
					proceed = false;
				}
			} while (proceed);
		}

		private void OnApplicationQuit() {
			_timer.Dispose();
		}

		public long GetSeconds() {
			return _seconds;
		}
	}

	public class TimedEvent {
		public long Ticks { get; private set; }
		public long RelativeTime { get; private set; }
		public Action Callback { get; private set; }

		public TimedEvent(long ticks, Action callback) {
			Ticks = ticks;
			Callback = callback;
		}

		public void CalculateRelativeTime(long seconds) {
			RelativeTime = Ticks + seconds;
		}
	}

	public class TimeComparator : IComparer<TimedEvent> {

		public int Compare(TimedEvent x, TimedEvent y) {
			return x.Ticks.CompareTo(y.Ticks);
		}
	}
}
