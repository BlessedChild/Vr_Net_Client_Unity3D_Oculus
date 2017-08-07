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
        public static byte[] ClientToServer(int TotalnumberOfUsers, int UserId, int position_x, int position_y, int position_z)
        {
            byte[] b = new byte[20];  //协议内容的长度

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

            return b; //返回打包好的数据
        }
    }
}