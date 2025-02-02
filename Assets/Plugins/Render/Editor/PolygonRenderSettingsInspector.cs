﻿using System;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

namespace PolygonRender
{
    [CustomEditor(typeof(PolygonRenderSettings))]
    public class PolygonRenderSettingsInspector : Editor
    {
        private SerializedProperty _isBloomGroupExpandedProperty;
        private SerializedProperty _bloomEnabledProperty;
        private SerializedProperty _bloomThresholdProperty;
        private SerializedProperty _bloomIntensityProperty;
        private SerializedProperty _bloomTintProperty;
        private SerializedProperty _bloomPreserveAspectRatioProperty;
        private SerializedProperty _bloomTextureWidth;
        private SerializedProperty _bloomTextureHeight;
        private SerializedProperty _bloomPassCountProperty;
        private SerializedProperty _bloomLumaVectorProperty;
        private SerializedProperty _bloomSelectedLumaVectorTypeProperty;

        private string[] _bloomPassCountVariants = new[] { "3", "5" };
        private int[] _bloomPassCountVariantInts = new[] { 3, 5 };
        private int _selectedBloomPassCountIndex = -1;

        private LumaVectorType _selectedLumaVectorType;

        private SerializedProperty _isColorizeGroupExpandedProperty;
        private SerializedProperty _colorizeEnabledProperty;
        private SerializedProperty _colorizeProperty;

        private SerializedProperty _isVignetteExpandedProperty;
        private SerializedProperty _vignetteEnabledProperty;
        private SerializedProperty _vignetteBeginRadiusProperty;
        private SerializedProperty _vignetteExpandRadiusProperty;
        private SerializedProperty _vignetteColorProperty;

        private SerializedProperty _isContrastAndBrightnessEditorExpandedProperty;
        private SerializedProperty _contrastAndBrightnessEnabledProperty;
        private SerializedProperty _contrasteIntensity;
        private SerializedProperty _brightnesseIntensity;

