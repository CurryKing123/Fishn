using System.Collections;
using System.Collections.Generic;
using UnityEditor.Scripting.Python;
using UnityEngine;

public class PythonInference : MonoBehaviour
{
    public void RunInference()
    {
        PythonRunner.RunFile(Application.dataPath + "/Classification/classification_rectangle_v7-1/inference.py");
        Debug.Log("Running Inference");
    }
}
