// DFVolume - Distance field volume generator for Unity
// https://github.com/keijiro/DFVolume

using UnityEditor;

namespace DFVolume
{
    [CustomEditor(typeof(VolumeData))]
    internal class VolumeDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // There is nothing to show in the inspector.
        }
    }
}