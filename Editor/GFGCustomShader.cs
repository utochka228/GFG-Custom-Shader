using System;
using UnityEditor;
using UnityEngine;

public class GFGCustomShader : ShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties) {
        GUIStyle headerStyle = new GUIStyle();
        headerStyle.fontSize = 16;
        headerStyle.normal.textColor = Color.white;

        GUILayout.BeginVertical();
        //Main texture settings
        GUILayout.BeginVertical("box");
        GUILayout.Label("Texture Settings", headerStyle);

        GUILayout.BeginHorizontal(GUILayout.Width(25), GUILayout.Height(10));
        MaterialProperty mainTex = FindProperty("_MainTex", properties);
        Rect texRect = EditorGUILayout.GetControlRect(GUILayout.MaxWidth(120));
        materialEditor.TexturePropertyMiniThumbnail(texRect, mainTex, "Main Texture", "");
        MaterialProperty albedoColor = FindProperty("AlbedoColor", properties);
        Rect colorRect = EditorGUILayout.GetControlRect(GUILayout.MaxWidth(60), GUILayout.Width(60));
        materialEditor.ColorProperty(colorRect, albedoColor, "");
        MaterialProperty useTint = FindProperty("UseTint", properties);
        bool value = useTint.floatValue > 0 ? true : false;
        value = EditorGUILayout.Toggle("", value, GUILayout.MaxWidth(10), GUILayout.Width(10));
        useTint.floatValue =  value == true ? 1f : 0f;
        GUILayout.Label("Tint", GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();

        MaterialProperty tilling = FindProperty("Texture_Tilling", properties);
        materialEditor.ShaderProperty(tilling, "Main texture tilling");
        MaterialProperty offset = FindProperty("Texture_Offset", properties);
        materialEditor.ShaderProperty(offset, "Main texture offset");

        try {
            if(useRefraction == false) {
                MaterialProperty opacity = FindProperty("Opacity", properties);
                materialEditor.ShaderProperty(opacity, opacity.displayName);
            }
            MaterialProperty alphaThreshold = FindProperty("AlphaThreshold", properties);
            materialEditor.ShaderProperty(alphaThreshold, alphaThreshold.displayName);
        } catch {

        }
        GUILayout.Space(15);

        MaterialProperty _useSpecular = FindProperty("UseSpecularMap", properties);
        materialEditor.ShaderProperty(_useSpecular, _useSpecular.displayName);
        bool useSpecularMap = _useSpecular.floatValue > 0 ? true : false;
        if (useSpecularMap) {
            EditorGUILayout.BeginHorizontal();
            MaterialProperty specularMap = FindProperty("_Specular", properties);
            Rect thumb = EditorGUILayout.GetControlRect(GUILayout.MaxWidth(85));
            materialEditor.TexturePropertyMiniThumbnail(thumb, specularMap, "Specular", "");
            MaterialProperty mask = FindProperty("Mask", properties);
            materialEditor.ShaderProperty(mask, "");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            MaterialProperty specularMap2 = FindProperty("_Specular2", properties);
            Rect thumb2 = EditorGUILayout.GetControlRect(GUILayout.MaxWidth(85));
            materialEditor.TexturePropertyMiniThumbnail(thumb2, specularMap2, "Specular", "");
            MaterialProperty mask2 = FindProperty("Mask2", properties);
            materialEditor.ShaderProperty(mask2, "");
            EditorGUILayout.EndHorizontal();

            MaterialProperty specBlend = FindProperty("SpecBlend", properties);
            materialEditor.ShaderProperty(specBlend, "Blend Specular");

            GUILayout.Space(15);
        } else {
            MaterialProperty _metallic = FindProperty("Metallic", properties);
            materialEditor.ShaderProperty(_metallic, _metallic.displayName);
        }
        MaterialProperty _smoothness = FindProperty("Smoothness", properties);
        materialEditor.ShaderProperty(_smoothness, _smoothness.displayName);
        GUILayout.Space(15);

        EditorGUILayout.BeginHorizontal();
        //Main tilled normal
        MaterialProperty normal = FindProperty("_NormalTex_UV0", properties);
        Rect normalRect = EditorGUILayout.GetControlRect(GUILayout.MaxWidth(85));
        materialEditor.TexturePropertyMiniThumbnail(normalRect, normal, "Normal", "");
        MaterialProperty normalPower = FindProperty("NormalPower_UV0", properties);
        materialEditor.ShaderProperty(normalPower, "");

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        //Additional normal
        EditorGUI.indentLevel++;
        EditorGUILayout.BeginHorizontal();
        MaterialProperty normalTextUV1 = FindProperty("_NormalTex_UV1", properties);
        Rect normalTextUV1Rect = EditorGUILayout.GetControlRect(GUILayout.MaxWidth(100));
        materialEditor.TexturePropertyMiniThumbnail(normalTextUV1Rect, normalTextUV1, "NormalUV1", "");
        MaterialProperty normalPower2 = FindProperty("NormalPower_UV1", properties);
        materialEditor.ShaderProperty(normalPower2, "");
        EditorGUILayout.EndHorizontal();

        MaterialProperty normalTillingUV1 = FindProperty("NormalTillingUV1", properties);
        materialEditor.ShaderProperty(normalTillingUV1, "Normal UV1 tilling");
        MaterialProperty normalOffsetUV1 = FindProperty("NormalOffsetUV1", properties);
        materialEditor.ShaderProperty(normalOffsetUV1, "Normal UV1 offset");
        EditorGUI.indentLevel--;

        GUILayout.Space(5);

        MaterialProperty useEmission = FindProperty("UseEmission", properties);
        materialEditor.ShaderProperty(useEmission, useEmission.displayName);
        bool _useEmission= useEmission.floatValue > 0 ? true : false;
        if (_useEmission) {
            EditorGUILayout.BeginHorizontal();
            //Main Emission

            MaterialProperty emissionTexture = FindProperty("EmissionTexUV0", properties);
            materialEditor.TexturePropertyMiniThumbnail(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(100)), emissionTexture, "Emission", "");
            MaterialProperty emission = FindProperty("_Emission", properties);
            materialEditor.ShaderProperty(emission, "");
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            //Additional Emission
            MaterialProperty emissionTexUV1 = FindProperty("EmissionTexUV1", properties);
            materialEditor.TexturePropertyMiniThumbnail(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(100)), emissionTexUV1, "EmissionUV1", "");
            EditorGUILayout.EndHorizontal();

