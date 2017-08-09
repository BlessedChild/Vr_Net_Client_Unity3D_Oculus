using System;
/// <summary>
/// 大端->高位在前
/// 小端->低位在前
/// </summary>

/*
0（零）xFF是16进制的255，也就是二进制的 1111，1111
& AND 按位与操作，同时为1时才是1，否则为0.
————位移运算计算机中存的都是数的补码，所以位移运算都是对补码而言的————
<< 左移 右补0
>> 有符号右移 左补符号位，即：如果符号位是1 就左补1，如果符号位是0 就左补0
>>>无符号右移 ，顾名思义，统一左补0
要想知道为什么？我们应该想想，我们的目的是干什么的？开始已经讲了：先取高8位写入，再写入低8位.。
0000,0000,0000,0011      3的二进制原码，假设要写入的short字符对应的unicode码是3。
0000,0000,0000,0000      这是">>>8"的结果                         
1111,1111        然后再 &0XFF                                                                          
0000,0000       最终结果                                                                                    
这就得到了 3的原码0000，0000，0000，0011 的高8位。
0000,0000,0000,0011          >>>0还是源码本身不变
1111,1111        &0XFF                                                                        
0000,0011        最终结果 
这就得到了 3的原码0000，0000，0000，0011 的低8位。
其实，用有符号的右移>>也一样得到高/低8位，因为右移操作不改变数本身，
返回一个新值，就像String。所以&0xFF就像计算机中的一把剪刀，
当‘&’两边的数bit位数相同时不改变数的大小，只是专门截出一个字节的长度。
同理，&0x0F呢？得到4bits
*/
namespace networkre
{
    public class pcak
    {
        //数据类型编码算法
        //将int转为低字节在前，高字节在后的byte数组
        public static byte[] InttoByteArray(int n)
        {
            byte[] b = new byte[4];
            b[0] = (byte)(n & 0xff);
            b[1] = (byte)(n >> 8 & 0xff);
            b[2] = (byte)(n >> 16 & 0xff);
            b[3] = (byte)(n >> 24 & 0xff);
            return b;
        }

        //将short转为低字节在前，高字节在后的byte数组
        public static byte[] ShorttoByteArray(short n)
        {
            byte[] b = new byte[2];
            b[1] = (byte)(n & 0xff);
            b[0] = (byte)(n >> 8 & 0xff);
            return b;
        }

        //将int类型网络字节转化为主机字节算法
        public static string ByteArraytoInt(byte[] b)
        {
            int iOutcome = 0;
            int iOutcome1 = 0;
            int iOutcome2 = 0;
            byte bLoop;
            for (int i = 0; i < 12; i++)
            {
                if (i < 4)
                {
                    bLoop = b[i];
                    iOutcome += (bLoop & 0xff) << (8 * i);
                }
                if (i > 3 && i < 8)
                {
                    bLoop = b[i];
                    iOutcome1 += (bLoop & 0xff) << (8 * i);
                }
                if (i > 7 && i < 12)
                {
                    bLoop = b[i];
                    iOutcome2 += (bLoop & 0xff) << (8 * i);
                }

            }
            string re = iOutcome.ToString() + "/" + iOutcome1.ToString() + "/" + iOutcome2.ToString();
            return re;
        }

        //用原始的办法 一步一步手工解析协议

        public static int ByteToInt_TotalNumberOfUsers(byte[] b)
        {
            int iOutcome = 0;

            byte bLoop;
            for (int i = 0; i < 4; i++)
            {
                if (i < 4)
                {
                    bLoop = b[i];
                    iOutcome += (bLoop & 0xff) << (8 * i);
                }
            }

            return iOutcome;
        }

