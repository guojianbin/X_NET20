﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using NewLife.Reflection;

namespace XCoder
{
    public partial class FrmMDI : Form
    {
        #region 窗口初始化
        public FrmMDI()
        {
            InitializeComponent();

            this.Icon = Source.GetIcon();
        }

        private void FrmMDI_Shown(object sender, EventArgs e)
        {
            var set = XConfig.Current;
            if (set.Width > 0 || set.Height > 0)
            {
                this.Width = set.Width;
                this.Height = set.Height;
                this.Top = set.Top;
                this.Left = set.Left;
            }

            var asm = AssemblyX.Create(Assembly.GetExecutingAssembly());
            if (set.Title.IsNullOrEmpty()) set.Title = asm.Title;
            Text = String.Format("{2} v{0} {1:HH:mm:ss}", asm.CompileVersion, asm.Compile, set.Title);

            //var name = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            var name = XConfig.Current.LastTool + "";
            switch (name.ToLower())
            {
                case "xcoder":
                    CreateForm<FrmMain>();
                    break;
                case "xcom":
                case "com":
                    CreateForm<XCom.FrmMain>();
                    break;
                case "xregex":
                case "regex":
                    CreateForm<NewLife.XRegex.FrmMain>();
                    break;
                case "xico":
                case "ico":
                    CreateForm<XICO.FrmMain>();
                    break;
                default:
                    //CreateForm<FrmMain>();
                    break;
            }
        }
        #endregion

        #region 应用窗口
        void CreateForm<TForm>() where TForm : Form, new()
        {
            // 倒数第二段
            var ss = typeof(TForm).FullName.Split(".");
            if (ss.Length >= 2)
            {
                var name = ss[ss.Length - 2];
                var cfg = XConfig.Current;
                if (name != cfg.LastTool)
                {
                    cfg.LastTool = name;
                    cfg.Save();
                }
            }

            var frm = new TForm();
            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        private void 数据建模工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateForm<FrmMain>();
        }

        private void 正则表达式工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateForm<NewLife.XRegex.FrmMain>();
        }

        private void 通讯调试工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateForm<XCom.FrmMain>();
        }

        private void 图标水印处理工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateForm<XICO.FrmMain>();
        }

        private void 网络调试工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateForm<XNet.FrmMain>();
        }

        private void 文件夹大小统计ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateForm<FolderInfo.FrmMain>();
        }
        private void 文件编码格式替换工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateForm<FolderInfo.FrmEncodeReplace>();
        }
        #endregion

        #region 菜单控制
        private void ShowNewForm(object sender, EventArgs e) { }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e) { LayoutMdi(MdiLayout.Cascade); }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e) { LayoutMdi(MdiLayout.TileVertical); }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e) { LayoutMdi(MdiLayout.TileHorizontal); }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e) { LayoutMdi(MdiLayout.ArrangeIcons); }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
        #endregion

        private void FrmMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            var set = XConfig.Current;
            //if (set.Width == 0 || set.Height == 0)
            {
                set.Width = this.Width;
                set.Height = this.Height;
                set.Top = this.Top;
                set.Left = this.Left;
                set.Save();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.NewLifeX.com");
        }
    }
}