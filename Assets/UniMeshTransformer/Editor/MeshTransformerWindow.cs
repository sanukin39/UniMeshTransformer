#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UMT
{
    public class MeshTransformerWindow : EditorWindow
    {
        private const string Title = "Mesh Transformer";

        private Mesh _mesh;
        private Vector3 _position;
        private Vector3 _rotation;
        private Vector3 _scale = Vector3.one;
        private string _exportMeshName;
        private DefaultAsset _exportDirectory;
        private Editor _gameObjectEditor; 

        [MenuItem("Window/UniMeshTransfer")]
        private static void Open()
        {
            GetWindow<MeshTransformerWindow>(Title).Show();
        }

        void OnGUI()
        {
            _mesh = (Mesh) EditorGUILayout.ObjectField("Target Mesh", _mesh, typeof(Mesh), false);
            _position = EditorGUILayout.Vector3Field("Position", _position);
            _rotation = EditorGUILayout.Vector3Field("Rotation", _rotation);
            _scale = EditorGUILayout.Vector3Field("Scale", _scale);

            GUILayout.Space(10);

            _exportDirectory =
                (DefaultAsset) EditorGUILayout.ObjectField("Directory", _exportDirectory, typeof(DefaultAsset), true);
            _exportMeshName = EditorGUILayout.TextField("Export Mesh Name", _exportMeshName);

            if (_mesh == null)
            {
                return;
            }

            if(_exportDirectory != null && !string.IsNullOrEmpty(_exportMeshName))
            {
                if (GUILayout.Button("Export"))
                {
                    var newMesh = MeshTransformer.GenerateMesh(_mesh, _position, _rotation, _scale);
                    Export(newMesh);
                }
            }

            if (GUILayout.Button("Refresh Preview"))
            {
                var newMesh = MeshTransformer.GenerateMesh(_mesh, _position, _rotation, _scale);
                _gameObjectEditor = Editor.CreateEditor(newMesh);    
            }

            if (_gameObjectEditor != null)
            {
                _gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(200, 200), new GUIStyle());
            }
        }

        void Export(Mesh mesh)
        {
            var exportDirectoryPath = AssetDatabase.GetAssetPath(_exportDirectory);
            if (Path.GetExtension(_exportMeshName) != ".asset")
            {
                _exportMeshName += ".asset";
            }

            var exportPath = Path.Combine(exportDirectoryPath, _exportMeshName);
            AssetDatabase.CreateAsset(mesh, exportPath);
        }
    }
}
#endif