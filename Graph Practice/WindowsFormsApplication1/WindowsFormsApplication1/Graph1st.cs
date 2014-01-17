using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Graph1st : Form
    {
        public Graph1st()
        {
            InitializeComponent();
        }
        
        // Respond to form 'Load' event 
        private void Graph1st_Load(object sender, EventArgs e)
        {
            // Setup the graph
            CreateGraph(zedGraphControl1);
            // Size the control to fill the form with a margin
           
        }

        // Respond to form 'Resize' event
        private void Graph1st_Resize(object sender, EventArgs e)
        {

        }

        private void CreateGraph(ZedGraph.ZedGraphControl zgc)
        {
            //get a reference to the graphpane
            ZedGraph.GraphPane myPane = zgc.GraphPane;

            myPane.Title.Text = "First Graph";

            // generate some data for the graph
            double x, y1, y2;
            ZedGraph.PointPairList list1 = new ZedGraph.PointPairList();
            ZedGraph.PointPairList list2 = new ZedGraph.PointPairList();
            
            for( int i=0; i<36; i++)
            {
                x = (double)i+5;
                y1 = 1.5*Math.Sin((double)i*0.2);
                y2 = 1.5+(3.0*Math.Sin((double)i*0.2));
                list1.Add(x, y1);
                list2.Add(x, y2);

            }

            ZedGraph.LineItem myCurve = myPane.AddCurve("Seth", list1, Color.Red, ZedGraph.SymbolType.Diamond);
            ZedGraph.LineItem myCurve2 = myPane.AddCurve("Obi", list2, Color.Blue, ZedGraph.SymbolType.Circle);

            zgc.AxisChange();


        }
        /*Additional function to add values to a graph
        private void AddDataToGraph(ZedGraphControl zgc, double x, double[] yValues)
        {
            GraphPane mypane = zgc.GraphPane;

            for (int i; i < yValues.Length; i++)
            {
                list1.Add(x, yValues[i]);/*Something to add values of array to list. All values will be added at same x val. 
                                            Add is a member of PointPairList and so can be used with list1 which was created
                                           by a PointPairList 
            }

            zgc.Invalidate(); //Used to force redraw of graph. 
        }*/
    }
}
