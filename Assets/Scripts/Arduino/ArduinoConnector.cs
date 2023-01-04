using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

/// <summary>
/// C# based connector to an Arduino Board. Usable in Unity, Attach this to a game object and call the open function first, then either read/write or 
/// the asynchronous function. When finished close the stream with the Close() function.
/// </summary>
public class ArduinoConnector : MonoBehaviour {
    //Serial Port where the Arduino is connected, this changes depending on what COM value the computer assigns the board
    //Change this to function that crawls COM ports and finds the one that returns PONG (or required data), then use that specific COM Port
    public string port = "COM3";
    //Baudrate of the port, this can change depending on the baudrate of the arduino
    public int baudrate = 9600;
    //Time for Write to Arduino to loop (this is for just testing positive writes to the arduino board)
    public int timesToLoop = 1;

    //SerialPort IO stream
    private SerialPort stream;

    //public ArduinoData datafluxArduino;

    /// <summary>
    /// Opens a Serial Port, timesout after 50 milliseconds
    /// </summary>
    /// Opens the Serial Port Connection, timesout after 50 milliseconds. 
    //This function iterates over the Serial Ports and gets the open ones. 
    //This enables multiple Arduino Boards to be connected to the PC and communicated with.
    public void Open () {
        //Open Serial Port

        if (SerialPort.GetPortNames ().Length > 0) {
            Debug.Log ("Open Serial Port " + SerialPort.GetPortNames () [0]);
            port = SerialPort.GetPortNames () [0];

            stream = new SerialPort (SerialPort.GetPortNames () [0], baudrate);
            stream.ReadTimeout = 5;
            stream.WriteTimeout = 5;
            try {
                stream.Open ();
            } catch (Exception e) {
                Debug.LogError ("Stream Failed to open: " + e.ToString ());
            }
        } else {
            Debug.LogWarning ("No COM ports found!");
        }

    }

    /// <summary>
    /// writes to arduino boards, then flushes the stream
    /// </summary>
    /// <param name="message">The Message to be sent</param>
    /// Writes to the Arduino Board, then flushes the IO stream. 
    /// Writes the contents of the message to the arduino board.
    // public void WriteToArduino(string message)
    // {
    //     if (stream != null)
    //     {
    //         if (stream.IsOpen)
    //         {
    //             //Send a Read Request
    //             Debug.Log("Write to Arduino: " + message);
    //             stream.WriteLine(message + System.Environment.NewLine);
    //             stream.BaseStream.Flush();
    //         }
    //         else
    //         {
    //             Debug.LogWarning("Can't Write; Stream isn't open!");
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogWarning("Serial Port isn't initialized");
    //     }
    // }

    /// <summary>
    /// Reads output from the Arduino boards and returns it as a string that can then be parsedin Unity. 
    /// </summary>
    /// <param name="timeout">If there is nothing to be read from the Arduino Board then timeout the function call.</param>
    /// <returns>Data from the Arduino board as a string format</returns>
    /// Reads output from the Arduino Board, returns it as a string, which can then be parsed an manipulated as needed.
    /// If nothing is read by the script then the connection timesout after the 100ms.
    public string ReadFromArduino (int timeout = 10) {
        if (stream != null) {
            if (stream.IsOpen) {
                stream.ReadTimeout = timeout;
                try {
                    Debug.Log (stream.ReadLine ());
                    return stream.ReadLine ();

                } catch (TimeoutException) {
                    return null;
                }
            } else {
                Debug.LogWarning ("Serial Port isn't open!");
                return null;
            }
        } else {
            Debug.LogWarning ("Serial Port isn't initialized!");
            return null;
        }
    }

    /// <summary>
    /// Asynchronous Read from Arduino board function, 
    /// By reading from the arduino board using this Enumerable coroutine, then we can avoid 
    /// lots of system lag
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="fail"></param>
    /// <param name="timeout"></param>
    /// <returns>Returns an enumerable set of data returned from the Arduino board</returns>
    /// This function asynchronously reads from the arduino board. 
    /// By utilising this rather than the previously implemented ReadFromArduino() function 
    /// we can reduce large amounts of system lag, if we require rapid calls.
    // public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    // {
    //     DateTime initialTime = DateTime.Now;
    //     DateTime nowTime;
    //     TimeSpan diff = default(TimeSpan);

    //     string dataString = null;

    //     do
    //     {
    //         //Single Read Attempt
    //         try
    //         {

    //             if (stream != null)
    //                 if (stream.IsOpen)
    //                     dataString = stream.ReadLine();

    //         }
    //         catch (TimeoutException)
    //         {
    //             dataString = null;

    //         }
    //         if (dataString != null)
    //         {
    //             callback(dataString);
    //             yield return null;
    //         }
    //         else
    //         {
    //             yield return new WaitForSeconds(0.05f);

    //         }

    //         nowTime = DateTime.Now;
    //         diff = nowTime - initialTime;
    //     }
    //     while (diff.Milliseconds < timeout);

    //     if (fail != null)
    //     {
    //         fail();
    //     }
    //     yield return null;

    // }

    /// <summary>
    /// Closes the IO Stream
    /// </summary>
    public void Close () {
        if (stream != null)
            if (stream.IsOpen)
                stream.Close ();
        Debug.Log ("Closed the stream");
    }

    /// <summary>
    /// Standard Unity start function
    /// </summary>
    public void Start () {
        Open ();

    }

    public string verif="";
    public void Update () {
        
        string x = ReadFromArduino();
        if (x != null) 
        {
            verif = x;
            Debug.Log(verif);
        }

        

    }

    public void OnApplicationQuit () {
        if (stream != null)
            if (stream.IsOpen)
                Close ();
    }
}