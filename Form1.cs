using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using WMPLib;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Media;
using colorswitchdesktop.Properties;

namespace colorswitchdesktop
{
    public partial class Form1 : Form
    {
        void Draw()
        {
            if (Backbuffer != null)
            {
                using (var g = Graphics.FromImage(Backbuffer))
                {
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;

                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.Clear(black);
                    {
                        if (!started)
                        {
                            if (started_color == pink)
                                color_wish = 180;
                            else if (started_color == gold)
                                color_wish = 90;
                            else if (started_color == blue)
                                color_wish = 0;
                            else if (started_color == yellow)
                                color_wish = 270;
                        }
                        //   draw_logo(g, 0, -3 * Height / 4, Properties.Resources.color_switch_logo3);

                        //   for (int i = 0; i < 50;i+=1 )
                        //   {
                        //     g.FillRectangle(new SolidBrush(Color.White),10,cc-320*i,1000,2);
                        // }

                        if (firsttime)
                        {
                            set_frame(1); set_frame(2); firsttime = false;
                        }
                        else
                        {
                            if (cc % (shape_zone * 20) <= 4 && cc > shape_zone * 3) { set_frame(1); at_cc = cc; }
                        }

                     //   Drawing_line_max_score(g, -Pass_Max, 10);
                        {

                            draw_shape_one(at_cc, g);
                            draw_shape_two(at_cc + shape_zone * shape_number_each_frame, g);

                            if (at_cc != 0)
                                draw_shape_two(true, at_cc + shape_zone * shape_number_each_frame - shape_zone * 20, g);
                        }

                        //     draw_800_coninue_once(g,790);
                        //-----------                
                        draw_motion_ball(g);
                        //------------
                        //  g.TextRenderingHint = TextRenderingHint.AntiAlias;
                        if (cc / shape_zone > Score)
                        {

                            if (First_time_pass_shape)
                            {
                                pass_shape.URL = get_file_path("colorswitchdesktop.Resources.button.wav");
                                First_time_color_switch = false;
                            }
                        }
                        Score = cc / shape_zone;
                        g.DrawString((Score).ToString() + ((GameOver) ? " = " + cc.ToString() + " m" : ""), new Font("Roboto", 24F, FontStyle.Bold), new SolidBrush(Color.White), 14, 15);

                        if (!GameOver)
                        {

                            g.FillEllipse(new SolidBrush(ball_color), Width / 2 - size / 2 - meo / 2, Pos.Y - size / 2 + cc - 3 + meo / 2, size + meo, size - meo);

                            g.FillEllipse(new SolidBrush(Color.FromArgb(50, ball_color.R, ball_color.G, ball_color.B)), Width / 2 - size / 2, Pos.Y - 2 * speed - size / 2 + cc - 3, size, size);
                            g.FillEllipse(new SolidBrush(Color.FromArgb(20, ball_color.R, ball_color.G, ball_color.B)), Width / 2 - size / 2, Pos.Y - 4 * speed - size / 2 + cc - 3, size, size);
                            if (Height - 110 + cc <= Height)
                                g.DrawString("TAP TO JUMP", new Font("VNI-Helve", 20F, FontStyle.Bold), new SolidBrush(ball_color), Width / 2, Height - 85 + cc, sf);
                        }
                        if (GameOver)
                            Break_ball(g, pf_break);

                        if (gamejustover)
                        {

                            if (!down)
                            {
                                whiten += 5;
                                whiten_s += whiten;
                            }
                            else
                            {
                                whiten += 1;
                                whiten_s -= whiten;
                            }
                            if (whiten_s >= 180 && !down) { down = true; whiten = 0; }
                            if (whiten_s <= -30 && down) {
                                gamejustover = false; if (this.GameOver && this.status != "at_result_screen")
                                    this.status = "at_result_screen";
                                if (cc / (shape_zone) > Properties.Settings.Default.MaxScore)
                                    Properties.Settings.Default.MaxScore = cc / (shape_zone);
                                if (cc > Settings.Default.Distance_Max) Settings.Default.Distance_Max = cc;
                            }
                            else g.FillRectangle(new SolidBrush(Color.FromArgb(50 + whiten_s, 255, 255, 255)), -1, -1, Width + 5, Height + 5);

                        }
                    }

                    Invalidate();
                }
            }
        }

        int Score = 0;
        
        private void Drawing_line_max_score(Graphics g,int height,int day)
        {
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddRectangle(new RectangleF(0, height - day / 2+cc, Width, day));
                g.FillPath(new SolidBrush(Color.White), gp);
            }
        }
        private void draw_shape_one(int height, Graphics g)
        {
              int i_min = (cc - height - Height + 100) / shape_zone;
            if (i_min < 0) i_min = 0;
            int i_max = (cc - height - 50) / shape_zone;
           for (int i = i_min; i < shape_number_each_frame && i <= i_max + 1; i++)
            {
                stt_to_draw_shape(frame_one.shape[i], height + i * shape_zone, g);
            }
        }
        private void draw_shape_two(int height, Graphics g)
        {
           int i_min = (cc - height - Height + 50) / shape_zone;
           if (i_min < 0) i_min = 0;
            int i_max = (cc - height - 50) / shape_zone;
           for (int i = i_min; i < shape_number_each_frame && i <= i_max + 1; i++)
                stt_to_draw_shape(frame_two.shape[i], height + i * shape_zone, g);
        }
        private void draw_shape_two(bool t, int height, Graphics g)
        {
           int i_min = (cc - height - Height + 50) / shape_zone;
           if (i_min < 0) i_min = 0;
           int i_max = (cc - height - 50) / shape_zone;
            for (int i = i_min; i < shape_number_each_frame&&i<i_max+1 ; i++)
                stt_to_draw_shape(frame_two_old.shape[i], height + i * shape_zone, g);
        }

