/*
	TUIO C# Demo - part of the reacTIVision project
	Copyright (c) 2005-2016 Martin Kaltenbrunner <martin@tuio.org>

	This program is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using TUIO;

	public class TuioDemo : Form , TuioListener
	{
		private TuioClient client;
		private Dictionary<long,TuioObject> objectList;
		private Dictionary<long,TuioCursor> cursorList;
		private Dictionary<long,TuioBlob> blobList;

		public static int width, height;
		private int window_width =  640;
		private int window_height = 480;
		private int window_left = 0;
		private int window_top = 0;
		private int screen_width = Screen.PrimaryScreen.Bounds.Width;
		private int screen_height = Screen.PrimaryScreen.Bounds.Height;

		private bool fullscreen;
		private bool verbose;
		public bool flag1 = false, flag2 = false, flag3 = false, flag4 = false, stop1 = false, stop2 = false, stop3 = false, stop4 = false;
		public int xf, yf;
		Font font = new Font("Arial", 10.0f);
		//Bitmap frame = new Bitmap("untitled.bmp");
		Bitmap ele1 = new Bitmap("cup.bmp");
		Bitmap ele2 =new Bitmap("pen.bmp");
		Bitmap ele3 = new Bitmap("notebook.bmp");
		SolidBrush fntBrush = new SolidBrush(Color.White);
		SolidBrush bgrBrush = new SolidBrush(Color.FromArgb(0,0,64));
		SolidBrush curBrush = new SolidBrush(Color.FromArgb(192, 0, 192));
		SolidBrush objBrush = new SolidBrush(Color.FromArgb(64, 0, 0));
		SolidBrush blbBrush = new SolidBrush(Color.FromArgb(64, 64, 64));
		Pen curPen = new Pen(new SolidBrush(Color.Blue), 1);

		public TuioDemo(int port) {
		
			verbose = false;
			fullscreen = false;
			width = window_width;
			height = window_height;

			this.ClientSize = new System.Drawing.Size(width, height);
			this.Name = "TuioDemo";
			this.Text = "TuioDemo";
			
			this.Closing+=new CancelEventHandler(Form_Closing);
			this.KeyDown+=new KeyEventHandler(Form_KeyDown);

			this.SetStyle( ControlStyles.AllPaintingInWmPaint |
							ControlStyles.UserPaint |
							ControlStyles.DoubleBuffer, true);

			objectList = new Dictionary<long,TuioObject>(128);
			cursorList = new Dictionary<long,TuioCursor>(128);
			blobList   = new Dictionary<long,TuioBlob>(128);
			
			client = new TuioClient(port);
			client.addTuioListener(this);

			client.connect();
		}

		private void Form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {

 			if ( e.KeyData == Keys.F1) {
	 			if (fullscreen == false) {

					width = screen_width;
					height = screen_height;

					window_left = this.Left;
					window_top = this.Top;

					this.FormBorderStyle = FormBorderStyle.None;
		 			this.Left = 0;
		 			this.Top = 0;
		 			this.Width = screen_width;
		 			this.Height = screen_height;

		 			fullscreen = true;
	 			} else {

					width = window_width;
					height = window_height;

		 			this.FormBorderStyle = FormBorderStyle.Sizable;
		 			this.Left = window_left;
		 			this.Top = window_top;
		 			this.Width = window_width;
		 			this.Height = window_height;

		 			fullscreen = false;
	 			}
 			} else if ( e.KeyData == Keys.Escape) {
				this.Close();

 			} else if ( e.KeyData == Keys.V ) {
 				verbose=!verbose;
 			}

 		}

		private void Form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			client.removeTuioListener(this);

			client.disconnect();
			System.Environment.Exit(0);
		}

		public void addTuioObject(TuioObject o) {
			lock(objectList) {
				objectList.Add(o.SessionID,o);
			} if (verbose) Console.WriteLine("add obj "+o.SymbolID+" ("+o.SessionID+") "+o.X+" "+o.Y+" "+o.Angle);
		}

		public void updateTuioObject(TuioObject o) {

			if (verbose) Console.WriteLine("set obj "+o.SymbolID+" "+o.SessionID+" "+o.X+" "+o.Y+" "+o.Angle+" "+o.MotionSpeed+" "+o.RotationSpeed+" "+o.MotionAccel+" "+o.RotationAccel);
		}

		public void removeTuioObject(TuioObject o) {
			lock(objectList) {
				objectList.Remove(o.SessionID);
			}
			if (verbose) Console.WriteLine("del obj "+o.SymbolID+" ("+o.SessionID+")");
		}

		public void addTuioCursor(TuioCursor c) {
			lock(cursorList) {
				cursorList.Add(c.SessionID,c);
			}
			if (verbose) Console.WriteLine("add cur "+c.CursorID + " ("+c.SessionID+") "+c.X+" "+c.Y);
		}

		public void updateTuioCursor(TuioCursor c) {
			if (verbose) Console.WriteLine("set cur "+c.CursorID + " ("+c.SessionID+") "+c.X+" "+c.Y+" "+c.MotionSpeed+" "+c.MotionAccel);
		}

		public void removeTuioCursor(TuioCursor c) {
			lock(cursorList) {
				cursorList.Remove(c.SessionID);
			}
			if (verbose) Console.WriteLine("del cur "+c.CursorID + " ("+c.SessionID+")");
 		}

		public void addTuioBlob(TuioBlob b) {
			lock(blobList) {
				blobList.Add(b.SessionID,b);
			}
			if (verbose) Console.WriteLine("add blb "+b.BlobID + " ("+b.SessionID+") "+b.X+" "+b.Y+" "+b.Angle+" "+b.Width+" "+b.Height+" "+b.Area);
		}

		public void updateTuioBlob(TuioBlob b) {
		
			if (verbose) Console.WriteLine("set blb "+b.BlobID + " ("+b.SessionID+") "+b.X+" "+b.Y+" "+b.Angle+" "+b.Width+" "+b.Height+" "+b.Area+" "+b.MotionSpeed+" "+b.RotationSpeed+" "+b.MotionAccel+" "+b.RotationAccel);
		}

		public void removeTuioBlob(TuioBlob b) {
			lock(blobList) {
				blobList.Remove(b.SessionID);
			}
			if (verbose) Console.WriteLine("del blb "+b.BlobID + " ("+b.SessionID+")");
		}

		public void refresh(TuioTime frameTime) {
			Invalidate();
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			// Getting the graphics object
			Graphics g = pevent.Graphics;
			Graphics g2 = pevent.Graphics;
			g.FillRectangle(bgrBrush, new Rectangle(0,0,width,height));
			ele1 = new Bitmap(ele1, new Size(50, 50));
			ele2 = new Bitmap(ele2, new Size(50, 50));
			ele3 = new Bitmap(ele3, new Size(50, 50));
			//frame.MakeTransparent();
			//frame = new Bitmap(frame,new Size(120,120));				
			//frame.MakeTransparent();
			//g.DrawImage(frame, ClientSize.Width / 2-frame.Width/2, ClientSize.Height / 2-frame.Height/2);
			xf = ClientSize.Width / 2 ;
			yf= ClientSize.Height / 2 ;

			

		// draw the cursor path
		if (cursorList.Count > 0) {
 			 lock(cursorList) {
			 foreach (TuioCursor tcur in cursorList.Values) {
					List<TuioPoint> path = tcur.Path;
					TuioPoint current_point = path[0];

					for (int i = 0; i < path.Count; i++) {
						TuioPoint next_point = path[i];
						g.DrawLine(curPen, current_point.getScreenX(width), current_point.getScreenY(height), next_point.getScreenX(width), next_point.getScreenY(height));
						current_point = next_point;
					}
					g.FillEllipse(curBrush, current_point.getScreenX(width) - height / 100, current_point.getScreenY(height) - height / 100, height / 50, height / 50);
					g.DrawString(tcur.CursorID + "", font, fntBrush, new PointF(tcur.getScreenX(width) - 10, tcur.getScreenY(height) - 10));
				}
			}
		 }

			// draw the objects
			if (objectList.Count > 0) {
			lock (objectList)
			{
				foreach (TuioObject tobj in objectList.Values)
				{
					int ox = tobj.getScreenX(width);
					int oy = tobj.getScreenY(height);
					int size = height / 10;
					//g.DrawImage(frame, ox - size / 2, oy - size / 2);
					if (tobj.SymbolID == 0 && stop1 == false)
					{
						flag1 = true;
						double x = tobj.Angle;
						int xx = Convert.ToInt32(x);
						g2 .TranslateTransform(ox, oy);
						//g2.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
						g2.TranslateTransform(-ox, -oy);
						g2.DrawImage(ele1, ox - size / 2, oy - size / 2);
						if (ox - size / 2 > 100 && ox - size / 2 < 250 && oy - size / 2 > 300 && oy - size / 2 < 450)
						{
							stop1 = true;
						}
					}
					if (tobj.SymbolID == 1 && stop2 == false)
					{
						flag2 = true;
						double x2 = tobj.Angle;
						int xx2 = Convert.ToInt32(x2);
						g.TranslateTransform(ox, oy);
						//g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-ox, -oy);
						g.DrawImage(ele2, ox - size / 2, oy - size / 2);
                        if (ox - size / 2 > 200 && ox - size / 2 < 350 && oy - size / 2 > 300 && oy - size / 2 < 450)
                        {
                            stop2 = true;
						}
					}
					if (tobj.SymbolID == 2 && stop3 == false)
					{
						flag3 = true;
						double x3 = tobj.Angle;
						int xx3 = Convert.ToInt32(x3);
						g.TranslateTransform(ox, oy);
						//g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-ox, -oy);
						g.DrawImage(ele3, ox - size / 2, oy - size / 2);
                        if (ox - size / 2 > 300 && ox - size / 2 < 450 && oy - size / 2 > 300 && oy - size / 2 < 450)
                        {
                            stop3 = true;
						}
					}
					//g.TranslateTransform(ox, oy);
					//g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
					//g.TranslateTransform(-ox, -oy);

					//g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));

					//g.TranslateTransform(ox, oy);
					//g.RotateTransform(-1 * (float)(tobj.Angle / Math.PI * 180.0f));
					//g.TranslateTransform(-ox, -oy);

					//g.DrawString(tobj.SymbolID + "", font, fntBrush, new PointF(ox - 10, oy - 10));
				}
				}
			}

			// draw the blobs
			if (blobList.Count > 0) {
				lock(blobList) {
					foreach (TuioBlob tblb in blobList.Values) {
						int bx = tblb.getScreenX(width);
						int by = tblb.getScreenY(height);
						float bw = tblb.Width*width;
						float bh = tblb.Height*height;
						
					
						g.TranslateTransform(bx, by);
						g.RotateTransform((float)(tblb.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-bx, -by);

						g.FillEllipse(blbBrush, bx - bw / 2, by - bh / 2, bw, bh);

						g.TranslateTransform(bx, by);
						g.RotateTransform(-1 * (float)(tblb.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-bx, -by);
						
						g.DrawString(tblb.BlobID + "", font, fntBrush, new PointF(bx, by));
					}
				}
			}
			if (flag1 == false)
			g.DrawImage(ele1, 55, 55);
			else
			{
			flag1 = true; flag2 = true; flag3 = true; flag4 = true;
			}
			if(flag1==true&&stop1==true)
			g.DrawImage(ele1, 100, 300);
			if (flag2 == false)
			g.DrawImage(ele2, 0, 0);
			if(flag2==true&&stop2==true)
			g.DrawImage(ele2, 200, 300);
			if (flag3 == false)
			g.DrawImage(ele3, 0, 55);
			if(flag3==true&&stop3==true)
			g.DrawImage(ele3, 300, 300);
			Pen Pn = new Pen(Color.White, 2);
			//g.DrawEllipse(Pn, num1, num2, 20, 20);
			g.DrawRectangle(Pn, 100, 300, 50, 50);
			g.DrawRectangle(Pn, 200, 300, 50, 50);
			g.DrawRectangle(Pn, 300, 300, 50, 50);

    }

    private void InitializeComponent()
    {
            this.SuspendLayout();
            // 
            // TuioDemo
            // 
            this.ClientSize = new System.Drawing.Size(278, 244);
            this.Name = "TuioDemo";
            this.Load += new System.EventHandler(this.TuioDemo_Load);
            this.ResumeLayout(false);

    }

    private void TuioDemo_Load(object sender, EventArgs e)
    {

    }

    public static void Main(String[] argv) {
	 		int port = 0;
			switch (argv.Length) {
				case 1:
					port = int.Parse(argv[0],null);
					if(port==0) goto default;
					break;
				case 0:
					port = 3333;
					break;
				default:
					Console.WriteLine("usage: mono TuioDemo [port]");
					System.Environment.Exit(0);
					break;
			}
			
			TuioDemo app = new TuioDemo(port);
			Application.Run(app);
		}
	}
