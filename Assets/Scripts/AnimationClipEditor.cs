using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class AnimationClipUpdater : EditorWindow
{
    GameObject sourceFbx;
    string folder = "Clips";

    [MenuItem("Window/Animation/Animation Events Migrator")]
    public static void ShowWindow()
    {
        AnimationClipUpdater wnd = GetWindow<AnimationClipUpdater>();
        wnd.titleContent = new GUIContent("Animation Clip Updater");
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        GUILayout.Label("Fill this in to update your animation clips while keeping their events and settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        sourceFbx = (GameObject) EditorGUILayout.ObjectField("Source FBX", sourceFbx, typeof (GameObject), false);
        folder = EditorGUILayout.TextField(new GUIContent("Clips Resources Folder", "The path is passed straight to Resources.LoadAll"), folder);

        EditorGUILayout.Separator();
        if (GUILayout.Button("Update"))
        {
            if (sourceFbx == null)
            {
                Debug.LogError("Missing source FBX.");
            }
            else
            {
                UpdateClips();
            }
        }
    }

    // https://forum.unity.com/threads/how-can-i-get-all-the-animation-clips-imported-from-an-fbx-solved.431669/
    // ^ this was incredibly helpful
    // https://discussions.unity.com/t/assetdatabase-replacing-an-asset-but-leaving-reference-intact/6549/2
    // ^ This told me about CopySerialized and SaveAssets
    void UpdateClips()
    {
        string sourceAssetPath = AssetDatabase.GetAssetPath(sourceFbx);
        string clipsPath = sourceAssetPath + "Resources/" + folder;

        AnimationClip[] newClips = GetAnimationClipsFromImporter(sourceAssetPath);
        AnimationClip[] oldClips = Resources.LoadAll<AnimationClip>(folder);

        foreach(AnimationClip newClip in newClips) {
            AnimationClip oldClip = Array.Find(oldClips, (AnimationClip old) => old.name == newClip.name);
            if (oldClip != null) {
                // Save stuff to port to new animation
                AnimationEvent[] events = AnimationUtility.GetAnimationEvents(oldClip);
                AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(oldClip);

                // Copy new animations into old animation
                EditorUtility.CopySerialized(newClip, oldClip);

                // Reset old stuff to old animation
                AnimationUtility.SetAnimationEvents(oldClip, events);
                AnimationUtility.SetAnimationClipSettings(oldClip, settings);

                AssetDatabase.SaveAssets();
                Debug.Log("Updated " + oldClip.name);
            } else {
                Debug.Log("New asset " + newClip.name);
                oldClip = new AnimationClip();
                EditorUtility.CopySerialized(newClip, oldClip);
                AssetDatabase.CreateAsset (oldClip, clipsPath);
            }
        }
    }

    AnimationClip[] GetAnimationClipsFromImporter(string modelImporterPath)
    {
        var animationClips = new List<AnimationClip>();

        var modelImporterAssets = AssetDatabase.LoadAllAssetsAtPath(modelImporterPath);
        foreach (var asset in modelImporterAssets)
        {
            if (asset is AnimationClip clip && !asset.name.Contains("__preview__"))
            {
                animationClips.Add(clip);
            }
        }

        return animationClips.ToArray();
    }
}