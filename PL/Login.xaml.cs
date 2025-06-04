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

namespace PL
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public string UserName { get; set; }
        public string Password { get; set; }
        public static readonly DependencyProperty RoleProperty =
        DependencyProperty.Register("Role", typeof(BO.Role), typeof(Login), new PropertyMetadata(BO.Role.Volunteer));

        public BO.Role Role
        {
            get => (BO.Role)GetValue(RoleProperty);
            set => SetValue(RoleProperty, value);
        }
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Values ​​must be entered");
                return;
            }

            if (!IsManager(UserName, Password))
            {
                new VolunteerWindow().Show();
                Close();
            }
            
        }
        private bool IsManager(string username, string password)
        {
            try
            {
                Role = s_bl.Volunteer.Login(username, password);
                return Role == BO.Role.Manager;
            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }

        private void mainWindow_Button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void volunteerWindow_Button_Click(object sender, RoutedEventArgs e)
        {
            new VolunteerWindow().Show();
            Close();
        }
    }
}
