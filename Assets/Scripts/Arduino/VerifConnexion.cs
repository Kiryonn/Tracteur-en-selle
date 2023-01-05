using System;
using System.Collections;
using System.IO.Ports;
using System.Linq;
using UnityEngine;

public class VerifConnexion : MonoBehaviour {

    string portcom;
   // static string URL = "connection.php";
    //Baudrate of the port, this can change depending on the baudrate of the arduino
    public int baudrate;
    private SerialPort stream;
    private IEnumerator coroutine;

    private bool canLaunch = false;
    //private bool isDbOk = false;
    private bool isArduinoOk = false;



    // Start is called before the first frame update
    void Start () {
        portcom = /*ReadDataManager.readSerialPort ();*/  "COM3";
        baudrate = /*ReadDataManager.readBaudrate ();*/ 9600;
        // coroutine = DbConnection ();
        TryConnection ();

    }

    // public void Open () {
    //     //Open Serial Port
    //     if (SerialPort.GetPortNames ().Length > 0) {
    //         // port = SerialPort.GetPortNames()[0];

    //         stream = new SerialPort (portcom, baudrate);
    //         stream.ReadTimeout = 5;
    //         try {
    //             stream.Open ();
    //             isArduinoOk = true;
    //         } catch (Exception e) {
    //             Debug.LogError ($"Stream Failed to open {portcom} with baudrate {baudrate} : " + e.ToString ());
    //         }
    //     } else {
    //         Debug.LogWarning ("No COM ports found!");
    //     }

    // }

    public void Open () {
        if (stream != null) {
            stream.DiscardOutBuffer ();
            stream.DiscardInBuffer ();
            stream.Close ();
            stream = null;
        }
        //Open Serial Port
        var portNames = SerialPort.GetPortNames ();
        if (portNames.Length > 0 && portNames.Contains (this.portcom)) {
            Debug.Log ("Open Serial Port " + portcom);
            // port = SerialPort.GetPortNames()[0];

            stream = new SerialPort (this.portcom, baudrate) {
                ReadTimeout = 5
            };

            try {
                stream.Open ();
                Debug.Log ("Arduino OK");
                isArduinoOk = true;

            } catch (Exception e) {
                Debug.LogError ("Stream Failed to open: " + e.ToString ());
            }

        } else {
            Debug.Log ("Arduino pas OK");
            isArduinoOk = false;
        }
    }
	
    // IEnumerator DbConnection () {
        // WWW dbCo = new WWW (configBDD.BddUrl + URL);
        // yield return dbCo;
        // string dbCoString = dbCo.text;

        // Debug.Log (dbCoString.Length);

        // Debug.Log ("DbCo : " + dbCoString);
        // if (dbCoString.Contains ("Success")) {
            // isDbOk = true;
            // Debug.Log ("isDbOk : " + isDbOk);
        // } else {
            // isDbOk = false;
        // }

        // /* 
        // if(String.IsNullOrWhiteSpace(dbCoString)){
            // isDbOk = true;
            // Debug.Log("isDbOk : "+ isDbOk);
        // }*/
    // }

    public bool CheckIfCanLaunch () {

        // if (isArduinoOk && isDbOk) {
		if (isArduinoOk) {
            canLaunch = true;
        }
        Debug.Log ("Can launch : " + canLaunch);
        //return canLaunch;
        return true;
    }

    public void TryConnection () {
        Open ();
        // coroutine = DbConnection ();

        StartCoroutine (coroutine);
        CheckIfCanLaunch ();
    }

}