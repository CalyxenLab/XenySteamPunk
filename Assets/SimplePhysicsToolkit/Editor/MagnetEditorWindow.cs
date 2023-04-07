using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SimplePhysicsToolkit {
	[CustomEditor(typeof(Magnet))]
	public class MagnetEditorWindow : Editor {

		private Magnet currentScript;

		void OnEnable(){
			currentScript = (Magnet)target;
		}

		public override void OnInspectorGUI(){

			currentScript.enable = EditorGUILayout.Toggle ("Enabled", currentScript.enable);
			currentScript.attract = EditorGUILayout.Toggle ("Attract", currentScript.attract);

			currentScript.magnetForce = EditorGUILayout.FloatField ("Force", currentScript.magnetForce);

			EditorGUILayout.Space();
			currentScript.useColliderAsTrigger = EditorGUILayout.Toggle("Use Collider As Trigger", currentScript.useColliderAsTrigger);

			if (!currentScript.useColliderAsTrigger) {
				currentScript.innerRadius = EditorGUILayout.FloatField("Inner Radius", currentScript.innerRadius);
				currentScript.outerRadius = EditorGUILayout.FloatField("Outer Radius", currentScript.outerRadius);
			} else{
				string message = "Using attached collider as the 'magnet zone' elements which enter the collider will be affected";
				MessageType type = MessageType.Info;
				if(currentScript.GetComponent<Collider>() != null) {
                    if (!currentScript.GetComponent<Collider>().isTrigger) {
						message = "You have a collider attached to this magner, however, it is not set to 'Is Trigger' and as such no magnet forces will be applied";
						type = MessageType.Warning;
					}
				} else {
					message = "You do not have a collider attached to this magnet, please add one to use it as a 'magnet zone', or switch back to simple radius mode";
					type = MessageType.Error;
                }

				EditorGUILayout.HelpBox(message, type);

			}


			EditorGUILayout.Space();

			currentScript.onlyAffectInteractableItems = EditorGUILayout.Toggle ("Only Interactable Items", currentScript.onlyAffectInteractableItems);

			if (currentScript.onlyAffectInteractableItems) {
				EditorGUILayout.HelpBox("Will only affect objects with the InteractableItem.cs script attached", MessageType.Warning);
			}

			currentScript.realismMode = EditorGUILayout.Toggle ("Realism Mode", currentScript.realismMode);

			if (currentScript.realismMode) {
				EditorGUILayout.HelpBox("Force accounts for object distance as well, increasing realism of effect\n\nAttempts to obey Newton's second law. Thanks to Marc Leatham for his assistance with this.", MessageType.Info);
			}

			EditorGUILayout.Space();
			
			SerializedProperty affectEvent = serializedObject.FindProperty("onAffect");
			  
			EditorGUILayout.PropertyField(affectEvent);
 
			serializedObject.ApplyModifiedProperties();

		}
	}
}
