using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using System.IO;
using vtk;

namespace vtkPointCloud
{
    class myControl : vtkFormsWindowControl
    {
        public int point_num;
        public int sphere_num;
        public int tube_num;
        public double[,] points;
        public int[] color;
        public int begin;
        public int end;
        public double[] centery;
        public bool pick_tag;
        public bool pick_trigger;
        //把用于体现取点小圆球的actor放在control里面
        public vtkActor[] spheres = new vtkActor[6];
        public vtkActor[] tubes = new vtkActor[3];
        public vtkRenderer control_renderer = null;
        public vtkActor xaxe_point = new vtkActor();
        public vtkActor yaxe_point = new vtkActor();
        public vtkActor zaxe_point = new vtkActor();
        public List<vtkActor> axe_points = new List<vtkActor>();
        public myControl()
        {
            this.point_num = 0;
            this.sphere_num = 0;
            this.tube_num = 0;
            this.points = new double[20,3];
            this.color = new int[3];
            this.centery = new double[3];
            this.pick_tag = false;
            this.pick_trigger = false;
            this.axe_points.Add(xaxe_point);
            this.axe_points.Add(yaxe_point);
            this.axe_points.Add(zaxe_point);
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if(pick_trigger==false)
            {
                MessageBox.Show("请先选择靶点或穿刺点！");
                return;
            }
            if(pick_tag==true)
            {
                control_renderer.RemoveActor(this.spheres[this.sphere_num - 1]);
                this.point_num -= 1;
                this.sphere_num -= 1;
            }
            if (this.sphere_num >= 6)
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("点数已满，是否清空？", "系统提示", messButton);
                if (dr == DialogResult.OK)
                {
                    for (int i = 0; i < this.sphere_num; i++)
                    {
                        control_renderer.RemoveActor(this.spheres[i]);
                        this.points = new double[20, 3];
                    }
                    this.point_num = 0;
                    this.sphere_num = 0;
                }
                return;
            }
            //vtkRenderWindowInteractor inter_ren = this.GetInteractor();
            Console.WriteLine(this.point_num.ToString());
            double[] temp = new double[2];
            double[] picked = new double[3];
            temp[0] = this.GetInteractor().GetEventPosition()[0];
            temp[1] = this.GetInteractor().GetEventPosition()[1];
            Console.WriteLine("picked position" + temp[0].ToString() + " , " + temp[1].ToString());
            this.GetInteractor().GetPicker().Pick(temp[0], temp[1], 0, this.GetInteractor().GetRenderWindow().GetRenderers().GetFirstRenderer());
            picked = this.GetInteractor().GetPicker().GetPickPosition();
            Console.WriteLine("picked point:" + picked[0].ToString() + " , " + picked[1].ToString() + " , " + picked[2].ToString());
            for(int i=0;i<3;i++)
            {
                this.points[point_num, i] = picked[i];
            }
            this.point_num++;
            base.OnMouseDoubleClick(e);

            //import a sphere object into scence
            vtkSphereSource sphere = new vtkSphereSource();
            sphere.SetCenter(picked[0],picked[1],picked[2]);
            sphere.SetRadius(3.0);

            vtkPolyDataMapper mapper = new vtkPolyDataMapper();
            mapper.SetInputConnection(sphere.GetOutputPort());

            vtkProperty property = new vtkProperty();
            property.SetColor(color[0], color[1], color[2]);
            property.SetOpacity(1);

            this.spheres[this.sphere_num] = new vtkActor();
            this.spheres[this.sphere_num].SetMapper(mapper);
            this.spheres[this.sphere_num].SetProperty(property);

            this.control_renderer.AddActor(this.spheres[this.sphere_num]);

            this.sphere_num++;
            this.pick_tag = true;
            //点数越界后弹窗确定是否清空

        }
        //protected override void OnMouseClick(MouseEventArgs e)
        //{
        //    Console.WriteLine("Single Click!!\n");
        //    base.OnMouseClick(e);
        //}
    }
}
