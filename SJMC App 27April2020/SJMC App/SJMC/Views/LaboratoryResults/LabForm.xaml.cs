using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace SJMC.Views.LaboratoryResults
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LabForm : ContentPage
    {
        //TODO : To Declare Class Level Variables...
        protected LabFormVM _LabFormVM;

        public LabForm()
        {
            InitializeComponent();

            _LabFormVM = new LabFormVM(this.Navigation);
            this.BindingContext = _LabFormVM;

            // iOS Platform
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

    }
}