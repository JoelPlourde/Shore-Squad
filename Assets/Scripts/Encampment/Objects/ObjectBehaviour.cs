using UnityEngine;
using EncampmentSystem;
using ConstructionSystem;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(NavMeshObstacle))]
public abstract class ObjectBehaviour : MonoBehaviour, IConstructable
{
	public NavMeshObstacle NavMeshObstacle { get; private set; }
	public Stack<ZoneBehaviour> ZoneBehaviours { get; protected set; }

	public virtual void Initialize() {
		ZoneBehaviours = new Stack<ZoneBehaviour>();
		NavMeshObstacle = GetComponent<NavMeshObstacle>();
		NavMeshObstacle.carving = false;
		NavMeshObstacle.enabled = false;
	}

	public virtual void Enable() {
		NavMeshObstacle.enabled = true;
		NavMeshObstacle.carving = true;								// Enable the carving of the NavMeshObstacle
		gameObject.layer = LayerMask.NameToLayer("Default");        // Reset the Layer to Default
		ZoneBehaviour?.Map.DrawRectangle(NavMeshObstacle, ZoneBehaviour, Color.red); // Record the position of this object on the map.

		// TODO REMOVE THIS AT ONE POINT.
		if (ZoneBehaviour) {
			Tester2.Instance.DrawMap(ZoneBehaviour.Map);
		}
	}

	public virtual void Disable() {
		NavMeshObstacle.carving = false;
		NavMeshObstacle.enabled = false;
		ZoneBehaviour?.Map.DrawRectangle(NavMeshObstacle, ZoneBehaviour, Color.white); // Reset the position of this object on the map.

		// TODO REMOVE THIS AT ONE POINT.
		if (ZoneBehaviour) {
			Tester2.Instance.DrawMap(ZoneBehaviour.Map);
		}
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
			return ZoneBehaviour.Map.IsPositionValid(NavMeshObstacle, ZoneBehaviour);
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
