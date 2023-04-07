using UnityEngine;
using System.Collections;

/* Simple Physics Toolkit - Bounce Plate
 * Description: A varient on Fan Controller - applies impulse force, using world space (UP)
 * Required Components: Collider (All Types Supported)
 * Author: Dylan Anthony Auty
 * Version: 1.1
 * Last Change: 2020-12-03
*/

namespace SimplePhysicsToolkit {
	public class BouncePlate : MonoBehaviour {

		public float bounce = 10.0f;
		public bool onlyAffectInteractableItems = false;

		public ColliderEvent onBounce;

		void Start () {
			if (GetComponent<Collider> ()) {
				GetComponent<Collider>().isTrigger = true;
			}
		}
		
		void OnTriggerEnter(Collider other){
			if (other.GetComponent<Rigidbody> ()) {
				if (onlyAffectInteractableItems) {
					if (other.GetComponent<InteractableItem> ()) {
						ApplyBounce(other);
					}
				} else {
					ApplyBounce(other);
				}
			}
		}

		void OnTriggerStay(Collider other){
			if (other.GetComponent<Rigidbody> ()) {
				if (onlyAffectInteractableItems) {
					if (other.GetComponent<InteractableItem> ()) {
						ApplyBounce(other);
					}
				} else {
					ApplyBounce(other);
				}
			}
		}

		void ApplyBounce(Collider other){
			other.GetComponent<Rigidbody> ().AddForce (bounce * Vector3.up, ForceMode.Impulse);
			onBounce.Invoke(other);
		}

		void OnDrawGizmos(){
			Gizmos.color = Color.cyan;
			if (GetComponent<BoxCollider> ()) {
				BoxCollider c = GetComponent<BoxCollider> ();
				Gizmos.DrawWireCube (transform.position, c.bounds.size);
			}
		}
	}
}
