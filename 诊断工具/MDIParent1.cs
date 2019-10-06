using Phenom.Extension;
using Phenom.UI;
using Phenom.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using static 诊断工具.AutoLoadTemplate;

namespace 诊断工具
{
    public partial class MDIParent1 : Form
    {
        [DllImport("user32.dll", EntryPoint = "SetParent")]
        public extern static IntPtr SetParent(IntPtr childPtr, IntPtr parentPtr);

        [STAThread]
        private static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(true);
                Application.Run(new MDIParent1());
            }
            catch (Exception ex)
            {
                new ErrorHandler(ex).CreateWPFWindow().ShowDialog();
            }
        }

        public MDIParent1()
        {
            InitializeComponent();
        }

        ///private readonly Dictionary<ToolStripMenuItem, Form> FormCollection = new Dictionary<ToolStripMenuItem, Form>();
        private readonly Dictionary<ToolStripMenuItem, Type> FormCollection = new Dictionary<ToolStripMenuItem, Type>();

        private readonly Dictionary<CateLogType, ToolStripMenuItem> Catelog = new Dictionary<CateLogType, ToolStripMenuItem>();

        private void MDIParent1_Load(object sender, EventArgs e)
        {
            foreach (Type i in AppDomain.CurrentDomain.ScanType(value => value.GetCustomAttribute<AutoLoadTemplate>() != null, value => true))
            {
                AutoLoadTemplate template = i.GetCustomAttribute<AutoLoadTemplate>();
                if (!Catelog.ContainsKey(template.Catalog))
                {
                    ToolStripMenuItem item = new ToolStripMenuItem
                    {
                        Text = template.Catalog.EnumGetFieldAttribute<DescriptionAttribute>()?.Description ?? template.Catalog.ToString(),
                    };
                    Catelog[template.Catalog] = item;
                    menuStrip.Items.Add(item);
                }
                ToolStripMenuItem menu = new ToolStripMenuItem()
                {
                    Text = template.TabName
                };
                Catelog[template.Catalog].DropDownItems.Add(menu);
                FormCollection[menu] = i;
                menu.Click += Menu_Click;
            }
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            Type type = FormCollection[sender as ToolStripMenuItem];
            AutoLoadTemplate template = type.GetCustomAttribute<AutoLoadTemplate>();
            Form window = new Form
            {
                Text = template.TabName,
                MdiParent = this,
                WindowState = FormWindowState.Maximized
            };
            System.Windows.Controls.UserControl ctl = Activator.CreateInstance(type) as System.Windows.Controls.UserControl;
            ctl.Margin = new System.Windows.Thickness(0, 0, 0, 0);
            window.Controls.Add(new ElementHost
            {
                Child = ctl,
                Dock = DockStyle.Fill,
            });
            window.Show();
        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            (sender as Form).Hide();
        }
    }
}