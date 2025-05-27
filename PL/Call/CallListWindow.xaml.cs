using BO;
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
    /// Interaction logic for CallListWindow.xaml
    /// </summary>
    public partial class CallListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public BO.VolunteerInList? SelectedCall { get; set; }

        public BO.CallType CallType { get; set; } = BO.CallType.None;
        public IEnumerable<BO.CallInList> CallList
        {
            get { return (IEnumerable<BO.CallInList>)GetValue(CallListProperty); }
            set { SetValue(CallListProperty, value); }
        }

        public static readonly DependencyProperty CallListProperty =
            DependencyProperty.Register("CallList", typeof(IEnumerable<BO.CallInList>), typeof(CallListWindow), new PropertyMetadata(null));

        public CallListWindow()
        {
            InitializeComponent();
        }

        private void filterBySelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
           => queryVolunteerList();

        private void queryVolunteerList()
         => CallList = (CallType == BO.CallType.None) ?
                s_bl?.Call.GetCallsList(null, null, null)! : s_bl?.Call.GetCallsList(BO.CallInListAttributes.CallType, CallType, null)!;

        private void callListObserver()
           => queryVolunteerList();

        private void Window_Loaded(object sender, RoutedEventArgs e)
          => s_bl.Volunteer.AddObserver(callListObserver);

        private void Window_Closed(object sender, EventArgs e)
           => s_bl.Volunteer.RemoveObserver(callListObserver);

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new CallWindow().Show();
        }
        private void lsvCallsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedCall != null)
                new CallWindow(SelectedCall.Id).Show();

        }
        private void delete_btnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageResult = MessageBox.Show("Are you sure you want to delete the call", "its ok?",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
            if (messageResult == MessageBoxResult.OK)
            {
                var button = sender as Button;
                BO.CallInList? call = button?.DataContext as BO.CallInList;
                if (call != null)
                    s_bl.Call.DeleteCall(call.Id);
            }
        }
    }
}

