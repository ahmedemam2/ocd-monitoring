using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Security.Policy;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        TcpListener listener;
        TcpClient client;
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        System.Windows.Forms.Timer T = new System.Windows.Forms.Timer();
        Bitmap off;
        public int num1, num2, flagct1 = 0,flagct2=0;
        public int move1x=0,move1y=0,move2x=100,move2y=0,move3x=200,move3y=0;
        Bitmap ele2 = new Bitmap("pen.bmp");
        Bitmap ele3 = new Bitmap("notebook.bmp");
        Bitmap ele4 = new Bitmap("cup.bmp");

        bool flagtick = false, flagcol1 = true, flagcol2 = false, flagcol3 = false, flagwin = false;
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Paint += new PaintEventHandler(Form1_Paint);
            T.Tick += new EventHandler(Timer_Tick);
        }
        void Timer_Tick(object sender, EventArgs e)
        {

            NetworkStream serverStream = clientSocket.GetStream();
            byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
            serverStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);


            string returndata = System.Text.Encoding.UTF8.GetString(bytesFrom);
            string line = returndata;
            string[] split = line.Split(',');
            if (Int16.Parse(split[0].Remove(0,1))==-1)
            {
                num1 = ClientSize.Height;
                num2 = ClientSize.Width;
                flagtick = true;
                T.Stop();
            }
            //if (Int16.Parse(split[0].Remove(0, 1)) < 288)
            //{
            //    flagcol1 = true;
            //}
            string listtt = split[0];
            string listt2 = split[1];
            
            //MessageBox.Show(returndata);
           


            listtt = listtt.Remove(0, 1);


            string[] split2 = listt2.Split(']');
            listt2 = split2[0];

            num1 = Int16.Parse(listtt);
            num2 = Int16.Parse(listt2);
            if (flagcol1 == true)
            {
                move1x = num1;
                move1y = num2;
                if(move1x>=100 && move1y>=300&&move1x<250 &&move1y<450)
                {
                    move1x = 100;
                    move1y = 300;
                    flagcol1 = false;
                    flagcol2 = true;
                    num1 = 0;
                    num2 = 0;
                }
            }
            if (flagcol2 == true)
            {
                flagct1++;
                if (flagct1 > 30)
                {
                    move2x = num1;
                    move2y = num2;
                    if (move2x >= 100 && move2y >= 200 && move2x < 350 && move2y < 450)
                    {
                        move2x = 200;
                        move2y = 300;
                        flagcol2 = false;
                        flagcol3 = true;
                        num1 = 0;
                        num2 = 0;
                    }
                }
            }
            if (flagcol3 == true)
            {
                flagct2++;
                if (flagct2 > 30)
                {
                    move3x = num1;
                    move3y = num2;
                    if (move3x >= 200 && move3y >= 300 && move3x < 450 && move3y < 450)
                    {
                        move3x = 300;
                        move3y = 300;
                        flagcol3 = false;
                        num1 = 0;
                        num2 = 0;
                        flagwin = true;
                    }
                }
            }
            if (flagtick==true)
            {
                num1 = ClientSize.Height;
                num2 = ClientSize.Width;
            }
            DrawDubble(this.CreateGraphics());
            //int num2 = listt2 - '0';
            //richTextBox1.Text += num1.ToString() + '\n';
        }
        void Form1_Paint(object sender, PaintEventArgs e)
        {
            ele2 = new Bitmap(ele2, new Size(100, 100));
            ele3 = new Bitmap(ele3, new Size(100, 100));
            ele4 = new Bitmap(ele4, new Size(100, 100));
            DrawDubble(e.Graphics);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

            clientSocket.Connect("127.0.0.1", 65434);

            T.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        
        }
        void DrawScene(Graphics g)
        {
            if (flagwin == false)
            {
                g.Clear(Color.Black);
            }
            else { g.Clear(Color.White); }
            Pen Pn = new Pen(Color.White, 2);
            g.DrawEllipse(Pn, num1,num2,20,20);
            g.DrawImage(ele2, move1x, move1y);
            g.DrawImage(ele3, move2x, move2y);
            g.DrawImage(ele4, move3x, move3y);
            g.DrawRectangle(Pn, 100, 300, 100, 100);
            g.DrawRectangle(Pn, 200, 300, 100, 100);
            g.DrawRectangle(Pn, 300, 300, 100, 100);

        }

        void DrawDubble(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}