        //2
        public static float[] ByteToFloar(byte[] b)
        {
            byte bLoop;
            int re1 = 0;
            int re2 = 0;
            int re3 = 0;

            float[] fff = new float[3];
            float f1 = 0;
            float f2 = 0;
            float f3 = 0;

            for (int i = 7; i < 20; i++)
            {
                if (i < 11)
                {
                    bLoop = b[i];
                    re1 += (bLoop & 0xff) << (8 * i);
                    if(i == 10)
                    {
                        f1 = (float)(re1 * 0.00000001);
                        fff[0] = f1;
                    }
                }
                if (i > 10 && i < 15)
                {
                    bLoop = b[i];
                    re2 += (bLoop & 0xff) << (8 * i);
                    if(i == 14)
                    {
                        f2 = (float)(re2 * 0.00000001);
                        fff[1] = f2;
                    }
                }
                if (i > 15 && i < 20)
                {
                    bLoop = b[i];
                    re3 += (bLoop & 0xff) << (8 * i);
                    if(i == 19)
                    {
                        f3 = (float)(re3 * 0.00000001);
                        fff[2] = f3;
                    }
                }
            }
            return fff;
        }

        //将short类型的网络字节转化为主机字节算法
        public static short ByteArraytoShort(byte[] b)
        {
            short iOutcome = 0;
            byte bLoop;
            for (int i = 0; i < 2; i++)
            {
                bLoop = b[i];
                iOutcome += (short)((bLoop & 0xff) << (8 * i));
            }
            return iOutcome;
        }

        /*
        //C#中ANSI字符数组转化为String字符串
        private static String ByteArraytoString(byte[] b)
        {
            String retstr = "";
            try
            {
                retstr = new String(b, string, ,System.Text.Encoding.ASCII);
            }
            catch(Exception e)
            {
            }
            return retstr.Trim();
        }
        //C#中String字符串转换为ANSI字符数组
        private static byte[] StringtoByteArray(String str)
        {
            byte[] retBytes = null;
            try
            {
                retBytes = str.
            }
        }
        */

