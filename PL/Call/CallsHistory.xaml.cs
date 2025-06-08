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
    /// Interaction logic for CallsHistory.xaml
    /// </summary>
    public partial class CallsHistory : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public int CurrentId { get; set; }
        public BO.CallType CallType { get; set; } = BO.CallType.None;
        public IEnumerable<BO.ClosedCallInList> ClosedCallList
        {
            get { return (IEnumerable<BO.ClosedCallInList>)GetValue(ClosedCallListProperty); }
            set { SetValue(ClosedCallListProperty, value); }
        }

        public static readonly DependencyProperty ClosedCallListProperty =
            DependencyProperty.Register("ClosedCallList", typeof(IEnumerable<BO.ClosedCallInList>), typeof(CallsHistory), new PropertyMetadata(null));

        public CallsHistory(int id)
        {
            CurrentId = id;
            InitializeComponent();
        }
        private void filterBySelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
           => queryClosedCallList();

        private void queryClosedCallList()
         => ClosedCallList = (CallType == BO.CallType.None) ?
                s_bl?.Call.ClosedCallsListHandledByVolunteer(CurrentId, null, null)! : s_bl?.Call.ClosedCallsListHandledByVolunteer(CurrentId, CallType, null)!;

        private void ClosedCallListObserver()
           => queryClosedCallList();

        private void Window_Loaded(object sender, RoutedEventArgs e)
          => s_bl.Call.AddObserver(ClosedCallListObserver);

        private void Window_Closed(object sender, EventArgs e)
           => s_bl.Call.RemoveObserver(ClosedCallListObserver);


    }
}
