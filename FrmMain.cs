using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using vtk;


namespace vtkPointCloud
{
    public partial class FrmMain : Form
    {
        /// <summary>
        /// 存储打开文件的全路径
        /// </summary>

        string[] Path = new string[10];
        int FileConut;

        List<string> File_names = new List<string>();
        List<Button> Color_Buttons = new List<Button>();
        List<TrackBar> Track_Bars = new List<TrackBar>();
        List<Label> Labels = new List<Label>();

        ////ColorDialog colorDialog1 = new ColorDialog();
        int[] color = new int[16];//自定义颜色列表 

        //默认颜色
        int[,] default_colors = new int[22, 3];

        //模型的全局中心点和全局半径
        double[] glo_center = new double[3];
        double glo_radius;
        vtkAxesActor axes = new vtkAxesActor();

        vtkRenderer ren = new vtkRenderer();
        //List<vtkActor> vtkActorAll1 = new List<vtkActor>();

        vtkActor[] vtkActorAll = new vtkActor[22];
        vtkImageActor imgActor = new vtkImageActor();
        vtkInteractorStyleTrackballActor[] trackball_actor = new vtkInteractorStyleTrackballActor[22];

        
        int po_index;
        //pd.yang
        byte[] imgVesselByte;

        //
        int xCount = 512;
        int yCount = 512;
        int zCount = 407;

        int dimX = 512;

        int dimY = 512;

        int dimZ = 407;

        int dimsX = 512; int dimsY = 512; int dimsZ = 407;
        double spacingX = 0.744;
        double spacingY = 0.744;
        double spacingZ = 0.800000000000011;
        
        double distance=0.0;

        vtkPlane plane = new vtkPlane();

        vtkCellPicker picker = new vtkCellPicker();

        vtkPointPicker sevenPicker = new vtkPointPicker();

        vtkImageShrink3D m_mask = new vtkImageShrink3D();

        MyRenderWindowInteracor iren = new MyRenderWindowInteracor();

        vtkRenderWindow renderWindow = new vtkRenderWindow();

        vtkWin32OpenGLRenderWindow openGLrenderWindow = new vtkWin32OpenGLRenderWindow();

        myControl vtkControl = null;

        MouseInteractorStylePP style = new MouseInteractorStylePP();

        MouseInteractorStyleActor style_actor = new MouseInteractorStyleActor();

        vtkInteractorStyleUser style_user = new vtkInteractorStyleUser();

        vtkCamera aCamera = new vtkCamera();

        Random rander = new Random();

        //vtkLineWidget lineWidget = new vtkLineWidget();

