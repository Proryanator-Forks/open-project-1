using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Character))]
public class CharacterEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		Character targetCharacter = (Character)target;
		EditorGUILayout.LabelField("Current state ", targetCharacter.GetCurrentStateString());
	}
}