        //client打包协议的函数，这里把client端逻辑的数据打包成byte数组发送给server端
        public static byte[] ClientToServer(int TotalnumberOfUsers, int UserId, int position_x, int position_y, int position_z, int rotation_x, int rotation_y, int rotation_z,int lefthand_position_x,
            int lefthand_position_y, int lefthand_position_z, int lefthand_rotation_x, int lefthand_rotation_y, int lefthand_rotation_z, int righthand_position_x,int righthand_position_y,
            int righthand_position_z, int righthand_rotation_x, int righthand_rotation_y, int righthand_rotation_z)
        {
            byte[] b = new byte[80];  //协议内容的长度

            b[0] = (byte)(TotalnumberOfUsers & 0xff);
            b[1] = (byte)(TotalnumberOfUsers >> 8 & 0xff);
            b[2] = (byte)(TotalnumberOfUsers >> 16 & 0xff);
            b[3] = (byte)(TotalnumberOfUsers >> 24 & 0xff);

            b[4] = (byte)(UserId >> 32 & 0xff);
            b[5] = (byte)(UserId >> 40 & 0xff);
            b[6] = (byte)(UserId >> 48 & 0xff);
            b[7] = (byte)(UserId >> 56 & 0xff);

            b[8] = (byte)(position_x >> 64 & 0xff);
            b[9] = (byte)(position_x >> 72 & 0xff);
            b[10] = (byte)(position_x >> 80 & 0xff);
            b[11] = (byte)(position_x >> 88 & 0xff);

            b[12] = (byte)(position_y >> 96 & 0xff);
            b[13] = (byte)(position_y >> 104 & 0xff);
            b[14] = (byte)(position_y >> 112 & 0xff);
            b[15] = (byte)(position_y >> 120 & 0xff);

            b[16] = (byte)(position_z >> 128 & 0xff);
            b[17] = (byte)(position_z >> 136 & 0xff);
            b[18] = (byte)(position_z >> 144 & 0xff);
            b[19] = (byte)(position_z >> 152 & 0xff);

            b[20] = (byte)(rotation_x >> 160 & 0xff);
            b[21] = (byte)(rotation_x >> 168 & 0xff);
            b[22] = (byte)(rotation_x >> 176 & 0xff);
            b[23] = (byte)(rotation_x >> 184 & 0xff);

            b[24] = (byte)(rotation_y >> 192 & 0xff);
            b[25] = (byte)(rotation_y >> 200 & 0xff);
            b[26] = (byte)(rotation_y >> 208 & 0xff);
            b[27] = (byte)(rotation_y >> 216 & 0xff);

            b[28] = (byte)(rotation_z >> 224 & 0xff);
            b[29] = (byte)(rotation_z >> 232 & 0xff);
            b[30] = (byte)(rotation_z >> 240 & 0xff);
            b[31] = (byte)(rotation_z >> 248 & 0xff);

            b[32] = (byte)(lefthand_position_x >> 256 & 0xff);
            b[33] = (byte)(lefthand_position_x >> 264 & 0xff);
            b[34] = (byte)(lefthand_position_x >> 272 & 0xff);
            b[35] = (byte)(lefthand_position_x >> 280 & 0xff);

            b[36] = (byte)(lefthand_position_y >> 288 & 0xff);
            b[37] = (byte)(lefthand_position_y >> 296 & 0xff);
            b[38] = (byte)(lefthand_position_y >> 304 & 0xff);
            b[39] = (byte)(lefthand_position_y >> 312 & 0xff);

            b[40] = (byte)(lefthand_position_z >> 320 & 0xff);
            b[41] = (byte)(lefthand_position_z >> 328 & 0xff);
            b[42] = (byte)(lefthand_position_z >> 336 & 0xff);
            b[43] = (byte)(lefthand_position_z >> 344 & 0xff);

            b[44] = (byte)(lefthand_rotation_x >> 352 & 0xff);
            b[45] = (byte)(lefthand_rotation_x >> 360 & 0xff);
            b[46] = (byte)(lefthand_rotation_x >> 368 & 0xff);
            b[47] = (byte)(lefthand_rotation_x >> 376 & 0xff);

            b[48] = (byte)(lefthand_rotation_y >> 384 & 0xff);
            b[49] = (byte)(lefthand_rotation_y >> 392 & 0xff);
            b[50] = (byte)(lefthand_rotation_y >> 400 & 0xff);
            b[51] = (byte)(lefthand_rotation_y >> 408 & 0xff);

            b[52] = (byte)(lefthand_rotation_z >> 416 & 0xff);
            b[53] = (byte)(lefthand_rotation_z >> 424 & 0xff);
            b[54] = (byte)(lefthand_rotation_z >> 432 & 0xff);
            b[55] = (byte)(lefthand_rotation_z >> 440 & 0xff);

            b[56] = (byte)(righthand_position_x >> 448 & 0xff);
            b[57] = (byte)(righthand_position_x >> 456 & 0xff);
            b[58] = (byte)(righthand_position_x >> 464 & 0xff);
            b[59] = (byte)(righthand_position_x >> 472 & 0xff);

            b[60] = (byte)(righthand_position_y >> 480 & 0xff);
            b[61] = (byte)(righthand_position_y >> 488 & 0xff);
            b[62] = (byte)(righthand_position_y >> 496 & 0xff);
            b[63] = (byte)(righthand_position_y >> 504 & 0xff);

            b[64] = (byte)(righthand_position_z >> 512 & 0xff);
            b[65] = (byte)(righthand_position_z >> 520 & 0xff);
            b[66] = (byte)(righthand_position_z >> 528 & 0xff);
            b[67] = (byte)(righthand_position_z >> 536 & 0xff);

            b[68] = (byte)(righthand_rotation_x >> 544 & 0xff);
            b[69] = (byte)(righthand_rotation_x >> 552 & 0xff);
            b[70] = (byte)(righthand_rotation_x >> 560 & 0xff);
            b[71] = (byte)(righthand_rotation_x >> 568 & 0xff);

            b[72] = (byte)(righthand_rotation_y >> 576 & 0xff);
            b[73] = (byte)(righthand_rotation_y >> 584 & 0xff);
            b[74] = (byte)(righthand_rotation_y >> 592 & 0xff);
            b[75] = (byte)(righthand_rotation_y >> 600 & 0xff);

            b[76] = (byte)(righthand_rotation_z >> 608 & 0xff);
            b[77] = (byte)(righthand_rotation_z >> 616 & 0xff);
            b[78] = (byte)(righthand_rotation_z >> 624 & 0xff);
            b[79] = (byte)(righthand_rotation_z >> 632 & 0xff);

            return b; //返回打包好的数据
        }