        private void _2_hinh_tron_dinh_lay_nhau(Graphics g, bool pink_blue, int height, int r1, int r2, bool cai_to_ben_phai, float speed, bool xoay_len,int dayhon,int daykem)
        {
            int degree = (pink_blue) ? 90 : 270;
            _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, (cai_to_ben_phai) ? r2 : -r2, r2, dayhon, 0, (xoay_len) ? speed : -speed);
            _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, (cai_to_ben_phai) ? -r1 : r1, r1, daykem, degree, (xoay_len) ? -speed : speed);
        }
        private void draw_800_coninue_once(Graphics g, int height_node)
        {
            _khuc_cui_4_nhanh(g, height_node + 100 + 90, -70, 100, 20, 0, 1F);
            _hinh_vuong_4_mau(3, g, height_node + 190 + 390, 0, 90, 20, 0, 1);
            _hinh_vuong_4_mau(4, g, height_node + 190 + 390 + 400, 0, 90, 20, 0, 1);
            //   _hinh_vuong_4_mau(6, g, height_node + 190 + 390+800, 0, 90, 20, 0, 1);
            //     _hinh_vuong_4_mau(9, g, height_node + 190 + 390+1200, 0, 90, 20, 0, 1);

        }
        const int shape_number_each_frame= 30;
        private void _hinh_vuong_4_mau(Color[] color, Graphics g, int height, int lechphai, int longth, int day, int degree, float speed)
        {

            height = -height;
            if (cc + height - 300 > Height + 10) return;
            int number = color.Length;
            height += cc;

            float hesoXoay = rot + degree;
            hesoXoay *= speed;
            longth *= 2;
            int width = Width / 2 + lechphai;
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddArc(width - longth / 2, height - longth / 2, day, day, -270, 180);
                gp.AddArc(width + longth / 2 - day, height - longth / 2, day, day, 270, 180);

                // gp.AddRectangle(new Rectangle( width - longth / 2 + day/2, height - longth / 2, longth-day, day));

                gp.CloseFigure();
                Matrix translateMatrix = new Matrix();
                translateMatrix.RotateAt(hesoXoay, new Point(width, height));

                for (int i = 0; i < number; i++)
                {
                    SolidBrush reff = new SolidBrush(color[i % 4]);
                    gp.Transform(translateMatrix);
                    g.FillPath(reff, gp);
                    if (i == 0) translateMatrix.RotateAt(360 / number - hesoXoay, new Point(width, height));

                }


            }
            if (number == 4)
                using (GraphicsPath gp = new GraphicsPath())
                {
                    gp.AddArc(width - longth / 2, height - longth / 2, day, day, -270, 180);

                    gp.AddRectangle(new Rectangle(width - longth / 2 + day / 2, height - longth / 2, day, day));

                    gp.CloseFigure();
                    Matrix translateMatrix = new Matrix();
                    translateMatrix.RotateAt(hesoXoay, new Point(width, height));
                    gp.Transform(translateMatrix);
                    g.FillPath(sb_pink, gp);

                }
        }
        private void _hinh_vuong_4_mau(int number, Graphics g, int height, int lechphai, int longth, int day, int degree, float speed)
        {

            height = -height;
            if (cc + height - 300 > Height + 10) return;

            height += cc;

            float hesoXoay = rot + degree;
            hesoXoay *= speed;
            longth *= 2;
            int width = Width / 2 + lechphai;
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddArc(width - longth / 2, height - longth / 2, day, day, -270, 180);
                gp.AddArc(width + longth / 2 - day, height - longth / 2, day, day, 270, 180);

                // gp.AddRectangle(new Rectangle( width - longth / 2 + day/2, height - longth / 2, longth-day, day));

                gp.CloseFigure();
                Matrix translateMatrix = new Matrix();
                translateMatrix.RotateAt(hesoXoay, new Point(width, height));

                for (int i = 0; i < number; i++)
                {
                    SolidBrush reff;
                    if (i % 4 == 0) reff = sb_pink;
                    else if (i % 4 == 1) reff = sb_gold;
                    else if (i % 4 == 2) reff = sb_blue;
                    else reff = sb_yellow;
                    gp.Transform(translateMatrix);
                    g.FillPath(reff, gp);
                    if (i == 0) translateMatrix.RotateAt(360 / number - hesoXoay, new Point(width, height));

                }


            }
            if (number == 4)
                using (GraphicsPath gp = new GraphicsPath())
                {
                    gp.AddArc(width - longth / 2, height - longth / 2, day, day, -270, 180);

                    gp.AddRectangle(new Rectangle(width - longth / 2 + day / 2, height - longth / 2, day, day));

                    gp.CloseFigure();
                    Matrix translateMatrix = new Matrix();
                    translateMatrix.RotateAt(hesoXoay, new Point(width, height));
                    gp.Transform(translateMatrix);
                    g.FillPath(sb_pink, gp);

                }
        }
        private void _khuc_cui_4_nhanh(Graphics g, int height, int lechphai, int longth, int day, int degree, float speed)
        {

            height = -height;
            if (cc + height > Height + 500) return;

            height += cc;
            float hesoXoay = rot + degree;
            hesoXoay *= speed;

            int width = Width / 2 + lechphai;
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddArc(width - day / 2, height - longth, day, day, -180, 180); ;

                Point[] path = new Point[] {
                         new Point(width,height),
                         new Point(width-day/2,height-day/2),
                         new Point(width-day/2,height-longth+day/2),
                         new Point(width+day/2,height-longth+day/2),
                         new Point(width+day/2,height-day/2)
                     };
                gp.AddPolygon(path);
                gp.CloseFigure();
                Matrix translateMatrix = new Matrix();
                translateMatrix.RotateAt(hesoXoay, new Point(width, height));
                gp.Transform(translateMatrix);
                g.FillPath(sb_pink, gp);

                translateMatrix.RotateAt(90 - hesoXoay, new Point(width, height));
                gp.Transform(translateMatrix);
                g.FillPath(sb_gold, gp);

                gp.Transform(translateMatrix);
                g.FillPath(sb_blue, gp);

                gp.Transform(translateMatrix);
                g.FillPath(sb_yellow, gp);
            }
        }
        private void draw_motion_ball(Graphics g)
        {

            meo = (Math.Abs(speed / 2) > 4) ? 4 : Math.Abs(speed / 2);
            meo = (speed < 0) ? -meo - 1.75F : meo;
            // meo *= 1/2;
            if (started && !GameOver)
                using (GraphicsPath gp = new GraphicsPath())
                {
                    if (Pos.Y - size / 2 + cc - 3 + meo / 2 + size - meo >= Height)
                    {
                        GameOver = true;
                        BreakOut(g, new PointF(Width / 2, Height));
                        return;
                    }
                    gp.AddEllipse(Width / 2 - size / 2 - meo / 2, Pos.Y - size / 2 + cc - 3 + meo / 2, size + meo, size - meo);
                    PointF[] pf = gp.PathData.Points;

                    for (int i = 0; i < pf.Length; i++)
                    {

                        using (GraphicsPath gppp = new GraphicsPath())
                        {
                            gppp.AddBeziers(pf_excepted);

                            //  g.FillPath(new SolidBrush(Color.FromArgb(0,122,255)), gppp);
                            if (gppp.IsVisible(pf[i]))
                            {

                               



                                if (!moved_height.Contains("[" + -height_setted + "]"))
                                {
                                    if ((!Properties.Settings.Default.Sound_OFF))
                                    {
                                        if (First_time_color_switch)
                                        {
                                            color_switch.URL = get_file_path("colorswitchdesktop.Resources.colorswitch.wav");
                                            First_time_color_switch = false;
                                        }
                                        color_switch.controls.play();
                                    }
                                    moved_height += "[" + -height_setted + "]";
                                }
                                ball_color = to_color;
                                setted = false;
                                
                                return;
                            }
                        }
                        Point p = new Point(Point.Round(pf[i]).X, Point.Round(pf[i]).Y);
                        if (p.Y <= 0 || p.Y > Height || GameOver) continue;
                        Color incolor = Backbuffer.GetPixel(p.X, p.Y);
                        //  if(false)
                        if ((incolor == pink || incolor == gold || incolor == blue || incolor == yellow) && incolor != ball_color)
                        {
                            //  GameTimer.Enabled = false;
                            BreakOut(g, pf[i]); return;
                            break;
                        }
                    }
                }
        }
        bool First_time_color_switch = true,First_time_pass_shape=true;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
          (
              int nLeftRect, // x-coordinate of upper-left corner
              int nTopRect, // y-coordinate of upper-left corner
              int nRightRect, // x-coordinate of lower-right corner
              int nBottomRect, // y-coordinate of lower-right corner
              int nWidthEllipse, // height of ellipse
              int nHeightEllipse // width of ellipse
           );

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        private bool m_aeroEnabled;                     // variables for box shadow
        private const int CS_DROPSHADOW = 0x00040000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        public struct MARGINS                           // struct for box shadow
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        private const int WM_NCHITTEST = 0x84;          // variables for dragging the form
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override CreateParams CreateParams
        {
            get
            {

                m_aeroEnabled = CheckAeroEnabled();

                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        bool shadow = true;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:                        // box shadow
                    if (m_aeroEnabled && shadow)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 0,
                            leftWidth = 0,
                            rightWidth = 1,
                            topHeight = 0
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);

                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);

            //   if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)     // drag the form
            //     m.Result = (IntPtr)HTCAPTION;

        }


        Bitmap Backbuffer;

        System.Windows.Forms.Timer GameTimer;
        public Form1()
        {
            Random rnd = new Random();
            switch (rnd.Next(0, 5))
            {
                case 1: started_color = pink; break;
                case 2: started_color = gold; break;
                case 3: started_color = blue; break;
                case 4: started_color = yellow; break;
                default: started_color = pink; break;
            }
            ball_color = started_color;
            InitializeComponent();

            this.SetStyle(
            ControlStyles.UserPaint
          | ControlStyles.AllPaintingInWmPaint
          | ControlStyles.DoubleBuffer
            , true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            GameTimer = new System.Windows.Forms.Timer();
            GameTimer.Interval = 2;
            GameTimer.Tick += new EventHandler(GameTimer_Tick);
            GameTimer.Start();

         //   this.ResizeEnd += new EventHandler(Form1_CreateBackBuffer);
            this.Load += new EventHandler(Form1_CreateBackBuffer);
            this.Paint += new PaintEventHandler(Form1_Paint);

            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }


        private void Play()
        {

            random_song();
            random_song(true);
            player = new WindowsMediaPlayer();
            player_main_menu = new WindowsMediaPlayer();
            player.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(myplayer_PlayStateChange);
            player_main_menu.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(myplayer_main_PlayStateChange);

            player.URL = get_file_path();
            player_main_menu.URL = get_file_path(true);
            try
            {
                player.controls.play();
                player_main_menu.controls.play();
            }
            catch (System.Exception)
            {

            }

            Update_setMusic();

        }
        private void random_song()
        {
            int mauGi = rnd.Next(1, all_music_file_path.Length);
            {

                using (Stream stream = Utility.GetEmbeddedResourceStream(all_music_file_path[mauGi]))
                {
                    using (Stream output = new FileStream(get_file_path(), FileMode.Create))
                    {
                        byte[] buffer = new byte[32 * 1024];
                        int read;

                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            output.Write(buffer, 0, read);
                        }
                    }

                }
            }
        }
        private void random_song(bool b)
        {
            int mauGi = rnd.Next(1, all_music_file_path.Length);
            {

                using (Stream stream = Utility.GetEmbeddedResourceStream(all_music_file_path[mauGi]))
                {
                    using (Stream output = new FileStream(get_file_path(true), FileMode.Create))
                    {
                        byte[] buffer = new byte[32 * 1024];
                        int read;

                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            output.Write(buffer, 0, read);
                        }
                    }

                }
            }
        }
        void myplayer_main_PlayStateChange(int NewState)
        {
            if (NewState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
            {

                player_main_menu.PlayStateChange -= myplayer_PlayStateChange;
                player_main_menu.close();
                random_song(true);
                player_main_menu = new WindowsMediaPlayer();
                player_main_menu.URL = get_file_path(true);
                player_main_menu.controls.play();

                player_main_menu.PlayStateChange += myplayer_PlayStateChange;

            }
        }
        void myplayer_PlayStateChange(int NewState)
        {
            if (NewState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
            {

                player.PlayStateChange -= myplayer_PlayStateChange;
                player.close();
                random_song();
                player = new WindowsMediaPlayer();
                player.URL = get_file_path();
                player.controls.play();

                player.PlayStateChange += myplayer_PlayStateChange;

            }
        }

        private string get_file_path()
        {
            string appDatafolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folder = System.IO.Path.Combine(appDatafolder, "Color Switch Temp");
            if (!System.IO.Directory.Exists(folder)) System.IO.Directory.CreateDirectory(folder);
            return System.IO.Path.Combine(folder, "temporary.mp3");

        }
        private string get_file_path(bool b)
        {
            string appDatafolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folder = System.IO.Path.Combine(appDatafolder, "Color Switch Temp");
            if (!System.IO.Directory.Exists(folder)) System.IO.Directory.CreateDirectory(folder);
            return System.IO.Path.Combine(folder, "temporary1.mp3");

        }
        private void load_audio()
        {
            for (int i = 0; i < all_files.Length; i++)
            {
                if(all_files[i].Contains(".wav"))
                using (Stream stream = Utility.GetEmbeddedResourceStream(all_files[i]))
                {
                    using (Stream output = new FileStream(get_file_path(all_files[i]), FileMode.Create))
                    {
                        byte[] buffer = new byte[32 * 1024];
                        int read;

                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            output.Write(buffer, 0, read);
                        }
                    }

                }
            }
        }
        private string get_file_path(string audio_name)
        {
            string appDatafolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folder = System.IO.Path.Combine(appDatafolder, "Color Switch Temp");
            if (!System.IO.Directory.Exists(folder)) System.IO.Directory.CreateDirectory(folder);
            return System.IO.Path.Combine(folder,audio_name);
        }
        private void Update_setMusic()
        {
            if (status != "at_gaming")
            {
                try
                {
                    player.controls.stop();
                    if (Properties.Settings.Default.Music_OFF)
                        player_main_menu.controls.stop();
                    else if (player_main_menu.playState == WMPPlayState.wmppsStopped)
                        player_main_menu.controls.play();
                }

                catch (Exception)
                {

                }
            }

            else
            {
                player_main_menu.controls.stop();
                if (Properties.Settings.Default.Music_OFF)
                    player.controls.stop();
                else player.controls.play();
            }
        }
        private void PlayMusic()
        {
            Thread thr = new Thread(new ThreadStart(Play));
            thr.Start();
        }
        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (status == "at_gaming")
                {
                    if (GameOver) return;
                    Sound_jump();
                    if (!started) started = true;
                    speed = speed_reserver;
                    return;
                }
               
            }
         
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (Backbuffer != null)
            {
                e.Graphics.DrawImageUnscaled(Backbuffer, new Point(0, 0));
             //   e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }
        }

        void Form1_CreateBackBuffer(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized) return;
            if (Backbuffer != null)
                Backbuffer.Dispose();

            Backbuffer = new Bitmap(ClientSize.Width, ClientSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb); ;
          
        }
        private void draw_logo(Graphics g, int lech_phai, int height, Bitmap bm)
        {
            height = -height;                                           //  
            g.DrawImageUnscaled(bm, Width / 2 - bm.Width / 2 + lech_phai, cc + height - bm.Width / 2);
        }

        struct frame
        {  // mỗi frame dài 600
           
            public int[] shape;
        };

        private void set_frame(int which_frame)
        {
            bool u = firsttime;
            if (firsttime)
            {
                frame_two.shape = new int[shape_number_each_frame];
                frame_one.shape = new int[shape_number_each_frame];
                firsttime = false;
            }
            for (int i = 0; i < shape_number_each_frame; i++)
                if (which_frame == 1)
                {      //  if (i == 0) frame_one.shape[i] = -1; else
                        frame_one.shape[i] = //22;
                      rnd.Next(0, 71);
                }
                else
                {
                 //   if (!u) frame_two_old = frame_two;
                    frame_two.shape[i] =// 22;
                        rnd.Next(0, 71);
                }
        }


        private void stt_to_draw_shape(int shap, int height, Graphics g)
        {
            switch (shap)
            {
                
                case 0:
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100, 13, 45, 1);
                    break;
                case 1:
                    Switch2OtherColor(g, height - 170, new Color[] { pink, blue });
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100, 13, 45, 1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100 - 13 - 3, 13, 45, -1);
                    break;
                // 2 hình tròn dính lấy nhau
                case 8:
                    Switch2OtherColor(g, height - 170, new Color[] { pink, blue });
                    _2_hinh_tron_dinh_lay_nhau(g, true, height, (Width <= 500) ? Width / 8 : 500 / 8, (Width <= 500) ? Width / 4 : 500 / 4, false, 1.5F, false,13,8);
                    break;
                case 9:
                    Switch2OtherColor(g, height - 170, new Color[] { pink, blue });
                    _2_hinh_tron_dinh_lay_nhau(g, true, height, (Width <= 500) ? Width / 8 : 500 / 8, (Width <= 500) ? Width / 4 : 500 / 4, false, 0.75F, true,13,8);
                    break;
                case 10:
                    Switch2OtherColor(g, height - 170, new Color[] { pink, blue });
                    _2_hinh_tron_dinh_lay_nhau(g, true, height, (Width <= 500) ? Width / 8 : 500 / 8, (Width <= 500) ? Width / 4 : 500 / 4, true, 0.75F, false,13,8);
                    break;
                case 11:
                    Switch2OtherColor(g, height - 170, new Color[] { pink, blue });
                    _2_hinh_tron_dinh_lay_nhau(g, true, height, (Width <= 500) ? Width / 8 : 500 / 8, (Width <= 500) ? Width / 4 : 500 / 4, true, 1.5F, true,13,8);
                    break;

                case 12:
                    Switch2OtherColor(g, height - 170, new Color[] { yellow, gold });
                    _2_hinh_tron_dinh_lay_nhau(g, false, height, (Width <= 500) ? Width / 8 : 500 / 8, (Width <= 500) ? Width / 4 : 500 / 4, false, 1.5F, false,13,8);
                    break;
                case 13:
                    Switch2OtherColor(g, height - 170, new Color[] { yellow, gold });
                    _2_hinh_tron_dinh_lay_nhau(g, false, height, (Width <= 500) ? Width / 8 : 500 / 8, (Width <= 500) ? Width / 4 : 500 / 4, false, 0.75F, true,13,8);
                    break;
                case 14:
                    Switch2OtherColor(g, height - 170, new Color[] { yellow, gold });
                    _2_hinh_tron_dinh_lay_nhau(g, false, height, (Width <= 500) ? Width / 8 : 500 / 8, (Width <= 500) ? Width / 4 : 500 / 4, true, 0.75F, false,13,8);
                    break;
                case 15:
                    Switch2OtherColor(g, height - 170, new Color[] { yellow, gold });
                    _2_hinh_tron_dinh_lay_nhau(g, false, height, (Width <= 500) ? Width / 8 : 500 / 8, (Width <= 500) ? Width / 4 : 500 / 4, true, 1.5F, true,13,8);
                    break;
                // 2 khuc cui 4 nhanh
                case 3:
                    _khuc_cui_4_nhanh(g, height, 70, 70, 18, 0, 3);
                    _khuc_cui_4_nhanh(g, height, -70, 70, 18, 0, -3);
                    break;
                case 16:
                    _khuc_cui_4_nhanh(g, height, 70, 70, 18, 0, 3);
                    _khuc_cui_4_nhanh(g, height, -70, 70, 18, 45, -3);
                    break;
                case 17:
                    _khuc_cui_4_nhanh(g, height, 70, 70, 18, 0, 3);
                    _khuc_cui_4_nhanh(g, height, -70, 70, 18, 90, -3);
                    break;
                case 18:
                    Switch2OtherColor(g, height - 170, new Color[] { pink, blue });
                    _khuc_cui_4_nhanh(g, height, 70, 70, 18, 0, -2);
                    _khuc_cui_4_nhanh(g, height, -70, 70, 18, 0, 2);
                    break;
                case 2:
                    Switch2OtherColor(g, height - 170, new Color[] { pink, blue, gold, yellow });
                    Color_Slide(g, height - 60, 14, 1);
                    Color_Slide(g, height + 60, 14, 1.5F);
                    break;
                case 19:
                    Switch2OtherColor(g, height - 170, new Color[] { gold, yellow });
                    Color_Slide(g, height - 60, 14, 1.5F);
                    Color_Slide(g, height + 60, 14, 1F);
                    break;
                case 20:
                    Color_Slide(g, height - 120, 14, 1.5F);
                    Color_Slide(g, height, 14, 2F);
                    Color_Slide(g, height + 120, 14, 1F);
                    break;
                case 21:
                    Switch2OtherColor(g, height - 170, new Color[] { gold, yellow, blue });
                    Color_Slide(g, height - 120, 14, 1.5F);
                    Color_Slide(g, height, 14, 2F);
                    Color_Slide(g, height + 120, 14, 1F);
                    break;
                case 22:
                    Switch2OtherColor(g, height - 170, new Color[] { pink, yellow, gold });
                    Color_Slide(g, height - 120, 14, 1F);
                    Color_Slide(g, height, 14, 2F);
                    Color_Slide(g, height + 120, 14, 0.5F);
                    break;
                case 5:
                    _khuc_cui_4_nhanh(g, height, -70, 100, 20, 0, 1.25F);
                    break;
                case 23:
                    _khuc_cui_4_nhanh(g, height, -60, 100, 24, 0, -3F);
                    break;
                case 24:
                    _khuc_cui_4_nhanh(g, height, 60, 100, 24, 0, 3F);
                    break;
                case 25:
                    _khuc_cui_4_nhanh(g, height, 60, 100, 24, 0, -1F);
                    break;

                case 26:
                    Switch2OtherColor(g, height - 170, new Color[] { gold, blue, pink });
                    Switch2OtherColor(g, height, new Color[] { yellow });
                    _hinh_vuong_4_mau(3, g, height, 0, 90, 20, 0, 1);
                    break;
                case 27:
                    Switch2OtherColor(g, height - 170, new Color[] { pink, blue, yellow });
                    Switch2OtherColor(g, height, new Color[] { gold });
                    _hinh_vuong_4_mau(new Color[] { pink, blue, yellow }, g, height, 0, 90, 20, 0, -1);
                    break;
                case 28:
                    if (ball_color != gold) Switch2OtherColor(g, height - 170, new Color[] { gold });
                    Switch2OtherColor(g, height, new Color[] { pink, blue, yellow });
                    _hinh_vuong_4_mau(new Color[] { pink, blue, yellow }, g, height, 0, 90, 20, 0, -1);
                    break;
                case 7:
                    _hinh_vuong_4_mau(4, g, height, 0, 90, 20, 0, 1);
                    break;
                case 29:
                    _hinh_vuong_4_mau(4, g, height, -50, 90, 20, 0, -2);
                    break;
                case 30:
                    _hinh_vuong_4_mau(4, g, height, 0, 56, 14, 0, 1);
                    break;
                case 31:
                    _hinh_vuong_4_mau(4, g, height, 0, 56, 14, 0, -1);
                    break;
                case 32:
                    Switch2OtherColor(g, height - 170, new Color[] { gold, blue, pink, yellow });
                    _hinh_vuong_4_mau(4, g, height, 0, 56, 14, 0, 1);
                    break;
                case 4:
                    Switch2OtherColor(g, height, new Color[] { gold, blue, pink, yellow });
                    _hinh_vuong_4_mau(4, g, height, 0, 90, 20, 0, -2);
                    break;
                case 33:
                    Switch2OtherColor(g, height - 170, new Color[] { pink, blue });
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100, 13, 45, -1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100 - 13 - 3, 13, 45, 1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100 - 13 - 3 - 13 - 3, 13, 45, -1);
                    break;
                case 34:
                    Switch2OtherColor(g, height - 170, new Color[] { yellow, blue });
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100, 13, 45, 1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100 - 13 - 3, 13, 45, -2);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100 - 13 - 3 - 13 - 3, 13, 45, 1);

                    break;
                case 35:
                    Switch2OtherColor(g, height - 170, new Color[] { yellow, gold });
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100, 13, 45, 1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100 - 13 - 3, 13, 180, -1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100 - 13 - 3 - 13 - 3, 13, 45, 1);
                    break;
                case 36:
                    Color_Slide(g, height - 120, 120, 1.5F);
                    break;
                case 37:
               _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100, 13, 0, -1);
                    _hinh_vuong_4_mau(4, g, height, 0, 62, 9, 0, 1.5F);
                    break;
                case 38:
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100, 13, 0, -1.5F);
                    _hinh_vuong_4_mau(4, g, height, 0, 62, 9, 0, 1.5F);
                    Switch2OtherColor(g, height - 170, new Color[] { pink, blue });
                    Switch2OtherColor(g, height, new Color[] { pink, blue });

                    break;
                case 39:
                    Switch2OtherColor(g, height - 170, new Color[] { pink, blue });

                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100, 13, 85, 1);
                    _hinh_vuong_4_mau(4, g, height, 0, 62, 9, 0, -1F);
                    break;
                case 40:
                    Switch2OtherColor(g, height - 170, new Color[] { pink, blue });

                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100, 13, 85, 1.5F);
                    _hinh_vuong_4_mau(4, g, height, 0, 62, 9, 0, -1.5F);
                    break;
                case 41:
                    Switch2OtherColor(g, height - 170, new Color[] { yellow,gold });

                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100, 13, 180, 1.5F);
                    _hinh_vuong_4_mau(4, g, height, 0, 62, 9, 0, -1.5F);
                    break;
                case 42:
                    Switch2OtherColor(g, height - 170, new Color[] { yellow, gold });

                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100, 13, 180, 1.5F);
                    _hinh_vuong_4_mau(4, g, height, 0, 62, 9, 0, -1.5F);
                    Switch2OtherColor(g, height, new Color[] { yellow, gold });

                    break;
                case 43:
                 //   Switch2OtherColor(g, height - 170, new Color[] { yellow, gold });

                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 140, 13, 180, 1.5F);
                    _khuc_cui_4_nhanh(g, height, -55, 60, 18, 0, -1);break;
                case 44:
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 64, 13, 180, 1.5F);
                    break;
                case 45:
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 75, 13, 180, -3F);
                    break;
                case 46:
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 75, 13, 180, 3F);
                    break;
                case 47:
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 75, 13, 180, -4F);
                    break;
                case 48:
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 136, 13, 180, 1.5F);
                    _khuc_cui_4_nhanh(g, height, 55, -60, 18, 0, 1); break;
                case 49:
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 140, 13, 180, 1.5F);
                    _khuc_cui_4_nhanh(g, height, 55, 60, 18, 0, -1); break;
                case 50:
                    Switch2OtherColor(g, height - 160, new Color[] { pink, blue });

                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 113, 20, 45, 1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 103 - 13 - 3, 15, 90, -1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100 - 13 - 3 - 13 - 3, 13, 45, 1);
                    Switch2OtherColor(g, height, new Color[] { pink, blue });

                    break;
                case 51:
                    Switch2OtherColor(g, height - 160, new Color[] { pink, blue });

                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 113, 20, 45, 1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 104 - 13 - 3, 15, 45, 1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100 - 13 - 3 - 13 - 3, 13, 90, -1);
                    Switch2OtherColor(g, height, new Color[] { pink, blue });

                    break;
                case 52:
                    Switch2OtherColor(g, height - 170, new Color[] { yellow, gold });
                    _2_hinh_tron_dinh_lay_nhau(g, false, height, 60, 60, false, 1.5F, false,8,8);
                    break;
                case 53:
                    Switch2OtherColor(g, height - 190, new Color[] { pink, blue });
                    _2_hinh_tron_dinh_lay_nhau(g, true, height-100, (Width <= 500) ? Width / 8 : 500 / 8, (Width <= 500) ? Width / 4 : 500 / 4, false, 1.5F, false, 13, 8);
                    _2_hinh_tron_dinh_lay_nhau(g, true, height+100, (Width <= 500) ? Width / 8 : 500 / 8, (Width <= 500) ? Width / 4 : 500 / 4, false, 0.75F, true, 13, 8);

                    break;
                case 54:
                    Switch2OtherColor(g, height - 190, new Color[] { pink, blue });
                    _2_hinh_tron_dinh_lay_nhau(g, true, height - 100, (Width <= 500) ? Width / 8 : 500 / 8, (Width <= 500) ? Width / 4 : 500 / 4, true, 1.5F, true, 13, 8);
                    _2_hinh_tron_dinh_lay_nhau(g, true, height + 100, (Width <= 500) ? Width / 8 : 500 / 8, (Width <= 500) ? Width / 4 : 500 / 4, false, 0.75F, true, 13, 8);

                    break;
                case 55:
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height-120, 0, 60, 11, 45, 1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 60, 11, 45, -1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height +120, 0, 60, 11, 45, 1);
                    Switch2OtherColor(g, height - 120, new Color[] { yellow,gold });

                    break;
                case 56:
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height - 126, 0, 63, 9, 45, 1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 63, 9, 180+45, -1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height + 126, 0, 63, 9, 45, 1);
                    Switch2OtherColor(g, height - 126, new Color[] {blue,pink });

                    break;
                case 57:
                    _hinh_vuong_4_mau(3, g, height, 0, 90, 20, 0, 1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 63, 9, 0, -1);
                    Switch2OtherColor(g, height, new Color[] { blue, pink });

                    break;
                case 58:
                    _Chu_chuyen_mau("WISH YOU HAPPY", g, height, 30, 0, 50);
                    break;
                case 59:
                    _Chu_chuyen_mau("CAREFULLY !", g, height, 30, 0, 40);
                    break;
                case 60:
                    _Chu_chuyen_mau("SLOW DOWN !", g, height, 30, 0, 60);
                    break;
                case 61:
                    _Chu_chuyen_mau("I'M CRAZY TEXT", g, height, 30, 0, 40);
                    break;
                case 62:
                    _Chu_chuyen_mau("IF U LIKE IT\nSHARE IT !", g, height, 30, 0, 80);
                    break;
                case 63:
                    _Chu_chuyen_mau("RELAX ?\nPRESS LEFT MOUSE BUTTON", g, height, 20, 0, 80);
                    break;
                case 64:
                    _Chu_Chay("DO NOT TOUCH ME !", g, height+50, 30, 4,-rot%250);
                    _Chu_Chay("DO NOT TOUCH ME !", g, height-50, 30, 4, rot%350);
                    break;
                case 65:
                    _nhieu_hinh_tron_quay_quay(g, height, 0, 60, 25, 0, 1,20);
                    Switch2OtherColor(g, height, new Color[] { pink, gold, blue, yellow });
                    break;
                case 66:
                    _nhieu_hinh_tron_quay_quay(g, height, 0, 60, 28, 0, -1.5F, 18);
                    Switch2OtherColor(g, height, new Color[] { pink, gold, blue, yellow });
                    break;
                case 67:
                    _nhieu_hinh_tron_quay_quay(g, height, 0, 60, 28, 0, 1.5F, 18);
                    Switch2OtherColor(g, height, new Color[] { pink, gold, blue, yellow });
                    break;
                case 68:
                    _nhieu_hinh_tron_ke_nhau(g, height, 0, 126, 28, 0, 1F, 24);
                    _nhieu_hinh_tron_ke_nhau(g, height, 0, 120-20-4, 21, 50, -1F, 24);
                    Switch2OtherColor(g, height-180, new Color[] { yellow,gold });
                    break;
                case 69:
                    _nhieu_hinh_tron_ke_nhau(g, height, 0, 126, 28, 0, -2F, 24);
                    _nhieu_hinh_tron_ke_nhau(g, height, 0, 120 - 20 - 4, 21, 50, 2F, 24);
                    Switch2OtherColor(g, height - 180, new Color[] { yellow, gold });
                    break;
                case 70:
                    _nhieu_hinh_tron_ke_nhau(g, height, 0, 126, 28, 0, -1.5F, 24);
                    _nhieu_hinh_tron_ke_nhau(g, height, 0, 120 - 20 - 4, 21,45+180, 1.5F, 24);
                    Switch2OtherColor(g, height - 180, new Color[] {blue,pink });
                    break;
                default:
                    Switch2OtherColor(g, height - 170, new Color[] { pink, blue });

                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100, 13, 45, 1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100 - 13 - 3, 13, 90, -1);
                    _hinh_tron_x_mau(new Color[] { pink, gold, blue, yellow }, g, height, 0, 100 - 13 - 3 - 13 - 3, 13, 45, 1);
                    Switch2OtherColor(g, height + 10, new Color[] { pink, blue });

                    break;
            }
        }

        private void _Chu_Chay(string text,Graphics g,int height,int FontSize,int switchFluently,int Distance)
        {
            height = -height;
            var d = g.TextRenderingHint;
            g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Color color;
            if (rot % (4 * switchFluently) <= switchFluently) color = pink;
            else if (rot % (3 * switchFluently) <= switchFluently) color = gold;
            else if (rot % (2 * switchFluently) <= switchFluently) color = blue;
            else color = yellow;
            g.DrawString(text, new Font(static_class.font_arcena, FontSize), new SolidBrush(color),Width/2- Distance, height + cc, sf);
            g.TextRenderingHint = d;
        }
        private void _Chu_chuyen_mau(string text,Graphics g, int height,int FontSize,int degree,int switchFluently)
        {
            height = -height;
            var d = g.TextRenderingHint;
            g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Color color;
            if (rot % (4 * switchFluently) <= switchFluently) color = pink;
          else  if (rot % (3 * switchFluently) <= switchFluently) color = gold;
            else if (rot % (2 * switchFluently) <= switchFluently) color = blue;
            else  color = yellow;
           
                g.DrawString(text, new Font(static_class.font_arcena,FontSize), new SolidBrush(color), Width / 2, height+cc, sf);
            g.TextRenderingHint=d;
        }
        private void _nhieu_hinh_tron_quay_quay(Graphics g, int height, int lechphai, int r, int day, int degree, float speed,int number)
        {
            height = -height - r;
            if (cc + height - 300 > Height + 10) return;
          

            height += cc;

            float hesoXoay = rot * speed + degree;


            int width = Width / 2 + lechphai;
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddEllipse(width, height - r, day, day);
                Matrix translateMatrix = new Matrix();
                translateMatrix.RotateAt(hesoXoay, new Point(width, height + r));
                for(int i=0;i<number;i++)
                {
                    SolidBrush reff;
                    if (i % 4 == 0) reff = sb_pink;
                    else if (i % 4 == 1) reff = sb_gold;
                    else if (i % 4 == 2) reff = sb_blue;
                    else reff = sb_yellow;
                    gp.Transform(translateMatrix);
                    g.FillPath(reff, gp);
                    if (i == 0)
                        translateMatrix.RotateAt(360 / number - hesoXoay, new Point(width, height + r));
                }

            }
        }
        private void _nhieu_hinh_tron_ke_nhau(Graphics g, int height, int lechphai, int r, int day, int degree, float speed, int number)
        {
            height = -height;
            if (cc + height - 300 > Height + 10) return;


            height += cc;

            float hesoXoay = rot * speed + degree;


            int width = Width / 2 + lechphai;
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddEllipse(width, height - r, day, day);
                Matrix translateMatrix = new Matrix();
                translateMatrix.RotateAt(hesoXoay, new Point(width, height));
                for (int i = 0; i < number; i++)
                {
                    SolidBrush reff;
                    if (i <number/4) reff = sb_pink;
                    else if (i <number/2) reff = sb_gold;
                    else if (i < 3*number/4) reff = sb_blue;
                    else reff = sb_yellow;
                    gp.Transform(translateMatrix);
                    g.FillPath(reff, gp);
                    if (i == 0)
                        translateMatrix.RotateAt(360 / number - hesoXoay, new Point(width, height));
                }

            }
        }
        private void _hinh_tron_x_mau(Color[] color, Graphics g, int height, int lechphai, int r, int day, int degree, float speed)
        {
            height = -height - r;
            if (cc + height - 300 > Height + 10) return;
            int number = color.Length;

            height += cc;

            float hesoXoay = rot * speed + degree;


            int width = Width / 2 + lechphai;
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddArc(new Rectangle(width - r, height, 2 * r, 2 * r), 180, 360 / number);
                gp.FillMode = FillMode.Winding;
                PointF[] pf = gp.PathData.Points;
                PointF p = pf[0], p_min = pf[0];
                for (int i = 0; i < pf.Length; i++)
                    if (pf[i].X > p.X) p = pf[i];

                if (number > 2)
                    gp.AddPolygon(new PointF[] { new Point(width, height + r), pf[0], p });
                gp.CloseFigure();
                Matrix translateMatrix = new Matrix();
                translateMatrix.RotateAt(hesoXoay, new Point(width, height + r));

                for (int i = 0; i < number; i++)
                {
                    SolidBrush reff;
                    if (i % 4 == 0) reff = sb_pink;
                    else if (i % 4 == 1) reff = sb_gold;
                    else if (i % 4 == 2) reff = sb_blue;
                    else reff = sb_yellow;
                    gp.Transform(translateMatrix);
                    g.FillPath(reff, gp);
                    if (i == 0)
                        translateMatrix.RotateAt(360 / number - hesoXoay, new Point(width, height + r));

                }
            }
            g.FillEllipse(new SolidBrush(black), width - r + day, height + day, r * 2 - day * 2, r * 2 - day * 2);

        }


        private void circle_size_100(bool d)
        {

            using (Bitmap bm = new Bitmap(600, 600))
            {
                Graphics gr = Graphics.FromImage(bm);
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.Clear(Color.Transparent);
                gr.DrawArc(new Pen(Color.FromArgb(140, 19, 251), 9), 50, 50, 100, 100, 90, 91);
                gr.DrawArc(new Pen(Color.FromArgb(53, 226, 242), 9), 50, 50, 100, 100, 180, 91);
                gr.DrawArc(new Pen(Color.FromArgb(246, 223, 13), 9), 50, 50, 100, 100, 270, 91);
                gr.DrawArc(new Pen(Color.FromArgb(255, 0, 128), 9), 50, 50, 100, 100, 0, 91);
                gr.InterpolationMode = InterpolationMode.HighQualityBilinear;

                //     g.DrawImageUnscaled(bm, 100, cc + height );


            }
            //       g.FillEllipse(new SolidBrush(Color.FromArgb(0,196,255)), BallPos.X - BallSize / 2, BallPos.Y - BallSize / 2, BallSize-xx, BallSize-yy);
            //    g.DrawEllipse(new Pen(Color.FromArgb(0,122,255)), BallPos.X - BallSize / 2, BallPos.Y - BallSize / 2, BallSize-xx, BallSize-yy);

        }
        /*
   static Color pink = Color.FromArgb(255, 45, 85);
   static Color gold = Color.FromArgb(255, 204, 0);
   static Color blue = Color.FromArgb(0, 156, 255);
   static Color yellow = Color.FromArgb(76, 217, 100);

     */
        static Color black = Color.FromArgb(41, 41, 41);
        // /*
        static Color pink = Color.FromArgb(255, 0, 118);
        static Color gold = Color.FromArgb(246, 213, 13);
        static Color blue = Color.FromArgb(53, 226, 248);
        static Color yellow = Color.FromArgb(140, 15, 251);
        //   */
        ///*
        static Color begin_pink = Color.FromArgb(251, 89, 255);
        static Color begin_gold = Color.FromArgb(255, 219, 77);
        static Color begin_blue = Color.FromArgb(89, 89, 228);
        static Color begin_yellow = Color.FromArgb(41, 242, 148);
        SolidBrush begin_sb_pink = new SolidBrush(begin_pink);
        SolidBrush begin_sb_gold = new SolidBrush(begin_gold);
        SolidBrush begin_sb_blue = new SolidBrush(begin_blue);
        SolidBrush begin_sb_yellow = new SolidBrush(begin_yellow);
        //   */
        SolidBrush sb_pink = new SolidBrush(pink);
        SolidBrush sb_gold = new SolidBrush(gold);
        SolidBrush sb_blue = new SolidBrush(blue);
        SolidBrush sb_yellow = new SolidBrush(yellow);

        private void thanh_ngang_4_mau(Graphics g, int height, int longth, int day, int degree, float speed)
        {
            height = -height;
            if (cc + height > Height + 10) return;
            int width_max = (Width <= 500) ? Width : 500;  // độ dài thanh slide này

            float heso = speed * rot;

        }
        private void Color_Slide(Graphics g, int height, int day, float speed)
        {
            height = -height;
            if (cc + height > Height + 10) return;
            int width = Width / 3 + 1;
            float heso = speed * rot;
            int Vitri_chuan = ((int)heso) % (Width + width);
            int t1, t2, t3, t4;
            t1 = Vitri_chuan;
            t2 = Vitri_chuan + width;
            t3 = Vitri_chuan + 2 * width;
            t4 = Vitri_chuan + 3 * width;
            if (speed > 0)
            {
                if (t4 > Width) t4 = t1 - width;
                if (t3 > Width) t3 = t4 - width;
                if (t2 > Width) t2 = t3 - width;
                if (t1 > Width) t1 = t2 - width;
            }
            else
            {
                if (t4 + width <= 0) t4 = t1 + width;
                if (t3 + width <= 0) t3 = t4 + width;
                if (t2 + width <= 0) t2 = t3 + width;
                if (t1 + width <= 0) t1 = t2 + width;

            }
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddRectangle(new Rectangle(t1, cc + height - day / 2, width, day));
                g.FillPath(sb_pink, gp);
            }
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddRectangle(new Rectangle(t2, cc + height - day / 2, width, day));
                g.FillPath(sb_gold, gp);
            }
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddRectangle(new Rectangle(t3, cc + height - day / 2, width, day));
                g.FillPath(sb_blue, gp);
            }
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddRectangle(new Rectangle(t4, cc + height - day / 2, width, day));
                g.FillPath(sb_yellow, gp);
            }

        }
        public static Array RemoveAt(Array source, int index)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (0 > index || index >= source.Length)
                throw new ArgumentOutOfRangeException("index", index, "index is outside the bounds of source array");

            Array dest = Array.CreateInstance(source.GetType().GetElementType(), source.Length - 1);
            Array.Copy(source, 0, dest, 0, index);
            Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }
        private void Switch2OtherColor(Graphics g, int height, Color[] color)
        {
            if (moved_height.Contains("[" + height + "]")) return;
            height = -height;
            int width_1_2 = Width / 2 + 1;
            int number = color.Length;
            int cc_height = cc + height - 15;
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddEllipse(width_1_2 - 16, cc_height - 1, 32, 32);
                if (!setted)
                {
                    setted = true;
                    int choose;
                    for (int i = 0; i < color.Length; i++)
                        if (color[i] == ball_color) RemoveAt(color, i);
                        choose = rnd.Next(0, color.Length);
                        
                    to_color = color[choose];

                    pf_excepted = gp.PathData.Points;

                    height_setted = height;
                }
                if (height == height_setted)
                {
                    pf_excepted = gp.PathData.Points;

                }
            }
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddArc(new Rectangle(width_1_2 - 15, cc_height, 30, 30), 175, 360 / number);
                gp.FillMode = FillMode.Winding;
                PointF[] pf = gp.PathData.Points;
                PointF p = pf[0], p_min = pf[0];
                for (int i = 0; i < pf.Length; i++)
                    if (pf[i].X > p.X) p = pf[i];

                if (number > 2)
                    gp.AddPolygon(new PointF[] { new Point(width_1_2, cc_height + 15), pf[0], p });
                gp.CloseFigure();
                Matrix translateMatrix = new Matrix();
                translateMatrix.RotateAt(92, new Point(width_1_2, cc_height + 15));

                for (int i = 0; i < number; i++)
                {
                    SolidBrush reff = new SolidBrush(color[i % number]);

                    gp.Transform(translateMatrix);
                    if (moved_height.Contains("[" + height + "]")) return;
                    /*
                     if (touch_what_color(gp, reff.Color, height, number))
                     {
                         int choose;
                        do 
                        {
                          choose = rnd.Next(0, color.Length);
                        }
                         while (color[choose]==ball_color);
                        ball_color = color[choose];
                        return;
                     }
                     */
                    g.FillPath(reff, gp);
                    if (i == 0)
                        translateMatrix.RotateAt(360 / number - 90, new Point(width_1_2, cc_height + 15));

                }

            }

        }

        private bool touch_what_color(GraphicsPath gp, Color col, int height, int number)
        {
            if (!started) return false;

            PointF[] pf = gp.PathData.Points;
            for (int i = 0; i < pf.Length; i++)
            {
                Point p = new Point(Point.Round(pf[i]).X, Point.Round(pf[i]).Y);
                if (p.Y <= 0 || p.Y > Height || GameOver) continue;
                Color incolor = Backbuffer.GetPixel(p.X, p.Y);
                if (incolor == ball_color) // Phát hiện chạm màu nhau
                {
                    moved_height += "[" + height + "]";

                    return true;

                }
            }
            return false;
        }
        public class Utility
        {
            /// <summary>
            /// Takes the full name of a resource and loads it in to a stream.
            /// </summary>
            /// <param name="resourceName">Assuming an embedded resource is a file
            /// called info.png and is located in a folder called Resources, it
            /// will be compiled in to the assembly with this fully qualified
            /// name: Full.Assembly.Name.Resources.info.png. That is the string
            /// that you should pass to this method.</param>
            /// <returns></returns>
            public static Stream GetEmbeddedResourceStream(string resourceName)
            {
                return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            }

            /// <summary>
            /// Get the list of all emdedded resources in the assembly.
            /// </summary>
            /// <returns>An array of fully qualified resource names</returns>
            public static string[] GetEmbeddedResourceNames()
            {
                return Assembly.GetExecutingAssembly().GetManifestResourceNames();
            }
        }

        private void circle_Size_100(bool pin_incc, Graphics g, int locaX, int locaY, int r, bool am_duong, int degree, int do_day, float speed)
        {
            if (!pin_incc) locaY = -locaY;
            int cc_in = cc;
            if (!pin_incc) cc_in = 0;
            locaY += r;

            locaY = -locaY;
            float hesoXoay = (am_duong) ? -rot + degree - 45 : rot + degree - 45;
            hesoXoay *= speed;
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddArc(Width / 2 - r, cc_in + locaY, 2 * r, 2 * r, 180, 90);
                gp.AddPolygon(new Point[] { new Point(Width / 2, cc_in + locaY + r), new Point(Width / 2 - r, cc_in + locaY + r), new Point(Width / 2, cc_in + locaY) });

                gp.CloseFigure();


                Matrix translateMatrix = new Matrix();
                translateMatrix.RotateAt(hesoXoay, new Point(Width / 2, cc_in + locaY + r));
                gp.Transform(translateMatrix);
                g.FillPath((pin_incc) ? sb_pink : begin_sb_pink, gp);
            }

            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddArc(Width / 2 - r, cc_in + locaY, 2 * r, 2 * r, 270, 90);
                gp.AddPolygon(new Point[] { new Point(Width / 2, cc_in + locaY + r), new Point(Width / 2 + r, cc_in + locaY + r), new Point(Width / 2, cc_in + locaY) });
                gp.CloseFigure();
                Matrix translateMatrix = new Matrix();
                translateMatrix.RotateAt(hesoXoay, new Point(Width / 2, cc_in + locaY + r));
                gp.Transform(translateMatrix);

                g.FillPath((pin_incc) ? sb_gold : begin_sb_gold, gp);

            }
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddArc(Width / 2 - r, cc_in + locaY, 2 * r, 2 * r, 0, 90);
                gp.AddPolygon(new Point[] { new Point(Width / 2, cc_in + locaY + r), new Point(Width / 2 + r, cc_in + locaY + r), new Point(Width / 2, cc_in + locaY + 2 * r) });
                gp.CloseFigure();
                Matrix translateMatrix = new Matrix();
                translateMatrix.RotateAt(hesoXoay, new Point(Width / 2, cc_in + locaY + r));
                gp.Transform(translateMatrix);

                g.FillPath((pin_incc) ? sb_blue : begin_sb_blue, gp);

            }
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddArc(Width / 2 - r, cc_in + locaY, 2 * r, 2 * r, 90, 90);
                gp.AddPolygon(new Point[] { new Point(Width / 2, cc_in + locaY + r), new Point(Width / 2 - r, cc_in + locaY + r), new Point(Width / 2, cc_in + locaY + 2 * r) });
                gp.CloseFigure();
                Matrix translateMatrix = new Matrix();
                translateMatrix.RotateAt(hesoXoay, new Point(Width / 2, cc_in + locaY + r));
                gp.Transform(translateMatrix);

                g.FillPath((pin_incc) ? sb_yellow : begin_sb_yellow, gp);

            }
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddEllipse(Width / 2 - r + do_day, cc_in + locaY + do_day, 2 * r - 2 * do_day, 2 * r - 2 * do_day);
                Matrix translateMatrix = new Matrix();
                translateMatrix.RotateAt(hesoXoay, new Point(Width / 2, cc_in + locaY + r));
                gp.Transform(translateMatrix);

                g.FillPath(new SolidBrush(black), gp);

            }

        }

        private void BreakOut(Graphics g, PointF pf)
        {
            GameOver = true; status = "game_over_screen"; Update_setMusic();
            if (!Properties.Settings.Default.Sound_OFF) audio3.Play();
            //   breakball.Play();
            gamejustover = true;
            whiten = 0; whiten_s = 0;
            pf_break = pf;
            set_breakball(pf);
        }
        struct bbb
        {
            public float speedX, speedY;
            public int size;
            public Point local;

        }
        bbb[] break_ball = new bbb[number_ball];
        private void set_breakball(PointF pf)
        {
            if (!GameOver) return;
            Point p = Point.Round(pf);
            Random rnd = new Random();

            for (int i = 0; i < number_ball; i++)
            {
                break_ball[i] = new bbb();
                break_ball[i].size = rnd.Next(0, 9 * Width / 350);

                break_ball[i].speedX = rnd.Next(-15, 15);
                break_ball[i].speedY = rnd.Next(-17, 17);
                float angle = (float)(2 * Math.PI * ((float)rnd.NextDouble() - 0.5));
                break_ball[i].local.Y = (int)(p.Y + 5 * Math.Cos(angle));
                break_ball[i].local.X = (int)(p.X + 5 * Math.Sin(angle));
            }
        }

        private void Break_ball(Graphics g, PointF pf)
        {
            for (int i = 0; i < number_ball; i++)
            {
                Color color;
                if (i <= number_ball / 5) color = pink;
                else if (i <= number_ball * 2 / 5) color = gold;
                else if (i <= number_ball * 3 / 5) color = blue;
                else if (i <= number_ball * 4 / 5) color = yellow;
                else color = Color.White;

                bbb breakb = break_ball[i];
                if (breakb.local.Y > Height + 25) continue;
                g.FillEllipse(new SolidBrush(color), breakb.local.X, breakb.local.Y, breakb.size, breakb.size);
                break_ball[i].local.X += (int)break_ball[i].speedX;
                break_ball[i].speedY += 0.1F;
                break_ball[i].local.Y += (int)break_ball[i].speedY;
                if ((break_ball[i].local.X <= 0 && break_ball[i].local.Y < Height * 2 / 3) || break_ball[i].local.X + breakb.size * 2 >= Width) break_ball[i].speedX *= -1;
                //   if (break_ball[i].local.Y <= 0 || break_ball[i].local.Y+breakb.size*2>= Height) break_ball[i].speedY *= -1;


            }
        }

        void GameTimer_Tick(object sender, EventArgs e)
        {
          
            if (rot >= 360 * 1000000) rot = 0;
            rot++;

            if (MousePosition.Y >= Location.Y -5&& MousePosition.Y < Location.Y + 21 && MousePosition.X >= Location.X && MousePosition.X <= Location.X + Size.Width)

                panel1.Visible = true;
            else if(panel1.Visible) panel1.Visible = false;

            if (status == "at_begin_screen")
                Draw_begining_screen();
            else
            {
                if (started)
                {
                    if (Pos.Y < Height * 7 / 12 && started)
                        if (cc < Height * 7 / 12 - Pos.Y)
                        {
                            //  int d = Height *7/ 12+50 - Pos.Y - cc;
                            cc = Height * 7 / 12 - Pos.Y;


                        }

                    if (Pos.Y + cc > Height + 12) GameOver = true;
                }
                if (Pos.Y > Height - 105 && !GameOver)
                {
                    Pos.Y = Height - 105;

                    speed = -speed * 2 / 3;
                    if (Math.Abs(speed) > 1.3)
                        if (!Properties.Settings.Default.Sound_OFF) audio2.Play();
                        else speed = 0;
                }
              //  if (speed >= 0)
                    speed += gravity1;
              //  else speed += gravity2;

                Pos.Y += (int)speed;

                Draw();
                if (this.status == "at_result_screen")
                    this.Drawing_Result_screen();
                
            }
            // TODO: Add the notion of dying (disable the timer and show a message box or something)
        }


        private void Draw_begining_screen()
        {
            if (Backbuffer == null) return;

            using (var g = Graphics.FromImage(Backbuffer))
            {
                
                g.TextRenderingHint = TextRenderingHint.AntiAlias;

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(black);
                Bitmap logo = Properties.Resources.color_switch_logo3;
                //     g.DrawImageUnscaled(logo, Width / 2 - logo.Width / 2, (Height / 2 - 35 - 120) / 2 - logo.Height / 2 + 5);
             //   g.DrawImageUnscaled(logo, Width / 2 - logo.Width / 2, Height / 2 - 35 - 120 - logo.Height - 10 + 10);

                circle_Size_100(false, g, Width / 2, Height / 2 - 35 + 10, 120, true, 0, 21, 0.6F);
                circle_Size_100(false, g, Width / 2, Height / 2 - 35 + 10, 95, false, 0, 19, 0.8F);
                circle_Size_100(false, g, Width / 2, Height / 2 - 35 + 10, 72, true, 0, 16, 1F);
                play_button(g, false, Height / 2 - 35 + 10, 53);
                if (clicked_up)
                {
                    if (!Properties.Settings.Default.NotFirstTime) Properties.Settings.Default.NotFirstTime = true;
                    go_up_down(true);
                }
                else if (clicked_down) go_up_down(false);
                else if (!slide_used)
                    if (rot > 180 * 4) go_up_down(false); else if (rot > 180&!Properties.Settings.Default.NotFirstTime) go_up_down(true);
                int r = ((Width - 80) / 2 < 185) ? (Width - 80) / 2 : 185;
                game_mode(g, false, (Height / 2 - 35 + 120 + 30) + 25 - out_o - out_o * 8, r, out_o, 238, true, false);
                game_mode(g, false, (Height / 2 - 35 + 120 + 30) + 25 - out_o - out_o * 6, r, out_o, 238, false, false);
                game_mode(g, false, (Height / 2 - 35 + 120 + 30) + 25 - out_o - out_o * 4, r, out_o, 238, false, false);

                game_mode(g, false, (Height / 2 - 35 + 120 + 30) + 25 - out_o - out_o * 2, r, out_o, 238, false, false);
                game_mode(g, false, (Height / 2 - 35 + 120 + 30) + 25 - out_o, r, out_o, 238, false, false);

                game_mode(g, false, (Height / 2 - 35 + 120 + 30) + 25 + 30, r, 30, 238, slided_up, true);
                g.DrawString("About", new Font("Roboto", 20F, FontStyle.Regular), new SolidBrush(Color.FromArgb(255, 220, 103, 65)), Width / 2 , (Height / 2 - 35 + 120 + 30) + 25 + 30, sf);
                if (out_o >= 30) draw_text_table(g, (Height / 2 - 35 + 120 + 30) + 25 - 30 * 9, r);


                draw_icon_feature(g, Width / 2 - 94, ((Height / 2 - 35 + 120 + 30) + 25 + 30 + Height) / 2 + 8, RotateImageByAngle(Properties.Resources.Musical_Notes_34, rot * 2, Color.FromArgb((Properties.Settings.Default.Music_OFF) ? 0 : 255, 137, 53, 242), 0, 0));
                draw_icon_feature(g, Width / 2 - 30, ((Height / 2 - 35 + 120 + 30) + 25 + 30 + Height) / 2 + 8, RotateImageByAngle(Properties.Resources.Room_Sound_Filled_34, -rot * 2, Color.FromArgb((Properties.Settings.Default.Sound_OFF) ? 0 : 255, 82, 192, 39), 0, 0));
                draw_icon_feature(g, Width / 2 + 30, ((Height / 2 - 35 + 120 + 30) + 25 + 30 + Height) / 2 + 8, RotateImageByAngle(Properties.Resources.Circled_Up_Right_2_Filled_34, rot * 4, Color.FromArgb(255, 242 - 30 * Properties.Settings.Default.Style_Mode, 151, 53 * Properties.Settings.Default.Style_Mode), 1, 2));
                draw_icon_feature(g, Width / 2 + 94, ((Height / 2 - 35 + 120 + 30) + 25 + 30 + Height) / 2 + 8, RotateImageByAngle(Properties.Resources.Logout_Rounded_Filled_34, -rot * 3, Color.FromArgb(242, 53, 84), 2, 1));

            }

            Invalidate();
        }


        /// <summary>
        /// Rotates the image by angle.
        /// </summary>
        /// <param name="oldBitmap">The old bitmap.</param>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        private Bitmap RotateImageByAngle(System.Drawing.Image oldBitmap, float angle, Color color, int x, int y)
        {
            var newBitmap = new Bitmap(oldBitmap.Width * 3 / 2, oldBitmap.Height * 3 / 2);
            var graphics = Graphics.FromImage(newBitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TranslateTransform((float)newBitmap.Width / 2, (float)newBitmap.Height / 2);
            graphics.RotateTransform(angle);
            graphics.TranslateTransform(-(float)newBitmap.Width / 2, -(float)newBitmap.Height / 2);
            int size = 48;
            graphics.FillEllipse(new SolidBrush(color), newBitmap.Width / 2 - size / 2 + 1, newBitmap.Height / 2 - size / 2 + 1, size, size);

            graphics.DrawImage(oldBitmap, new Point(newBitmap.Width / 2 - oldBitmap.Width / 2 + x, newBitmap.Height / 2 - oldBitmap.Height / 2 + y));
            return newBitmap;
        }
        void draw_text_table(Graphics g, int Y, int r)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            g.DrawString("Color Switch is a popular game in mobile,called as the #1 Addictive Game on Google Play and Apple Store, now I bring it to you by a clone desktop Color Switch.\nTap the ball carefully and pass through the same color or you'll have to start again!\nMore information : This game's made with GDI+ drawing, in C#, some images and icons were got from icons8.com, music & sound 're got from the original executable apk file\n If you liked it, share it to Facebook ^^\n            (ldt)", new Font(static_class.font_arcena, 11.5F, FontStyle.Regular), new SolidBrush(Color.FromArgb(240, 220, 103, 65)), new Rectangle(Width / 2 - r + 10, (Height / 2 - 35 + 120 + 30) + 25 - 30 * 9 - 20, 2 * r - 20, 30 * 10), sf);
        }
        private void draw_icon_feature(Graphics g, int X, int Y, Bitmap bmp)
        {
            g.DrawImageUnscaled(bmp, X - bmp.Width / 2, Y - bmp.Height / 2);
        }
        private void go_up_down(bool vari)
        {
            if (!vari) { if (rot % 2 == 0) { if (out_s < 8) { out_s++; out_o -= out_s; } else { out_o = 0; slided_up = true; if (!slide_used) slide_used = true; out_s = 9; } } }

            else { if (rot % 2 == 0) { if (slided_up) slided_up = false; if (out_s >= -2) { out_s--; out_o += out_s; } } }

        }

        private void play_button(Graphics g, bool pressed, int LocalY, int r)
        {
            g.FillEllipse(new SolidBrush(Color.FromArgb(93, 93, 93)), Width / 2 - r, LocalY - r, 2 * r, 2 * r);

            Bitmap bmp = Properties.Resources.media_play_symbol__2_;
            Bitmap bbb = new Bitmap(bmp, r, r);
            g.DrawImageUnscaled(bbb, Width / 2 - bbb.Width / 2 + 7, LocalY - bbb.Height / 2 - 2);

        }
        private void game_mode(Graphics g, bool pressed, int LocalY, int r, int he, int anpha, bool tren, bool duoi)
        {
            using (GraphicsPath gp = new GraphicsPath())
            {

                Point[] p = new Point[]{
                    new Point(Width/2-r,LocalY-he),
                    new Point(Width/2+r,LocalY-he),
                    new Point(Width/2+r-10,LocalY),
                    new Point(Width/2+r,LocalY+he),
                    new Point(Width/2-r,LocalY+he),
                    new Point(Width/2-r+10,LocalY),
                };

                gp.AddPolygon(p);
                gp.CloseFigure();
                g.FillPath(new SolidBrush(Color.FromArgb(anpha, 158, 57, 43)), gp);
            }
            for (int i = 5; i < 2 * r - 5; i += 5)
            {
                if (tren)
                    g.FillEllipse(new SolidBrush(Color.FromArgb(anpha, 190, 103, 65)), Width / 2 - r + i, LocalY - he + 6 - 3, 3, 3);
                if (duoi)
                    g.FillEllipse(new SolidBrush(Color.FromArgb(anpha, 190, 103, 65)), Width / 2 - r + i, LocalY + he - 6, 3, 3);

            }
            if (!duoi) g.DrawLine(new Pen(Color.FromArgb(190, 190, 103, 65)), Width / 2 - r + 1, LocalY + he, Width / 2 + r - 1, LocalY + he);
        }

        private void label1_Click(object sender, EventArgs e)
        { 
            using (Graphics g = Graphics.FromImage(Backbuffer))
            {
                int rezise = 70;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                   Bitmap thump = new Bitmap(Backbuffer, Width / rezise, Height /rezise);
                  Image  vackbuffer = new Bitmap(thump, Width, Height);
                //  vackbuffer.Save("file.png");


               //    var blur = new GaussianBlur(Backbuffer as Bitmap);  var sw = System.Diagnostics.Stopwatch.StartNew();   var thump = blur.Process(rezise);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawImage(thump, new Rectangle(-25, -25, Width + 100, Height + 100));
                g.DrawString("PAUSE", new Font(static_class.font_arcena, 33), new SolidBrush(ball_color), Width / 2, Height / 2, sf);
                Invalidate();
            }
            GameTimer.Enabled = !GameTimer.Enabled;
            
        }
        string[] all_music_file_path;
        string[] all_files;
        private void Form1_Load(object sender, EventArgs e)
        {

            load_Mode_again();
            all_files = Utility.GetEmbeddedResourceNames();

            int number = 0;
            int len = all_files.Length;
            for (int i = 0; i < len; i++)
                if (all_files[i].Contains(".mp3"))
                    number++;
            all_music_file_path = new string[number];
            number = 0;
            for (int i = 0; i < len; i++)
                if (all_files[i].Contains(".mp3"))
                {
                    all_music_file_path[number] = all_files[i];
                    number++;
                }
            load_audio();
           PlayMusic();
            audio = new System.Media.SoundPlayer(colorswitchdesktop.Properties.Resources.jump);
            audio2 = new System.Media.SoundPlayer(colorswitchdesktop.Properties.Resources.plop);
          
            audio3 = new System.Media.SoundPlayer(colorswitchdesktop.Properties.Resources.dead);
            breakball = new System.Media.SoundPlayer(colorswitchdesktop.Properties.Resources.breakball1);
            //  color_switch = new SoundPlayer(colorswitchdesktop.Properties.Resources.colorswitch);
            color_switch = new WindowsMediaPlayer();
            pass_shape = new WindowsMediaPlayer();
          }
        System.Media.SoundPlayer audio,
            audio2, audio3, breakball;
        WindowsMediaPlayer color_switch,pass_shape;
        private void Sound_jump()
        {
            if (!Properties.Settings.Default.Sound_OFF)
            audio.Play();
           
        }
        
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (status == "at_begin_screen")
            {
                using (Graphics g = Graphics.FromImage(Backbuffer))
                {
                    using (GraphicsPath about_button = new GraphicsPath())
                    {
                        int r = ((Width - 80) / 2 < 185) ? (Width - 80) / 2 : 185;
                        int LocalY = (Height / 2 - 35 + 120 + 30 - 5) + 30 + 30;
                        int he = 30;
                        Point[] p = new Point[]{
                    new Point(Width/2-r,LocalY-he),
                    new Point(Width/2+r,LocalY-he),
                    new Point(Width/2+r-10,LocalY),
                    new Point(Width/2+r,LocalY+he),
                    new Point(Width/2-r,LocalY+he),
                    new Point(Width/2-r+10,LocalY),
                };

                        about_button.AddPolygon(p);
                        about_button.CloseFigure();
                        if (about_button.IsVisible(e.Location))
                        {
                            g.FillPath(new SolidBrush(Color.FromArgb(200, 255, 0, 0)), about_button);

                            if (!clicked_down) clicked_down = true;
                            clicked_up = !clicked_up;
                            return;
                        }
                    }
                    int size = 48;

                    using (GraphicsPath music = new GraphicsPath())
                    {
                        music.AddEllipse(Width / 2 - 94 - size / 2, ((Height / 2 - 35 + 120 + 30) + 25 + 30 + Height) / 2 + 8 - size / 2, size, size);
                        if (music.IsVisible(e.Location))
                        {
                            Properties.Settings.Default.Music_OFF = !Properties.Settings.Default.Music_OFF;
                            Update_setMusic();
                            return;
                        }
                    }
                    using (GraphicsPath sound = new GraphicsPath())
                    {
                        sound.AddEllipse(Width / 2 - 30 - size / 2, ((Height / 2 - 35 + 120 + 30) + 25 + 30 + Height) / 2 + 8 - size / 2, size, size);
                        if (sound.IsVisible(e.Location))
                        {
                            Properties.Settings.Default.Sound_OFF = !Properties.Settings.Default.Sound_OFF;
                            return;
                        }
                    }
                    using (GraphicsPath mode = new GraphicsPath())
                    {
                        mode.AddEllipse(Width / 2 + 30 - size / 2, ((Height / 2 - 35 + 120 + 30) + 25 + 30 + Height) / 2 + 8 - size / 2, size, size);
                        if (mode.IsVisible(e.Location))
                        {
                            if (Properties.Settings.Default.Style_Mode != 3) Properties.Settings.Default.Style_Mode++;
                            else Properties.Settings.Default.Style_Mode = 1;
                            load_Mode_again(); return;
                        }

                    }
                    using (GraphicsPath exit = new GraphicsPath())
                    {
                        exit.AddEllipse(Width / 2 + 94 - size / 2, ((Height / 2 - 35 + 120 + 30) + 25 + 30 + Height) / 2 + 8 - size / 2, size, size);
                        if (exit.IsVisible(e.Location))
                        { this.Close(); return; }
                    }

                    using (GraphicsPath play_button_path = new GraphicsPath())
                    {
                        play_button_path.AddEllipse(Width / 2 - 53, Height / 2 - 35 - 53 + 10, 2 * 53, 2 * 53);
                        // g.FillPath(Color.Black, play_button_path);
                        if (play_button_path.IsVisible(e.Location) && slided_up)
                        {
                            status = "at_gaming";
                            Update_setMusic();
                            //  myplayer_PlayStateChange((int)WMPLib.WMPPlayState.wmppsMediaEnded);
                        }
                        return;
                    }

                }
            }
            else if (status == "at_gaming") // 
            {
                if (e.Button == MouseButtons.Left)
                {
                    Form1_KeyDown(this, new KeyEventArgs(Keys.Space));
                }
                else if (e.Button == MouseButtons.Right)
                    label1_Click(label1, new EventArgs());
            }
            else
            {
                using (GraphicsPath gp = new GraphicsPath())
                {
                    Bitmap bmp = Properties.Resources.Recurring_Appointment;
                    gp.AddEllipse(Width / 2 - bmp.Width / 2, Height / 2 + 100, bmp.Width, bmp.Height);
                    gp.CloseFigure();
                    if (gp.IsVisible(e.Location))
                    { set_replay(); status = "at_gaming"; Update_setMusic(); return; }


                    int size = 48;

                    using (GraphicsPath music = new GraphicsPath())
                    {
                        music.AddEllipse(Width / 2 - 94 - size / 2, ((Height / 2 - 35 + 120 + 30) + 25 + 30 + Height) / 2 + 8 - size / 2, size, size);
                        if (music.IsVisible(e.Location))
                        {
                            Properties.Settings.Default.Music_OFF = !Properties.Settings.Default.Music_OFF;
                            Update_setMusic();
                            return;
                        }
                    }
                    using (GraphicsPath sound = new GraphicsPath())
                    {
                        sound.AddEllipse(Width / 2 - 30 - size / 2, ((Height / 2 - 35 + 120 + 30) + 25 + 30 + Height) / 2 + 8 - size / 2, size, size);
                        if (sound.IsVisible(e.Location))
                        {
                            Properties.Settings.Default.Sound_OFF = !Properties.Settings.Default.Sound_OFF;
                            return;
                        }
                    }
                    using (GraphicsPath mode = new GraphicsPath())
                    {
                        mode.AddEllipse(Width / 2 + 30 - size / 2, ((Height / 2 - 35 + 120 + 30) + 25 + 30 + Height) / 2 + 8 - size / 2, size, size);
                        if (mode.IsVisible(e.Location))
                        {
                            if (Properties.Settings.Default.Style_Mode != 3) Properties.Settings.Default.Style_Mode++;
                            else Properties.Settings.Default.Style_Mode = 1;
                            load_Mode_again(); return;
                        }

                    }
                    using (GraphicsPath exit = new GraphicsPath())
                    {
                        exit.AddEllipse(Width / 2 + 94 - size / 2, ((Height / 2 - 35 + 120 + 30) + 25 + 30 + Height) / 2 + 8 - size / 2, size, size);
                        if (exit.IsVisible(e.Location))
                        { this.Close(); return; }
                    }


                }
            }
        }
       
        Size recommend_size = new Size(400, 600);
            // = new Size((350 * Screen.PrimaryScreen.Bounds.Size.Width / 1366>500)?500: 350 * Screen.PrimaryScreen.Bounds.Size.Width / 1366, (350 * Screen.PrimaryScreen.Bounds.Size.Width / 1366 > 450)? 600*450/350:600 * Screen.PrimaryScreen.Bounds.Size.Height / 768);
        private void load_Mode_again()
        {
            switch (Properties.Settings.Default.Style_Mode)
            {
                case 1: WindowState = FormWindowState.Normal; this.Size =recommend_size; CenterToScreen(); break;
                case 2: WindowState = FormWindowState.Normal; this.Size = new Size(350 * Screen.PrimaryScreen.Bounds.Height / 600, Screen.PrimaryScreen.Bounds.Height); CenterToScreen(); break;
                case 3: WindowState = FormWindowState.Maximized; break;
                default: break;
            }
        }
        private bool _dragging = false;

        private Point _start_point = new Point(0, 0);


        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _start_point = new Point(e.X, e.Y);
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;

        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {

            if (_dragging)
                if (WindowState == FormWindowState.Normal)
                {
                    Point p = PointToScreen(e.Location);
                    Location = new Point(p.X - this._start_point.X, p.Y - this._start_point.Y);

                }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Style_Mode == 3)
                Properties.Settings.Default.Style_Mode = 2;
            else Properties.Settings.Default.Style_Mode = 3;
            load_Mode_again();
        }


        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Style_Mode == 1) Properties.Settings.Default.Style_Mode = 2; else Properties.Settings.Default.Style_Mode = 1;
            load_Mode_again();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
        string status = "at_begin_screen";
        /* at_begin_screen
         * at_gaming
         */ 
        bool clicked_up = false;
        bool clicked_down = false;
        Color ball_color;
        bool GameOver = false;
        bool slided_up = true;
        bool slide_used = false;
        int out_s = 9;
        int out_o = 0;
        float meo;
        bool started = false;
        Color started_color;
        const int number_ball = 100;
        int color_wish = 0;

        int rot = 0;

        PointF pf_break;
        bool down = false;
        int whiten = 0;
        int whiten_s = 0;
        bool gamejustover = false;
        Color to_color = Color.White;
        frame frame_one, frame_two;
        PointF[] pf_excepted = new PointF[]
            {
                new PointF(0,0),
                new PointF(0,1),
                new PointF(1,0),
                new PointF(1,1),
            };
        bool firsttime = true;
        const int frame_length = 1000;
        int cc = 0;
        const float speed_reserver = -6.5F;
        const float gravity1 = 0.32F;
        const float gravity2 = 0.32F;
        float speed = 0;
        int shape_zone = 350;
        int at_cc = 0;
        bool setted = false;
        int height_setted = 0;
        string moved_height = "";
        int size = 19;
        Point Pos = new Point(30, 30);
        frame frame_two_old;
        WindowsMediaPlayer player, player_main_menu;

      
        
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(status=="at_gaming")
            {
                if (ReleaseKey)

                { ReleaseKey = false; }
            }
            if (status == "at_begin_screen")
            {
                if (!ReleaseKey) ReleaseKey = true;
                status = "at_gaming";
                Update_setMusic();
                return;
            }
            if (status == "at_result_screen")
            {
                if (ReleaseKey)

                { ReleaseKey = false; }
                else
                {
                    set_replay(); status = "at_gaming"; Update_setMusic();
                    ReleaseKey = true;
                }
            }
          
        }
        bool ReleaseKey = true;
        Random rnd = new Random();
        private void set_replay()
        {
            status = "at_begin_screen";
            clicked_up = false;
            clicked_down = false;
         
            GameOver = false;
            slided_up = true;
            slide_used = false;
            out_s = 9;
            out_o = 0;

            started = false;
            // Color started_color;
            // const int number_ball = 100;
            color_wish = 0;

            rot = 0;


            down = false;
            whiten = 0;
            whiten_s = 0;
            gamejustover = false;
            to_color = Color.White;
            // frame frame_one, frame_two;
            pf_excepted = new PointF[]
            {
                new PointF(0,0),
                new PointF(0,1),
                new PointF(1,0),
                new PointF(1,1),
            };
            firsttime = true;
            // const i frame_length = 1000;
            cc = 0;
            // const float speed_reserver = -6F;
            // const float gravity1 = 0.32F;
            // const float gravity2 = 0.32F;
            speed = 0;
            // const int shape_zone = 350;
            at_cc = 0;
            setted = false;
            height_setted = 0;
            moved_height = "";
            size = 19;
            Pos = new Point(30, 30);
        }
        private void label3_Click_1(object sender, EventArgs e)
        {
            set_replay();
        }
        private void Drawing_Result_screen()
        {
            if (this.status != "at_result_screen" || this.Backbuffer == null)
                return;
            using (Graphics g = Graphics.FromImage((Image)this.Backbuffer))
            {
               
           //     g.TextRenderingHint = TextRenderingHint.AntiAlias;
                g.SmoothingMode = SmoothingMode.AntiAlias;
        //    Bitmap thump = new Bitmap(Backbuffer, Width / 50, Height /50);
                //  Image  vackbuffer = new Bitmap(thump, Width, Height);
                //  vackbuffer.Save("file.png");


            //    var blur = new GaussianBlur(Backbuffer as Bitmap);

             //   var sw = System.Diagnostics.Stopwatch.StartNew();
             //   var result = blur.Process(10);
            //    g.DrawImage(thump, new Rectangle(0, 0, Width+50, Height+50));
            //    g.FillRectangle((Brush)new SolidBrush(Color.FromArgb(180, 0,0,0)), 0, 0, this.Width, this.Height);
             //   Bitmap colorSwitchLogo3 = Resources.color_switchLogo;

            //    g.FillRectangle((Brush)new LinearGradientBrush(new Point(Width/2, Height/8-5-20),new Point(Width/2,Height/8-5-20+ colorSwitchLogo3.Height + 10),Color.FromArgb(0,0,0,0),Color.FromArgb(51,51,51)), 0, Height / 8 - 5-20 ,this.Width,colorSwitchLogo3.Height+10);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.FillRectangle((Brush)new SolidBrush(Color.FromArgb(200,57,57,57)), 0, Height / 2 - 50 - 30-50-58, this.Width, 50);
                g.DrawString("SCORE", new Font("Roboto", 25F, FontStyle.Regular), new SolidBrush(Color.FromArgb(255, 255, 255)), Width / 2, Height / 2 - 50-30-50-58+28, sf);

                g.DrawString((cc/(shape_zone)).ToString(), new Font("Roboto", 25F, FontStyle.Regular), new SolidBrush(Color.FromArgb(255, 255, 255)),Width/2, Height / 2 - 50 - 30-25, sf);

                g.FillRectangle((Brush)new SolidBrush(Color.FromArgb(200,90, 0, 224)), 0, Height / 2-50-30,this.Width,50);
                g.DrawString("BEST SCORE", new Font("Roboto", 25F, FontStyle.Regular), new SolidBrush(Color.FromArgb(255, 255, 255)), Width / 2, Height / 2-20-31, sf);

                g.DrawString(Properties.Settings.Default.MaxScore.ToString(), new Font("Roboto", 25F, FontStyle.Regular), new SolidBrush(Color.FromArgb(255, 255, 255)),Width/2,Height/2+5,sf);
           //     g.DrawImageUnscaled((Image)colorSwitchLogo3, this.Width / 2 - colorSwitchLogo3.Width / 2, Height/8-20);
                Bitmap bmp = Properties.Resources.Recurring_Appointment;
                g.FillEllipse(new SolidBrush(Color.FromArgb(50,ball_color.R,ball_color.G,ball_color.B)), Width / 2 - bmp.Width / 2-10, Height / 2 + 100-10, bmp.Width+20, bmp.Height+20);
                g.DrawImageUnscaled(bmp, Width / 2 - bmp.Width / 2, Height / 2 + 100);
                draw_icon_feature(g, Width / 2 - 94, ((Height / 2 - 35 + 120 + 30) + 25 + 30 + Height) / 2 + 8, RotateImageByAngle(Properties.Resources.Musical_Notes_34, rot * 2, Color.FromArgb((Properties.Settings.Default.Music_OFF) ? 0 : 255, 137, 53, 242), 0, 0));
                draw_icon_feature(g, Width / 2 - 30, ((Height / 2 - 35 + 120 + 30) + 25 + 30 + Height) / 2 + 8, RotateImageByAngle(Properties.Resources.Room_Sound_Filled_34, -rot * 2, Color.FromArgb((Properties.Settings.Default.Sound_OFF) ? 0 : 255, 82,192,39), 0, 0));
                draw_icon_feature(g, Width / 2 + 30, ((Height / 2 - 35 + 120 + 30) + 25 + 30 + Height) / 2 + 8, RotateImageByAngle(Properties.Resources.Circled_Up_Right_2_Filled_34, rot * 4, Color.FromArgb(255, 242 - 30 * Properties.Settings.Default.Style_Mode, 151, 53 * Properties.Settings.Default.Style_Mode), 1, 2));
                draw_icon_feature(g, Width / 2 + 94, ((Height / 2 - 35 + 120 + 30) + 25 + 30 + Height) / 2 + 8, RotateImageByAngle(Properties.Resources.Logout_Rounded_Filled_34, -rot * 3, Color.FromArgb(242, 53, 84), 2, 1));

            }
            this.Invalidate();
        }
        /*
        void fastblur(Bitmap img, int radius)
        {

            if (radius < 1)
            {
                return;
            }
            int w = img.Width;
            int h = img.Height;
            int wm = w - 1;
            int hm = h - 1;
            int wh = w * h;
            int div = radius + radius + 1;
            int[] r = new int[wh];
            int[] g = new int[wh];
            int[] b = new int[wh];
            int rsum, gsum, bsum, x, y, i, p, p1, p2, yp, yi, yw;
            int[] vmin = new int[Math.Max(w, h)];
            int[] vmax = new int[Math.Max(w, h)];
            int[] pix = new int[w * h];

            img.GetPixel(pix, 0, w, 0, 0, w, h);

            int dv[] = new int[256 * div];
            for (i = 0; i < 256 * div; i++)
            {
                dv[i] = (i / div);
            }

            yw = yi = 0;

            for (y = 0; y < h; y++)
            {
                rsum = gsum = bsum = 0;
                for (i = -radius; i <= radius; i++)
                {
                    p = pix[yi + Math.Min(wm, Math.Max(i, 0))];
                    rsum += (p & 0xff0000) >> 16;
                    gsum += (p & 0x00ff00) >> 8;
                    bsum += p & 0x0000ff;
                }
                for (x = 0; x < w; x++)
                {

                    r[yi] = dv[rsum];
                    g[yi] = dv[gsum];
                    b[yi] = dv[bsum];

                    if (y == 0)
                    {
                        vmin[x] = Math.Min(x + radius + 1, wm);
                        vmax[x] = Math.Max(x - radius, 0);
                    }
                    p1 = pix[yw + vmin[x]];
                    p2 = pix[yw + vmax[x]];

                    rsum += ((p1 & 0xff0000) - (p2 & 0xff0000)) >> 16;
                    gsum += ((p1 & 0x00ff00) - (p2 & 0x00ff00)) >> 8;
                    bsum += (p1 & 0x0000ff) - (p2 & 0x0000ff);
                    yi++;
                }
                yw += w;
            }

            for (x = 0; x < w; x++)
            {
                rsum = gsum = bsum = 0;
                yp = -radius * w;
                for (i = -radius; i <= radius; i++)
                {
                    yi = Math.Max(0, yp) + x;
                    rsum += r[yi];
                    gsum += g[yi];
                    bsum += b[yi];
                    yp += w;
                }
                yi = x;
                for (y = 0; y < h; y++)
                {
                    pix[yi] = 0xff000000 | (dv[rsum] << 16) | (dv[gsum] << 8) | dv[bsum];
                    if (x == 0)
                    {
                        vmin[y] = Math.Min(y + radius + 1, hm) * w;
                        vmax[y] = Math.Max(y - radius, 0) * w;
                    }
                    p1 = x + vmin[y];
                    p2 = x + vmax[y];

                    rsum += r[p1] - r[p2];
                    gsum += g[p1] - g[p2];
                    bsum += b[p1] - b[p2];

                    yi += w;
                }
            }

            img.SetPixel(pix, 0, w, 0, 0, w, h);
        }
        */
        public class GaussianBlur
        {
            private readonly int[] _red;
            private readonly int[] _green;
            private readonly int[] _blue;

            private readonly int _width;
            private readonly int _height;

            private readonly ParallelOptions _pOptions = new ParallelOptions { MaxDegreeOfParallelism = 16 };

            public GaussianBlur(Bitmap image)
            {
                var rct = new Rectangle(0, 0, image.Width, image.Height);
                var source = new int[rct.Width * rct.Height];
                var bits = image.LockBits(rct, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Marshal.Copy(bits.Scan0, source, 0, source.Length);
                image.UnlockBits(bits);

                _width = image.Width;
                _height = image.Height;

                _red = new int[_width * _height];
                _green = new int[_width * _height];
                _blue = new int[_width * _height];

                Parallel.For(0, source.Length, _pOptions, i =>
                {
                    _red[i] = (source[i] & 0xff0000) >> 16;
                    _green[i] = (source[i] & 0x00ff00) >> 8;
                    _blue[i] = (source[i] & 0x0000ff);
                });
            }

            public Bitmap Process(int radial)
            {
                var newRed = new int[_width * _height];
                var newGreen = new int[_width * _height];
                var newBlue = new int[_width * _height];
                var dest = new int[_width * _height];

                Parallel.Invoke(
                    () => gaussBlur_4(_red, newRed, radial),
                    () => gaussBlur_4(_green, newGreen, radial),
                    () => gaussBlur_4(_blue, newBlue, radial));

                Parallel.For(0, dest.Length, _pOptions, i =>
                {
                    if (newRed[i] > 255) newRed[i] = 255;
                    if (newGreen[i] > 255) newGreen[i] = 255;
                    if (newBlue[i] > 255) newBlue[i] = 255;

                    if (newRed[i] < 0) newRed[i] = 0;
                    if (newGreen[i] < 0) newGreen[i] = 0;
                    if (newBlue[i] < 0) newBlue[i] = 0;

                    dest[i] = (int)(0xff000000u | (uint)(newRed[i] << 16) | (uint)(newGreen[i] << 8) | (uint)newBlue[i]);
                });

                var image = new Bitmap(_width, _height);
                var rct = new Rectangle(0, 0, image.Width, image.Height);
                var bits2 = image.LockBits(rct, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Marshal.Copy(dest, 0, bits2.Scan0, dest.Length);
                image.UnlockBits(bits2);
                return image;
            }

            private void gaussBlur_4(int[] source, int[] dest, int r)
            {
                var bxs = boxesForGauss(r, 3);
                boxBlur_4(source, dest, _width, _height, (bxs[0] - 1) / 2);
                boxBlur_4(dest, source, _width, _height, (bxs[1] - 1) / 2);
                boxBlur_4(source, dest, _width, _height, (bxs[2] - 1) / 2);
            }

            private int[] boxesForGauss(int sigma, int n)
            {
                var wIdeal = Math.Sqrt((12 * sigma * sigma / n) + 1);
                var wl = (int)Math.Floor(wIdeal);
                if (wl % 2 == 0) wl--;
                var wu = wl + 2;

                var mIdeal = (double)(12 * sigma * sigma - n * wl * wl - 4 * n * wl - 3 * n) / (-4 * wl - 4);
                var m = Math.Round(mIdeal);

                var sizes = new System.Collections.Generic.List<int>();
                for (var i = 0; i < n; i++) sizes.Add(i < m ? wl : wu);
                return sizes.ToArray();
            }

            private void boxBlur_4(int[] source, int[] dest, int w, int h, int r)
            {
                for (var i = 0; i < source.Length; i++) dest[i] = source[i];
                boxBlurH_4(dest, source, w, h, r);
                boxBlurT_4(source, dest, w, h, r);
            }

            private void boxBlurH_4(int[] source, int[] dest, int w, int h, int r)
            {
                var iar = (double)1 / (r + r + 1);
                Parallel.For(0, h, _pOptions, i =>
                {
                    var ti = i * w;
                    var li = ti;
                    var ri = ti + r;
                    var fv = source[ti];
                    var lv = source[ti + w - 1];
                    var val = (r + 1) * fv;
                    for (var j = 0; j < r; j++) val += source[ti + j];
                    for (var j = 0; j <= r; j++)
                    {
                        val += source[ri++] - fv;
                        dest[ti++] = (int)Math.Round(val * iar);
                    }
                    for (var j = r + 1; j < w - r; j++)
                    {
                        val += source[ri++] - dest[li++];
                        dest[ti++] = (int)Math.Round(val * iar);
                    }
                    for (var j = w - r; j < w; j++)
                    {
                        val += lv - source[li++];
                        dest[ti++] = (int)Math.Round(val * iar);
                    }
                });
            }

            private void boxBlurT_4(int[] source, int[] dest, int w, int h, int r)
            {
                var iar = (double)1 / (r + r + 1);
                Parallel.For(0, w, _pOptions, i =>
                {
                    var ti = i;
                    var li = ti;
                    var ri = ti + r * w;
                    var fv = source[ti];
                    var lv = source[ti + w * (h - 1)];
                    var val = (r + 1) * fv;
                    for (var j = 0; j < r; j++) val += source[ti + j * w];
                    for (var j = 0; j <= r; j++)
                    {
                        val += source[ri] - fv;
                        dest[ti] = (int)Math.Round(val * iar);
                        ri += w;
                        ti += w;
                    }
                    for (var j = r + 1; j < h - r; j++)
                    {
                        val += source[ri] - source[li];
                        dest[ti] = (int)Math.Round(val * iar);
                        li += w;
                        ri += w;
                        ti += w;
                    }
                    for (var j = h - r; j < h; j++)
                    {
                        val += lv - source[li];
                        dest[ti] = (int)Math.Round(val * iar);
                        li += w;
                        ti += w;
                    }
                });
            }
        }
    }
}