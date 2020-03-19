using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Siccity.GLTFUtility {
	[CustomEditor(typeof(GLTFImporter))]
	public class GLTFImporterEditor : Editor {

		public override void OnInspectorGUI() {
			EditorGUILayout.LabelField("ASDF");
		}
	}
}