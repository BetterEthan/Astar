using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Drawing;
namespace Astar
{
    class Rplidar
    {
        
        private SerialPort rp;
        public Coor coor;
        public Point startPoint;
        public Rplidar(Point _startPoint)
        {
            startPoint = _startPoint;
            rp = new SerialPort();
            rp.BaudRate = 115200;
            rp.PortName = "COM4";
            rp.Open();
            coor = new Coor();
        }
        public void StartMotor()
        {
            byte[] buff = new byte[6];
            buff[0] = 0xA5;
            buff[1] = 0xF0;
            buff[2] = 0x02;
            buff[3] = 0x94;
            buff[4] = 0x02;
            buff[5] = 0xc1;
            rp.Write(buff, 0, 6);
        }

        public void StartScan()
        {
            byte[] buff = new byte[2];
            buff[0] = 0xA5;
            buff[1] = 0x20;
            rp.Write(buff, 0, 2);
        }
        public void StartExpressScan()
        {
            byte[] buff = new byte[9];
            buff[0] = 0xA5;
            buff[1] = 0x82;
            buff[2] = 0x05;
            buff[3] = 0x00;
            buff[4] = 0x00;
            buff[5] = 0x00;
            buff[6] = 0x00;
            buff[7] = 0x00;
            buff[8] = 0x22;
            rp.Write(buff, 0, buff.Length);
        }

        public void GetInfo()
        {
            byte[] buff = new byte[2];
            buff[0] = 0xA5;
            buff[1] = 0x50;
            rp.Write(buff, 0, 2);
        }

        public void StopMotor()
        {
            byte[] buff = new byte[6];
            buff[0] = 0xA5;
            buff[1] = 0xF0;
            buff[2] = 0x02;
            buff[3] = 0x00;
            buff[4] = 0x00;
            buff[5] = 0x57; 
            rp.Write(buff, 0, 6);
        }

        public void StopScan()
        {
            byte[] buff = new byte[2];
            buff[0] = 0xA5;
            buff[1] = 0x25;
            rp.Write(buff, 0, 2);
        }
        public struct Coor
        {
            public float x;
            public float y;
            public float angle;
        }