            MaterialProperty emissionTillingUV1 = FindProperty("EmissionTillingUV1", properties);
            materialEditor.ShaderProperty(emissionTillingUV1, "EmissionUV1 tilling");
            MaterialProperty emissionOffsetUV1 = FindProperty("EmissionOffsetUV1", properties);
            materialEditor.ShaderProperty(emissionOffsetUV1, "EmissionUV1 offset");
            EditorGUI.indentLevel--;
        }

        GUILayout.EndVertical();
        //AO
        GUILayout.BeginVertical("box");
        
        GUILayout.Label("AO settings", headerStyle);
        MaterialProperty textureAO = FindProperty("_AO_UV0", properties);
        materialEditor.TexturePropertyMiniThumbnail(EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true)), textureAO, "Main texture AO", "");
        MaterialProperty texStrength = FindProperty("AO_UV0_Strength", properties);
        materialEditor.ShaderProperty(texStrength, "Strength");

        MaterialProperty textureAO_uv1 = FindProperty("_AO_UV1", properties);
        materialEditor.TexturePropertyMiniThumbnail(EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true)), textureAO_uv1, "UV1 texture", "");
        MaterialProperty texStrength_uv1 = FindProperty("AO_UV1_Strength", properties);
        materialEditor.ShaderProperty(texStrength_uv1, "Strength");

        MaterialProperty textureAO_uv2 = FindProperty("_AO_UV2", properties);
        materialEditor.TexturePropertyMiniThumbnail(EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true)), textureAO_uv2, "UV2 texture", "");
        MaterialProperty texStrength_uv2 = FindProperty("AO_UV2_Strength", properties);
        materialEditor.ShaderProperty(texStrength_uv2, "Strength");

        GUILayout.Space(15);
        GUILayout.EndVertical();

        //ADVANCED
        GUILayout.BeginVertical();
        GUILayout.Label("Advanced", headerStyle);
      
        //Fresnel
        GUILayout.BeginVertical("box");
        MaterialProperty _useFresnel = FindProperty("UseFresnel", properties);
        materialEditor.ShaderProperty(_useFresnel, _useFresnel.displayName);
        bool useFresnel = _useFresnel.floatValue > 0 ? true : false;
        if (useFresnel) {
            MaterialProperty fresnel = FindProperty("Fresnel", properties);
            materialEditor.ShaderProperty(fresnel, fresnel.displayName);
            MaterialProperty intensivity = FindProperty("Fresnel_Intens", properties);
            materialEditor.ShaderProperty(intensivity, "Fresnel Intensivity");
            MaterialProperty color = FindProperty("FresnelColor", properties);
            materialEditor.ShaderProperty(color, "Fresnel Color");

        }
        try {
            MaterialProperty ior = FindProperty("IOR", properties);
            MaterialProperty _useRefraction = FindProperty("UseRefraction", properties);
            materialEditor.ShaderProperty(_useRefraction, _useRefraction.displayName);
            useRefraction = _useRefraction.floatValue > 0 ? true : false;
            if (useRefraction) {
                MaterialProperty opacity = FindProperty("Opacity", properties);
                opacity.floatValue = 1;
                materialEditor.ShaderProperty(ior, "Refraction");
                MaterialProperty chromeAbberation = FindProperty("ChromeAbberation", properties);
                materialEditor.ShaderProperty(chromeAbberation, "Chromatic abberation");
            } else
                ior.floatValue = -1;
        } catch (Exception ex) {
            
        }

        GUILayout.EndVertical();
        GUILayout.EndVertical();

        //BUILT_IN
        materialEditor.RenderQueueField();
        materialEditor.DoubleSidedGIField();
        materialEditor.EnableInstancingField();
        GUILayout.EndVertical();
    }
    bool useRefraction;
}
