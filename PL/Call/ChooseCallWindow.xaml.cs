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
    /// Interaction logic for ChooseCallWindow.xaml
    /// </summary>
    public partial class ChooseCallWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public int currentId { get; set; }
        public BO.CallType CallType { get; set; } = BO.CallType.None;
        public BO.OpenCallInListAttributes Attribute { get; set; } = BO.OpenCallInListAttributes.Id;

        public IEnumerable<BO.OpenCallInList> OpenCallList
        {
            get { return (IEnumerable<BO.OpenCallInList>)GetValue(OpenCallListProperty); }
            set { SetValue(OpenCallListProperty, value); }
        }

        public static readonly DependencyProperty OpenCallListProperty =
            DependencyProperty.Register("OpenCallList", typeof(IEnumerable<BO.OpenCallInList>), typeof(ChooseCallWindow), new PropertyMetadata(null));
        public ChooseCallWindow(int id)
        {
            currentId = id;
            InitializeComponent();
        }

        private void filterBySelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
           => queryOpenCallList();

        private void queryOpenCallList()
         => OpenCallList = (CallType == BO.CallType.None) ?
                s_bl?.Call.OpenCallsListSelectedByVolunteer(currentId, null, Attribute)! : s_bl?.Call.OpenCallsListSelectedByVolunteer(currentId, CallType, Attribute)!;

        private void openCallListObserver()
           => queryOpenCallList();

        private void Window_Loaded(object sender, RoutedEventArgs e)
          => s_bl.Call.AddObserver(openCallListObserver);

        private void Window_Closed(object sender, EventArgs e)
           => s_bl.Call.RemoveObserver(openCallListObserver);

        private void Choose_Button_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageResult = MessageBox.Show("Are you sure you want to choose this call", "its ok?",
          MessageBoxButton.OKCancel,
          MessageBoxImage.Information);
            if (messageResult == MessageBoxResult.OK)
            {
                var button = sender as Button;
                BO.OpenCallInList? call = button?.DataContext as BO.OpenCallInList;
                if (call?.Id != null)
                    try
                    {
                        s_bl.Call.ChooseTreatmentCall(currentId, call.Id);
                    }
                    catch (BO.BlDoesNotExistException ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                    }
            }


        }
    }
}