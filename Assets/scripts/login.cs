using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEditor;

public class login : MonoBehaviour
{
    public GameObject cube111;
    public GameObject otherlefthand;
    public GameObject otherrighthand;

    private int countCube = 0;
    public int UserId = 1;
    public int TotalnumberOfUsers = 1;
    Thread connectServer;
    public static Socket a;

    static byte[] RECEIVE_BUFFER = new byte[1024];
    static byte[] SEND_BUFFER = new byte[1024];

    public Transform character_;
    public Transform mylefthand;
    public Transform myrighthand;
    private float x_;
    private float y_;
    private float z_;
    private float x_r;
    private float y_r;
    private float z_r;
    private float lefthand_position_x;
    private float lefthand_position_y;
    private float lefthand_position_z;
    private float lefthand_rotation_x;
    private float lefthand_rotation_y;
    private float lefthand_rotation_z;
    private float righthand_position_x;
    private float righthand_position_y;
    private float righthand_position_z;
    private float righthand_rotation_x;
    private float righthand_rotation_y;
    private float righthand_rotation_z;


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
            x_r = character_.rotation.x;
            y_r = character_.rotation.y;
            z_r = character_.rotation.z;
            lefthand_position_x = mylefthand.position.x;
            lefthand_position_y = mylefthand.position.y;
            lefthand_position_z = mylefthand.position.z;
            lefthand_rotation_x = mylefthand.rotation.x;
            lefthand_rotation_y = mylefthand.rotation.y;
            lefthand_rotation_z = mylefthand.rotation.z;
            righthand_position_x = myrighthand.position.x;
            righthand_position_y = myrighthand.position.y;
            righthand_position_z = myrighthand.position.z;
            righthand_rotation_x = myrighthand.rotation.x;
            righthand_rotation_y = myrighthand.rotation.y;
            righthand_rotation_z = myrighthand.rotation.z;

            int _x = (int)(x_ * 100000000);
            int _y = (int)(y_ * 100000000);
            int _z = (int)(z_ * 100000000);
            int r_x = (int)(x_r * 100000000);
            int r_y = (int)(y_r * 100000000);
            int r_z = (int)(z_r * 100000000);
            int lh_p_x = (int)(lefthand_position_x * 100000000);
            int lh_p_y = (int)(lefthand_position_y * 100000000);
            int lh_p_z = (int)(lefthand_position_z * 100000000);
            int lh_r_x = (int)(lefthand_rotation_x * 100000000);
            int lh_r_y = (int)(lefthand_rotation_y * 100000000);
            int lh_r_z = (int)(lefthand_rotation_z * 100000000);
            int rh_p_x = (int)(righthand_position_x * 100000000);
            int rh_p_y = (int)(righthand_position_y * 100000000);
            int rh_p_z = (int)(righthand_position_z * 100000000);
            int rh_r_x = (int)(righthand_rotation_x * 100000000);
            int rh_r_y = (int)(righthand_rotation_y * 100000000);
            int rh_r_z = (int)(righthand_rotation_z * 100000000);

            //Debug.Log(_x + _y + _z);

