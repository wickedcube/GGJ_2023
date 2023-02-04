using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Waves
{
    public class WavePointTagger : MonoBehaviour
    {
        // private string FilePath = $"/Scripts/Waves/TargetMarker.cs";
        // public TargetMarker targetMarker;
        //
        // public string NewTagToAdd;
        //
        // [ContextMenu("New Tag!!!")]
        // public void Test()
        // {
        //     if (string.IsNullOrEmpty(NewTagToAdd))
        //     {
        //         Debug.LogError($"Tag is empty");
        //         return;
        //     }
        //
        //     if (Enum.GetNames(typeof(TargetMarker)).ToList().Contains(NewTagToAdd))
        //     {
        //         Debug.LogError($"this {NewTagToAdd} is already added");
        //         return;
        //     }
        //
        //     var tempString = "public enum TargetMarker {";
        //     var namesList = Enum.GetNames(typeof(TargetMarker)).ToList();
        //     for (var i = 0; i < namesList.Count; i++)
        //     {
        //         var s = namesList[i];
        //         
        //         if (i == namesList.Count - 1)
        //         {
        //             tempString += $"{s},\n";
        //             tempString += $"{NewTagToAdd}\n";
        //         }
        //         else
        //         {
        //             tempString += $"{s},\n";
        //         }
        //     }
        //     tempString += "}";
        //
        //     File.WriteAllText($"{Application.dataPath}{FilePath}", tempString);
        //     AssetDatabase.Refresh();
        // }
    }
}