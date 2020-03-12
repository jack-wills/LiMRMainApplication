using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class AudioStream : MonoBehaviour
{
    public AudioSource TargetSrc;
    System.Threading.Thread SocketThread;
    static int BUFFER_SIZE = 20;
    int bufferLoadIndex = 0;
    int bufferReadIndex = 0;
    DateTime lastTime = new DateTime();

    // Data buffer for incoming data.
    byte[] bytes = new byte[8192];
    float[] audioBuffer = new float[2048 * BUFFER_SIZE];

    void Start()
    {
        Application.runInBackground = true;
        startServer();
        //var dummy = AudioClip.Create("dummy", 1, 1, 48000, true, PCMReaderCallback);
        AudioSettings.outputSampleRate = 44100;
        var dummy = AudioClip.Create("dummy", 1, 1, AudioSettings.outputSampleRate, false);

        dummy.SetData(new float[] { 1 }, 0);
        TargetSrc.clip = dummy; //just to let unity play the audiosource
        TargetSrc.loop = true;
        TargetSrc.spatialBlend = 1;
        Debug.Log(dummy.frequency);
        TargetSrc.Play();
    }
    /*void OnAudioRead(float[] data)
    {
        if (bufferLoadIndex != bufferReadIndex)
        {
            // "data" contains the weights of spatial calculations ready by unity
            // multiply "data" with streamed audio data
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] *= audioBuffer[i + (2048 * bufferReadIndex)];
            }
            //Debug.Log("BufferReadIndex = " + bufferReadIndex);
            bufferReadIndex++;
            if (bufferReadIndex == BUFFER_SIZE)
            {
                bufferReadIndex = 0;
            }
        }
        else
        {
            Debug.Log("Buffer Underflow");
        }
    }*/

    /*private void PCMReaderCallback(float[] data)
    {
        if (bufferLoadIndex != bufferReadIndex)
        {
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = audioBuffer[i + (2048 * bufferReadIndex)];
            }
            bufferReadIndex++;
            if (bufferReadIndex == BUFFER_SIZE)
            {
                bufferReadIndex = 0;
            }
        }
        else
        {
            Debug.Log("Buffer Underflow");
        }
    }*/
    void OnAudioFilterRead(float[] data, int channels)
    {
        if (bufferLoadIndex != bufferReadIndex)
        {
            // "data" contains the weights of spatial calculations ready by unity
            // multiply "data" with streamed audio data
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] *= audioBuffer[i + (2048 * bufferReadIndex)];
            }
            //Debug.Log("BufferReadIndex = " + bufferReadIndex);
            bufferReadIndex++;
            if (bufferReadIndex == BUFFER_SIZE)
            {
                bufferReadIndex = 0;
            }
        }
        else
        {
            //Debug.Log("Buffer Underflow");
        }
    }

    void startServer()
    {
        SocketThread = new System.Threading.Thread(networkCode);
        SocketThread.IsBackground = true;
        SocketThread.Start();
    }

    private string getIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            Debug.Log(ip.ToString());
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
            }

        }
        return localIP;
    }
    Socket listener;
    Socket handler;

    void networkCode()
    {


        // host running the application.
        Debug.Log("Ip " + getIPAddress().ToString());
        IPAddress[] ipArray = Dns.GetHostAddresses(getIPAddress());
        IPEndPoint localEndPoint = new IPEndPoint(ipArray[0], 17555);

        while(true)
        {
            try
            {
                // Create a TCP/IP socket.
                listener = new Socket(ipArray[0].AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and 
                // listen for incoming connections.

                listener.Bind(localEndPoint);
                listener.Listen(10);

                handler = listener.Accept();
                Debug.Log("Client Connected");

                // An incoming connection needs to be processed.
                while (true)
                {
                    bytes = new byte[8192];
                    int byteCnt = handler.Receive(bytes);

                    if (byteCnt == 0)
                    {
                        handler.Disconnect(true);
                    }

                    for (int i = 0; i < 2048; ++i)
                    {
                        audioBuffer[i + (2048 * bufferLoadIndex)] = BitConverter.ToSingle(bytes, i*4);
                    }
                    bufferLoadIndex++;
                    if (bufferLoadIndex == BUFFER_SIZE)
                    {
                        bufferLoadIndex = 0;
                    }
                    if (bufferLoadIndex == bufferReadIndex)
                    {
                        Debug.Log("Buffer Overflow");
                    }

                    System.Threading.Thread.Sleep(1);
                }
            }
            catch (SocketException e)
            {
                continue;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return;
            }
        }
       
    }

    void stopServer()
    {
        //stop thread
        if (SocketThread != null)
        {
            SocketThread.Abort();
        }

        if (handler != null && handler.Connected)
        {
            handler.Disconnect(false);
            Debug.Log("Disconnected!");
        }
    }

    void OnDisable()
    {
        stopServer();
    }
}
