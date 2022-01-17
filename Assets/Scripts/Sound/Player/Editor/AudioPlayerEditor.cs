using Sound.Source.Internal;
using Sound.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Sound.Player
{
    [CustomEditor(typeof(AudioPlayer))]
    public class AudioPlayerEditor : Editor
    {
        bool foldState;
        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            AudioPlayer player = (AudioPlayer)target;
            EditorGUILayout.LabelField("Is Playing: ", (player.IsPlaying ? "true" : "false"));
            EditorGUILayout.LabelField("Is Delayed: ", (player.IsDelayed ? "true" : "false"));
            GUILayout.Space(20);
            EditorGUILayout.FloatField("Player Volume", player.Volume);


            foldState = EditorGUILayout.BeginFoldoutHeaderGroup(foldState, "Events", EditorStyles.foldout);
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (foldState)
            {
                GUILayout.BeginHorizontal("box");
                GUILayout.Space(10);

                GUILayout.BeginVertical();
                GUILayout.Space(10);
                
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OnPlay"), true);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OnPlayEnd"), true);
                
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            }


            SerializedProperty container = this.serializedObject.FindProperty("Container");
            if (container != null)
            {
                int count = 0;
                DisplayContainers(this.serializedObject, "Container", ref player.Container, ref count);
            }
            this.serializedObject.ApplyModifiedProperties();
            
        }

        List<bool> foldoutList = new List<bool>();
        List<ContainerType> typeList = new List<ContainerType>();
        List<ContainerType> currentType = new List<ContainerType>();
        List<int> selectionIndex = new List<int>();

        public void DisplayContainers(SerializedObject baseObject, string path, ref ISound parent, ref int count, int level = 0) 
        {
            if (parent == null) 
            {
                parent = ContainerType.SoundClip.Resolve();
            }
            if (count > foldoutList.Count-1) 
            {
                foldoutList.Add(false);
            }
            if (count > typeList.Count - 1)
                typeList.Add(ContainerType.SoundClip);
            if(count > currentType.Count -1)
                currentType.Add(ContainerType.SoundClip);
            if(count > selectionIndex.Count - 1)
                selectionIndex.Add(-1);
            foldoutList[count] = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutList[count], currentType[count].ToString(), EditorStyles.foldout);
            currentType[count] = parent.Resolve();
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (foldoutList[count++])
            {

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("Set ISound Type:");
                EditorGUILayout.BeginHorizontal();
                typeList[count-1] = (ContainerType)EditorGUILayout.EnumPopup(typeList[count-1]);
                if (GUILayout.Button("Set"))
                {
                    currentType[count-1] = typeList[count-1];
                    ISound sound = typeList[count-1].Resolve();
                    if (sound != null)
                    {
                        if (sound.Containers != null && parent.Containers != null)
                            sound.Containers = parent.Containers;
                        parent = sound;
                    }
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(10);


                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20);
                // Display parent
                EditorGUILayout.BeginVertical();

                // get parent field
                SerializedProperty parentProp = baseObject.FindProperty(path);
                EditorGUILayout.PropertyField(parentProp, true);

                // Display container
                if (parent is Container)
                {
                    // Add Remove Index buttons
                    EditorGUILayout.BeginHorizontal();
                    selectionIndex[count - 1] = Mathf.Clamp(EditorGUILayout.IntField(selectionIndex[count - 1]), 0, parent.Containers.Count - 1);
                    int value = selectionIndex[count - 1];
                    if (GUILayout.Button("Add at Index"))
                    {
                        foldoutList.Insert(count, false);
                        typeList.Insert(count, ContainerType.SoundClip);
                        currentType.Insert(count, typeList[count - 1]);
                        if (parent.Containers.Count == 0)
                        {
                            parent.Containers.Add(typeList[count - 1].Resolve());
                        }
                        else
                        {
                            parent.Containers.Insert(value + 1, typeList[count - 1].Resolve());
                        }
                    }
                    if (parent.Containers.Count > 0)
                    {
                        if (GUILayout.Button("Remove at Index"))
                        {
                            parent.Containers.RemoveAt(value);
                            foldoutList.RemoveAt(count + value);
                            typeList.RemoveAt(count + value);
                            currentType.RemoveAt(count + value);
                            return;
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginVertical();
                    for (int i = 0; i < parent.Containers.Count; i++)
                    {
                        ISound child = parent.Containers[i];
                        string builder = path + $"._containers.Array.data[{i}]";
                        SerializedProperty prop = baseObject.FindProperty(builder);
                        if (prop != null)
                        {
                            DisplayContainers(baseObject, builder, ref child, ref count, level++);
                            parent.Containers[i] = child;
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                GUILayout.Space(20);
            }
        }
    }
}