using PL.Call;
using PL.Volunteer;
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
       DependencyProperty.Register("CurrentRiskRange", typeof(TimeSpan), typeof(TimeSpan));

        public MainWindow()
        {
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

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentTime = s_bl.Admin.GetClock();
            CurrentRiskRange = s_bl.Admin.GetRiskRange();
            s_bl.Admin.AddClockObserver(clockObserver);
            s_bl.Admin.AddConfigObserver(configObserver);
        }

        private void window_Closed(object sender, EventArgs e)
        {
            s_bl.Admin.RemoveClockObserver(clockObserver);
            s_bl.Admin.RemoveConfigObserver(configObserver);
        }

        private void btnViewVolunteerList_Click(object sender, RoutedEventArgs e)
        {
            new VolunteerListWindow().Show();
        }
        private void btnViewCallList_Click(object sender, RoutedEventArgs e)
        {
            new CallListWindow().Show();
        }

        private void resetDB_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageResult = MessageBox.Show("Are you sure you want to reset?", "its ok?",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
            if (messageResult == MessageBoxResult.Yes)
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