using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using System.Xml;
using System.Diagnostics;

namespace GanJian0609ERSWindowsFormsApp
{
    public partial class EmployeeRecordsForm : Form
    {
        private TreeNode tvRootNode;

        public EmployeeRecordsForm()
        {
            InitializeComponent();
            PopulateTreeView();
        }



        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void EmployeeRecordsForm_Load(object sender, EventArgs e)
        {

        }
        protected void PopulateTreeView()
        {
            statusBarPanel1.Tag ="Refreshing Employee Codes.Please wait...";
            this.Cursor = Cursors.WaitCursor;
            treeView1.Nodes.Clear();
            tvRootNode = new TreeNode("Employee Records");
            this.Cursor = Cursors.Default;
            treeView1.Nodes.Add(tvRootNode);
            TreeNodeCollection nodeCollection = tvRootNode.Nodes;
            string strVal ="";
            XmlTextReader reader = new XmlTextReader("E:\\oasdr\\source\\repos\\GanJian0609ERSWindowsFormsApp\\EmpRec.xml");
            reader.MoveToElement();
            try
            {
                while(reader.Read())
                {
                    if (reader.HasAttributes && reader.NodeType == XmlNodeType.Element)
                    {
                        reader.MoveToElement();
                        reader.MoveToElement();
                        reader.MoveToAttribute(  "Id"  );
                        strVal= reader.Value;
                        reader.Read();
                        reader.Read();
                        if (reader.Name == "Dept") 
                        {
                            reader.Read();
                        }
                        //create the child nodes
                        TreeNode EcodeNode = new TreeNode(strVal);
                    // Add the Node 
                        nodeCollection.Add(EcodeNode);
                    }
                }
                statusBarPanel1.Tag ="Click on an employee code to see their record.";
            }
               catch(XmlException ex)
            {
                MessageBox.Show(ex.Message);
               // MessageBox.Show("XML Exception:"+e.ToString());
            }
        }
        protected void initializeListControl()
        {
            listView1.Clear();
            listView1.Columns.Add("Employee Name", 225, HorizontalAlignment.Left);
            listView1.Columns.Add("Date of Join", 70, HorizontalAlignment.Right);
            listView1.Columns.Add("Grade", 105, HorizontalAlignment.Left);
            listView1.Columns.Add("Salary", 105, HorizontalAlignment.Left);
        }
        protected void PopulateListView(TreeNode currNode)
        {
            initializeListControl();
            XmlTextReader listRead = new XmlTextReader("E:\\oasdr\\source\\repos\\GanJian0609ERSWindowsFormsApp\\EmpRec.xml");
            listRead.MoveToElement();
            while(listRead.Read())
{
                string strNodename;
                string strNodePath;
                string name;
                string grade;
                string doj;
                string sal;
                string[] strItemsArr = new String[4];
                listRead.MoveToFirstAttribute();
                strNodename=listRead.Value;
                strNodePath=currNode.FullPath.Remove(0,17) ;
                if(strNodePath==strNodename)
                {
                    ListViewItem lvi;
                    listRead.MoveToNextAttribute();
                    name = listRead.Value;
                    lvi = listView1.Items.Add(name);
                    listRead.Read();
                    listRead.Read();
                    listRead.MoveToFirstAttribute();
                    doj=listRead.Value;
                    lvi.SubItems.Add(doj);
                    listRead.MoveToNextAttribute();
                    grade = listRead.Value;
                    lvi.SubItems.Add(grade);
                    listRead.MoveToNextAttribute();
                    sal = listRead.Value;
                    lvi.SubItems.Add(sal);
                    listRead.MoveToNextAttribute();
                    listRead.MoveToElement();
                    listRead.ReadString();
                }
            }
        }
        private void treeView1_AfterSelect(object sender, System.Windows.Forms
        .TreeViewEventArgs e)
        {
            TreeNode currNode = e.Node;
            if (tvRootNode == currNode)
            {
                initializeListControl();
                statusBarPanel1.Text ="Double click the Employee Records";
                return;
            }
            else
        {
                statusBarPanel1.Text ="Click an employee code to view individual records";
            }
            PopulateListView(currNode);
        }
    }
}