            if (!issend)
            {
                //string recint1 = networkre.pcak.ByteArraytoInt(RECEIVE_BUFFER);
                //Debug.Log(recint1);
                int[] recint = networkre.pcak.ServerToClient(RECEIVE_BUFFER);


                if(recint[1].ToString() == UserId.ToString())
                {
                    /*
                    if (countCube == 0)
                    {
                        Instantiate(cube111, new Vector3((float)(recint[7] * 0.00000001), (float)(recint[8] * 0.00000001), (float)(recint[9] * 0.00000001)), Quaternion.identity);
                        countCube++;
                    }
                    */
                    cube111.transform.position = new Vector3((float)(recint[22] * 0.00000001), (float)(recint[23] * 0.00000001), (float)(recint[24] * 0.00000001));
                    cube111.transform.forward = new Vector3((float)(recint[22] * 0.00000001), (float)(recint[23] * 0.00000001), (float)(recint[24] * 0.00000001));
                    cube111.transform.rotation = new Quaternion((float)(recint[25] * 0.00000001), (float)(recint[26] * 0.00000001), (float)(recint[27] * 0.00000001), Quaternion.identity.w);
                    otherlefthand.transform.position = new Vector3((float)(recint[28] * 0.00000001), (float)(recint[29] * 0.00000001), (float)(recint[30] * 0.00000001));
                    otherlefthand.transform.forward = new Vector3((float)(recint[28] * 0.00000001), (float)(recint[29] * 0.00000001), (float)(recint[30] * 0.00000001));
                    otherlefthand.transform.rotation = new Quaternion((float)(recint[31] * 0.00000001), (float)(recint[32] * 0.00000001), (float)(recint[33] * 0.00000001), Quaternion.identity.w);
                    otherrighthand.transform.position = new Vector3((float)(recint[34] * 0.00000001), (float)(recint[35] * 0.00000001), (float)(recint[36] * 0.00000001));
                    otherrighthand.transform.forward = new Vector3((float)(recint[34] * 0.00000001), (float)(recint[35] * 0.00000001), (float)(recint[36] * 0.00000001));
                    otherrighthand.transform.rotation = new Quaternion((float)(recint[37] * 0.00000001), (float)(recint[38] * 0.00000001), (float)(recint[39] * 0.00000001), Quaternion.identity.w);

                    //Debug.Log(recint[5].ToString() + "/" + recint[6].ToString() + "/" + ((float)(recint[7] * 0.00000001)).ToString() + "/" + ((float)(recint[8] * 0.00000001)).ToString() + "/" + ((float)(recint[9] * 0.00000001)).ToString() + "/");
                }
                else if(recint[21].ToString() == UserId.ToString())
                {
                    /*
                    if (countCube == 0)
                    {
                        Instantiate(cube111, new Vector3((float)(recint[3] * 0.00000001), (float)(recint[4] * 0.00000001), (float)(recint[5] * 0.00000001)), Quaternion.identity);
                        countCube++;
                    }
                    */
                    cube111.transform.position = new Vector3((float)(recint[2] * 0.00000001), (float)(recint[3] * 0.00000001), (float)(recint[4] * 0.00000001));
                    cube111.transform.forward = new Vector3((float)(recint[2] * 0.00000001), (float)(recint[3] * 0.00000001), (float)(recint[4] * 0.00000001));
                    cube111.transform.rotation = new Quaternion((float)(recint[5] * 0.00000001), (float)(recint[6] * 0.00000001), (float)(recint[7] * 0.00000001), Quaternion.identity.w);
                    //Debug.Log(recint[0].ToString() + "/" + recint[1].ToString() + "/" + ((float)(recint[2] * 0.00000001)).ToString() + "/" + ((float)(recint[3] * 0.00000001)).ToString() + "/" + ((float)(recint[4] * 0.00000001)).ToString() + "/");
                    otherlefthand.transform.position = new Vector3((float)(recint[8] * 0.00000001), (float)(recint[9] * 0.00000001), (float)(recint[10] * 0.00000001));
                    otherlefthand.transform.forward = new Vector3((float)(recint[8] * 0.00000001), (float)(recint[9] * 0.00000001), (float)(recint[10] * 0.00000001));
                    otherlefthand.transform.rotation = new Quaternion((float)(recint[11] * 0.00000001), (float)(recint[12] * 0.00000001), (float)(recint[13] * 0.00000001), Quaternion.identity.w);
                    otherrighthand.transform.position = new Vector3((float)(recint[14] * 0.00000001), (float)(recint[15] * 0.00000001), (float)(recint[16] * 0.00000001));
                    otherrighthand.transform.forward = new Vector3((float)(recint[14] * 0.00000001), (float)(recint[15] * 0.00000001), (float)(recint[16] * 0.00000001));
                    otherrighthand.transform.rotation = new Quaternion((float)(recint[17] * 0.00000001), (float)(recint[18] * 0.00000001), (float)(recint[19] * 0.00000001), Quaternion.identity.w);


                }

                SEND_BUFFER = networkre.pcak.ClientToServer(TotalnumberOfUsers, UserId, _x, _y, _z, r_x, r_y, r_z,lh_p_x,lh_p_y,lh_p_z,lh_r_x,lh_r_y,lh_r_z,rh_p_x,rh_p_y,rh_p_z,rh_r_x,rh_r_y,rh_r_z);

                issend = true;
            }
        }
    }




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
        string ip = "192.168.15.155";
        IPAddress ipAddress = IPAddress.Parse(ip);
        a = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
                Socket b = (Socket)s;
                b.Send(SEND_BUFFER, SEND_BUFFER.Length, SocketFlags.None);
                Thread.Sleep(20);
                //buffer = networkre.pcak.InttoByteArray(timei);
                //Debug.Log("FromClient: send to server: ... ");
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
                count = a.Receive(RECEIVE_BUFFER, 160, 0);
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

    private void OnApplicationQuit()
    {
        connectServer.Abort();
        a.Close();
    }

}