#region

using System;
using System.IO;
using System.Web.UI.WebControls;
using Cross.Modules.ChildPageList.Common;
using DotNetNuke.Abstractions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Exceptions;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace Cross.Modules.ChildPageList
{
    public partial class Settings : PortalModuleBase
    {
        private readonly INavigationManager _navigationManager;

        public Settings()
        {
            _navigationManager = DependencyProvider.GetRequiredService<INavigationManager>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ddlParentTab.DataSource = TabController.GetPortalTabs(PortalSettings.PortalId, -1, true, false);
                    ddlParentTab.DataBind();
                    if (ModuleConfiguration.ModuleSettings[Constants.PARENT_TAB] != null && Convert.ToString(ModuleConfiguration.ModuleSettings[Constants.PARENT_TAB]) != "")
                    {
                        string parentTab = Convert.ToString(ModuleConfiguration.ModuleSettings[Constants.PARENT_TAB]);
                        if (ddlParentTab.Items.FindByValue(parentTab) != null)
                        {
                            ddlParentTab.SelectedValue = parentTab;
                        }
                    }
                    else
                    {
                        ddlParentTab.SelectedValue = TabId.ToString();
                    }

                    if (ModuleConfiguration.ModuleSettings[Constants.INCLUDE_SELF] != null && (string) ModuleConfiguration.ModuleSettings[Constants.INCLUDE_SELF] != "")
                    {
                        chkIncludeSelf.Checked = Convert.ToBoolean(Convert.ToString(ModuleConfiguration.ModuleSettings[Constants.INCLUDE_SELF]));
                    }

                    if (ModuleConfiguration.ModuleSettings[Constants.INCLUDE_INVISIBLE] != null && (string) ModuleConfiguration.ModuleSettings[Constants.INCLUDE_INVISIBLE] != "")
                    {
                        chkIncludeInvisible.Checked = Convert.ToBoolean(Convert.ToString(ModuleConfiguration.ModuleSettings[Constants.INCLUDE_INVISIBLE]));
                    }

                    if (ModuleConfiguration.ModuleSettings[Constants.RECURSIVE] != null && (string) ModuleConfiguration.ModuleSettings[Constants.RECURSIVE] != "")
                    {
                        chkRecursive.Checked = Convert.ToBoolean(Convert.ToString(ModuleConfiguration.ModuleSettings[Constants.RECURSIVE]));
                    }

                    if (ModuleConfiguration.ModuleSettings[Constants.DISPLAY_ICON] != null && (string) ModuleConfiguration.ModuleSettings[Constants.DISPLAY_ICON] != "")
                    {
                        chkDisplayIcon.Checked = Convert.ToBoolean(Convert.ToString(ModuleConfiguration.ModuleSettings[Constants.DISPLAY_ICON]));
                    }

                    if (ModuleConfiguration.ModuleSettings[Constants.COLUMN_PER_ROW] != null && (string) ModuleConfiguration.ModuleSettings[Constants.COLUMN_PER_ROW] != "")
                    {
                        txtColumnCount.Text = Convert.ToString(ModuleConfiguration.ModuleSettings[Constants.COLUMN_PER_ROW]);
                    }

                    string defaultListTemplate = Constants.DEFAULT_LIST_TEMPLATE;
                    if (ModuleConfiguration.ModuleSettings[Constants.LIST_TEMPLATE] != null && (string) ModuleConfiguration.ModuleSettings[Constants.LIST_TEMPLATE] != "")
                    {
                        defaultListTemplate = Convert.ToString(ModuleConfiguration.ModuleSettings[Constants.LIST_TEMPLATE]);
                    }

                    ddlListTemplate = FillSubDirectoryTemplate(ddlListTemplate, Request.MapPath(ControlPath + Constants.LIST_TEMPLATE_PATH), defaultListTemplate, "*.ascx");

                    if (ModuleConfiguration.ModuleSettings[Constants.LINK_TARGET] != null && Convert.ToString(ModuleConfiguration.ModuleSettings[Constants.LINK_TARGET]) != "")
                    {
                        string linkTarget = Convert.ToString(ModuleConfiguration.ModuleSettings[Constants.LINK_TARGET]);
                        if (ddlLinkTarget.Items.FindByValue(linkTarget) != null)
                        {
                            ddlLinkTarget.SelectedValue = linkTarget;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private DropDownList FillSubDirectoryTemplate(DropDownList ddlTemplate, string path, string selectedValue, string fileExtension)
        {
            const string separator = "/";

            if (Directory.Exists(path))
            {
                var folders = Directory.GetDirectories(path);
                foreach (string folder in folders)
                {
                    var files = Directory.GetFiles(folder, fileExtension);
                    foreach (string file in files)
                    {
                        string folderName = Path.GetFileName(Path.GetDirectoryName(file));

                        var item = new ListItem {Text = folderName + separator + Path.GetFileNameWithoutExtension(file), Value = folderName + separator + Path.GetFileName(file)};
                        if (item.Value == selectedValue)
                        {
                            item.Selected = true;
                        }

                        ddlTemplate.Items.Add(item);
                    }
                }
            }

            return ddlTemplate;
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            ModuleController ctlModule = new ModuleController();

            ctlModule.UpdateModuleSetting(ModuleId, Constants.PARENT_TAB, ddlParentTab.SelectedValue);
            ctlModule.UpdateModuleSetting(ModuleId, Constants.INCLUDE_SELF, chkIncludeSelf.Checked.ToString());
            ctlModule.UpdateModuleSetting(ModuleId, Constants.INCLUDE_INVISIBLE, chkIncludeInvisible.Checked.ToString());
            ctlModule.UpdateModuleSetting(ModuleId, Constants.RECURSIVE, chkRecursive.Checked.ToString());
            ctlModule.UpdateModuleSetting(ModuleId, Constants.RECURSIVE, chkDisplayIcon.Checked.ToString());
            ctlModule.UpdateModuleSetting(ModuleId, Constants.COLUMN_PER_ROW, txtColumnCount.Text);
            ctlModule.UpdateModuleSetting(ModuleId, Constants.LIST_TEMPLATE, ddlListTemplate.SelectedValue);
            ctlModule.UpdateModuleSetting(ModuleId, Constants.LINK_TARGET, ddlLinkTarget.SelectedValue);

            Response.Redirect(_navigationManager.NavigateURL(TabId), true);
        }

        protected void cmdReturn_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(_navigationManager.NavigateURL(TabId), true);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}