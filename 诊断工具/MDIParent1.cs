using Phenom.Extension;
using Phenom.UI;
using Phenom.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using static 诊断工具.AutoLoadTemplate;

namespace 诊断工具
{
    public partial class MDIParent1 : Form
    {
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

        private readonly Dictionary<ToolStripMenuItem, Form> FormCollection = new Dictionary<ToolStripMenuItem, Form>();

        private readonly Dictionary<CateLogType, ToolStripMenuItem> Catelog = new Dictionary<CateLogType, ToolStripMenuItem>();

        private void MDIParent1_Load(object sender, EventArgs e)
        {
            foreach (Type i in AppDomain.CurrentDomain.ScanType(value => value.GetCustomAttribute<AutoLoadTemplate>() != null, value => true))
            {
                System.Windows.Controls.UserControl catelog = Activator.CreateInstance(i) as System.Windows.Controls.UserControl;
                catelog.Margin = new System.Windows.Thickness(0, 0, 0, 0);
                AutoLoadTemplate template = catelog.GetType().GetCustomAttribute<AutoLoadTemplate>();
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
                Form window = new Form
                {
                    Text = template.TabName,
                    MdiParent = this,
                    WindowState = FormWindowState.Maximized
                };
                window.Controls.Add(new ElementHost
                {
                    Child = catelog,
                    Dock = DockStyle.Fill
                });
                window.FormClosing += Window_FormClosing;
                FormCollection[menu] = window;
                menu.Click += Menu_Click;
            }
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            Form form = FormCollection[sender as ToolStripMenuItem];
            form.Show();
            form.TopMost = true;
            form.WindowState = FormWindowState.Maximized;
        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            (sender as Form).Hide();
        }
    }
}