        int time = DateTime.Now.DayOfYear;
        int deadline;
        public FrmMain()
        {
            StreamReader sr = new StreamReader("config.txt", Encoding.Default);
            String line = sr.ReadLine();
            sr.Close();
            if (line == null)
            {
                System.IO.File.WriteAllText("config.txt", (time+30).ToString());
                deadline = time + 30;
            }
            else
            {
                deadline = int.Parse(line);
            }

            //产品授权时间检测
            //if ((deadline - time)<0)
            //{
            //    MessageBox.Show("产品试用期已过，请购买使用");
            //    Environment.Exit(0);
            //}
           // MessageBox.Show("产品还有"+(deadline - time).ToString()+"天过期");
            InitializeComponent();
            //读取logo图片
            Random num = new Random();
            vtkPNGReader img_reader = new vtkPNGReader();
            img_reader.SetFileName("./logo.png");
            img_reader.Update();

            for (int i = 0; i < 20; i++)
            {
                default_colors[i, 0] = num.Next(1, 255);
                default_colors[i, 1] = num.Next(1, 255);
                default_colors[i, 2] = num.Next(1, 255);
            }
            //this.button1.Text = "重置取点";
            this.button2.Text = "1";
            this.button3.Text = "2";
            this.button4.Text = "3";
            this.button5.Text = "4";
            this.button6.Text = "5";
            this.button7.Text = "6";
            this.button8.Text = "7";
            this.button9.Text = "8";
            this.button10.Text = "9";
            this.button11.Text = "10";
            this.button12.Text = "11";
            this.button13.Text = "12";
            this.button14.Text = "13";
            this.button15.Text = "14";
            this.button16.Text = "15";
            this.button17.Text = "16";
            this.button18.Text = "17";
            this.button19.Text = "18";
            this.button20.Text = "19";
            this.button21.Text = "20";
            this.button28.Text = "21";
            this.button29.Text = "22";
            //this.button22.Text = "恢复旋转";
            this.button25.Text = "生成穿刺路径";
            //this.button24.Text = "删除所有穿刺路径";
            this.button26.Text = "选择项目";
            this.button30.Text = "计算角度";
            //this.button22.Text = "撤销路径";
            this.button27.Text = "测量深度";

            this.button2.BackColor = Color.FromArgb(default_colors[0, 0], default_colors[0, 1], default_colors[0, 2]);
            Color_Buttons.Add(this.button2);
            this.button3.BackColor = Color.FromArgb(default_colors[1, 0], default_colors[1, 1], default_colors[1, 2]);
            Color_Buttons.Add(this.button3);
            this.button4.BackColor = Color.FromArgb(default_colors[2, 0], default_colors[2, 1], default_colors[2, 2]);
            Color_Buttons.Add(this.button4);
            this.button5.BackColor = Color.FromArgb(default_colors[3, 0], default_colors[3, 1], default_colors[3, 2]);
            Color_Buttons.Add(this.button5);
            this.button6.BackColor = Color.FromArgb(default_colors[4, 0], default_colors[4, 1], default_colors[4, 2]);
            Color_Buttons.Add(this.button6);
            this.button7.BackColor = Color.FromArgb(default_colors[5, 0], default_colors[5, 1], default_colors[5, 2]);
            Color_Buttons.Add(this.button7);
            this.button8.BackColor = Color.FromArgb(default_colors[6, 0], default_colors[6, 1], default_colors[6, 2]);
            Color_Buttons.Add(this.button8);
            this.button9.BackColor = Color.FromArgb(default_colors[7, 0], default_colors[7, 1], default_colors[7, 2]);
            Color_Buttons.Add(this.button9);
            this.button10.BackColor = Color.FromArgb(default_colors[8, 0], default_colors[8, 1], default_colors[8, 2]);
            Color_Buttons.Add(this.button10);
            this.button11.BackColor = Color.FromArgb(default_colors[9, 0], default_colors[9, 1], default_colors[9, 2]);
            Color_Buttons.Add(this.button11);
            this.button12.BackColor = Color.FromArgb(default_colors[10, 0], default_colors[10, 1], default_colors[10, 2]);
            Color_Buttons.Add(this.button12);
            this.button13.BackColor = Color.FromArgb(default_colors[11, 0], default_colors[11, 1], default_colors[11, 2]);
            Color_Buttons.Add(this.button13);
            this.button14.BackColor = Color.FromArgb(default_colors[12, 0], default_colors[12, 1], default_colors[12, 2]);
            Color_Buttons.Add(this.button14);
            this.button15.BackColor = Color.FromArgb(default_colors[13, 0], default_colors[13, 1], default_colors[13, 2]);
            Color_Buttons.Add(this.button15);
            this.button16.BackColor = Color.FromArgb(default_colors[14, 0], default_colors[14, 1], default_colors[14, 2]);
            Color_Buttons.Add(this.button16);
            this.button17.BackColor = Color.FromArgb(default_colors[15, 0], default_colors[15, 1], default_colors[15, 2]);
            Color_Buttons.Add(this.button17);
            this.button18.BackColor = Color.FromArgb(default_colors[16, 0], default_colors[16, 1], default_colors[16, 2]);
            Color_Buttons.Add(this.button18);
            this.button19.BackColor = Color.FromArgb(default_colors[17, 0], default_colors[17, 1], default_colors[17, 2]);
            Color_Buttons.Add(this.button19);
            this.button20.BackColor = Color.FromArgb(default_colors[18, 0], default_colors[18, 1], default_colors[18, 2]);
            Color_Buttons.Add(this.button20);
            this.button21.BackColor = Color.FromArgb(default_colors[19, 0], default_colors[19, 1], default_colors[19, 2]);
            Color_Buttons.Add(this.button21);
            this.button28.BackColor = Color.FromArgb(default_colors[20, 0], default_colors[20, 1], default_colors[20, 2]);
            Color_Buttons.Add(this.button28);
            this.button29.BackColor = Color.FromArgb(default_colors[21, 0], default_colors[21, 1], default_colors[21, 2]);
            Color_Buttons.Add(this.button29);
            this.button1.Text ="";
            this.button22.Text = "";
            this.button31.Text = this.button32.Text = this.button33.Text = "";
            this.button31.BackColor = Color.FromArgb(255, 0, 0);
            this.button32.BackColor = Color.FromArgb(0, 255, 0);
            this.button33.BackColor = Color.FromArgb(0, 0, 255);

            trackBar2.Maximum = 1000;
            trackBar2.Minimum = 0;
            trackBar2.Value = 1000;
            Track_Bars.Add(this.trackBar2);
            trackBar3.Maximum = 1000;
            trackBar3.Minimum = 0;
            trackBar3.Value = 1000;
            Track_Bars.Add(this.trackBar3);
            trackBar4.Maximum = 1000;
            trackBar4.Minimum = 0;
            trackBar4.Value = 1000;
            Track_Bars.Add(this.trackBar4);
            trackBar5.Maximum = 1000;
            trackBar5.Minimum = 0;
            trackBar5.Value = 1000;
            Track_Bars.Add(this.trackBar5);
            trackBar6.Maximum = 1000;
            trackBar6.Minimum = 0;
            trackBar6.Value = 1000;
            Track_Bars.Add(this.trackBar6);
            trackBar7.Maximum = 1000;
            trackBar7.Minimum = 0;
            trackBar7.Value = 1000;
            Track_Bars.Add(this.trackBar7);
            trackBar8.Maximum = 1000;
            trackBar8.Minimum = 0;
            trackBar8.Value = 1000;
            Track_Bars.Add(this.trackBar8);
            trackBar9.Maximum = 1000;
            trackBar9.Minimum = 0;
            trackBar9.Value = 1000;
            Track_Bars.Add(this.trackBar9);
            trackBar10.Maximum = 1000;
            trackBar10.Minimum = 0;
            trackBar10.Value = 1000;
            Track_Bars.Add(this.trackBar10);
            trackBar11.Maximum = 1000;
            trackBar11.Minimum = 0;
            trackBar11.Value = 1000;
            Track_Bars.Add(this.trackBar11);
            trackBar12.Maximum = 1000;
            trackBar12.Minimum = 0;
            trackBar12.Value = 1000;
            Track_Bars.Add(this.trackBar12);
            trackBar13.Maximum = 1000;
            trackBar13.Minimum = 0;

            trackBar13.Value = 1000;
            Track_Bars.Add(this.trackBar13);
            trackBar14.Maximum = 1000;
            trackBar14.Minimum = 0;
            trackBar14.Value = 1000;
            Track_Bars.Add(this.trackBar14);
            trackBar15.Maximum = 1000;
            trackBar15.Minimum = 0;
            trackBar15.Value = 1000;
            Track_Bars.Add(this.trackBar15);
            trackBar16.Maximum = 1000;
            trackBar16.Minimum = 0;
            trackBar16.Value = 1000;
            Track_Bars.Add(this.trackBar16);
            trackBar17.Maximum = 1000;
            trackBar17.Minimum = 0;
            trackBar17.Value = 1000;
            Track_Bars.Add(this.trackBar17);
            trackBar18.Maximum = 1000;
            trackBar18.Minimum = 0;
            trackBar18.Value = 1000;
            Track_Bars.Add(this.trackBar18);
            trackBar19.Maximum = 1000;
            trackBar19.Minimum = 0;
            trackBar19.Value = 1000;
            Track_Bars.Add(this.trackBar19);
            trackBar20.Maximum = 1000;
            trackBar20.Minimum = 0;
            trackBar20.Value = 1000;
            Track_Bars.Add(this.trackBar20);
            trackBar21.Maximum = 1000;
            trackBar21.Minimum = 0;
            trackBar21.Value = 1000;
            Track_Bars.Add(this.trackBar21);
            trackBar22.Maximum = 1000;
            trackBar22.Minimum = 0;
            trackBar22.Value = 1000;
            Track_Bars.Add(this.trackBar22);
            trackBar23.Maximum = 1000;
            trackBar23.Minimum = 0;
            trackBar23.Value = 1000;
            Track_Bars.Add(this.trackBar23);

            label1.Text = "1";
            Labels.Add(label1);
            label2.Text = "2";
            Labels.Add(label2);
            label3.Text = "3";
            Labels.Add(label3);
            label4.Text = "4";
            Labels.Add(label4);
            label5.Text = "5";
            Labels.Add(label5);
            label6.Text = "6";
            Labels.Add(label6);
            label7.Text = "7";
            Labels.Add(label7);
            label8.Text = "8";
            Labels.Add(label8);
            label9.Text = "9";
            Labels.Add(label9);
            label10.Text = "10";
            Labels.Add(label10);
            label11.Text = "11";
            Labels.Add(label11);
            label12.Text = "12";
            Labels.Add(label12);
            label13.Text = "13";
            Labels.Add(label13);
            label14.Text = "14";
            Labels.Add(label14);
            label15.Text = "15";
            Labels.Add(label15);
            label16.Text = "16";
            Labels.Add(label16);
            label17.Text = "17";
            Labels.Add(label17);
            label18.Text = "18";
            Labels.Add(label18);
            label19.Text = "19";
            Labels.Add(label19);
            label20.Text = "20";
            Labels.Add(label20);
            label21.Text = "21";
            Labels.Add(label21);
            label22.Text = "22";
            Labels.Add(label22);
            //label23.Text = "靶点";
            //label24.Text = "穿刺点";
            //this.button1.Visible = false;
            //this.button22.Visible = false;
            this.button23.Text = "靶点";
            this.button24.Text = "穿刺点";
            //this.button25.Visible = false;
            //this.button27.Visible = false;
            this.textBox1.Text = "1.所有取点操作均通过双击完成" + Environment.NewLine +
                                 "2.选择“靶点”或“穿刺点”，则双击生成的点有且只有一个，即靶点或穿刺点" + Environment.NewLine +
                                 "3.最多可生成3根穿刺针" + Environment.NewLine + 
                                 "4.请先加载stl模型，再点击“靶点”或“穿刺点”"
                                 ;
            for (int i=0;i<Color_Buttons.Count;i++)
            {
                Color_Buttons[i].Visible = false;
            }
            for(int i=0;i<Track_Bars.Count;i++)
            {
                Track_Bars[i].Visible = false;
            }
            for(int i=0;i<Labels.Count;i++)
            {
                Labels[i].Visible = false; 
            }
            this.panel1.AutoScroll = true;
            this.panel1.VerticalScroll.Enabled = true;
            this.panel1.VerticalScroll.Visible = true;
            Console.WriteLine(this.panel1.Height);
            Image img = Image.FromFile(@"./qblogo.png");
            this.pictureBox1.Image = img;

            //this.test1ToolStripMenuItem.Visible = false;
            vtkControl = new myControl();
            vtkControl.Location = new Point(30, 30);
            vtkControl.Name = "vtkControl";
            vtkControl.TabIndex = 5;
            vtkControl.Text = "vtkFormsWindowControl";
            vtkControl.Dock = DockStyle.Fill;
            vtkControl.SetRenderWindow(openGLrenderWindow);
            vtkControl.control_renderer = ren;
            this.Controls.Add(vtkControl);
            ren.SetActiveCamera(aCamera);

            //toolStripMenuItem3_Click(null, null);
        }

        private void ToolStripMenuItemImportPointCloud_Click(object sender, EventArgs e)
        {
            trackBar1.Visible = false;
            trackBar1.Maximum = 1000;
            trackBar1.Minimum = 0;

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = true;//打开多个

            openFile.Filter += "数据(*.*)|*.*";
            openFile.Title = "打开文件";

            if (openFile.ShowDialog() == DialogResult.OK)
                if (false)
                {
                po_index = 0;
                Path = openFile.FileNames;
                FileConut = Path.Length - 1;
                long lTick = DateTime.Now.Ticks;

                Random num = new Random();
                for (int i = 0; i <= FileConut; i++)
                {

                    vtkActor Actor1 = new vtkActor();
                    vtkActorAll[i] = Actor1;
                    //   vtkControl.GetRenderWindow().Render();


                    getSTL(Path[i], default_colors[i, 0] * 1.0 / 256, default_colors[i, 1] * 1.0 / 256, default_colors[i, 2] * 1.0 / 256, 1.0, Actor1);
                    
                }


                ren.SetBackground(0f, 0f, 0f);//设置背景为绿色

                
                aCamera.SetViewUp(0, 5, 0);
                aCamera.SetPosition(5, 0, 0);
                aCamera.SetFocalPoint(0, 0, 0);
                aCamera.ComputeViewPlaneNormal();
                
                //iren.SetRenderWindow(renWin);

                ren.ResetCamera();
                aCamera.Dolly(1.5);
                ren.ResetCameraClippingRange();
                vtkControl.GetRenderWindow().AddRenderer(ren);
                iren.SetPicker(sevenPicker);
                iren.SetRenderWindow(vtkControl.GetRenderWindow());
                iren.SetInteractorStyle(style);
                vtkControl.GetRenderWindow().Render();
                //lineWidget.SetInteractor(iren);
                //lineWidget.On();
                iren.Start();
            }
        }

        private void style_OnLeftButtonDown()
        {
            MessageBox.Show("Hello world");
        }

