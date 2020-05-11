using Syncfusion.XForms.TabView;
using System;
using System.Collections.Generic;
using System.Text;

namespace SJMC.CustomControl
{
   public class CustomTabView: Syncfusion.XForms.TabView.SfTabView
    {
        public CustomTabView()
        {
            TabHeight = 60;
        }
    }
    public class CustomTabItem: SfTabItem
    {

    }
}
