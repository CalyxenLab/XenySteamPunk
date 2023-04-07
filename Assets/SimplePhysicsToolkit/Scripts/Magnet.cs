using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/* Simple Physics Toolkit - Magnet
 * Description: Magnet applies force to any rigidbody within range
 * Required Components: None
 * Author: Dylan Anthony Auty
 * Version: 1.4
 * Last Change: 2020-12-03
*/
namespace SimplePhysicsToolkit {
	public class Magnet : MonoBehaviour {
		public float magnetForce = 15.0f;
		public bool enable = true;
		public bool attract = true;
		public float innerRadius = 2.0f;
		public float outerRadius = 5.0f;

		public bool useColliderAsTrigger = false;
		public bool onlyAffectInteractableItems = false;
		public bool realismMode = false;

		public ColliderEvent onAffect;

		void FixedUpdate () {
			if (enable) {
				if (!useColliderAsTrigger) {
					Collider[] objects = Physics.OverlapSphere(transform.position, outerRadius);
					foreach (Collider col in objects) {
						if (col.GetComponent<Rigidbody>()) { //Must be rigidbody
							if (onlyAffectInteractableItems) {
								if (col.GetComponent<InteractableItem>()) {
									attractOrRepel(col);
								}
							} else {
								attractOrRepel(col);
							}
						}
					}
				} else { 
					/* The user has opted to use a custom collider as a trigger, we don't need to do anything as this is handled by magic methods */
				}
			}
		}

        void OnTriggerStay(Collider col){
            if (useColliderAsTrigger){
                if (col.GetComponent<Rigidbody>()){
					if (onlyAffectInteractableItems) {
						if (col.GetComponent<InteractableItem>()) {
							attractOrRepel(col);
						}
					} else {
						attractOrRepel(col);
					}
				}
			}
        }

        void attractOrRepel(Collider col){
			if (!useColliderAsTrigger) { 
				if (Vector3.Distance (transform.position, col.transform.position) > innerRadius) {
					//Apply force in direction of magnet center
					if (realismMode) {
						float dynamicDistance = Mathf.Abs( (Vector3.Distance (transform.position, col.transform.position) ) - (outerRadius + (innerRadius * 2)) );
						float multiplier = dynamicDistance / outerRadius;

						if (attract) {
							col.GetComponent<Rigidbody> ().AddForce ( (magnetForce * (transform.position - col.transform.position).normalized) * multiplier, ForceMode.Force);
						} else {
							col.GetComponent<Rigidbody>().AddForce(-(magnetForce * (transform.position - col.transform.position).normalized) * multiplier, ForceMode.Force);
						}
					} else {
						if (attract) {
							col.GetComponent<Rigidbody> ().AddForce (magnetForce * (transform.position - col.transform.position).normalized, ForceMode.Force);
						} else { 
							col.GetComponent<Rigidbody> ().AddForce (-magnetForce * (transform.position - col.transform.position).normalized, ForceMode.Force);
						}
					}
					
				} else {
					//Inner Radius float gentle - Future additional handling here
				}
			} else {
				/* Handles as part of the new collider logic
				 * 
				 * This system does not need to check inner radius as it does not apply
				 * 
				 * Date: 2021-12-22
				*/
				Collider trigger = GetComponent<Collider>();
				if (realismMode) {
					float longestSide = Mathf.Max(trigger.bounds.size.x, trigger.bounds.size.y);
					longestSide = Mathf.Max(longestSide, trigger.bounds.size.z);

					float dynamicDistance = Mathf.Abs((Vector3.Distance (transform.position, col.transform.position)) - (longestSide));
					float multiplier = dynamicDistance / longestSide;

					if (attract) {
						col.GetComponent<Rigidbody>().AddForce((magnetForce * (transform.position - col.transform.position).normalized) * multiplier, ForceMode.Force);
					} else {
						col.GetComponent<Rigidbody>().AddForce(-(magnetForce * (transform.position - col.transform.position).normalized) * multiplier, ForceMode.Force);
					}
				} else {
                    if (attract){
						col.GetComponent<Rigidbody> ().AddForce (magnetForce * (transform.position - col.transform.position).normalized, ForceMode.Force);
					} else { 
						col.GetComponent<Rigidbody> ().AddForce (-magnetForce * (transform.position - col.transform.position).normalized, ForceMode.Force);
					}
				}
               
            }

			onAffect.Invoke(col);
		}

		void OnDrawGizmos(){
			if (enable) {
				if (!useColliderAsTrigger){
					Gizmos.color = Color.red;
					Gizmos.DrawWireSphere(transform.position, outerRadius);
					Gizmos.color = Color.yellow;
					Gizmos.DrawWireSphere(transform.position, innerRadius);

					Gizmos.DrawIcon (transform.position, "sptk_magnet.png", true);
				}
			}
		}
	}
}