        public struct rplidar_measurement_data
        {
            public float angle_q6;
            public float distance_q2;
        };
        rplidar_measurement_data tempData = new rplidar_measurement_data();
        List<rplidar_measurement_data> measurementData = new List<rplidar_measurement_data>(360);
        public rplidar_measurement_data[] measureData_arr = new rplidar_measurement_data[360];
        public Point[] obsPoint_arr = new Point[120];
        struct cabin
        {
            public float distance1;
            public float distance2;
            public float delta_xita1;
            public float delta_xita2;
        }
        struct express_response_message
        {
            public float start_angle_q6;
            public cabin[] cabin_arr;
        }
        express_response_message temp_message = new express_response_message();
        express_response_message cur_message = new express_response_message();
        public bool updateFLag = false;
        public void Rplidar_DataReceived()
        {
            Point curPoint = new Point();
            Point obsPoint = new Point();
            List<byte> rplidar_buffer = new List<byte>(1024);
            byte messageType = 0;//0代表起始应答报文，1代表数据应答报文

            UInt16 angle_q6 = 0;
            UInt16 distance_q2 = 0;
            byte[] angle_q6_buf = new byte[2];
            byte[] distance_q2_buf = new byte[2];
            int dataSize = 0;
            sbyte delta_xita_q3 = 0;
            float angleDiff = 0.0f;
            int index = 0;
            byte cmdType = 0;//0x04代表设备信息获取命令，0x81代表开始扫描采样命令
            int majorModel = 0;
            byte subModel = 0;
            byte firmwareVer_minor = 0;
            byte firmwareVer_major = 0;
            byte hardware = 0;
            byte[] serialNumber = new byte[32];
            byte quality = 0;
            temp_message.cabin_arr = new cabin[16];
            cur_message.cabin_arr = new cabin[16];
            while (true)
            {
                int length = rp.BytesToRead;
                if (length > 0)
                {
                    byte[] readBuf = new byte[length];
                    rp.Read(readBuf, 0, length);
                    rplidar_buffer.AddRange(readBuf);
                }

                if (rplidar_buffer.Count > 0)
                {
                    if (messageType == 0)
                    {
                        while (rplidar_buffer.Count > 1)
                        {
                            //检验帧头
                            if (rplidar_buffer[0] == 0xA5 && rplidar_buffer[1] == 0x5A)
                            {
                                //检查数据长度
                                if (rplidar_buffer.Count >= 7)
                                {
                                    messageType = 1;
                                    dataSize = rplidar_buffer[2];
                                    cmdType = rplidar_buffer[6];
                                    rplidar_buffer.RemoveRange(0, 7);
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                rplidar_buffer.RemoveAt(0);
                            }
                        }
                    }
                    else
                    {
                        if (cmdType == 0x04)
                        {
                            if (rplidar_buffer.Count >= 20)
                            {
                                subModel = (byte)(rplidar_buffer[0] & 0x0f);
                                majorModel = (rplidar_buffer[0] & 0xf0) >> 4;
                                Console.WriteLine("Ver:{0}.{1}", majorModel, subModel);
                                firmwareVer_minor = rplidar_buffer[1];
                                firmwareVer_major = rplidar_buffer[2];
                                Console.WriteLine("firmware ver:{0}.{1}", firmwareVer_major, firmwareVer_minor);
                                hardware = rplidar_buffer[3];
                                Console.WriteLine("hardware ver:{0}", hardware);
                                Console.Write("S/N:");
                                for (int i = 0; i < 16; i++)
                                {
                                    serialNumber[i * 2] = (byte)(rplidar_buffer[4 + i] & 0x0f);
                                    serialNumber[i * 2 + 1] = (byte)((rplidar_buffer[4 + i] & 0xf0) >> 4);
                                    Console.Write("{0:X}{1:X}", serialNumber[i * 2 + 1], serialNumber[i * 2]);
                                    messageType = 0;
                                }
                                Console.Write("\r\n");
                            }
                            else
                            {
                                break;
                            }
                        }
                        else if (cmdType == 0x81)
                        {
                            while (rplidar_buffer.Count > 1)
                            {
                                if ((rplidar_buffer[0] & 0x01) + ((rplidar_buffer[0] & 0x02) >> 1) == 1)
                                {
                                    if (rplidar_buffer.Count >= 5)
                                    {
                                        quality = (byte)(rplidar_buffer[0] & 0xfc);
                                        if (quality != 0)
                                        {
                                            distance_q2_buf[0] = rplidar_buffer[3];
                                            distance_q2_buf[1] = rplidar_buffer[4];
                                            distance_q2 = BitConverter.ToUInt16(distance_q2_buf, 0);
                                            if (distance_q2 != 0)
                                            {
                                                angle_q6_buf[0] = rplidar_buffer[1];
                                                angle_q6_buf[1] = rplidar_buffer[2];
                                                angle_q6 = (UInt16)(BitConverter.ToUInt16(angle_q6_buf, 0) >> 1);

                                                tempData.angle_q6 = angle_q6 / 64.0f / 360 * 2 * (float)Math.PI;
                                                tempData.distance_q2 = distance_q2 / 4.0f;

                                                curPoint.X = (int)Math.Round(coor.x / 0.05, 0) + startPoint.X;
                                                curPoint.Y = (int)Math.Round(coor.y / 0.05, 0) + startPoint.Y;
                                                float xita = coor.angle + tempData.angle_q6 - (float)Math.PI;
                                                if (tempData.angle_q6 < (float)Math.PI / 3 || tempData.angle_q6 > (float)Math.PI / 3 * 5)
                                                {
                                                    int X = curPoint.X - (int)(tempData.distance_q2 * Math.Cos(xita) / 50);
                                                    int Y = curPoint.Y - (int)(tempData.distance_q2 * Math.Sin(xita) / 50);
                                                    obsPoint.X = X;
                                                    obsPoint.Y = Y;
                                                    obsPoint_arr[index] = obsPoint;
                                                    index++;
                                                    if (index == 120)
                                                    {
                                                        index = 0;
                                                        updateFLag = true;
                                                    }
                                                }
                                               
                                            }
                                        }
                                        rplidar_buffer.RemoveRange(0, 5);

                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    rplidar_buffer.RemoveAt(0);
                                }
                            }
                        }
                        else if (cmdType == 0x82)
                        {
                            while (rplidar_buffer.Count > 1)
                            {
                                //检验帧头
                                if ((rplidar_buffer[0] >> 4 == 0xA) && (rplidar_buffer[1] >> 4) == 0x5)
                                {
                                    if (rplidar_buffer.Count >= dataSize)
                                    {
                                        //获取S标志位
                                        //sFlag = (byte)(rplidar_buffer[3] >> 7);
                                        //第一次
                                        if (temp_message.start_angle_q6 == 0)
                                        {
                                            angle_q6_buf[0] = rplidar_buffer[2];
                                            angle_q6_buf[1] = (byte)(rplidar_buffer[3] & 0x7f);
                                            temp_message.start_angle_q6 = BitConverter.ToUInt16(angle_q6_buf, 0) / 64.0f;/// 360 * 2 * (float)Math.PI;
                                            for (int i = 0; i < 16; i++)
                                            {
                                                delta_xita_q3 |= (sbyte)((rplidar_buffer[i * 5 + 4] & 0x01) << 4);
                                                delta_xita_q3 |= (sbyte)((rplidar_buffer[i * 5 + 4] & 0x02) << 6);//符号位
                                                delta_xita_q3 |= (sbyte)(rplidar_buffer[i * 5 + 4 + 4] & 0x0f);
                                                temp_message.cabin_arr[i].delta_xita1 = delta_xita_q3 / 8.0f;

                                                delta_xita_q3 |= (sbyte)((rplidar_buffer[i * 5 + 4 + 2] & 0x01) << 4);
                                                delta_xita_q3 |= (sbyte)((rplidar_buffer[i * 5 + 4] & 0x02) << 6);//符号位
                                                delta_xita_q3 |= (sbyte)(rplidar_buffer[i * 5 + 4 + 4] & 0xf0);
                                                temp_message.cabin_arr[i].delta_xita2 = delta_xita_q3 / 8.0f;

                                                distance_q2_buf[0] = rplidar_buffer[i * 5 + 4];
                                                distance_q2_buf[1] = rplidar_buffer[i * 5 + 4 + 1];
                                                distance_q2 = (UInt16)(BitConverter.ToUInt16(distance_q2_buf, 0) >> 2);
                                                temp_message.cabin_arr[i].distance1 = distance_q2;

                                                distance_q2_buf[0] = rplidar_buffer[i * 5 + 4 + 2];
                                                distance_q2_buf[1] = rplidar_buffer[i * 5 + 4 + 3];
                                                distance_q2 = (UInt16)(BitConverter.ToUInt16(distance_q2_buf, 0) >> 2);
                                                temp_message.cabin_arr[i].distance2 = distance_q2;
                                            }
                                        }
                                        else
                                        {
                                            angle_q6_buf[0] = rplidar_buffer[2];
                                            angle_q6_buf[1] = (byte)(rplidar_buffer[3] & 0x7F);
                                            cur_message.start_angle_q6 = BitConverter.ToUInt16(angle_q6_buf, 0) / 64.0f;/// 360 * 2 * (float)Math.PI;

                                            angleDiff = cur_message.start_angle_q6 - temp_message.start_angle_q6;
                                            if (angleDiff < 0)
                                                angleDiff += 360;

                                            for (int i = 0; i < 16; i++)
                                            {
                                                delta_xita_q3 |= (sbyte)((rplidar_buffer[i * 5 + 4] & 0x01) << 4);
                                                delta_xita_q3 |= (sbyte)((rplidar_buffer[i * 5 + 4] & 0x02) << 6);//符号位
                                                delta_xita_q3 |= (sbyte)(rplidar_buffer[i * 5 + 4 + 4] & 0x0f);
                                                cur_message.cabin_arr[i].delta_xita1 = delta_xita_q3 / 8.0f;
                                                tempData.angle_q6 = (temp_message.start_angle_q6 + angleDiff / 32.0f * (i * 2 + 1) - temp_message.cabin_arr[i].delta_xita1 - 10) /**/;
                                                if (tempData.angle_q6 > 360)
                                                    tempData.angle_q6 -= 360;
                                                tempData.angle_q6 = tempData.angle_q6 / 360 * 2 * (float)Math.PI;
                                                tempData.distance_q2 = temp_message.cabin_arr[i].distance1;
                                                if (tempData.distance_q2 != 0)
                                                {
                                                    //laserData_sw.WriteLine("theta:{0}\tdistance:{1}", tempData.angle_q6, tempData.distance_q2);
                                                    if (measurementData.Count < 360)
                                                        measurementData.Add(tempData);
                                                    else
                                                    {
                                                        measurementData.CopyTo(measureData_arr);
                                                        measurementData.RemoveRange(0, measurementData.Count);
                                                        updateFLag = true;
                                                    }
                                                }

                                                delta_xita_q3 |= (sbyte)((rplidar_buffer[i * 5 + 4 + 2] & 0x01) << 4);
                                                delta_xita_q3 |= (sbyte)((rplidar_buffer[i * 5 + 4] & 0x02) << 6);//符号位
                                                delta_xita_q3 |= (sbyte)(rplidar_buffer[i * 5 + 4 + 4] & 0xf0);
                                                cur_message.cabin_arr[i].delta_xita2 = delta_xita_q3 / 8.0f;

                                                tempData.angle_q6 = (temp_message.start_angle_q6 + angleDiff / 32.0f * (i * 2 + 2) - temp_message.cabin_arr[i].delta_xita2 - 10)/*/ 360 * 2 * (float)Math.PI*/;
                                                tempData.distance_q2 = temp_message.cabin_arr[i].distance2;
                                                if (tempData.angle_q6 > 360)
                                                    tempData.angle_q6 -= 360;
                                                tempData.angle_q6 = tempData.angle_q6 / 360 * 2 * (float)Math.PI;
                                                if (tempData.distance_q2 != 0)
                                                {
                                                    //laserData_sw.WriteLine("theta:{0}\tdistance:{1}", tempData.angle_q6, tempData.distance_q2);
                                                    if (measurementData.Count < 360)
                                                        measurementData.Add(tempData);
                                                    else
                                                    {
                                                        measurementData.CopyTo(measureData_arr);
                                                        measurementData.RemoveRange(0, measurementData.Count);
                                                        updateFLag = true;
                                                    }
                                                }


                                                distance_q2_buf[0] = rplidar_buffer[i * 5 + 4];
                                                distance_q2_buf[1] = rplidar_buffer[i * 5 + 4 + 1];
                                                distance_q2 = (UInt16)(BitConverter.ToUInt16(distance_q2_buf, 0) >> 2);
                                                cur_message.cabin_arr[i].distance1 = distance_q2;

                                                distance_q2_buf[0] = rplidar_buffer[i * 5 + 4 + 2];
                                                distance_q2_buf[1] = rplidar_buffer[i * 5 + 4 + 3];
                                                distance_q2 = (UInt16)(BitConverter.ToUInt16(distance_q2_buf, 0) >> 2);
                                                cur_message.cabin_arr[i].distance2 = distance_q2;

                                            }
                                            temp_message = cur_message;
                                        }
                                        rplidar_buffer.RemoveRange(0, dataSize);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    rplidar_buffer.RemoveAt(0);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
