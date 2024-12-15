using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;
using System.IO;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine.PlayerLoop;

public class SplineImporter : EditorWindow
{
    private string filePath = "";
    private string containerName = "Imported Splines Container";
        
    [MenuItem("Tools/Import Bezier Curves")]
    public static void ShowWindow()
    {
        GetWindow<SplineImporter>("Import Bezier Curves");
    }

    void OnGUI()
    {
        GUILayout.Label("Select JSON File and Set Container Name", EditorStyles.boldLabel);

        if (GUILayout.Button("Select Json File"))
        {
            filePath = EditorUtility.OpenFilePanel("Select JSON File", "", "json");
        }
        
        containerName = EditorGUILayout.TextField("Container Name", containerName);
        if (GUILayout.Button("Import Curves"))
        {
            ImportCurves();
        }
    }

    void ImportCurves()
    {
        string jsonText = File.ReadAllText(filePath);
        JArray curves = JArray.Parse(jsonText);  // 解析 JSON 数据

        // 创建一个统一的 GameObject 用于存放所有曲线
        GameObject splineContainerObject = new GameObject("Imported Splines Container");
        var masterSplineContainer = splineContainerObject.AddComponent<SplineContainer>();
        
        foreach (JObject curve in curves)
        {
            string name = curve["name"].ToString();
            JArray knots = (JArray)curve["knots"];

            var spline = new Spline();

            foreach (JObject knot in knots)
            {
                Vector3 position = ParseVector3(knot["position"]);
                Vector3 handleLeft = ParseVector3(knot["handle_left"]);
                Vector3 handleRight = ParseVector3(knot["handle_right"]);

                BezierKnot bezierKnot = new BezierKnot(position, handleLeft, handleRight);
                spline.Add(bezierKnot);
            }

            // 将每个 Spline 添加到同一个 SplineContainer 中
            masterSplineContainer.AddSpline(spline);
        }
    }

    Vector3 ParseVector3(JToken vector)
    {
        float x = (float)vector[0];
        float y = (float)vector[1];
        float z = (float)vector[2];
        return new Vector3(x, y, z);
    }
}
