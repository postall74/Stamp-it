﻿using System;
using UnityEngine;

namespace PolygonRender
{
    [CreateAssetMenu(menuName = "GamePolygon Render Settings")]
    [Serializable]
    public class PolygonRenderSettings : ScriptableObject
    {
        [Header("Bloom")]
        public bool bloomExpanded = false;
        public bool bloomEnabled = true;
        public float bloomThreshold = 0.6f;
        public float bloomIntensity = 2.5f;
        public Color bloomTint = Color.white;

        public bool preserveAspectRatio = true;
        public int bloomPasses = 5;
        public int bloomTextureWidth = 128;
        public int bloomTextureHeight = 128;

        public LumaVectorType bloomLumaCalculationType = LumaVectorType.Uniform;
        public Vector3 bloomLumaVector = new Vector3(1f / 3f, 1f / 3f, 1f / 3f);

        [Header("Color overlay (alpha sets intensity)")]
        public bool colorizeExpanded = true;
        public bool colorizeEnabled = true;

        // Warm sepia by default
        public Color32 colorize = new Color32(246, 176, 114, 100);

        [Header("Vignette")]
        public bool vignetteExpanded = true;
        public bool vignetteEnabled = true;

        public float vignetteBeginRadius = 0.166f;
        public float vignetteExpandRadius = 1.34f;
        public Color vignetteColor = Color.black;

        [Header("Contrast/Brightness")]
        public bool brightnessContrastExpanded = false;
        public bool brightnessContrastEnabled = true;

        public float contrast = 0f;
        public float brightness = 0f;
    }

    public enum LumaVectorType
    {
        Uniform,
        sRGB,
        Custom
    }
}