        //操作按键0
        private void 保存图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clear_render();
            this.button26.Visible = false;
            string[] disgus_names = {                   "direct_X_64_linux.dll",
                                                        "direct_x_64_amd.dll",
                                                        "vtkimagereaders_x64_amd.dll",
                                                        "vtkjpegReader_i386.dll",
                                                        "vtkPNGreader_x64.dll",
                                                        "vtkSTLfilters_1_x64.dll",
                                                        "vtkSTLfilters_2_x64.dll",
                                                        "vtkAuto_x64.dll",
                                                        "vtkIMAGEreader_x64.dll",
                                                        "vtkVain_x64.dll",
                                                        "vtkInner_x64_1.dll",
                                                        "vtkInner_x64_2.dll",
                                                        "vtkAW_x64_1.dll",
                                                        "vtk3Dmax_x64_2.dll",
                                                        "vtk3Dmax_x64_1.dll",
                                                        "vtkSerfaceDrawer_x64.dll",
                                                        "vtkOuter_x64.dll",
                                                        "vtkButtom_x64.dll"
                                                        };
            string[] names = { "骨骼","背段", "动脉", "后底段", "后段", "尖后段", "结节1" , "结节2", "结节3",
                                            "静脉","内侧段" ,"内底段" ,"气道" ,"前底段" ,"前段" ,"体表" ,"外侧段" ,"外底段"};

            List<List<string>> StringPairs = new List<List<string>>();
            for (int i = 0; i < 18; i++)
            {
                List<string> StringPair = new List<string>();
                StringPair.Add(disgus_names[i]);
                StringPair.Add(names[i]);
                StringPairs.Add(StringPair);
                Console.Write(disgus_names[i]+"\t"+ names[i]+"\n");
            }

            init();
            string folder = "./.vs/vtkPointCloud.vshost/v14/l";
            DirectoryInfo LungFolder = new DirectoryInfo(folder);
            FileInfo[] files_info = LungFolder.GetFiles();
            //int File_Count = files_info.Length-1;
            foreach (FileInfo NextFile in files_info)
                File_names.Add(NextFile.Name);
            Console.Write(File_names.Count);
            for(int i=0;i<File_names.Count;i++)
            {
                string temp = File_names[i];
                int tag = 0;
                for (int j = 0; j < StringPairs.Count; j++)
                {
                    if (String.Compare(File_names[i], StringPairs[j][0]) == 0)
                    {
                        tag = 1;
                        Labels[i].Text = StringPairs[j][1];
                        break;
                    }
                }
                if (tag == 0)
                    Labels[i].Text = File_names[i];
            }
            for (int i = 0; i < File_names.Count; i++)
            {
                File_names[i] = folder + "/" + File_names[i];
                //Console.WriteLine(File_names[i]);
                //显示各组建
                Color_Buttons[i].Visible = true;
                Track_Bars[i].Visible = true;
                Track_Bars[i].BringToFront();
                Labels[i].Visible = true;

                vtkActor Actor1 = new vtkActor();
                vtkActorAll[i] = Actor1;
                //   vtkControl.GetRenderWindow().Render();
                getSTL(File_names[i], default_colors[i, 0] * 1.0 / 256, default_colors[i, 1] * 1.0 / 256, default_colors[i, 2] * 1.0 / 256, 1.0, Actor1);

            }
            ren.SetBackground(0f, 0f, 0f);//设置背景为绿色

            aCamera.SetViewUp(0, 0, 1);
            aCamera.SetPosition(0, 1, 0);
            aCamera.SetFocalPoint(0, 0, 0);
            aCamera.ComputeViewPlaneNormal();

            //iren.SetRenderWindow(renWin);

            ren.ResetCamera();
            aCamera.Dolly(1);
            ren.ResetCameraClippingRange();
            vtkControl.GetRenderWindow().AddRenderer(ren);
            iren.SetPicker(sevenPicker);
            iren.SetRenderWindow(vtkControl.GetRenderWindow());
            iren.SetInteractorStyle(style);
            vtkControl.GetRenderWindow().Render();
            //lineWidget.SetInteractor(iren);
            //lineWidget.On();
            iren.Start();
        }

        //操作按键1
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            clear_render();
            this.button26.Visible = false;
            string[] disgus_names = { "direct_x_64_amd.dll", "vtkimagereaders_x64_amd.dll", "vtkjpegReader_i386.dll", "vtkPNGreader_x64.dll", "vtkSTLfilters_1_x64.dll", "vtkSTLfilters_2_x64.dll" };
            string[] names = { "动脉", "肝实质", "静脉", "门脉", "下腔", "肿瘤" };

            List<List<string>> StringPairs = new List<List<string>>();
            for (int i = 0; i < 6; i++)
            {
                List<string> StringPair = new List<string>();
                StringPair.Add(disgus_names[i]);
                StringPair.Add(names[i]);
                StringPairs.Add(StringPair);
            }

            init();
            string folder = "./.vs/vtkPointCloud.vshost/l";
            DirectoryInfo LungFolder = new DirectoryInfo(folder);
            FileInfo[] files_info = LungFolder.GetFiles();
            //int File_Count = files_info.Length-1;
            foreach (FileInfo NextFile in files_info)
                File_names.Add(NextFile.Name);
            for (int i = 0; i < File_names.Count; i++)
            {
                string temp = File_names[i];
                for (int j = 0; j < StringPairs.Count; j++)
                {
                    if (String.Compare(File_names[i], StringPairs[j][0]) == 0)
                    {
                        Labels[i].Text = StringPairs[j][1];
                        break;
                    }
                }
                File_names[i] = folder + "/" + File_names[i];
                //Console.WriteLine(File_names[i]);
                //显示各组件
                Color_Buttons[i].Visible = true;
                Track_Bars[i].Visible = true;
                Track_Bars[i].BringToFront();
                Labels[i].Visible = true;

                vtkActor Actor1 = new vtkActor();
                vtkActorAll[i] = Actor1;
                //   vtkControl.GetRenderWindow().Render();
                getSTL(File_names[i], default_colors[i, 0] * 1.0 / 256, default_colors[i, 1] * 1.0 / 256, default_colors[i, 2] * 1.0 / 256, 1.0, Actor1);

            }
            ren.SetBackground(0f, 0f, 0f);//设置背景为绿色

            aCamera.SetViewUp(0, 0, 1);
            aCamera.SetPosition(0, 1, 0);
            aCamera.SetFocalPoint(0, 0, 0);
            aCamera.ComputeViewPlaneNormal();

            //iren.SetRenderWindow(renWin);

