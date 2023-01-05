using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

public static class ReadDataManager {
    /*
    public static string configGameplay = "configGameplay.json";
    static string scenarioXML = "Scenarios.xml";
    static string obstaclesXML = "Obstacles.xml";
    static JSONObject dataAsJson;
    public static string readSerialPort () {
        try {
            dataAsJson = new JSONObject (File.ReadAllText (Path.Combine (Application.streamingAssetsPath, configGameplay)));
        } catch {
            Debug.LogError ($"Fichier {configGameplay} introuvable");
        }
        return dataAsJson.GetField ("com").str;
    }

    public static string readBddUrl () {
        try {
            dataAsJson = new JSONObject (File.ReadAllText (Path.Combine (Application.streamingAssetsPath, configGameplay)));
        } catch {
            Debug.LogError ($"Fichier {configGameplay} introuvable");
        }
        return dataAsJson.GetField ("urlBdd").str;
    }

    public static int readBaudrate () {
        try {
            dataAsJson = new JSONObject (File.ReadAllText (Path.Combine (Application.streamingAssetsPath, configGameplay)));
        } catch {
            Debug.LogError ($"Fichier {configGameplay} introuvable");
        }
        return (int) dataAsJson.GetField ("baudrate").n;
    }

    public static string getScenarioPath () {
        return Path.Combine (Application.streamingAssetsPath, scenarioXML);
    }
    public static string getObstaclesPath () {
        return Path.Combine (Application.streamingAssetsPath, obstaclesXML);
    }

    public static XDocument readScenarios () {
        return readXmlFile (getScenarioPath ());

    }

    public static XDocument readObstacles () {
        return readXmlFile (getObstaclesPath ());
    }

    private static XDocument readXmlFile (string filename) {
        return XDocument.Load (filename);
    }*/

}