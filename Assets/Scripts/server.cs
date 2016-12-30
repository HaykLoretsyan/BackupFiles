using UnityEngine;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Net;
using System.IO;

public class Server : MonoBehaviour {

    public int port = 6321;

    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    private TcpListener server;
    private bool serverStarted;

    public void init()
    {
        DontDestroyOnLoad(gameObject);
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();
            serverStarted = true;
        }
        catch(Exception e)
        {
            Debug.Log("Socket Error: " + e.Message);
        }
    }

    private void Update()
    {
        if(!serverStarted)
        {
            return;
        }

        foreach(ServerClient c in clients)
        {
            if(!IsConnected(c.tcp))
            {
                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            }
            else
            {
                NetworkStream s = c.tcp.GetStream();
                if(s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();

                    if (data != null)
                        OnIncomingData(c, data);
                }
            }
        }

        for(int i = 0; i < disconnectList.Count - 1; i++)
        {
            clients.Remove(disconnectList[i]);
        }

        disconnectList.Clear();
    }

    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;

        ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));
        clients.Add(sc);

        Debug.Log("Somebody has connected");

        StartListening();
    }

    private bool IsConnected(TcpClient c)
    {
        try
        {
            if(c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    private void OnIncomingData(ServerClient c, string data)
    {
        Debug.Log(c.clientname + " : " + data);
    }
}

public class ServerClient
{
    public string clientname;
    public TcpClient tcp;

    public ServerClient(TcpClient tcp)
    {
        this.tcp = tcp;
    }
}