            ren.ResetCamera();
            aCamera.Dolly(1);
            ren.ResetCameraClippingRange();
            vtkControl.GetRenderWindow().AddRenderer(ren);
            iren.SetPicker(sevenPicker);
            iren.SetRenderWindow(vtkControl.GetRenderWindow());
            iren.SetInteractorStyle(style);
            vtkControl.GetRenderWindow().Render();
            //lineWidget.SetInteractor(iren);
            //lineWidget.On();
            iren.Start();
        }

        // 打开文件
        private void test1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackBar1.Visible = false;
            trackBar1.Maximum = 1000;
            trackBar1.Minimum = 0;

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = true;//打开多个

            openFile.Filter += "数据(*.*)|*.*";
            openFile.Title = "打开文件";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                po_index = 0;
                Path = openFile.FileNames;
                FileConut = Path.Length - 1;
                long lTick = DateTime.Now.Ticks;

                Random num = new Random();
                for (int i = 0; i <= FileConut; i++)
                {

                    vtkActor Actor1 = new vtkActor();
                    vtkActorAll[i] = Actor1;
                    //   vtkControl.GetRenderWindow().Render();


                    getSTL(Path[i], default_colors[i, 0] * 1.0 / 256, default_colors[i, 1] * 1.0 / 256, default_colors[i, 2] * 1.0 / 256, 1.0, Actor1);

                }

                ren.SetBackground(0f, 0f, 0f);//设置背景为绿色

                vtkCamera aCamera = new vtkCamera();
                aCamera.SetViewUp(0, 5, 0);
                aCamera.SetPosition(5, 0, 0);
                aCamera.SetFocalPoint(0, 0, 0);
                aCamera.ComputeViewPlaneNormal();

                //iren.SetRenderWindow(renWin);

                ren.ResetCamera();
                aCamera.Dolly(1.5);
                ren.ResetCameraClippingRange();
                vtkControl.GetRenderWindow().AddRenderer(ren);
                iren.SetPicker(sevenPicker);
                iren.SetRenderWindow(vtkControl.GetRenderWindow());
                iren.SetInteractorStyle(style);
                vtkControl.GetRenderWindow().Render();
                //lineWidget.SetInteractor(iren);
                //lineWidget.On();
                iren.Start();
            }
        }

        private void getSTL(string path, double r, double g, double b, double po, vtkActor partActor)
        {


            //读源对象读取stl数据文件
            vtkSTLReader part = new vtkSTLReader();
            part.SetOutput(part.GetOutput());
            part.SetFileName(path);
            vtkTriangleFilter triangleFilter = new vtkTriangleFilter();//三角片过滤器
            triangleFilter.SetInputConnection(part.GetOutputPort());//导入合并后的polydata


            //创建过滤器对象，该对象将输入数据集的每个单元向单元质心收缩
            //将会导致相邻单元之间出现裂缝
            vtkShrinkPolyData shrink = new vtkShrinkPolyData();
            //将源对象和过滤器连接
            shrink.SetInput((vtkPolyData)triangleFilter.GetOutput());
            //设置收缩系数，如果为1，不收缩
            shrink.SetShrinkFactor(1);
            //创建映射器对象
            vtkPolyDataMapper partMapper = new vtkPolyDataMapper();
            partMapper.SetInput((vtkPolyData)shrink.GetOutput());
            partMapper.SetNumberOfPieces(1);
            partMapper.SetScalarRange(0, 1);
            partMapper.SetColorMode(0);
            //   partMapper.SetResolveCoincidentTopology(0);
            partMapper.SetScalarMode(0);
            partMapper.SetImmediateModeRendering(0);
            partMapper.SetScalarVisibility(1);
            partMapper.SetUseLookupTableScalarRange(0);
            //创建Props对象(Actor)
            //partActor = new vtkLODActor();
            partActor.SetMapper(partMapper);
            //partActor.GetProperty().SetAmbientColor(0.8275, 0.8275, 0.8275);
            partActor.GetProperty().SetColor(r, g, b);
            //partActor.GetProperty().SetDiffuseColor(0.8275, 0.8275, 0.8275);
            partActor.GetProperty().SetOpacity(po);
            partActor.GetProperty().SetInterpolation(1);
            partActor.GetProperty().SetRepresentation(2);
            partActor.GetProperty().SetBackfaceCulling(0);
            partActor.GetProperty().SetEdgeVisibility(0);
            partActor.GetProperty().SetFrontfaceCulling(0);
            partActor.SetOrigin(0, 0, 0);
            partActor.SetPosition(0, 0, 0);
            partActor.SetScale(1, 1, 1);
            partActor.SetVisibility(1);



            partActor.GetProperty().SetDiffuse(0.5);
            partActor.GetProperty().SetSpecular(0.3);
            partActor.GetProperty().SetSpecularPower(20);
            partActor.GetProperty().SetInterpolationToFlat();
            partActor.GetProperty().SetAmbient(0.5);
            

            ren.AddActor(partActor);
            part.Dispose();
            triangleFilter.Dispose();
            shrink.Dispose();
            partMapper.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();


        }

        
        //操作按键2
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            po_index = 2;
            setcolor();

            //trackBar1.Value = Po_index[3];
            trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[2].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);


            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }
        //操作按键3
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            po_index = 3;
            setcolor();

            //trackBar1.Value = Po_index[4];
            trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[3].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);


            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }
            vtkControl.GetRenderWindow().Render();
        }
        //操作按键4
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            po_index = 4;
            setcolor();

            //trackBar1.Value = Po_index[4];
            trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[4].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);


            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }
            vtkControl.GetRenderWindow().Render();
        }
        //操作按键5
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            po_index = 5;
            setcolor();

            //trackBar1.Value = Po_index[4];
            trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[5].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);


            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }
            vtkControl.GetRenderWindow().Render();
        }
        //操作按键6
        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            po_index = 6;
            setcolor();

            //trackBar1.Value = Po_index[3];
            trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[6].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);


            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }
            vtkControl.GetRenderWindow().Render();
        }
        //操作按键7
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            po_index = 7;
            setcolor();

            //trackBar1.Value = Po_index[3];
            trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[7].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);


            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }
            vtkControl.GetRenderWindow().Render();
        }
        //操作按键8
        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            po_index = 8;
            setcolor();

            //trackBar1.Value = Po_index[3];
            trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[8].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);


            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }
            vtkControl.GetRenderWindow().Render();
        }

        //操作按键9
        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            //po_index = 9;
            vtkControl.GetRenderWindow().Render();
            //setcolor();

        }

        private void setcolor()
        {
            color[0] = Color.Blue.ToArgb();
            color[1] = Color.Black.ToArgb();
            color[2] = Color.Pink.ToArgb();
            color[3] = Color.Red.ToArgb();
            color[4] = Color.Purple.ToArgb();
            color[5] = Color.Blue.ToArgb();
            color[6] = Color.Black.ToArgb();
            color[7] = Color.Pink.ToArgb();
            color[8] = Color.Red.ToArgb();
            color[9] = Color.Blue.ToArgb();
            color[10] = Color.Black.ToArgb();
            color[11] = Color.Pink.ToArgb();
            color[12] = Color.Red.ToArgb();
            color[13] = Color.Blue.ToArgb();
            color[14] = Color.Black.ToArgb();
            color[15] = Color.Pink.ToArgb();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

            vtkActorAll[po_index].GetProperty().SetOpacity(trackBar1.Value / 1000.0);

            vtkControl.GetRenderWindow().Render();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            trackBar1.Visible = false;
            trackBar1.Maximum = 1000;
            trackBar1.Minimum = 0;


            vtkPlane plane = new vtkPlane();

            vtkCellPicker picker = new vtkCellPicker();

            vtkPointPicker sevenPicker = new vtkPointPicker();

            vtkImageShrink3D m_mask = new vtkImageShrink3D();

            vtkRenderWindowInteractor iren = new vtkRenderWindowInteractor();
            vtkCamera aCamera = new vtkCamera();
            aCamera.SetViewUp(0, 5, 0);
            aCamera.SetPosition(5, 0, 0);
            aCamera.SetFocalPoint(0, 0, 0);
            aCamera.ComputeViewPlaneNormal();

            //iren.SetRenderWindow(renWin);

            ren.ResetCamera();
            aCamera.Dolly(1.5);
            ren.ResetCameraClippingRange();

            vtkControl.GetRenderWindow().AddRenderer(ren);

            if (null == imgVesselByte)
            {
                imgVesselByte = new byte[xCount * yCount * zCount];
            }
            FileStream fsa = new FileStream("e:\\左肺.dat", FileMode.Open, FileAccess.Read);//  ll.dat为左肺数据
            fsa.Read(imgVesselByte, 0, imgVesselByte.Length);
            fsa.Close();

            int[] shrinkFactor = new int[3] { 1, 1, 1 };
            Random num = new Random();
            for (int i = 5; i <= 5; i++)
            {
                vtkActor Actor1 = new vtkActor();
                vtkActorAll[i] = Actor1;
                //lungshow(imgVesselByte,shrinkFactor, 30 * i, num.Next(1, 100) / 100.0, num.Next(1, 100) / 100.0, num.Next(1, 100) / 100.0, 1.0, Actor1);
            }
            //vesselRendering(imgVesselByte, xCount, yCount, zCount, 0.744, 0.744, 0.800000000000011, shrinkFactor, "右125", 125); 

            vtkControl.GetRenderWindow().Render();




        }

        private void lungshow(byte[] img, int[] shrink, int i_f, double r, double g, double b, double po, vtkActor partActor)
        {
            //面绘制方法:用于血管
            //接口说明
            //1.array:存放体数据的一维数组
            //2.dimsX,dimsY,dimsZ:体数据的长，宽，高
            //3.spacingX,spacingY,spacingZ:长，宽，高的间隔
            //4.shrinkX,shrinkY,shrinkZ:长 宽 高 的缩放比例
            //5.disPara 透明度设置
            //6.vtkRenderWindow,窗口中添加的vtkFormsWindowControl通过GetRenderWindow方法获得的变量


            //


            for (int i = 0; i < img.Length; i++)
            {

                if ((img[i] - i_f) != 0)
                {
                    img[i] = 0;
                }
            }
            vtkUnsignedCharArray arr = new vtkUnsignedCharArray();
            arr.Allocate(dimsX * dimsY * dimsZ, 1);
            arr.SetArray(img, dimsX * dimsY * dimsZ, 1);


            vtkImageData id = new vtkImageData();
            id.SetDimensions(dimsX, dimsY, dimsZ);
            id.SetSpacing(spacingX, spacingY, spacingZ);
            id.SetScalarTypeToUnsignedChar();

            id.AllocateScalars();
            id.GetPointData().SetScalars(arr);

            m_mask.SetInput(id);
            m_mask.SetShrinkFactors(shrink[0], shrink[1], shrink[2]);


            vtkImageThreshold vit = new vtkImageThreshold();
            vit.SetInputConnection(m_mask.GetOutputPort());
            vit.Update();

            vtkImageShiftScale viss = new vtkImageShiftScale();
            viss.SetInputConnection(vit.GetOutputPort());
            viss.SetOutputScalarTypeToUnsignedChar();
            viss.ClampOverflowOn();//必须要加上，不然边界就不明显 


            vtkContourFilter cf = new vtkContourFilter();
            cf.SetInputConnection(viss.GetOutputPort());
            //  cf.GenerateValues(1, 0, 30);
            cf.SetValue(0, i_f);

            vtkPolyDataMapper mapper = new vtkPolyDataMapper();
            mapper.SetInputConnection(cf.GetOutputPort());
            mapper.ScalarVisibilityOff();//不用灰度值来做颜色映射，自己指定


            ////保存
            //vtkSTLWriter writer = new vtkSTLWriter();

            //vtkTriangleFilter tri = new vtkTriangleFilter();
            //tri.SetInput(cf.GetOutput());


            //writer.SetInputConnection(tri.GetOutputPort());
            //writer.SetFileName("e:\\" + path + ".stl");
            //writer.Update();
            //writer.Write();

            ////Application.Exit();
            //vtkActor vessel = new vtkActor();

            partActor.GetProperty().SetColor(r, g, b);
            partActor.GetProperty().SetOpacity(po);
            partActor.SetMapper(mapper);

            ren.AddActor(partActor);
            vtkControl.GetRenderWindow().Render();



        }
        private void vesselRendering(byte[] img, int dimsX, int dimsY, int dimsZ, double spacingX, double spacingY, double spacingZ, int[] shrink, string path, int i_f)
        {
            //面绘制方法:用于血管
            //接口说明
            //1.array:存放体数据的一维数组
            //2.dimsX,dimsY,dimsZ:体数据的长，宽，高
            //3.spacingX,spacingY,spacingZ:长，宽，高的间隔
            //4.shrinkX,shrinkY,shrinkZ:长 宽 高 的缩放比例
            //5.disPara 透明度设置
            //6.vtkRenderWindow,窗口中添加的vtkFormsWindowControl通过GetRenderWindow方法获得的变量

            int dimX = dimsX;
            int dimY = dimsY;
            int dimZ = dimsZ;



            vtkPlane plane = new vtkPlane();

            vtkCellPicker picker = new vtkCellPicker();

            vtkPointPicker sevenPicker = new vtkPointPicker();

            vtkImageShrink3D m_mask = new vtkImageShrink3D();

            vtkRenderWindowInteractor iren = new vtkRenderWindowInteractor();

            vtkUnsignedCharArray arr = new vtkUnsignedCharArray();
            arr.Allocate(dimsX * dimsY * dimsZ, 1);
            arr.SetArray(img, dimsX * dimsY * dimsZ, 1);


            vtkImageData id = new vtkImageData();
            id.SetDimensions(dimsX, dimsY, dimsZ);
            id.SetSpacing(spacingX, spacingY, spacingZ);
            id.SetScalarTypeToUnsignedChar();

            id.AllocateScalars();
            id.GetPointData().SetScalars(arr);

            m_mask.SetInput(id);
            m_mask.SetShrinkFactors(shrink[0], shrink[1], shrink[2]);


            vtkImageThreshold vit = new vtkImageThreshold();
            vit.SetInputConnection(m_mask.GetOutputPort());
            vit.Update();

            vtkImageShiftScale viss = new vtkImageShiftScale();
            viss.SetInputConnection(vit.GetOutputPort());
            viss.SetOutputScalarTypeToUnsignedChar();
            viss.ClampOverflowOn();//必须要加上，不然边界就不明显 


            vtkMarchingCubes cf = new vtkMarchingCubes();
            cf.SetInputConnection(viss.GetOutputPort());
            //  cf.GenerateValues(1, 0, 30);
            cf.SetValue(0, i_f);

            vtkPolyDataMapper mapper = new vtkPolyDataMapper();
            mapper.SetInputConnection(cf.GetOutputPort());
            mapper.ScalarVisibilityOff();//不用灰度值来做颜色映射，自己指定


            ////保存
            //vtkSTLWriter writer = new vtkSTLWriter();

            //vtkTriangleFilter tri = new vtkTriangleFilter();
            //tri.SetInput(cf.GetOutput());


            //writer.SetInputConnection(tri.GetOutputPort());
            //writer.SetFileName("e:\\" + path+".stl");
            //writer.Update();
            //writer.Write();

            //Application.Exit();
            vtkActor vessel = new vtkActor();

            vessel.GetProperty().SetColor(1.0f, 0.0f, 0.0f);
            vessel.GetProperty().SetOpacity(0.9f);
            vessel.SetMapper(mapper);

            ren.AddActor(vessel);

            vtkCamera aCamera = new vtkCamera();
            aCamera.SetViewUp(0, 5, 0);
            aCamera.SetPosition(5, 0, 0);
            aCamera.SetFocalPoint(0, 0, 0);
            aCamera.ComputeViewPlaneNormal();

            iren.SetRenderWindow(vtkControl.GetRenderWindow());

            ren.ResetCamera();
            aCamera.Dolly(1.5);
            ren.ResetCameraClippingRange();

            vtkControl.GetRenderWindow().AddRenderer(ren);

            ren.SetBackground(1.0f, 1.0f, 1.0f);
            vtkControl.GetRenderWindow().Render();//刷新显示
            vtkOutputWindow.GlobalWarningDisplayOff();//去掉警告窗口


        }

        private void button1_Click(object sender, EventArgs e)
        {
            //vtkActorAll[po_index].GetProperty().SetOpacity(trackBar1.Value / 1000.0);
            //vtkControl.GetRenderWindow().Render();
            int i;
            for (i = 0; i < 20; i++)
            {
                if (vtkActorAll[i] != null)
                {
                    vtkActorAll[i].GetProperty().SetOpacity(0.5);
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            bool is_instance = vtkActorAll[10] == null;
            MessageBox.Show(is_instance.ToString());
        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            po_index = 0;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button2.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();

        }


        private void button11_Click(object sender, EventArgs e)
        {
            po_index = 9;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button11.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            po_index = 1;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button3.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            po_index = 2;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button4.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            po_index = 3;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button5.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            po_index = 4;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button6.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            po_index = 5;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button7.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            po_index = 6;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button8.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            po_index = 7;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button9.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            po_index = 8;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button10.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }

        private void FrmMain_Activated(object sender, EventArgs e)
        {
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
        }

        private void button12_Click(object sender, EventArgs e)
        {
            po_index = 10;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button12.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            po_index = 11;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button13.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            po_index = 12;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button14.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            po_index = 13;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button15.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            po_index = 14;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button16.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            po_index = 15;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button17.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            po_index = 16;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button18.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            po_index = 17;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button19.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            po_index = 18;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button20.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            po_index = 19;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button21.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }
        
        void my_callback_func_1(vtkObject caller, uint eventId, object clientData, IntPtr callData)
        {
            Console.WriteLine("HELLO!!!!!");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //if (vtkControl.point_num != 0)//对应非第一次取点的情况
            //    vtkControl.point_num = 1;
            //else
            //    vtkControl.point_num = 0;
            //iren.SetInteractorStyle(style_user);
            for (int i = 0; i < vtkControl.sphere_num; i++)
            {
                ren.RemoveActor(vtkControl.spheres[i]);
            }
            vtkControl.point_num = 0;
            vtkControl.sphere_num = 0;
            vtkControl.points = new double[20, 3];
        }
        private void button22_Click(object sender, EventArgs e)
        {
            iren.SetInteractorStyle(style);
        }
        

        private void button24_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < vtkControl.tube_num; i++)
            {
                ren.RemoveActor(vtkControl.tubes[i]);
            }
            vtkControl.tube_num = 0;
            //vtkPoints points = new vtkPoints();
            //vtkPolyData polydata = new vtkPolyData();
            //vtkPolyDataMapper lineMaper = new vtkPolyDataMapper();
            //vtkActor lineActor1 = new vtkActor();
            //double[] p1 = new double[3];
            //double[] p2 = new double[3];
            //double[] outer = new double[3];
            //for(int i=0;i<3;i++)
            //{
            //    p1[i] = vtkControl.points[0, i];
            //    p2[i] = vtkControl.points[1,i];
            //}
            ////延展点
            //double m, n;
            //double distance_x = 100;
            //double distance_y = 0;
            //double distance_z = 0;
            ////手动确定x轴的移动
            //outer[0] = p1[0] - distance_x;
            ////计算z轴的移动
            //m = (p1[0] - p2[0]) / (p1[1] - p2[1]);
            //n = (p1[2] - p2[2]) / (p1[1] - p2[1]);
            //distance_z = distance_x * n / m;
            //outer[2] = p1[2] - distance_z;
            ////计算y轴的移动
            //m = (p1[0] - p2[0]) / (p1[2] - p2[2]);
            //n= (p1[1] - p2[1]) / (p1[2] - p2[2]);
            //distance_y = distance_x*n/m;
            //outer[1] = p1[1] - distance_y;
            //points.InsertNextPoint(p2[0], p2[1], p2[2]);
            //points.InsertNextPoint(outer[0], outer[1], outer[2]);
            ////SetId()的第一个参数是线段的端点ID，第二个参数是连接的点的ID
            //vtkLine line0 = new vtkLine();
            //line0.GetPointIds().SetId(0,1);
            ////创建Cell数组，用于存储以上创建的线段
            //vtkCellArray lines1 = new vtkCellArray();
            //lines1.InsertNextCell(line0);
            //polydata.SetPoints(points);
            //polydata.SetLines(lines1);
            //lineMaper.SetInput(polydata);
            //vtkProperty property = new vtkProperty();
            //property.SetColor(0,255,0);
            //property.SetOpacity(1);
            //lineActor1.SetMapper(lineMaper);
            //lineActor1.SetProperty(property);
            //ren.AddActor(lineActor1);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (vtkControl.tube_num >= 3)
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("路径数已满，是否清空？", "系统提示", messButton);
                if (dr == DialogResult.OK)
                {
                    for (int i = 0; i < vtkControl.tube_num; i++)
                    {
                        ren.RemoveActor(vtkControl.tubes[i]);
                    }
                    vtkControl.tube_num = 0;
                    for (int i = 0; i < vtkControl.sphere_num; i++)
                    {
                        ren.RemoveActor(vtkControl.spheres[i]);
                    }
                    vtkControl.point_num = 0;
                    vtkControl.sphere_num = 0;
                    vtkControl.points = new double[20, 3];
                }
                return;
            }
            double[] p1 = new double[3];
            double[] p2 = new double[3];
            double[] outer = new double[3];
            for (int i = 0; i < 3; i++)
            {
                p1[i] = vtkControl.points[vtkControl.begin, i];
                p2[i] = vtkControl.points[vtkControl.end, i];
            }
            //延展点
            double m, n;
            double distance = 30;
            //double distance_x = 0;
            //double distance_y = 0;
            //double distance_z = 0;
            double[] direction = new double[3];
            double length = 0;
            for(int i=0;i<3;i++)
            {
                direction[i] = p2[i] - p1[i];
                length += Math.Pow(direction[i],2);
            }
            length = Math.Sqrt(length);
            for(int i=0;i<3;i++)
            {
                direction[i] = direction[i] / length;
            }
            //计算x轴的移动
            outer[0] = p2[0] + distance*direction[0];
            //计算z轴的移动
            //m = (p1[0] - p2[0]) / (p1[1] - p2[1]);
            //n = (p1[2] - p2[2]) / (p1[1] - p2[1]);
            //distance_z = distance_x * n / m;
            outer[2] = p2[2] + distance*direction[2];
            //计算y轴的移动
            //m = (p1[0] - p2[0]) / (p1[2] - p2[2]);
            //n = (p1[1] - p2[1]) / (p1[2] - p2[2]);
            //distance_y = distance_x * n / m;
            outer[1] = p2[1] + distance*direction[1];
            vtkLineSource line = new vtkLineSource();
            line.SetPoint1(outer[0], outer[1], outer[2]);
            line.SetPoint2(p1[0], p1[1], p1[2]);
            vtkTubeFilter tubeFilter = new vtkTubeFilter();
            tubeFilter.SetInputConnection(line.GetOutputPort());
            tubeFilter.SetRadius(2.0);
            tubeFilter.SetNumberOfSides(100);
            tubeFilter.CappingOn();

            vtkPolyDataMapper mapper = new vtkPolyDataMapper();
            mapper.SetInputConnection(tubeFilter.GetOutputPort());

            vtkProperty property = new vtkProperty();
            property.SetColor(0, 255, 0);
            property.SetOpacity(1);

            vtkActor tube_actor = new vtkActor();
            vtkControl.tubes[vtkControl.tube_num] = tube_actor;
            tube_actor.SetMapper(mapper);
            tube_actor.SetProperty(property);

            vtkControl.tube_num++;
            ren.AddActor(tube_actor);
            this.button22.BackColor = Color.FromArgb(255, 255, 255);
            this.button1.BackColor = Color.FromArgb(255, 255, 255);
            vtkControl.pick_trigger = false;
        }

        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 操作ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[0].GetProperty().SetOpacity(trackBar2.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[1].GetProperty().SetOpacity(trackBar3.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[2].GetProperty().SetOpacity(trackBar4.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[3].GetProperty().SetOpacity(trackBar5.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[4].GetProperty().SetOpacity(trackBar6.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[5].GetProperty().SetOpacity(trackBar7.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[6].GetProperty().SetOpacity(trackBar8.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar9_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[7].GetProperty().SetOpacity(trackBar9.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar10_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[8].GetProperty().SetOpacity(trackBar10.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar11_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[9].GetProperty().SetOpacity(trackBar11.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar12_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[10].GetProperty().SetOpacity(trackBar12.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar13_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[11].GetProperty().SetOpacity(trackBar13.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar14_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[12].GetProperty().SetOpacity(trackBar14.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar15_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[13].GetProperty().SetOpacity(trackBar15.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar16_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[14].GetProperty().SetOpacity(trackBar16.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar17_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[15].GetProperty().SetOpacity(trackBar17.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar18_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[16].GetProperty().SetOpacity(trackBar18.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar19_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[17].GetProperty().SetOpacity(trackBar19.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar20_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[18].GetProperty().SetOpacity(trackBar20.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar21_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[19].GetProperty().SetOpacity(trackBar21.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar22_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[20].GetProperty().SetOpacity(trackBar22.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void trackBar23_Scroll(object sender, EventArgs e)
        {
            vtkActorAll[21].GetProperty().SetOpacity(trackBar23.Value / 1000.0);
            vtkControl.GetRenderWindow().Render();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //初始化界面
        public void init()
        {
            this.glo_radius = 0;
            this.glo_center[0] =0;
            this.glo_center[1] = 0;
            this.glo_center[2] = 0;
            for (int i = 0; i < Color_Buttons.Count; i++)
            {
                if(Color_Buttons[i].Visible)
                Color_Buttons[i].Visible = false;
            }
            for (int i = 0; i < Track_Bars.Count; i++)
            {
                if(Track_Bars[i].Visible)
                Track_Bars[i].Visible = false;
            }
            for (int i = 0; i < Labels.Count; i++)
            {
                if (Labels[i].Visible)
                    Labels[i].Text = i.ToString();
                Labels[i].Visible = false;
            }
            File_names.Clear();
            //清空渲染器中的内容，并移除渲染器，因为上面的renderwindow中用的是addrenderer
            ren.Clear();
            vtkActorCollection Actor_Collections = ren.GetActors();
            int sum = Actor_Collections.GetNumberOfItems();
            for (int i = 0;i< sum; i++)
            {
                try
                {
                    vtkActorCollection temp_Collections = ren.GetActors();
                    ren.RemoveActor(temp_Collections.GetLastActor());
                }
                catch
                {
                    Console.WriteLine("failed");
                }
            }
            vtkControl.GetRenderWindow().RemoveRenderer(ren);
            
            //重置进度条
            for(int i=0;i< Track_Bars.Count;i++)
            {
                Track_Bars[i].Value = Track_Bars[i].Maximum;
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {

        }

        private void Create_Support_Spheres(double[] centro)
        {
            //for(int i=0;i<3;i++)
            //{
            //    Console.Write(centro[i].ToString());
            //}
            double[,] centers = new double[6, 3];
            for(int i=0;i<6;i++)
            {
                for(int j=0;j<3;j++)
                {
                    centers[i, j] = centro[j];
                }
            }
            for(int i=0;i<3;i++)
            {
                centers[i, i] -= 300;
                centers[i + 3, i] += 300;
            }
            int[] selected = new int[3];
            selected[0] = 3;
            selected[1] = 4;
            selected[2] = 5;
            for (int i=0;i<3;i++)
            {
                int[] color = new int[3];
                for(int k=0;k<3;k++)
                {
                    color[k] = 0;
                }
                color[i] = 1;
                vtkSphereSource sphere = new vtkSphereSource();
                sphere.SetCenter(centers[selected[i],0]-vtkControl.centery[0], centers[selected[i], 1]- vtkControl.centery[1], centers[selected[i], 2]- vtkControl.centery[2]);
                sphere.SetRadius(4.5);

                vtkPolyDataMapper mapper = new vtkPolyDataMapper();
                mapper.SetInputConnection(sphere.GetOutputPort());

                vtkProperty property = new vtkProperty();
                property.SetColor(color[0], color[1], color[2]);
                property.SetOpacity(1);
                
                vtkControl.axe_points[i].SetMapper(mapper);
                vtkControl.axe_points[i].SetProperty(property);

                this.ren.AddActor(vtkControl.axe_points[i]);
            }
        }

        private void button26_Click_1(object sender, EventArgs e)
        {
            clear_render();
            this.button26.Visible = false;
            double[] cur_center = new double[3];
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = true;//打开多个

            openFile.Filter += "数据(*.*)|*.*";
            openFile.Title = "打开文件";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                po_index = 0;
                Path = openFile.FileNames;
                FileConut = Path.Length - 1;
                long lTick = DateTime.Now.Ticks;

                Random num = new Random();
                for (int i = 0; i <= FileConut; i++)
                {

                    vtkActor Actor1 = new vtkActor();
                    vtkActorAll[i] = Actor1;
                    //   vtkControl.GetRenderWindow().Render();
                    //显示各组建
                    string[] subPaths = Path[i].Split('\\');
                    Console.WriteLine(subPaths[subPaths.Length-1].Substring(0, subPaths[subPaths.Length - 1].Length-4));
                    Color_Buttons[i].Visible = true;
                    Track_Bars[i].Visible = true;
                    Track_Bars[i].BringToFront();
                    Labels[i].Visible = true;
                    Labels[i].Text = subPaths[subPaths.Length - 1].Substring(0, subPaths[subPaths.Length - 1].Length - 4);

                    getSTL(Path[i], default_colors[i, 0] * 1.0 / 256, default_colors[i, 1] * 1.0 / 256, default_colors[i, 2] * 1.0 / 256, 1.0, Actor1);
                    
                }

                //计算actor的全局中心点          
                double[] radius = new double[Path.Length];
                for (int i = 0; i <= FileConut; i++)
                {
                    cur_center = vtkActorAll[i].GetCenter();
                    //Console.WriteLine(cur_center[0].ToString()+" "+ cur_center[1].ToString()+" "+ cur_center[2].ToString());
                    glo_center[0] += cur_center[0];
                    glo_center[1] += cur_center[1];
                    glo_center[2] += cur_center[2];
                }
                //Console.WriteLine("global  :  " + glo_center[0].ToString() + " " + glo_center[1].ToString() + " " + glo_center[2].ToString());
                //Console.WriteLine("quantaty:  "+ (FileConut+1).ToString());
                for (int j = 0; j < glo_center.Length; j++)
                {
                    glo_center[j] = glo_center[j] / (FileConut + 1);
                }
                Console.WriteLine("global  :  " + glo_center[0].ToString() + " " + glo_center[1].ToString() + " " + glo_center[2].ToString());
                for (int j = 0; j < 3; j++)
                {
                    vtkControl.centery[j] = glo_center[j];
                }

                this.Create_Support_Spheres(glo_center);

                //对准坐标轴和三维对象位置
                for (int i = 0; i <= FileConut; i++)
                {
                    vtkActorAll[i].SetPosition(-1 * glo_center[0], -1 * glo_center[1], -1 * glo_center[2]);
                }

                //添加坐标轴
                if (this.axes == null)
                    axes = new vtkAxesActor();
                axes.SetTotalLength(300, 300, 300);
                axes.SetShaftType(0);
                axes.SetCylinderRadius(0.02);
                axes.SetAxisLabels(0);
                axes.SetPosition(glo_center[0], glo_center[1], glo_center[2]);
                ren.AddActor(axes);

                ren.SetBackground(0f, 0f, 0f);//设置背景为绿色

                vtkCamera aCamera = new vtkCamera();
                aCamera.SetViewUp(0, 5, 0);
                aCamera.SetPosition(5, 0, 0);
                aCamera.SetFocalPoint(0, 0, 0);
                aCamera.ComputeViewPlaneNormal();

                //iren.SetRenderWindow(renWin);

                ren.ResetCamera();
                aCamera.Dolly(1.5);
                ren.ResetCameraClippingRange();
                vtkControl.GetRenderWindow().AddRenderer(ren);
                iren.SetPicker(sevenPicker);
                iren.SetRenderWindow(vtkControl.GetRenderWindow());
                iren.SetInteractorStyle(style);
                style.Rotate();
                vtkControl.GetRenderWindow().Render();
                //lineWidget.SetInteractor(iren);
                //lineWidget.On();
                iren.Start();
            }
        }

        private void 选择项目ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clear_render();
            this.button26.Visible = false;
            init();
            double[] cur_center = new double[3];
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = true;//打开多个

            openFile.Filter += "数据(*.*)|*.*";
            openFile.Title = "打开文件";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                po_index = 0;
                Path = openFile.FileNames;
                FileConut = Path.Length - 1;
                long lTick = DateTime.Now.Ticks;

                Random num = new Random();
                for (int i = 0; i <= FileConut; i++)
                {

                    vtkActor Actor1 = new vtkActor();
                    vtkActorAll[i] = Actor1;
                    //   vtkControl.GetRenderWindow().Render();
                    //显示各组建
                    string[] subPaths = Path[i].Split('\\');
                    //Console.WriteLine(subPaths[subPaths.Length - 1].Substring(0, subPaths[subPaths.Length - 1].Length - 4));
                    Color_Buttons[i].Visible = true;
                    Track_Bars[i].Visible = true;
                    Track_Bars[i].BringToFront();
                    Labels[i].Visible = true;
                    Labels[i].Text = subPaths[subPaths.Length - 1].Substring(0, subPaths[subPaths.Length - 1].Length - 4);

                    getSTL(Path[i], default_colors[i, 0] * 1.0 / 256, default_colors[i, 1] * 1.0 / 256, default_colors[i, 2] * 1.0 / 256, 1.0, Actor1);
                   
                }

                //计算actor的全局中心点          
                double[] radius = new double[Path.Length];
                for (int i=0;i<= FileConut; i++)
                {
                    cur_center = vtkActorAll[i].GetCenter();
                    //Console.WriteLine(cur_center[0].ToString()+" "+ cur_center[1].ToString()+" "+ cur_center[2].ToString());
                    glo_center[0] += cur_center[0];
                    glo_center[1] += cur_center[1];
                    glo_center[2] += cur_center[2];
                }
                //Console.WriteLine("global  :  " + glo_center[0].ToString() + " " + glo_center[1].ToString() + " " + glo_center[2].ToString());
                //Console.WriteLine("quantaty:  "+ (FileConut+1).ToString());
                for (int j=0;j<glo_center.Length;j++)
                {
                    glo_center[j] = glo_center[j] / (FileConut + 1);
                }
                Console.WriteLine("global  :  " + glo_center[0].ToString() + " " + glo_center[1].ToString() + " " + glo_center[2].ToString());
                for(int j=0;j<3;j++)
                {
                    vtkControl.centery[j] = glo_center[j];
                }

                this.Create_Support_Spheres(glo_center);
                //对准坐标轴和三维对象位置
                for (int i = 0; i <= FileConut; i++)
                {
                    vtkActorAll[i].SetPosition(-1 * glo_center[0], -1 * glo_center[1], -1 * glo_center[2]);
                }

                //添加坐标轴
                if (this.axes == null)
                    axes = new vtkAxesActor();
                axes.SetTotalLength(300, 300, 300);
                axes.SetShaftType(1);
                axes.SetCylinderRadius(0.2);
                axes.SetAxisLabels(2);
                axes.SetPosition(glo_center[0], glo_center[1], glo_center[2]);
                ren.AddActor(axes);
                for (int i = 0; i < radius.Length; i++)
                {
                    double temp = 0;
                    double[] this_center = new double[3];
                    this_center = vtkActorAll[i].GetCenter();
                    for (int j = 0; j < glo_center.Length; j++)
                    {
                        temp += Math.Pow((glo_center[j] - this_center[j]), 2);
                    }
                    radius[i] = vtkActorAll[i].GetLength() / 2 + Math.Pow(temp, 0.5);
                    if (glo_radius < radius[i])
                        glo_radius = radius[i];
                }

                Console.WriteLine("global radius  :  " + glo_radius);

                ren.SetBackground(0f, 0f, 0f);//设置背景为绿色
                
                vtkCamera aCamera = new vtkCamera();
                aCamera.SetViewUp(0, 5, 0);
                aCamera.SetPosition(5, 0, 0);
                aCamera.SetFocalPoint(0, 0, 0);
                aCamera.ComputeViewPlaneNormal();
                //iren.SetRenderWindow(renWin);

                ren.ResetCamera();
                aCamera.Dolly(1.5);
                ren.ResetCameraClippingRange();
                vtkControl.GetRenderWindow().AddRenderer(ren);
                iren.SetPicker(sevenPicker);
                iren.SetRenderWindow(vtkControl.GetRenderWindow());
                iren.SetInteractorStyle(style);
                vtkControl.GetRenderWindow().Render();
                //lineWidget.SetInteractor(iren);
                //lineWidget.On();
                iren.Start();

                
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        
        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            this.panel1.Height = this.Height-100;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void button28_Click(object sender, EventArgs e)
        {
            po_index =20;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button28.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            po_index = 21;
            setcolor();

            //trackBar1.Value = Po_index[2];
            //trackBar1.Visible = true;
            //partActor1.GetProperty().SetOpacity(trackBar1.Value/100.0);

            //trackBar1.Visible = false;

            colorDialog1.FullOpen = true; //是否显示ColorDialog有半部分，运行一下就很了然了 
            colorDialog1.CustomColors = color;//设置自定义颜色 
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应 
            {
                vtkActorAll[po_index].GetProperty().SetColor(colorDialog1.Color.R * 1.0 / 256, colorDialog1.Color.G * 1.0 / 256, colorDialog1.Color.B * 1.0 / 256);
                this.button29.BackColor = Color.FromArgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            }

            if (result == DialogResult.Cancel)//取消事件响应 
            {; }

            vtkControl.GetRenderWindow().Render();
        }

        //private void button30_Click(object sender, EventArgs e)
        //{
        //    if (vtkControl.sphere_num > 0 && vtkControl.point_num > 0)
        //    {
        //        ren.RemoveActor(vtkControl.spheres[vtkControl.sphere_num - 1]);
        //        vtkControl.sphere_num -= 1;
        //        vtkControl.point_num -= 1;
        //    }
        //}

        private void change_color_test(vtkActor actor)
        {
            //用于测试的调用
            actor.GetProperty().SetColor(255, 0, 0);
            vtkControl.GetRenderWindow().Render();
        }
        private void button30_Click_1(object sender, EventArgs e)
        {
            if(vtkControl.sphere_num>0 && vtkControl.point_num>0)
            {
                ren.RemoveActor(vtkControl.spheres[vtkControl.sphere_num-1]);
                vtkControl.sphere_num -= 1;
                vtkControl.point_num -= 1;
            }
        }

        private void button22_Click_1(object sender, EventArgs e)
        {
            if(vtkControl.tube_num>0)
            {
                ren.RemoveActor(vtkControl.tubes[vtkControl.tube_num - 1]);
                vtkControl.tube_num -= 1;
            }
        }

        private void 撤销取点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (vtkControl.sphere_num > 0 && vtkControl.point_num > 0)
            {
                ren.RemoveActor(vtkControl.spheres[vtkControl.sphere_num - 1]);
                vtkControl.sphere_num -= 1;
                vtkControl.point_num -= 1;
            }
        }

        private void 撤销路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (vtkControl.tube_num > 0)
            {
                ren.RemoveActor(vtkControl.tubes[vtkControl.tube_num - 1]);
                vtkControl.tube_num -= 1;
            }
        }

        private void 删除所有点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < vtkControl.sphere_num; i++)
            {
                ren.RemoveActor(vtkControl.spheres[i]);
            }
            vtkControl.point_num = 0;
            vtkControl.sphere_num = 0;
            vtkControl.points = new double[20, 3];
        }

        private void 删除所有路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < vtkControl.tube_num; i++)
            {
                ren.RemoveActor(vtkControl.tubes[i]);
            }
            vtkControl.tube_num = 0;
        }

        //private void 选取靶点ToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    vtkControl.pick_tag = false;
        //    MessageBox.Show("请双击选取靶点");
        //    int[] color = new int[3];
        //    for (int i = 0; i < 3; i++)
        //    {
        //        color[i] = 1;
        //        vtkControl.color[i] = color[i];
        //    }
        //    int choiced = rander.Next(0, 3);
        //    color[choiced] = 0;
        //    vtkControl.color[choiced] = 0;
        //    vtkControl.begin = vtkControl.point_num;
        //    this.button1.BackColor = Color.FromArgb(color[0]*255, color[1]*255, color[2]*255);
        //}

        //private void 选取穿刺点ToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    vtkControl.pick_tag = false;
        //    MessageBox.Show("请双击选取穿刺点");
        //    int[] color = new int[3];
        //    for (int i = 0; i < 3; i++)
        //    {
        //        color[i] = 0;
        //        vtkControl.color[i] = color[i];
        //    }
        //    int choiced = rander.Next(0,3);
        //    color[choiced] = 1;
        //    vtkControl.color[choiced] = 1;
        //    vtkControl.end = vtkControl.point_num;
        //    this.button22.BackColor = Color.FromArgb(color[0]*255, color[1]*255, color[2]*255);
        //}

        private void button23_Click(object sender, EventArgs e)
        {
            vtkControl.pick_trigger = true;
            vtkControl.pick_tag = false;
            //MessageBox.Show("请双击选取靶点");
            int[] color = new int[3];
            for (int i = 0; i < 3; i++)
            {
                color[i] = 1;
                vtkControl.color[i] = color[i];
            }
            int choiced = rander.Next(0, 3);
            color[choiced] = 0;
            vtkControl.color[choiced] = 0;
            vtkControl.begin = vtkControl.point_num;
            this.button1.BackColor = Color.FromArgb(color[0] * 255, color[1] * 255, color[2] * 255);
        }

        private void button24_Click_1(object sender, EventArgs e)
        {
            vtkControl.pick_trigger = true;
            vtkControl.pick_tag = false;
            //MessageBox.Show("请双击选取穿刺点");
            int[] color = new int[3];
            for (int i = 0; i < 3; i++)
            {
                color[i] = 0;
                vtkControl.color[i] = color[i];
            }
            int choiced = rander.Next(0, 3);
            color[choiced] = 1;
            vtkControl.color[choiced] = 1;
            vtkControl.end = vtkControl.point_num;
            this.button22.BackColor = Color.FromArgb(color[0] * 255, color[1] * 255, color[2] * 255);
        }

        private void button27_Click(object sender, EventArgs e)
        {
            double distance=0;
            double[] begin_point = new double[3];
            double[] end_point = new double[3];
            for(int i=0;i<3;i++)
            {
                begin_point[i] = vtkControl.points[vtkControl.begin,i];
                end_point[i] = vtkControl.points[vtkControl.end, i];
                distance += Math.Pow((begin_point[i] - end_point[i]), 2);
            }
            distance = Math.Sqrt(distance);
            distance = distance / 2;
            MessageBox.Show(distance.ToString()+" mm");
        }

        private double Calculate_angle(double[] a,double[] b)
        {
            double dot_mul,a_dis,b_dis;
            double angle,cos_val;
            dot_mul = a_dis = b_dis = 0;
            for (int i = 0; i <3;i++)
            {
                dot_mul += a[i] * b[i];
                a_dis += Math.Pow(a[i],2);
                b_dis += Math.Pow(b[i],2);
            }
            a_dis = Math.Sqrt(a_dis);
            b_dis = Math.Sqrt(b_dis);
            cos_val = dot_mul / (a_dis * b_dis);
            angle = (180 * System.Math.Acos(cos_val)) / System.Math.PI;
            //Console.Write(cos_val + "\n");
            return angle;
        }

        private void button30_Click_2(object sender, EventArgs e)
        {
            Console.WriteLine("global  :  " + vtkControl.centery[0].ToString() + " " + vtkControl.centery[1].ToString() + " " + vtkControl.centery[2].ToString());
            double[] centery = new double[3];
            double[] xaxe = new double[3];
            double[] yaxe = new double[3];
            double[] zaxe = new double[3];
            double[] begin = new double[3];
            double[] end = new double[3];
            double[] sting = new double[3];
            double angle_x,angle_y,angle_z;
            xaxe = vtkControl.xaxe_point.GetCenter();
            yaxe = vtkControl.yaxe_point.GetCenter();
            zaxe = vtkControl.zaxe_point.GetCenter();
            for (int i=0;i<3;i++)
            {
                centery[i] = vtkControl.centery[i];
                begin[i] = vtkControl.points[vtkControl.begin,i];
                end[i] = vtkControl.points[vtkControl.end, i];
            }
            for(int i=0;i<3;i++)
            {
                xaxe[i] = xaxe[i] - 0;
                yaxe[i] = yaxe[i] - 0;
                zaxe[i] = zaxe[i] - 0;
                sting[i] = end[i] - begin[i];
                //Console.Write(xaxe[i].ToString()+"    "+ yaxe[i].ToString() + "    "+ zaxe[i].ToString() + "    ");
            }
            angle_x = Math.Round(this.Calculate_angle(sting, xaxe),3);
            angle_y = Math.Round(this.Calculate_angle(sting, yaxe),3);
            angle_z = Math.Round(this.Calculate_angle(sting, zaxe),3);
            this.textBox2.Font = new Font("黑体", 16, FontStyle.Regular);
            this.textBox2.Text = "X : " + angle_x.ToString() + Environment.NewLine +
                                 "Y : " + angle_y.ToString() + Environment.NewLine +
                                 "Z : " + angle_z.ToString() + Environment.NewLine
                                 ;
            //MessageBox.Show("X : " + angle_x.ToString() + "\nY : " + angle_y.ToString() + "\nZ : " + angle_z.ToString());
        }

        private void button34_Click(object sender, EventArgs e)
        {

        }
        private void clear_render()
        {
            for(int i=0;i<22;i++)
            {
                if (this.vtkActorAll[i] != null)
                {
                    ren.RemoveActor(this.vtkActorAll[i]);
                    vtkActorAll[i] = null;
                    GC.Collect();
                }
            }
            if(axes!=null)
            {
                ren.RemoveActor(axes);
                axes = null;
                GC.Collect();
            }
            //ren.Clear();
        }
    }
}
