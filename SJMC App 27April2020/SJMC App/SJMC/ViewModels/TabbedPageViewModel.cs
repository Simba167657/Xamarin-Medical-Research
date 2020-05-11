using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SJMC.Helpers;
using SJMC.ServiceProviders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SJMC.ViewModels
{
    public class TabbedPageViewModel : ViewModelBase
    {

        private bool _labCountVisibility;
        public bool LabCountVisibility
        {
            get { return _labCountVisibility; }
            set
            {
                _labCountVisibility = value;
                RaisePropertyChanged();
            }
        }
        private bool _radioCountVisibility;
        public bool RadioCountVisibility
        {
            get { return _radioCountVisibility; }
            set
            {
                _radioCountVisibility = value;
                RaisePropertyChanged();
            }
        }

        private int _radioCount;
        public int RadioCount
        {
            get { return _radioCount; }
            set
            {
                _radioCount = value;
                RaisePropertyChanged();
            }
        }
        private int _labCount;
        public int LabCount
        {
            get { return _labCount; }
            set
            {
                _labCount = value;
                RaisePropertyChanged();
            }
        }

        private int _currentTabIndex;
        public int CurrentTabIndex
        {
            get => _currentTabIndex;
            set
            {
                _currentTabIndex = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand RadioCommand => new RelayCommand(RadioClick);

        private void RadioClick()
        {
            CurrentTabIndex = 1;
        }

        public RelayCommand LabCommand => new RelayCommand(LabClick);

        private void LabClick()
        {
            CurrentTabIndex = 0;
        }

        internal async void Initilize(int index)
        {
            CurrentTabIndex = index;
            LabCountVisibility = RadioCountVisibility = false;
            var result = await new ReportsService().GetLabReportsCount(Constants.UserProfile.Role, Constants.UserProfile.LabID);
            if (result.Success)
            {
                if (result.labCount > 0)
                {
                    LabCount = result.labCount;
                    LabCountVisibility = true;
                }
                if (result.radioCount > 0)
                {
                    RadioCount = result.radioCount;
                    RadioCountVisibility = true;
                }
            }
        }
    }
}
