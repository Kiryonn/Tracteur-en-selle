using System;
using System.IO.Ports;
using UnityEngine;
using System.Globalization;
using System.Collections;
/// <summary>
/// C# based connector to an Arduino Board. Usable in Unity, Attach this to a game object and call the open function first, then either read/write or 
/// the asynchronous function. When finished close the stream with the Close() function.
/// </summary>
public class ArduinoConnector : MonoBehaviour
{
    //Serial Port where the Arduino is connected, this changes depending on what COM value the computer assigns the board
    //Change this to function that crawls COM ports and finds the one that returns PONG (or required data), then use that specific COM Port
    public string port = "COM6";
    //Baudrate of the port, this can change depending on the baudrate of the arduino
    public int baudrate = 9600;
    //Time for Write to Arduino to loop (this is for just testing positive writes to the arduino board)
    public int timesToLoop = 1;
    public int writeTimeout = 500;

    public string str_received = "";
    //SerialPort IO stream
    private SerialPort stream;
    public static ArduinoConnector Instance; // singleton
    public float speed = 0;
    public float reverseSpeed = 0;
    public float direction = 0;
    public string[] strData_received = new string[4];
    public float yaw, pitch, roll;

    
    public void Start()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
        Open();
        /*
        StartCoroutine
        (
            AsynchronousReadFromArduino
            ( (string s) => str_received = s,
            () => Debug.LogError("Error arduino"),
            10000f
            )
        );*/
        //stream.WriteLine("Calibrate");
    }


    public void Update()
    {

        string x = ReadFromArduino();
        //str_received = x;
        //Debug.Log(ReadFromArduino());
        if (!String.IsNullOrEmpty(x))
        {
            //Debug.Log("[Debug] Arduino Stream : "+x);
            string[] ds = x.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            try
            {
                if (ds[2] != "" && ds[3] != "" && ds[4] != "")//make sure data are ready
                {

                    yaw = float.Parse(ds[2], CultureInfo.InvariantCulture);
                    pitch = float.Parse(ds[3], CultureInfo.InvariantCulture);
                    roll = float.Parse(ds[4], CultureInfo.InvariantCulture);
                    //qz = float.Parse(strData_received[3], CultureInfo.InvariantCulture);

                    speed = float.Parse(ds[0], CultureInfo.InvariantCulture);
                    reverseSpeed = float.Parse(ds[1], CultureInfo.InvariantCulture);
                    //transform.rotation = new Quaternion(-qy, -qz, qx, qw);

                }
            }
            catch (Exception)
            {
                //Debug.Log("Error while reading Arduino stream, string was : " + ds[2]);
                //throw;
            }

            /*
			if (!(ds.Length>1 && float.TryParse(ds[0], out float tempDirection) && float.TryParse(ds[1], out float tempSpeed)))
				Debug.LogError("[Error] Wrong information sent from Arduino : " + x +" (variable weren't modified)");
            else if(x[^1] != '\n'){
				direction = tempDirection;
				speed = tempSpeed;
				
			}*/
        }
        //&& x.Contains('?')
    }

    public void Open()
    {
        //Open Serial Port
        if (SerialPort.GetPortNames().Length > 0)
        {
            // port = SerialPort.GetPortNames () [0];
            Debug.Log("Open Serial Port " + port);

            stream = new SerialPort(port, baudrate);
            stream.ReadTimeout = 50;
            //stream.WriteTimeout = writeTimeout;

            try
            {
                stream.Open();

            }
            catch (Exception e)
            {
                Debug.LogError("Stream Failed to open: " + e.ToString());
            }
        }
        else
            Debug.LogWarning("No COM ports found!");

    }

    public string ReadFromArduino(int timeout = 10)
    {
        if (stream != null)
        {
            if (stream.IsOpen)
            {
                stream.ReadTimeout = timeout;
                try
                {
                    return stream.ReadLine();
                }
                catch (TimeoutException)
                {
                    return null;
                }
            }
            else
            {
                Debug.LogWarning("Serial Port isn't open!");
                return null;
            }
        }
        else
        {
            Debug.LogWarning("Serial Port isn't initialized!");
            return null;
        }
    }

    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);
        string dataString = null;
        do
        {
            try
            {
                dataString = stream.ReadLine();
            }
            catch (TimeoutException)
            {
                dataString = null;
            }
            if (dataString != null)
            {
                callback(dataString);
                yield break; // Terminates the Coroutine
            }
            else
                yield return null; // Wait for next frame
            nowTime = DateTime.Now;
            diff = nowTime - initialTime;
        } while (diff.Milliseconds < timeout);
        if (fail != null)
            fail();
        yield return null;
    }

    public void Close()
    {
        if (stream != null && stream.IsOpen)
            stream.Close();
        Debug.Log("Closed the Arduino stream");
    }

    public void OnApplicationQuit()
    {
        if (stream != null)
            if (stream.IsOpen)
                Close();
    }

    public void Calibrate()
    {

        if (!stream.IsOpen)
        {
            stream.Open();
            Debug.Log("Stream was NOT open");
        }

        stream.WriteLine("Calibrate");
    }

}