        private void OnEnable()
        {
            SetupBloomProperties();

            _isColorizeGroupExpandedProperty = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.colorizeExpanded));
            _colorizeEnabledProperty = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.colorizeEnabled));
            _colorizeProperty = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.colorize));

            _isVignetteExpandedProperty = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.vignetteExpanded));
            _vignetteEnabledProperty = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.vignetteEnabled));
            _vignetteBeginRadiusProperty = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.vignetteBeginRadius));
            _vignetteExpandRadiusProperty = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.vignetteExpandRadius));
            _vignetteColorProperty = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.vignetteColor));

            _isContrastAndBrightnessEditorExpandedProperty = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.brightnessContrastExpanded));
            _contrastAndBrightnessEnabledProperty = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.brightnessContrastEnabled));
            _contrasteIntensity = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.contrast));
            _brightnesseIntensity = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.brightness));
        }

        private void SetupBloomProperties()
        {
            _isBloomGroupExpandedProperty =
                serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.bloomExpanded));
            _bloomEnabledProperty = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.bloomEnabled));
            _bloomThresholdProperty = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.bloomThreshold));
            _bloomIntensityProperty = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.bloomIntensity));
            _bloomTintProperty = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.bloomTint));

            _bloomPreserveAspectRatioProperty =
                serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.preserveAspectRatio));

            _bloomTextureWidth = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.bloomTextureWidth));
            _bloomTextureHeight =
                serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.bloomTextureHeight));

            _bloomPassCountProperty = serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.bloomPasses));
            _selectedBloomPassCountIndex = Array.IndexOf(_bloomPassCountVariantInts, _bloomPassCountProperty.intValue);

            _bloomLumaVectorProperty =
                serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.bloomLumaVector));
            _bloomSelectedLumaVectorTypeProperty =
                serializedObject.FindProperty(GetMemberName((PolygonRenderSettings s) => s.bloomLumaCalculationType));
            _selectedLumaVectorType = (LumaVectorType)_bloomSelectedLumaVectorTypeProperty.enumValueIndex;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            int indent = EditorGUI.indentLevel;

            DrawBloomEditor();
            EditorGUILayout.Space();

            DrawColorizeEditor();
            EditorGUILayout.Space();

            DrawVignetteEditor();
            EditorGUILayout.Space();

            DrawContrastAndBrightnessEditor();

            DrawTotalCost();

            EditorGUI.indentLevel = indent;
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawContrastAndBrightnessEditor()
        {
            Header("Brightness / Contrast", _isContrastAndBrightnessEditorExpandedProperty, _contrastAndBrightnessEnabledProperty);

            if (_isContrastAndBrightnessEditorExpandedProperty.boolValue)
            {
                EditorGUI.indentLevel += 1;

                EditorGUILayout.LabelField("Contrast Intensity");
                EditorGUILayout.Slider(_contrasteIntensity, -1f, 1f, "");

                EditorGUILayout.LabelField("Brightness Intensity");
                EditorGUILayout.Slider(_brightnesseIntensity, -1f, 1f, "");

                EditorGUI.indentLevel -= 1;
            }
        }

        private void DrawVignetteEditor()
        {
            Header("Vignette", _isVignetteExpandedProperty, _vignetteEnabledProperty);

            if (_isVignetteExpandedProperty.boolValue)
            {
                EditorGUI.indentLevel += 1;

                EditorGUILayout.LabelField("Begin radius");
                EditorGUILayout.Slider(_vignetteBeginRadiusProperty, 0f, 1f, "");

                EditorGUILayout.LabelField("Expand radius");
                EditorGUILayout.Slider(_vignetteExpandRadiusProperty, 0f, 3f, "");

                EditorGUILayout.LabelField("Color");
                _vignetteColorProperty.colorValue = EditorGUILayout.ColorField("", _vignetteColorProperty.colorValue);

                EditorGUI.indentLevel -= 1;
            }
        }

        private void DrawColorizeEditor()
        {
            Header("Colorize", _isColorizeGroupExpandedProperty, _colorizeEnabledProperty);

            if (_isColorizeGroupExpandedProperty.boolValue)
            {
                EditorGUI.indentLevel += 1;

                EditorGUILayout.LabelField("Color");
                _colorizeProperty.colorValue = EditorGUILayout.ColorField("", _colorizeProperty.colorValue);

                
                EditorGUILayout.HelpBox("Note: Color Alpha value should not be zero for the effect to be visible", MessageType.Info);

                EditorGUI.indentLevel -= 1;
            }
        }

        private void DrawBloomEditor()
        {
            Header("Bloom", _isBloomGroupExpandedProperty, _bloomEnabledProperty);

            if (_isBloomGroupExpandedProperty.boolValue)
            {
                EditorGUI.indentLevel += 1;

                EditorGUILayout.LabelField("Bloom threshold");
                EditorGUILayout.Slider(_bloomThresholdProperty, 0f, 1f, "");
                EditorGUILayout.LabelField("Bloom intensity");
                EditorGUILayout.Slider(_bloomIntensityProperty, 0f, 15f, "");
                EditorGUILayout.LabelField("Bloom tint");
                _bloomTintProperty.colorValue = EditorGUILayout.ColorField("", _bloomTintProperty.colorValue);

                DrawCommonBloomParameters();
                DrawDualFilterBloomTextureSizeSettings();

                DisplayLumaVectorProperties();

                EditorGUI.indentLevel -= 1;
            }
        }

        private void DrawCommonBloomParameters()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Bloom texture size");

            _bloomPreserveAspectRatioProperty.boolValue = EditorGUILayout.ToggleLeft("Preserve aspect ratio", _bloomPreserveAspectRatioProperty.boolValue);

            if (!_bloomPreserveAspectRatioProperty.boolValue)
            {
                EditorGUILayout.IntSlider(_bloomTextureWidth, 32, 164, "X");
            }

            EditorGUILayout.IntSlider(_bloomTextureHeight, 32, 164, "Y");
        }

        private void DrawDualFilterBloomTextureSizeSettings()
        {
            EditorGUILayout.Space();
            var bloomPassesControlRect = EditorGUILayout.GetControlRect();
            var bloomPassesLabelRect = new Rect(bloomPassesControlRect.x, bloomPassesControlRect.y,
                bloomPassesControlRect.width * 0.5f, bloomPassesControlRect.height);
            var bloomPassesPopupRect = new Rect(bloomPassesControlRect.x + bloomPassesLabelRect.width, bloomPassesControlRect.y,
                bloomPassesControlRect.width * 0.5f, bloomPassesControlRect.height);

            EditorGUI.LabelField(bloomPassesLabelRect, "Bloom passes");

            _selectedBloomPassCountIndex = _selectedBloomPassCountIndex != -1 ? _selectedBloomPassCountIndex : 1;
            _selectedBloomPassCountIndex = EditorGUI.Popup(bloomPassesPopupRect, _selectedBloomPassCountIndex, _bloomPassCountVariants);
            _bloomPassCountProperty.intValue = _bloomPassCountVariantInts[_selectedBloomPassCountIndex];
        }

        private void DisplayLumaVectorProperties()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Brightpass Luma calculation");

            _selectedLumaVectorType = (LumaVectorType)EditorGUILayout.EnumPopup(_selectedLumaVectorType);
            _bloomSelectedLumaVectorTypeProperty.enumValueIndex = (int)_selectedLumaVectorType;
            switch (_selectedLumaVectorType)
            {
                case LumaVectorType.Custom:
                    EditorGUILayout.PropertyField(_bloomLumaVectorProperty, new GUIContent(""));
                    break;
                case LumaVectorType.Uniform:
                    var oneOverThree = 1f / 3f;
                    _bloomLumaVectorProperty.vector3Value = new Vector3(oneOverThree, oneOverThree, oneOverThree);
                    break;
                case LumaVectorType.sRGB:
                    _bloomLumaVectorProperty.vector3Value = new Vector3(0.2126f, 0.7152f, 0.0722f);
                    break;
            }

            var vector = _bloomLumaVectorProperty.vector3Value;
            if (!Mathf.Approximately(vector.x + vector.y + vector.z, 1f))
            {
                EditorGUILayout.HelpBox("Luma vector is not normalized.\nVector values should sum up to 1.",
                    MessageType.Warning);
            }
        }

        private void DrawTotalCost()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.HelpBox(PolygonRenderCostCalculator.GetTotalCostStringFor(target as PolygonRenderSettings),
                MessageType.Info);
        }

        public static bool Header(string title, SerializedProperty isExpanded, SerializedProperty enabledField)
        {
            var display = isExpanded == null || isExpanded.boolValue;
            var enabled = enabledField.boolValue;
            var rect = GUILayoutUtility.GetRect(16f, 22f, FxStyles.header);
            GUI.Box(rect, title, FxStyles.header);

            var toggleRect = new Rect(rect.x + 4f, rect.y + 4f, 13f, 13f);
            var e = Event.current;

            if (e.type == EventType.Repaint)
            {
                FxStyles.headerCheckbox.Draw(toggleRect, false, false, enabled, false);
            }

            if (e.type == EventType.MouseDown)
            {
                const float kOffset = 2f;
                toggleRect.x -= kOffset;
                toggleRect.y -= kOffset;
                toggleRect.width += kOffset * 2f;
                toggleRect.height += kOffset * 2f;

                if (toggleRect.Contains(e.mousePosition))
                {
                    enabledField.boolValue = !enabledField.boolValue;
                    e.Use();
                }
                else if (rect.Contains(e.mousePosition) && isExpanded != null)
                {
                    display = !display;
                    isExpanded.boolValue = !isExpanded.boolValue;
                    e.Use();
                }
            }

            return display;
        }

        public static string GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
        {
            return ((MemberExpression)memberAccess.Body).Member.Name;
        }
    }
}