        public static int[] ServerToClient(byte[] b)
        {
            int[] iOutcome = new int[40];
            byte bLoop;

            for (int i = 0; i < 160; i++)
            {
                if (i < 4)
                {
                    bLoop = b[i];
                    iOutcome[0] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 3 && i < 8)
                {
                    bLoop = b[i];
                    iOutcome[1] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 7 && i < 12)
                {
                    bLoop = b[i];
                    iOutcome[2] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 11 && i < 16)
                {
                    bLoop = b[i];
                    iOutcome[3] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 15 && i < 20)
                {
                    bLoop = b[i];
                    iOutcome[4] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 19 && i < 24)
                {
                    bLoop = b[i];
                    iOutcome[5] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 23 && i < 28)
                {
                    bLoop = b[i];
                    iOutcome[6] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 27 && i < 32)
                {
                    bLoop = b[i];
                    iOutcome[7] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 31 && i < 36)
                {
                    bLoop = b[i];
                    iOutcome[8] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 35 && i < 40)
                {
                    bLoop = b[i];
                    iOutcome[9] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 39 && i < 44)
                {
                    bLoop = b[i];
                    iOutcome[10] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 43 && i < 48)
                {
                    bLoop = b[i];
                    iOutcome[11] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 47 && i < 52)
                {
                    bLoop = b[i];
                    iOutcome[12] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 51 && i < 56)
                {
                    bLoop = b[i];
                    iOutcome[13] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 55 && i < 60)
                {
                    bLoop = b[i];
                    iOutcome[14] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 59 && i < 64)
                {
                    bLoop = b[i];
                    iOutcome[15] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 63 && i < 68)
                {
                    bLoop = b[i];
                    iOutcome[16] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 67 && i < 72)
                {
                    bLoop = b[i];
                    iOutcome[17] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 71 && i < 76)
                {
                    bLoop = b[i];
                    iOutcome[18] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 75 && i < 80)
                {
                    bLoop = b[i];
                    iOutcome[19] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 79 && i < 84)
                {
                    bLoop = b[i];
                    iOutcome[20] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 83 && i < 88)
                {
                    bLoop = b[i];
                    iOutcome[21] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 87 && i < 92)
                {
                    bLoop = b[i];
                    iOutcome[22] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 91 && i < 96)
                {
                    bLoop = b[i];
                    iOutcome[23] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 95 && i < 100)
                {
                    bLoop = b[i];
                    iOutcome[24] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 99 && i < 104)
                {
                    bLoop = b[i];
                    iOutcome[25] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 103 && i < 108)
                {
                    bLoop = b[i];
                    iOutcome[26] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 107 && i < 112)
                {
                    bLoop = b[i];
                    iOutcome[27] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 111 && i < 116)
                {
                    bLoop = b[i];
                    iOutcome[28] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 115 && i < 120)
                {
                    bLoop = b[i];
                    iOutcome[29] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 119 && i < 124)
                {
                    bLoop = b[i];
                    iOutcome[30] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 123 && i < 128)
                {
                    bLoop = b[i];
                    iOutcome[31] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 127 && i < 132)
                {
                    bLoop = b[i];
                    iOutcome[32] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 131 && i < 136)
                {
                    bLoop = b[i];
                    iOutcome[33] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 135 && i < 140)
                {
                    bLoop = b[i];
                    iOutcome[34] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 139 && i < 144)
                {
                    bLoop = b[i];
                    iOutcome[35] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 143 && i < 148)
                {
                    bLoop = b[i];
                    iOutcome[36] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 147 && i < 152)
                {
                    bLoop = b[i];
                    iOutcome[37] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 151 && i < 156)
                {
                    bLoop = b[i];
                    iOutcome[38] += (bLoop & 0xff) << (8 * i);
                }
                if (i > 155 && i < 160)
                {
                    bLoop = b[i];
                    iOutcome[39] += (bLoop & 0xff) << (8 * i);
                }

            }

            return iOutcome;
        }
    }
}