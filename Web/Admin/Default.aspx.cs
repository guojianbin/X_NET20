﻿using System;
using System.Collections.Generic;
using NewLife.Common;
using NewLife.CommonEntity;
using XCode;
using XCode.Membership;

public partial class Admin_Default : System.Web.UI.Page
{
    private String _DefaultLeft = "Frame/Left.aspx";
    /// <summary>默认左菜单</summary>
    public String DefaultLeft { get { return _DefaultLeft; } set { _DefaultLeft = value; } }

    private String _DefaultMain = "Main.aspx";
    /// <summary>默认内容页</summary>
    public String DefaultMain { get { return _DefaultMain; } set { _DefaultMain = value; } }

    protected IUser Current { get { return ManageProvider.Provider.Current as IUser; } }

    /// <summary>系统配置。如果重载，修改这里即可。</summary>
    public static SysConfig Config { get { return SysConfig.Current; } }

    private IList<IMenu> _Menus = new List<IMenu>();
    /// <summary>菜单</summary>
    public IList<IMenu> Menus { get { return _Menus; } set { _Menus = value; } }

    protected override void OnPreLoad(EventArgs e)
    {
        base.OnPreLoad(e);

        IManageUser user = ManageProvider.Provider.Current;
        if (user == null) Response.Redirect("Login.aspx");

        IManageProvider provider = ManageProvider.Provider;
        IMenu root = ManageProvider.Menu.Root;

        IUser admin = user as IUser;
        if (admin == null)
        {
            if (root != null)
            {
                Menus = root.Childs;
                //menuItem.DataSource = root.Childs;
                //menuItem.DataBind();
            }
            return;
        }

        if (Request["act"] == "logout")
        {
            admin.Logout();
            // 再跳一次，除去Url中的尾巴
            if (!String.IsNullOrEmpty(Request.Url.Query)) Response.Redirect("Default.aspx");
        }

        if (admin.Role != null)
        {
            //List<IMenu> list = admin.Role.GetMySubMenus(root.ID);
            IList<IMenu> list = ManageProvider.Menu.GetMySubMenus(root.ID);
            Menus = list;
            //menuItem.DataSource = list;
            //menuItem.DataBind();

            if (list != null && list.Count > 0)
            {
                IMenu first = list[0];
                DefaultLeft = String.Format("Frame/Left.aspx?ID={0}", first.ID);
                if (!String.IsNullOrEmpty(first.Url)) DefaultMain = first.Url;
            }
        }

        #region 自动修正菜单
        // 自动修正菜单中英文
        if (root != null)
        {
            using (EntityTransaction trans = new EntityTransaction(EntityFactory.CreateOperate(root.GetType())))
            {
                //root.CheckMenuName("Admin", "管理平台")
                //    .CheckMenuName(@"Admin\Sys", "系统管理")
                //    .CheckMenuName(@"Admin\Advance", "高级设置")
                //    .CheckMenuName(@"Admin\Help", "帮助手册");

                // 自动挂载Main.aspx
                IMenu menu = root.FindByPath("Admin");
                if (menu != null && menu.Url == "../Admin/Default.aspx")
                {
                    menu.Url = "../Admin/Main.aspx";
                    menu.Save();
                }
                if (menu == null) menu = root;
                if (menu != null)
                {
                    #region 自动排序
                    IMenu menu2 = menu.FindByPath("Sys");
                    if (menu2 != null)
                    {
                        menu2.Sort = 3;
                        menu2.Save();
                    }
                    menu2 = menu.FindByPath("Advance");
                    if (menu2 != null)
                    {
                        menu2.Sort = 2;
                        menu2.Save();
                    }
                    menu2 = menu.FindByPath("Help");
                    if (menu2 != null)
                    {
                        menu2.Sort = 1;
                        menu2.Save();
                    }
                    #endregion
                }

                trans.Commit();
            }
        }
        #endregion
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //this.Title = Config.DisplayName;
    }

    String[] icos = new String[] { "fa-tachometer", "fa-desktop", "fa-list", "fa-pencil-square-o", "fa-list-alt", "fa-calendar", "fa-picture-o", "fa-tag", "fa-file-o" };
    Int32 _idx = 0;
    public String GetIco()
    {
        return icos[_idx++];
    }
}