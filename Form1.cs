using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using vtk;

namespace vtkPointCloud
{

    public partial class Form1 : Form
    {
        vtkPoints pts = new vtkPoints();
        vtkCellArray strip = new vtkCellArray();

        vtkPolyData poly = new vtkPolyData();                          // polydata
        vtkPolyDataMapper mapper = new vtkPolyDataMapper(); // mapper
        vtkActor actor = new vtkActor();                                 // actor

        vtkRenderer ren = new vtkRenderer();                         // renderer

        vtkFormsWindowControl vtkControl = null;

        vtkConeSource cone = new vtkConeSource();// 添加一个 Soure Object


        public Form1()
        {
            InitializeComponent();


            vtkControl = new vtkFormsWindowControl();
            vtkControl.Location = new Point(30, 30);
            vtkControl.Name = "vtkControl";
            vtkControl.TabIndex = 0;
            vtkControl.Text = "vtkFormsWindowControl";
            vtkControl.Dock = DockStyle.Fill;
            this.Controls.Add(vtkControl);

            PipeLine();
        }

        // 渲染管道
        void PipeLine()
        {
            // 把Soure Object, mapper, actor, renderer连接起来
            mapper.SetInput(cone.GetOutput());
            actor.SetMapper(mapper);
            ren.AddActor(actor);
            vtkControl.GetRenderWindow().AddRenderer(ren);
        }

    }
}
