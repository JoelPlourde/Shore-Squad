using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularBuffer<T> {

	private T[] _buffers;

	private int _index;

	public CircularBuffer(int capacity) {
		_buffers = new T[capacity];
	}

	/// <summary>
	/// Whether or not the element at the current index has been populated or not.
	/// </summary>
	/// <returns></returns>
	public bool IsNull() {
		return _buffers[_index] == null;
	}

	/// <summary>
	/// Insert Element at the current index. May override existing values.
	/// </summary>
	/// <param name="t">The element to insert at current index.</param>
	/// <returns>Returns the element that has been inserted.</returns>
	public T Insert(T t) {
		_buffers[_index] = t;
		Increment();
		return t;
	}

	/// <summary>
	/// Get the current Element at index and increment the index.
	/// </summary>
	/// <returns>Returns the current element.</returns>
	public T Next() {
		T t = _buffers[_index];
		Increment();
		return t;
	}

	private void Increment() {
		_index = (_index + 1) % _buffers.Length;
	}
}
