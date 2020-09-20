using UnityEngine;
using EncampmentSystem;
using ConstructionSystem;
using UnityEngine.AI;
using System.Collections.Generic;
using NavigationSystem;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Obstacle))]
public abstract class ObjectBehaviour : MonoBehaviour, IConstructable
{
	public Obstacle Obstacle { get; private set; }
	public Stack<ZoneBehaviour> ZoneBehaviours { get; protected set; }

	public virtual void Initialize() {
		ZoneBehaviours = new Stack<ZoneBehaviour>();
		Obstacle = GetComponent<Obstacle>();
	}

	public virtual void Enable() {
		Obstacle.Enable();
		gameObject.layer = LayerMask.NameToLayer("Default");        // Reset the Layer to Default
		ZoneBehaviour?.Map.DrawRectangle(Obstacle, ZoneBehaviour, Color.red); // Record the position of this object on the map.

		// TODO REMOVE THIS AT ONE POINT.
		if (ZoneBehaviour) {
			Tester.Instance.DrawMap(ZoneBehaviour.Map);
		}
	}

	public virtual void Disable() {
		Obstacle.Disable();
		ZoneBehaviour?.Map.DrawRectangle(Obstacle, ZoneBehaviour, Color.white); // Reset the position of this object on the map.

		// TODO REMOVE THIS AT ONE POINT.
		if (ZoneBehaviour) {
			Tester.Instance.DrawMap(ZoneBehaviour.Map);
		}
	}

	public void Rotate() {
		transform.Rotate(Vector3.up, 90f);
	}

	#region Zone Management
	public virtual void RegisterZone(ZoneBehaviour zoneBehaviour) {
		Debug.Log("Registering: " + zoneBehaviour + " has the zone where the object is: " + this.name);
		ZoneBehaviours.Push(zoneBehaviour);
	}

	public virtual void UnregisterZone() {
		Debug.Log("Unregistering: " + ZoneBehaviour + " has the zone where the object is: " + this.name);
		ZoneBehaviours.Pop();
	}
	#endregion

	#region IPlaceable
	public virtual bool IsPlacementValid() {
		if (ZoneBehaviour) {
			return ZoneBehaviour.Map.IsPositionValid(Obstacle, ZoneBehaviour);
		} else {
			return false;
		}
	}
	#endregion

	public ZoneBehaviour ZoneBehaviour { get {
			if (ZoneBehaviours.Count > 0) {
				return ZoneBehaviours.Peek();
			}
			return null;
		}
	}
}
