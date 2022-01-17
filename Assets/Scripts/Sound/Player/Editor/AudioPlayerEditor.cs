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
            else 
            {
                // Add container of choice
                // player.Container;
            }

            this.serializedObject.ApplyModifiedProperties();
            //DrawDefaultInspector();
        }

        List<bool> foldoutList = new List<bool>();
        List<ContainerType> typeList = new List<ContainerType>();

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
            foldoutList[count] = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutList[count], "ISound", EditorStyles.foldout);
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (foldoutList[count++])
            {

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("Set ISound Type:");
                EditorGUILayout.BeginHorizontal();
                typeList[count-1] = (ContainerType)EditorGUILayout.EnumPopup(typeList[count-1]);
                if (GUILayout.Button("Set"))
                {
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


                if (parent.Containers != null)
                {
                    // Add Remove Index buttons
                    EditorGUILayout.BeginHorizontal();
                    int value = Mathf.Clamp(EditorGUILayout.IntField(parent.Containers.Count - 1), 0, parent.Containers.Count - 1);
                    if (GUILayout.Button("Add at Index"))
                    {
                        foldoutList.Insert(count, false);
                        typeList.Insert(count, ContainerType.SoundClip);
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
                            if (prop.isArray)
                                prop = prop.GetArrayElementAtIndex(i);
                            if (child is Container)
                            {
                                DisplayContainers(baseObject, builder, ref child, ref count, level++);
                                parent.Containers[i] = child;
                                
                            }
                            else
                            {
                                EditorGUILayout.PropertyField(prop, true);
                            }
                        }
                        //if (children != null)
                        //{
                        //    EditorGUILayout.PropertyField(children);
                        //    // List
                        //}
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