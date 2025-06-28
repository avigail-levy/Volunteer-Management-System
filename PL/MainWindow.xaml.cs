using PL.Call;
using PL.Volunteer;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public int Id { get; }
        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }
        public static readonly DependencyProperty CurrentTimeProperty =
       DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow));

        public TimeSpan CurrentRiskRange
        {
            get { return (TimeSpan)GetValue(CurrentRiskRangeProperty); }
            set { SetValue(CurrentRiskRangeProperty, value); }
        }
        public static readonly DependencyProperty CurrentRiskRangeProperty =
        DependencyProperty.Register("CurrentRiskRange", typeof(TimeSpan), typeof(MainWindow));

        public int[] CallByStatus
        {
            get { return (int[])GetValue(CallByStatusProperty); }
            set { SetValue(CallByStatusProperty, value); }
        }

        public static readonly DependencyProperty CallByStatusProperty =
            DependencyProperty.Register("CallByStatus", typeof(int[]), typeof(MainWindow));

        public MainWindow(int id)
        {
            CallByStatus = s_bl.Call.GetCallQuantitiesByStatus();
            Id = id;
            InitializeComponent();
        }

        private void clockObserver()
        {
            CurrentTime = s_bl.Admin.GetClock();
        }
        private void configObserver()
        {
            CurrentRiskRange = s_bl.Admin.GetRiskRange();
        }
        //functions to advnce time
        private void btnAddOneMinute_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Minute);
        }
        private void btnAddOneDay_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Day);
        }
        private void btnAddOneHour_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Hour);
        }
        private void btnAddOneMonth_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Month);
        }
        private void btnAddOneYear_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Year);
        }

        private void btnUpdateRiskRange_click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.SetRiskRange(CurrentRiskRange);
        }

        private void callByStatusObserver()
        {
            CallByStatus = s_bl.Call.GetCallQuantitiesByStatus();
        }
        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentTime = s_bl.Admin.GetClock();
            CurrentRiskRange = s_bl.Admin.GetRiskRange();
            s_bl.Admin.AddClockObserver(clockObserver);
            s_bl.Admin.AddConfigObserver(configObserver);
            s_bl.Call.AddObserver(callByStatusObserver);
        }

        private void window_Closed(object sender, EventArgs e)
        {
            s_bl.Admin.RemoveClockObserver(clockObserver);
            s_bl.Admin.RemoveConfigObserver(configObserver);
            s_bl.Call.RemoveObserver(callByStatusObserver);
        }

        private void showCallsByStatus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string roleTag)
            {
                new CallListWindow(Id, (BO.StatusCall)Enum.Parse(typeof(BO.StatusCall), roleTag)).Show();
            }
            else
            {
                new CallListWindow(Id).Show();
            }
        }
        private void btnViewVolunteerList_Click(object sender, RoutedEventArgs e)
        {
            new VolunteerListWindow().Show();
        }
        private void btnViewCallList_Click(object sender, RoutedEventArgs e)
        {
            new CallListWindow(Id).Show();
        }

        private void resetDB_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageResult = MessageBox.Show("Are you sure you want to reset?", "its ok?",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
            if (messageResult == MessageBoxResult.OK)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                s_bl.Admin.ResetDB();
                Mouse.OverrideCursor = null;

            }

        }
        private void initDB_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageResult = MessageBox.Show("Are you sure you want to init?", "its ok?",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
            if (messageResult == MessageBoxResult.Yes)
            {   //Changes the mouse to an hourglass shape.
                Mouse.OverrideCursor = Cursors.Wait;
                s_bl.Admin.InitializeDB();
                Mouse.OverrideCursor = null;
            }

        }
    }
}