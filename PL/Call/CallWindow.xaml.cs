using PL.Volunteer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Call
{
    /// <summary>
    /// Interaction logic for CallWindow.xaml
    /// </summary>
    public partial class CallWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public CallWindow(int id = 0)
        {
            ButtonText = id == 0 ? "Add" : "Update";
            InitializeComponent();

            try
            {
                CurrentCall = id != 0
                    ? s_bl.Call.GetCallDetails(id)
                    :
                    new BO.Call() { CallType =BO.CallType.None, OpeningTime = s_bl.Admin.GetClock(),StatusCall=BO.StatusCall.Open};
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                Close();
            }
        }
        public BO.Call? CurrentCall
        {
            get { return (BO.Call?)GetValue(CurrentCallProperty); }
            set { SetValue(CurrentCallProperty, value); }
        }

        public static readonly DependencyProperty CurrentCallProperty =
            DependencyProperty.Register("CurrentCall", typeof(BO.Call), typeof(CallWindow), new PropertyMetadata(null));

        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(CallWindow));

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (CurrentCall == null) return;
                if (ButtonText == "Add")
                {
                    
                    s_bl.Call.AddCall(CurrentCall!);
                    MessageBox.Show("call added successfully.");
                }
                else
                {
                    s_bl.Call.UpdateCallDetails(CurrentCall);
                    MessageBox.Show("call updated successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }
        private void RefreshCall()
        {
            int id = CurrentCall!.Id;
            CurrentCall = null;
            CurrentCall = s_bl.Call.GetCallDetails(id);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (CurrentCall!.Id != 0)
                s_bl.Call.AddObserver(CurrentCall!.Id, RefreshCall);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (CurrentCall != null && CurrentCall.Id != 0)
                s_bl.Call.RemoveObserver(CurrentCall!.Id, RefreshCall);
        }
    }
}
