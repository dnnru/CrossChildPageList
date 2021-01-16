using System;
using System.Collections.Generic;
using System.Linq;
using Cross.Modules.ChildPageList.Common;
using DotNetNuke.Abstractions;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace Cross.Modules.ChildPageList.Template
{
    public partial class ListStandard : PortalModuleBase
    {
        public INavigationManager NavigationManager { get; }

        public ListStandard()
        {
            NavigationManager = DependencyProvider.GetRequiredService<INavigationManager>();
        }
        public int ColumnPerRow
        {
            get
            {
                bool isRecursive = false;
                if (ModuleConfiguration.ModuleSettings[Constants.RECURSIVE] != null && (string)ModuleConfiguration.ModuleSettings[Constants.RECURSIVE] != "")
                {
                    isRecursive = Convert.ToBoolean(ModuleConfiguration.ModuleSettings[Constants.RECURSIVE]);
                }

                if (isRecursive)
                {
                    return 1;
                }

                if (ModuleConfiguration.ModuleSettings[Constants.COLUMN_PER_ROW] != null && (string)ModuleConfiguration.ModuleSettings[Constants.COLUMN_PER_ROW] != "")
                {
                    return Convert.ToInt32(ModuleConfiguration.ModuleSettings[Constants.COLUMN_PER_ROW]);
                }

                return 1;
            }
        }

        public string LinkTarget
        {
            get
            {
                if (ModuleConfiguration.ModuleSettings[Constants.LINK_TARGET] != null && (string)ModuleConfiguration.ModuleSettings[Constants.LINK_TARGET] != "")
                {
                    return Convert.ToString(ModuleConfiguration.ModuleSettings[Constants.LINK_TARGET]);
                }

                return "_self";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LocalResourceFile = Localization.GetResourceFile(this, "ListStandard.ascx");
                int parentTab = TabId;

                if (ModuleConfiguration.ModuleSettings[Constants.PARENT_TAB] != null && Convert.ToString(ModuleConfiguration.ModuleSettings[Constants.PARENT_TAB]) != "")
                {
                    parentTab = Convert.ToInt32(ModuleConfiguration.ModuleSettings[Constants.PARENT_TAB]);
                }

                TabController ctlTab = new TabController();
                TabInfo objParent = ctlTab.GetTab(parentTab, PortalId);

                if (parentTab != Null.NullInteger && (objParent == null || !objParent.HasChildren))
                {
                    return;
                }

                bool includeSelf = false;
                if (ModuleConfiguration.ModuleSettings[Constants.INCLUDE_SELF] != null && (string)ModuleConfiguration.ModuleSettings[Constants.INCLUDE_SELF] != "")
                {
                    includeSelf = Convert.ToBoolean(ModuleConfiguration.ModuleSettings[Constants.INCLUDE_SELF]);
                }

                bool includeInvisible = false;
                if (ModuleConfiguration.ModuleSettings[Constants.INCLUDE_INVISIBLE] != null && (string)ModuleConfiguration.ModuleSettings[Constants.INCLUDE_INVISIBLE] != "")
                {
                    includeInvisible = Convert.ToBoolean(ModuleConfiguration.ModuleSettings[Constants.INCLUDE_INVISIBLE]);
                }

                bool isRecursive = false;
                if (ModuleConfiguration.ModuleSettings[Constants.RECURSIVE] != null && (string)ModuleConfiguration.ModuleSettings[Constants.RECURSIVE] != "")
                {
                    isRecursive = Convert.ToBoolean(ModuleConfiguration.ModuleSettings[Constants.RECURSIVE]);
                }

                bool isDisplayIcon = false;
                if (ModuleConfiguration.ModuleSettings[Constants.DISPLAY_ICON] != null && (string)ModuleConfiguration.ModuleSettings[Constants.DISPLAY_ICON] != "")
                {
                    isDisplayIcon = Convert.ToBoolean(ModuleConfiguration.ModuleSettings[Constants.DISPLAY_ICON]);
                }

                List<TabInfo> arrTab = new List<TabInfo>();

                if (isRecursive)
                {
                    arrTab = GetChildPageList(parentTab, parentTab == Null.NullInteger ? -1 : objParent.Level, includeSelf, includeInvisible, isDisplayIcon);
                }
                else
                {
                    List<TabInfo> tabList = new List<TabInfo>();
                    if (parentTab == Null.NullInteger)
                    {
                        tabList.AddRange(ctlTab.GetTabsByPortal(PortalId).AsList().Where(tabInfo => tabInfo.Level == 0 && tabInfo.ParentId == -1));
                    }
                    else
                    {
                        tabList = TabController.GetTabsByParent(parentTab, PortalId);
                    }

                    foreach (TabInfo tabInfo in tabList)
                    {
                        if (PortalSecurity.IsInRoles(tabInfo.TabPermissions.ToString("VIEW")) && (!tabInfo.IsDeleted) && (!tabInfo.DisableLink))
                        {
                            if (tabInfo.IsVisible || includeInvisible)
                            {
                                if (tabInfo.TabID != TabId || includeSelf)
                                {
                                    TabInfo childTab = tabInfo.Clone();
                                    string iconUrl = "";
                                    if (isDisplayIcon)
                                    {
                                        iconUrl = string.IsNullOrEmpty(childTab.IconFile) ? $"<img src='{Page.ResolveUrl("~/images/icon_unknown_16px.gif")}' border='0' width='16px' height='16px'/>" : $"<img src='{Page.ResolveUrl(childTab.IconFile)}' border='0' width='16px' height='16px'/>";
                                    }

                                    childTab.TabName = iconUrl + tabInfo.TabName;
                                    arrTab.Add(childTab);
                                }
                            }
                        }
                    }
                }

                if (arrTab.Count > 0)
                {
                    dlSubTabList.DataSource = arrTab;
                    dlSubTabList.DataBind();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private List<TabInfo> GetChildPageList(int parentTabId, int parentTabLevel, bool includeSelf, bool includeInvisible, bool isDisplayIcon)
        {
            List<TabInfo> arrTab = new List<TabInfo>();
            List<TabInfo> tabList = new List<TabInfo>();
            TabController ctlTab = new TabController();
            if (parentTabId == Null.NullInteger)
            {
                tabList.AddRange(ctlTab.GetTabsByPortal(PortalId).AsList().Where(tabInfo => tabInfo.Level == 0 && tabInfo.ParentId == -1));
            }
            else
            {
                tabList = TabController.GetTabsByParent(parentTabId, PortalId);
            }

            if (parentTabId != Null.NullInteger)
            {
                TabInfo objParent = ctlTab.GetTab(parentTabId, PortalId);
                if (objParent == null || !objParent.HasChildren)
                {
                    return arrTab;
                }
            }

            foreach (TabInfo tabInfo in tabList)
            {
                if (PortalSecurity.IsInRoles(tabInfo.TabPermissions.ToString("VIEW")) && (!tabInfo.IsDeleted) && (!tabInfo.DisableLink))
                {
                    if (tabInfo.IsVisible || includeInvisible)
                    {
                        if (tabInfo.TabID != TabId || includeSelf)
                        {
                            TabInfo childTab = tabInfo.Clone();
                            int levelDiff = tabInfo.Level - parentTabLevel;
                            string prefix = "";
                            for (int i = 2; i < levelDiff; i++)
                            {
                                prefix += $"<img src='{Page.ResolveUrl("~/desktopmodules/CrossChildPageList/images/line.gif")}' border='0'/>";
                            }

                            if (levelDiff > 1)
                            {
                                prefix += $"<img src='{Page.ResolveUrl("~/desktopmodules/CrossChildPageList/images/node.gif")}' border='0'/>";
                            }

                            string iconUrl = "";
                            if (isDisplayIcon)
                            {
                                iconUrl = string.IsNullOrEmpty(childTab.IconFile)
                                              ? $"<img src='{Page.ResolveUrl("~/images/icon_unknown_16px.gif")}' border='0' width='16px' height='16px'/>"
                                              : $"<img src='{Page.ResolveUrl(childTab.IconFile)}' border='0' width='16px' height='16px'/>";
                            }

                            childTab.TabName = prefix + iconUrl + tabInfo.TabName;
                            arrTab.Add(childTab);
                            List<TabInfo> subTabArr = GetChildPageList(tabInfo.TabID, parentTabLevel, includeSelf, includeInvisible, isDisplayIcon);
                            arrTab.AddRange(subTabArr);
                        }
                    }
                }
            }

            return arrTab;
        }

    }
}