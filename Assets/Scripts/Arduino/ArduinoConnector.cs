using System;
using System.IO.Ports;
using UnityEngine;

/// <summary>
/// C# based connector to an Arduino Board. Usable in Unity, Attach this to a game object and call the open function first, then either read/write or 
/// the asynchronous function. When finished close the stream with the Close() function.
/// </summary>
public class ArduinoConnector : MonoBehaviour {
	//Serial Port where the Arduino is connected, this changes depending on what COM value the computer assigns the board
	//Change this to function that crawls COM ports and finds the one that returns PONG (or required data), then use that specific COM Port
	public string port = "COM6";
	//Baudrate of the port, this can change depending on the baudrate of the arduino
	public int baudrate = 9600;
	//Time for Write to Arduino to loop (this is for just testing positive writes to the arduino board)
	public int timesToLoop = 1;

	//SerialPort IO stream
	private SerialPort stream;
	public static ArduinoConnector Instance; // singleton
	public int speed = 0;
	public int direction = 0;

	public void Open () {
		//Open Serial Port
		if (SerialPort.GetPortNames().Length > 0) {
			// port = SerialPort.GetPortNames () [0];
			Debug.Log("Open Serial Port " + port);

			stream = new SerialPort(port, baudrate);
			stream.ReadTimeout = 5;
			stream.WriteTimeout = 5;
			try {
				stream.Open();
			} catch (Exception e) {
				Debug.LogError("Stream Failed to open: " + e.ToString ());
			}
		} else
			Debug.LogWarning("No COM ports found!");

	}

	public string ReadFromArduino (int timeout = 10) {
		if (stream != null) {
			if (stream.IsOpen) {
				stream.ReadTimeout = timeout;
				try {
					return stream.ReadLine();
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

	public void Close () {
		if (stream != null && stream.IsOpen)
			stream.Close();
		Debug.Log("Closed the Arduino stream");
	}
	public void Start () {
		if (Instance != null)
			Destroy(this);
		else
			Instance = this;
		Open();
	}

	public void Update () {
		string x = ReadFromArduino();
		if (x != null) {
			string[] ds = x.Split(',');
			direction = int.Parse(ds[0]);
			speed = int.Parse(ds[1]);
		}
	}

	public void OnApplicationQuit () {
		if (stream != null)
			if (stream.IsOpen)
				Close ();
	}
}