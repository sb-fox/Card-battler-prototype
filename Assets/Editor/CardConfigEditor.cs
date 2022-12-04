﻿using System;
using Cards;
using Creatures;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardConfig))]
public class CardConfigEditor : Editor
{
	private SerializedProperty _cardTargetTypeProperty;
	private SerializedProperty _effectsListProperty;
	private SerializedProperty _cardTypeProperty;
	private SerializedProperty _prefabProperty;
	private CardConfig _script;
	private readonly GUILayoutOption _effectDurationWidth = GUILayout.Width(75);
	private readonly GUILayoutOption _valuesWidth = GUILayout.Width(150);
	private readonly GUILayoutOption _labelsWidth = GUILayout.Width(100);
	private readonly GUILayoutOption _xButtonWidth = GUILayout.Width(60), _xButtonHeight = GUILayout.Height(60);

	public override void OnInspectorGUI()
	{
		_script = target as CardConfig;
		_cardTargetTypeProperty = serializedObject.FindProperty("targetType");
		_effectsListProperty = serializedObject.FindProperty("effects");
		_cardTypeProperty = serializedObject.FindProperty("type");
		_prefabProperty = serializedObject.FindProperty("prefab");

		DrawDefaultInspector();

		//DrawSpecificScriptLayout();

		serializedObject.ApplyModifiedProperties();
		if (GUI.changed)
			EditorUtility.SetDirty(_script);
	}

	private void DrawSpecificScriptLayout()
	{
		//EditorGUILayout.BeginHorizontal();
		//GUILayout.Label("Card type", EditorStyles.whiteLargeLabel, GUILayout.ExpandWidth(false));
		//var cardType = DrawCardTypeEnumPopup();
		//EditorGUILayout.EndHorizontal();
		//GUILayout.Space(10);

		//EditorGUILayout.ObjectField(_prefabProperty);

		//EditorGUILayout.BeginHorizontal();
		//GUILayout.Label("Target type:", GUILayout.ExpandWidth(false));
		//DrawCardTargetTypeEnumPopup();
		//EditorGUILayout.EndHorizontal();

		//GUILayout.Label("Effects:");
		//switch (cardType)
		//{
		//	case CardType.InstantEffect:
		//		DrawInstantEffectBlock(); break;
		//	case CardType.TemporaryEffect:
		//		DrawTemporaryEffectBlock(); break;
		//	default:
		//		break;
		//}

		//DrawNavigationButtons();
	}

	private void DrawInstantEffectBlock()
	{
		DrawListElements(false);
	}

	private void DrawTemporaryEffectBlock()
	{
		DrawListElements(true);
	}

	private void DrawListElements(bool drawDurationField)
	{
		EditorGUILayout.BeginVertical(EditorStyles.helpBox); // level: 0
		for (var i = 0; i < _effectsListProperty.arraySize; i++)
		{
			var property = _effectsListProperty.GetArrayElementAtIndex(i);

			EditorGUILayout.BeginHorizontal(EditorStyles.helpBox); // level: 1
			EditorGUILayout.BeginVertical(); // level: 2
			EditorGUILayout.BeginHorizontal(); // level: 3
			GUILayout.Label("Stat:", _labelsWidth);
			var type = property.FindPropertyRelative("type");
			var selectedTypeName = EditorGUILayout.EnumPopup((CreatureStatType)type.enumValueIndex, _valuesWidth).ToString();
			var selectedTypeIndex = (int)Enum.Parse<CreatureStatType>(selectedTypeName);
			type.enumValueIndex = selectedTypeIndex;
			EditorGUILayout.EndHorizontal(); // level: 3
			EditorGUILayout.BeginHorizontal(); // level: 3
			GUILayout.Label("Modifier:", _labelsWidth);
			var value = property.FindPropertyRelative("value");
			value.floatValue = EditorGUILayout.FloatField(value.floatValue, _valuesWidth);
			EditorGUILayout.EndHorizontal(); // level: 3
			if (drawDurationField)
			{
				EditorGUILayout.BeginHorizontal(); // level: 3
				GUILayout.Label("Turns duration:", _labelsWidth);
				var duration = property.FindPropertyRelative("duration");
				duration.intValue = EditorGUILayout.IntField(duration.intValue, _effectDurationWidth);
				EditorGUILayout.EndHorizontal(); // level: 3
			}
			EditorGUILayout.EndVertical(); // level: 2
			if (GUILayout.Button("X", _xButtonWidth, _xButtonHeight))
				_effectsListProperty.DeleteArrayElementAtIndex(i);
			EditorGUILayout.EndHorizontal(); // level: 1
		}
		EditorGUILayout.EndVertical(); // level: 0
	}

	private void DrawNavigationButtons()
	{
		if (GUILayout.Button("Add new effect"))
			_effectsListProperty.InsertArrayElementAtIndex(0);
		if (GUILayout.Button("Remove all effects"))
		{
			if (EditorUtility.DisplayDialog("Effects removing", "Remove all effects?", "Sure!", "Nope."))
				_effectsListProperty.ClearArray();
		}
	}

	//private CardType DrawCardTypeEnumPopup()
	//{
	//	var selectedTypeName = EditorGUILayout.EnumPopup((CardType)_cardTypeProperty.enumValueIndex, _valuesWidth).ToString();
	//	var selectedTypeIndex = (int)Enum.Parse<CardType>(selectedTypeName);
	//	_cardTypeProperty.enumValueIndex = selectedTypeIndex;
	//	return (CardType)selectedTypeIndex;
	//}

	private void DrawCardTargetTypeEnumPopup()
	{
		var selectedTypeName = EditorGUILayout.EnumPopup((CardTargetType)_cardTargetTypeProperty.enumValueIndex,
												   _valuesWidth,
												   GUILayout.ExpandWidth(false)).ToString();
		var selectedTypeIndex = (int)Enum.Parse<CardTargetType>(selectedTypeName);
		_cardTargetTypeProperty.enumValueIndex = selectedTypeIndex;
	}
}
