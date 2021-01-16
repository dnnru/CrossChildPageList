#region

using System;
using Cross.Modules.ChildPageList.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

#endregion

namespace Cross.Modules.ChildPageList
{
    public partial class View : PortalModuleBase, IActionable
    {
        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actions = new ModuleActionCollection
                                                 {
                                                     {
                                                         GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, LocalResourceFile),
                                                         ModuleActionType.AddContent, "",
                                                         "", EditUrl(), false, SecurityAccessLevel.Edit, true, false
                                                     },
                                                     {
                                                         GetNextActionID(), Localization.GetString("OnlineHelp.Text", LocalResourceFile),
                                                         ModuleActionType.OnlineHelp, "", "",
                                                         "http://www.DnnModules.cn", false, SecurityAccessLevel.Edit, true, true
                                                     }
                                                 };
                return actions;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string defaultListTemplate = Constants.DEFAULT_LIST_TEMPLATE;
                if (!string.IsNullOrEmpty((string) ModuleConfiguration.ModuleSettings[Constants.LIST_TEMPLATE]))
                {
                    defaultListTemplate = Convert.ToString(ModuleConfiguration.ModuleSettings[Constants.LIST_TEMPLATE]);

                    if (defaultListTemplate.IndexOf("List_Standard", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        defaultListTemplate = Constants.DEFAULT_LIST_TEMPLATE;
                    }
                }

                var objListTemplate = (PortalModuleBase) LoadControl("Template/" + defaultListTemplate);
                objListTemplate.ModuleConfiguration = ModuleConfiguration;
                phTemplate.Controls.Add(objListTemplate);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}