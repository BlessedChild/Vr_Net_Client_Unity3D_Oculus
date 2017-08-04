using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class login : MonoBehaviour
{
    public GameObject cube111;

    static byte[] RECEIVE_BUFFER = new byte[1024];
    static byte[] SEND_BUFFER = new byte[1024];

    public Transform character_;
    private float x_;
    private float y_;
    private float z_;

    public static bool isclientconnected = false;
    public static bool issend = false;
    public static int count = 0;

    private void Update()
    {
        updateposition();


    }

    public void updateposition()
    {
        if(isclientconnected)
        {
            x_ = character_.position.x;
            y_ = character_.position.y;
            z_ = character_.position.z;

            int _x = (int)(x_ * 100000000);
            int _y = (int)(y_ * 100000000);
            int _z = (int)(z_ * 100000000);
            //Debug.Log(_x + _y + _z);

            if (!issend)
            {
                //string recint1 = networkre.pcak.ByteArraytoInt(RECEIVE_BUFFER);
                //Debug.Log(recint1);
                float[] recint = networkre.pcak.ByteToFloar(RECEIVE_BUFFER);
                Debug.Log(recint[0].ToString() + "/" + recint[1].ToString() + "/" + recint[2].ToString()  + "/");

                Instantiate(cube111, new Vector3(recint[0], recint[1], recint[2]), Quaternion.identity);

                RECEIVE_BUFFER = networkre.pcak.ClientToServer(_x, _y, _z);

                issend = true;
            }
        }
    }


    Thread connectServer;

    //static byte[] recbuffer = new byte[4];

    private void Start()
    {
        connectServer = new Thread(initGame);
    }

    public void SingIn()
    {
        connectServer.Start();
    }

    public void CreatID()
    {

    }

    public void GetbackIDorPassword()
    {

    }

    private void initGame()
    {
        string ip = "192.168.15.156";
        IPAddress ipAddress = IPAddress.Parse(ip);
        Socket a = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        a.Connect(ip, 6666);


        if (a.Connected)
        {
            isclientconnected = true;
            Thread newsend = new Thread(login.sendmsg);
            newsend.Start(a);

            
            Thread newaccept = new Thread(login.acceptmsg);
            newaccept.Start(a);
            



            /*
            int count = a.Receive(recbuffer, recbuffer.Length, 0);
            if(count > 0)
            {
                Debug.Log(networkre.pcak.ByteArraytoInt(recbuffer));
            }
            */
        }
        else
        {
            isclientconnected = false;
            Debug.Log("connect faile");
            a.Close();
        }
    }

    public static void sendmsg(object s)
    {
        while (true)
        {
            if (issend)
            {
                //buffer = networkre.pcak.InttoByteArray(timei);
                Socket b = (Socket)s;
                b.Send(RECEIVE_BUFFER, RECEIVE_BUFFER.Length, SocketFlags.None);

                //Debug.Log("FromClient: send to server: ... ");
                Thread.Sleep(20);
                issend = false;
            }
        }
    }
    
    
    public static void acceptmsg(object s)
    {
        while (true)
        {          
            if (!issend)
            {
                Socket a = (Socket)s;
                count = a.Receive(RECEIVE_BUFFER, 12, 0);
                /*
                if (count > 0)
                {
                    string recint = networkre.pcak.ByteArraytoInt(buffer);
                    Debug.Log(recint);
                }
                issend = true;
                */
            }         
        }
    }
    


}