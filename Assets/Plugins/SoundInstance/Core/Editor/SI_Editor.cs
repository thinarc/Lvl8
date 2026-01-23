using UnityEngine;
using UnityEditor;
using System.Collections;
using System;


    public class SI_Editor : EditorWindow
    {
        Vector2 scrollPos;

        MusicsStore Storage;

        bool saved = true;

        [MenuItem("Tools/SoundInstance/Game Musics Window")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(SI_Editor));
        }


        [MenuItem("Tools/SoundInstance/Sounds Library")]
        public static void ShowSEWindow()
        {
            EditorWindow.GetWindow(typeof(SI_SoundLibrary));
        }

    [MenuItem("Tools/SoundInstance/Add the music handler to the scene")]
        private static void AddManager()
        {

            GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in gos)
            {
                if (go.name == "[SoundInstanceHandler]")
                {
                    Selection.activeGameObject = go;
                    return;
                }
            }

            GameObject mng = Instantiate(Resources.Load("[SoundInstanceHandler]")) as GameObject;
            mng.name = "[SoundInstanceHandler]";
            Selection.activeGameObject = mng;

        }

        void OnEnable()
        {
            Storage = (MusicsStore)Resources.Load("SIMusicStore");

            if (Storage == null)
            {

                Storage = CreateInstance<MusicsStore>();
                AssetDatabase.CreateAsset(Storage, "Assets/Plugins/SoundInstance/Core/Resources/SIMusicStore.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        void OnGUI()
        {
            titleContent = new GUIContent("Game Musics");
            minSize = new Vector2(250, 300);
            maxSize = new Vector2(500, 2000);

            EditorGUILayout.LabelField("Game Musics ("+Storage.musics.Count+")", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.BeginVertical("box");


            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width - 15), GUILayout.Height(position.height - 100));

            int mscs = Storage.musics.Count;

            for (int i = 0; i < mscs; i++)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.Space(5);

                EditorGUILayout.BeginHorizontal();


                EditorGUILayout.BeginVertical(GUILayout.Width(position.width/1.35f));

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Name: ");
                Storage.musics[i].name = EditorGUILayout.TextField(Storage.musics[i].name);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Song: ");
                Storage.musics[i].Song = (AudioClip)EditorGUILayout.ObjectField(Storage.musics[i].Song, typeof(AudioClip), false);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                GUILayout.Space(7);
                if (GUILayout.Button("-", GUILayout.Width(35), GUILayout.Height(35)))
                {
                    Storage.musics.RemoveAt(i);
                    return;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(5);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(5);
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(3);

            GUILayout.BeginHorizontal(); //
            GUILayout.FlexibleSpace();   //  Centering the button
            GUILayout.BeginHorizontal(); //
            GUILayout.FlexibleSpace();   //
            if (GUILayout.Button("Insert", GUILayout.Width(position.width / 1.5f), GUILayout.Height(25)))
            {
                Storage.musics.Add(new Music());
            }
            GUILayout.FlexibleSpace(); //
            GUILayout.EndHorizontal(); //
            GUILayout.FlexibleSpace(); // Centered
            GUILayout.EndHorizontal(); //

        GUILayout.BeginHorizontal(); //
        GUILayout.FlexibleSpace();   //  Centering the button
        GUILayout.BeginHorizontal(); //
        GUILayout.FlexibleSpace();   //
        if (GUILayout.Button("Save", GUILayout.Width(position.width / 1.5f), GUILayout.Height(25)))
        {
            EditorUtility.SetDirty(Storage);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        GUILayout.FlexibleSpace(); //
        GUILayout.EndHorizontal(); //
        GUILayout.FlexibleSpace(); // Centered
        GUILayout.EndHorizontal(); //

    }


    }

public class SI_SoundLibrary : EditorWindow
{
    Vector2 scrollPos;

    MusicsStore Storage;


    void OnEnable()
    {
        Storage = (MusicsStore)Resources.Load("SIMusicStore");

        if (Storage == null)
        {

            Storage = CreateInstance<MusicsStore>();
            AssetDatabase.CreateAsset(Storage, "Assets/Plugins/SoundInstance/Core/Resources/SIMusicStore.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    void OnGUI()
    {
        titleContent = new GUIContent("Sounds Library");
        minSize = new Vector2(250, 300);
        maxSize = new Vector2(500, 2000);

        EditorGUILayout.LabelField("Sounds Library (" + Storage.soundEffects.Count + ")", EditorStyles.centeredGreyMiniLabel);
        EditorGUILayout.BeginVertical("box");

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width - 15), GUILayout.Height(position.height - 100));

        int mscs = Storage.soundEffects.Count;

        for (int i = 0; i < mscs; i++)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();


            EditorGUILayout.BeginVertical(GUILayout.Width(position.width / 1.35f));

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Name: ");
            Storage.soundEffects[i].name = EditorGUILayout.TextField(Storage.soundEffects[i].name);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Sound: ");
            Storage.soundEffects[i].Sound = (AudioClip)EditorGUILayout.ObjectField(Storage.soundEffects[i].Sound, typeof(AudioClip), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            GUILayout.Space(7);
            if (GUILayout.Button("-", GUILayout.Width(35), GUILayout.Height(35)))
            {
                Storage.soundEffects.RemoveAt(i);
                return;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(3);

        GUILayout.BeginHorizontal(); //
        GUILayout.FlexibleSpace();   //  Centering the button
        GUILayout.BeginHorizontal(); //
        GUILayout.FlexibleSpace();   //
        if (GUILayout.Button("Insert", GUILayout.Width(position.width / 1.5f), GUILayout.Height(25)))
        {
            Storage.soundEffects.Add(new S_Effect());
        }
        GUILayout.FlexibleSpace(); //
        GUILayout.EndHorizontal(); //
        GUILayout.FlexibleSpace(); // Centered
        GUILayout.EndHorizontal(); //


        GUILayout.BeginHorizontal(); //
        GUILayout.FlexibleSpace();   //  Centering the button
        GUILayout.BeginHorizontal(); //
        GUILayout.FlexibleSpace();   //
        if (GUILayout.Button("Save", GUILayout.Width(position.width / 1.5f), GUILayout.Height(25)))
        {
            EditorUtility.SetDirty(Storage);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        GUILayout.FlexibleSpace(); //
        GUILayout.EndHorizontal(); //
        GUILayout.FlexibleSpace(); // Centered
        GUILayout.EndHorizontal(); //
    }


}